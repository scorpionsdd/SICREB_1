using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace testConectionOracle
{
    public abstract class OracleBase2012
    {

        private static readonly Database db = DatabaseFactory.CreateDatabase("SICREB");
     

        public static Microsoft.Practices.EnterpriseLibrary.Data.Database DB
        {
            get
            {
                return db;
            }
        }

        protected bool HandleException(System.Exception exceptionToHandle)
        {
            return ExceptionPolicy.HandleException(exceptionToHandle, "DataLayer");
        }
    }
}