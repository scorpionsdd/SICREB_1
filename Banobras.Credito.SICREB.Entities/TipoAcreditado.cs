using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{

    public class TipoAcreditado
    {
        public int Id { get; private set; }
        public string RfcAcreditado { get; private set; }
        public string NombreAcreditado { get; private set; }
        public Enums.Persona Tipo_Acreditado { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        public TipoAcreditado(int id, string rfcAcreditado, string nombreAcreditado, Enums.Persona tipo_Acreditado, Enums.Estado estatus)
        {
            this.Id = id;
            this.RfcAcreditado = rfcAcreditado;
            this.NombreAcreditado = nombreAcreditado;
            this.Tipo_Acreditado = tipo_Acreditado;
            this.Estatus = estatus;
        }

    }

}
