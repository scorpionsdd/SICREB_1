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

    public class ReporteConciliacionesDataAccess : OracleBase
    {
        private const string SP_Get_PF = "SICREB.SP_GET_CONCILIACION_PF";        
        private const string SP_Get_PM = "SICREB.SP_GET_CONCILIACION_PM";
        private const string SP_GetResumen_PF = "SICREB.SP_GET_CONCILIACION_RESUMEN_PF";
        private const string SP_GetResumen_PM = "SICREB.SP_GET_CONCILIACION_RESUMEN_PM";

        private const string SP_Get_PFTest = "SICREB.SP_GET_CONCIL_PF"; 
        private const string SP_Get_PMTest = "SICREB.SP_GET_CONCIL_PM";
        private const string SP_Get_PFActualizado = "PKG_CONCILIACION.SP_GET_REPORTE_PF";
        private const string SP_Get_PMActualizado = "PKG_CONCILIACION.SP_GET_REPORTE_PM";
        private const string SP_GetResumen_PFTest = "SICREB.SP_GET_CONCIL_RESUMEN_PF";
        private const string SP_GetResumen_PMTest = "SICREB.SP_GET_CONCIL_RESUMEN_PM";
                      
        private const string SP_Get_ConciliacionGL = "PKG_GPOS_LCCYR.SP_GET_CONCILIACION_GL";
        private const string SP_Get_ResumenGL = "PKG_GPOS_LCCYR.SP_GET_RESUMEN_GL";

        private const string SP_Get_HistorialGL = "PKG_GPOS_LCCYR.SP_GET_HISTORIAL_GL";        
     

        public static DataSet Get_PF()
        {
            DataSet ds;
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_PF);
            ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet Get_PM()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_PM);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet GetResumen_PF()
        {
            DataSet ds;
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_GetResumen_PF);
            ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet GetResumen_PM()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_GetResumen_PM);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }


        public static DataSet Get_PFTest()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_PFTest);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet Get_PFActualizado()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_PFActualizado);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet Get_PMTest()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_PMTest);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet Get_PMActualizado()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_PMActualizado);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet GetResumen_PFTest()
        {
            DataSet ds;
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_GetResumen_PFTest);
            ds = DB.ExecuteDataSet(cmd);
            return ds;
        }
        
        public static DataSet GetResumen_PMTest()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_GetResumen_PMTest);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }


        public static DataSet Get_ConciliacionGL()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_ConciliacionGL);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet Get_ResumenGL()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_ResumenGL);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }


        public static DataSet Get_HistorialGL()
        {
            DbCommand cmd = DB.GetStoredProcCommand(ReporteConciliacionesDataAccess.SP_Get_HistorialGL);
            DataSet ds = DB.ExecuteDataSet(cmd);
            return ds;
        }

    }

}
