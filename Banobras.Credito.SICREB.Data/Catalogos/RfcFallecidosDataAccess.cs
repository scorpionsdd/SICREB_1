using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class RfcFallecidosDataAccess : OracleBase
    {

        private List<RfcFallecido> allCatalogos = null;

        public List<RfcFallecido> GetRegistros()
        {
            try
            {
                if (allCatalogos == null)
                {
                    allCatalogos = new List<RfcFallecido>();

                    DbCommand cmd = DB.GetStoredProcCommand("CATALOGOS.SP_CATALOGOS_GetRfcFallecidos");

                    using (IDataReader reader = DB.ExecuteReader(cmd))
                    {

                        while (reader.Read())
                        {
                            //los campos que siempre estan presentes
                            int id = Parser.ToNumber(reader["ID"]);
                            string rfc = reader["rfc"].ToString();
                            //SICREB-INICIO ACA Sep-2012
                            //string nombre = reader["nombre"].ToString();
                            //int credito = Parser.ToNumber(reader["credito"]);
                            //string auxiliar = reader["auxiliar"].ToString();
                            //SICREB-FIN ACA Sep-2012
                            DateTime fecha = Parser.ToDateTime(reader["Fecha"]);

                            allCatalogos.Add(new RfcFallecido(id, rfc, fecha));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar los datos en catalgo Rfc de Personas Fallecidas", ex); 
            }
            return allCatalogos;
        }

        public RfcFallecido GetItem(string rfc)
        {
            var toReturn = GetRegistros()
                            .Where(r => r.Rfc.Equals(rfc, StringComparison.InvariantCultureIgnoreCase))
                            .FirstOrDefault();
            return toReturn;
        }

    }

}
