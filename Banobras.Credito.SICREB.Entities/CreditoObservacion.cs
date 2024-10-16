using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{

    public class CreditoObservacion
    {
        public int Id { get; set; }
        public String Credito { get; set; }
        public int IdCvesObservacion { get; set; }
        public Enums.Estado Estatus { get; set; }
        public string CveExterna { get; set; }
        public string Nombre { get; set; }
        public string RFC { get; set; }

        public CreditoObservacion(int pId, string pCredito, int pIdCveObs, string pCveExterna, Enums.Estado pEstatus)
        {
            this.Id = pId;
            this.Credito = pCredito;
            this.IdCvesObservacion = pIdCveObs;
            this.CveExterna = pCveExterna;
            this.Estatus = pEstatus;
        }

        public CreditoObservacion(int pId, string pCredito, int pIdCveObs, string pCveExterna,string rfc,string nombre, Enums.Estado pEstatus)
        {
            this.Id = pId;
            this.Credito = pCredito;
            this.IdCvesObservacion = pIdCveObs;
            this.CveExterna = pCveExterna;
            this.Estatus = pEstatus;
            this.Nombre = nombre;
            this.RFC = rfc; 
        }

    }

}
