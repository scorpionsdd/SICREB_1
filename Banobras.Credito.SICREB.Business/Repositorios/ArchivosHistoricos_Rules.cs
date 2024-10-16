using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ArchivosHistoricos_Rules
    {
        ArchivosHistoricosDataAccess datosCuentas = null;
        
        public ArchivosHistoricos_Rules()
        {
            datosCuentas = new ArchivosHistoricosDataAccess();
        }

        public List<ArchivosHistoricos> GetRecords(bool soloActivos)
        {
            return datosCuentas.GetRecords(soloActivos);
        }
        public int Update(ArchivosHistoricos entityOld, ArchivosHistoricos entityNew)
        {
            return datosCuentas.UpdateRecord(entityOld, entityNew);
        }

    }
}
