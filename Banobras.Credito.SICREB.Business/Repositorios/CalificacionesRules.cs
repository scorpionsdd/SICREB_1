using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class CalificacionesRules
    {
        CalificacionesDataAccess datosCalificaciones = null;

        public CalificacionesRules()
        {

            datosCalificaciones = new CalificacionesDataAccess();

        }

        public List<Calificacion> GetRecords(bool soloActivos)
        {

            return datosCalificaciones.GetRecords(soloActivos);

        }
        public int Update(Calificacion entityOld, Calificacion entityNew)
        {

            return datosCalificaciones.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(Calificacion entityToInsert)
        {

            return datosCalificaciones.InsertRecord(entityToInsert);
           
        }

        public int Delete(Calificacion entityToDelete)
        {

            return datosCalificaciones.DeleteRecord(entityToDelete);
        
        }

    }
}
