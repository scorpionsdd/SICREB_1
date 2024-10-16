using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Segmento
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; }
        public string Descripcion { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        public Segmento(int id, string codigo, string descripcion, Enums.Estado estatus)
        {
            this.Id = id;
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            this.Estatus = estatus;
        }
    }
}
