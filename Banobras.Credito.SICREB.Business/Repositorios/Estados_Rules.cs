using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class Estados_Rules
    {

        EstadosDataAccess datosEstados = null;

        public Estados_Rules(Enums.Persona pers)
        {
            datosEstados = new EstadosDataAccess(pers);
        }

        public List<Estado> GetRecords(bool soloActivos)
        {
            return datosEstados.GetRecords(soloActivos);
        }

        public List<Estado> GetEstadoPorClaveBuro(string Persona, string IdEstado, string ClaveBuro, bool soloActivos)
        {
            return datosEstados.GetEstadoPorClaveBuro(Persona, IdEstado, ClaveBuro, soloActivos);
        }

        public int Update(Estado entityOld, Estado entityNew )
        {
            return datosEstados.UpdateRecord(entityOld, entityNew);
        }

        public int Insert(Estado entityToInsert)
        {
            return datosEstados.InsertRecord(entityToInsert); 
        }

        public int Delete(Estado entityToDelete)
        {
            return datosEstados.DeleteRecord(entityToDelete);
        }

    }
}
