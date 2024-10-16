using System;
using Banobras.Credito.SICREB.Entities;
using System.Data.Common;
using System.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{
    public class SegmentosDataAccess : CatalogoBase<Segmento, string>
    {
        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetSegmentos"; }                          
        }

        public override string IdField
        {
            get { return "idsegmentos"; }
        }

        public override String ErrorGet { get { return " 1063 - Error al cargar los datos de la base de datos en Segmentos."; } }
        public override String ErrorInsert { get { return " - No Aplica   Error al insertar nuevo registro en catálogo Sepomex."; } }
        public override String ErrorUpdate { get { return " - No Aplica   Error al actualizar registro en catálogo Sepomex."; } }
        public override String ErrorDelete { get { return " - No Aplica   Error al borrar el registro en catálogo Sepomex."; } }
        

        public override Segmento GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["ID"]);
            string codigo = reader["CODIGO"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            Segmento seg = new Segmento(id, codigo, descripcion, estatus);
            activo = seg.Estatus == Enums.Estado.Activo;
            return seg;
        }
        public override string StoredProcedureUpdate
        {
            get { return "PACKAVISORECHAZO.SpUpdate"; }
        }
        public override void SetEntity(DbCommand cmd, Segmento entityOld, Segmento entityNew)
        {
            //Ejemplo
            DB.AddInParameter(cmd, "nombre", DbType.AnsiString, entityOld.Codigo);
            DB.AddInParameter(cmd, "segudnodato", DbType.AnsiString, entityNew.Descripcion);

        }
    }
}
