using System;
using System.Web;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.BinaryDrawingFormat;
using ExcelLibrary.BinaryFileFormat;
using ExcelLibrary.SpreadSheet;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text;



/// <summary>
/// Summary description for ExportToExcel
/// </summary>
public class ExportToExcel : System.Web.UI.Page
{
    public ExportToExcel()
	{
	}
    private void ResponseWritePDF(string realFileName)
    {
        Document doc = new Document();
        string ext = ".pdf";
        string rutaArchivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + realFileName)); 
        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.OpenOrCreate));

        writer.ViewerPreferences = PdfWriter.HideMenubar;
        writer.ViewerPreferences = PdfWriter.HideToolbar;
        writer.ViewerPreferences = PdfWriter.HideWindowUI;

        doc.Open();

        doc.Add(new Paragraph("Fecha: " + DateTime.Now.ToLongDateString()));

        doc.Close();

        //System.Diagnostics.Process.Start("AcroRd32.exe", rutaArchivo);

        FileStream fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
        //FileStream fs = new FileStream(Server.MapPath(rutaArchivo), FileMode.Open, FileAccess.Read);
        Byte[] fileData;
        fileData = new Byte[fs.Length];
        long bytesRead = fs.Read(fileData, 0, (int)(fs.Length));
        fs.Close();

        Response.ClearContent();
        Response.ClearHeaders();

        Response.AddHeader("Content-Disposition", "attachment;filename=pdf" + DateTime.Now.Ticks.ToString() + ext);
        Response.ContentType = "application/pdf";

        Response.AddHeader("Content-length", bytesRead.ToString());

        Response.BinaryWrite(fileData);
    }

    protected void ResponseWriteExcelFile(DataTable dt, string fileSaveName)
    {
        ResponseWriteExcelFile(dt, fileSaveName, null);
    }

    private void hola()
    {
        DataSet ds = new DataSet();

        if (ds.Tables[0].Columns.Contains("columna_con_html"))
        {
            foreach (DataRow my_row in ds.Tables[0].Rows)
            {
                my_row["columna_con_html"] = " '' " + my_row["columna_con_html"];
            }
        }


        ResponseWriteExcelFile(ds.Tables[0], "archivo.xls");
    }

    protected void ResponseWriteExcelFile(DataTable dt, string fileSaveName, string[] columns)
    {
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        if (columns != null)
        {
            DataTable send = dt.DefaultView.ToTable(false, columns);
            getRenderControl(ref send).RenderControl(hw);
        }
        else
        {
            getRenderControl(ref dt).RenderControl(hw);
        }

        //Response.ContentType = application/vnd.ms-excel;
        Response.ContentType = "application/vnd.ms-excel";
        Response.ContentEncoding = System.Text.Encoding.Default;
        Response.AppendHeader("Content-Disposition", "inline; filename=" + fileSaveName + "");
        string s_styleInfo = @"<style> td {mso-number-format:0\.00;}</style>";
        string s_excel = tw.ToString();
        Response.Write(s_styleInfo + s_excel);
        //Response.Write(tw.ToString());
        Response.End();
    }

    private DataGrid getRenderControl(ref DataTable dt)
    {
        DataGrid DataControl = new DataGrid();
        DataControl.DataSource = dt;
        DataControl.DataBind();
        return DataControl;
    }

    protected DataSet GetDatasetFromExcelFile(string excelFileFullPath)
    {
        return GetDatasetFromExcelFile(excelFileFullPath, 0, "DefaultTable");
    }
    protected DataSet GetDatasetFromExcelFile(string excelFileFullPath, int ExcelSheet, string TableName)
    {
        Workbook book = null;
        try
        {
            book = Workbook.Load(excelFileFullPath);
        }
        catch (Exception ex)
        {
            throw new Exception("No se puede cargar el archivo, formato desconocido (excel 2003 / xls)");
        }
        Worksheet sheet = book.Worksheets[ExcelSheet];
        //sheet.Cells[5, 4].Format = new CellFormat(CellFormatType.Text, null);
        DataSet ds = new DataSet();
        ds.Tables.Add(TableName);
        List<string> renglon = null;
        for (int i = 0; i <= sheet.Cells.LastRowIndex; i++)
        {
            renglon = new List<string>();
            for (int j = 0; j <= sheet.Cells.LastColIndex; j++)
            {
                if (i < 1)
                    ds.Tables[0].Columns.Add(sheet.Cells[i, j].StringValue, typeof(System.String));
                else
                    renglon.Add(sheet.Cells[i, j].StringValue);
            }
            if (i > 0)
                if (renglon[0].Length > 0)
                    ds.Tables[0].Rows.Add(renglon.ToArray());
        }
        return ds;
    }


    public struct ReportFileType
    {
        public string Excel
        {
            get { return "Excel"; }
        }
        public string Pdf
        {
            get { return "PDF"; }
        }
    }
}

