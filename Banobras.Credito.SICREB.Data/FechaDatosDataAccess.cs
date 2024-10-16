using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Common;

namespace Banobras.Credito.SICREB.Data
{

    public class FechaDatosDataAccess : OracleBase
    {

        public List<DateTime> FechasVistas()
        {
            DbCommand command = DB.GetStoredProcCommand("DatosExternos.SP_Fechas_Vistas");

            //MASS. 29septiembre2021 para pruebas en ambiente de desarrollo
            if (WebConfig.MailFrom.StartsWith("desarrollo"))
            {
                command = DB.GetStoredProcCommand("DATOSEXTERNOS_Z.SP_Fechas_Vistas");
            }            

            //TODO: revisar SP_fechas_vistas
            List<DateTime> Fechas = new List<DateTime>();
            try
            {
                using (IDataReader reader = DB.ExecuteReader(command))
                {
                    while (reader.Read())
                        Fechas.Add(Parser.ToDateTime(reader["fecha"].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la busqueda de fechas", ex);
            }

            return Fechas;
        }

    }

}
