using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Etiqueta
    {
        
        public int Id { get; private set; }
        public int SegmentoId { get; private set; }
        public string Codigo { get; private set; }
        public string Descripcion { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        public Etiqueta(int id, int segmentoId, string codigo, string descripcion, Enums.Estado estatus)
        {
            this.Id = id;
            this.SegmentoId = segmentoId;
            this.Codigo = codigo;
            this.Descripcion = descripcion;
            this.Estatus = estatus;
        }
    }
}
