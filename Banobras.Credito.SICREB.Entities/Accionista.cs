using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Accionista
    {
        public int Id { get; private set; }
        public string RfcAcreditado { get; private set; }
        public string RfcAccionista { get; private set; }
        public string NombreCompania { get; private set; }
        public string Nombre { get; private set; }
        public string SNombre { get; private set; }
        public string ApellidoP { get; private set; }
        public string ApellidoM { get; private set; }
        public int Porcentaje { get; private set; }
        public string Direccion { get; private set; }
        public string ColoniaPoblacion { get; private set; }
        public string DelegacionMunicipio { get; private set; }
        public string Ciudad { get; private set; }
        public string EstadoMexico { get; private set; }
        public string CodigoPostal { get; private set; }
        public Enums.Persona Persona { get; private set; }
        public string EstadoExtranjero { get; private set; }
        public string PaisOrigenDomicilio { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        public Accionista(int id, string rfcAcreditado, string rfcAccionista, string nombreCompania, string nombre, string sNombre, string apellidoP, string apellidoM, int porcentaje, string direccion, string coloniaPoblacion, string delegacionMunicipio, string ciudad, string estadoMexico, string codigoPostal, Enums.Persona persona, string estadoExtranjero, string paisOrigenDomicilio, Enums.Estado estatus)
        {
            this.Id = id;
            this.RfcAcreditado = rfcAcreditado;
            this.RfcAccionista = rfcAccionista;
            this.NombreCompania = nombreCompania;
            this.Nombre = nombre;
            this.SNombre = sNombre;
            this.ApellidoP = apellidoP;
            this.ApellidoM = apellidoM;
            this.Porcentaje = porcentaje;
            this.Direccion = direccion;
            this.ColoniaPoblacion = coloniaPoblacion;
            this.DelegacionMunicipio = delegacionMunicipio;
            this.Ciudad = ciudad;
            this.EstadoMexico = estadoMexico;
            this.CodigoPostal = codigoPostal;
            this.Persona = persona;
            this.EstadoExtranjero = estadoExtranjero;
            this.PaisOrigenDomicilio = paisOrigenDomicilio;
            this.Estatus = estatus; 
        }
    }
}
