using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{

    public class CredCveObs_Rules
    {

        CredCvesObservacionDataAccess datosCreditosCveObs = null;

        public CredCveObs_Rules()
        {
            datosCreditosCveObs = new CredCvesObservacionDataAccess();
        }

        public List<CreditoObservacion> GetRecords(bool soloActivos)
        {
            return datosCreditosCveObs.GetRecords(soloActivos);
        }

        public int Update(CreditoObservacion entityOld, CreditoObservacion entityNew)
        {
            return datosCreditosCveObs.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(CreditoObservacion entityToInsert)
        {
            return datosCreditosCveObs.InsertRecord(entityToInsert);           
        }

        public int Delete(CreditoObservacion entityToDelete)
        {
            return datosCreditosCveObs.DeleteRecord(entityToDelete);        
        }

    }

}
