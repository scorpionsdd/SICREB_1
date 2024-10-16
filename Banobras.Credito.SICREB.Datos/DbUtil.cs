using System;
using Oracle.DataAccess.Client;
using Banobras.Credito.SICREB.Data.Helpers;

namespace SHCP.SIGAIF.Datos
{

    public sealed class DbUtil
    {

        private static string dbConnectionString = "SICREB";
       
        public static void SetConnectionString(string connectionString)
        {
            dbConnectionString = connectionString;
        }

        public static OracleConnection GetConnection(string connectionStringName)
        {
            //return new OracleConnection(connectionStringName);
            return GetDataBaseConnection(connectionStringName);
        }

        private static string Decrypted(string EncryptedData)
        {
            string DataLeap = String.Empty;
            string DataDecrypted = String.Empty;
            Int16 Leap = 2;

            while (EncryptedData.Length > 0)
            {
                DataLeap = System.Convert.ToChar(System.Convert.ToUInt32(EncryptedData.Substring(0, Leap), 16)).ToString();
                DataDecrypted = DataDecrypted + DataLeap;
                EncryptedData = EncryptedData.Substring(Leap, EncryptedData.Length - Leap);
            }

            return DataDecrypted;
        }

        /// <summary>
        /// Obtener conexión hacia la BD con 
        /// </summary>
        /// <param name="connectionStringName">Nombre de la cadena de conexión</param>
        /// <returns></returns>
        private static OracleConnection GetDataBaseConnection(string connectionStringName)
        {
            OracleConnection connection = OracleConnectionHelper.GetConnection("SICREB", "SICREBPR");
            return connection;
        }

    }
    
}
