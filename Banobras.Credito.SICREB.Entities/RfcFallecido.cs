using System;

namespace Banobras.Credito.SICREB.Entities
{
    public class RfcFallecido
    {

        public int Id { get; private set; }
        public string Rfc { get; private set; }
        public DateTime Fecha { get; private set; }

        public RfcFallecido(int id, string rfc, DateTime fecha)
        {
            this.Id = id;
            this.Rfc = rfc;
            this.Fecha = fecha;
        }
    }
}
