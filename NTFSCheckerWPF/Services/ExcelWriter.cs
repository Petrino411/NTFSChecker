using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NTFSChecker.DTO;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace NTFSChecker.Services
{
    public class ExcelWriter
    {
        private ExcelPackage _excelPackage;
        private ExcelWorksheet _worksheet;
        private string _filePath;
        private ILogger<ExcelWriter> _logger;


        public ExcelWriter(ILogger<ExcelWriter> logger)
        {
            _logger = logger;
            _excelPackage = new ExcelPackage();
            _worksheet = _excelPackage.Workbook.Worksheets.Add("Sheet1");
            _filePath = GetUniqueFilePath();
        }

        public void CreateNewFile()
        {
            _excelPackage?.Dispose();
            _excelPackage = new ExcelPackage();
            _worksheet = _excelPackage.Workbook.Worksheets.Add("Sheet1");
            _filePath = GetUniqueFilePath();
            _logger.LogInformation($"Создан новый файл: {_filePath}");
        }


        private string GetUniqueFilePath()
        {
            var workDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Reports";
            if (!Directory.Exists(workDirectory))
            {
                Directory.CreateDirectory(workDirectory);
            }

            var fileName = Path.Combine(workDirectory, $"{Guid.NewGuid()}.xlsx");

            while (File.Exists(fileName))
            {
                fileName = Path.Combine(workDirectory, $"{Guid.NewGuid()}.xlsx");
            }

            _logger.LogInformation($"Получен путь сохранения файла {fileName}");
            return fileName;
        }

        public async Task CreateLegendAsync()
        {
            var legendStartColumn = _worksheet.Dimension.End.Column + 2;
            var legendStartRow = 1;

            var legends = new List<(string text, Color color)>
            {
                ("Группы пользователей нет у корневого каталога", Color.Red),
                ("Группы пользователей нет у дочернего каталога", Color.Orange),
                ("Отличия в правах", Color.Purple),
                ("Без изменений", Color.Black)
            };

            foreach (var (text, color) in legends)
            {
                var colorCell = _worksheet.Cells[legendStartRow, legendStartColumn];
                colorCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                colorCell.Style.Fill.BackgroundColor.SetColor(color);

                var textCell = _worksheet.Cells[legendStartRow + 1, legendStartColumn];
                textCell.Value = text;
                textCell.Style.WrapText = true;

                legendStartColumn++;
            }
        }

        private async Task WriteCellAsync(int row, int column, string value, Color color = default)
        {
            var cell = _worksheet.Cells[row, column];
            cell.Value = value;
            cell.Style.WrapText = true;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            if (color != default)
            {
                cell.Style.Font.Color.SetColor(color);
            }
        }


        public async Task SetTableHeadAsync(List<string> headers)
        {
            for (var i = 0; i < headers.Count; i++)
            {
                await WriteCellAsync(1, i + 1, headers[i]);
                _worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            }
        }

        public async Task WriteDataAsync(List<ExcelDataModel> data)
        {
            var row = 2;
            var mainDirAccessUsers = data.FirstOrDefault()!.AccessUsers;
            var prevRow = 0;

            foreach (var item in data)
            {
                _logger.LogInformation($"Экспортируется {item.DirName}");
                
                await WriteCellAsync(row, 1, item.ServerName);
                await WriteCellAsync(row, 2, item.Ip);
                await WriteCellAsync(row, 3, item.DirName);
                await WriteCellAsync(row, 4, item.Purpose);
                
                prevRow =  row;
                
                row = await WriteAccessUsersAsync(row, 5, item.AccessUsers, mainDirAccessUsers, row == 2);
                
                _worksheet.Cells[prevRow, 1, row - 1, 1].Merge = true;
                _worksheet.Cells[prevRow, 2, row - 1, 2].Merge = true;
                _worksheet.Cells[prevRow, 3, row - 1, 3].Merge = true;
                _worksheet.Cells[prevRow, 4, row - 1, 4].Merge = true;
                
            }
        }

        private async Task<int> WriteAccessUsersAsync(int startRow, int startColumn, List<List<string>> accessUsers,
            List<List<string>> mainDirAccessUsers, bool isRootDirectory)
        {
            int[] EqualsIndices = { 1, 2, 3 };
            var count = 0;

            foreach (var accessUser in accessUsers)
            {
                if (isRootDirectory)
                {
                    // корневой каталог
                    for (int i = 0; i < accessUser.Count; i++)
                    {
                        var cell = _worksheet.Cells[startRow, startColumn + i];
                        cell.Value = accessUser[i];
                        cell.Style.WrapText = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Font.Color.SetColor(Color.Black); // Без изменений
                    }
                }
                else
                {
                    bool isGroupInMainDir = mainDirAccessUsers.Any(mu => mu[1] == accessUser[1]);

                    for (int i = 0; i < accessUser.Count; i++)
                    {
                        var cell = _worksheet.Cells[startRow, startColumn + i];
                        cell.Value = accessUser[i];
                        cell.Style.WrapText = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        if (isGroupInMainDir)
                        {
                            if (mainDirAccessUsers.Any(mainDirAccessUser =>
                                    EqualsIndices.All(index =>
                                        accessUser[index] == mainDirAccessUser[index]))) // Полное равенство
                            {
                                cell.Style.Font.Color.SetColor(Color.Black); // Без изменений
                            }
                            else if (mainDirAccessUsers.Any(mu => mu[3] == accessUser[3]))
                            {
                                if (i == 2)
                                {
                                    cell.Style.Font.Color.SetColor(Color.Purple); // Отличие в наборе прав
                                }
                            }
                            else
                            {
                                if (i == 3)
                                {
                                    cell.Style.Font.Color.SetColor(Color.Purple); // Отличие в типе прав
                                }
                            }
                        }
                        else
                        {
                            cell.Style.Font.Color.SetColor(Color.Red); // Нет в корневом
                        }
                    }
                }


                startRow++;
            }
            foreach (var mainDirAccessUser in mainDirAccessUsers)
            {
                if (!accessUsers.Any(x => x.SequenceEqual(mainDirAccessUser)))
                {
                    for (int i = 0; i < mainDirAccessUser.Count; i++)
                    {
                        var cell = _worksheet.Cells[startRow, startColumn + i];
                        cell.Value = mainDirAccessUser[i];
                        cell.Style.WrapText = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Font.Color.SetColor(Color.Orange); // Нет в дочернем
                    }
                    startRow++;
                }
                
            }
            return startRow;
        }


        public async Task AutoFitColumnsAndRowsAsync()
        {
            _worksheet.Cells[_worksheet.Dimension.Address].AutoFitColumns();
            for (int i = 1; i <= _worksheet.Dimension.Columns; i++)
            {
                _worksheet.Column(i).Width = 40;
            }
        }

        public async Task SaveTempAndShowAsync()
        {
            try
            {
                _excelPackage.SaveAs(new FileInfo(_filePath));

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = _filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при сохранении файла {_filePath}: {e}");
                throw;
            }
        }
    }
}