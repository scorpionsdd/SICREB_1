using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;

/*
 * JAGH 04/03/13
 * Permite obtener fechas de procesos mensual y semanal desde BD.
 */

namespace Banobras.Credito.SICREB.Data
{
    public  class Obtener_Fecha_Cintas : OracleBase
    {

        #region Proceso Semanal

            //obtiene fecha para cinta semanal
            public static string fecha_Cinta_Semanal()
            {
                string strFechaReturn = string.Empty;

                try
                {
                    string query = "SELECT FN_GETFECHA_RPTSEMANAL() FROM DUAL";
                    DbCommand cmd = DB.GetSqlStringCommand(query);
                    DateTime dtFecha = Convert.ToDateTime(DB.ExecuteScalar(cmd));
                    strFechaReturn = dtFecha.ToString(Util.FORMATO_FECHA);  
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al solicitar fecha proceso cinta PF_Semanal", ex);
                }

                return strFechaReturn;
            }

            //obtiene periodo para cinta semanal
            public static string periodo_Cinta_Semanal()
            {
                string strFechaReturn = string.Empty;

                try
                {
                    string query = "SELECT FN_GETPERIODO_RPTSEMANAL() FROM DUAL";
                    DbCommand cmd = DB.GetSqlStringCommand(query);
                    strFechaReturn = (string)DB.ExecuteScalar(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al solicitar periodo proceso cinta PF_Semanal", ex);
                }

                return strFechaReturn;
            }

        #endregion

        #region Proceso Mensual

            //obtiene fecha para cinta mensual
            public static string fecha_Cinta_Mensual()
            {
                string strFechaReturn = string.Empty;

                try
                {
                    string query = "SELECT FN_GETFECHA_RPTMENSUAL() FROM DUAL";
                    DbCommand cmd = DB.GetSqlStringCommand(query);
                    DateTime dtFecha = Convert.ToDateTime(DB.ExecuteScalar(cmd));
                    strFechaReturn = dtFecha.ToString(Util.FORMATO_FECHA);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al solicitar fecha proceso cinta PF_Mensual", ex);
                }

                return strFechaReturn;
            }
        
            //obtiene periodo para cinta mensual
            public static string periodo_Cinta_Mensual()
            {
                string strFechaReturn = string.Empty;

                try
                {
                    string query = "SELECT FN_GETPERIODO_RPTMENSUAL() FROM DUAL";
                    DbCommand cmd = DB.GetSqlStringCommand(query);
                    strFechaReturn = (string) DB.ExecuteScalar(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al solicitar periodo proceso cinta PF_Mensual", ex);
                }

                return strFechaReturn;
            }
        
        #endregion
    
    }

}
