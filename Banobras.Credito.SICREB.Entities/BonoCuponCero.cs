using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{

    public class BonoCuponCero
    {
        public int Id { get; private set; }
        public string Credito { get; private set; }
        public string Rfc { get; private set; }
        public string NombreAcreditado { get; private set; }
        public double MontoInversion { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        public BonoCuponCero(int id, string credito, string rfc, string nombreAcreditado, double montoInversion, Enums.Estado estatus)
        {
            this.Id = id;
            this.Credito = credito;
            this.Rfc = rfc;
            this.NombreAcreditado = nombreAcreditado;
            this.MontoInversion = montoInversion;
            this.Estatus = estatus;
        }

    }

}
