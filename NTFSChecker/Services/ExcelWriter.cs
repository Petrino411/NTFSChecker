using System;
using System.Collections.Generic;
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
        private readonly ExcelPackage _excelPackage;
        private readonly ExcelWorksheet _worksheet;
        private readonly string _filePath;
        private readonly ILogger<ExcelWriter> _logger;

        public ExcelWriter(ILogger<ExcelWriter> logger)
        {
            _logger = logger;
            _excelPackage = new ExcelPackage();
            _worksheet = _excelPackage.Workbook.Worksheets.Add("Sheet1");
            _filePath = GetUniqueFilePath();
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

        private async Task WriteCellAsync(int row, int column, string value)
        {
            var cell = _worksheet.Cells[row, column];
            cell.Value = value;
            cell.Style.WrapText = true;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
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
            foreach (var item in data)
            {
                await WriteCellAsync(row, 1, item.ServerName);
                await WriteCellAsync(row, 2, item.Ip);
                await WriteCellAsync(row, 3, item.DirName);
                _logger.LogInformation($"Экспортируется {item.DirName}");
                await WriteCellAsync(row, 4, item.Purpose);
                await WriteCellAsync(row, 5, string.Join("\n", item.DescriptionUsers));
                await WriteCellAsync(row, 6, string.Join("\n", item.AccessUsers));
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