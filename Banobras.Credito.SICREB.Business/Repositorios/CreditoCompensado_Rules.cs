using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
   
    public class CreditoCompensado_Rules
    {

        CreditoCompensadoDataAccess datosCreditosCompensados = null;

        public CreditoCompensado_Rules()
        {
            datosCreditosCompensados = new CreditoCompensadoDataAccess();
        }

        public List<CreditoCompensado> GetRecords(bool soloActivos)
        {
            return datosCreditosCompensados.GetRecords(soloActivos);
        }

        public int Update(CreditoCompensado entityOld, CreditoCompensado entityNew)
        {
            return datosCreditosCompensados.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(CreditoCompensado entityToInsert)
        {
            return datosCreditosCompensados.InsertRecord(entityToInsert);           
        }

        public int Delete(CreditoCompensado entityToDelete)
        {
            return datosCreditosCompensados.DeleteRecord(entityToDelete);        
        }

    }

}
