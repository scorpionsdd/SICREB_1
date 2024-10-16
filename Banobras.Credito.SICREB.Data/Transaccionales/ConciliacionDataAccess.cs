using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{

    public class ConciliacionDataAccess : OracleBase
    {

        public void AddConciliacion(Conciliacion conciliacion)
        {

            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_AddConciliacion");
            DB.AddInParameter(cmd, "pCredito", System.Data.DbType.AnsiString, conciliacion.Credito);
            DB.AddInParameter(cmd, "pSaldoVigenteOrig", System.Data.DbType.Double, conciliacion.SaldoVigenteOriginal);
            DB.AddInParameter(cmd, "pSaldoVigente", System.Data.DbType.Int32, conciliacion.SaldoVigente);
            DB.AddInParameter(cmd, "pSaldoVencidoOrig", System.Data.DbType.Double, conciliacion.SaldoVencidoOriginal);
            DB.AddInParameter(cmd, "pSaldoVencido", System.Data.DbType.AnsiString, conciliacion.SaldoVencido);
            DB.AddInParameter(cmd, "pArchivoId", System.Data.DbType.Int32, conciliacion.IdArchivo);
            DB.AddInParameter(cmd, "pAuxiliar", System.Data.DbType.AnsiString, conciliacion.Auxiliar);
            DB.AddInParameter(cmd, "pRfc", System.Data.DbType.AnsiString, conciliacion.Rfc);
            
            DB.ExecuteNonQuery(cmd);
        }

        public List<ConciliacionInfo> GetConcliacionInfo(Enums.Persona persona, int archivoId)
        {
            List<ConciliacionInfo> conciliaciones = new List<ConciliacionInfo>();
            try
            {
                string query = string.Format("TRANSACCIONALES.SP_TRANS_GetConciliacionInfo{0}", (persona == Enums.Persona.Moral ? "PM" : "PF"));

                DbCommand cmd = DB.GetStoredProcCommand(query);
                DB.AddInParameter(cmd, "pArchivoId", DbType.Int32, archivoId);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        Conciliacion con = new Conciliacion();
                        con.Id = Parser.ToNumber(reader["ID"]);
                        con.Credito = reader["CREDITO"].ToString();
                        con.SaldoVencido = Parser.ToNumber(reader["SALDO_VENCIDO"]);
                        con.SaldoVencidoOriginal = Parser.ToDouble(reader["SALDO_VENCIDO_ORIGINAL"]);
                        con.SaldoVigente = Parser.ToNumber(reader["SALDO_VIGENTE"]);
                        con.SaldoVigenteOriginal = Parser.ToDouble(reader["SALDO_VIGENTE_ORIGINAL"]);
                        con.Rfc = reader["RFC"].ToString();
                        con.Auxiliar = reader["AUXILIAR"].ToString();

                        string delegacion = reader["MUNICIPIO"].ToString();
                        string acreditado = reader["ACREDITADO"].ToString();

                        conciliaciones.Add(new ConciliacionInfo(delegacion, "1", acreditado, con));
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar conciliación", ex);
            }

            return conciliaciones;
        }

    }

}
