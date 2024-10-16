using System;
using System.Data;
using System.Data.Common;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class TipoCreditosDataAccess : CatalogoBase<TipoCredito, int>
    {

        public override string IdField { get { return "IdCredito"; } }
        public override String ErrorGet { get { return " 1058 - Error al cargar los datos de la base de datos en catálogo Tipo Credito."; } }
        public override String ErrorInsert { get { return "1059 - Error al insertar nuevo registro en catálogo Tipo Credito."; } }
        public override String ErrorUpdate { get { return "1060 - Error al actualizar registro en catálogo Tipo Credito."; } }
        public override String ErrorDelete { get { return "1061 - Error al borrar el registro en catálogo Tipo Credito."; } }

        public TipoCreditosDataAccess()
        {

        }

        public override TipoCredito GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string tipoCredito = reader["TIPO_CREDITO"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();
            string nombreGenerico = reader["NOMBRE_GENERICO"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            TipoCredito credito = new TipoCredito(idValue, tipoCredito, descripcion, nombreGenerico, estado);

            activo = (credito.Estatus == Enums.Estado.Activo);

            return credito;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetCreditos"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKTIPOCREDITO.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKTIPOCREDITO.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKTIPOCREDITO.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, TipoCredito entityOld, TipoCredito entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pTIPO_CREDITO", DbType.AnsiString, entityNew.tipoCredito);
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityNew.Descripcion);
            DB.AddInParameter(cmd, "pNOMBRE_GENERICO", DbType.AnsiString, entityNew.NombreGenerico);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, TipoCredito entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pTIPO_CREDITO", DbType.AnsiString, entityToDelete.tipoCredito);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, TipoCredito entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pTIPO_CREDITO", DbType.Int32, entityToInsert.tipoCredito);
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityToInsert.Descripcion);
            DB.AddInParameter(cmd, "pNOMBRE_GENERICO", DbType.AnsiString, entityToInsert.NombreGenerico);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
