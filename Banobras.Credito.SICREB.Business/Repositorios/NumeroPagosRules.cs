using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
namespace Banobras.Credito.SICREB.Business
{
    public class NumeroPagosRules
    {
        public List<Num_Pago> NumeroPagos()
        {
            NumPagosDataAccess npda = new NumPagosDataAccess();
            return npda.GetRecords(true);
        }
        public int ActualizarNumeroPagos(Num_Pago NumeroPago)
        {
            NumPagosDataAccess npda = new NumPagosDataAccess();
            return npda.UpdateRecord(null, NumeroPago);
        }
        public int BorrarNumeroPagos(Num_Pago NumeroPago)
        {
            NumPagosDataAccess npda = new NumPagosDataAccess();
            return npda.DeleteRecord(NumeroPago);
        }
        public int InsertarNumPagos(Num_Pago NumeroPago)
        {
            NumPagosDataAccess npda = new NumPagosDataAccess();
            return npda.InsertRecord(NumeroPago);
        }
    }
}
