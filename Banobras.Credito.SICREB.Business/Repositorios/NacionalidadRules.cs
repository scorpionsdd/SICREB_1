using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class NacionalidadRules
    {
        public List<Nacionalidad> GetNacionalidades(Enums.Persona persona)
        {
            NacionalidadDataAccess nac = new NacionalidadDataAccess(persona);
            //PaisesDataAccess pda = new PaisesDataAccess(persona);
            return nac.GetRecords(true);
            //return pda.GetRecords(true);
        }
        public int InsertarNacionalidad(Nacionalidad Nac, Enums.Persona persona)
        {
            NacionalidadDataAccess nac = new NacionalidadDataAccess(persona);
            return nac.InsertRecord(Nac);
        }
        public int BorrarNacionalidad(Nacionalidad Nac, Enums.Persona persona)
        {
            NacionalidadDataAccess nac = new NacionalidadDataAccess(persona);
            return nac.DeleteRecord(Nac);
        }
        public int ActualizaNacionalidad(Nacionalidad Nac, Enums.Persona persona)
        {
            NacionalidadDataAccess nac = new NacionalidadDataAccess(persona);
            return nac.UpdateRecord(null, Nac);
        }
    }
}
