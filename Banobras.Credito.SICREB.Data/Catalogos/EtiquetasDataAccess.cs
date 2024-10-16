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

    public class EtiquetasDataAccess : CatalogoBase<Etiqueta, string>
    {

        public override string IdField { get { return "idetiqueta"; } }
        public override String ErrorGet { get { return " 1051 - Error al cargar los datos de la base de datos en catálogo Etiquetas."; } }
        //public override String ErrorInsert { get { return "1048 - Error al insertar nuevo registro en catálogo Estados."; } }
        //public override String ErrorUpdate { get { return "1049 - Error al actualizar registro en catálogo Estados."; } }
        //public override String ErrorDelete { get { return "1050 - Error al borrar el registro en catálogo Estados."; } }

        public override Etiqueta GetEntity(IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["ID"]);
            int segmentoId = Parser.ToNumber(reader["ID_SEGMENTO"]);
            string codigo = reader["CODIGO_ETIQUETA"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            Etiqueta etiq = new Etiqueta(id, segmentoId, codigo, descripcion, estatus);
            activo = etiq.Estatus == Enums.Estado.Activo;

            return etiq;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetEtiquetas"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKAVISORECHAZO.SpUpdate"; }
        }

        public override void SetEntity(DbCommand cmd, Etiqueta entityOld, Etiqueta entityNew)
        {
            //Ejemplo
            DB.AddInParameter(cmd, "nombre", DbType.AnsiString, entityOld.Codigo);
            DB.AddInParameter(cmd, "segudnodato", DbType.AnsiString, entityNew.Descripcion);
        }

    }

}
