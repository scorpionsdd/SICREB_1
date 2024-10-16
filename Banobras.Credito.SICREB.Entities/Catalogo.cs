using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Catalogo
    {
        public int Id { get; set; }
        public string CodigoExterno { get; set; }
        public string CodigoInterno { get; set; }
        public string Descripcion { get; set; }

        public Enums.Estado Estado { get; set; }



        public Catalogo(int id, string codigo_externo, string codigo_interno, string descripcion, Enums.Estado estado)
        {
            this.Id = id;
            this.CodigoExterno = codigo_externo;
            this.CodigoInterno = codigo_interno;
            this.Descripcion = descripcion;
            this.Estado = estado;
        }
    }


    interface ICatalogo
    {

    }
}
