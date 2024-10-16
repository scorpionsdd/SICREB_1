using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities
{
    public class ErrorWarningInfo
    {
        public ErrorWarning Error { get; private set; }
        public Validacion Validacion { get; private set; }
        public Etiqueta Etiqueta { get; private set; }
        public Segmento Segmento { get; private set; }

        public string SegEtiq {
            get {
                if (Segmento != default(Segmento) && Etiqueta != default(Etiqueta))
                {
                    return Segmento.Codigo + Etiqueta.Codigo;
                }
                return string.Empty;
            }
        }


        public ErrorWarningInfo(ErrorWarning error, Validacion validacion, Segmento segmento, Etiqueta etiqueta)
        {
            this.Error = error;
            this.Validacion = validacion;
            this.Segmento = segmento;
            this.Etiqueta = etiqueta;
        }
    }
}
