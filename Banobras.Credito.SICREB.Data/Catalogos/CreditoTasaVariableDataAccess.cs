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

    public class CreditoTasaVariableDataAccess : CatalogoBase<CreditoTasaVariable, string>
    {

        public override string IdField { get { return "IdCredito"; } }
        public override String ErrorGet { get { return " 1023 - Error al cargar los datos de la base de datos en catálogo Créditos Tasa Variable."; } }
        public override String ErrorInsert { get { return "1027 - Error al insertar nuevo registro en catálogo Créditos Tasa Variable."; } }
        public override String ErrorUpdate { get { return "1028 - Error al actualizar registro en catálogo Créditos Tasa Variable."; } }
        public override String ErrorDelete { get { return "1029 - Error al borrar el registro en catálogo Créditos Tasa Variable."; } }

        public CreditoTasaVariableDataAccess()
        {

        }

        public override CreditoTasaVariable GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int IdCredito = Parser.ToNumber(reader["ID_CREDITO"]);
            string Credito = reader["NUMERO_CREDITO"].ToString();
            string RFC = reader["RFC_ACREDITADO"].ToString();
            string Acreditado = reader["NOMBRE_ACREDITADO"].ToString();
            string Informacion = reader["INFORMACION"].ToString();
            Enums.Estado Estado = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            activo = true;
            CreditoTasaVariable CredTasaVariable = new CreditoTasaVariable(IdCredito, Credito, RFC, Acreditado, Informacion, Enums.Estado.Activo);

            return CredTasaVariable;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetCreditoTasaV"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCREDVARIA.SpInsert"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKCREDVARIA.SpUpdate"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCREDVARIA.SpDelete"; }
        }

        public override void SetEntityInsert(DbCommand cmd, CreditoTasaVariable entityToInsert)
        {
            // Parametros para Insertar
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityToInsert.RFC);
            DB.AddInParameter(cmd, "pNOMBRE", DbType.AnsiString, entityToInsert.Acreditado);
            DB.AddInParameter(cmd, "pINFORMACION", DbType.AnsiString, entityToInsert.Informacion);        
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntity(DbCommand cmd, CreditoTasaVariable entityOld, CreditoTasaVariable entityNew)
        {
            // Parametros para Actualizar
            DB.AddInParameter(cmd, "pIdCREDITO", DbType.Int32, entityNew.Id_Credito);
            DB.AddInParameter(cmd, "newCREDITO", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "newRFC", DbType.AnsiString, entityNew.RFC);
            DB.AddInParameter(cmd, "newNOMBRE", DbType.AnsiString, entityNew.Acreditado);
            DB.AddInParameter(cmd, "newINFORMACION", DbType.AnsiString, entityNew.Informacion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, CreditoTasaVariable entityToDelete)
        {
            // Parametros para Borrar 
            DB.AddInParameter(cmd, "pIdCREDITO", DbType.Int32, entityToDelete.Id_Credito);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
