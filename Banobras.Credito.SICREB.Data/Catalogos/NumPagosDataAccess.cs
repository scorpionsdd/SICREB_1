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

    public class NumPagosDataAccess : CatalogoBase<Num_Pago, string>
    {

        public override string IdField { get { return "idnumpagos"; } }
        public override string ClaveField{ get { return "clave"; } }
        public override String ErrorGet { get { return " 1076 - Error al cargar los datos de la base de datos en catálogo Numero de Pagos."; } }
        public override String ErrorInsert { get { return "1077 - Error al insertar nuevo registro en catálogo Numero de Pagos."; } }
        public override String ErrorUpdate { get { return "1078 - Error al actualizar registro en catálogo Numero de Pagos."; } }
        public override String ErrorDelete { get { return "1079 - Error al borrar el registro en catálogo Numero de Pagos."; } }

        public override Num_Pago GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["ID"]);
            string claveBuro = reader["CLAVE_BURO"].ToString();
            string claveSIC = reader["CLAVE_SIC"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            Num_Pago pago = new Num_Pago(id, claveBuro, claveSIC, estatus);
            activo = (pago.Estatus == Enums.Estado.Activo);

            return pago;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetNumPagos"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "CATALOGOS.F_CATALOGOS_DelNumPag"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "CATALOGOS.F_CATALOGOS_InsNumPag"; }  
        }
        
        public override string StoredProcedureUpdate
        {
            get { return "CATALOGOS.F_CATALOGOS_UpdNumPag"; }
        }

        public override void SetEntity(DbCommand cmd, Num_Pago entityOld, Num_Pago entityNew)
        {
            DB.AddInParameter(cmd, "idnumeropago", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pclaveburo", DbType.AnsiString, entityNew.ClaveBuro);
            DB.AddInParameter(cmd, "pclavesic", DbType.AnsiString, entityNew.ClaveSIC);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Num_Pago entityToDelete)
        {
            DB.AddInParameter(cmd, "idnumeropago", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Num_Pago entityToInsert)
        {
            DB.AddInParameter(cmd, "pclaveburo", DbType.String, entityToInsert.ClaveBuro);
            DB.AddInParameter(cmd, "pclavesic", DbType.String, entityToInsert.ClaveSIC);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
