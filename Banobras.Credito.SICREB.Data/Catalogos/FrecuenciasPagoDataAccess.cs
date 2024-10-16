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

    public class FrecuenciasPagoDataAccess : CatalogoBase<Frecuencia_Pago, string>
    {

        public override string IdField { get { return "idfrecuencia"; } }
        public override String ErrorGet { get { return " 1085 - Error al cargar los datos de la base de datos en catálogo Frecuencia de Pagos."; } }
        public override String ErrorInsert { get { return "1086 - Error al insertar nuevo registro en catálogo Frecuencia de Pagos."; } }
        public override String ErrorUpdate { get { return "1087 - Error al actualizar registro en catálogo Frecuencia de Pagos."; } }
        public override String ErrorDelete { get { return "1088 - Error al borrar el registro en catálogo Frecuencia de Pagos."; } }

        public override Frecuencia_Pago GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["ID"]);
            string claveBuro = reader["CLAVE_BURO"].ToString();
            string claveSIC = reader["CLAVE_SIC"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            Frecuencia_Pago freq = new Frecuencia_Pago(id, claveBuro, claveSIC, estatus);
            activo = (freq.Estatus == Enums.Estado.Activo);

            return freq;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetFrecuenciasPag"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "CATALOGOS.F_CATALOGOS_UpdFrecPag"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "CATALOGOS.F_CATALOGOS_InsFrecPag"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "CATALOGOS.F_CATALOGOS_DelFrecPag"; }
        }

        public override void SetEntity(DbCommand cmd, Frecuencia_Pago entityOld, Frecuencia_Pago entityNew)
        {
            DB.AddInParameter(cmd, "idfrecuenciapago", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pclaveburo", DbType.AnsiString, entityNew.ClaveBuro);
            DB.AddInParameter(cmd, "pclavesic", DbType.AnsiString, entityNew.ClaveSIC);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Frecuencia_Pago entityToDelete)
        {
            DB.AddInParameter(cmd, "idfrecuenciapago", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Frecuencia_Pago entityToInsert)
        {
            DB.AddInParameter(cmd, "pclaveburo", DbType.AnsiString, entityToInsert.ClaveBuro);
            DB.AddInParameter(cmd, "pclavesic", DbType.AnsiString, entityToInsert.ClaveSIC);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }
        
    }

}
