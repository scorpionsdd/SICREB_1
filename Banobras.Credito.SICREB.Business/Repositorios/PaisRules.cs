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
    public class PaisRules
    {

        public List<Pais> Paises(Enums.Persona persona)
        {
            PaisesDataAccess pda = new PaisesDataAccess(persona);
            return pda.GetRecords(true);
        }

        public List<Pais> GetPaisPorClaveBuro(Enums.Persona Persona, string IdPais, string ClaveBuro, bool soloActivos)
        {
            PaisesDataAccess datosPaises = new PaisesDataAccess(Persona);
            return datosPaises.GetPaisPorClaveBuro(Persona, IdPais, ClaveBuro, soloActivos);
        }

        public int InsertarPais(Pais pais, Enums.Persona persona)
        {
            PaisesDataAccess pda = new PaisesDataAccess(persona);
            return pda.InsertRecord(pais);
        }

        public int BorrarPais(Pais pais, Enums.Persona persona)
        {
            PaisesDataAccess pda = new PaisesDataAccess(persona);
            return pda.DeleteRecord(pais);
        }

        public int ActualizaPais(Pais pais, Enums.Persona persona)
        {
            PaisesDataAccess pda = new PaisesDataAccess(persona);
            return pda.UpdateRecord(null,pais);
        }

    }
}
