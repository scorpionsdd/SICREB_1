using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class TipoRelacionRules
    {

        TipoRelacionDataAccess datosTipoRelacion = null;

        public TipoRelacionRules()
        {
            datosTipoRelacion = new TipoRelacionDataAccess();
        }

        public List<TipoRelacion> GetRecords(bool soloActivos)
        {
            return datosTipoRelacion.GetRecords(soloActivos);
        }

        public List<TipoRelacion> GetRecordsPorClave(string ClaveRelacion, bool soloActivos)
        {
            return datosTipoRelacion.GetTipoRelacionPorClave(ClaveRelacion, soloActivos);
        }

        public List<TipoRelacion> GetRecordsPorUtilidad(string Ocupadas, bool soloActivos)
        {
            return datosTipoRelacion.GetTipoRelacionPorUtilidad(Ocupadas, soloActivos);
        }

        public int Update(TipoRelacion entityOld, TipoRelacion entityNew)
        {
            return datosTipoRelacion.UpdateRecord(entityOld, entityNew);
        }

        public int Insert(TipoRelacion entityToInsert)
        {
            return datosTipoRelacion.InsertRecord(entityToInsert);
        }

        public int Delete(TipoRelacion entityToDelete)
        {
            return datosTipoRelacion.DeleteRecord(entityToDelete);
        }

    }
}
