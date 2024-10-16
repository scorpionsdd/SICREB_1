using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities.Vistas;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules.PF
{

    public class PF_TL_Rules
    {
        
        public void GuardarValores(List<PF_TL> invalidTLs)
        {
            CLIC_PFDataAccess data = new CLIC_PFDataAccess();

            foreach (PF_TL tl in invalidTLs)
            {
                data.InvalidateTLs(tl);
            }
        }
        
    }

}
