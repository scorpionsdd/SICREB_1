using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities.Types.PM;

namespace Banobras.Credito.SICREB.Entities.Types
{

    public class PM_Cinta : SegmentoType<PM_Cinta, PM_Cinta>
    {

        private Enums.Estado estado = Enums.Estado.Activo;
        public int NumErrores { get; set; }
        public int NumWarnings { get; set; }
        public int ArchivoId { get; set; }

        #region Segmentos

        public PM_HD HD { get; set; }
        public List<PM_EM> EMs { get; private set; }
        public PM_TS TS { get; set; }
        public List<Segmento> Segmentos { get; private set; }

        #endregion

        #region Constructores

        public PM_Cinta()
            : base(Enums.Persona.Moral)
        { }

        public PM_Cinta(List<Segmento> segmentos, List<Etiqueta> etiquetas, List<Validacion> validaciones)
            : base(Enums.Persona.Moral)
        {
            this.Segmentos = segmentos;
            this.Etiquetas = etiquetas;
            this.Validaciones = validaciones;
        }

        #endregion

        public override void InicializaEmpty()
        {
            EMs = new List<PM_EM>();

            // Inicializamos los contadores de Errores y Warnings
            NumErrores = 0;
            NumWarnings = 0;
        }

        public void AgregaEM(PM_EM em)
        {
            if (this.EMs == null)
            {
                this.EMs = new List<PM_EM>();
            }

            this.EMs.Add(em);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            System.Data.DataSet ds = new System.Data.DataSet();  // Generamos dataset para la exportacion a excel
            
            ds.Tables.Add("HD");
            ds.Tables["HD"].Columns.Add("HDHD");
            ds.Tables["HD"].Columns.Add("HD00");
            ds.Tables["HD"].Columns.Add("HD01");
            ds.Tables["HD"].Columns.Add("HD02");
            ds.Tables["HD"].Columns.Add("HD03");
            ds.Tables["HD"].Columns.Add("HD04");
            ds.Tables["HD"].Columns.Add("HD05");
            ds.Tables["HD"].Columns.Add("HD06");
            ds.Tables["HD"].Columns.Add("HD07"); // PM_v4 - 24/06/2015  Se recorre HD07 a HD08
            ds.Tables["HD"].Columns.Add("HD08");  
            
            ds.Tables.Add("EM");
            ds.Tables["EM"].Columns.Add("EMEM");
            ds.Tables["EM"].Columns.Add("EM00");
            ds.Tables["EM"].Columns.Add("EM01");
            ds.Tables["EM"].Columns.Add("EM02");
            ds.Tables["EM"].Columns.Add("EM03");
            ds.Tables["EM"].Columns.Add("EM04");
            ds.Tables["EM"].Columns.Add("EM05");
            ds.Tables["EM"].Columns.Add("EM06");
            ds.Tables["EM"].Columns.Add("EM07");
            ds.Tables["EM"].Columns.Add("EM08");
            ds.Tables["EM"].Columns.Add("EM09");
            ds.Tables["EM"].Columns.Add("EM10");
            ds.Tables["EM"].Columns.Add("EM11");
            ds.Tables["EM"].Columns.Add("EM12");
            ds.Tables["EM"].Columns.Add("EM13");
            ds.Tables["EM"].Columns.Add("EM14");
            ds.Tables["EM"].Columns.Add("EM15");
            ds.Tables["EM"].Columns.Add("EM16");
            ds.Tables["EM"].Columns.Add("EM17");
            ds.Tables["EM"].Columns.Add("EM18");
            ds.Tables["EM"].Columns.Add("EM19");
            ds.Tables["EM"].Columns.Add("EM20");
            ds.Tables["EM"].Columns.Add("EM21");
            ds.Tables["EM"].Columns.Add("EM22");
            ds.Tables["EM"].Columns.Add("EM23");
            ds.Tables["EM"].Columns.Add("EM24");
            ds.Tables["EM"].Columns.Add("EM25");
            ds.Tables["EM"].Columns.Add("EM26");
            ds.Tables["EM"].Columns.Add("EM27");
            
            ds.Tables.Add("AC");
            ds.Tables["AC"].Columns.Add("ACAC");
            ds.Tables["AC"].Columns.Add("AC00");
            ds.Tables["AC"].Columns.Add("AC01");
            ds.Tables["AC"].Columns.Add("AC02");
            ds.Tables["AC"].Columns.Add("AC03");
            ds.Tables["AC"].Columns.Add("AC04");
            ds.Tables["AC"].Columns.Add("AC05");
            ds.Tables["AC"].Columns.Add("AC06");
            ds.Tables["AC"].Columns.Add("AC07");
            ds.Tables["AC"].Columns.Add("AC08");
            ds.Tables["AC"].Columns.Add("AC09");
            ds.Tables["AC"].Columns.Add("AC10");
            ds.Tables["AC"].Columns.Add("AC11");
            ds.Tables["AC"].Columns.Add("AC12");
            ds.Tables["AC"].Columns.Add("AC13");
            ds.Tables["AC"].Columns.Add("AC14");
            ds.Tables["AC"].Columns.Add("AC15");
            ds.Tables["AC"].Columns.Add("AC16");
            ds.Tables["AC"].Columns.Add("AC17");
            ds.Tables["AC"].Columns.Add("AC18");
            ds.Tables["AC"].Columns.Add("AC19");
            ds.Tables["AC"].Columns.Add("AC20");
            ds.Tables["AC"].Columns.Add("AC21");
            ds.Tables["AC"].Columns.Add("AC22");
            
            ds.Tables.Add("CR");
            ds.Tables["CR"].Columns.Add("CRCR");
            ds.Tables["CR"].Columns.Add("CR00");
            ds.Tables["CR"].Columns.Add("CR01");
            ds.Tables["CR"].Columns.Add("CR02");
            ds.Tables["CR"].Columns.Add("CR03");
            ds.Tables["CR"].Columns.Add("CR04");
            ds.Tables["CR"].Columns.Add("CR05");
            ds.Tables["CR"].Columns.Add("CR06");
            ds.Tables["CR"].Columns.Add("CR07");
            ds.Tables["CR"].Columns.Add("CR08");
            ds.Tables["CR"].Columns.Add("CR09");
            ds.Tables["CR"].Columns.Add("CR10");
            ds.Tables["CR"].Columns.Add("CR11");
            ds.Tables["CR"].Columns.Add("CR12");
            ds.Tables["CR"].Columns.Add("CR13");
            ds.Tables["CR"].Columns.Add("CR14");
            ds.Tables["CR"].Columns.Add("CR15");
            ds.Tables["CR"].Columns.Add("CR16");
            ds.Tables["CR"].Columns.Add("CR17");
            ds.Tables["CR"].Columns.Add("CR18");
            ds.Tables["CR"].Columns.Add("CR19");
            ds.Tables["CR"].Columns.Add("CR20");
            ds.Tables["CR"].Columns.Add("CR21");
            ds.Tables["CR"].Columns.Add("CR22");
            ds.Tables["CR"].Columns.Add("CR23"); // PM_v4 - 24/06/2015  Se recorre CR23 a CR24
            ds.Tables["CR"].Columns.Add("CR24");

            //<MASS 08-nov-2017 ajustes PM-V05>            
            ds.Tables["CR"].Columns.Add("CR25");            
            //</MASS>
            
            ds.Tables.Add("DE");
            ds.Tables["DE"].Columns.Add("DEDE");
            ds.Tables["DE"].Columns.Add("DE00");
            ds.Tables["DE"].Columns.Add("DE01");
            ds.Tables["DE"].Columns.Add("DE02");
            ds.Tables["DE"].Columns.Add("DE03");
            ds.Tables["DE"].Columns.Add("DE04");
            //<MASS 19-oct-2017 ajustes PM-V05
            ds.Tables["DE"].Columns.Add("DE05");
            //</MASS>
            
            ds.Tables.Add("AV");
            ds.Tables["AV"].Columns.Add("AVAC");
            ds.Tables["AV"].Columns.Add("AV00");
            ds.Tables["AV"].Columns.Add("AV01");
            ds.Tables["AV"].Columns.Add("AV02");
            ds.Tables["AV"].Columns.Add("AV03");
            ds.Tables["AV"].Columns.Add("AV04");
            ds.Tables["AV"].Columns.Add("AV05");
            ds.Tables["AV"].Columns.Add("AV06");
            ds.Tables["AV"].Columns.Add("AV07");
            ds.Tables["AV"].Columns.Add("AV08");
            ds.Tables["AV"].Columns.Add("AV09");
            ds.Tables["AV"].Columns.Add("AV10");
            ds.Tables["AV"].Columns.Add("AV11");
            ds.Tables["AV"].Columns.Add("AV12");
            ds.Tables["AV"].Columns.Add("AV13");
            ds.Tables["AV"].Columns.Add("AV14");
            ds.Tables["AV"].Columns.Add("AV15");
            ds.Tables["AV"].Columns.Add("AV16");
            ds.Tables["AV"].Columns.Add("AV17");
            ds.Tables["AV"].Columns.Add("AV18");
            ds.Tables["AV"].Columns.Add("AV19");
            ds.Tables["AV"].Columns.Add("AV20");
            ds.Tables["AV"].Columns.Add("AV21");
            
            ds.Tables.Add("TS");
            ds.Tables["TS"].Columns.Add("TSAC");
            ds.Tables["TS"].Columns.Add("TS00");
            ds.Tables["TS"].Columns.Add("TS01");
            ds.Tables["TS"].Columns.Add("TS02");

            builder.AppendLine(this.HD.ToString(ref ds));

            int iContador = 0;
            int iValidEM, iValidCR, iValidDE;
            string strInfo = string.Empty;

            foreach (PM_EM em in this.EMs) // Recorremos la cinta para encontrar todos los registros validos
            {
                // Reseteamos valores
                iValidEM = 0;
                iValidCR = 0;
                iValidDE = 0;
                strInfo = string.Empty;

                // Verifica si EM es valido e incrementa variable
                if (em.IsValid)
                {
                    iValidEM++;
                    strInfo += em.ToString(ref ds);

                    // Recorremos los Accionistas del Acreditado y agregamos los que sean validos
                    foreach (PM_AC ac in em.ACs)
                    {
                        if (ac.IsValid)
                            strInfo += ac.ToString(ref ds);
                    }

                    // Recorremos los Creditos del Acreditado y agregamos los que sean validos
                    foreach (PM_CR cr in em.CRs)
                    {
                        // Verifica si CR es valido e incrementa variable
                        if (cr.IsValid)
                        {
                            iValidCR++;
                            strInfo += cr.ToString(ref ds);

                            // Recorresmos los Detalles del Credito y agregamos los que sean validos
                            foreach (PM_DE de in cr.DEs)
                            {
                                // Verifica si DE es valido e incrementa variable
                                if (de.IsValid)
                                {
                                    iValidDE++;
                                    strInfo += de.ToString(ref ds);
                                }
                            }

                            // Recorremos los Avales del Credito y agregamos los que sean validos
                            foreach (PM_AV av in cr.AVs)
                            {
                                if (av.IsValid)
                                    strInfo += av.ToString(ref ds);
                            }
                        }
                    } // Fin for CR  
                } // Fin EM isValid

                // Debemos tener por lo menos un EM, CR y DE valido
                if (iValidEM > 0 && iValidCR > 0 && iValidDE > 0)
                {
                    builder.AppendLine(strInfo);
                    iContador++;
                }
            } // Fin recorre cinta para encontrar todos los registros validos
            
            builder.AppendLine(this.TS.ToString(ref ds));

            try
            {
                ds.Tables["HD"].WriteXml(@"C:\sicreb\SICREB_PM_HD.xml");
                ds.Tables["EM"].WriteXml(@"C:\sicreb\SICREB_PM_EM.xml");
                ds.Tables["AC"].WriteXml(@"C:\sicreb\SICREB_PM_AC.xml");
                ds.Tables["CR"].WriteXml(@"C:\sicreb\SICREB_PM_CR.xml");
                ds.Tables["DE"].WriteXml(@"C:\sicreb\SICREB_PM_DE.xml");
                ds.Tables["AV"].WriteXml(@"C:\sicreb\SICREB_PM_AV.xml");
                ds.Tables["TS"].WriteXml(@"C:\sicreb\SICREB_PM_TS.xml");

                ExcelLibrary.SpreadSheet.Workbook excelFile = new ExcelLibrary.SpreadSheet.Workbook();
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["HD"], "HD", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["EM"], "EM", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["AC"], "AC", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["CR"], "CR", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["DE"], "DE", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["AV"], "AV", ref excelFile);
                Util.ExportToExcel.AddDataSheetToExcelWorkbook(ds.Tables["TS"], "TS", ref excelFile);

                excelFile.Save(@"C:\sicreb\SICREB_PM_" + DateTime.Now.Ticks.ToString() + ".xls");
            }
            catch (Exception ex)
            {
                try
                {
                    string sSource = "SICREBService";
                    string sLog = "Application";
                    string sEvent = ex.Message + Environment.NewLine + "Data:" + ex.Data.ToString();
                    if ( !EventLog.SourceExists(sSource) )
                    {
                        EventLog.CreateEventSource(sSource, sLog);
                    }

                    EventLog.WriteEntry(sSource, sEvent);
                    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 889);
                }
                catch 
                {
                    ;
                }
 
                Console.WriteLine(ex.Message);
            }

            // Convertios todo el texto a mayusculas y quitamos los acentos de las vocales
            return builder.ToString().ToUpper().Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U').Replace('Ü', 'U').Replace('Ñ','N');
        }

    }
    
}
