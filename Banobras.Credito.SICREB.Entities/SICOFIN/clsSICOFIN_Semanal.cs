using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities.SICOFIN
{
    public class clsSICOFIN_Semanal
    {
        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }

        public class Query
        {
        }

        public class Root
        {
            public string status { get; set; }
            public string dataType { get; set; }
            public string payloadType { get; set; }
            public string version { get; set; }
            public int dataItems { get; set; }
            public string requestId { get; set; }
            public List<Link> links { get; set; }
            public Query query { get; set; }
            public List<Registro> data { get; set; }
        }
        public class Registro
        {
            public int anio { get; set; }
            public int mes { get; set; }
            public int dia { get; set; }
            public string area { get; set; }
            public int moneda { get; set; }
            public string numeroMayor { get; set; }
            public string cuenta { get; set; }
            public string sector { get; set; }
            public string auxiliar { get; set; }
            public DateTime fechaUltimoMovimiento { get; set; }
            public double saldo { get; set; }
            public int monedaOrigen { get; set; }
            public int naturalezaCuenta { get; set; }
            public double saldoPromedio { get; set; }
            public double montoDebito { get; set; }
            public double montoCredito { get; set; }
            public double saldoAnterior { get; set; }
            public int empresa { get; set; }
            public string calificaMoneda { get; set; }
        }
    }
}
