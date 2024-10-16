using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ClavesObservacion_Rules
    {
        ObservacionesDataAccess datosClavesObs = null;

        public ClavesObservacion_Rules(Enums.Persona pers)
        {

            datosClavesObs = new ObservacionesDataAccess(pers);

        }

        public List<Observacion> GetRecords(bool soloActivos)
        {

            return datosClavesObs.GetRecords(soloActivos);

        }
        public int Update(Observacion entityOld, Observacion entityNew)
        {

            return datosClavesObs.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(Observacion entityToInsert)
        {

            return datosClavesObs.InsertRecord(entityToInsert);
           
        }

        public int Delete(Observacion entityToDelete)
        {

            return datosClavesObs.DeleteRecord(entityToDelete);
        
        }

    }
}
