using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NTFSChecker.Models;
// using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace NTFSChecker.Services;

public class ExcelWriter
{
    public Excel.Application _excelApp { get; }
    public Excel.Workbook _workbook { get; }
    public Excel.Worksheet _Worksheet { get; set; }



    public ExcelWriter()
    {
        _excelApp = new Excel.Application();
        _workbook = _excelApp.Workbooks.Add();
        _Worksheet = (Excel.Worksheet)_workbook.ActiveSheet;
    }

    public void Show()
    {
        _excelApp.Visible = true;
    }
    
    public async Task WriteCellAsync(int row, int column, string value)
    {
        _Worksheet.Cells[row, column] = value;
    }

    public async Task SetTableHeadAsync(List<string> headers)
    {
        for (var i = 0; i < headers.Count; i++)
        {
            await WriteCellAsync( 1, i + 1, headers[i]);
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
            Console.WriteLine($"Exporting {item.DirName} to excel");
            await WriteCellAsync(row, 4, item.Purpose);
            await WriteCellAsync(row, 5, string.Join("\n", item.DescriptionUsers));
            await WriteCellAsync(row, 6, string.Join("\n", item.AccessUsers));
            row++;
        }
        
        AutoFitColumnsAndRows();
    }

    private void AutoFitColumnsAndRows()
    {
        _Worksheet.Columns.AutoFit();
        _Worksheet.Rows.AutoFit();
    }



}