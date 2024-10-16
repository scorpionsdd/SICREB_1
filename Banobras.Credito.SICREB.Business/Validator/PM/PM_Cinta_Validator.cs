using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Business.Validator.PM
{
    public class PM_Cinta_Validator
    {

        public void Valida()
        {
            Valida(Enums.Estado.Activo);
        }
        public void Valida(Enums.Estado estado)
        {
            //ValidationResults allResults = new ValidationResults();
            //this.estado = estado;
            //this.Correctos = 0;

            //allResults.AddAllResults(this.HD.Valida());
            //this.Correctos += this.HD.Correctos;

            //foreach (PM_EM em in this.EMs)
            //{
            //    allResults.AddAllResults(em.Valida());
            //    this.Correctos += em.Correctos;

            //    foreach (PM_CR cr in em.CRs)
            //    {
            //        allResults.AddAllResults(cr.Valida());
            //        this.Correctos += cr.Correctos;

            //        foreach (PM_AV av in cr.AVs)
            //        {
            //            allResults.AddAllResults(av.Valida());
            //            this.Correctos += av.Correctos;
            //        }
            //        foreach (PM_DE de in cr.DEs)
            //        {
            //            allResults.AddAllResults(de.Valida());
            //            this.Correctos += de.Correctos;
            //        }

            //    }

            //    foreach (PM_AC ac in em.ACs)
            //    {
            //        allResults.AddAllResults(ac.Valida());
            //        this.Correctos += ac.Correctos;
            //    }
            //}

            //LogErrorWarning(allResults);
            //Checar validez de cinta!
        }
        //private bool ValidaHD(PM_HD hd)
        //{
        //    List<Validacion> val1 = new List<Validacion>();
        //    List<Validacion> val2 = new List<Validacion>();

        //    val1.AddRange(val2);

        //    ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
        //    Validator<PM_HD> HDValidator
        //               = factory.CreateValidator<PM_HD>();

        //    ValidationResults r = HDValidator.Validate(hd);

        //    LogErrorWarning(r);

        //    return hd.IsValid;

        //}

        //private void ValidaEM(PM_EM em) {

        //    ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
        //    Validator<PM_EM> EMValidator
        //               = factory.CreateValidator<PM_EM>();

        //    ValidationResults r = EMValidator.Validate(em);

        //    LogErrorWarning(r);
        //}

        //private void ValidaAC(PM_AC ac) {
        //    ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
        //    Validator<PM_AC> ACValidator
        //               = factory.CreateValidator<PM_AC>();

        //    ValidationResults r = ACValidator.Validate(ac);

        //    LogErrorWarning(r);
        //}

        //private void ValidaCR(PM_CR cr) {
        //    ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
        //    Validator<PM_CR> CRValidator
        //               = factory.CreateValidator<PM_CR>();

        //    ValidationResults r = CRValidator.Validate(cr);

        //    LogErrorWarning(r);
        //}

        //private void ValidaDE(PM_DE de) {
        //    ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
        //    Validator<PM_DE> DEValidator
        //               = factory.CreateValidator<PM_DE>();

        //    ValidationResults r = DEValidator.Validate(de);

        //    LogErrorWarning(r);
        //}

        //private void ValidaAV(PM_AV av) {
        //    ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
        //    Validator<PM_AV> AVValidator
        //               = factory.CreateValidator<PM_AV>();

        //    ValidationResults r = AVValidator.Validate(av);

        //    LogErrorWarning(r);
        //}


        //private bool LogErrorWarning(ValidationResults result)
        //{
        //    bool toReturn = true;

        //    foreach (ValidationResult res in result)
        //    {
        //        bool loopResult;
        //        try
        //        {
        //            ValidacionEntry entry = (ValidacionEntry)res.Target;
        //            loopResult = LogErrorWarning(res.Message, entry);

        //        }
        //        catch (InvalidCastException ex)
        //        {
        //            loopResult = false;
        //        }

        //        if (!loopResult)
        //            toReturn = false;

        //    }
        //    return toReturn;
        //}

        //private bool LogErrorWarning(string message, ValidacionEntry result)
        //{

        //    ErrorAdvertenciaDataAccess errorData = new ErrorAdvertenciaDataAccess();
        //    errorData.AddErrorAdvertencia(result.ErrorWarning);

        //    Validacion val = Util.GetValidacion(Validaciones, result.ErrorWarning.ValidacionId);
        //    if (val != default(Validacion))
        //    {

        //        if (val.Tipo == Enums.Rechazo.Error)
        //        {
        //            NumErrores++;
        //        }
        //        else
        //        {
        //            NumWarnings++;
        //        }

        //        if (val.Aplicable)
        //        {
        //            result.Rechazar.IsValid = false;
        //        }
        //        return val.Aplicable;
        //    }
        //    return false;

        //}
    }
}
