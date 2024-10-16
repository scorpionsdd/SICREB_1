using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;

using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Banobras.Credito.SICREB.Data.Reportes
{

    class ResumenEjecutivoDataAccess : OracleBase
    {

        private const string SP_Get_PF = "SICREB.SP_GET_RESUMEN_EJECUTIVO_PF";
        private const string SP_Get_PM = "SICREB.SP_GET_RESUMEN_EJECUTIVO_PM";

        public static DataSet Get_PF()
        {
            DataSet ds;
            DbCommand cmd = DB.GetStoredProcCommand(ResumenEjecutivoDataAccess.SP_Get_PF);
            ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet Get_PM()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ResumenEjecutivoDataAccess.SP_Get_PM);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

    }

}
