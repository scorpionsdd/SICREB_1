using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class FormaPagos_Rules
    {
        FormasPagoDataAccess datosFormasPago = null;

        public FormaPagos_Rules()
        {

            datosFormasPago = new FormasPagoDataAccess();

        }

        public List<FormaPagos> GetRecords(bool soloActivos)
        {

            return datosFormasPago.GetRecords(soloActivos);

        }
        public int Update(FormaPagos entityOld, FormaPagos entityNew)
        {

            return datosFormasPago.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(FormaPagos entityToInsert)
        {

            return datosFormasPago.InsertRecord(entityToInsert);
           
        }

        public int Delete(FormaPagos entityToDelete)
        {

            return datosFormasPago.DeleteRecord(entityToDelete);
        
        }

    }
}
