using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules.PF
{

    public class PF_PN_Rules
    {

        private CLIC_PFDataAccess cda = null;

        public PF_PN_Rules()
        {
            cda = new CLIC_PFDataAccess();
        }

        public void LoadPNs(PF_Cinta cinta, Enums.Reporte tipoReporte, bool procesa, string grupos)
        { 
            // conectarse a la data access de las vistas y traerse la info de CLIC para llenar un segmento PF_PN y un segmento PF_PA
            // si en un futuro es necesario tomar mas PF_PA podran agregarse tambien.

            cinta.PNs.AddRange(cda.GetPF_PN(cinta, tipoReporte, procesa, grupos));
            
            foreach (PF_PN pn in cinta.PNs)
            {
                var pas = (from pa in cda.GetPF_PA(cinta.ArchivoId, procesa)
                           where pa.PN_ID.Equals(pn.Id)
                           select pa).ToList();

                foreach (PF_PA p in pas)
                {
                    p.Parent = pn;
                }

                pn.PAs.AddRange(pas);
            }

        }

        public void GuardarValores(List<PF_PN> invalidPNs)
        {
            foreach (PF_PN pn in invalidPNs)
            {
                cda.InvalidatePNs(pn);
            }
        }

        public void GuardarValores(List<PF_PA> invalidPAs)
        {
            foreach (PF_PA pa in invalidPAs)
            {
                cda.InvalidatePAs(pa);
            }
        }
   
    }

}
