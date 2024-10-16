using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;
namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class FrecuenciasPagoRules
    {
        public List<Frecuencia_Pago> FrecuenciasPago()
        {
            FrecuenciasPagoDataAccess fpda = new FrecuenciasPagoDataAccess();
            return fpda.GetRecords(true);
        }
        public int InsertarFrecuenciaPago(Frecuencia_Pago FrecuenciaPago)
        {
            FrecuenciasPagoDataAccess fpda = new FrecuenciasPagoDataAccess();
            return fpda.InsertRecord(FrecuenciaPago);
        }
        public int BorrarFrecuenciaPago(Frecuencia_Pago FrecuenciaPago)
        {
            FrecuenciasPagoDataAccess fpda = new FrecuenciasPagoDataAccess();
            return fpda.DeleteRecord(FrecuenciaPago);
        }
        public int ActualizarFrecuenciaPago(Frecuencia_Pago FrecuenciaPago)
        {
            FrecuenciasPagoDataAccess fpda = new FrecuenciasPagoDataAccess();
            return fpda.UpdateRecord(null, FrecuenciaPago);
        }
    }
}
