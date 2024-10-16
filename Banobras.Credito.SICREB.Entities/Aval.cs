using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{

    public class Aval
    {
        public int Id { get; private set; }
        public string Credito { get; private set; }
        public string RfcAcreditado { get; private set; }
        public string RfcAval { get; private set; }
        public string NombreCompania { get; private set; }
        public string Nombre { get; private set; }
        public string SNombre { get; private set; }
        public string ApellidoP { get; private set; }
        public string ApellidoM { get; private set; }
        public string Direccion { get; private set; }
        public string ColoniaPoblacion { get; private set; }
        public string DelegacionMunicipio { get; private set; }
        public string Ciudad { get; private set; }
        public string EstadoMexico { get; private set; }
        public string CodigoPostal { get; private set; }
        public Enums.Persona TipoAval { get; private set; }
        public string EstadoExtranjero { get; private set; }
        public string PaisOrigenDomicilio { get; private set; }
        public Enums.TipoOperacion TipoOperacion { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        public Aval(int id, string credito, string rfcAcreditado, string rfcAval, string nombreCompania, string nombre, string sNombre, string apellidoP, string apellidoM, string direccion, string coloniaPoblacion, string delegacionMunicipio, string ciudad, string estadoMexico, string codigoPostal, Enums.Persona tipoAval, string estadoExtranjero, string paisOrigenDomicilio, Enums.TipoOperacion tipoOperacion, Enums.Estado estatus)
        {
            this.Id = id;
            this.Credito = credito;
            this.RfcAcreditado = rfcAcreditado;
            this.RfcAval = rfcAval;
            this.NombreCompania = nombreCompania;
            this.Nombre = nombre;
            this.SNombre = sNombre;
            this.ApellidoP = apellidoP;
            this.ApellidoM = apellidoM;
            this.Direccion = direccion;
            this.ColoniaPoblacion = coloniaPoblacion;
            this.DelegacionMunicipio = delegacionMunicipio;
            this.Ciudad = ciudad;
            this.EstadoMexico = estadoMexico;
            this.CodigoPostal = codigoPostal;
            this.TipoAval = tipoAval;
            this.EstadoExtranjero = estadoExtranjero;
            this.PaisOrigenDomicilio = paisOrigenDomicilio;
            this.TipoOperacion = tipoOperacion;
            this.Estatus = estatus;
        }

    }

}