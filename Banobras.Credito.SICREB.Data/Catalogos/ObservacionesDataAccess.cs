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

    public class ObservacionesDataAccess : CatalogoBase<Observacion, string>
    {

        public override string IdField { get { return "idobservacion"; } }
        public override String ErrorGet { get { return " 1072 - Error al cargar los datos de la base de datos en catálogo Claves de Observacion."; } }
        public override String ErrorInsert { get { return "1073 - Error al insertar nuevo registro en catálogo Claves de Observacion."; } }
        public override String ErrorUpdate { get { return "1074 - Error al actualizar registro en catálogo Claves de Observacion."; } }
        public override String ErrorDelete { get { return "1075 - Error al borrar el registro en catálogo Claves de Observacion."; } }

        public ObservacionesDataAccess(Enums.Persona pers)
            : base(pers)
        {

        }

        public override Observacion GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string clave = reader["CLAVE_EXTERNA"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();
            string comentario = reader["COMENTARIO"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            Enums.Persona persona = Util.GetPersona(Parser.ToChar(reader["PERSONA"]));

            activo = true;
            Observacion obs = new Observacion(idValue, clave, descripcion, comentario, estado, persona);

            return obs;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetClavesObs"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKCVEOBS.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCVEOBS.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCVEOBS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, Observacion entityOld, Observacion entityNew)
        {
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCLAVE_EXTERNA", DbType.AnsiString, entityNew.Clave);
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityNew.Descripcion);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddInParameter(cmd, "pPERSONA", DbType.AnsiString, Util.SetPersona(entityNew.Persona.ToString()) );
            DB.AddInParameter(cmd, "pCOMENTARIO", DbType.AnsiString, entityNew.Comentario);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Observacion entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pCLAVE_EXTERNA", DbType.AnsiString, entityToDelete.Clave);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Observacion entityToInsert)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pCLAVE_EXTERNA", DbType.AnsiString, entityToInsert.Clave);
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityToInsert.Descripcion);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddInParameter(cmd, "pPERSONA", DbType.AnsiString, Util.SetPersona(entityToInsert.Persona.ToString()));
            DB.AddInParameter(cmd, "pCOMENTARIO", DbType.AnsiString, entityToInsert.Comentario);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
