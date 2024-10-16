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

    public class CreditosFiduciariaDataAccess : CatalogoBase<CreditoFiduciario, int>
    {

        public override string IdField { get { return "idcredito"; } }
        public override String ErrorGet { get { return " 1034 - Error al cargar los datos de la base de datos en catálogo Creditos en Contabilidad Fiduciaria."; } }
        public override String ErrorInsert { get { return "1036 - Error al insertar nuevo registro en catálogo Creditos en Contabilidad Fiduciaria."; } }
        public override String ErrorUpdate { get { return "1037 - Error al actualizar registro en catálogo Creditos en Contabilidad Fiduciaria."; } }
        public override String ErrorDelete { get { return "1038 - Error al borrar el registro en catálogo Creditos en Contabilidad Fiduciaria."; } }

        public CreditosFiduciariaDataAccess()
        {

        }

        public override CreditoFiduciario GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string credito = reader["CREDITO"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));
            string rfc = reader["rfc"].ToString();
            string nombre=reader["nombre"].ToString();
            CreditoFiduciario credFiduciaria = new CreditoFiduciario(idValue, credito,rfc,nombre, estado);

            activo = (credFiduciaria.Estatus == Enums.Estado.Activo);

            return credFiduciaria;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetCredFiduci"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKFIDUCIARIA.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKFIDUCIARIA.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKFIDUCIARIA.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, CreditoFiduciario entityOld, CreditoFiduciario entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, CreditoFiduciario entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToDelete.Credito);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, CreditoFiduciario entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pCredito", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
