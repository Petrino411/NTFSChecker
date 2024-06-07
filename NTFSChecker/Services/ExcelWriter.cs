using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NTFSChecker.Extentions;
using NTFSChecker.Models;
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
            var directory = Path.GetTempPath();
            var fileName = Path.Combine(directory, $"{Guid.NewGuid()}.xlsx");

            while (File.Exists(fileName))
            {
                fileName = Path.Combine(directory, $"{Guid.NewGuid()}.xlsx");
            }

            _logger.LogInformation($"Получен путь сохранения файла {fileName}");
            return fileName;
        }

        public void CreateLegend()
        {
            var legendStartColumn = _worksheet.Dimension.End.Column + 2;
            var legendStartRow = 1;

            var legends = new List<(string text, Color color)>
            {
                ("Группы пользователей нет у корневого каталога или прав меньше", Color.Red),
                ("Группы пользователей нет у дочернего каталога или прав меньше", Color.Orange),
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

        private async Task WriteCellAsync(int row, int column, List<string> users, List<string> mainDirUsers)
        {
            var mainDirUsersDict = mainDirUsers
                .Select(u => u.Split(new[] { ':' }, 2))
                .ToDictionaryWithSuffix(
                    u => u[0].Trim(),
                    u => u.Length > 1 ? u[1].Trim() : string.Empty
                );

            var usersDict = users
                .Select(u => u.Split(new[] { ':' }, 2))
                .ToDictionaryWithSuffix(
                    u => u[0].Trim(),
                    u => u.Length > 1 ? u[1].Trim() : string.Empty
                );

            var colorDifferences = new List<(string user, string difference, Color color, bool fullColor)>();

            foreach (var mainUser in mainDirUsersDict)
            {
                if (usersDict.TryGetValue(mainUser.Key, out var userRights))
                {
                    // Если права отличаются
                    if (mainUser.Value != userRights)
                    {
                        colorDifferences.Add((
                            mainUser.Key,
                            userRights,
                            Color.Purple,
                            false)); // Название группы черное, различие выделено
                    }
                    else
                    {
                        colorDifferences.Add((
                            mainUser.Key,
                            mainUser.Value,
                            Color.Black,
                            false)); // Все черное
                    }
                }
                else
                {
                    colorDifferences.Add((
                        mainUser.Key,
                        mainUser.Value,
                        Color.Orange,
                        true)); // Весь объект оранжевый
                }
            }

            foreach (var user in usersDict)
            {
                if (!mainDirUsersDict.ContainsKey(user.Key))
                {
                    colorDifferences.Add((
                        user.Key,
                        user.Value,
                        Color.Red,
                        true)); // Весь объект красный
                }
            }

            var cell = _worksheet.Cells[row, column];
            cell.Style.WrapText = true;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

            foreach (var (user, difference, color, fullColor) in colorDifferences)
            {
                if (fullColor)
                {
                    var richTextFull = cell.RichText.Add($"{user}: {difference}\n");
                    richTextFull.Color = color;
                }
                else
                {
                    var richTextUser = cell.RichText.Add($"{user}: ");
                    richTextUser.Color = Color.Black;

                    var richTextDifference = cell.RichText.Add($"{difference}\n");
                    richTextDifference.Color = color;
                }
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
            var mainDirData = data.FirstOrDefault().AccessUsers.OrderBy(x => x).ToList();


            foreach (var item in data)
            {
                await WriteCellAsync(row, 1, item.ServerName);
                await WriteCellAsync(row, 2, item.Ip);
                await WriteCellAsync(row, 3, item.DirName);
                _logger.LogInformation($"Экспортируется {item.DirName}");
                await WriteCellAsync(row, 4, item.Purpose);

                var sortedDescriptionUsers = item.DescriptionUsers.OrderBy(x => x).ToList();
                await WriteCellAsync(row, 5, string.Join("\n", sortedDescriptionUsers));

                var sortedAccessUsers = item.AccessUsers.OrderBy(x => x).ToList();
                await WriteCellAsync(row, 6, sortedAccessUsers, mainDirData);

                row++;
            }
        }

        public void AutoFitColumnsAndRows()
        {
            _worksheet.Cells[_worksheet.Dimension.Address].AutoFitColumns();
            for (int i = 1; i <= _worksheet.Dimension.Columns; i++)
            {
                _worksheet.Column(i).Width = 40;
            }
        }

        public void SaveTempAndShow()
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