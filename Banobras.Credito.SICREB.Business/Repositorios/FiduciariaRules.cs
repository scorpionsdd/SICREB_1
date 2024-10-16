using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class FiduciariaRules
    {
        CreditosFiduciariaDataAccess datosFiduciaria = null;

        public FiduciariaRules()
        {

            datosFiduciaria = new CreditosFiduciariaDataAccess();

        }

        public List<CreditoFiduciario> GetRecords(bool soloActivos)
        {

            return datosFiduciaria.GetRecords(soloActivos);

        }
        public int Update(CreditoFiduciario entityOld, CreditoFiduciario entityNew)
        {

            return datosFiduciaria.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(CreditoFiduciario entityToInsert)
        {

            return datosFiduciaria.InsertRecord(entityToInsert);
           
        }

        public int Delete(CreditoFiduciario entityToDelete)
        {

            return datosFiduciaria.DeleteRecord(entityToDelete);
        
        }

    }
}
