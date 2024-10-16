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

    public class ExceptuadosDataAccess : CatalogoBase<Exceptuado, string>
    {

        public override string IdField { get { return "idexceptuado"; } }
        public override string ClaveField{ get{ return "numerocredito"; } }
        public override String ErrorGet { get { return " 1030 - Error al cargar los datos de la base de datos en catálogo Creditos Exceptuados."; } }
        public override String ErrorInsert { get { return "1031 - Error al insertar nuevo registro en catálogo Creditos Exceptuados."; } }
        public override String ErrorUpdate { get { return "1032 - Error al actualizar registro en catálogo Creditos Exceptuados."; } }
        public override String ErrorDelete { get { return "1033 - Error al borrar el registro en catálogo Creditos Exceptuados."; } }

        public override Exceptuado GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["ID"]);
            string credito = reader["CREDITO"].ToString();
            string motivo = reader["MOTIVO"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            Exceptuado ex = new Exceptuado(id, credito, motivo, estatus);
            activo = ex.Estatus == Enums.Estado.Activo;

            return ex;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GETEXCEPTUADOS"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKEXCEPTUADOS.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKEXCEPTUADOS.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKEXCEPTUADOS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, Exceptuado entityOld, Exceptuado entityNew)
        {
            DB.AddInParameter(cmd, "pId", DbType.Int32, Parser.ToNumber(entityOld.Id));
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityNew.Credito.ToString());
            DB.AddInParameter(cmd, "pMOTIVO", DbType.AnsiString, entityNew.Motivo.ToString());
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Exceptuado entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pId", DbType.Int32, Parser.ToNumber(entityToDelete.Id));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Exceptuado entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToInsert.Credito.ToString());
            DB.AddInParameter(cmd, "pMOTIVO", DbType.AnsiString, entityToInsert.Motivo.ToString());
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
