using System;
using System.Data.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using System.Data;

namespace Banobras.Credito.SICREB.Data.Catalogos
{
    public class ArchivosHistoricosDataAccess: CatalogoBase<ArchivosHistoricos, int>    
    {

        public override string IdField { get { return "Id"; } } 
        public override String ErrorGet { get { return " 1039 - Error al cargar los Archivos Historicos de la base de datos."; } }

        public ArchivosHistoricosDataAccess()
            : base()
        {
        }
        
        public override ArchivosHistoricos GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string Nombre = reader["Nombre"].ToString();
            string URL = reader["URL"].ToString();
            DateTime fecha = Convert.ToDateTime(reader["FECHA"].ToString());
            int idUsuario = Parser.ToNumber(reader["IDUSUARIO"]);
            int idEstatus = Parser.ToNumber(reader["ESTATUS"]);
            
            ArchivosHistoricos cuenta = new ArchivosHistoricos(idValue, Nombre, URL, fecha, idUsuario, idEstatus);
            activo = (cuenta.Estatus == 1);
            return cuenta;
        }

        public override string StoredProcedure
        {
            get { return "PACKCUENTAS_IFRS.SP_GetArchivosHist"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return ""; }
        }

        public override string StoredProcedureInsert
        {
            get { return ""; }
        }

        public override string StoredProcedureDelete
        {
            get { return ""; }
        }

        public override void SetEntity(DbCommand cmd, ArchivosHistoricos entityOld, ArchivosHistoricos entityNew)
        {
            DB.AddInParameter(cmd, "id_OUT", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "Nombrep", DbType.AnsiString, entityNew.Nombre.ToString());
            DB.AddInParameter(cmd, "urlp", DbType.AnsiString, entityNew.Url.ToString());
            DB.AddInParameter(cmd, "fechap", DbType.DateTime, entityNew.Fecha.ToString());
            DB.AddInParameter(cmd, "idusuariop", DbType.Int32, entityNew.IdUsuario.ToString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }
}
