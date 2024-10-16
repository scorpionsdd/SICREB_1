using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Vistas;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities.Vistas;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules.PM
{

    public class PM_CR_Rules
    {

        private V_SICDataAccess data;
        private CarteraDataAccess carteraData;

        public PM_CR_Rules()
        {
            data = new V_SICDataAccess();
            carteraData = new CarteraDataAccess(Enums.Persona.Moral);
        }

        /// <summary>
        /// Asigna los CRs correspondiente al segmento EM
        /// </summary>
        /// <param name="em">Segmento padre EM</param>
        /// <param name="creditos">Lista de todos los creditos cargados desde la BD</param>
        public void LoadCRs(PM_EM em, List<PM_CR> creditos)
        {

            var crs = (from cr in creditos
                       where cr.EM_ID.Equals(em.Id) 
                       select cr).ToList();

            foreach (PM_CR cr in crs)
                cr.Parent = em;

            em.CRs.AddRange(crs);

        }

        /// <summary>
        /// Cancela los segmentos CR en la base de datos.
        /// </summary>
        /// <param name="invalidCRs">Segmentos CR que se deben cancelar</param>
        public void GuardarValores(List<PM_CR> invalidCRs)
        {
            foreach (PM_CR cr in invalidCRs)
            {
                carteraData.InvalidateCRs(cr);
            }
        }

        /// <summary>
        /// Cancela los segmentos DE en la base de datos.
        /// </summary>
        /// <param name="invalidDEs">Segmentos DE que se deben cancelar</param>
        public void GuardarValores(List<PM_DE> invalidDEs)
        {
            foreach (PM_DE de in invalidDEs)
            {
                carteraData.InvalidateDEs(de);
            }
        }

    }

}
