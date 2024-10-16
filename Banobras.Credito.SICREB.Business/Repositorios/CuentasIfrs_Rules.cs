using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class CuentasIfrs_Rules
    {
        CuentasIfrsDataAccess datosCuentas = null;

        public CuentasIfrs_Rules()
        {
            datosCuentas = new CuentasIfrsDataAccess();
        }

        public List<CuentasIfrs> GetRecords(bool soloActivos)
        {
            return datosCuentas.GetRecords(soloActivos);
        }


        public int Update(CuentasIfrs entityOld, CuentasIfrs entityNew)
        {
            return datosCuentas.UpdateRecord(entityOld, entityNew);
        }

        public int Insert(CuentasIfrs entityToInsert)
        {
            return datosCuentas.InsertRecord(entityToInsert);
        }

        public int Delete(CuentasIfrs entityToDelete)
        {
            return datosCuentas.DeleteRecord(entityToDelete);
        }

    }
}
