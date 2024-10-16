using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    
    public class CreditoTasaVariable_Rules
    {

        CreditoTasaVariableDataAccess datosCreditosVariables = null;

        public CreditoTasaVariable_Rules()
        {
            datosCreditosVariables = new CreditoTasaVariableDataAccess();
        }

        public List<CreditoTasaVariable> GetRecords(bool soloActivos)
        {
            return datosCreditosVariables.GetRecords(soloActivos);
        }

        public int Update(CreditoTasaVariable entityOld, CreditoTasaVariable entityNew)
        {
            return datosCreditosVariables.UpdateRecord(entityOld, entityNew);
        }

        public int Insert(CreditoTasaVariable entityToInsert)
        {
            return datosCreditosVariables.InsertRecord(entityToInsert);
        }

        public int Delete(CreditoTasaVariable entityToDelete)
        {
            return datosCreditosVariables.DeleteRecord(entityToDelete);
        }

    }

}
