using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    class AvisoRechazo_Rules
    {
        ObservacionesDataAccess datosObservacion = null;

        public AvisoRechazo_Rules(Enums.Persona pers)
        {
            datosObservacion = new ObservacionesDataAccess(pers);
        }
        public int Update(Observacion entityOld, Observacion entityNew )
        {
            return datosObservacion.UpdateRecord(entityOld, entityNew); 
        }
    }
}
