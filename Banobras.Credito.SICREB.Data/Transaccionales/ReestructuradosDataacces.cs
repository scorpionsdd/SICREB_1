using System;
using System.Data;
using System.Data.Common;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{
    public class ReestructuradoDataAccess : OracleBase
    {
        public DataTable GetReestructuradoInfo()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "TRANSACCIONALES.SP_TRANS_GetReestructurados";
                DbCommand cmd = DB.GetStoredProcCommand(query);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar Reestructurados", ex);
            }
            return dt;
        }

    }

}
