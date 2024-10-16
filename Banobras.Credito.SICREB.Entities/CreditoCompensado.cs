using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{

    public class CreditoCompensado
    {
        public int Id_Credito { get; set; }
        public string Credito { get; set; }
        public string RFC { get; set; }
        public string Acreditado { get; set; }
        public string Informacion { get; set; }
        public Enums.Estado Estatus { get; set; }

        public CreditoCompensado(int pId, string pCredito, string pRFC, string pAcreditado, string pInformacion, Enums.Estado pEstatus)
        {
            this.Id_Credito = pId;
            this.Credito = pCredito;
            this.RFC = pRFC;
            this.Acreditado = pAcreditado;
            this.Informacion = pInformacion;
            this.Estatus = pEstatus;        
        }

    }

}
