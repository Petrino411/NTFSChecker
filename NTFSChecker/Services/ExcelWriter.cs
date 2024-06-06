using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
            try
            {
                Dictionary<string, string> mainDirUsersDict = new();
                Dictionary<string, string> usersDict = new();
                try
                {
                    mainDirUsersDict = mainDirUsers
                        .Select(u => u.Split(new[] { ':' }, 2))
                        .ToDictionary(
                            u => u[0].Trim(),
                            u => u.Length > 1 ? u[1].Trim() : string.Empty
                        );

                    usersDict = users
                        .Select(u => u.Split(new[] { ':' }, 2))
                        .ToDictionary(
                            u => u[0].Trim(),
                            u => u.Length > 1 ? u[1].Trim() : string.Empty
                        );
                }
                catch (ArgumentException e)
                {
                    _logger.LogError($"Словарь - пиво {e}");
                }

                var colorDifferences = new List<(string user, Color color)>();

                foreach (var mainUser in mainDirUsersDict)
                {
                    if (usersDict.TryGetValue(mainUser.Key, out var userRights))
                    {
                        // Если права отличаются
                        if (mainUser.Value != userRights)
                        {
                            colorDifferences.Add((
                                $"{mainUser.Key}: {mainUser.Value} (корневой) -> {userRights} (дочерний)",
                                Color.Purple));
                        }
                        else
                        {
                            colorDifferences.Add(($"{mainUser.Key}: {mainUser.Value}", Color.Black));
                        }
                    }
                    else
                    {
                        colorDifferences.Add(($"{mainUser.Key}: {mainUser.Value}", Color.Orange));
                    }
                }


                foreach (var user in usersDict)
                {
                    if (!mainDirUsersDict.ContainsKey(user.Key))
                    {
                        // Пользователь отсутствует в корневом каталоге
                        colorDifferences.Add(($"{user.Key}: {user.Value}", Color.Red));
                    }
                }

                colorDifferences.Sort((x, y) => string.Compare(x.user, y.user, StringComparison.Ordinal));

                foreach (var (user, color) in colorDifferences)
                {
                    var cell = _worksheet.Cells[row, column];
                    cell.Style.WrapText = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    var richText = cell.RichText.Add(user + "\n");
                    richText.Color = color;
                }
            }
            
            catch (Exception e)
            {
                _logger.LogError($"Ошибка  {e}");
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
            var mainDirData = data[0].AccessUsers.OrderBy(x => x).ToList();


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