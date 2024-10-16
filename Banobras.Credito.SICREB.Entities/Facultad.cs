using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;
namespace Banobras.Credito.SICREB.Entities
{
    public class Facultad
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public Enums.Estado Estatus { get; set; }
        public Facultad(int id,string descripcion,Enums.Estado estatus)
        {
            Id = id;
            Descripcion = descripcion;
            Estatus = estatus;
        }

    }
}
