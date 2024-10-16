using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{

    public class ValoresArchivoDataAccess : OracleBase
    {

        public bool AddValorArchivo(ValorArchivo valor, out string mensaje)
        {

            mensaje = string.Empty;
            if (!String.IsNullOrWhiteSpace(valor.Valor))
            {
                try
                {
                    DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_AddValoresArchivo");
                    
                    DB.AddInParameter(cmd, "pArchivoId", System.Data.DbType.Int32, valor.ArchivoId);
                    DB.AddInParameter(cmd, "pEtiquetaID", System.Data.DbType.Int32, valor.EtiquetaId);
                    DB.AddInParameter(cmd, "pValor", System.Data.DbType.AnsiString, valor.Valor);
                    DB.ExecuteNonQuery(cmd);

                    return true;
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    return false;
                }
            }

            return true;
        }

        public bool AddValorArchivo(ValorArchivoCollection valor, out string mensaje)
        {
            mensaje = string.Empty;
            if ((valor.Count > 0))
            {
                try
                {
                    using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                    {

                        using (OracleCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "TRANSACCIONALES.SP_TRANS_AddValoresArchivo";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ArrayBindCount = valor.Count;

                            cmd.Parameters.Add("pArchivoId", OracleDbType.Int32).Value = valor.Select(v => v.ArchivoId).ToArray();
                            cmd.Parameters.Add("pEtiquetaID", OracleDbType.Int32).Value = valor.Select(v => v.EtiquetaId).ToArray();
                            cmd.Parameters.Add("pValor", OracleDbType.Varchar2).Value = valor.Select(v => v.Valor).ToArray();

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            return true;
        }

        public bool AddValorArchivo(ValorArchivo valor)
        {
            string msg;
            return AddValorArchivo(valor, out msg);
        }

        public List<ValorArchivo> GetValoresArchivo(Enums.Persona persona)
        {
            List<ValorArchivo> valores = new List<ValorArchivo>();

            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetValoresArchivo");
            DB.AddInParameter(cmd, "pPersona", System.Data.DbType.AnsiString, (persona == Enums.Persona.Moral ? 'M' : 'F'));

            using (IDataReader reader = DB.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    int id = Parser.ToNumber(reader["ID"]);
                    int id_Archivo = Parser.ToNumber(reader["ID_ARCHIVO"]);
                    int id_Etiqueta = Parser.ToNumber(reader["ID_ETIQUETA"]);
                    string valor = reader["VALOR"].ToString();

                    ValorArchivo val = new ValorArchivo(id, id_Archivo, id_Etiqueta, valor);
                    valores.Add(val);
                }
            }

            return valores;
        }   
        
    }

}
