using System;
using System.Collections.Generic;
using System.Text;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Entities.Util;
using System.Diagnostics;

namespace Banobras.Credito.SICREB.Entities.Types
{
    public class PF_Cinta : SegmentoType<PF_Cinta, PF_Cinta>
    {
        #region declaracion cinta


        public PF_INTF INTF { get; set; }
        public List<PF_PN> PNs = new List<PF_PN>();
        public PF_TR TR { get; set; }


        #endregion

        #region Otros Miembros

        private Enums.Estado estado = Enums.Estado.Activo;

        public List<Segmento> Segmentos { get; private set; }

        public int NumErrores { get; set; }
        public int NumWarnings { get; set; }
        public int ArchivoId { get; set; }

        #endregion

        # region Constructores

        public PF_Cinta()
            : base(Enums.Persona.Fisica)
        { }

        public PF_Cinta(List<Segmento> segmentos, List<Etiqueta> etiquetas, List<Validacion> validaciones)
            : base(Enums.Persona.Fisica)
        {
            this.Segmentos = segmentos;
            this.Etiquetas = etiquetas;
            this.Validaciones = validaciones;
        }

        #endregion

        #region Inicializar


        public override void InicializaEmpty()
        {
            PNs = new List<PF_PN>();
            INTF = new PF_INTF(MainRoot);
            TR = new PF_TR(MainRoot);

            //inicializa
            NumErrores = 0;
            NumWarnings = 0;
        }

        #endregion

        public static int GuardarDatoEtiqueta(int IdEtiqueta, int IdArchivo, string Valor)
        {
            return 0;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            #region genera dataset para exportar a excel
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add("INTF");
            ds.Tables["INTF"].Columns.Add("INTF_01");
            ds.Tables["INTF"].Columns.Add("INTF_05");
            ds.Tables["INTF"].Columns.Add("INTF_07");
            ds.Tables["INTF"].Columns.Add("INTF_17");
            ds.Tables["INTF"].Columns.Add("INTF_33");
            ds.Tables["INTF"].Columns.Add("INTF_35");
            ds.Tables["INTF"].Columns.Add("INTF_43");
            ds.Tables["INTF"].Columns.Add("INTF_53");
            ds.Tables.Add("PN");
            ds.Tables["PN"].Columns.Add("PN_PN");
            ds.Tables["PN"].Columns.Add("PN_00");
            ds.Tables["PN"].Columns.Add("PN_01");
            ds.Tables["PN"].Columns.Add("PN_02");
            ds.Tables["PN"].Columns.Add("PN_03");
            ds.Tables["PN"].Columns.Add("PN_04");
            ds.Tables["PN"].Columns.Add("PN_05");
            ds.Tables["PN"].Columns.Add("PN_06");
            ds.Tables["PN"].Columns.Add("PN_07");
            ds.Tables["PN"].Columns.Add("PN_08");
            ds.Tables["PN"].Columns.Add("PN_09");
            ds.Tables["PN"].Columns.Add("PN_10");
            ds.Tables["PN"].Columns.Add("PN_11");
            ds.Tables["PN"].Columns.Add("PN_12");
            ds.Tables["PN"].Columns.Add("PN_13");
            ds.Tables["PN"].Columns.Add("PN_14");
            ds.Tables["PN"].Columns.Add("PN_15");
            ds.Tables["PN"].Columns.Add("PN_16");
            ds.Tables["PN"].Columns.Add("PN_17");
            ds.Tables["PN"].Columns.Add("PN_18");
            ds.Tables["PN"].Columns.Add("PN_20");
            ds.Tables["PN"].Columns.Add("PN_21");
            ds.Tables.Add("PA");
            ds.Tables["PA"].Columns.Add("PA_PA");
            ds.Tables["PA"].Columns.Add("PA_00");
            ds.Tables["PA"].Columns.Add("PA_01");
            ds.Tables["PA"].Columns.Add("PA_02");
            ds.Tables["PA"].Columns.Add("PA_03");
            ds.Tables["PA"].Columns.Add("PA_04");
            ds.Tables["PA"].Columns.Add("PA_05");
            ds.Tables["PA"].Columns.Add("PA_06");
            ds.Tables["PA"].Columns.Add("PA_07");
            ds.Tables["PA"].Columns.Add("PA_08");
            ds.Tables["PA"].Columns.Add("PA_09");
            ds.Tables["PA"].Columns.Add("PA_10");
            ds.Tables["PA"].Columns.Add("PA_11");
            ds.Tables.Add("PE");
            ds.Tables["PE"].Columns.Add("PE_01");
            ds.Tables["PE"].Columns.Add("PE_05");
            ds.Tables["PE"].Columns.Add("PE_07");
            ds.Tables["PE"].Columns.Add("PE_17");
            ds.Tables["PE"].Columns.Add("PE_33");
            ds.Tables["PE"].Columns.Add("PE_35");
            ds.Tables["PE"].Columns.Add("PE_43");
            ds.Tables["PE"].Columns.Add("PE_53");
            ds.Tables.Add("TL");
            ds.Tables["TL"].Columns.Add("TL_01");
            ds.Tables["TL"].Columns.Add("TL_05");
            ds.Tables["TL"].Columns.Add("TL_07");
            ds.Tables["TL"].Columns.Add("TL_17");
            ds.Tables["TL"].Columns.Add("TL_33");
            ds.Tables["TL"].Columns.Add("TL_35");
            ds.Tables["TL"].Columns.Add("TL_43");
            ds.Tables["TL"].Columns.Add("TL_53");
            ds.Tables.Add("TR");
            ds.Tables["TR"].Columns.Add("TR__");
            ds.Tables["TR"].Columns.Add("TR00");
            ds.Tables["TR"].Columns.Add("TR01");
            ds.Tables["TR"].Columns.Add("TR02");


            #endregion

            builder.Append(this.INTF.ToString(ref ds));//TODO: this.INTF.ToString(ds)
            try
            {

                foreach (PF_PN pn in this.PNs)
                {
                    if (pn.IsValid && pn.PAs != null && pn.PAs.Count > 0 && pn.TLs != null && pn.TLs.Count > 0)// && pn.TLs.Exists(delegate(PF.PF_TL tl) { return tl.IsValid == true; }) && pn.PAs.Exists(delegate(PF.PF_PA pa) { return pa.IsValid == true; }))
                    {
                        builder.Append(pn.ToString(ref ds));//TODO: this.INTF.ToString(ds)
                        foreach (PF_PA pa in pn.PAs)
                        {
                            //if (pa.IsValid)
                            builder.Append(pa.ToString(ref ds));//TODO: this.INTF.ToString(ds)
                        }
                        foreach (PF_PE pe in pn.PEs)
                        {
                            //if (pe.IsValid)
                            builder.Append(pe.ToString(ref ds));//TODO: this.INTF.ToString(ds)
                        }

                        //JAGH 07/02/13 se muestran todos los creditos que regrese el procedimiento 
                        for (int i = 0; i < pn.TLs.Count; i++)
                        {
                            if (i == 0)
                            {
                                //if (pn.TLs[i].IsValid)
                                builder.Append(pn.TLs[i].ToString(ref ds));//TODO: this.INTF.ToString(ds)
                            }
                            else
                            {
                                //if (pn.TLs[i].IsValid)
                                //{
                                builder.Append(pn.ToString(ref ds));//TODO: this.INTF.ToString(ds)
                                foreach (PF_PA pa in pn.PAs)
                                {
                                    //if (pa.IsValid)
                                    builder.Append(pa.ToString(ref ds));//TODO: this.INTF.ToString(ds)
                                }
                                foreach (PF_PE pe in pn.PEs)
                                {
                                    //if (pe.IsValid)
                                    builder.Append(pe.ToString(ref ds));//TODO: this.INTF.ToString(ds)
                                }
                                builder.Append(pn.TLs[i].ToString(ref ds));//TODO: this.INTF.ToString(ds)
                                //}
                            }
                        }
                        //fin se muestran todos los creditos que regrese el procedimiento 

                    }
                }
            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }

            builder.AppendLine(this.TR.ToString());//TODO: this.INTF.ToString(ds)
            try
            {
                ds.Tables["INTF"].WriteXml(@"C:\sicreb\SICREB_PF_INTF.xml");
                ds.Tables["PA"].WriteXml(@"C:\sicreb\SICREB_PF_PA.xml");
                ds.Tables["PN"].WriteXml(@"C:\sicreb\SICREB_PF_PN.xml");
                ds.Tables["PE"].WriteXml(@"C:\sicreb\SICREB_PF_PE.xml");
                ds.Tables["TL"].WriteXml(@"C:\sicreb\SICREB_PF_TL.xml");
                ds.Tables["TR"].WriteXml(@"C:\sicreb\SICREB_PF_TR.xml");
                ExcelLibrary.SpreadSheet.Workbook excelFile = new ExcelLibrary.SpreadSheet.Workbook();
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["INTF"], "INTF", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["PA"], "PA", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["PN"], "PN", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["PE"], "PE", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["TL"], "TL", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["TR"], "TR", ref excelFile);
                excelFile.Save(@"C:\sicreb\SICREB_PF_" + DateTime.Now.Ticks.ToString() + ".xls");

            }
            catch (Exception ex)
            {
                try
                {
                    string sSource = "SICREBService";
                    string sLog = "Application";
                    string sEvent = ex.Message + Environment.NewLine + "Data:" + ex.Data.ToString();
                    if (!EventLog.SourceExists(sSource))
                        EventLog.CreateEventSource(sSource, sLog);

                    EventLog.WriteEntry(sSource, sEvent);
                    EventLog.WriteEntry(sSource, sEvent,
                        EventLogEntryType.Warning, 889);
                }
                catch 
                {
                    ;
                } 
                Console.WriteLine(ex.Message);
            }
 
            return builder.ToString().ToUpper().Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U').Replace('Ü', 'U').Replace('Ñ', 'N');
        }


    }





}
