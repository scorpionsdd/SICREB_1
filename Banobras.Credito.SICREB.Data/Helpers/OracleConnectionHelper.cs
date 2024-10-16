using Banobras.Credito.SICREB.Entities;
using Newtonsoft.Json;
using System;
using System.Configuration;
using Oracle.DataAccess.Client;

namespace Banobras.Credito.SICREB.Data.Helpers
{

    /// <summary>
    /// Clase que permite obtener la cadena de conexión o una conexión a BD
    /// </summary>
    public static class OracleConnectionHelper
    {

        /// <summary>
        /// Obtener la cadena de conexión de una BD
        /// </summary>
        /// <param name="connectionStringName">Nombre de la cadena de conexión</param>
        /// <param name="dataBaseName">Nombre de la BD</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionStringName, string dataBaseName)
        {
            string valueConnectionString = string.Empty;

            //Nombre de la variable de ambiente definida en el SO anfitrión
            string environmentVariable = ConfigurationManager.AppSettings["EnvironmentVariable"].ToString();
            //Obteniendo el contenido de la variable de ambiente: par | valor
            string environmentVariableValue = Environment.GetEnvironmentVariable(environmentVariable);
            if (!string.IsNullOrEmpty(environmentVariableValue))
            {
                //Convirtiendo el contenido en una entidad
                var data = JsonConvert.DeserializeObject<EnvironmentVariables>(environmentVariableValue);
                //Obteniendo la cadena de conexión para concatenar los valores de la variable de ambiente
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
                //Apuntando a la BD objetivo
                var valueSICREBDB = data.Databases.Find(x => x.Scheme == dataBaseName);
                //Formateando la cadena de conexión resultante
                valueConnectionString = string.Format(connectionString, valueSICREBDB.Host, valueSICREBDB.Port, valueSICREBDB.Scheme, valueSICREBDB.UserId, valueSICREBDB.Password);
            }

            return valueConnectionString;
        }

        /// <summary>
        /// Obtener una conexión con una BD
        /// </summary>
        /// <param name="connectionStringName">Nombre de la cadena de conexión</param>
        /// <param name="dataBaseName">Nombre de la BD</param>
        /// <returns></returns>
        public static OracleConnection GetConnection(string connectionStringName, string dataBaseName)
        {
            string valueConnectionString = GetConnectionString(connectionStringName, dataBaseName);
            if (!string.IsNullOrEmpty(valueConnectionString))
            {
                var connection = new OracleConnection(valueConnectionString);

                return connection;
            }

            return new OracleConnection();
        }

    }

}
