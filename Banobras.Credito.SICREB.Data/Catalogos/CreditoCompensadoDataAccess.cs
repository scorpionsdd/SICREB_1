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
    
    public class CreditoCompensadoDataAccess : CatalogoBase<CreditoCompensado, string>
    {

        public override string IdField { get { return "IdCredito"; } }
        public override String ErrorGet { get { return " 1023 - Error al cargar los datos de la base de datos en catálogo Créditos Compensados."; } }
        public override String ErrorInsert { get { return "1027 - Error al insertar nuevo registro en catálogo Créditos Compensados."; } }
        public override String ErrorUpdate { get { return "1028 - Error al actualizar registro en catálogo Créditos Compensados."; } }
        public override String ErrorDelete { get { return "1029 - Error al borrar el registro en catálogo Créditos Compensados."; } }

        public CreditoCompensadoDataAccess()
        {

        }

        public override CreditoCompensado GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int IdCredito       = Parser.ToNumber(reader["ID_CREDITO"]);
            string Credito      = reader["NUMERO_CREDITO"].ToString();
            string RFC          = reader["RFC_ACREDITADO"].ToString();
            string Acreditado   = reader["NOMBRE_ACREDITADO"].ToString();
            string Informacion  = reader["INFORMACION"].ToString();
            Enums.Estado Estado = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            activo = true;
            CreditoCompensado CredCompensado = new CreditoCompensado(IdCredito, Credito, RFC, Acreditado, Informacion, Enums.Estado.Activo);

            return CredCompensado;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetCreditoComp"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCREDCOMP.SpInsert"; }
        }
        
        public override string StoredProcedureUpdate
        {
            get { return "PACKCREDCOMP.SpUpdate"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCREDCOMP.SpDelete"; }
        }

        public override void SetEntityInsert(DbCommand cmd, CreditoCompensado entityToInsert)
        {
            // Parametros para Insertar
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityToInsert.RFC );
            DB.AddInParameter(cmd, "pNOMBRE", DbType.AnsiString, entityToInsert.Acreditado);
            DB.AddInParameter(cmd, "pINFORMACION", DbType.AnsiString, entityToInsert.Informacion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntity(DbCommand cmd, CreditoCompensado entityOld, CreditoCompensado entityNew)
        {
            // Parametros para Actualizar
            DB.AddInParameter(cmd, "pIdCREDITO", DbType.Int32, entityNew.Id_Credito);
            DB.AddInParameter(cmd, "newCREDITO", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "newRFC", DbType.AnsiString, entityNew.RFC);
            DB.AddInParameter(cmd, "newNOMBRE", DbType.AnsiString, entityNew.Acreditado);
            DB.AddInParameter(cmd, "newINFORMACION", DbType.AnsiString, entityNew.Informacion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, CreditoCompensado entityToDelete)
        {
            // Parametros para Borrar 
            DB.AddInParameter(cmd, "pIdCREDITO", DbType.Int32, entityToDelete.Id_Credito);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
