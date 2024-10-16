using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities
{
    public class ValorArchivo
    {
        public int Id { get; private set; }
        public int ArchivoId { get; private set; }
        public int EtiquetaId { get; private set; }
        public string Valor { get; private set; }


        public ValorArchivo(int id, int archivoId, int etiquetaId, string valor)
        {
            this.Id = id;
            this.ArchivoId = archivoId;
            this.EtiquetaId = etiquetaId;
            this.Valor = valor;
        }
    }
}
