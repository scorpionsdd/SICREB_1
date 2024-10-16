using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities
{
    public class ErrorWarning
    {
        public int Id { get; private set; }
        public int ValidacionId { get; private set; }
        public string Rfc { get; private set; }
        public string Credito { get; private set; }
        public string Dato { get; private set; }
        public string UsuarioAlta { get; private set; }

        public ErrorWarning(int id, int validacionId, string rfc, string credito, string dato, string usuarioAlta)
        {
            this.Id = id;
            this.ValidacionId = validacionId;
            this.Rfc = rfc;
            this.Credito = credito;
            this.Dato = dato;
            this.UsuarioAlta = usuarioAlta;
        }
    }
}
