using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class BanxicoRules
    {
        BanxicoDataAccess datosBanxico = null;

        public BanxicoRules()
        {
            datosBanxico = new BanxicoDataAccess();
        }

        public List< Banxico> GetRecords(bool soloActivos)
        {
            return datosBanxico.GetRecords(soloActivos);
        }
        public int Update(Banxico entityOld, Banxico entityNew)
        {
            return datosBanxico.UpdateRecord(entityOld, entityNew); 
        }
        public int Insert(Banxico entityToInsert)
        {
            return datosBanxico.InsertRecord(entityToInsert);           
        }

        public int Delete(Banxico entityToDelete)
        {
            return datosBanxico.DeleteRecord(entityToDelete);        
        }

    }
}
