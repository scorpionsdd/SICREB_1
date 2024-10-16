using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities.Vistas
{
    public class Cartera
    {

        public int Status { get; set; }
        public string Rfc { get; set; }
        public int Rfc_Conse { get; set; }
        public int Credito { get; set; }
        public string Auxiliar { get; set; }
        public string Credito_Anterior { get; set; }
        public string Cta_Contab_Capital { get; set; }
        public DateTime Fecha_Apertura { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public int Tipo_Credito { get; set; }
        public double Saldo_Inicial { get; set; }
        public int Cve_Moneda { get; set; }
        public string Moneda { get; set; }
        public string MonedaBuro { get; set; }
        public int Numero_Pagos { get; set; }
        public string Numero_Pagos_Buro { get; set; }
        public string Frecuencia_Pago { get; set; }
        public string Frecuencia_Pago_Buro { get; set; }
        public double Importe_Pago { get; set; }
        public DateTime Fecha_Ultimo_Pago { get; set; }
        public DateTime Fecha_Ultima_Compra { get; set; }
        public DateTime Fecha_Reestructura { get; set; }
        public DateTime Fecha_Cierre { get; set; }
        public double Importe { get; set; }
        public double Quita { get; set; }
        public double Dacion_Pago { get; set; }
        public double Quebranto { get; set; }
        public int Dias_Vencimiento { get; set; }
        public int Num_Pag_Vencidos { get; set; }
        public DateTime Fec_Amort_Vencida { get; set; }
        public DateTime Fecha { get; set; }
        public double Pago_Cta_13 { get; set; }
        public double Pago_Cta_Orden { get; set; }
        public double Pago_Vigete { get; set; }
        public string Calificacion { get; set; }
        public double Monto_Pagar { get; set; }
        public double Monto_Pagar_Vencido { get; set; }
        public double Saldo_Insoluto { get; set; }


    }
}
