using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{

    public class AvalRules
    {
        //APA 26 Feb 2015

        AvalDataAccess datosAvales = null;

        public AvalRules(Enums.Persona pers)
        {
            datosAvales = new AvalDataAccess(pers);  
        }

        public List<Aval> GetRecords(bool soloActivos)
        {
            return datosAvales.GetRecords(soloActivos);
        }

        public int Update(Aval entityOld, Aval entityNew)
        {
            return datosAvales.UpdateRecord(entityOld, entityNew);
        }

        public int Insert(Aval entityToInsert)
        {
            return datosAvales.InsertRecord(entityToInsert);
        }

        public int Delete(Aval entityToDelete)
        {
            return datosAvales.DeleteRecord(entityToDelete);
        }

    }

}
