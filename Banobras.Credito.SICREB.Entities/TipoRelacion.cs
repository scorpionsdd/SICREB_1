using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class TipoRelacion
    {
        public int Id { get; private set; }
        public string ClaveRelacion { get; private set; }
        public string TipoRelaciones { get; private set; }
        public string Descripcion { get; private set; }
        public string Ocupada { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        public TipoRelacion(int id, string claveRelacion, string tipoRelaciones, string descripcion, string ocupado, Enums.Estado estatus)
        {
            this.Id = id;
            this.ClaveRelacion = claveRelacion;
            this.TipoRelaciones = tipoRelaciones;
            this.Descripcion = descripcion;
            this.Ocupada = ocupado;
            this.Estatus = estatus;
        }

    }
}
