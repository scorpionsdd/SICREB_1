using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class ArchivosHistoricos
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public string Url { get; private set; }
        public DateTime Fecha { get; private set; }
        public int IdUsuario { get; private set; }
        public int Estatus { get; private set; }
                
        public ArchivosHistoricos(int pId, string pNombre, string pUrl, DateTime pFecha, int pUsuario, int pEstatus)
        {
            this.Id = pId;
            this.Nombre = pNombre;
            this.Url = pUrl;
            this.Fecha = pFecha;
            this.IdUsuario = pUsuario;
            this.Estatus = pEstatus;
        }
    }
}
