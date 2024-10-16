using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class TipoCreditos_Rules
    {
        TipoCreditosDataAccess datosTipoCred = null;

        public TipoCreditos_Rules()
        {
            datosTipoCred = new TipoCreditosDataAccess();
        }

        public List<TipoCredito> GetRecords(bool soloActivos)
        {

            return datosTipoCred.GetRecords(soloActivos);

        }
        public int Update(TipoCredito entityOld, TipoCredito entityNew)
        {

            return datosTipoCred.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(TipoCredito entityToInsert)
        {

            return datosTipoCred.InsertRecord(entityToInsert);
           
        }

        public int Delete(TipoCredito entityToDelete)
        {

            return datosTipoCred.DeleteRecord(entityToDelete);
        
        }

    }
}
