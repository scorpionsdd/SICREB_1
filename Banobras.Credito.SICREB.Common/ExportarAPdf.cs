using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Banobras.Credito.SICREB.Common
{
    public class ExportarAPdf
    {
        // Conversion a PDF
        private static PdfPTable AgregaTituloGrid(string Titulo, int NumCols, Font fuente_bold, float[] AnchoCols)
        {
            PdfPCell cell;
            int i;
            PdfPTable tabla = new PdfPTable(NumCols);
            try
            {
                tabla.DefaultCell.Padding = 1;
                tabla.SetWidths(AnchoCols);
                tabla.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase(Titulo, fuente_bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.SetLeading(1, 1);
                cell.Colspan = NumCols;
                cell.Border = Rectangle.NO_BORDER;
                cell.GrayFill = 0.9f;
                tabla.AddCell(cell);
                tabla.DefaultCell.Border = Rectangle.NO_BORDER;
                for (i = 0; i < NumCols; i++)
                    tabla.AddCell(new Phrase(" ", fuente_bold));
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tabla = null;
            }
            return tabla;
        }

        private static PdfPTable AgregaHeadersGrid(ArrayList headers, Font fuente_bold, PdfPTable tabla)
        {
            IEnumerator ie;
            int i;
            try
            {
                ie = headers.GetEnumerator();
                tabla.DefaultCell.Border = Rectangle.BOTTOM_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER;
                tabla.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla.DefaultCell.PaddingBottom = 3;
                tabla.DefaultCell.PaddingTop = 1;
                for (i = 0; i < headers.Count && ie.MoveNext(); i++)
                {                    
                    if (i > 0 && i < headers.Count - 1)
                        tabla.DefaultCell.Border = Rectangle.BOTTOM_BORDER + Rectangle.TOP_BORDER;
                    else if (i == headers.Count - 1)
                        tabla.DefaultCell.Border = Rectangle.BOTTOM_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tabla.AddCell(new Phrase((string)ie.Current, fuente_bold));
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tabla = null;
            }
            return tabla;
        }

        public static bool AgregaLogo(Document document, string filename, float x, float y)
        {
            bool exito = false;
            try
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(filename);
                logo.ScalePercent(70);
                logo.SetAbsolutePosition(x, y);
                document.Add(logo);
                exito = true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                exito = false;
            }
            return exito;
        }

        /// <summary>
        /// Metodo para imprimir en PDF
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="Titulo"></param>
        /// <param name="headers"></param>
        /// <param name="AlignCols"></param>
        /// <param name="AnchoCols"></param>
        /// <param name="NumCols"></param>
        /// <returns></returns>
        public static PdfPTable AgregaGrid_PDF(RadGrid dg, string Titulo, ArrayList headers, int[] AlignCols, float[] AnchoCols, int NumCols)
        {

            string temp;
            float fill = 0.0f;
            int alignNum = 0;
            int repeats = 2;
            PdfPCell cell;
            Font fuente = FontFactory.GetFont(FontFactory.TIMES, 8.5f);
            Font fuente_bold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 8.5f);
            PdfPTable tabla;
            try
            {
                tabla = AgregaTituloGrid(Titulo, NumCols, fuente_bold, AnchoCols);
                tabla = AgregaHeadersGrid(headers, AlignCols, fuente_bold, tabla);

                foreach (GridDataItem dgi in dg.Items)
                {
                    alignNum = 0;
                    repeats = 0;

                    fill = (fill == 0.0f) ? 0.9f : 0.0f;

                    foreach (TableCell celda in dgi.Cells)
                    {
                        if (repeats >= 2)
                        {
                            if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                            {
                                temp = GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda);
                                temp = temp.Trim();
                                temp.ToUpper();
                                temp = temp.Replace("&nbsp;", "");
                                cell = new PdfPCell(new Phrase(((temp == string.Empty || temp == "&nbsp;") ? " " : temp), fuente));
                                cell.HorizontalAlignment = GetHorizontalAlignCelda(dg.MasterTableView.Columns[repeats - 2].ItemStyle.HorizontalAlign.ToString());
                                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cell.Padding = 1;
                                cell.Colspan = 1;
                                cell.GrayFill = fill;
                                cell.Border = Rectangle.NO_BORDER;
                                tabla.AddCell(cell);

                                alignNum++;
                            }
                        }

                        repeats++;
                    }
                }
            }
            catch (Exception exc)
            {
                temp = exc.Message;
                return null;
            }

            return tabla;
        }

        /// <summary>
        /// Metodo para mandar a imprimir un RadGrid en Pdf
        /// </summary>
        /// <param name="radGrid"></param>
        /// <param name="tituloDoc"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string ImprimirPdf(List<RadGrid> radGridList, string[] radGridNames, string tituloDoc, string wcSite)
        {

            string path = string.Empty;
            string strScript = string.Empty;
            iTextSharp.text.Font fuente = FontFactory.GetFont(FontFactory.TIMES, 8.5f);
            iTextSharp.text.Font fuente_bold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 8.5f);
            Document document = new Document();
            ArrayList headers = null;
            PdfPTable tabla = null;
            PdfWriter writer = null;
            HeaderFooter header = null;
            float[] AnchoCols;
            string[] hdr;
            char[] splitter = { ';' };
            string hdrName = string.Empty;
            int[] AlignCols;
            int i = 0;
            int gridCount = 0;
            string fileName = string.Empty;
            string fileUrl = string.Empty;
            string imageName = string.Empty;

            try
            {

                fileName = string.Format("{0}{1}.pdf", FirstLetterToCapital(tituloDoc), DateTime.Today.ToString("ddMMMyy"));
                fileUrl = string.Format("{0}Resources\\PDF\\{1}", System.Web.HttpContext.Current.Request.PhysicalApplicationPath, fileName);
                writer = PdfWriter.GetInstance(document, new FileStream(fileUrl, FileMode.OpenOrCreate));
                header = new HeaderFooter(new Phrase(tituloDoc, fuente_bold), false);
                header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                header.Alignment = Element.ALIGN_RIGHT;
                document.Header = header;
                document.AddAuthor("Banobras.");
                document.AddSubject("Reporte.");
                document.Open();

                // Logo de banobras
                imageName = string.Format("{0}HeadersFooters/BNB-SIC-Header_LogoBanobras.png", WebConfig.UrlImages);
                ExportarAPdf.AgregaLogo(document, imageName, 25, document.PageSize.Height - 50);
                document.Add(new Phrase("\r\n\r\n", fuente));


                foreach (RadGrid radGrid in radGridList)
                {

                    headers = new ArrayList();
                    i = 0;
                    hdrName = string.Empty;

                    foreach (GridColumn gdColumn in radGrid.Columns)
                    {
                        if (gdColumn.Display && (gdColumn.ColumnType != "GridButtonColumn" && gdColumn.ColumnType != "GridTemplateColumn" && gdColumn.ColumnType != "GridHyperLinkColumn" && gdColumn.ColumnType != "GridEditCommandColumn" && gdColumn.ColumnType != "GridClientSelectColumn"))
                        {

                            if (hdrName == string.Empty)
                            {
                                hdrName = gdColumn.HeaderText;
                            }
                            else
                            {
                                hdrName = string.Format("{0};{1}", hdrName, gdColumn.HeaderText);
                            }

                            i++;
                        }
                    }

                    hdr = hdrName.Split(splitter);

                    AlignCols = new int[i];
                    AnchoCols = new float[i];

                    i = 0;

                    foreach (GridColumn gdColumn in radGrid.Columns)
                    {

                        if (gdColumn.Display && (gdColumn.ColumnType != "GridButtonColumn" && gdColumn.ColumnType != "GridTemplateColumn" && gdColumn.ColumnType != "GridHyperLinkColumn" && gdColumn.ColumnType != "GridEditCommandColumn" && gdColumn.ColumnType != "GridClientSelectColumn"))
                        {

                            switch (gdColumn.HeaderStyle.HorizontalAlign.ToString())
                            {
                                case "Right":
                                    AlignCols[i] = Element.ALIGN_RIGHT;
                                    break;
                                case "Center":
                                    AlignCols[i] = Element.ALIGN_CENTER;
                                    break;
                                default:
                                    AlignCols[i] = Element.ALIGN_LEFT;
                                    break;
                            }

                            AnchoCols[i] = 15;
                            i++;
                        }

                    }

                    for (i = 0; i < hdr.Length; i++)
                        headers.Add(hdr[i]);

                    if (radGrid.Items.Count > 0)
                    {
                        tabla = ExportarAPdf.AgregaGrid_PDF(radGrid, radGridNames[gridCount], headers, AlignCols, AnchoCols, headers.Count);
                        document.Add(tabla);
                        document.Add(new Phrase("\r\n", fuente));
                    }
                    else
                    {
                        document.Add(new Phrase(string.Format("\r\n No hay ninguna {0}", radGridNames[gridCount]), fuente));
                    }

                    gridCount++;
                }

                document.Close();

                //strScript = string.Format("javascript:window.open('{0}/PDF/{1}', '_blank', 'toolbar=0, directories=0, height= 500, width= 500, top= 30, left= 10, resizable=yes');", wcSite, fileName);
                strScript = string.Format("javascript:window.open('{0}PDF/{1}', '_blank', 'toolbar=0, directories=0, height= 500, width= 500, top= 30, left= 10, resizable=yes');", wcSite, fileName);

            }
            catch (Exception exc)
            {

                if (document.IsOpen())
                    document.Close();
                string temp = exc.Message;

            }

            return strScript;

        }

        public static string FirstLetterToCapital(string titulo)
        {

            string newString = string.Empty;
            string[] arrayString = null;
            char[] splitter = { ' ' };

            titulo = titulo.Replace("/", " ");

            arrayString = titulo.Split(splitter);

            for (int x = 0; x < arrayString.Count(); x++)
            {

                newString = string.Format("{0}{1}", newString, char.ToUpper(arrayString[x][0]) + arrayString[x].Substring(1).ToLower());

            }

            return newString;

        }

        private static PdfPTable AgregaHeadersGrid(ArrayList headers, int[] AlignCols, Font fuente_bold, PdfPTable tabla)
        {

            IEnumerator ie;
            int i;
            try
            {
                ie = headers.GetEnumerator();
                tabla.DefaultCell.Border = Rectangle.BOTTOM_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER;
                tabla.DefaultCell.PaddingBottom = 3;
                tabla.DefaultCell.PaddingTop = 1;
                for (i = 0; i < headers.Count && ie.MoveNext(); i++)
                {
                    tabla.DefaultCell.HorizontalAlignment = AlignCols[i];
                    if (i > 0 && i < headers.Count - 1)
                        tabla.DefaultCell.Border = Rectangle.BOTTOM_BORDER + Rectangle.TOP_BORDER;
                    else if (i == headers.Count - 1)
                        tabla.DefaultCell.Border = Rectangle.BOTTOM_BORDER + Rectangle.TOP_BORDER + Rectangle.RIGHT_BORDER;
                    tabla.AddCell(new Phrase((string)ie.Current, fuente_bold));
                }
            }
            catch (Exception exc)
            {
                string temp = exc.Message;
                tabla = null;
            }
            return tabla;

        }

        private static int GetHorizontalAlignCelda(string horizontal)
        {

            int alineacion = 0;

            switch (horizontal)
            {

                case "Right":
                    alineacion = Element.ALIGN_RIGHT;
                    break;
                case "Center":
                    alineacion = Element.ALIGN_CENTER;
                    break;
                default:
                    alineacion = Element.ALIGN_LEFT;
                    break;

            }

            return alineacion;

        }

        public static string GetTextFromCell(string tipoColumna, TableCell celda)
        {

            string texto = string.Empty;

            switch (tipoColumna)
            {

                case "GridCheckBoxColumn":
                    if (((CheckBox)celda.Controls[0]).Checked)
                        texto = "Si";
                    else
                        texto = "No";
                    break;
                case "GridDropDownColumn":
                    texto = ((Literal)celda.Controls[0]).Text;
                    break;
                default:
                    texto = celda.Text;
                    break;

            }

            return texto;

        }

        public static PdfPTable AgregaGrid_PDF(DataGrid dg, string Titulo, ArrayList headers, int[] AlignCols, float[] AnchoCols, int NumCols)
        {
            string temp;
            float fill = 0.0f;
            int i;
            PdfPCell cell;
            Font fuente = FontFactory.GetFont(FontFactory.TIMES, 8.5f);
            Font fuente_bold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 8.5f);
            PdfPTable tabla;
            try
            {
                tabla = AgregaTituloGrid(Titulo, NumCols, fuente_bold, AnchoCols);
                tabla = AgregaHeadersGrid(headers, fuente_bold, tabla);

                foreach (DataGridItem dgi in dg.Items)
                {
                    fill = (fill == 0.0f) ? 0.9f : 0.0f;
                    for (i = 0; i < NumCols; i++)
                    {
                        temp = dgi.Cells[i].Text;
                        temp = temp.Trim();
                        temp.ToUpper();
                        temp = temp.Replace("&nbsp;", "");
                        cell = new PdfPCell(new Phrase(((temp == string.Empty || temp == "&nbsp;" || dgi.Cells[i].Visible == false) ? " " : temp), fuente));
                        cell.HorizontalAlignment = AlignCols[i];
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 1;
                        cell.Colspan = 1;
                        cell.GrayFill = fill;
                        cell.Border = Rectangle.NO_BORDER;
                        tabla.AddCell(cell);
                    }
                }
            }
            catch (Exception exc)
            {
                temp = exc.Message;
                return null;
            }
            return tabla;
        }


        /// <summary>
        /// Método ocupado por páginas que contienen grids con linkbutton y checkbox
        /// </summary>
        /// <returns>Regresa true si se puede realizar la exportación a PFD, en caso contrario regresa false</returns>
        public static bool AgregaGrid_PDF(DataGrid dg, Document document, PdfWriter writer, string Titulo, ArrayList headers, int[] AlignCols, float[] AnchoCols, string fileLogo, int NumCols)
        {
            bool exito;
            string temp;
            float fill = 0.0f;
            int i;
            PdfPCell cell;
            Font fuente = FontFactory.GetFont(FontFactory.TIMES, 8.5f);
            Font fuente_bold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 8.5f);
            PdfPTable tabla;
            try
            {
                tabla = AgregaTituloGrid(Titulo, NumCols - 1, fuente_bold, AnchoCols);
                tabla = AgregaHeadersGrid(headers, fuente_bold, tabla);

                foreach (DataGridItem dgi in dg.Items)
                {
                    fill = (fill == 0.0f) ? 0.9f : 0.0f;
                Retry:
                    for (i = 1; i < NumCols; i++)
                    {
                        temp = dgi.Cells[i].Text;
                        temp = temp.Trim();
                        cell = new PdfPCell(new Phrase(((temp == string.Empty || temp == "&nbsp;" || dgi.Cells[i].Visible == false) ? "    " : temp), fuente));
                        cell.HorizontalAlignment = AlignCols[i - 1];
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.SetLeading(1, 1);
                        cell.Colspan = 1;
                        cell.GrayFill = fill;
                        cell.Border = Rectangle.NO_BORDER;
                        tabla.AddCell(cell);
                    }
                    if (!writer.FitsPage(tabla))
                    {
                        tabla.DeleteLastRow();
                        document.Add(tabla);
                        document.NewPage();
                        // Logo de banobras
                        AgregaLogo(document, fileLogo, 25, document.PageSize.Height - 50);
                        document.Add(new Phrase("\r\n\r\n", fuente));
                        tabla = AgregaTituloGrid(Titulo, NumCols - 1, fuente_bold, AnchoCols);
                        tabla = AgregaHeadersGrid(headers, fuente_bold, tabla);
                        goto Retry;
                    }
                }
                document.Add(tabla);
                exito = true;
            }
            catch (Exception exc)
            {
                temp = exc.Message;
                exito = false;
            }
            return exito;
        }

    }
}
