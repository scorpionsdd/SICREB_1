using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class MonedasRules
    {
        public List<Moneda> Monedas()
        {
            MonedasDataAccess mda = new MonedasDataAccess();
            return mda.GetRecords(true);
        }

        public int InsertarMoneda(Moneda moneda)
        {
            MonedasDataAccess mda = new MonedasDataAccess();
            return mda.InsertRecord(moneda);
        }
        public int ActulizarMoneda(Moneda moneda)
        {
            MonedasDataAccess mda = new MonedasDataAccess();
            return mda.UpdateRecord(null, moneda);
        }
        public int BorrarMoneda(Moneda moneda)
        {
            MonedasDataAccess mda = new MonedasDataAccess();
            return mda.DeleteRecord(moneda);
        }
    }
}
