using System;
using System.Data;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
namespace Banobras.Credito.SICREB.Common
{
    public class ExportarExcel_GL
    {

        public static string GenerarExcelDetalles(List<RadGrid> listaGrids, string[] nombresGrids, string tituloReporte, string nombreUsuario)
        {
            string table = "<table>{0}</table>";
            string tempTable = string.Empty;
            string row = "<tr>{0}</tr>";
            string blankRow = "<tr></tr>";
            string tempRow = string.Empty;
            string gridTitle = "<td style=\"text-align:center;\" colspan=\"{0}\">{1}</td>";
            string itemBold = "<td style=\"border: 1 solid;\"><b>{0}</b></td>";
            string item = "<td>{0}</td>";
            string headers = string.Empty;
            string[] headersArray = null;
            char[] splitter = new char[] { ';' };
            int columnsCount = 0;
            int gridsCount = 0;
            List<int> columnasHijas;
            foreach (RadGrid dg in listaGrids)
            {

                gridTitle = "<td style=\"text-align:center;\" colspan=\"{0}\">{1}</td>";
                tempRow = string.Empty;
                headers = string.Empty;
                columnsCount = 0;
                columnasHijas = new List<int>();
                foreach (GridColumn gdColumn in dg.Columns)
                {

                    if (gdColumn.Display && (gdColumn.ColumnType != "GridButtonColumn" && gdColumn.ColumnType != "GridTemplateColumn" && gdColumn.ColumnType != "GridHyperLinkColumn" && gdColumn.ColumnType != "GridEditCommandColumn" && gdColumn.ColumnType != "GridClientSelectColumn"))
                    {

                        if (headers == string.Empty)
                        {
                            headers = gdColumn.HeaderText;
                        }
                        else
                        {
                            headers = string.Format("{0};{1}", headers, gdColumn.HeaderText);
                        }
                        columnsCount++;
                    }
                    
                }
                columnasHijas.Add(columnsCount);
                foreach (GridTableView gtv in dg.MasterTableView.DetailTables)
                {
                    int columnashijas=0;
                    foreach (GridColumn gdColumn in gtv.Columns)
                    {
                        if (gdColumn.Display && (gdColumn.ColumnType != "GridButtonColumn" && gdColumn.ColumnType != "GridTemplateColumn" && gdColumn.ColumnType != "GridHyperLinkColumn" && gdColumn.ColumnType != "GridEditCommandColumn" && gdColumn.ColumnType != "GridClientSelectColumn"))
                        {
                            
                            if (headers == string.Empty)
                            {
                                headers = gdColumn.HeaderText;
                            }
                            else
                            {
                                headers = string.Format("{0};{1}", headers, gdColumn.HeaderText);
                            }
                            columnashijas++;
                            columnsCount++;
                        }
                    }
                    columnasHijas.Add(columnashijas);
                }

                gridTitle = string.Format(gridTitle, columnsCount.ToString(), "Detalle Personas Morales");
                tempRow = string.Format(row, Add(tempRow, gridTitle));

                //Agrega Titulo del Grid
                tempTable = Add(tempTable, tempRow);

                headersArray = headers.Split(splitter);
                tempRow = string.Empty;

                for (int i = 0; i < columnsCount; i++)
                {
                    tempRow = Add(tempRow, string.Format(itemBold, headersArray[i]));
                }

                //Agrega los titulos de las columnas del grid
                tempTable = Add(tempTable, string.Format(row, tempRow));

                foreach (GridDataItem dgi in dg.Items)
                {
                    
                    bool registroprimerpersona=false;
                    tempRow = string.Empty;
                    int repeats = 0;
                    foreach (TableCell celda in dgi.Cells)
                    {
                        if (repeats >= 2 && (repeats-2) < dg.MasterTableView.Columns.Count )
                        {
                            if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                            {
                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda)));
                            }
                        }
                        registroprimerpersona = true;
                        repeats++;
                    }
                    int hijos=0;
                    if (dgi.ChildItem != null)
                    {
                        if (dgi.ChildItem.NestedTableViews.Length == 1)
                        {
                            if (dgi.ChildItem.NestedTableViews[0].Name == "Credito")
                            {
                                foreach (GridDataItem dgv in dgi.ChildItem.NestedTableViews[0].Items)
                                {
                                    if (!registroprimerpersona)
                                    {
                                        repeats = 0;
                                        foreach (TableCell celda in dgi.Cells)
                                        {
                                            if (repeats >= 2 && (repeats - 2) < dg.MasterTableView.Columns.Count)
                                            {
                                                if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                                                {
                                                    tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda)));
                                                }
                                            }
                                            registroprimerpersona = true;
                                            repeats++;
                                        }
                                    }
                                    registroprimerpersona = false;
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2)
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                    tempTable = Add(tempTable, string.Format(row, tempRow));
                                    tempRow = string.Empty;
                                }
                            }
                            else
                            {
                                foreach (GridDataItem dgv in dgi.ChildItem.NestedTableViews[0].Items)
                                {
                                    if (!registroprimerpersona)
                                    {
                                        repeats = 0;
                                        foreach (TableCell celda in dgi.Cells)
                                        {
                                            if (repeats >= 2 && (repeats - 2) < dg.MasterTableView.Columns.Count)
                                            {
                                                if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                                                {
                                                    tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda)));
                                                }
                                            }
                                            registroprimerpersona = true;
                                            repeats++;
                                        }
                                    }
                                    registroprimerpersona = false;
                                    for (int i = 0; i <= columnasHijas[1]; i++)
                                    {
                                        tempRow = Add(tempRow, string.Format(item, ""));
                                    }
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2)
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                    tempTable = Add(tempTable, string.Format(row, tempRow));
                                    tempRow = string.Empty;
                                }
                            }
                        }
                        else
                        {
                            while (hijos < dgi.ChildItem.NestedTableViews[0].Items.Count || hijos < dgi.ChildItem.NestedTableViews[1].Items.Count || hijos < dgi.ChildItem.NestedTableViews[2].Items.Count)
                            {
                                if (!registroprimerpersona)
                                {
                                    repeats = 0;
                                    foreach (TableCell celda in dgi.Cells)
                                    {
                                        if (repeats >= 2 && (repeats - 2) < dg.MasterTableView.Columns.Count)
                                        {
                                            if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda)));
                                            }
                                        }
                                        registroprimerpersona = true;
                                        repeats++;
                                    }
                                }
                                registroprimerpersona = false;

                                // Primer Nivel de Detalle 
                                if (hijos < dgi.ChildItem.NestedTableViews[0].Items.Count)
                                {
                                    GridDataItem dgv = dgi.ChildItem.NestedTableViews[0].Items[hijos];
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2 )
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i <= columnasHijas[1]-1; i++)
                                    {
                                        tempRow = Add(tempRow, string.Format(item, ""));
                                    }
                                }

                                // Segundo Nivel de Detalle
                                if (hijos < dgi.ChildItem.NestedTableViews[1].Items.Count)
                                {
                                    GridDataItem dgv = dgi.ChildItem.NestedTableViews[1].Items[hijos];
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2 )
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i <= columnasHijas[2] - 1; i++)
                                    {
                                        tempRow = Add(tempRow, string.Format(item, ""));
                                    }
                                }


                                // Tercer Nivel de Detalle
                                if (hijos < dgi.ChildItem.NestedTableViews[2].Items.Count)
                                {
                                    GridDataItem dgv = dgi.ChildItem.NestedTableViews[2].Items[hijos];
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2)
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                }                                

                                tempTable = Add(tempTable, string.Format(row, tempRow));
                                tempRow = string.Empty;
                                hijos++;
                            }
                        }
                    }
                }
                gridsCount++;
                tempTable = Add(tempTable, blankRow);
            }

            table = string.Format(table, tempTable);

            return table;

        }



        private static Boolean ValidarColumna(GridColumn gdColumna)
        {
            if (gdColumna.Display == false) return false;
            if (gdColumna.ColumnType == "GridButtonColumn") return false;
            if (gdColumna.ColumnType == "GridTemplateColumn") return false;
            if (gdColumna.ColumnType == "GridHyperLinkColumn") return false;
            if (gdColumna.ColumnType == "GridEditCommandColumn") return false;
            if (gdColumna.ColumnType == "GridClientSelectColumn") return false;

            return true;
        }

        public static DataSet GenerarExcelDetallesPM(List<RadGrid> listaGrids, string[] nombresGrids, string tituloReporte, string nombreUsuario)
        {

            DataTable dtReportePM = new DataTable("Reporte PM");
            dtReportePM.Columns.Add("01 RFC");
            dtReportePM.Columns.Add("02 CURP");
            dtReportePM.Columns.Add("03 Compañía");
            dtReportePM.Columns.Add("04 Nacionalidad");
            dtReportePM.Columns.Add("05 Calificación");
            dtReportePM.Columns.Add("06 Banxico");
            dtReportePM.Columns.Add("07 Dirección");
            dtReportePM.Columns.Add("08 Colonia");
            dtReportePM.Columns.Add("09 Municipio");
            dtReportePM.Columns.Add("10 Ciudad");
            dtReportePM.Columns.Add("11 Estado");
            dtReportePM.Columns.Add("12 Código Postal");
            dtReportePM.Columns.Add("13 Tipo Cliente");
            dtReportePM.Columns.Add("14 País de domicilio");
            dtReportePM.Columns.Add("15 Clave Consolidación");
            dtReportePM.Columns.Add("16 RFC");
            dtReportePM.Columns.Add("17 # Contrato");
            dtReportePM.Columns.Add("18 Contrato Anterior");
            dtReportePM.Columns.Add("19 Apertura");
            dtReportePM.Columns.Add("20 Plazo");
            dtReportePM.Columns.Add("21 Tipo");
            dtReportePM.Columns.Add("22 Saldo Inicial");
            dtReportePM.Columns.Add("23 Moneda");
            dtReportePM.Columns.Add("24 # Pagos");
            dtReportePM.Columns.Add("25 Frecuencia");
            dtReportePM.Columns.Add("26 Importe");
            dtReportePM.Columns.Add("27 Último Pago");
            dtReportePM.Columns.Add("28 Reestructura");
            dtReportePM.Columns.Add("29 Pago");
            dtReportePM.Columns.Add("30 Liquidación");
            dtReportePM.Columns.Add("31 Quita");
            dtReportePM.Columns.Add("32 Dación");
            dtReportePM.Columns.Add("33 Quebranto");
            dtReportePM.Columns.Add("34 Monto");
            dtReportePM.Columns.Add("35 Monto Vencido");
            dtReportePM.Columns.Add("36 Dias Vencido");
            dtReportePM.Columns.Add("37 Fecha de Primer Incumplimiento");
            dtReportePM.Columns.Add("38 Saldo Insoluto");
            dtReportePM.Columns.Add("39 Clave Observacion");
            dtReportePM.Columns.Add("40 RFC Accionista");
            dtReportePM.Columns.Add("41 Compañía Accionista");
            dtReportePM.Columns.Add("42 Nombre Accionista");
            dtReportePM.Columns.Add("43 Apellido Paterno");
            dtReportePM.Columns.Add("44 Apellido Materno");
            dtReportePM.Columns.Add("45 Porcentaje");
            dtReportePM.Columns.Add("46 Direccion Accionista");
            dtReportePM.Columns.Add("47 Colonia o Poblacion");
            dtReportePM.Columns.Add("48 Delegacion o Municipio");
            dtReportePM.Columns.Add("49 Ciudad");
            dtReportePM.Columns.Add("50 Estado (Mexico)");
            dtReportePM.Columns.Add("51 Codigo Postal");

            dtReportePM.Columns.Add("52 No. Credito");
            dtReportePM.Columns.Add("53 RFC Aval");
            dtReportePM.Columns.Add("54 Compañía Aval");
            dtReportePM.Columns.Add("55 Nombre Aval");
            dtReportePM.Columns.Add("56 Apellido Paterno");
            dtReportePM.Columns.Add("57 Apellido Materno");
            dtReportePM.Columns.Add("58 Direccion Aval");
            dtReportePM.Columns.Add("59 Colonia o Poblacion");
            dtReportePM.Columns.Add("60 Delegacion o Municipio");
            dtReportePM.Columns.Add("61 Ciudad");
            dtReportePM.Columns.Add("62 Estado (Mexico)");
            dtReportePM.Columns.Add("63 Codigo Postal");

            List<int> ColumnasPorSeccion = new List<int>();

            foreach (RadGrid dg in listaGrids)
            {

                // Recorremos las filas del Grid Principal
                foreach (GridDataItem gdItem in dg.Items)
                {
                    
                    // Revisamos si la Fila actual tiene Vistas Hijo
                    if (gdItem.ChildItem != null)
                    {

                        int numeroDetalles = gdItem.ChildItem.NestedTableViews.Length;
                        int numeroAccionistas = gdItem.ChildItem.NestedTableViews[1].Items.Count;
                        int numeroCreditos = gdItem.ChildItem.NestedTableViews[0].Items.Count;
                        int numeroAvales = gdItem.ChildItem.NestedTableViews[2].Items.Count;

                        int ActualAccionista = 0;
                        int ActualCredito = 0; 
                        int AvalesFaltantes = 0;
                        int AvalesActualCredito = 0;

                        while (ActualAccionista < numeroAccionistas || ActualCredito < numeroCreditos || AvalesFaltantes > 0)
                        {

                            // ACREDITADO Agregamos la fila con sus datos 
                            DataRow drFila;
                            drFila = dtReportePM.NewRow();
                            drFila["01 RFC"] = gdItem.Cells[2].Text.Replace("&nbsp;", "");   
                            drFila["02 CURP"] = gdItem.Cells[3].Text.Replace("&nbsp;", "");  
                            drFila["03 Compañía"] = gdItem.Cells[4].Text.Replace("&nbsp;", "");  
                            drFila["04 Nacionalidad"] = gdItem.Cells[5].Text.Replace("&nbsp;", "");   
                            drFila["05 Calificación"] = gdItem.Cells[6].Text.Replace("&nbsp;", "");   
                            drFila["06 Banxico"] = gdItem.Cells[7].Text.Replace("&nbsp;", "");   
                            drFila["07 Dirección"] = gdItem.Cells[8].Text.Replace("&nbsp;", "");   
                            drFila["08 Colonia"] = gdItem.Cells[9].Text.Replace("&nbsp;", "");   
                            drFila["09 Municipio"] = gdItem.Cells[10].Text.Replace("&nbsp;", "");  
                            drFila["10 Ciudad"] = gdItem.Cells[11].Text.Replace("&nbsp;", "");  
                            drFila["11 Estado"] = gdItem.Cells[12].Text.Replace("&nbsp;", "");  
                            drFila["12 Código Postal"] = gdItem.Cells[13].Text.Replace("&nbsp;", "");  
                            drFila["13 Tipo Cliente"] = gdItem.Cells[14].Text.Replace("&nbsp;", "");  
                            drFila["14 País de domicilio"] = gdItem.Cells[15].Text.Replace("&nbsp;", "");  
                            drFila["15 Clave Consolidación"] = gdItem.Cells[16].Text.Replace("&nbsp;", "");  

                            // ACCIONISTA Si existe lo agregamos
                            if (ActualAccionista < numeroAccionistas)
                            {
                                drFila["40 RFC Accionista"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[2].Text.Replace("&nbsp;", "");  
                                drFila["41 Compañía Accionista"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[3].Text.Replace("&nbsp;", ""); 
                                drFila["42 Nombre Accionista"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[4].Text.Replace("&nbsp;", "");  
                                drFila["43 Apellido Paterno"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[5].Text.Replace("&nbsp;", "");  
                                drFila["44 Apellido Materno"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[6].Text.Replace("&nbsp;", "");  
                                drFila["45 Porcentaje"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[7].Text.Replace("&nbsp;", "");  
                                drFila["46 Direccion Accionista"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[8].Text.Replace("&nbsp;", ""); 
                                drFila["47 Colonia o Poblacion"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[9].Text.Replace("&nbsp;", "");  
                                drFila["48 Delegacion o Municipio"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[10].Text.Replace("&nbsp;", "");  
                                drFila["49 Ciudad"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[11].Text.Replace("&nbsp;", "");  
                                drFila["50 Estado (Mexico)"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[12].Text.Replace("&nbsp;", ""); 
                                drFila["51 Codigo Postal"] = gdItem.ChildItem.NestedTableViews[1].Items[ActualAccionista].Cells[13].Text.Replace("&nbsp;", ""); 
                                ActualAccionista++;
                            }

                            // CREDITO Si hay credito y no hay avales faltes de un credito anterior agregamos el nuevo credito
                            // de lo contrario agregamos espacios en blanco para continuar cargando los avales pendientes
                            if (ActualCredito < numeroCreditos && AvalesFaltantes == 0)
                            {
                                drFila["16 RFC"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[2].Text.Replace("&nbsp;", "");   
                                drFila["17 # Contrato"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[3].Text.Replace("&nbsp;", "");   
                                drFila["18 Contrato Anterior"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[4].Text.Replace("&nbsp;", "");  
                                drFila["19 Apertura"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[5].Text.Replace("&nbsp;", "");   
                                drFila["20 Plazo"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[6].Text.Replace("&nbsp;", "");   
                                drFila["21 Tipo"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[7].Text.Replace("&nbsp;", "");   
                                drFila["22 Saldo Inicial"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[8].Text.Replace("&nbsp;", "");   
                                drFila["23 Moneda"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[9].Text.Replace("&nbsp;", "");   
                                drFila["24 # Pagos"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[10].Text.Replace("&nbsp;", "");  
                                drFila["25 Frecuencia"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[11].Text.Replace("&nbsp;", "");  
                                drFila["26 Importe"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[12].Text.Replace("&nbsp;", "");  
                                drFila["27 Último Pago"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[13].Text.Replace("&nbsp;", "");  
                                drFila["28 Reestructura"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[14].Text.Replace("&nbsp;", "");  
                                drFila["29 Pago"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[15].Text.Replace("&nbsp;", ""); 
                                drFila["30 Liquidación"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[16].Text.Replace("&nbsp;", "");  
                                drFila["31 Quita"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[17].Text.Replace("&nbsp;", "");  
                                drFila["32 Dación"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[18].Text.Replace("&nbsp;", "");  
                                drFila["33 Quebranto"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[19].Text.Replace("&nbsp;", "");  
                                drFila["34 Monto"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[20].Text.Replace("&nbsp;", "");  
                                drFila["35 Monto Vencido"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[21].Text.Replace("&nbsp;", "");  
                                drFila["36 Dias Vencido"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[22].Text.Replace("&nbsp;", "");  
                                drFila["37 Fecha de Primer Incumplimiento"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[23].Text.Replace("&nbsp;", "");  
                                drFila["38 Saldo Insoluto"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[24].Text.Replace("&nbsp;", "");  
                                drFila["39 Clave Observacion"] = gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[25].Text.Replace("&nbsp;", ""); 

                                // Revisamos si el credito tiene avales para agregarlos
                                int NumAvales = 0;
                                for (int ActualAval = 0; ActualAval < numeroAvales; ActualAval++)
                                {
                                    if (gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito].Cells[3].Text == gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[2].Text)
                                    {
                                        NumAvales++;

                                        // Si es el primer aval lo agreamos a nivel del registro del credito
                                        if (NumAvales == 1)
                                        {
                                            drFila["52 No. Credito"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[2].Text.Replace("&nbsp;", "");
                                            drFila["53 RFC Aval"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[3].Text.Replace("&nbsp;", "");
                                            drFila["54 Compañía Aval"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[4].Text.Replace("&nbsp;", "");
                                            drFila["55 Nombre Aval"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[5].Text.Replace("&nbsp;", "");
                                            drFila["56 Apellido Paterno"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[6].Text.Replace("&nbsp;", "");
                                            drFila["57 Apellido Materno"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[7].Text.Replace("&nbsp;", "");
                                            drFila["58 Direccion Aval"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[8].Text.Replace("&nbsp;", "");
                                            drFila["59 Colonia o Poblacion"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[9].Text.Replace("&nbsp;", "");
                                            drFila["60 Delegacion o Municipio"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[10].Text.Replace("&nbsp;", "");
                                            drFila["61 Ciudad"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[11].Text.Replace("&nbsp;", "");
                                            drFila["62 Estado (Mexico)"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[12].Text.Replace("&nbsp;", "");
                                            drFila["63 Codigo Postal"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[13].Text.Replace("&nbsp;", "");  
                                        }
                                    }
                                }
                                AvalesFaltantes = NumAvales - 1;
                                if (AvalesFaltantes < 0) { AvalesFaltantes = 0; }
                                                                
                                ActualCredito++;
                                AvalesActualCredito = 0;
                            }
                            else
                            {
                                // Revisamos si el credito tiene avales faltantes para agregarlos
                                AvalesActualCredito++;
                                
                                int NumAvales = 0;

                                for (int ActualAval = 0; ActualAval < numeroAvales; ActualAval++)
                                {
                                    if (gdItem.ChildItem.NestedTableViews[0].Items[ActualCredito-1].Cells[3].Text == gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[2].Text)
                                    {
                                        NumAvales++;
                                        if (NumAvales > AvalesActualCredito)
                                        {
                                            drFila["52 No. Credito"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[2].Text.Replace("&nbsp;", "");   
                                            drFila["53 RFC Aval"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[3].Text.Replace("&nbsp;", "");  
                                            drFila["54 Compañía Aval"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[4].Text.Replace("&nbsp;", "");   
                                            drFila["55 Nombre Aval"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[5].Text.Replace("&nbsp;", "");   
                                            drFila["56 Apellido Paterno"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[6].Text.Replace("&nbsp;", "");   
                                            drFila["57 Apellido Materno"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[7].Text.Replace("&nbsp;", "");   
                                            drFila["58 Direccion Aval"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[8].Text.Replace("&nbsp;", "");   
                                            drFila["59 Colonia o Poblacion"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[9].Text.Replace("&nbsp;", "");   
                                            drFila["60 Delegacion o Municipio"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[10].Text.Replace("&nbsp;", "");  
                                            drFila["61 Ciudad"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[11].Text.Replace("&nbsp;", "");  
                                            drFila["62 Estado (Mexico)"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[12].Text.Replace("&nbsp;", "");  
                                            drFila["63 Codigo Postal"] = gdItem.ChildItem.NestedTableViews[2].Items[ActualAval].Cells[13].Text.Replace("&nbsp;", "");  
                                            
                                            // Agregamos el siguiente aval, disminuimos el contador, registramos el ultimo aval agregado y
                                            // salimos del for para agregar una nueva linea.
                                            AvalesFaltantes = AvalesFaltantes - 1;
                                            break;
                                        }
                                    }
                                }
                            }

                            dtReportePM.Rows.Add(drFila);
                        }

                        int TotalFilas = dtReportePM.Rows.Count;
                    }

                }
            }

            DataSet dsFinal = new DataSet();
            dsFinal.Tables.Add(dtReportePM);

            return dsFinal;
        }



        private static string Add(string string1, string string2)
        {
            string string3 = string.Empty;
            string3 = string.Format("{0}{1}", string1, string2);

            return string3;
        }       

        public static string GenerarExcelDetallesFisicas(List<RadGrid> listaGrids, string[] nombresGrids, string tituloReporte, string nombreUsuario)
        {
            string table = "<table>{0}</table>";
            string tempTable = string.Empty;
            string row = "<tr>{0}</tr>";
            string blankRow = "<tr></tr>";
            string tempRow = string.Empty;
            string gridTitle = "<td style=\"text-align:center;\" colspan=\"{0}\">{1}</td>";
            string itemBold = "<td style=\"border: 1 solid;\"><b>{0}</b></td>";
            string item = "<td>{0}</td>";
            string headers = string.Empty;
            string[] headersArray = null;
            char[] splitter = new char[] { ';' };
            int columnsCount = 0;
            int gridsCount = 0;
            List<int> columnasHijas;
            foreach (RadGrid dg in listaGrids)
            {

                gridTitle = "<td style=\"text-align:center;\" colspan=\"{0}\">{1}</td>";
                tempRow = string.Empty;
                headers = string.Empty;
                columnsCount = 0;
                columnasHijas = new List<int>();
                foreach (GridColumn gdColumn in dg.Columns)
                {

                    if (gdColumn.Display && (gdColumn.ColumnType != "GridButtonColumn" && gdColumn.ColumnType != "GridTemplateColumn" && gdColumn.ColumnType != "GridHyperLinkColumn" && gdColumn.ColumnType != "GridEditCommandColumn" && gdColumn.ColumnType != "GridClientSelectColumn"))
                    {

                        if (headers == string.Empty)
                        {
                            headers = gdColumn.HeaderText;
                        }
                        else
                        {
                            headers = string.Format("{0};{1}", headers, gdColumn.HeaderText);
                        }
                        columnsCount++;
                    }

                }
                columnasHijas.Add(columnsCount);
                foreach (GridTableView gtv in dg.MasterTableView.DetailTables)
                {
                    if (!string.IsNullOrWhiteSpace(gtv.Name))
                    {
                        int columnashijas = 0;
                        foreach (GridColumn gdColumn in gtv.Columns)
                        {
                            if (gdColumn.Display && (gdColumn.ColumnType != "GridButtonColumn" && gdColumn.ColumnType != "GridTemplateColumn" && gdColumn.ColumnType != "GridHyperLinkColumn" && gdColumn.ColumnType != "GridEditCommandColumn" && gdColumn.ColumnType != "GridClientSelectColumn"))
                            {

                                if (headers == string.Empty)
                                {
                                    headers = gdColumn.HeaderText;
                                }
                                else
                                {
                                    headers = string.Format("{0};{1}", headers, gdColumn.HeaderText);
                                }
                                columnashijas++;
                                columnsCount++;
                            }
                        }
                        columnasHijas.Add(columnashijas);
                    }
                }

                gridTitle = string.Format(gridTitle, columnsCount.ToString(), "Detalle Personas Fisicas");
                tempRow = string.Format(row, Add(tempRow, gridTitle));

                //Agrega Titulo del Grid
                tempTable = Add(tempTable, tempRow);

                headersArray = headers.Split(splitter);
                tempRow = string.Empty;

                for (int i = 0; i < columnsCount; i++)
                {
                    tempRow = Add(tempRow, string.Format(itemBold, headersArray[i]));
                }

                //Agrega los titulos de las columnas del grid
                tempTable = Add(tempTable, string.Format(row, tempRow));

                foreach (GridDataItem dgi in dg.Items)
                {
                    
                    bool registroprimerpersona = false;
                    tempRow = string.Empty;
                    int repeats = 0;
                    foreach (TableCell celda in dgi.Cells)
                    {
                        if (repeats >= 2 && (repeats - 2) < dg.MasterTableView.Columns.Count)
                        {
                            if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                            {
                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda)));
                            }
                        }
                        registroprimerpersona = true;
                        repeats++;
                    }
                    int hijos = 0;
                    if (dgi.ChildItem != null)
                    {
                        if (dgi.ChildItem.NestedTableViews.Length == 1)
                        {
                            if (dgi.ChildItem.NestedTableViews[0].Name == "Direccion")
                            {
                                foreach (GridDataItem dgv in dgi.ChildItem.NestedTableViews[0].Items)
                                {
                                    if (!registroprimerpersona)
                                    {
                                         repeats = 0;
                                         foreach (TableCell celda in dgi.Cells)
                                         {
                                            if (repeats >= 2 && (repeats - 2) < dg.MasterTableView.Columns.Count)
                                            {
                                                if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                                                {
                                                    tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda)));
                                                }
                                            }
                                            repeats++;
                                        }
                                    }
                                    registroprimerpersona = false;
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2 && (repeatsdetail -2) < dgi.ChildItem.NestedTableViews[0].Columns.Count)
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                    tempTable = Add(tempTable, string.Format(row, tempRow));
                                    tempRow = string.Empty;
                                }
                            }
                            else
                            {
                                foreach (GridDataItem dgv in dgi.ChildItem.NestedTableViews[0].Items)
                                {
                                    if (!registroprimerpersona)
                                    {
                                        repeats = 0;
                                         foreach (TableCell celda in dgi.Cells)
                                         {
                                            if (repeats >= 2 && (repeats - 2) < dg.MasterTableView.Columns.Count)
                                            {
                                                if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                                                {
                                                    tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda)));
                                                }
                                            }
                                            repeats++;
                                        }
                                    }
                                    registroprimerpersona = false;
                                    for (int i = 0; i <= columnasHijas[1]-1; i++)
                                    {
                                        tempRow = Add(tempRow, string.Format(item, ""));
                                    }
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2 && (repeatsdetail - 2) < dgi.ChildItem.NestedTableViews[0].Columns.Count)
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                    tempTable = Add(tempTable, string.Format(row, tempRow));
                                    tempRow = string.Empty;
                                }
                            }
                        }
                        else
                        {
                            while (hijos < dgi.ChildItem.NestedTableViews[0].Items.Count || hijos < dgi.ChildItem.NestedTableViews[2].Items.Count)
                            {
                                if (!registroprimerpersona)
                                {
                                    repeats = 0;
                                         foreach (TableCell celda in dgi.Cells)
                                         {
                                            if (repeats >= 2 && (repeats - 2) < dg.MasterTableView.Columns.Count)
                                            {
                                                if (dg.MasterTableView.Columns[repeats - 2].Display && (dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridButtonColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridTemplateColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridHyperLinkColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridEditCommandColumn" && dg.MasterTableView.Columns[repeats - 2].ColumnType != "GridClientSelectColumn"))
                                                {
                                                    tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dg.MasterTableView.Columns[repeats - 2].ColumnType, celda)));
                                                }
                                            }
                                            repeats++;
                                        }
                                }
                                registroprimerpersona = false;
                                if (hijos < dgi.ChildItem.NestedTableViews[0].Items.Count)
                                {
                                    GridDataItem dgv = dgi.ChildItem.NestedTableViews[0].Items[hijos];
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2 && (repeatsdetail - 2) < dgi.ChildItem.NestedTableViews[0].Columns.Count)
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                }
                                else
                                {
                                    if (dgi.ChildItem.NestedTableViews[0].Items.Count>0)
                                    {
                                        
                                             GridDataItem dgv = dgi.ChildItem.NestedTableViews[0].Items[0];
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2 && (repeatsdetail - 2) < dgi.ChildItem.NestedTableViews[0].Columns.Count)
                                        {
                                            if (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[0].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                    }
                                    else
                                    {
                                    for (int i = 0; i <= columnasHijas[1] - 1; i++)
                                    {
                                        tempRow = Add(tempRow, string.Format(item, ""));
                                    }
                                    }
                                }
                                if (hijos < dgi.ChildItem.NestedTableViews[2].Items.Count)
                                {
                                    GridDataItem dgv = dgi.ChildItem.NestedTableViews[2].Items[hijos];
                                    int repeatsdetail = 0;
                                    foreach (TableCell celda in dgv.Cells)
                                    {
                                        if (repeatsdetail >= 2 &&(repeatsdetail - 2) < dgi.ChildItem.NestedTableViews[2].Columns.Count)
                                        {
                                            if (dgi.ChildItem.NestedTableViews[2].Columns[repeatsdetail - 2].Display && (dgi.ChildItem.NestedTableViews[2].Columns[repeatsdetail - 2].ColumnType != "GridButtonColumn" && dgi.ChildItem.NestedTableViews[2].Columns[repeatsdetail - 2].ColumnType != "GridTemplateColumn" && dgi.ChildItem.NestedTableViews[2].Columns[repeatsdetail - 2].ColumnType != "GridHyperLinkColumn" && dgi.ChildItem.NestedTableViews[2].Columns[repeatsdetail - 2].ColumnType != "GridEditCommandColumn" && dgi.ChildItem.NestedTableViews[2].Columns[repeatsdetail - 2].ColumnType != "GridClientSelectColumn"))
                                            {
                                                tempRow = Add(tempRow, string.Format(item, ExportarAPdf.GetTextFromCell(dgi.ChildItem.NestedTableViews[2].Columns[repeatsdetail - 2].ColumnType, celda)));
                                            }
                                        }
                                        repeatsdetail++;
                                    }
                                }
                                tempTable = Add(tempTable, string.Format(row, tempRow));
                                tempRow = string.Empty;
                                hijos++;
                            }
                        }
                    }
                }
                gridsCount++;
                tempTable = Add(tempTable, blankRow);
            }

            table = string.Format(table, tempTable);

            return table;

        }
            

    }
}
