using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ClasificacionRules
    {
        ClasificacionDataAccess datosClasificaciones = null;
     
        public ClasificacionRules()
        {

            datosClasificaciones = new ClasificacionDataAccess();

        }

        public List<Clasificacion> GetRecords(bool soloActivos)
        {

            return datosClasificaciones.GetRecords(soloActivos);

        }
        public int Update(Clasificacion entityOld, Clasificacion entityNew)
        {

            return datosClasificaciones.UpdateRecord(entityOld, entityNew);
 
        }
        public int Insert(Clasificacion entityToInsert)
        {

            return datosClasificaciones.InsertRecord(entityToInsert);
           
        }

        public int Delete(Clasificacion entityToDelete)
        {

            return datosClasificaciones.DeleteRecord(entityToDelete);
        
        }

    }
}
