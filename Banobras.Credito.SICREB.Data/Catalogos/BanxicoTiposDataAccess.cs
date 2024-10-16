using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class BanxicoTiposDataAccess : CatalogoBase<BanxicoTipo, int>
    {

        public override string IdField{ get { return "IdBanxicoTipo"; } }
        public override String ErrorGet { get { return "1015 - Error al cargar los datos de la base de datos en catálogo BanxicoTipos."; } }
        public override String ErrorInsert { get { return "1016 - Error al insertar nuevo registro en catálogo BanxicoTipos."; } }
        public override String ErrorUpdate { get { return "1017 - Error al actualizar registro en catálogo BanxicoTipos."; } }
        public override String ErrorDelete { get { return "1018 - Error al borrar el registro en catálogo BanxicoTipos."; } }

        public override BanxicoTipo GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string descripcion = reader["DESCRIPCION"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            BanxicoTipo banTipo = new BanxicoTipo(idValue, descripcion, estado);
            activo = (banTipo.Estatus == Enums.Estado.Activo);

            return banTipo;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetBanxicoTipos"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKBANXICOTIPO.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKBANXICOTIPO.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKBANXICOTIPO.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, BanxicoTipo entityOld, BanxicoTipo entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityNew.Descripcion);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, BanxicoTipo entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, BanxicoTipo entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityToInsert.Descripcion);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        //public override BanxicoTipo GetEntity(System.Data.IDataReader reader, out bool activo)
        //{
        //    int idValue = Parser.ToNumber(reader["ID"]);
        //    string descripcion = reader["Descripcion"].ToString();
        //    Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));


        //    BanxicoTipo tipo = new BanxicoTipo(idValue, descripcion, estado);
        //    activo = true;

        //    return tipo;
        //}
       
    }

}
