using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Util;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Banobras.Credito.SICREB.Business.Validator.PM;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Business.Validator.PF;

namespace Banobras.Credito.SICREB.Business.Validator
{
    public class Validator
    {
        private PM_HD_Validator hdVal = null;
        private PM_EM_Validator emVal = null;
        private PM_AV_Validator avVal = null;
        private PM_CR_Validator crVal = null;
        private PM_DE_Validator deVal = null;
        private PM_AC_Validator acVal = null;

        private PF_INTF_Validator intfVal = null;
        private PF_PN_Validator pnVal = null;
        private PF_PA_Validator paVal = null;
        private PF_PE_Validator peVal = null;
        private PF_TL_Validator tlVal = null;
       

        private ISegmentoType cinta = null;

        public Validator(ISegmentoType cinta)
        {
            this.cinta = cinta;
        }

        public void Valida()
        {
            PM_Cinta cPM = cinta as PM_Cinta;
            PF_Cinta cPF = cinta as PF_Cinta;

            if (cPM != null)
                Valida(cPM, Enums.Estado.Activo);
            else if (cPF != null)
                Valida(cPF, Enums.Estado.Activo);
            
        }
        public void Valida(PM_Cinta cintaPM, Enums.Estado estado)
        {
            
            ValidationResults allResults = new ValidationResults();
            
            cinta.Correctos = 0;

            hdVal = new PM_HD_Validator(cintaPM.HD);
            allResults.AddAllResults(hdVal.Valida());
            cinta.Correctos += cintaPM.HD.Correctos;

            foreach (PM_EM em in cintaPM.EMs)
            {
                emVal = new PM_EM_Validator(em);
                allResults.AddAllResults(emVal.Valida());
                cinta.Correctos += em.Correctos;

                foreach (PM_CR cr in em.CRs)
                {
                    crVal = new PM_CR_Validator(cr);
                    allResults.AddAllResults(crVal.Valida());
                    cinta.Correctos += cr.Correctos;

                    foreach (PM_AV av in cr.AVs)
                    {
                        avVal = new PM_AV_Validator(av);
                        allResults.AddAllResults(avVal.Valida());
                        cinta.Correctos += av.Correctos;
                    }
                    foreach (PM_DE de in cr.DEs)
                    {
                        deVal = new PM_DE_Validator(de);
                        allResults.AddAllResults(deVal.Valida());
                        cinta.Correctos += de.Correctos;
                    }

                }

                foreach (PM_AC ac in em.ACs)
                {
                    acVal = new PM_AC_Validator(ac);
                    allResults.AddAllResults(acVal.Valida());
                    cinta.Correctos += ac.Correctos;
                }
            }

            LogErrorWarning(allResults);
            //Checar validez de cinta!
        }

        public void Valida(PF_Cinta cinta, Enums.Estado estado)
        {
            ValidationResults allResults = new ValidationResults();
            cinta.Correctos = 0;

            intfVal = new PF_INTF_Validator(cinta.INTF);
            allResults.AddAllResults(intfVal.Valida());
            cinta.Correctos += cinta.INTF.Correctos;

            foreach (PF_PN pn in cinta.PNs)
            {
                pnVal = new PF_PN_Validator(pn);
                allResults.AddAllResults(pnVal.Valida());
                cinta.Correctos += pn.Correctos;

                foreach (PF_PA pa in pn.PAs)
                {
                    paVal = new PF_PA_Validator(pa);
                    allResults.AddAllResults(paVal.Valida());
                    cinta.Correctos += pa.Correctos;
                }
                foreach (PF_PE pe in pn.PEs)
                {
                    peVal = new PF_PE_Validator(pe);
                    allResults.AddAllResults(peVal.Valida());
                    cinta.Correctos += pe.Correctos;
                }

                foreach (PF_TL tl in pn.TLs)
                {
                    tlVal = new PF_TL_Validator(tl);
                    allResults.AddAllResults(tlVal.Valida());
                    cinta.Correctos += tl.Correctos;
                }
            }

            LogErrorWarning(allResults);
            ////Checar validez de cinta!
        }


        private bool LogErrorWarning(ValidationResults result)
        {
            bool toReturn = true;
            //----PSL - 31 11 2021
            ErrorAdvertenciaDataAccess errorData = new ErrorAdvertenciaDataAccess();
            errorData.ValidCuentasIFRS9();
            //----
            foreach (ValidationResult res in result)
            {
                bool loopResult;
                try
                {
                    ValidacionEntry entry = (ValidacionEntry)res.Target;
                    loopResult = LogErrorWarning(res.Message, entry);

                }
                catch (InvalidCastException ex)
                {
                    loopResult = false;
                }

                if (!loopResult)
                    toReturn = false;

            }
            return toReturn;
        }

        private bool LogErrorWarning(string message, ValidacionEntry result)
        {

            ErrorAdvertenciaDataAccess errorData = new ErrorAdvertenciaDataAccess();
            errorData.AddErrorAdvertencia(result.ErrorWarning);

            Validacion val = Util.GetValidacion(cinta.Validaciones, result.ErrorWarning.ValidacionId);
            if (val != default(Validacion))
            {
                PF_Cinta cPF = cinta as PF_Cinta;
                PM_Cinta cPM = cinta as PM_Cinta;

                if (val.Tipo == Enums.Rechazo.Error)
                {
                    if (cPF != null)
                        cPF.NumErrores++;
                    else if (cPM != null)
                        cPM.NumErrores++;
                }
                else
                {
                    if (cPF != null)
                        cPF.NumWarnings++;
                    else if(cPM != null)
                        cPM.NumWarnings++;
                }

                if (val.Aplicable)
                {
                    result.Rechazar.IsValid = false;
                }
                return val.Aplicable;
            }
            return false;

        }
    }
}
