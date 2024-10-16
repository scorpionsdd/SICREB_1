using System;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Common.Validator.PM;

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Banobras.Credito.SICREB.Business.Validator.PF
{

    [HasSelfValidation]
    public class PF_INTF_Validator
    {

        private PF_INTF intf = null;

        public PF_INTF_Validator(PF_INTF intf)
        {
            this.intf = intf;
        }
            
        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PF_INTF_Validator> HDValidator = factory.CreateValidator<PF_INTF_Validator>();
            return HDValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Etiqueta del Segmento | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_INTF(ValidationResults results)
            { 
                // Sin Validaciones que realizar
            }

            /// <summary>
            /// Version | Numerico | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_05(ValidationResults results)
            {
                // Sin Validaciones que realizar
            }

            /// <summary>
            /// Clave del Usurio o Member Code | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_07(ValidationResults results)
            {

                // 101-F El dato Clave de Usuario es requerido
                if (string.IsNullOrEmpty(intf.INTF_07))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("101-F", intf.Validaciones, intf.INTF_07, intf), "", "", null));
                    return;
                }


                // 102-F La longitud de la Clave de usuario es mayor de lo permitido (10 posiciones)
                if (intf.INTF_07.Length != 10)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("102-F", intf.Validaciones, intf.INTF_07, intf), "", "", null));
                    return;
                }

                intf.Correctos++;
            }

            /// <summary>
            /// Nombre del Usuario | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_17(ValidationResults results)
            {

                // 103-F El dato Nombre del Usuario es requerido
                if (string.IsNullOrEmpty(intf.INTF_17))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("103-F", intf.Validaciones, "", intf), "", "", null));
                    return;
                }

                // 104-F La longitud del dato Nombre de Usuario no puede ser diferente de 16 posiciones (incluyendo espacios en blanco)
                if (intf.INTF_17.Length != 16)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("104-F", intf.Validaciones, intf.INTF_17, intf), "", "", null));
                    return;
                }

                intf.Correctos++;
            }

            /// <summary>
            /// Reservado (Numero de Ciclo) | Texto | Requerido (2 espacios en blanco)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_33(ValidationResults results)
            {

                // 105-F El dato Reservado (Numero de Ciclo) se debe llenar con dos espacios en blanco
                if (string.IsNullOrEmpty(intf.INTF_33))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("105-F", intf.Validaciones, intf.INTF_33, intf), "", "", null));
                    return;
                }

                // 106-F La longitud del dato Reservado (Numero de Ciclo) debe ser de 2 posiciones
                if (intf.INTF_33.Length != 2)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("106-F", intf.Validaciones, intf.INTF_33, intf), "", "", null));
                    return;
                }

                // 107-F El dato Reservado (Numero de Ciclo) no es valido
                if (intf.INTF_33 != "  ")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("107-F", intf.Validaciones, intf.INTF_33, intf), "", "", null));
                    return;
                }

                intf.Correctos++;
            }

            /// <summary>
            /// Fecha de Reporte | Fecha | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_35(ValidationResults results)
            {

                // 108-F El dato Fecha de Reporte es requerido
                if (string.IsNullOrEmpty(intf.INTF_35))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("108-F", intf.Validaciones, intf.INTF_35, intf), "", "", null));
                    return;
                }

                // 109-F La longitud del dato Fecha de Reporte debe ser de 8 posiciones
                if (intf.INTF_35.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("109-F", intf.Validaciones, intf.INTF_35, intf), "", "", null));
                    return;
                }

                // 110-F El formato de la Fecha de Reporte es incorrecto (Formato correcto DDMMYYYY)
                Regex re = new Regex(WebConfig.PatronDeFecha);
                if (!re.IsMatch(intf.INTF_35))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("110-F", intf.Validaciones, intf.INTF_35, intf), "", "", null));
                    return;
                }

                intf.Correctos++;
            }

            /// <summary>
            /// Reservado | Numerico | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_43(ValidationResults results)
            {

                // 111-F El dato Reservado es requerido
                if (string.IsNullOrEmpty(intf.INTF_43))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("111-F", intf.Validaciones, intf.INTF_43, intf), "", "", null));
                    return;
                }

                // 112-F La longitud del dato Reservado debe ser de 10 posiciones
                if (intf.INTF_43.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("112-F", intf.Validaciones, intf.INTF_43, intf), "", "", null));
                    return;
                }

                // 113-F El dato Reservado no es valido
                if (intf.INTF_43 != "0000000000")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("113-F", intf.Validaciones, intf.INTF_43, intf), "", "", null));
                    return;
                }

                intf.Correctos++;
            }

            /// <summary>
            /// Informacion Adicional de Usuario
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_53(ValidationResults results)
            {

                // 114-F El dato Informacion Adicional del Usuario no puede estar vacio
                if (string.IsNullOrEmpty(intf.INTF_53))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("114-F", intf.Validaciones, "", intf), "", "", null));
                    return;
                }

                // 115-F La longitud del dato Informacion Adicional del Usuario no puede ser diferente de 98 posiciones
                if (intf.INTF_53.Length != 98)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("115-F", intf.Validaciones, intf.INTF_53, intf), "", "", null));
                    return;
                }

                intf.Correctos++;
            }

        #endregion Metodos Validadores

    }

}
