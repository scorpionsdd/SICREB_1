using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Observacion
    {
        public int Id { get; private set; }
        public string Clave { get; private set; }
        public string Descripcion { get; private set; }
        public Enums.Estado Estatus { get; private set; }
        public Enums.Persona Persona { get; private set; }
        public String Comentario { get; private set; }

        public Observacion(int id, string clave, string descripcion, string comentario, Enums.Estado estatus, Enums.Persona persona)
        {
            this.Id = id;
            this.Clave = clave;
            this.Descripcion = descripcion;
            this.Comentario = comentario;
            this.Estatus = estatus;
            this.Persona = persona;
        }
    }
}
