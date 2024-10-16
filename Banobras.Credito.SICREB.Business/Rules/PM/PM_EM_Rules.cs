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
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Common.Validator.PM;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules.PM
{

    public class PM_EM_Rules
    {

        private CLIC_PM_DataAccess repoCLIC;
        
        public PM_EM_Rules()
        {
            repoCLIC = new CLIC_PM_DataAccess();
        }

        /// <summary>
        /// Carga los segmentos EMs en la cinta
        /// </summary>
        /// <param name="cinta">Cinta donde se cargarán los EMs. Debe contener su correspondiente ArchivoID</param>
        /// <param name="tipoReporte">Indica el tipo de reporte. Mensual o Semanal</param>
        /// <param name="procesar">Indica si se debe procesar la información o tomar los últimos datos procesados</param>
        public void LoadEMs(PM_Cinta cinta, Enums.Reporte tipoReporte, bool procesar, string grupos)
        {
            List<PM_EM> registros = repoCLIC.GetPM_EMs(cinta, tipoReporte, procesar, grupos); 
            cinta.EMs.AddRange(registros);    
        }

        /// <summary>
        /// Cancela los segmentos EM en la base de datos.
        /// </summary>
        /// <param name="invalidEMs">Segmentos EM que se deben cancelar</param>
        public void GuardaValores(List<PM_EM> invalidEMs)
        {
            foreach (PM_EM em in invalidEMs)
            {
                repoCLIC.InvalidateEMs(em);
            }
        }

    }

}
