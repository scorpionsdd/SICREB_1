using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class CreditosFaltantes_Rules
    {
        CreditosFaltantesDataAccess datosCuentas = null;

        public CreditosFaltantes_Rules()
        {
            datosCuentas = new CreditosFaltantesDataAccess();
        }

        public List<CreditosFaltantes> GetRecords(bool soloActivos)
        {
            return datosCuentas.GetRecords(soloActivos);
        }

        public int Update(CreditosFaltantes entityOld, CreditosFaltantes entityNew)
        {
            return datosCuentas.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(CreditosFaltantes entityToInsert)
        {
            return datosCuentas.InsertRecord(entityToInsert);           
        }

        public int Delete(CreditosFaltantes entityToDelete)
        {
            return datosCuentas.DeleteRecord(entityToDelete);        
        }
        
    }
}
