using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{

    public class TipoAcreditado_Rules
    {
        //APA 05 Nov 2015

        TipoAcreditadoDataAccess datosAcreditados = null;

        public TipoAcreditado_Rules(Enums.Persona pers)
        {
            datosAcreditados = new TipoAcreditadoDataAccess(pers);  
        }

        public List<TipoAcreditado> GetRecords(bool soloActivos)
        {
            return datosAcreditados.GetRecords(soloActivos);
        }

        public List<TipoAcreditado> GetRecorsPorRFCAcreditado(string RFCAcreditado, bool soloActivos)
        {
            return datosAcreditados.GetTipoAcreditadoPorRFC(RFCAcreditado, soloActivos);
        }

        public int Update(TipoAcreditado entityOld, TipoAcreditado entityNew)
        {
            return datosAcreditados.UpdateRecord(entityOld, entityNew);
        }

        public int Insert(TipoAcreditado entityToInsert)
        {
            return datosAcreditados.InsertRecord(entityToInsert);
        }

        public int Delete(TipoAcreditado entityToDelete)
        {
            return datosAcreditados.DeleteRecord(entityToDelete);
        }

    }

}
