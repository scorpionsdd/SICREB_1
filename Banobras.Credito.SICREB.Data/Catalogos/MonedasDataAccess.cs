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

    public class MonedasDataAccess : CatalogoBase<Moneda, String>
    {

        public override string IdField { get { return "IdMonedas"; } }
        public override String ErrorGet { get { return " 1080 - Error al cargar los datos de la base de datos en catálogo Monedas."; } }
        public override String ErrorInsert { get { return "1081 - Error al insertar nuevo registro en catálogo Monedas."; } }
        public override String ErrorUpdate { get { return "1082 - Error al actualizar registro en catálogo Monedas."; } }
        public override String ErrorDelete { get { return "1083 - Error al borrar el registro en catálogo Monedas."; } }

        public override Moneda GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            int claveCLIC = Parser.ToNumber(reader["CLAVE_CLIC"]);
            int claveBuro = Parser.ToNumber(reader["CLAVE_BURO"]);
            string descripcion = reader["DESCRIPCION"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            Moneda moneda = new Moneda(idValue, claveBuro, claveCLIC, descripcion, estado);
            activo = (moneda.Estatus == Enums.Estado.Activo);

            return moneda;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetMonedas"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "CATALOGOS.F_CATALOGOS_UpdMoneda"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "CATALOGOS.F_CATALOGOS_DelMoneda"; }            
        }

        public override string StoredProcedureInsert
        {
            get { return "CATALOGOS.F_CATALOGOS_InsMoneda"; }
        }

        public override void SetEntity(DbCommand cmd, Moneda entityOld, Moneda entityNew)
        {
            DB.AddInParameter(cmd, "idmoneda", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pclaveburo", DbType.Int32, entityNew.ClaveBuro);
            DB.AddInParameter(cmd, "pclaveclic", DbType.Int32, entityNew.ClaveClic);
            DB.AddInParameter(cmd, "pdescripcion", DbType.String, entityNew.Descripcion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Moneda entityToDelete)
        {
            DB.AddInParameter(cmd, "idmoneda", DbType.AnsiString, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Moneda entityToInsert)
        {
            DB.AddInParameter(cmd, "pclaveburo", DbType.Int32, entityToInsert.ClaveBuro);
            DB.AddInParameter(cmd, "pclaveclic", DbType.Int32, entityToInsert.ClaveClic);
            DB.AddInParameter(cmd, "pdescripcion", DbType.String, entityToInsert.Descripcion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
