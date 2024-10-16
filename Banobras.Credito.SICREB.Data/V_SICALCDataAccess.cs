using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

//using testConectionOracle;

namespace Banobras.Credito.SICREB.Data
{

    public class V_SICALCDataAccess : OracleBase
    {

        private List<SICALCItem> allRecords = null;

        public List<SICALCItem> GetRegistros()
        {
            if (allRecords == null)
            {
                allRecords = new List<SICALCItem>();
                String query = "DATOSEXTERNOS.SP_ObtenerSICALC";

                //MASS. 04octubre2021 para pruebas en desarrollo
                if (WebConfig.MailFrom.StartsWith("desarrollo"))
                {
                    query = "DATOSEXTERNOS_Z.SP_ObtenerSICALC";
                }

                DbCommand cmd = DB.GetStoredProcCommand(query);
                //  DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, persona);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        SICALCItem item = new SICALCItem();
                        item.Credito = Parser.ToNumber(reader["CREDITO"]);
                        item.Calificacion = reader["CALIFICACION"].ToString();
                        
                        allRecords.Add(item);
                    }
                }
            }

            return allRecords;
        }

        public SICALCItem GetCalificacion(int credito)
        {
            var calif = (from cal in GetRegistros()
                         where cal.Credito.Equals(credito)
                         select cal).FirstOrDefault();

            return calif;
        }

        public String GetCalificacionAltoRiesgoAccess(string RFC_RIESGO)
        {

            string calificacionRFC = "";
            String query = "SP_getRFCCALIFICACION";

            DbParameter[] parametros = new DbParameter[2];
            parametros[0] = OracleBase.DB.DbProviderFactory.CreateParameter();
            parametros[0].ParameterName = "clv_buro_riesgo";
            parametros[0].DbType = System.Data.DbType.String;
            parametros[0].Direction = System.Data.ParameterDirection.Output;
            parametros[0].Size = 12;

            parametros[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
            parametros[1].ParameterName = "str_rfc";
            parametros[1].DbType = System.Data.DbType.String;
            parametros[1].Value = RFC_RIESGO;
            parametros[1].Direction = System.Data.ParameterDirection.Input;
            parametros[1].Size = 12;

            DbCommand cmd = DB.GetStoredProcCommand(query);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            cmd.Connection = OracleBase.DB.CreateConnection();
            
            cmd.Parameters.Add(parametros[0]);
            cmd.Parameters.Add(parametros[1]);
          
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            
            calificacionRFC = parametros[0].Value.ToString();
            cmd.Connection.Close();
            cmd.Dispose();
    
            return calificacionRFC;
        }

    }

}
