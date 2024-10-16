using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;


namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class IdentificadorRules
    {
        IdentificadorDataAccess datosIdentificador = null;

        public IdentificadorRules(Enums.Persona pers)
        {

            datosIdentificador = new IdentificadorDataAccess(pers);

        }

        public List<Identificador> GetRecords(bool soloActivos)
        {

           return datosIdentificador.GetRecords(soloActivos);
           

        }
        public int Update(Identificador entityOld, Identificador entityNew)
        {

            return datosIdentificador.UpdateRecord(entityOld, entityOld);
 
        }
        public int Insert(Identificador entityToInsert)
        {

            return datosIdentificador.InsertRecord(entityToInsert);
           
        }

        public int Delete(Identificador entityToDelete)
        {

            return datosIdentificador.DeleteRecord(entityToDelete);
        
        }
    }
}
