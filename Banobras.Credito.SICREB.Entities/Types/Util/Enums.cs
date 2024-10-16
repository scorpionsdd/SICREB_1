using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Banobras.Credito.SICREB.Entities.Util
{

    public class Enums
    {
        public enum Rechazo { Error, Warning };
        public enum Persona { Moral, Fisica, Fideicomiso, Gobierno};
        public enum Estado { Activo, Inactivo, Test };
        public enum Reporte { Mensual, Semanal };
        public enum TipoOperacion { Credito, Linea };
        public enum StoreGrafica { EstadisticasArchivo, ErrorHistorico }
    }

}
