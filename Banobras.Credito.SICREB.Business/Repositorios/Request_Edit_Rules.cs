using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class Request_Edit_Rules
    {
        Request_EditDataAccess datosCuentas = null;

        public Request_Edit_Rules()
        {
            datosCuentas = new Request_EditDataAccess();
        }

        public List<Request_Edit> GetRecords(bool soloActivos)
        {
            return datosCuentas.GetRecords(soloActivos);
        }

        public int Update(Request_Edit entityOld, Request_Edit entityNew)
        {
            return datosCuentas.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(Request_Edit entityToInsert)
        {
            return datosCuentas.InsertRecord(entityToInsert);           
        }

        public int Reset(Request_Edit entityToDelete)
        {
            return datosCuentas.DeleteRecord(entityToDelete);        
        }        
        
    }
}
