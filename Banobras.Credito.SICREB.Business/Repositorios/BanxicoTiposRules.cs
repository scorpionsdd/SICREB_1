using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class BanxicoTiposRules
    {
        BanxicoTiposDataAccess datosBanxicoTipos = null;

        public BanxicoTiposRules()
        {
            datosBanxicoTipos = new BanxicoTiposDataAccess();
        }

        public List< BanxicoTipo> GetRecords(bool soloActivos)
        {
            return datosBanxicoTipos.GetRecords(soloActivos);
        }
        public int Update(BanxicoTipo entityOld, BanxicoTipo entityNew)
        {
            return datosBanxicoTipos.UpdateRecord(entityOld, entityNew); 
        }
        public int Insert(BanxicoTipo entityToInsert)
        {
            return datosBanxicoTipos.InsertRecord(entityToInsert);           
        }

        public int Delete(BanxicoTipo entityToDelete)
        {
            return datosBanxicoTipos.DeleteRecord(entityToDelete);        
        }

    }
}
