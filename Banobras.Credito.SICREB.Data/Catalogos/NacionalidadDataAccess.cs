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

    public class NacionalidadDataAccess : CatalogoBase<Nacionalidad, int>
    {

        public override string IdField { get { return "CATALOGOS.IdPais"; } }
        public override String ErrorGet { get { return " 1068 - Error al cargar los datos de la base de datos en catálogo Nacionalidad."; } }
        public override String ErrorInsert { get { return "1069 - Error al insertar nuevo registro en catálogo Nacionalidad."; } }
        public override String ErrorUpdate { get { return "1070 - Error al actualizar registro en catálogo Nacionalidad."; } }
        public override String ErrorDelete { get { return "1071 - Error al borrar el registro en catálogo Nacionalidad."; } }

        public NacionalidadDataAccess(Enums.Persona pers)
            : base(pers)
        {

        }

        public override Nacionalidad GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            int claveSIC = Parser.ToNumber(reader["CLAVE_SIC"]);
            string claveBuro = reader["CLAVE_BURO"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();
            Enums.Persona persona = Util.GetPersona(Parser.ToChar(reader["Persona"]));
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            Nacionalidad Nacion = new Nacionalidad(idValue, claveSIC, claveBuro, descripcion, persona, estado);
            //Pais pais = new Pais(idValue, claveSIC, claveBuro, descripcion, persona, estado);
            activo = (Nacion.Estatus == Enums.Estado.Activo);

            return Nacion;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetNacionalidad"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "CATALOGOS.F_CATALOGOS_UpdateNacion"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "CATALOGOS.F_CATALOGOS_DeleteNacion"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "CATALOGOS.F_CATALOGOS_InsertNacion"; }
        }

        public override void SetEntity(DbCommand cmd, Nacionalidad entityOld, Nacionalidad entityNew)
        {
            DB.AddInParameter(cmd, "idnacionalidad", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pclaveburo", DbType.AnsiString, entityNew.ClaveBuro);
            DB.AddInParameter(cmd, "pclavesic", DbType.AnsiString, entityNew.ClaveSIC);
            DB.AddInParameter(cmd, "pdescripcion", DbType.AnsiString, entityNew.Descripcion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Nacionalidad entityToDelete)
        {
            DB.AddInParameter(cmd, "idnacionalidad", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Nacionalidad entityToInsert)
        {
            DB.AddInParameter(cmd, "pclaveburo", DbType.String, entityToInsert.ClaveBuro);
            DB.AddInParameter(cmd, "pclavesic", DbType.Int32, entityToInsert.ClaveSIC);
            DB.AddInParameter(cmd, "pdescripcion", DbType.String, entityToInsert.Descripcion);
            DB.AddInParameter(cmd, "ppersona", DbType.String,Util.SetPersona(entityToInsert.TipoPersona.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
