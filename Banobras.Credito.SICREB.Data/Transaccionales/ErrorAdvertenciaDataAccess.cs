using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

using Oracle.DataAccess.Client;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{

    public class ErrorAdvertenciaDataAccess : OracleBase
    {

        public bool AddErrorAdvertencia(ErrorWarning error, out string mensaje)
        {
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_AddErrorAdv");

                DB.AddInParameter(cmd, "pValidacionId", System.Data.DbType.Int32, error.ValidacionId);
                DB.AddInParameter(cmd, "pRfc", System.Data.DbType.AnsiString, error.Rfc);
                DB.AddInParameter(cmd, "pCredito", System.Data.DbType.AnsiString, error.Credito);
                DB.AddInParameter(cmd, "pDato", System.Data.DbType.AnsiString, error.Dato);

                DB.ExecuteNonQuery(cmd);
                mensaje = string.Empty;

                //JAGH
                cmd.Connection.Close();
                cmd.Connection.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool AddErrorAdvertencia(ErrorWarning error)
        {
            string msg;
            return AddErrorAdvertencia(error, out msg);
        }

        public List<ErrorWarning> GetRegistros(Enums.Persona persona)
        {
            List<ErrorWarning> toReturn = new List<ErrorWarning>();

            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetErrorAdv");
            DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (persona == Enums.Persona.Moral ? "M" : "F"));

            using (IDataReader reader = DB.ExecuteReader(cmd))
            {
                //recorres los resultados
                while (reader.Read())
                {
                    int id = Parser.ToNumber(reader["ID"]);
                    int validacionId = Parser.ToNumber(reader["ID_VALIDACIONES"]);
                    string rfc = reader["RFC"].ToString();
                    string credito = reader["CREDITO"].ToString();
                    string dato = reader["DATO"].ToString();
                    string usuario = reader["usuario_alta"].ToString();

                    toReturn.Add(new ErrorWarning(id, validacionId, rfc, credito, dato, usuario));
                }
            }
            return toReturn;
        }

        //SICREB-INICIO-VHCC SEP-2012
        public System.Data.DataTable GetRegistros(Enums.Persona persona, int iErr_Adver) 
        {
            System.Data.DataTable dtErrores = new System.Data.DataTable();

            DbCommand cmd = DB.GetStoredProcCommand("sp_trans_geterror_adv");
            DB.AddInParameter(cmd, "TipoErrorAdvert", DbType.Int32, iErr_Adver);
            DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (persona == Enums.Persona.Moral ? "M" : "F"));

            using (IDataReader reader = DB.ExecuteReader(cmd)) 
            {
                dtErrores.Load(reader);
            }

            return dtErrores;
        }

        //JAGH se agregan grupos seleccionados  08/01/13
        public System.Data.DataTable GetRegistros(Enums.Persona persona, int iErr_Adver, string strStored,  string strGrupos)
        {
            System.Data.DataTable dtErrores = new System.Data.DataTable();

            DbCommand cmd = DB.GetStoredProcCommand(strStored);
            DB.AddInParameter(cmd, "TipoErrorAdvert", DbType.Int32, iErr_Adver);
            DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (persona == Enums.Persona.Moral ? "M" : "F"));

            //verificas si existen grupos para enviar parametros
            if (!strGrupos.Equals(string.Empty))
            {
                string strSQL = string.Empty;
                string[] arrayGrupos = strGrupos.Split(',');

                //checas cuantos grupos se envian
                if (arrayGrupos.Length == 1)
                    strSQL = " AND cue.grupo  = " + arrayGrupos[0].ToString() + " )";
                else
                {
                    //si hay mas de un grupo creas dinamicamente el filtro de consulta
                    for (int i = 0; i < arrayGrupos.Length; i++)
                    {
                        if (i == 0)
                            strSQL = " AND (cue.grupo = " + arrayGrupos[i].ToString();
                        else
                            strSQL += "  OR cue.grupo = " + arrayGrupos[i].ToString();
                    }

                    strSQL += "))  ";
                }

                DB.AddInParameter(cmd, "pGrupos", DbType.String, strSQL);
            }

            using (IDataReader reader = DB.ExecuteReader(cmd))
            {
                dtErrores.Load(reader);
            }

            return dtErrores;
        }
        //SICREB-FIN-VHCC SEP-2012

        public void ClearErrorAdv()
        {
            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_ClearErrorAdv");
            DB.ExecuteNonQuery(cmd);
        }

        public System.Data.DataTable GetErroresWarningsGL(int TipoConsulta)
        {
            System.Data.DataTable dtErroresWarnings = new System.Data.DataTable();

            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "PKG_GPOS_LCCYR.SP_GET_ERRORES_WARNINGS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("pCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("pTipoConsulta", OracleDbType.Varchar2, TipoConsulta.ToString(), ParameterDirection.Input);

                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            dtErroresWarnings.Load(reader);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Consultar Errores y Warnings de GPOs y LCCyR", ex);
            }
            
            return dtErroresWarnings;
        }

        //PSL 26112021
        public bool ValidCuentasIFRS9()
        {
            string mensaje = string.Empty;
            try {
                DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES_z.SP_validAccountsIFRS9");
                DB.ExecuteNonQuery(cmd);
                return true;
            }catch(Exception ex){
                mensaje = ex.Message;
                return false;
            }
            
        }


    }

}
