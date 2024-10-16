using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{
    //MASS 05/07/13 
    public class Request_EditDataAccess : CatalogoBase<Request_Edit, int>
    {

        public override string IdField { get { return "IdCuenta"; } }
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Request_Edit."; } }        
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Request_Edit."; } }

        public Request_EditDataAccess()
            : base()
        {
        } 

        public override Request_Edit GetEntity(IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["id"].ToString());
            string fecha_inicio = reader["fecha_inicio"].ToString();           
            string fecha_final = reader["fecha_final"].ToString();
            string estado = reader["estado"].ToString();            
            int id_archivo_pm = Parser.ToNumber(reader["id_archivo_pm"].ToString());
            int id_archivo_pf = Parser.ToNumber(reader["id_archivo_pf"].ToString());
            string reporte = reader["reporte"].ToString();
            string notificaciones = reader["notificaciones"].ToString();
            string grupos = reader["grupos"].ToString();                        
            
            Request_Edit cuenta = new Request_Edit(id, fecha_inicio, fecha_final, estado, id_archivo_pm, id_archivo_pf, reporte, notificaciones, grupos);
            activo = true;

            return cuenta;
        }

        public override string StoredProcedure
        {
            get { return "PACKREQUEST_EDIT.SP_CATALOGOS_Get"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKREQUEST_EDIT.SpUpdate"; }
        }

        public override void SetEntity(DbCommand cmd, Request_Edit entityOld, Request_Edit entityNew)
        {
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityOld.id);
            DB.AddInParameter(cmd, "pestado", DbType.AnsiString, entityNew.estado.ToString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public string Estado_actual()
        {
            string r = "";
            DataSet ds = new DataSet();
            using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "PACKREQUEST_EDIT.SP_Get_Estado";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("cur_OUT", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    r = dr["estado"].ToString();
                }
                conn.Close();
            }
            return r;
        }

    }

}
