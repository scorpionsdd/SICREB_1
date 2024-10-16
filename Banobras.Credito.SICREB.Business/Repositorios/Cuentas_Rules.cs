using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class Cuentas_Rules
    {
        CuentasDataAccess datosCuentas = null;

        public Cuentas_Rules(Enums.Persona pers)
        {

            datosCuentas = new CuentasDataAccess(pers);

        }

        public List<Cuentas> GetRecords(bool soloActivos)
        {

            return datosCuentas.GetRecords(soloActivos);

        }
        public int Update(Cuentas entityOld, Cuentas entityNew )
        {

            return datosCuentas.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(Cuentas entityToInsert)
        {

            return datosCuentas.InsertRecord(entityToInsert);
           
        }

        public int Delete(Cuentas entityToDelete)
        {

            return datosCuentas.DeleteRecord(entityToDelete);
        
        }

        public List<int> GetGrupos()
        {
            return datosCuentas.GetGrupos();
        }
    }
}
