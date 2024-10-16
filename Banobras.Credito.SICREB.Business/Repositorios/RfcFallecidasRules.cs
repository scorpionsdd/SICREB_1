using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class RfcFallecidasRules
    {
        PersonasFallecidasDataAccess datosRfcFallecidas = null;

        public RfcFallecidasRules()
        {

            datosRfcFallecidas = new PersonasFallecidasDataAccess();

        }

        public List<PersonasFallecidas> GetRecords(bool soloActivos)
        {

            return datosRfcFallecidas.GetRecords(soloActivos);

        }
        public int Update(PersonasFallecidas entityOld, PersonasFallecidas entityNew )
        {

            return datosRfcFallecidas.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(PersonasFallecidas entityToInsert)
        {

            return datosRfcFallecidas.InsertRecord(entityToInsert);
           
        }

        public int Delete(PersonasFallecidas entityToDelete)
        {

            return datosRfcFallecidas.DeleteRecord(entityToDelete);
        
        }

    }
}
