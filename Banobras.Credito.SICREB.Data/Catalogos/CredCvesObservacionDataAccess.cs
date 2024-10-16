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

    public class CredCvesObservacionDataAccess : CatalogoBase<CreditoObservacion, string>
    {

        public override string IdField { get { return "idobservacion"; } }
        public override String ErrorGet { get { return " 1023 - Error al cargar los datos de la base de datos en catálogo Créditos con Clave de Observación."; } }
        public override String ErrorInsert { get { return "1027 - Error al insertar nuevo registro en catálogo Créditos con Clave de Observación."; } }
        public override String ErrorUpdate { get { return "1028 - Error al actualizar registro en catálogo Créditos con Clave de Observación."; } }
        public override String ErrorDelete { get { return "1029 - Error al borrar el registro en catálogo Créditos con Clave de Observación."; } }

        public CredCvesObservacionDataAccess()
        {

        }

        public override CreditoObservacion GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string Credito = reader["CREDITO"].ToString();
            int IdCveObs = Parser.ToNumber(reader["ID_CLAVES_OBSERVACION"]);
            string cveExterna = reader["CLAVE_EXTERNA"].ToString();
            string rfc = reader["RFC"].ToString();
            string nombre = reader["Nombre"].ToString();
            //Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            activo = true;
            CreditoObservacion CredCveObs = new CreditoObservacion(idValue, Credito, IdCveObs, cveExterna,rfc,nombre, Enums.Estado.Activo);

            return CredCveObs;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetObservacionCre"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKCREDOBS.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCREDOBS.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCREDOBS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, CreditoObservacion entityOld, CreditoObservacion entityNew)
        {
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "newCREDITO", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "newID_CLAVES_OBSERVACION", DbType.Int32, entityNew.IdCvesObservacion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, CreditoObservacion entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, CreditoObservacion entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pID_CLAVES_OBSERVACION", DbType.Int32, entityToInsert.IdCvesObservacion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
