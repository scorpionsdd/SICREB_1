using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class CuentasAct_Rules
    {
        CuentasActDataAccess datosCuentas = null;

        public CuentasAct_Rules(Enums.Persona pers)
        {

            datosCuentas = new CuentasActDataAccess(pers);

        }

        public List<CuentasAct> GetRecords(bool soloActivos)
        {

            return datosCuentas.GetRecords(soloActivos);

        }
        public int Update(CuentasAct entityOld, CuentasAct entityNew )
        {

            return datosCuentas.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(CuentasAct entityToInsert)
        {

            return datosCuentas.InsertRecord(entityToInsert);
           
        }

        public int Delete(CuentasAct entityToDelete)
        {

            return datosCuentas.DeleteRecord(entityToDelete);
        
        }

        public List<int> GetGrupos()
        {
            return datosCuentas.GetGrupos();
        }
    }
}
