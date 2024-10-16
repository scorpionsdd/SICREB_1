using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ClasificacionActivos_Rules
    {
        ClasificacionActivosDataAccess datosCuentas = null;

        public ClasificacionActivos_Rules()
        {
            datosCuentas = new ClasificacionActivosDataAccess();
        }

        public List<ClasificacionActivos> GetRecords(bool soloActivos)
        {
            return datosCuentas.GetRecords(soloActivos);
        }

        public int Update(ClasificacionActivos entityOld, ClasificacionActivos entityNew)
        {
            return datosCuentas.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(ClasificacionActivos entityToInsert)
        {
            return datosCuentas.InsertRecord(entityToInsert);           
        }

        public int Delete(ClasificacionActivos entityToDelete)
        {
            return datosCuentas.DeleteRecord(entityToDelete);        
        }
        
    }
}
