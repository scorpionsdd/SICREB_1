using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ClasificacionAct_Rules
    {
        ClasificacionActDataAccess datosCuentas = null;

        public ClasificacionAct_Rules()
        {
            datosCuentas = new ClasificacionActDataAccess();
        }

        public List<ClasificacionAct> GetRecords(bool soloActivos)
        {
            return datosCuentas.GetRecords(soloActivos);
        }

        public int Update(ClasificacionAct entityOld, ClasificacionAct entityNew)
        {
            return datosCuentas.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(ClasificacionAct entityToInsert)
        {
            return datosCuentas.InsertRecord(entityToInsert);           
        }

        public int Delete(ClasificacionAct entityToDelete)
        {
            return datosCuentas.DeleteRecord(entityToDelete);        
        }
        
    }
}
