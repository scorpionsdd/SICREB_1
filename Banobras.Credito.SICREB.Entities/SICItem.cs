using System;

namespace Banobras.Credito.SICREB.Entities
{
    public class SICItem
    {
        public string Rfc { get; set; }
        public int Credito { get; set; }
        public int Auxiliar { get; set; }
        public int Credito_Anterior { get; set; }
        public string Numero_Cuenta { get; set; }
        public string Cuenta_Contable_Capital { get; set; }
        public DateTime Fecha_Apertura { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public int Tipo_Credito { get; set; }
        public int Saldo_Inicial { get; set; }
        public int Clave_Moneda { get; set; }
        public string Moneda { get; set; }
        public int Periodicidad_Intereses { get; set; }
        public string Numero_Pagos { get; set; }
        public string Frecuencia_Pagos { get; set; }
        public int Importe_Pagos { get; set; }
        public int Monto_Pagar { get; set; }
        public int Monto_Pagar_Vencido { get; set; }
        //public DateTime Fecha_Apertura { get; set; }
        public DateTime Fecha_Ultimo_Pago { get; set; }
        public DateTime Fecha_Ultima_Compra { get; set; }
        public DateTime Fecha_Reestructura { get; set; }
        public DateTime Fecha_Cierre { get; set; }
        public int Importe { get; set; }
        public int Pago_Efectivo { get; set; }
        public DateTime Fecha_Liquidacion { get; set; }
        public int Quita { get; set; }
        public int Dacion_Pago { get; set; }
        public int Quebranto { get; set; }
        public int Dias_Vencimiento { get; set; }
        public int Num_Pagos_Vencidos { get; set; }
        public int Formas_Pago { get; set; }
        public DateTime Fecha_Amortizacion_Vencida { get; set; }
    
    }
}
