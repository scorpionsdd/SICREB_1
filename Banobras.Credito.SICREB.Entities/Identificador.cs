using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Identificador
    {
        public int Id { get; private set; }
        public string Rfc { get; private set; }
        public string Credito { get; private set; }
        public string DigitoIdentificador { get; private set; }
        public string Nombre { get; private set; }
        public Enums.Estado Estatus { get; private set; }
        public Enums.Persona Persona { get; private set; }

        public Identificador(int id, string rfc, string credito, string digitoIdentificador, string nombre, Enums.Estado estatus, Enums.Persona persona)
        {
            this.Id = id;
            this.Rfc = rfc;
            this.Credito = credito;
            this.DigitoIdentificador = digitoIdentificador;
            this.Nombre = nombre;
            this.Estatus = estatus;
            this.Persona = persona;
        }
    }
}
