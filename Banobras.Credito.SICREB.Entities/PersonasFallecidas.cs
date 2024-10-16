using System;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class PersonasFallecidas
    {
        public int Id { get; private set; }
        public string Rfc { get; private set; }
        //SICREB-INICIO ACA Sep-2012
        public string Nombre { get; private set; }
        public string Credito { get; private set; }
        public string Auxiliar { get; private set; }
        //SICREB-INICIO ACA Sep-2012
        public DateTime Fecha { get; private set; }
        public Enums.Estado Estatus { get; private set; }
        
        //SICREB-INICIO ACA Sep-2012
        public PersonasFallecidas(int pId, string pRfc, string pNombre, string pCredito, string pAuxiliar, DateTime pFecha, Enums.Estado pEstatus)
        {
            this.Id = pId;
            this.Rfc = pRfc;
            this.Nombre = pNombre;
            this.Credito = pCredito;
            this.Auxiliar = pAuxiliar;
            this.Fecha = pFecha;
            this.Estatus = pEstatus;
        }
        //SICREB-FIN ACA Sep-2012
    }
}
