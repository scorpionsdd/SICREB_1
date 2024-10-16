using System;
using System.Data;
using System.Data.Common;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{
    public class CreditosExceptuadosDataAccess:CatalogoBase<CreditoExceptuado,int>
    {
        private readonly Enums.Persona pers = Enums.Persona.Moral;
        public override string IdField { get { return "idexceptuado"; } }
        public override String ErrorGet { get { return " 1030 - Error al cargar los datos de la base de datos en catálogo Creditos Exceptuados."; } }
        public override String ErrorInsert { get { return "1031 - Error al insertar nuevo registro en catálogo Creditos Exceptuados."; } }
        public override String ErrorUpdate { get { return "1032 - Error al actualizar registro en catálogo Creditos Exceptuados."; } }
        public override String ErrorDelete { get { return "1033 - Error al borrar el registro en catálogo Creditos Exceptuados."; } }

        public CreditosExceptuadosDataAccess() { }

        public CreditosExceptuadosDataAccess(Enums.Persona persona)
            : base(persona)
        {
            this.pers = persona;
        }

        public override CreditoExceptuado GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int mid = Parser.ToNumber(reader["ID"]);
            string mcredito = reader["credito"].ToString();
            string mmmotivo = reader["motivo"].ToString();
            Enums.Estado mestatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            CreditoExceptuado ce = new CreditoExceptuado(mid, mcredito, mmmotivo, mestatus);
            activo = ce.Estatus == Enums.Estado.Activo;

            return ce;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GETEXCEPTUADOS"; }
        }

        public override string StoredProcedureDelete
        {
            get
            { return "CATALOGOS.F_CATALOGOS_DelExcep"; }
        }

        public override string StoredProcedureInsert
        {
            get
            { return "CATALOGOS.F_CATALOGOS_InsExcep"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "CATALOGOS.F_CATALOGOS_UpdExcep"; }
        }

        public override void SetEntityInsert(DbCommand cmd, CreditoExceptuado entityToInsert)
        {
            DB.AddInParameter(cmd, "pcredito", DbType.String, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pmotivo", DbType.String, entityToInsert.Motivo);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntity( DbCommand cmd, CreditoExceptuado entityOld,CreditoExceptuado entityNew)
        {
            DB.AddInParameter(cmd, "idexceptuado", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pcredito", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "pmotivo", DbType.AnsiString, entityNew.Motivo);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, CreditoExceptuado entityToDelete)
        {
            DB.AddInParameter(cmd, "idexceptuado", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }
        
    }

}
