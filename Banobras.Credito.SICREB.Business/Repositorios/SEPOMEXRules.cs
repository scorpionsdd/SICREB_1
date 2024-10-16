using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class SEPOMEXRules
    {

        SEPOMEXDataAccess smda = new SEPOMEXDataAccess();
        public List<SEPOMEX> GetSepomex()
        {   
            return  smda.GetRecords(true);
        }

        public List<SEPOMEX> GetSepomexPorCP(string CP, bool soloActivos)
        {
            return smda.GetSEPOMEXPorCP(CP, soloActivos);
        }

    }
}
