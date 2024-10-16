using Banobras.Credito.SICREB.Data.Helpers;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Banobras.Credito.SICREB.Data
{
    public abstract class OracleBase
    {
        //private static readonly Database db = DatabaseFactory.CreateDatabase("SICREB");
        //private static readonly Database db_intran = DatabaseFactory.CreateDatabase("INTRAN");

        //public static Database DB_OLD
        //{
        //    get { return db; }
        //}

        //public static Database DB_Intran_OLD
        //{
        //    get { return db_intran; }
        //}

 
        /// <summary>
        /// Obtener instancia de la BD de SICREB
        /// </summary>
        public static Database DB
        {            
            get 
            {
                string connectionString = OracleConnectionHelper.GetConnectionString("SICREB", "SICREBPR");
                OracleDatabase oraDB = new OracleDatabase(connectionString);

                return oraDB; 
            }
        }

        /// <summary>
        /// Obtener la instancia de la BD de INTRAN
        /// </summary>
        public static Database DB_Intran
        {
            get
            {
                string connectionString = OracleConnectionHelper.GetConnectionString("INTRAN", "DINTRAN");
                OracleDatabase oraDB = new OracleDatabase(connectionString);

                return oraDB;
            }
        }


        protected bool HandleException(System.Exception exceptionToHandle)
        {
            return ExceptionPolicy.HandleException(exceptionToHandle, "DataLayer");
        }
                
    }

}
