using System;
using System.Data;
using System.Data.OracleClient;
using Banobras.Credito.SICREB.Common;

namespace Banobras.Credito.SICREB.Data.Guias_Contables
{

    public class Sicofin2DataAccess : OracleBase
    {

        public void GeneraSICOFIN2()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();

                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.CommandText = "PKG_GUIAS_CONTABLES.SP_SET_V_SICOFIN";

                         if (WebConfig.MailFrom.StartsWith("desarrollo"))
                         {
                             cmd.CommandText = "PKG_GUIAS_CONTABLES_Z.SP_SET_V_SICOFIN";
                         }
                         cmd.ExecuteNonQuery();                         
                     }
                }                              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public void GeneraSICOFIN2Semanal()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.CommandText = "PKG_GUIAS_CONTABLES.SP_SET_V_SICOFIN_SEMANAL";

                        //MASS. 04octubre2021 para pruebas en ambiente de desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            cmd.CommandText = "PKG_GUIAS_CONTABLES_Z.SP_SET_V_SICOFIN_SEMANAL";
                        }                        

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GeneraSICOFIN2Mensual()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.CommandText = "PKG_GUIAS_CONTABLES.SP_SET_V_SICOFIN_MENSUAL";

                        //MASS. 04octubre2021 para pruebas en ambiente de desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            cmd.CommandText = "PKG_GUIAS_CONTABLES_Z.SP_SET_V_SICOFIN_MENSUAL";
                        }                        

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}
