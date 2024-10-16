using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Request
    {
        
        public int Id { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Final { get; set; }
        public Request_Estado.Estado Status { get; set; }
        public int ArchivoPM { get; set; }
        public int ArchivoPF { get; set; }
        public int ArchivoPF_2011 { get; set; }
        public Enums.Reporte TipoReporte { get; set; }
        public bool MandaNotificaciones { get; set; }
        public string Mensaje { get; set; }
        public string Grupos { get; set; }

        public Request()
        {
            Id = 0;
            Status = Request_Estado.Estado.INICIO;
            ArchivoPM = 0;
            ArchivoPF = 0;
        }

        
        public Request ReportaEstado(Request_Estado.Estado estado)
        {
            this.Status = estado;
            return this;
        }

        

    }
}

