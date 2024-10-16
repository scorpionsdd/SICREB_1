using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{

    public class EstadoCivil_Rules
    {

        EstadoCivilDataAccess datosEstadoCivil = null;

        public EstadoCivil_Rules()
        {
            datosEstadoCivil = new EstadoCivilDataAccess();
        }

        public List<EstadoCivil> GetRecords(bool soloActivos)
        {
            return datosEstadoCivil.GetRecords(soloActivos);
        }

        public int Update(EstadoCivil entityOld, EstadoCivil entityNew)
        {
            return datosEstadoCivil.UpdateRecord(entityOld, entityNew); 
        }

        public int Insert(EstadoCivil entityToInsert)
        {
            return datosEstadoCivil.InsertRecord(entityToInsert);           
        }

        public int Delete(EstadoCivil entityToDelete)
        {
            return datosEstadoCivil.DeleteRecord(entityToDelete);        
        }

    }

}
