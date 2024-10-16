using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{

    public class AccionistaRules
    {
        //APA 26 Feb 2015

        AccionistaDataAccess datosAccionistas = null;

        public AccionistaRules(Enums.Persona pers)
        {
            datosAccionistas = new AccionistaDataAccess(pers);
        }

        public List<Accionista> GetRecords(bool soloActivos)
        {
            return datosAccionistas.GetRecords(soloActivos);
        }

        public int Update(Accionista entityOld, Accionista entityNew)
        {
            return datosAccionistas.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(Accionista entityToInsert)
        {
            return datosAccionistas.InsertRecord(entityToInsert);           
        }

        public int Delete(Accionista entityToDelete)
        {
            return datosAccionistas.DeleteRecord(entityToDelete);        
        }

    }

}
