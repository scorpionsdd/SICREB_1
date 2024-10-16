using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class BanxicoTipo
    {
        public int Id { get; private set; }
        public string Descripcion { get; private set; }
        public Enums.Estado Estatus { get; set; }

        public BanxicoTipo(int id, string descripcion, Enums.Estado estatus)
        {
            this.Id = id;
            this.Descripcion = descripcion;
            this.Estatus = estatus;
        }
    }
}

