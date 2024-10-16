using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class CreditoExceptuado
    {
        
            public int Id { get; set; }
            public string Credito { get; set; }
            public string Motivo { get; set; }
            public Enums.Estado Estatus { get; set; }



            public CreditoExceptuado(int id, string credito, string motivo, Enums.Estado estatus)
            {
                this.Id = id;
                this.Credito = credito;
                this.Motivo = motivo;
                this.Estatus = estatus;
            }
        
    }
}
