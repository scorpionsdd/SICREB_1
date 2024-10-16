using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Data.Seguridad
{

    public class UsuarioRolDataAccess:CatalogoBase<UsuarioRol,int>
    {

        public override string IdField { get { return "idusuariorol"; } }
        public override String ErrorGet { get { return "Error al cargar los datos de la base de datos en catálogo Usuario Rol."; } }
        public override String ErrorInsert { get { return "Error al insertar nuevo registro en catálogo Usuario Rol."; } }
        public override String ErrorUpdate { get { return "Error al actualizar registro en catálogo Usuario Rol."; } }
        public override String ErrorDelete { get { return "Error al borrar el registro en catálogo Usuario Rol."; } }

        public override UsuarioRol GetEntity(IDataReader reader, out bool activo)
        {
            int Id = Parser.ToNumber(reader["id"]);
            int IdRol = Parser.ToNumber(reader["id_rol"]);
            int IdUsuario = Parser.ToNumber(reader["id_usuario"]);
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            UsuarioRol ur = new UsuarioRol(Id, IdRol, IdUsuario, estatus);
            activo = (ur.Estatus == Enums.Estado.Activo);

            return ur;
        }

        public override string StoredProcedure
        {
            get { return  "Seguridad.SP_GetUsuariosRol"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "seguridad.F_UpdUsuarioRol"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "seguridad.F_DelUsuarioRolbyUsuario"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "Seguridad.F_InsUsuarioRol"; }
        }

        public override void SetEntity(DbCommand cmd, UsuarioRol entityOld, UsuarioRol entityNew)
        {
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pRolId", DbType.Int32, entityNew.IdRol);
            DB.AddInParameter(cmd, "pUsuarioId", DbType.Int32, entityNew.IdUsuario);
            DB.AddInParameter(cmd, "pEstatus", DbType.String, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, UsuarioRol entityToDelete)
        {
            DB.AddInParameter(cmd, "idusuario", DbType.Int32, entityToDelete.IdUsuario);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, UsuarioRol entityToInsert)
        {
            DB.AddInParameter(cmd, "pidrol", DbType.Int32, entityToInsert.IdRol);
            DB.AddInParameter(cmd, "pidusuario", DbType.Int32, entityToInsert.IdUsuario);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
