using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Common.Validator.PM;

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Banobras.Credito.SICREB.Business.Validator.PM
{
    [HasSelfValidation]
    public partial class PM_HD_Validator
    {
        private PM_HD hd = null;
        public const string IDENTIFICADOR = "BNCPM";

        public PM_HD_Validator(PM_HD hd)
        {
            this.hd = hd;
        }

        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PM_HD_Validator> HDValidator = factory.CreateValidator<PM_HD_Validator>();
            return HDValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Identificador del Segmento | Requerido | Valor Fijo
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiquetaHD(ValidationResults results)
            {
                ValidacionEntry result;

                // 101-M Valor de identificador HD inválido.
                if (hd.HD_HD != IDENTIFICADOR)
                {
                    result = CommonValidator.GetValidacionEntry("101-M", hd.Validaciones, hd.HD_HD, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Clave del Usuario | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta00(ValidationResults results)
            {
                ValidacionEntry result;

                // 102-M El dato Clave del Usuario es requerido.
                if (String.IsNullOrWhiteSpace(hd.HD_00))
                {
                    result = CommonValidator.GetValidacionEntry("102-M", hd.Validaciones, hd.HD_00, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 103-M El dato Clave del Usuario no puede ser Cero o un número negativo.
                int clave = Parser.ToNumber(hd.HD_00);
                if (clave <= 0)
                {
                    result = CommonValidator.GetValidacionEntry("103-M", hd.Validaciones, hd.HD_00, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Clave del Usuario Anterior | Opcional | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta01(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Tipo de Usuario (Institucion) | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta02(ValidationResults results)
            {
                ValidacionEntry result;

                // 104-M El dato Tipo de Usuario es requerido.
                if (String.IsNullOrWhiteSpace(hd.HD_02))
                {
                    result = CommonValidator.GetValidacionEntry("104-M", hd.Validaciones, hd.HD_02, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return; 
                }

                // 105-M El dato Tipo de Usuario no se encontro en el catalogo correspondiente.
                string inst = CommonValidator.GetInstitucion(Parser.ToNumber(hd.HD_02), "");
                if (String.IsNullOrWhiteSpace(inst))
                {
                    result = CommonValidator.GetValidacionEntry("105-M", hd.Validaciones, hd.HD_02, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Tipo de Formato | Requerido | Numerico - Opciones [1 Entidades Financieras, 2 Empresas Comerciales]
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta03(ValidationResults results)
            {
                ValidacionEntry result;

                // 106-M El dato Tipo de Formato es requerido.
                if (String.IsNullOrWhiteSpace(hd.HD_03))
                {
                    result = CommonValidator.GetValidacionEntry("106-M", hd.Validaciones, hd.HD_03, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 107-M Dato Tipo de Formato inválido, solo se admite 1 = Detallado para Entidades Financieras
                // Se ocupa la opcion 1 porque Banobras en una Entidad Financiera
                if (hd.HD_03 != "1")
                {
                    result = CommonValidator.GetValidacionEntry("107-M", hd.Validaciones, hd.HD_03, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Fecha de Reporte de Información | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta04(ValidationResults results)
            {
                ValidacionEntry result;

                // 108-M El Dato Fecha de Reporte de Información es requerido.
                if (String.IsNullOrWhiteSpace(hd.HD_04))
                {
                    result = CommonValidator.GetValidacionEntry("108-M", hd.Validaciones, hd.HD_04, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 109-M El Formato de la Fecha de Reporte es Información es invalido. Formato [DDMMAAAA]
                if (!CommonValidator.Match(hd.HD_04, CommonValidator.REGX_FECHA_DDMMAAAA, false))
                {
                    result = CommonValidator.GetValidacionEntry("109-M", hd.Validaciones, hd.HD_04, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

                // 110-M La Fecha de Reporte es Información es posterior a la fecha de entrega del archivo
            }

            /// <summary>
            /// Periodo | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta05(ValidationResults results)
            {
                ValidacionEntry result;

                // 111-M El dato Periodo es requerido.
                if (String.IsNullOrWhiteSpace(hd.HD_05))
                {
                    result = CommonValidator.GetValidacionEntry("111-M", hd.Validaciones, hd.HD_05, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }


                // 112-M El Formato del Periodo es invalido
                if (!CommonValidator.Match(hd.HD_05, CommonValidator.REGX_FECHA_DDMMAAAA, false))
                {
                    result = CommonValidator.GetValidacionEntry("112-M", hd.Validaciones, hd.HD_05, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Version | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta06(ValidationResults results)
            {
                ValidacionEntry result;

                // 113-M El dato Version es requerido.
                if (String.IsNullOrWhiteSpace(hd.HD_06))
                {
                    result = CommonValidator.GetValidacionEntry("113-M", hd.Validaciones, hd.HD_06, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }
            }

            /// <summary>
            /// Nombre del Otorgante | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta07(ValidationResults results)
            {
                ValidacionEntry result;

                // 114-M El dato Nombre del Otorgante es requerido.
                if (String.IsNullOrWhiteSpace(hd.HD_07))
                {
                    result = CommonValidator.GetValidacionEntry("114-M", hd.Validaciones, hd.HD_07, hd.Parent);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }
            }

            /// <summary>
            /// Filler | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta08(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

        #endregion

    }

}
