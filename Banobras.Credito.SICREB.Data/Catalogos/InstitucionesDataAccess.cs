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

    public class InstitucionesDataAccess : CatalogoBase<Institucion, int>
    {

        public override string IdField { get { return "idInstituciones"; } }
        public override String ErrorGet { get { return " 1084 - Error al cargar los datos de la base de datos en catálogo Instituciones."; } }
        public override String ErrorInsert { get { return "0000 - NO APLICA    Error al insertar nuevo registro en catálogo Monedas."; } }
        public override String ErrorUpdate { get { return "0000 - NO APLICA    Error al actualizar registro en catálogo Instituciones."; } }
        public override String ErrorDelete { get { return "0000 - NO APLICA    Error al borrar el registro en catálogo Monedas."; } }

        public override Institucion GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            int clave = Parser.ToNumber(reader["CLAVE_EXTERNA"]);
            string descripcion = reader["DESCRIPCION"].ToString();
            string nombreGenerico = reader["GENERICO"].ToString();

            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            Institucion inst = new Institucion(idValue, clave, descripcion, estado, nombreGenerico);
            activo = (inst.Estatus == Enums.Estado.Activo);

            return inst;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetInstituciones"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKAVISORECHAZO.SpUpdate"; }
        }

        public override void SetEntity(DbCommand cmd, Institucion entityOld, Institucion entityNew)
        {
            //Ejemplo
            DB.AddInParameter(cmd, "nombre", DbType.AnsiString, entityOld.ClaveExterna);
            DB.AddInParameter(cmd, "segudnodato", DbType.AnsiString, entityNew.Descripcion);
        }

    }

}
