using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities
{
    public class ConciliacionInfo
    {
        public string Delegacion { get; private set; }
        public string TipoCartera { get; private set; }
        public string Acreditado { get; private set; }
        public Conciliacion SaldosInfo { get; private set; }

        public double SaldoActual
        {
            get 
            {
                double saldo = 0;
                if (SaldosInfo != null)
                {
                    saldo = SaldosInfo.SaldoVigenteOriginal - SaldosInfo.SaldoVigente;
                }
                return saldo;
            }
        }
        public double SaldoVencido
        {
            get
            {
                double saldo = 0;
                if (SaldosInfo != null)
                {
                    saldo = SaldosInfo.SaldoVencidoOriginal - SaldosInfo.SaldoVencido;
                }
                return saldo;
            }
        }



        public ConciliacionInfo(string delegacion, string tipoCartera, string acreditado, Conciliacion conciliacion)
        {
            this.Delegacion = delegacion;
            this.TipoCartera = tipoCartera;
            this.Acreditado = acreditado;
            this.SaldosInfo = conciliacion;
        }
    }
}
