using System;
using System.Data;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{

    public class InconsistenciasDataAccess : OracleBase
    {

        private Enums.Persona persona;
        private int iArchivoId;
        private DateTime fdFecha;

        public InconsistenciasDataAccess()
        {
        }

        public InconsistenciasDataAccess(Enums.Persona persona, int archivoId, DateTime fecha)
        {
            this.persona = persona;
            this.iArchivoId = archivoId;
            this.fdFecha = fecha;
        }

        public void LlenarInconsistencias()
        {
            string query = string.Format("SP_INCONSISTENCIAS");
            System.Data.Common.DbCommand cmd = DB.GetStoredProcCommand(query);
            DB.ExecuteNonQuery(cmd);
        }

        public DataTable GetInconsistencias()
        {
            DataSet ds = new DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            string query = String.Empty;
            if (this.persona == Enums.Persona.Moral)
                query = string.Format("SP_INCONSISTENCIAS_REPORTE_PM");
            else
                query = string.Format("SP_INCONSISTENCIAS_REPORTE_PF");

            System.Data.Common.DbCommand cmd = DB.GetStoredProcCommand(query);
            ds = DB.ExecuteDataSet(cmd);
            cmd.Dispose();

            return ds.Tables[0];
        }

    }

}
