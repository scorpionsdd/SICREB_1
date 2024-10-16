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
    public class CreditosExceptuadosRules
    {
        public List<CreditoExceptuado> CreditosExceptuados()
        {
           CreditosExceptuadosDataAccess ceda = new CreditosExceptuadosDataAccess();
           return ceda.GetRecords(true);
        }

        public int CreditosExceptuadosInsertar(CreditoExceptuado Credito)
        {
            CreditosExceptuadosDataAccess ceda = new CreditosExceptuadosDataAccess();
            return ceda.InsertRecord(Credito);
        }
        public int CreditosExceptuadosBorrar(CreditoExceptuado Credito)
        {
            CreditosExceptuadosDataAccess ceda = new CreditosExceptuadosDataAccess();
            return ceda.DeleteRecord(Credito);
        }

        public int CreditosExceptuadosActualizar(CreditoExceptuado Credito)
        {
            CreditosExceptuadosDataAccess ceda = new CreditosExceptuadosDataAccess();
            return ceda.UpdateRecord(null, Credito);

        }
    }
}
