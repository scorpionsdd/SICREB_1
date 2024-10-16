using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Data;
using System.Data.Common;

namespace Banobras.Credito.SICREB.Data.Seguridad
{

    public class FacultadRolDataAccess:CatalogoBase<FacultadRol,int>
    {

        public override string IdField { get { return "idfacultadrol"; } }
        public override String ErrorGet { get { return "Error al cargar los datos de la base de datos en catálogo Factultad Rol."; } }
        public override String ErrorInsert { get { return "Error al insertar nuevo registro en catálogo Factultad Rol."; } }
        public override String ErrorUpdate { get { return "Error al actualizar registro en catálogo Factultad Rol."; } }
        public override String ErrorDelete { get { return "Error al borrar el registro en catálogo Factultad Rol."; } }

        public override string StoredProcedure
        {
            get { return "Seguridad.SP_GetFacultadesRol"; }
        }

        public override string StoredProcedureDelete
        {
            get
            {
                return "Seguridad.f_delFacultadRolbyRol";
            }
        }

        public override string StoredProcedureInsert
        {
            get
            {
                return "Seguridad.F_InsFacultadRol";
            }
        }

        public override string StoredProcedureUpdate
        {
            get { return "seguridad.F_UpdFacultadRol"; }
        }

        public override FacultadRol GetEntity(IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["id"]);
            int idrol = Parser.ToNumber(reader["id_rol"]);
            int idfacultad = Parser.ToNumber(reader["id_facultad"]);
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            FacultadRol ur = new FacultadRol(id, idrol, idfacultad, estatus);
            activo = (ur.Estatus == Enums.Estado.Activo);

            return ur;
        }

        public override void SetEntity(DbCommand cmd, FacultadRol entityOld, FacultadRol entityNew)
        {
            DB.AddInParameter(cmd, "idFacultadRol", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pidFacultad", DbType.Int32, entityNew.IdFacultad);
            DB.AddInParameter(cmd, "pidRol", DbType.Int32, entityNew.IdRol);
            DB.AddInParameter(cmd, "pEstatus", DbType.String, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, FacultadRol entityToDelete)
        {
            DB.AddInParameter(cmd, "pidrol", DbType.Int32, entityToDelete.IdRol);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, FacultadRol entityToInsert)
        {
            DB.AddInParameter(cmd, "pidFacultad", DbType.Int32, entityToInsert.IdFacultad);
            DB.AddInParameter(cmd, "pidRol", DbType.Int32, entityToInsert.IdRol);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
