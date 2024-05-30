using System;
using System.Collections.Generic;
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
    
    public void WriteCell(int row, int column, string value)
    {
        _Worksheet.Cells[row, column] = value;
    }

    public void SetTableHead(List<string> headers)
    {
        for (var i = 0; i < headers.Count; i++)
        {
            WriteCell( 1, i + 1, headers[i]);
        }
    }

    public void WriteData(List<ExcelDataModel> data)
    {
        int row = 2; 
        foreach (var item in data)
        {
           
            WriteCell(row, 1, item.ServerName);
            WriteCell(row, 2, item.Ip);
            WriteCell(row, 3, item.DirName);
            WriteCell(row, 4, item.Purpose);
            WriteCell(row, 5, item.AccessUsers.Item1);
            WriteCell(row, 6, item.AccessUsers.Item2); //TODO 
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