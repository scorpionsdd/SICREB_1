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
    public class PM_DE_Validator
    {

        private PM_DE de = null;
        public const string IDENTIFICADOR = "DE";

        public PM_DE_Validator(PM_DE de)
        {
            this.de = de;
        }

        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PM_DE_Validator> DEValidator = factory.CreateValidator<PM_DE_Validator>();
            return DEValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Identificador | Requerido | Valor Fijo DE
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiquetaDE(ValidationResults results)
            {
                if (de.TypedParent == null)
                { de.TypedParent = new PM_CR(); }
                    
                ValidacionEntry result;

                // 501-M Valor de identificador DE inválido.
                if (de.DE_DE != IDENTIFICADOR)
                {
                    result = CommonValidator.GetValidacionEntry("501-M", de.Validaciones, de.DE_DE, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// RFC del Acreditado | Requerido | Texto | - EX - | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta00(ValidationResults results)
            {
                if (de.TypedParent == null)
                { de.TypedParent = new PM_CR(); } 

                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero el RFC es opcional
                // ----------------------------------------------------------
                if (de.TypedParent.EsExtranjero && String.IsNullOrWhiteSpace(de.DE_00))
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 502-M El RFC es obligatorio para Clientes/Acreditados Mexicanos.
                if (!de.TypedParent.EsExtranjero && String.IsNullOrWhiteSpace(de.DE_00))
                {
                    result = CommonValidator.GetValidacionEntry("502-M", de.Validaciones, de.DE_00, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 503-M Formato inválido de RFC. Debe ser PM:AAANNNNNNZZZ (12 posiciones) ó PF:AAAANNNNNNZZZ (13 posiciones)
                // 504-M RFC reportado como fallecido.
                int codigo;
                if (!CommonValidator.V_Rfc(de.DE_00, false, out codigo))
                {
                    string codigo_error = (codigo == 102) ? "503-M" : "504-M";
                    result = CommonValidator.GetValidacionEntry(codigo_error, de.Validaciones, de.DE_00, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 505-M El RFC reportado como generico no es aceptado.
                if (CommonValidator.V_RFCGenerico(de.DE_00))
                {
                    result = CommonValidator.GetValidacionEntry("505-M", de.Validaciones, de.DE_00, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 506-M El RFC No debe contener caracteres especiales.
                // 507-M El RFC esta incompleto.
                // 508-M El cliente se reporta como Fideicomiso, pero el RFC no cumple las validaciones correspondientes.

                // 509-M El RFC en el detalle del Credito debe ser el mismo que el del Acreditado.
                if (de.MainRoot.EM_00.Trim() != de.DE_00.Trim())
                {
                    result = CommonValidator.GetValidacionEntry("509-M", de.Validaciones, de.DE_00, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

            }

            /// <summary>
            /// Numero de Cuenta, Credito o Contrato | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta01(ValidationResults results)
            {
                if (de.TypedParent == null)
                { de.TypedParent = new PM_CR(); } 

                ValidacionEntry result;

                // 510-M El dato de Numero de Cuenta, Credito o Contrato es requerido.
                if (String.IsNullOrWhiteSpace(de.DE_01))
                {
                    result = CommonValidator.GetValidacionEntry("510-M", de.Validaciones, de.DE_01, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

                // 511-M El Numero de Contrato del detalle de credito debe ser igual al del Crédito padre que lo contiene.
                if (de.DE_01 != de.TypedParent.CR_02)
                {
                    result = CommonValidator.GetValidacionEntry("511-M", de.Validaciones, de.DE_01, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Numero de Dias de Vencido | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta02(ValidationResults results)
            {
                if (de.TypedParent == null)
                { de.TypedParent = new PM_CR(); }

                ValidacionEntry result;

                // 512-M El dato de Numero de Dias de Vencido es requerido.
                if (String.IsNullOrWhiteSpace(de.DE_02))
                {
                    result = CommonValidator.GetValidacionEntry("512-M", de.Validaciones, de.DE_01, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

            }

            /// <summary>
            /// Cantidad (Saldo) | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta03(ValidationResults results)
            {
                if (de.TypedParent == null)
                { de.TypedParent = new PM_CR(); }

                ValidacionEntry result;
                int DiasVencido = Parser.ToNumber(de.DE_02);
                double Cantidad = Parser.ToDouble(de.DE_03);

                // 513-M El dato de Cantidad (Saldo) es requerido.
                if (String.IsNullOrWhiteSpace(de.DE_03))
                {
                    result = CommonValidator.GetValidacionEntry("513-M", de.Validaciones, de.DE_03, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

                if (Cantidad == 0 && CommonValidator.ValidTipoCreditoEspecial("DE", "03", de.TypedParent.CR_06))
                {
                    // Únicamente se podrá reportar este campo igual a 0 y SIN FECHA DE LIQUIDACIÓN 
                    // para los créditos Revolventes como:
                    //   - Tarjeta de Crédito Empresarial (1380)
                    //   - Tarjeta de Servicio (6250)
                    //   - Línea de Crédito (6280)
                    // Por lo tanto si es un credito especial con saldo cero no aplicamos validaciones extra.
                    return;
                }

                if (Cantidad > 0)
                {
                    // Cuando la cantidad es mayor a cero, se valida que se reporten datos en:
                    // Número de Pagos (Segmento CR: Campo 09)
                    // Frecuencia de Pago (Segmento CR: Campo 10)
                    // Importe de Pago (Segmento CR: Campo 11)
                    // Fecha de Último Pago (Segmento CR: Campo 12)

                    // 514-M La Cantidad (Saldo) es mayor a Cero, pero no se reporta el Numero de Pagos.
                    if (String.IsNullOrWhiteSpace(de.TypedParent.CR_09))
                    {
                        result = CommonValidator.GetValidacionEntry("514-M", de.Validaciones, de.DE_03, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }

                    // 515-M La Cantidad (Saldo) es mayor a Cero, pero no se reporta la Frecuencia de Pago.
                    if (String.IsNullOrWhiteSpace(de.TypedParent.CR_10))
                    {
                        result = CommonValidator.GetValidacionEntry("515-M", de.Validaciones, de.DE_03, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }

                    // 516-M La Cantidad (Saldo) es mayor a Cero, pero no se reporta Importe de Pago.
                    if (String.IsNullOrWhiteSpace(de.TypedParent.CR_11))
                    {
                        result = CommonValidator.GetValidacionEntry("516-M", de.Validaciones, de.DE_03, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }

                    // 517-M La Cantidad (Saldo) es mayor a Cero, pero no se reporta la Fecha del Ultimo Pago.
                    if (CommonValidator.EmptyDate(de.TypedParent.CR_12))
                    {
                        result = CommonValidator.GetValidacionEntry("517-M", de.Validaciones, "Saldo: " + de.DE_03 + " Fecha Ultimo Pago: " + de.TypedParent.CR_12, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }

                if (Cantidad == 0)
                {
                    // 518-M Si Saldo = 0, el Campo 02 “Dias de Vencido” debe ser = 0 y con "Fecha de Liquidacion" (Credito Normal).
                    if (DiasVencido != 0 || CommonValidator.EmptyDate(de.TypedParent.CR_15))
                    {
                        result = CommonValidator.GetValidacionEntry("518-M", de.Validaciones, "Saldo: " + de.DE_03 + " Dias Vencido: " + de.DE_02 + " Fecha Liquidacion: " + de.TypedParent.CR_15, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }

                if (Cantidad < 0)
                {
                    // 519-M Se detecto una inconsistencia debido a que la Cantidad (Saldo) es negativa.
                    result = CommonValidator.GetValidacionEntry("519-M", de.Validaciones, de.DE_03, de.TypedParent, de.MainRoot.EM_00, de.TypedParent.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

            }

            /// <summary>
            /// Filler | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta04(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

        #endregion

    }
}
