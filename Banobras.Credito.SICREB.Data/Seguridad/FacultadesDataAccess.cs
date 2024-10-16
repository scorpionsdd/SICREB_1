using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Data.Seguridad
{

    public class FacultadesDataAccess:CatalogoBase<Facultad,int>
    {

        public override string IdField { get { return "IdFacultad"; } }
        public override String ErrorGet { get { return "Error al cargar los datos de la base de datos en catálogo de Facultades"; } }

        public override Facultad GetEntity(IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["id"].ToString());
            string descripcion = reader["descripcion"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            Facultad fc = new Facultad(id, descripcion, estatus);
            activo = fc.Estatus == Enums.Estado.Activo;
            return fc;
        }

        public override string StoredProcedure
        {
            get { return "Seguridad.SP_GetFacultades"; }
        }

        public override string StoredProcedureUpdate
        {
            get { throw new NotImplementedException(); }
        }

        public override void SetEntity(DbCommand cmd, Facultad entityOld, Facultad entityNew)
        {
            throw new NotImplementedException();
        }

    }

}
