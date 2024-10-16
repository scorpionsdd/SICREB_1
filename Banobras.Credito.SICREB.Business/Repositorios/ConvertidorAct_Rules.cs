using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ConvertidorAct_Rules
    {
        ConvertidorActDataAccess datosCuentas = null;

        public ConvertidorAct_Rules()
        {
            datosCuentas = new ConvertidorActDataAccess();
        }

        public List<ConvertidorAct> GetRecords(bool soloActivos)
        {
            return datosCuentas.GetRecords(soloActivos);
        }

        public int Update(ConvertidorAct entityOld, ConvertidorAct entityNew)
        {
            return datosCuentas.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(ConvertidorAct entityToInsert)
        {
            return datosCuentas.InsertRecord(entityToInsert);           
        }

        public int Delete(ConvertidorAct entityToDelete)
        {
            return datosCuentas.DeleteRecord(entityToDelete);        
        }
        
    }
}
