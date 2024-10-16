using System;
using System.Text;
using System.Data;
using System.Collections;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{
    public class ExecSystemAlerts: OracleBase
    {
        //variables a emplear
        DateTime dFechaEvaluar;
        Hashtable htConfig;
        int iTipoConfig = 0; //indica que tipo de configuracion empleara
        System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo("es-MX", true);

        #region  Constructores

            public ExecSystemAlerts()
            {    }
        
            public ExecSystemAlerts(DateTime dtParam)
            {
                this.dFechaEvaluar = dtParam;
                this.iTipoConfig = 0; 
            }

            public ExecSystemAlerts(DateTime dtParam, Hashtable phtConfig)
            {
                this.dFechaEvaluar = dtParam;
                this.htConfig = phtConfig;
                this.iTipoConfig = 1; 
            }

        #endregion

        //ejecutara alertas de sistema
        public string ejecutaSystemAlerts()
        {
            //valida dia habil
            DateTime dtFechaQuery = esDiaHabil(this.dFechaEvaluar);            
            string strFechaQuery = dtFechaQuery.Day.ToString();
            string strDiaQuery = Culture.DateTimeFormat.GetDayName(dtFechaQuery.DayOfWeek).ToString();
            
            //datos a insertar si devuelve dia inhabil
            DateTime dtFechaInsert = dtFechaQuery;
            string strFechaInsert = dtFechaInsert.Day.ToString();
            string strDiaInsert = Culture.DateTimeFormat.GetDayName(dtFechaInsert.DayOfWeek).ToString();

            //verifica si fechas son diferentes para insertar en pendientes
            int iEsPendiente = 0;
            if (!dtFechaQuery.ToString("dd/MM/yyyy").Equals(this.dFechaEvaluar.ToString("dd/MM/yyyy")))
            {
                iEsPendiente = 1;
                dtFechaQuery = this.dFechaEvaluar;
                strFechaQuery = dtFechaQuery.Day.ToString();
                strDiaQuery = Culture.DateTimeFormat.GetDayName(dtFechaQuery.DayOfWeek).ToString();
            }
                                    
            //ejecuta store para obtener alertas
            DataTable dtInfo = obtieneInfoAlertas(strFechaQuery, strDiaQuery, strFechaInsert, strDiaInsert, iEsPendiente);
            
            //envias mail de alerta
            return sendMailAlerts(dtInfo);
        }
                
        //valida si fecha es dia habil
        private DateTime esDiaHabil(DateTime dtFecha)
        {
            ValidaFechaHabil esFechaHabil = new ValidaFechaHabil();
            DateTime dt = esFechaHabil.ValidaFecha(dtFecha, ValidaFechaHabil.Dir.Adelante);            
            return dt;
        }

        //regresa cursor con datos de alertas
        private DataTable obtieneInfoAlertas(string strFechaQuery, string strDiaQuery, string strFechaInsert, string strDiaInsert, int iEsPendiente)
        {
            //defines objeto a regresar
            System.Data.DataTable dt = new System.Data.DataTable();

            try
            {              
                //indicas store a ejecutar
                string query = string.Format("SP_SEND_ALERTS");
                
                //creas comando para ejecutar store
                System.Data.Common.DbCommand cmd1 = DB.GetStoredProcCommand(query);
                DB.AddInParameter(cmd1, "p_strFechaQuery", System.Data.DbType.String, strFechaQuery);
                DB.AddInParameter(cmd1, "p_strDiaQuery", System.Data.DbType.String, strDiaQuery);
                DB.AddInParameter(cmd1, "p_strFechaInsert", System.Data.DbType.String, strFechaInsert);
                DB.AddInParameter(cmd1, "p_strDiaInsert", System.Data.DbType.String, strDiaInsert);
                DB.AddInParameter(cmd1, "p_iEsPendiente", System.Data.DbType.Int32, iEsPendiente);
            
                //obtienes datos
                using (System.Data.IDataReader reader = DB.ExecuteReader(cmd1))
                {  
                    //llenas tabla
                    dt = new System.Data.DataTable();
                    dt.Load(reader);
                    reader.Close();
                }
                cmd1.Connection.Close();
                return dt;            
            }
            catch (Exception e) 
            { 
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        //obtienes info y envias mail
        private string sendMailAlerts(DataTable dtInfo)
        {            
            string strCadena = string.Empty;
            string strMsg = string.Empty;

            if (dtInfo.Rows.Count == 0)
                return strMsg = "No hay alertas por enviar";
            
            //recorres tabla 
            foreach (DataRow dr in dtInfo.Rows)
            {
                if (dr[0].ToString().ToUpper().Contains("AVISO"))
                {
                    strCadena = dr[0].ToString() + string.Empty+ dr[1].ToString();
                   
                    //mandas un mensaje para indicar que se reprogramo el envio
                    return strMsg = strCadena;
                }
                else
                {
                    // campos que regresa dt(id, destinatario, identificadoralerta, mensajealerta, tituloalerta, periodicidad, fechaaplicacionperiodo, tipo)
                    //ejecutas proceso para envio de alerta
                    try
                    {
                        // verificas de donde obtienes datos de configuracion para envio de correo
                        if(iTipoConfig == 0)
                            EnvioMail.EnviaAlerta(dr["destinatario"].ToString(), dr["mensajealerta"].ToString(), dr["tituloalerta"].ToString()); // desde web.config
                        else
                            EnvioMail.EnviaServicioAlerta(dr["destinatario"].ToString(), dr["mensajealerta"].ToString(), dr["tituloalerta"].ToString(), this.htConfig); // desde servicio
                        
                        //borras datos de tabla pendientes si es el caso
                        if(dr[7].ToString().Equals("pendientes"))
                            borraDato_AlertaPendiente(Convert.ToInt32(dr["id"].ToString()), dr["identificadoralerta"].ToString(), dr["fechaaplicacionperiodo"].ToString(), dr["periodicidad"].ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return strMsg = "Error al enviar alertas: " + e.ToString();
                    }                       
                }
            }

            strMsg = "Alertas enviadas correctamente";
            return strMsg;
        }
        
        //borra registro de t_alerta_pendientes una vez que se ha enviado el correo
        private void borraDato_AlertaPendiente(int id, string identificadoralerta, string fechaaplicacionperiodo, string periodicidad)
        {
            string strQuery = "delete from t_alerta_pendientes";
            strQuery += " where id = " + id;
            strQuery += " and identificadoralerta= '" + identificadoralerta + "' ";
            strQuery += " and fechaaplicacionperiodo= '" + fechaaplicacionperiodo + "' ";
            strQuery += " and periodicidad= '" + periodicidad + "' ";

            System.Data.Common.DbCommand cmd1 = DB.GetSqlStringCommand(strQuery);
            //System.Data.Common.DbTransaction tran;
            int iD = DB.ExecuteNonQuery(cmd1);
            cmd1.Connection.Close();
        }

        //regresas como string un BLOB de oracle
        private  string regresaMensaje(DataRow drMsg)
        {
            string strReturn = string.Empty;           
            try
            {
                Byte[] byteBLOBData = new Byte[0];
                byteBLOBData = (Byte[])(drMsg["mensajealerta"]);
                strReturn = System.Text.Encoding.UTF8.GetString(byteBLOBData);
            }
            catch (Exception ex) { return ex.ToString();}
            return strReturn;
        }

    } 

}
