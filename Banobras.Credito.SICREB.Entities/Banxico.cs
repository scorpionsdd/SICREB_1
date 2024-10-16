using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Banxico
    {
        public int Id { get; private set; }
        public int ClaveCLIC { get; set; }
        public int ClaveBuro { get; set; }
        public string Actividad { get; set; }
        public Enums.Estado Estatus { get; set; }
        public BanxicoTipo Tipo { get; set; }

        public Banxico(int id, int claveCLIC, int claveBuro, string actividad, BanxicoTipo tipo, Enums.Estado estatus)
        {
            this.Id = id;
            this.ClaveCLIC = claveCLIC;
            this.ClaveBuro = claveBuro;
            this.Actividad = actividad;
            this.Tipo = tipo;
            this.Estatus = estatus;
        }

    }
}
