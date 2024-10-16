using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Data;
using System.Data.Common;

namespace Banobras.Credito.SICREB.Data.Seguridad
{

    public class RolesDataAccess:CatalogoBase<Rol,string>
    {

        public override string IdField { get { return "IdRol"; } }
        public override String ErrorGet { get { return "Error al cargar los datos de la base de datos en catálogo Roles."; } }
        public override String ErrorInsert { get { return "Error al insertar nuevo registro en catálogo Roles."; } }
        public override String ErrorUpdate { get { return "Error al actualizar registro en catálogo Roles."; } }
        public override String ErrorDelete { get { return "Error al borrar el registro en catálogo Roles."; } }

        public override string StoredProcedure
        {
            get { return "seguridad.SP_GetRoles"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "seguridad.F_UpdRol"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "seguridad.F_InsRol"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "seguridad.F_DelRol"; }
        }


        public override Rol GetEntity(IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["id"]);
            string descripcion = reader["rol"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            //TODO: SOL56792 Bitacora Roles
            DateTime creationDate = Parser.ToDateTime(reader["Fecha_Registro"]);
            DateTime transactionDate = Parser.ToDateTime(reader["Fecha_Actualizacion"]);
            string transactionUserLogin = reader["Login_Actualizacion"].ToString();

            Rol ur = new Rol
            {
                CreationDate = creationDate,
                Descripcion = descripcion,
                Estatus = estatus,
                Id = id,
                TransactionDate = transactionDate,
                TransactionLogin =
                transactionUserLogin
            };

            activo = (ur.Estatus == Enums.Estado.Activo);

            return ur;
        }

        public override void SetEntity(DbCommand cmd, Rol entityOld, Rol entityNew)
        {
            DB.AddInParameter(cmd, "pRolId", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pRol", DbType.String, entityNew.Descripcion);
            //TODO: SOL56792 Bitacora Roles
            DB.AddInParameter(cmd, "pEstatus", DbType.String, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddInParameter(cmd, "pFechaRegistro", DbType.DateTime, entityNew.CreationDate);
            DB.AddInParameter(cmd, "pFechaActualizacion", DbType.DateTime, entityNew.TransactionDate);
            DB.AddInParameter(cmd, "pLoginActualizacion", DbType.String, entityNew.TransactionLogin);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Rol entityToDelete)
        {
            DB.AddInParameter(cmd, "IdRol", DbType.Int32, entityToDelete.Id);
            //TODO: SOL56792 Bitacora Roles
            DB.AddInParameter(cmd, "pFechaRegistro", DbType.DateTime, entityToDelete.CreationDate);
            DB.AddInParameter(cmd, "pFechaActualizacion", DbType.DateTime, entityToDelete.TransactionDate);
            DB.AddInParameter(cmd, "pLoginActualizacion", DbType.String, entityToDelete.TransactionLogin);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Rol entityToInsert)
        {
            DB.AddInParameter(cmd, "pRol", DbType.String, entityToInsert.Descripcion);
            //TODO: SOL56792 Bitacora Roles
            DB.AddInParameter(cmd, "pFechaRegistro", DbType.DateTime, entityToInsert.CreationDate);
            DB.AddInParameter(cmd, "pFechaActualizacion", DbType.DateTime, entityToInsert.TransactionDate);
            DB.AddInParameter(cmd, "pLoginActualizacion", DbType.String, entityToInsert.TransactionLogin);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
