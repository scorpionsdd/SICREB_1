using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class AuxiliaresRules
    {
        AuxiliaresDataAccess datosAuxiliares = null;

        public AuxiliaresRules()
        {
            datosAuxiliares = new AuxiliaresDataAccess();
        }

        public List<Auxiliares> GetRecords(bool soloActivos)
        {
            return datosAuxiliares.GetRecords(soloActivos);
        }
        public int Update(Auxiliares entityOld, Auxiliares entityNew)
        {
            return datosAuxiliares.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(Auxiliares entityToInsert)
        {
            return datosAuxiliares.InsertRecord(entityToInsert);           
        }

        public int Delete(Auxiliares entityToDelete)
        {
            return datosAuxiliares.DeleteRecord(entityToDelete);        
        }
    }
}
