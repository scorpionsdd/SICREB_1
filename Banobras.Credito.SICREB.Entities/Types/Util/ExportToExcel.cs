using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace Banobras.Credito.SICREB.Entities.Types.Util
{

    public sealed class ExportToExcel
    {

        public static void AddDataSheetToExcelWorkbook(DataTable dt, string sheetName, ref ExcelLibrary.SpreadSheet.Workbook wb)
        {

            ExcelLibrary.SpreadSheet.Worksheet ws = new ExcelLibrary.SpreadSheet.Worksheet(sheetName);
            
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ws.Cells[0, i].Value = dt.Columns[i].ColumnName;
            }
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ws.Cells[1 + i, j].Value = dt.Rows[1 + i][j].ToString();
                }
            }
            
            wb.Worksheets.Add(ws);
        }

    }

}
