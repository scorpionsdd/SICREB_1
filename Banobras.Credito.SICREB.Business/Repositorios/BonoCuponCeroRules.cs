using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{

    public class BonoCuponCeroRules
    {

        BonoCuponCeroDataAccess datosBonoCuponCero = null;

        public BonoCuponCeroRules(Enums.Persona pers)
        {
            datosBonoCuponCero = new BonoCuponCeroDataAccess(pers);  
        }

        public List<BonoCuponCero> GetRecords(bool soloActivos)
        {
            return datosBonoCuponCero.GetRecords(soloActivos);
        }

        public int Update(BonoCuponCero entityOld, BonoCuponCero entityNew)
        {
            return datosBonoCuponCero.UpdateRecord(entityOld, entityNew);
        }

        public int Insert(BonoCuponCero entityToInsert)
        {
            return datosBonoCuponCero.InsertRecord(entityToInsert);
        }

        public int Delete(BonoCuponCero entityToDelete)
        {
            return datosBonoCuponCero.DeleteRecord(entityToDelete);
        }

    }

}
