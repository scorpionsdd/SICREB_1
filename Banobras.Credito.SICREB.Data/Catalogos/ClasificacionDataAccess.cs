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

    public class ClasificacionDataAccess : CatalogoBase<Clasificacion, string>
    {

        public override string IdField { get { return "idclasificaciones"; } }
        public override String ErrorGet { get { return " 1019 - Error al cargar los datos de la base de datos en catálogo Calificaciones."; } }
        public override String ErrorInsert { get { return "1020 - Error al insertar nuevo registro en catálogo Calificaciones."; } }
        public override String ErrorUpdate { get { return "1021 - Error al actualizar registro en catálogo Calificaciones."; } }
        public override String ErrorDelete { get { return "1022 - Error al borrar el registro en catálogo Calificaciones."; } }

        public override Clasificacion GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            
            int idValue = Parser.ToNumber(reader["ID"]);
            string descripcion = reader["DESCRIPCION"].ToString();

            Clasificacion clasif = new Clasificacion(idValue, descripcion);
            activo = true;

            return clasif;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetClasificacion"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKCALIFCARTERA.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCALIFCARTERA.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCALIFCARTERA.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, Clasificacion entityOld, Clasificacion entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityNew.Descripcion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Clasificacion entityToDelete)
        {
            //parametros para especificar el borrado  NUMBER,  VARCHAR2
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Clasificacion entityToInsert)
        {
            //parametros para insertar pCLAVE_BURO VARCHAR2,  VARCHAR2, pESTATUS VARCHAR
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityToInsert.Descripcion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
