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

    public class AuxiliaresDataAccess : CatalogoBase<Auxiliares, int>
    {

        public override string IdField { get { return "idestado"; } }
        public override String ErrorGet { get { return "1007 - Error al cargar los datos de la base de datos en catálogo Auxiliares."; } }
        public override String ErrorInsert { get { return "1008 - Error al insertar nuevo registro en catálogo Auxiliares."; } }
        public override String ErrorUpdate { get { return "1009 - Error al actualizar registro en catálogo Auxiliares."; } }
        public override String ErrorDelete { get { return "1010 - Error al borrar el registro en catálogo Auxiliares."; } }

        public AuxiliaresDataAccess()
        {

        }

        public override Auxiliares GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string credito = reader["CREDITO"].ToString();
            string auxiliar = reader["AUXILIAR"].ToString();
            string rfc = reader["rfc"].ToString();
            string nombre = reader["nombre"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            Auxiliares auxiliares = new Auxiliares(idValue, credito, auxiliar,rfc,nombre, estado);
            activo = (auxiliares.Estatus == Enums.Estado.Activo);

            return auxiliares;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetCredAuxilia"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKAUXILIARES.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKAUXILIARES.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKAUXILIARES.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, Auxiliares entityOld, Auxiliares entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "pAUXILIAR", DbType.AnsiString, entityNew.Auxiliar);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Auxiliares entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Auxiliares entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pAUXILIAR", DbType.AnsiString, entityToInsert.Auxiliar);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
