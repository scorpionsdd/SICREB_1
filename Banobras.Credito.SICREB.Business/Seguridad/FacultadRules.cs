using Banobras.Credito.SICREB.Data.Seguridad;
using Banobras.Credito.SICREB.Entities;
using System.Collections.Generic;

namespace Banobras.Credito.SICREB.Business.Seguridad
{
    public class FacultadRules
    {

        /// <summary>
        /// Obtener el catálogo de facultades
        /// </summary>
        /// <param name="estatus">True = Sólo los activos | False = Todos los registros</param>
        /// <returns></returns>
        public List<Facultad> Facultades(bool estatus = true)
        {
            FacultadesDataAccess fda = new FacultadesDataAccess();
            //return fda.GetRecords(true);
            return fda.GetRecords(estatus);
        }
    }
}
