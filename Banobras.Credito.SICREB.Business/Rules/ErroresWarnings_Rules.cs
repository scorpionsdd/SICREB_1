using System.Collections.Generic;
using System.Linq;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Common;


namespace Banobras.Credito.SICREB.Business.Rules
{
    public class ErroresWarnings_Rules
    {
        
        public ErroresWarnings_Rules()
        {
            
        }

        public List<ErrorWarningInfo> GetErroresPresentacion(Enums.Persona persona)
        {
            ValidacionesDataAccess validaciones = new ValidacionesDataAccess(persona);
            List<Validacion> vals = validaciones.GetRecords(true);

            ErrorAdvertenciaDataAccess errores = new ErrorAdvertenciaDataAccess();
            List<ErrorWarning> errs = errores.GetRegistros(persona);
            
            EtiquetasDataAccess etiquetas = new EtiquetasDataAccess();
            List<Etiqueta> etis = etiquetas.GetRecords(true);

            SegmentosDataAccess segmentos = new SegmentosDataAccess();
            List<Segmento> segs = segmentos.GetRecords(true);

            List<ErrorWarningInfo> result = new List<ErrorWarningInfo>();
            result = (from er in errs
                      join va in vals
                          on er.ValidacionId equals va.Id 
                      join et in etis
                          on va.Etiqueta_Id equals et.Id
                      join sg in segs
                          on et.SegmentoId equals sg.Id
                      select new ErrorWarningInfo(er, va, sg, et)).ToList();

            return result;
        }

        //SICREB-INICIO-VHCC SEP-2012
        //JAGH se agregan grupos seleccionados  08/01/13
        public System.Data.DataTable GetErroresPresentacion(Enums.Persona persona, int iErr_Adver, string strGrupos)
        {
            System.Data.DataTable dtErrores = new System.Data.DataTable();
            System.Data.DataTable dtInformacionAux = new System.Data.DataTable();

            ErrorAdvertenciaDataAccess errores = new ErrorAdvertenciaDataAccess();
            //dtErrores = errores.GetRegistros(persona, iErr_Adver);
            //System.Data.DataTable dtInformacionAux = errores.GetRegistros(persona, iErr_Adver, "sp_trans_geterror_adv", strGrupos);  //JAGH 08/05/2013
            //System.Data.DataTable dtInformacionAux = errores.GetRegistros(persona, iErr_Adver, "PKG_GET_ERROR_ADV.SP_ERR_ADV_PM", strGrupos);

            //MASS. 22octubre2021 para pruebas en ambiente de desarrollo
            if (WebConfig.MailFrom.StartsWith("desarrollo"))
            {
                dtInformacionAux = errores.GetRegistros(persona, iErr_Adver, "PKG_GET_ERROR_ADV_Z.SP_ERR_ADV_PM", strGrupos);
            }
            else
            {
                dtInformacionAux = errores.GetRegistros(persona, iErr_Adver, "PKG_GET_ERROR_ADV.SP_ERR_ADV_PM", strGrupos);
            }

            System.Data.DataTable dtRFCErrores = errores.GetRegistros(persona, iErr_Adver, "SP_BUSCARFC_ERR_WARN", string.Empty); // el store no requiere grupos

            //dtErrores = ValidacionErroresWarnings(dtInformacionAux, dtRFCErrores); //JAGH 08/05/2013

            return dtInformacionAux; // dtErrores;
        }

        public System.Data.DataTable GetErrorWarningGL(int TipoConsulta)
        {
            // 1 Errores, 0 Warnings

            ErrorAdvertenciaDataAccess errores = new ErrorAdvertenciaDataAccess();
            System.Data.DataTable dtErroresWarnings = errores.GetErroresWarningsGL(TipoConsulta);                      
            return dtErroresWarnings; 
        }

        

        private System.Data.DataRow[] SelectDataTable(System.Data.DataTable dt, string strCondicion)
        {
            System.Data.DataRow[] rows;
            //string strFiltro = strNombreColumn +" " + strOperadorRelacional + " '" + strValorBusqueda + "'";
            rows = dt.Select(strCondicion);
            return rows;
        }

        private void AddRowsTable(ref System.Data.DataTable dt, System.Data.DataRow[] rows)
        {
            foreach (System.Data.DataRow dr in rows)
            {
                dt.ImportRow(dr);
            }
        }

        private System.Data.DataTable ValidacionErroresWarnings(System.Data.DataTable dtInformacion, System.Data.DataTable dtRFCs)
        {
            System.Data.DataTable dtResult = new System.Data.DataTable();

            dtResult = dtInformacion.Clone();
            foreach (System.Data.DataRow dr in dtRFCs.Rows)
            {
                System.Data.DataRow[] rows = SelectDataTable(dtInformacion, "rfc = '" + dr["rfc"].ToString() + "'");
                if (rows.Length > 0)
                    if (rows.Length == 1)
                        AddRowsTable(ref dtResult, rows);
                    else
                    {
                        System.Data.DataRow[] rowsAux = SelectDataTable(dtInformacion, "rfc = '" + dr["rfc"].ToString() + "' AND credito > ''");
                        double dCapitalVigente = 0;
                        double dCapitalVencido = 0;
                        double dInteresVigente = 0;
                        double dInteresVencido = 0;
                        double dTotal = 0;
                        if (rowsAux.Length == rows.Length)
                            AddRowsTable(ref dtResult, rows);
                        else if (rowsAux.Length == 0)
                        {
                            for (int iCount = 0; iCount < rows.Length; iCount++)
                            {
                                double dAux = 0;
                                double.TryParse(rows[iCount]["Capital_Vigente"].ToString(), out dAux);
                                dCapitalVigente = dCapitalVigente + dAux;
                                rows[iCount]["Capital_Vigente"] = 0;

                                dAux = 0;
                                double.TryParse(rows[iCount]["Capital_Vencido"].ToString(), out dAux);
                                dCapitalVencido = dCapitalVencido + dAux;
                                rows[iCount]["Capital_Vencido"] = 0;

                                dAux = 0;
                                double.TryParse(rows[iCount]["Interes_Vigente"].ToString(), out dAux);
                                dInteresVigente = dInteresVigente + dAux;
                                rows[iCount]["Interes_Vigente"] = 0;

                                dAux = 0;
                                double.TryParse(rows[iCount]["Interes_Vencido"].ToString(), out dAux);
                                dInteresVencido = dInteresVencido + dAux;
                                rows[iCount]["Interes_Vencido"] = 0;

                                dAux = 0;
                                double.TryParse(rows[iCount]["total"].ToString(), out dAux);
                                dTotal = dTotal + dAux;
                                rows[iCount]["total"] = 0;

                                rows[iCount].AcceptChanges();
                            }
                            rows[0]["Capital_Vigente"] = dCapitalVigente;
                            rows[0]["Capital_Vencido"] = dCapitalVencido;
                            rows[0]["Interes_Vigente"] = dInteresVigente;
                            rows[0]["Interes_Vencido"] = dInteresVencido;
                            rows[0]["total"] = dTotal;
                            AddRowsTable(ref dtResult, rows);
                        }
                        else
                        {
                            int iReg = 0;
                            for (int iCount = 0; iCount < rows.Length; iCount++)
                            {
                                if (rows[iCount]["credito"].ToString().Equals(string.Empty) && iReg == 0)
                                    iReg = iCount;

                                double dAux = 0;
                                double.TryParse(rows[iCount]["Capital_Vigente"].ToString(), out dAux);
                                dCapitalVigente = dCapitalVigente + dAux;
                                rows[iCount]["Capital_Vigente"] = 0;

                                dAux = 0;
                                double.TryParse(rows[iCount]["Capital_Vencido"].ToString(), out dAux);
                                dCapitalVencido = dCapitalVencido + dAux;
                                rows[iCount]["Capital_Vencido"] = 0;

                                dAux = 0;
                                double.TryParse(rows[iCount]["Interes_Vigente"].ToString(), out dAux);
                                dInteresVigente = dInteresVigente + dAux;
                                rows[iCount]["Interes_Vigente"] = 0;

                                dAux = 0;
                                double.TryParse(rows[iCount]["Interes_Vencido"].ToString(), out dAux);
                                dInteresVencido = dInteresVencido + dAux;
                                rows[iCount]["Interes_Vencido"] = 0;

                                dAux = 0;
                                double.TryParse(rows[iCount]["total"].ToString(), out dAux);
                                dTotal = dTotal + dAux;
                                rows[iCount]["total"] = 0;

                                rows[iCount].AcceptChanges();
                            }
                            rows[iReg]["Capital_Vigente"] = dCapitalVigente;
                            rows[iReg]["Capital_Vencido"] = dCapitalVencido;
                            rows[iReg]["Interes_Vigente"] = dInteresVigente;
                            rows[iReg]["Interes_Vencido"] = dInteresVencido;
                            rows[iReg]["total"] = dTotal;
                            AddRowsTable(ref dtResult, rows);
                        }
                    }
            }

            return dtResult;
        }
        //SICREB-FIN-VHCC SEP-2012

        public void ClearErrors()
        {
            ErrorAdvertenciaDataAccess errores = new ErrorAdvertenciaDataAccess();
            errores.ClearErrorAdv();
        }
        
    }
}
