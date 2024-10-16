using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ConvertidorActivos_Rules
    {
        ConvertidorActivosDataAccess datosCuentas = null;

        public ConvertidorActivos_Rules()
        {
            datosCuentas = new ConvertidorActivosDataAccess();
        }

        public List<ConvertidorActivos> GetRecords(bool soloActivos)
        {
            return datosCuentas.GetRecords(soloActivos);
        }

        public int Update(ConvertidorActivos entityOld, ConvertidorActivos entityNew)
        {
            return datosCuentas.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(ConvertidorActivos entityToInsert)
        {
            return datosCuentas.InsertRecord(entityToInsert);           
        }

        public int Delete(ConvertidorActivos entityToDelete)
        {
            return datosCuentas.DeleteRecord(entityToDelete);        
        }
        
    }
}
