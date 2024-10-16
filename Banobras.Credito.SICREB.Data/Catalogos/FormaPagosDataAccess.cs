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

    public class FormasPagoDataAccess : CatalogoBase<FormaPagos, string>
    {

        public override string IdField{ get { return "IdFormasPago"; } }
        public override String ErrorGet { get { return " 1052 - Error al cargar los datos de la base de datos en catálogo Formas de Pago."; } }
        public override String ErrorInsert { get { return "1053 - Error al insertar nuevo registro en catálogo Formas de Pago."; } }
        public override String ErrorUpdate { get { return "1054 - Error al actualizar registro en catálogo Formas de Pago."; } }
        public override String ErrorDelete { get { return "1055 - Error al borrar el registro en catálogo Formas de Pago."; } }

        public FormasPagoDataAccess()
        {

        }

        public override FormaPagos GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string Descripcion = reader["Descripcion"].ToString();
            string comentario = reader["Comentarios"].ToString();
            string clave = reader["CLAVE"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            activo = true;
            FormaPagos FormaPago = new FormaPagos(idValue,Descripcion,comentario, estado, clave);

            return FormaPago;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetFormasPago"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKFORMPAGOS.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKFORMPAGOS.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKFORMPAGOS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, FormaPagos entityOld, FormaPagos entityNew)
        {
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "newDESCRIPCION", DbType.AnsiString, entityNew.Descripcion);
            DB.AddInParameter(cmd, "newCOMENTARIOS", DbType.AnsiString, entityNew.Comentarios);
            DB.AddInParameter(cmd, "newESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddInParameter(cmd, "pClave", DbType.AnsiString, entityNew.Clave);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, FormaPagos entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, FormaPagos entityToInsert)
        {
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityToInsert.Descripcion);
            DB.AddInParameter(cmd, "pCOMENTARIOS", DbType.AnsiString, entityToInsert.Comentarios);
            DB.AddInParameter(cmd, "pClave", DbType.AnsiString, entityToInsert.Clave);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado( entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
