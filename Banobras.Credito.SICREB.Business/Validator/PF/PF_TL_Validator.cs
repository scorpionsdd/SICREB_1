using System;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Common.Validator.PM;

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.IO;
using System.Threading.Tasks;

namespace Banobras.Credito.SICREB.Business.Validator.PF
{

    [HasSelfValidation]
    public class PF_TL_Validator
    {

        private PF_TL tl = null;

        public PF_TL_Validator(PF_TL tl)
        {
            this.tl = tl;
        }

        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PF_TL_Validator> HDValidator = factory.CreateValidator<PF_TL_Validator>();
            return HDValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Etiqueta del Segmento
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_TL(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Clave del Usuario o Member Code
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_01(ValidationResults results)
            {

                // 501-F El dato Clave del Usuario es requerido
                if (string.IsNullOrEmpty(tl.TL_01))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("501-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 502-F La longitud del dato Clave de Usuario debe ser de 10 posiciones
                if (tl.TL_01.Length != 10)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("502-F", tl.Validaciones, tl.TL_01, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Nombre del Usuario
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_02(ValidationResults results)
            {

                // 503-F El dato Nombre del Usuario es requerido
                if (string.IsNullOrEmpty(tl.TL_02))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("503-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 504-F La longitud del dato Nombre del Usuario es mayor a lo permitido (16 posiciones)
                if (tl.TL_02.Length > 16)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("504-F", tl.Validaciones, tl.TL_02, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Numero de Cuenta o Credito Actual
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_04(ValidationResults results)
            {

                // 505-F El dato Numero de Cuenta o Credito es requerido
                if (string.IsNullOrEmpty(tl.TL_04))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("505-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 506-F La longiud del dato Numero de Cuenta o Credito es mayor a lo permitido (25 posiciones)
                if (tl.TL_04.Length > 25)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("506-F", tl.Validaciones, tl.TL_04, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }
        
            /// <summary>
            /// Tipo de Responsabilidad de la Cuenta
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_05(ValidationResults results)
            {

                // 507-F El dato Tipo de Responsabilidad de la Cuenta es requerido
                if (string.IsNullOrEmpty(tl.TL_05))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("507-F", tl.Validaciones, tl.TL_05, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 508-F La longitud del dato Tipo de Responsabilidad de la Cuenta debe ser de 1 posicion
                if (tl.TL_05.Length != 1)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("508-F", tl.Validaciones, tl.TL_05, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 509-F El dato Tipo de Responsabilidad de la Cuenta no se encontro en el catalogo correspondiente
                if (tl.TL_05 != "I" && tl.TL_05 != "J" && tl.TL_05 != "C")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("509-F", tl.Validaciones, tl.TL_05, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Tipo de Cuenta
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_06(ValidationResults results)
            {

                // 510-F El dato Tipo de Cuenta es requerido
                if (string.IsNullOrEmpty(tl.TL_06))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("510-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 511-F La longitud del dato Tipo de Cuenta debe ser de 1 posicion
                if (tl.TL_06.Length != 1)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("511-F", tl.Validaciones, tl.TL_06, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 512-F El dato Tipo de Cuenta no se localizo en el catalogo correspondiente
                if (tl.TL_05 != "I" && tl.TL_05 != "M" && tl.TL_05 != "O" && tl.TL_05 != "R")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("512-F", tl.Validaciones, tl.TL_06, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Tipo de Contrato o Producto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_07(ValidationResults results)
            {

                // 513-F El dato Tipo de Contrato es requerido
                if (string.IsNullOrEmpty(tl.TL_07))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("513-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 514-F La longitud del dato Tipo de Contrato debe ser de 2 posiciones
                if (tl.TL_07.Length != 2)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("514-F", tl.Validaciones, tl.TL_07, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 515-F El dato Tipo de Contrato no se localizo en el catalogo correspondiente

                tl.Correctos++;
            }

            /// <summary>
            /// Moneda del Credito
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_08(ValidationResults results)
            {

                // 516-F El dato Moneda del Credito es requerido
                if (string.IsNullOrEmpty(tl.TL_08))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("516-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 517-F La longitud del dato Moneda del Credito debe ser de 2 posiciones
                if (tl.TL_08.Length != 2)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("517-F", tl.Validaciones, tl.TL_08, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 518-F El dato Moneda del Credito no se localizo en el catalogo correspondiente
                if (tl.TL_08 != "MX" && tl.TL_05 != "UD" && tl.TL_05 != "US")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("518-F", tl.Validaciones, tl.TL_08, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Importe del Avaluo
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_09(ValidationResults results)
            {

                if (string.IsNullOrEmpty(tl.TL_09))
                {
                    return;
                }

                // 519-F La longitud del dato Importe del Avaluo es mayor a lo permitido (9 posiciones)
                if (tl.TL_09.Length > 9)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("519-F", tl.Validaciones, tl.TL_09, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Numero de Pagos
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_10(ValidationResults results)
            {

                // 520-F El dato Numero de Pagos es requerido para creditos de Pagos Fijos (I) o Hipotecarios (M)
                if (string.IsNullOrEmpty(tl.TL_10) && (tl.TL_06 == "I" || tl.TL_06 == "H") )
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("520-F", tl.Validaciones, tl.TL_10, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 521-F La longitud de dato Numero de Pagos es mayor de lo permitido (4 posiciones)
                if (tl.TL_10.Length > 4)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("521-F", tl.Validaciones, tl.TL_10, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 522-F Regla de Rechazo #13

                tl.Correctos++;
            }

            /// <summary>
            /// Frecuencia de Pagos
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_11(ValidationResults results)
            {

                // 523-F El dato Fecuencia de Pagos es requerido
                if (string.IsNullOrEmpty(tl.TL_11))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("523-F", tl.Validaciones, tl.TL_11, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 524-F La longitud del dato Frecuencia de Pagos debe ser de 1 posicion
                if (tl.TL_11.Length != 1)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("524-F", tl.Validaciones, tl.TL_11, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 525-F El dato Frecuencia de Pagos no se localizo en el catalogo correspondiente
                if (tl.TL_11 != "B" &&
                    tl.TL_11 != "D" &&
                    tl.TL_11 != "H" &&
                    tl.TL_11 != "K" &&
                    tl.TL_11 != "M" &&
                    tl.TL_11 != "P" &&
                    tl.TL_11 != "Q" &&
                    tl.TL_11 != "S" &&
                    tl.TL_11 != "V" &&
                    tl.TL_11 != "W" &&
                    tl.TL_11 != "Y" &&
                    tl.TL_11 != "Z" )
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("525-F", tl.Validaciones, tl.TL_11, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 526-F (Warning) Solo los creditos Tipo Revolvente (R) y Sin limite Preestablecido (O) deben tener Frecuencia de Pago (Z)
                if (tl.TL_11 == "Z" && (tl.TL_06 == "R" || tl.TL_06 == "O"))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("526-F", tl.Validaciones, tl.TL_11, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Monto a Pagar
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_12(ValidationResults results)
            {

                // 527-F El dato Monto a Pagar es requerido
                if (string.IsNullOrEmpty(tl.TL_11))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("527-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 528-F El dato Monto a Pagar no puede ser negativo
                double MontoPagar = Parser.ToDouble(tl.TL_12);
                if (MontoPagar < 0)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("528-F", tl.Validaciones, "Monto a Pagar: " + tl.TL_12.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 529-F La longitud del dato Monto a Pagar es mayor a lo permitido (9 posiciones)
                if (tl.TL_12.ToString().Length > 9)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("529-F", tl.Validaciones, tl.TL_12.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Fecha de Apertura de Cuenta o Credito
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_13(ValidationResults results)
            {

                // 530-F La Fecha de Apertura de Cuenta es requerida
                if (string.IsNullOrWhiteSpace(tl.TL_13))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("530-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 531-F La longitud del dato Fecha de Apertura de Cuenta debe ser de 8 digitos
                if (tl.TL_13.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("531-F", tl.Validaciones, tl.TL_13, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }
                
                // 532-F El formato de la Fecha de Apertura de Cuenta no es correcto (Formato correcto DDMMYYYY)
                Regex re = new Regex(WebConfig.PatronDeFecha);
                if (!re.IsMatch(tl.TL_13))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("532-F", tl.Validaciones, tl.TL_13, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }
                
                // 533-F La Fecha de Apertura de la Cuenta no puede ser mayor a la fecha de reporte de la informacion

                // 534-F Regla de la circular 7/2014

                tl.Correctos++;
            }

            /// <summary>
            /// Fecha de Ultimo Pago
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_14(ValidationResults results)
            {

                // 535-F El dato Fecha de Ultimo Pago es requerido
                if (string.IsNullOrWhiteSpace(tl.TL_14)  && string.IsNullOrWhiteSpace(tl.TL_15))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("535-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // Si se reporto el dato de Fecha de Ultimo Pago, lo validamos
                if (!string.IsNullOrWhiteSpace(tl.TL_14)) 
                {
                    // 536-F La longitud del dato Fecha de Ultimo Pago debe ser de 8 posiciones
                    if (tl.TL_14.Length != 8)
                    {
                        results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("536-F", tl.Validaciones, tl.TL_14, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        return;
                    }

                    // 537-F El formato de la Fecha de Ultimo Pago no es correcto (Formato correcto DDMMYYYY)
                    Regex re = new Regex(WebConfig.PatronDeFecha);
                    if (!re.IsMatch(tl.TL_14))
                    {
                        results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("537-F", tl.Validaciones, tl.TL_14, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        return;
                    }
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Fecha de Ultima Compra o Disposicion
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_15(ValidationResults results)
            {

                // 538-F El dato Fecha de Ultima Compra es requerido
                if (string.IsNullOrWhiteSpace(tl.TL_15) && string.IsNullOrWhiteSpace(tl.TL_14))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("538-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // Si se reporto el dato de Fecha de Ultima Compra, lo validamos
                if (!string.IsNullOrWhiteSpace(tl.TL_15))
                {
                    // 539-F La longitud de dato Fecha de Ultmina Compra debe ser de 8 posiciones
                    if (tl.TL_15.Length != 8)
                    {
                        results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("539-F", tl.Validaciones, tl.TL_15, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        return;
                    }

                    // 540-F El formato de la Fecha de Ultima Compra no es correcta (Formato correcto DDMMYYYY)
                    Regex re = new Regex(WebConfig.PatronDeFecha);
                    if (!re.IsMatch(tl.TL_15))
                    {
                        results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("540-F", tl.Validaciones, tl.TL_15, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        return;
                    }
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Fecha de Cierre
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_16(ValidationResults results)
            {

                if (string.IsNullOrWhiteSpace(tl.TL_16))
                {
                    return;
                }

                // 541-F La longitud del dato Fecha de Cierre debe ser de 8 posiciones
                if (tl.TL_16.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("541-F", tl.Validaciones, tl.TL_16, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 542-F El formato de la Fecha de Cierre no es correcto (Formato correcto DDMMYYYY)
                Regex re = new Regex(WebConfig.PatronDeFecha);
                if (!re.IsMatch(tl.TL_16))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("542-F", tl.Validaciones, tl.TL_16, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Fecha de Reporte de Informacion
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_17(ValidationResults results)
            {

                // 543-F El dato Fecha de Reporte de Informacion es requerido
                if (string.IsNullOrWhiteSpace(tl.TL_17))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("543-F", tl.Validaciones, tl.TL_17, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 544-F La longitud del dato Fecha de Reporte de Informacion debe ser de 8 posiciones
                if (tl.TL_17.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("544-F", tl.Validaciones, tl.TL_17, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 545-F El formato de la Fecha de Reporte de Informacion no es correcto (Formato correcto DDMMYYYY)
                Regex re = new Regex(WebConfig.PatronDeFecha);
                if (!re.IsMatch(tl.TL_17))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("545-F", tl.Validaciones, tl.TL_17, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Garantia
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_20(ValidationResults results)
            {

                if (string.IsNullOrEmpty(tl.TL_20))
                {
                    return;
                }

                // 546-F La longitud del dato Garantia es mayor a lo permitido (40 posiciones)
                if (tl.TL_20.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("546-F", tl.Validaciones, tl.TL_20, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Credito Maximo Autorizado
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_21(ValidationResults results)
            {

                // 547-F El dato Credito Maximo Autorizado es requerido
                if (string.IsNullOrEmpty(tl.TL_21))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("547-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 548-F El dato Credito Maximo Autorizado no puede ser negativo o Cero
                double CreditoMaximoAutorizado = Parser.ToDouble(tl.TL_21);
                if (CreditoMaximoAutorizado <= 0)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("548-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 549-F La longitud del dato Credito Maximo Autorizado es mayor de lo permitido (9 posiciones)
                if (tl.TL_21.ToString().Length > 9)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("549-F", tl.Validaciones, tl.TL_21.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Saldo Actual
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_22(ValidationResults results)
            {
                
                // 550-F El dato Saldo Actual es requerido si se cuenta con el
                if (string.IsNullOrEmpty(tl.TL_21))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("550-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 551-F El Saldo Actual no puede ser negativo o Cero
                double SaldoActual = Parser.ToDouble(tl.TL_22);
                if (SaldoActual <= 0)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("551-F", tl.Validaciones, "Saldo Actual: " + tl.TL_22.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 552-F La longitud del dato Saldo Actual  es mayor de lo permitido (9 posiciones)
                if (tl.TL_22.ToString().Length > 9)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("552-F", tl.Validaciones, tl.TL_22.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Limite de Credito
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_23(ValidationResults results)
            {

                // 553-F El dato Limite de Credito es requerido para creditos Revolventes (R)
                if (string.IsNullOrEmpty(tl.TL_23) && tl.TL_06[0] == 'R')
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("553-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 554-F La longitud del dato Limite de Credito es mayor a lo permitido (9 posiciones)
                if (tl.TL_23.ToString().Length > 9 && tl.TL_06[0] == 'R')
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("554-F", tl.Validaciones, "Limite Credito: " + tl.TL_23.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Saldo Vencido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_24(ValidationResults results)
            {


                // 552-F El dato Saldo Vencido es requerido en caso de incumplimiento del cliente (MOP mayor a 01)

                ////Double? SaldoVencido = Parser.ToDouble(tl.TL_24);
                ////if (!SaldoVencido.HasValue || Parser.ToNumber(SaldoVencido) <= 0)
                ////{
                ////    if (tl.TL_16 == "00000000")
                ////    {
                ////        if (!string.IsNullOrEmpty(tl.TL_26))
                ////        {
                ////            if (!(tl.TL_26 == "UR" || tl.TL_26 == "00" || tl.TL_26 == "01"))
                ////            {
                ////                //TMR. verificar que aqui se cumpla la regla req 14.
                ////                // Se cambia el mensaje a : la forma de pago no es valida
                ////                // results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("134-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                ////                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("139-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                ////                return;
                ////            }
                ////        }
                ////    }
                ////}

                ////if (SaldoVencido.HasValue && Parser.ToNumber(SaldoVencido) > 0)
                ////{
                ////    if (tl.TL_16 != "00000000")
                ////    {
                ////        //TMR. verificar que aqui se cumpla la regla req 14.- aquí si se cumple
                ////        results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("134-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                ////        return;
                ////    }
                ////}


                // 555-F La longitud del dato Saldo Vencido es mayor a lo permitido (9 posiciones)
                if (tl.TL_24.Length > 9)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("555-F", tl.Validaciones, tl.TL_24.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Numero de Pagos Vencidos
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_25(ValidationResults results)
            {
                
                ////if (string.IsNullOrEmpty(tl.TL_25))
                ////{
                ////    if (!string.IsNullOrEmpty(tl.TL_26))
                ////    {
                ////        if ((tl.TL_26 == "UR" || tl.TL_26 == "00" || tl.TL_26 == "99") && Parser.ToNumber(tl.TL_25) != 0)
                ////        {
                ////            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("136-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                ////            return;
                ////        }

                ////        if ((tl.TL_26 != "UR" || tl.TL_26 != "00" || tl.TL_26 != "99") && Parser.ToNumber(tl.TL_25) <= 0)
                ////        {
                ////            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("137-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                ////            return;
                ////        }
                ////    }
                ////    else
                ////        return;
                ////}

                // 556-F La longitud del dato Numero de Pagos Vencidos es mayor a lo permitido (4 posiciones)
                if (tl.TL_25.Length > 4)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("556-F", tl.Validaciones, tl.TL_25.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Forma de Pago Actual (MOP)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_26(ValidationResults results)
            {

                // 557-F El dato Forma de Pago Actual es requerido
                if (string.IsNullOrEmpty(tl.TL_26))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("557-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 558-F La longitu del dato Forma de Pago debe ser de 2 posiciones
                if (tl.TL_26.Length != 2)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("558-F", tl.Validaciones, tl.TL_26, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Clave de Observacion
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_30(ValidationResults results)
            {

                if (string.IsNullOrEmpty(tl.TL_30))
                {
                    return;
                }

                // 559-F La longitud del dato Clave de Observacion debe ser de 2 posiciones
                if (tl.TL_30.Length != 2)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("559-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 560-F El dato Clave de Observacion no se encontro en el catalogo correspondiente

                tl.Correctos++;
            }

            /// <summary>
            /// Clave del Usuario Anterior
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_39(ValidationResults results)
            {

                if (string.IsNullOrEmpty(tl.TL_39))
                {
                    return;
                }

                // 561-F La longitud dato Clave del Usuario Anterior debe ser de 10 posiciones
                if (tl.TL_39.Length != 10)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("561-F", tl.Validaciones, tl.TL_39, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Nombre del Usuario Anterior
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_40(ValidationResults results)
            {

                if (string.IsNullOrEmpty(tl.TL_40))
                {
                    return;
                }

                // 562-F La longitud del dato Nombre del Usuario Anterior es mayor a lo permitido (16 posiciones)
                if (tl.TL_40.Length > 16)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("562-F", tl.Validaciones, tl.TL_40, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Numero de Cuenta Anterior
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_41(ValidationResults results)
            {
                if (string.IsNullOrEmpty(tl.TL_41) )
                {
                    return;
                }

                // 563-F La longitud del dato Numero de Cuenta Anterior es mayor a lo permitido (25 posiciones)
                if (tl.TL_41.Length >  25)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("563-F", tl.Validaciones, tl.TL_41, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Fecha de Primer Incumplimiento
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_43(ValidationResults results)
            {

                // 564-F El dato Fecha de Primer Incumplimiento es requerido
                if (string.IsNullOrEmpty(tl.TL_43))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("564-F", tl.Validaciones, tl.TL_43, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 565-F La longitud del dato Fecha de Primer Incumplimiento debe ser de 8 posiciones
                if (tl.TL_43.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("565-F", tl.Validaciones, tl.TL_43, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 566-F El formato de la Fecha de Primer Incumplimiento es incorrecto (formato correcto DDMMYYYY)
                 Regex re = new Regex(WebConfig.PatronDeFecha);
                 if (!re.IsMatch(tl.TL_43))
                 {
                     results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("566-F", tl.Validaciones, tl.TL_43, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                     return;
                 }

                 tl.Correctos++;
            }

            /// <summary>
            /// Saldo Insoluto del Principal
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_44(ValidationResults results)
            {

                // 567-F El dato Saldo Insoluto del Principal es requerido
                if (string.IsNullOrEmpty(tl.TL_44))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("567-F", tl.Validaciones, tl.TL_44, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 568-F La longitud del dato Saldo Insoluto del Principal es mayor a lo permitido (10 posiciones)
                if (tl.TL_44.Length > 10)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("568-F", tl.Validaciones, tl.TL_44, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Monto de Ultimo Pago
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_45(ValidationResults results)
            {

                // 569-F El dato Monto de Ultimo Pago es requerido
                if (string.IsNullOrEmpty(tl.TL_45))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("569-F", tl.Validaciones, tl.TL_45, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 570-F La longitud del dato Monto de Ultimo Pago es mayor a lo permitido (9 posiciones)
                if (tl.TL_45.Length > 9)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("570-F", tl.Validaciones, tl.TL_45, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Plazo en Meses
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_50(ValidationResults results)
            {

                // 571-F El dato Plazo en Meses es requerido
                if (string.IsNullOrEmpty(tl.TL_50) && tl.TL_06 != "R" && tl.TL_06 != "O")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("571-F", tl.Validaciones, tl.TL_50, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 572-F La longitud del dato Plazo en Meses es mayor a lo permitido (6 posiciones)
                if (tl.TL_50.Length > 6)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("572-F", tl.Validaciones, tl.TL_50, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Monto de Credito a la Originacion
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_51(ValidationResults results)
            {

                // 573-F El dato Monto de Credito a la Originacion es requerido
                if (string.IsNullOrEmpty(tl.TL_50) && tl.TL_06 == "R" && tl.TL_06 == "O" && tl.TL_06 == "O")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("573-F", tl.Validaciones, tl.TL_51, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                // 574-F La longitud del dato Monto de Credito a la Originacion es mayor a lo permitido (9 posiciones)
                if (tl.TL_51.Length > 9)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("574-F", tl.Validaciones, tl.TL_51, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                }

                tl.Correctos++;
            }

            /// <summary>
            /// Ultrasist 2021/10/13 campos telefono y mail obligatorios
            /// Correo electronico del consumidor obligatorio
            /// </summary>
            /// <param name="results"></param>
            //[SelfValidation]
            //public void V_52(ValidationResults results)
            //{
                //Ultrasist 2021/10/13 campos telefono y mail obligatorios
                // 575-F Correo electronico del consumidor es requerido
                //if (string.IsNullOrEmpty(tl.TL_52))
                //{
                //    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("575-F", tl.Validaciones, tl.TL_52, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                //    return;
                //}
                //Ultrasist 2021/10/13 campos telefono y mail obligatorios
                // 576-F La longitud del Correo electronico del consumidor es 99 posiciones maximo
            //    if (tl.TL_52.Length > 99)
            //    {
            //        results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("576-F", tl.Validaciones, tl.TL_52, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
            //        return;
            //    }

            //    tl.Correctos++;
            //}

            /// <summary>
            /// Fin del Segmento TL
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_99(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

        #endregion Metodos Validadores

        #region Metodos Validadores - Reglas Especificas de Validacion y Rechazo

            /// <summary>
            /// Regla 01 - Cuenta al Corriente con Saldo Vencido > 0
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla01(ValidationResults results)
            {
                // Saldo Vencido > 0 y
                // MOP = 01, 00 o UR y
                // Clave de Observación <> "AD"
                Double SaldoVencido = Parser.ToDouble(tl.TL_24);
                string FormaPagoMOP = tl.TL_26;
                string ClaveObservacion = tl.TL_30;
                
                if (SaldoVencido > 0 && (FormaPagoMOP == "00" || FormaPagoMOP == "01" || FormaPagoMOP == "UR") && ClaveObservacion != "AD")
                {
                    File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "--------ValidacionRegla01------------para " + tl.TL_04 + "\n");
                    File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "saldoVencido >0 y FormaPago(00,01,UR) y Cve != AD " + "\n");
                    File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "SaldoVencido: " + tl.TL_24 + " FormaPago: " + tl.TL_26 + " ClaveObser" + tl.TL_30 + "\n");
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R01-F", tl.Validaciones, "Saldo Vencido: " + SaldoVencido.ToString() + " Forma de Pago: " + FormaPagoMOP + " Clave de Observacion: " + ClaveObservacion, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 02 - Cuenta con Atraso y con Saldo Actual = 0
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla02(ValidationResults results)
            {
                // Saldo Actual = 0 y
                // MOP = 02, 03, 04, 05, 06, 07, 96, 97 y
                // Sin Clave de Observación

                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                string FormaPagoMOP = tl.TL_26;
                string ClaveObservacion = tl.TL_30;

                if (SaldoActual == 0 && (FormaPagoMOP == "02" || FormaPagoMOP == "03" || FormaPagoMOP == "04" || FormaPagoMOP == "05" || FormaPagoMOP == "06" || FormaPagoMOP == "07" || FormaPagoMOP == "96" || FormaPagoMOP == "97") && string.IsNullOrEmpty(ClaveObservacion))
                {
                    StreamWriter fichero;
                    fichero = File.AppendText(@"c:\a\errorsPN_PROCESO.txt");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "--------ValidacionRegla02------------para " + tl.TL_04 + "\n");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "saldoActual =0 y FormaPago(02,03,04,05,06,07,96,97) y Cve = null " + "\n");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual: " + tl.TL_22 + " FormaPago: " + tl.TL_26 + " ClaveObser" + tl.TL_30 + "\n");
                    fichero.Close();

        /*fichero = File.AppendText("prueba2.txt");
        fichero.WriteLine("Segunda línea");
        fichero.Close();*/

                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R02-F", tl.Validaciones, "Saldo Actual: " + SaldoActual.ToString() + " Forma de Pago: " + FormaPagoMOP + " Clave de Observacion: " + ClaveObservacion, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
                    
            }

            /// <summary>
            /// Regla 03 - Cuenta con Atraso y con Saldo Vencido = 0
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla03(ValidationResults results)
            {
                // Saldo Vencido = 0 y
                // MOP = 02, 03, 04, 05, 06, 07, 96, 97 y
                // Sin Clave de Observación

                Double SaldoVencido = Parser.ToDouble(tl.TL_24);
                string FormaPagoMOP = tl.TL_26;
                string ClaveObservacion = tl.TL_30;

                if (SaldoVencido == 0 && (FormaPagoMOP == "02" || FormaPagoMOP == "03" || FormaPagoMOP == "04" || FormaPagoMOP == "05" || FormaPagoMOP == "06" || FormaPagoMOP == "07" || FormaPagoMOP == "96" || FormaPagoMOP == "97") && string.IsNullOrEmpty(ClaveObservacion))
                {
                    StreamWriter fichero;
                    fichero = File.AppendText(@"c:\a\errorsPN_PROCESO.txt");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "--------ValidacionRegla03------------ para" + tl.TL_04 + "\n");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "saldoVencido =0 y FormaPago(02,03,04,05,06,07,96,97) y Cve = null " + "\n");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "SaldoVencido: " + tl.TL_24 + " FormaPago: " + tl.TL_26 + " ClaveObser" + tl.TL_30 + "\n");
                    fichero.Close();

                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R03-F", tl.Validaciones, "Saldo Vencido: " + SaldoVencido.ToString() + " Forma de Pago: " + FormaPagoMOP + " Clave de Observacion: " + ClaveObservacion, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                } 
                
            }

            /// <summary>
            /// Regla 04 - Cuenta Abierta sin movimiento y con Saldos Actual = 0 y Saldo Vencido = 0 y Monto a Pagar = 0 Para cuentas diferentes a Sin limite preestablecido (O)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla04(ValidationResults results)
            {
                // Saldo Actual = 0 y
                // Saldo Vencido = 0 y
                // Límite de Crédito = 0 y
                // Monto a Pagar = 0 y
                // Tipo de Cuenta <> "O" y
                // Sin Fecha de Cierre y
                // Sin Fecha de Ultimo Pago y
                // Sin Fecha de Ultima Compra

                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double LimiteCredito = Parser.ToDouble(tl.TL_23);
                Double SaldoVencido = Parser.ToDouble(tl.TL_24);
                string TipoCuenta = tl.TL_06;
                string FechaCierre = tl.TL_16;
                string FechaUltimoPago = tl.TL_14;
                string FechaUltimaCompra = tl.TL_15;

                if (SaldoActual == 0 && LimiteCredito == 0 && SaldoVencido == 0 && TipoCuenta != "O" && string.IsNullOrWhiteSpace(FechaCierre) && string.IsNullOrWhiteSpace(FechaUltimoPago) && string.IsNullOrWhiteSpace(FechaUltimaCompra))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R04-F", tl.Validaciones, " Saldo Actual: " + SaldoActual.ToString() + " Limite Credito: " + LimiteCredito.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 05 - Cuenta sin Fecha de Apertura o inválida
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla05(ValidationResults results)
            {
                // Fecha de Apertura <= "01/01/1900" (valor por default) o
                // Sin Fecha de Apertura o en blanco o
                // Fecha de Apertura > Fecha de Reporte

                string FechaAperura = tl.TL_13;
                string FechaReporte = tl.TL_17;

                if (FechaAperura == "19000101" || string.IsNullOrWhiteSpace(FechaAperura) || Parser.ToDateTime(FechaAperura).Subtract(Parser.ToDateTime(FechaReporte)).Days > 0)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R05-F", tl.Validaciones, "Fecha de Apertura:" + FechaAperura + " Fecha de Reporte: " + FechaReporte, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
                                   
            }

            /// <summary>
            /// Regla 06 - Cuenta con Fecha de Apertura mayor a 3 meses y MOP 00
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla06(ValidationResults results)
            {
                // MOP = 00 y
                // Fecha de Reporte y Fecha de Apertura reportadas y
                // Fecha de Reporte – Fecha de Apertura >= 90 días

                string FormaPagoMOP = tl.TL_26;
                string FechaReporte = tl.TL_17;
                string FechaApertura = tl.TL_13;

                if (FormaPagoMOP == "00" && !string.IsNullOrWhiteSpace(FechaReporte) && !string.IsNullOrWhiteSpace(FechaApertura) && Parser.ToDateTime(FechaReporte).Subtract(Parser.ToDateTime(FechaApertura)).Days >= 90)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R06-F", tl.Validaciones, "Forma de Pago:" + FormaPagoMOP + " Fecha de Reporte: " + FechaReporte + " Fecha Apertura: " + FechaApertura, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
                    
            }

            /// <summary>
            /// Regla 07 - Límite de Crédito menor o igual a 0 para Cuentas Revolventes (R)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla07(ValidationResults results)
            {
                // Límite de Crédito <= 0 y
                // Tipo de Cuenta = "R" y
                // Sin Fecha de Cierre

                Double LimiteCredito = Parser.ToDouble(tl.TL_23);
                string TipoCuenta = tl.TL_06;
                string FechaCierre = tl.TL_16;

                if (LimiteCredito <= 0 && TipoCuenta == "R" && !string.IsNullOrWhiteSpace(FechaCierre))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R07-F", tl.Validaciones, " Limite Credito: " + LimiteCredito.ToString() + " Tipo de Cuenta: " + TipoCuenta + " Fecha de Cierre: " + FechaCierre, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                } 

            }

            /// <summary>
            /// Regla 08 - Cuentas con Saldo Vencido mayor Saldo Actual
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla08(ValidationResults results)
            {
                // Saldo Vencido >= 0 y
                // Saldo Actual >= 0 y
                // Saldo Vencido > Saldo Actual y
                // Sin Clave de Observación reportada

                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double SaldoVencido = Parser.ToDouble(tl.TL_24);
                string ClaveObservacion = tl.TL_30;

                if (SaldoVencido >= 0 && SaldoActual >= 0 && SaldoVencido > SaldoActual && string.IsNullOrWhiteSpace(ClaveObservacion))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R08-F", tl.Validaciones, "Saldo Actual: " + SaldoActual.ToString() + " Saldo Vencido: " + SaldoVencido.ToString() + " Clave Observacion: " + ClaveObservacion, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 09 - Cuenta Cerrada con Saldos Inconsistentes
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla09(ValidationResults results)
            {
                // Fecha de Cierre reportada y
                // (Saldo Vencido <> 0 ó Saldo Actual <> 0 ó Monto a Pagar <> 0) y
                // Sin Clave de Observación

                string FechaCierre = tl.TL_16;
                string ClaveObservacion = tl.TL_30;
                Double MontoPagar = Parser.ToDouble(tl.TL_12);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double SaldoVencido = Parser.ToDouble(tl.TL_24);

                if (!string.IsNullOrWhiteSpace(FechaCierre) && (SaldoVencido != 0 || SaldoActual != 0 || MontoPagar != 0) && string.IsNullOrWhiteSpace(ClaveObservacion))
                {
                    string Mensaje = "Fecha de Cierre: " + FechaCierre + " Saldo Vencido: " + SaldoVencido + " Saldo Actual: " + SaldoActual + " Monto a Pagar: " + MontoPagar + "Clave de Observacion: " + ClaveObservacion;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R09-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 10 - Cuenta Abierta Revolvente (R) ó Sin Límite Preestablecido (O) con Saldo Actual > 0 y Monto a Pagar = 0
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla10(ValidationResults results)
            {
                // Sin Fecha de Cierre reportada y
                // Tipo de Cuenta = "R", "O" y
                // Saldo Actual > 0 y
                // Monto a Pagar < 0 y
                // Fecha de Apertura - Fecha de Reporte >= 31 días y
                // Fecha de Reporte – Fecha de Ultima Compra >= 31 días y
                // Monto a Pagar <= 0 y
                // (Sin Fecha de Ultimo Pago ó Fecha de Reporte – Fecha de Ultimo Pago > 30 días) y
                // Clave de Observación <> "IA"

                string TipoCuenta = tl.TL_06;
                string FechaApertura = tl.TL_13;
                string FechaUltimoPago = tl.TL_14;
                string FechaUltimaCompra = tl.TL_15;
                string FechaCierre = tl.TL_16;
                string FechaReporte = tl.TL_17;
                string ClaveObservacion = tl.TL_30;
                Double MontoPagar = Parser.ToDouble(tl.TL_12);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);

                if (string.IsNullOrWhiteSpace(FechaCierre) &&
                     (TipoCuenta == "R" || TipoCuenta == "O") &&
                     SaldoActual > 0 &&
                     MontoPagar < 0 &&
                     Parser.ToDateTime(FechaApertura).Subtract(Parser.ToDateTime(FechaReporte)).Days >= 31 &&
                     Parser.ToDateTime(FechaReporte).Subtract(Parser.ToDateTime(FechaUltimaCompra)).Days >= 31 &&
                     MontoPagar <= 0 &&
                     (string.IsNullOrWhiteSpace(FechaUltimoPago) || Parser.ToDateTime(FechaReporte).Subtract(Parser.ToDateTime(FechaUltimoPago)).Days > 30) &&
                     ClaveObservacion != "IA"
                   )
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R10-F", tl.Validaciones, tl.TL_06, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
                    
            }

            /// <summary>
            /// Regla 11 - Crédito Revolvente (R) o Sin Límite Preestablecido (O) y Fecha de Ultimo Pago ó Fecha de Compra mayor a Fecha de Apertura
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla11(ValidationResults results)
            {
                // Tipo de cuenta = "R", "O" y
                // Con Fecha de Ultimo Pago reportada y Fecha de Apertura > Fecha de Ultimo Pago ó
                // Con Fecha de Ultima Compra y Fecha de Apertura > Fecha de Ultima Compra ó
                // Con Fecha de Cierre y Fecha de Apertura > Fecha de Cierre

                string TipoCuenta = tl.TL_06;
                string FechaApertura = tl.TL_13;
                string FechaUltimoPago = tl.TL_14;
                string FechaUltimaCompra = tl.TL_15;
                string FechaCierre = tl.TL_16;

                if ((TipoCuenta == "R" || TipoCuenta == "O") && (
                     (!string.IsNullOrWhiteSpace(FechaUltimoPago) && Parser.ToDateTime(FechaApertura) > Parser.ToDateTime(FechaUltimoPago)) ||
                     (!string.IsNullOrWhiteSpace(FechaUltimaCompra) && Parser.ToDateTime(FechaApertura) > Parser.ToDateTime(FechaUltimaCompra)) ||
                     (!string.IsNullOrWhiteSpace(FechaCierre) && Parser.ToDateTime(FechaApertura) > Parser.ToDateTime(FechaCierre))
                   ))
                {
                    string Mensaje = "Tipo de Cuenta: " + TipoCuenta + " Fecha Ultimo Pago: " + FechaUltimoPago + "Fecha Apertura: " + FechaApertura + " Fecha Ultima Compra: " + FechaUltimaCompra + " Fecha Cierre: " + FechaCierre;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R11-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
                    
            }

            /// <summary>
            /// Regla 12 - Monto a Pagar > 0 y Saldo Actual = 0
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla12(ValidationResults results)
            {
                // Saldo Actual <= 0 y
                // Monto a Pagar > 0 y
                // Fecha de Reporte – Fecha de Ultimo Pago >= 31

                Double MontoPagar = Parser.ToDouble(tl.TL_12);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                string FechaReporte = tl.TL_17;
                string FechaUltimoPago = tl.TL_14;
                int DiasDiferencia = Parser.ToDateTime(FechaReporte).Subtract(Parser.ToDateTime(FechaUltimoPago)).Days;

                if (SaldoActual <= 0 && MontoPagar > 0 &&  DiasDiferencia >= 31)
                {
                    StreamWriter fichero;
                    fichero = File.AppendText(@"c:\a\errorsPN_PROCESO.txt");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "--------ValidacionRegla12------------ para" + tl.TL_04 + "\n");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual <=0 y MontoPagar > 0  y DiasDiff > = 31 " + "\n");
                    fichero.WriteLine(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual: " + tl.TL_22 + " MontoPagar: " + tl.TL_12 + " DiffFechas" + DiasDiferencia + "\n");
                    fichero.Close();

                    //File.AppendText(@"c:\a\errorsPN_PROCESO.txt", "--------ValidacionRegla12------------ para" + tl.TL_04 + "\n");
                    //File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual <=0 y MontoPagar > 0  y DiasDiff > = 31 " + "\n");
                    //File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual: " + tl.TL_22 + " MontoPagar: " + tl.TL_12 + " DiffFechas" + DiasDiferencia + "\n");
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R12-F", tl.Validaciones, "Saldo: " + tl.TL_12 + " Monto: " + tl.TL_22 + " Dias: " + DiasDiferencia.ToString() , tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                } 

            }

            /// <summary>
            /// Regla 13 - Crédito Hipotecario (M) ó Crédito de Pago Fijo (I) con Saldo Actual > 0 y Monto a Pagar = 0
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla13(ValidationResults results)
            {
                // Sin Fecha de Cierre y
                // Saldo Actual > 0 y
                // Tipo de Cuenta = "M", "I" y
                // Tipo de Contrato <> "LR" (Línea de crédito reinstalable) y
                // ((Monto a Pagar <= 0 y Sin Fecha de Ultimo Pago) ó (Monto a Pagar <=0 y Fecha de Reporte – Fecha de Ultimo Pago > 30 días) ó Número de Pagos <= 0)

                string TipoCuenta = tl.TL_06;
                string TipoContrato = tl.TL_07;
                string FechaUltimoPago = tl.TL_14;
                string FechaCierre = tl.TL_16;
                string FechaReporte = tl.TL_17;
                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double MontoPagar = Parser.ToDouble(tl.TL_12);
                int NumeroPagos = Convert.ToInt32(tl.TL_10);

                if ( string.IsNullOrWhiteSpace(FechaCierre) &&
                     SaldoActual > 0 &&
                     (TipoCuenta == "M" || TipoCuenta == "I") &&
                     TipoCuenta != "LR" &&
                     ((MontoPagar <= 0 && !string.IsNullOrWhiteSpace(FechaUltimoPago)) ||
                       (MontoPagar <= 0 && Parser.ToDateTime(FechaReporte).Subtract(Parser.ToDateTime(FechaUltimoPago)).Days > 30) ||
                       NumeroPagos <= 0
                     )
                    )
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R13-F", tl.Validaciones, tl.TL_06, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 14 - Crédito Revolvente (R) o Sin Límite Preestablecido (O) y Fecha de Pago o Fecha de Ultima Compra incongruente (futura al reporte o cierre).
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla14(ValidationResults results)
            {
                // Tipo de Cuenta = "R", "O" y
                // (Con Fecha de Ultimo Pago y
                // (Fecha de Ultimo Pago – Fecha de Reporte > 15 días ó
                // (Con Fecha de Cierre y
                // Fecha de Ultimo Pago > Fecha de Cierre)))
                // ó
                // (Con Fecha de Ultima Compra y
                // (Fecha de Ultima Compra – Fecha de Reporte > 15 días ó
                // (Con Fecha de Cierre y
                // Fecha de Ultima Compra > Fecha de Cierre)))

                string TipoCuenta = tl.TL_06;
                string FechaUltimoPago = tl.TL_14;
                string FechaUltimaCompra = tl.TL_15;
                string FechaCierre = tl.TL_16;
                string FechaReporte = tl.TL_17;

                if ((TipoCuenta == "R" || TipoCuenta == "O") &&
                     (!string.IsNullOrWhiteSpace(FechaUltimoPago) && (Parser.ToDateTime(FechaUltimoPago).Subtract(Parser.ToDateTime(FechaReporte)).Days > 15 || (!string.IsNullOrWhiteSpace(FechaCierre) && Parser.ToDateTime(FechaUltimoPago) > Parser.ToDateTime(FechaCierre)))) ||
                     (!string.IsNullOrWhiteSpace(FechaUltimaCompra) && (Parser.ToDateTime(FechaUltimaCompra).Subtract(Parser.ToDateTime(FechaReporte)).Days > 15 || (!string.IsNullOrWhiteSpace(FechaCierre) && Parser.ToDateTime(FechaUltimaCompra) > Parser.ToDateTime(FechaCierre))))
                    )
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R14-F", tl.Validaciones, tl.TL_06, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
                   
            }

            /// <summary>
            /// Regla 15 - Fecha de Cierre incongruente (futura con respecto a la de reporte)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla15(ValidationResults results)
            {
                // Fecha de Cierre reportada y
                // Fecha de Cierre – Fecha de Reporte > 15 días

                string FechaCierre = tl.TL_16;
                string FechaReporte = tl.TL_17;

                if (string.IsNullOrWhiteSpace(FechaCierre))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A15-F", tl.Validaciones, tl.TL_16.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                    return;
                } 

                if (Parser.ToDateTime(FechaCierre).Subtract(Parser.ToDateTime(FechaReporte)).Days > 15)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R15-F", tl.Validaciones, tl.TL_16.ToString(), tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }   

            }

            /// <summary>
            /// Regla 16 - Crédito de Pagos Fijos (I) abierta y Saldo Actual = 0 y/o Saldo Vencido = 0 y Tipo de Crédito = "Préstamo de Nómina" o "Reinstalable"
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla16(ValidationResults results)
            {
                // Tipo de cuenta = "I"
                //      Si Tipo de Crédito = "Préstamo de Nómina" o "Crédito Reinstalable"
                //          ( Saldo actual < 0 ó Saldo vencido < 0 ó Monto a pagar < 0 )
                //      Para Tipo de Crédito distinto a "Préstamo de Nómina" o "Crédito Reinstalable"
                //          ( Saldo Actual <= 0 ó Saldo Vencido < 0 ó Monto a Pagar <= 0 ) y
                //          Sin Fecha de Cierre y
                //          Sin Clave de Observación

                string TipoCuenta = tl.TL_06;
                string TipoCredito = tl.TL_07;
                string FechaCierre = tl.TL_16;
                string ClaveObservacion = tl.TL_30;
                Double MontoPagar = Parser.ToDouble(tl.TL_12);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double SaldoVencido = Parser.ToDouble(tl.TL_24);

                if (TipoCuenta == "I")
                {
                    if (TipoCredito == "PN" || TipoCredito == "LR")
                    {
                        if (SaldoActual <= 0 || SaldoVencido < 0 || MontoPagar <= 0)
                        {
                            File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "--------ValidacionRegla16_A------------para " + tl.TL_04 + "\n");
                            File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual <=0 y SaldoVEncido < 0  o MontoPagar <= 0 " + "\n");
                            File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual: " + tl.TL_22 + " MontoPagar: " + tl.TL_12 + " saldoVencido" + tl.TL_24 + "\n");
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A21-F", tl.Validaciones, tl.TL_07, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        }
                    }
                    else
                    {
                        if ((SaldoActual <= 0 || SaldoVencido < 0 || MontoPagar <= 0) && string.IsNullOrWhiteSpace(FechaCierre) && string.IsNullOrEmpty(ClaveObservacion))
                        {
                            File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "--------ValidacionRegla16_B------------para " + tl.TL_04 + "\n");
                            File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual <=0 o SaldoVEncido < 0  o MontoPagar <= 0  y fechaCierre= null y CveObs =null " + "\n");
                            File.AppendAllText(@"c:\a\errorsPN_PROCESO.txt", "SaldoActual: " + tl.TL_22 + " MontoPagar: " + tl.TL_12 + " saldoVencido" + tl.TL_24 + " fechaCierr:" + tl.TL_16 + " Cve:" + tl.TL_30 + "\n");
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R16-F", tl.Validaciones, "Saldo Act: " + tl.TL_22 + " Saldo Venc: " + tl.TL_24 + " Monto: " + tl.TL_12 + " Fecha: " + tl.TL_16 + " Clave: " + tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        }
                    }
                }

            }

            /// <summary>
            /// Regla 17 - Crédito Hipotecario (M) abierto con Saldo Actual = 0 y Saldo Vencido = 0
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla17(ValidationResults results)
            {
                // Tipo de Cuenta = "M" y
                // Sin Fecha de Cierre y
                // Sin Clave de Observación y
                // (Saldo Actual <= 0 ó Monto a Pagar <= 0) y
                // Saldo Vencido <= 0

                string TipoCuenta = tl.TL_06;
                string FechaCierre = tl.TL_16;
                string ClaveObservacion = tl.TL_30;
                Double MontoPagar = Parser.ToDouble(tl.TL_12);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double SaldoVencido = Parser.ToDouble(tl.TL_24);

                if (TipoCuenta == "M" && string.IsNullOrWhiteSpace(FechaCierre) && string.IsNullOrEmpty(ClaveObservacion) && (SaldoActual <= 0 || MontoPagar <= 0) && SaldoVencido <= 0)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R17-F", tl.Validaciones, tl.TL_06, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
                    
            }

            /// <summary>
            /// Regla 37 - Cuenta con atraso y sin Fecha de Primer Incumplimiento
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla37(ValidationResults results)
            {
                // Sin Fecha de Primer Incumplimiento y
                // MOP = 02, 03, 04, 05, 06, 07, 96, 97, 99
                // Clave de Observación <> "IA"

                string FechaIncumplimiento = tl.TL_43;
                string FormaPagoMOP = tl.TL_26;
                string ClaveObservacion = tl.TL_30;

                if (string.IsNullOrWhiteSpace(FechaIncumplimiento) && (FormaPagoMOP == "02" || FormaPagoMOP == "03" || FormaPagoMOP == "04" || FormaPagoMOP == "05" || FormaPagoMOP == "06" || FormaPagoMOP == "07" || FormaPagoMOP == "96" || FormaPagoMOP == "97" || FormaPagoMOP == "99") && ClaveObservacion != "IA")
                {
                    string Mensaje = " Fecha Incumplimiento: " + FechaIncumplimiento + " Forma de Pago: " + FormaPagoMOP + " Clave de Observacion: " + ClaveObservacion;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R37-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 38 - Fecha de Primer Incumplimiento incongruente
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla38(ValidationResults results)
            {
                // Con Fecha de Primer Incumplimiento y
                // (Fecha de Primer Incumplimiento < Fecha de Apertura ó
                // (Con Fecha de Cierre y Fecha de Primer Incumplimiento > Fecha de Cierre) ó
                // Fecha de Primer Incumplimiento – Fecha de Reporte > 15 días)

                string FechaApertura = tl.TL_13;
                string FechaCierre = tl.TL_16;
                string FechaReporte = tl.TL_17;
                string FechaIncumplimiento = tl.TL_43;
                string FormaPagoMOP = tl.TL_26;

                // Ajuste: Si Fecha de Primer Incumplimiento es igual a 01011900 y la Forma de Pago es al Corriente (MOP 00, 01, UR)
                // colocamos la fecha en blanco para la validacion. Ya que el dato 01011900 solo se requiere para la creacion del 
                // archivo PF en la etiqueta 43, sin embargo en la validacion este dato causa rechazo del registro.
                if (FechaIncumplimiento == "01011900" && (FormaPagoMOP == "00" || FormaPagoMOP == "01" || FormaPagoMOP == "UR"))
                {
                    FechaIncumplimiento = "";
                }

                if (!string.IsNullOrWhiteSpace(FechaIncumplimiento) &&
                     ((Parser.ToDateTime(FechaIncumplimiento) < Parser.ToDateTime(FechaApertura)) ||
                      (!string.IsNullOrWhiteSpace(FechaCierre) && Parser.ToDateTime(FechaIncumplimiento) > Parser.ToDateTime(FechaCierre)) ||
                      (Parser.ToDateTime(FechaIncumplimiento).Subtract(Parser.ToDateTime(FechaReporte)).Days > 15)
                     )
                   )
                {
                    string Mensaje = "MOP: " + FormaPagoMOP + " F. Apertura: " + FechaApertura + " F. Cierre: " + FechaCierre + " F. Reporte: " + FechaReporte + " F. Incumplimiento: " + FechaIncumplimiento;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R38-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
           }

            /// <summary>
            /// Regla 39 - Tipo de Contrato (LR) Línea de Crédito Reinstalable y Tipo de Cuenta Pagos Fijos (I)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla39(ValidationResults results)
            {
                // Tipo de Contrato = "LR" y
                // Tipo de Cuenta = "I" y
                // (Saldo actual < 0 ó
                // Saldo vencido < 0 ó
                // Límite de crédito <= 0 ó
                // Crédito máximo < 0 ó
                // Importe de pago < 0)

                string TipoContrato = tl.TL_07;
                string TipoCuenta = tl.TL_06;
                Double ImportePago = Parser.ToDouble(tl.TL_12);
                Double CreditoMaximo = Parser.ToDouble(tl.TL_21);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double LimiteCredito = Parser.ToDouble(tl.TL_23);
                Double SaldoVencido = Parser.ToDouble(tl.TL_24);

                if ( TipoContrato == "LR" && TipoCuenta == "I" &&
                     (SaldoActual < 0 || SaldoVencido < 0 || LimiteCredito <= 0 || CreditoMaximo < 0 || ImportePago < 0)
                   )
                {
                    string Mensaje = " Tipo Contrato: " + TipoContrato + " Tipo Cuenta: " + TipoCuenta + " Credito Maximo: " + CreditoMaximo + " Saldo Actual: " + SaldoActual + " Limite Credito: " + LimiteCredito + " Saldo Vencido:" + SaldoVencido;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R39-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 40 - Saldo Insoluto del Principal > Saldo Actual
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla40(ValidationResults results)
            {
                // Saldo Insoluto del Principal > Saldo Actual

                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double SaldoInsoluto = Parser.ToDouble(tl.TL_44);

                if ( SaldoInsoluto > SaldoActual)
                {
                    string Mensaje = " Saldo Insoluto: " + SaldoInsoluto.ToString() + " Saldo ACtual: " + SaldoActual.ToString() ;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R40-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 42 - Monto de Ultimo Pago Sin fecha de pago
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla42(ValidationResults results)
            {
                // Monto de Ultimo Pago > 0 y
                // Sin Fecha de Ultimo Pago

                string FechaUltimoPago = tl.TL_14; 
                Double MontoUltimoPago = Parser.ToDouble(tl.TL_45);

                if (MontoUltimoPago > 0 && string.IsNullOrWhiteSpace(FechaUltimoPago))
                {
                    string Mensaje = " Monto Ultimo Pago: " + MontoUltimoPago.ToString() + " Fecha Ultimo Pago: " + FechaUltimoPago;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R42-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 43 - Monto de último pago no reportado
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla43(ValidationResults results)
            {
                // Monto de Ultimo Pago < 0

                Double MontoUltimoPago = Parser.ToDouble(tl.TL_45);

                if (MontoUltimoPago < 0)
                {
                    string Mensaje = " Monto Ultimo Pago: " + MontoUltimoPago.ToString();
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R43-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Regla 44 - Plazo en meses requerido para créditos de pago fijo o hipotecario
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionRegla44(ValidationResults results)
            {
                // Tipo de Cuenta = "I" o "M" y
                // Termino en meses < = 0

                string TipoCuenta = tl.TL_06;
                Double PlazoMeses = Parser.ToDouble(tl.TL_50);

                if ((TipoCuenta == "I" || TipoCuenta == "M") && PlazoMeses < 0)
                {
                    string Mensaje = " Tipo Cuenta: " + TipoCuenta + " Plazo en Meses: " + PlazoMeses.ToString();
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("R44-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

        #endregion

        #region Metodos Validadores - Reglas Específicas de Validación y Alerta (Informativas)

            /// <summary>
            /// Alerta Informativa 01 - Cambio del Número de Cuenta
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaI01(ValidationResults results)
            {
                // Con Número de Crédito y
                // Con Nuevo Número de Crédito

                string NumeroCredito = tl.TL_16;
                string NuevoNumeroCredito = tl.TL_16;

                if (!string.IsNullOrWhiteSpace(NumeroCredito) && !string.IsNullOrWhiteSpace(NuevoNumeroCredito))
                {
                    string Mensaje = "Numero Credito: " + NumeroCredito + " Nuevo Numero de Credito: " + NuevoNumeroCredito;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("AI1-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Informativa 02 - Reapertura de Cuentas
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaI02(ValidationResults results)
            {
                // Fecha de Cierre = 02021900

                string FechaCierre = tl.TL_16;

                if (FechaCierre == "02021900")
                {
                    string Mensaje = "Fecha de Cierre: " + FechaCierre;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("AI2-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Informativa 03 - Eliminación de Clave de Observación
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaI03(ValidationResults results)
            {
                // Clave de Observación = "EL"

                string ClaveObservacion = tl.TL_30;

                if (ClaveObservacion == "EL")
                {
                    string Mensaje = "Clave de Observacion: " + ClaveObservacion;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("AI3-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

        #endregion 

        #region Metodos Validadores - Reglas Específicas de Validación y Alerta (Preventivas)

            /// <summary>
            /// Alerta Preventiva 01 - Cuenta con Fecha de Apertura >= 90 días y sin RFC
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP01(ValidationResults results)
            {
                // Sin RFC y
                // Fecha de Apertura >= "01/01/1998" y
                // Fecha de Reporte – Fecha de Apertura >= 90 días.

                string RFC = tl.TypedParent.PN_05;
                string FechaApertura = tl.TL_13;
                string FechaReporte = tl.TL_17;

                if (string.IsNullOrWhiteSpace(RFC) && 
                    Parser.ToDateTime(FechaApertura) > Parser.ToDateTime("1998-01-01") 
                    && Parser.ToDateTime(FechaReporte).Subtract(Parser.ToDateTime(FechaApertura)).Days >= 90 )
                {
                    string Mensaje = "RFC: " + RFC + " Fecha de Apertura: " + FechaApertura + " Fecha de Reporte: " + FechaReporte;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A01-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 02 - Fecha de Reporte incongruente
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP02(ValidationResults results)
            {
                // (Con Fecha de Ultimo Pago y Fecha de Reporte < Fecha de Ultimo Pago) ó
                // (Con Fecha de Ultima Compra y Fecha de Reporte < Fecha de Ultima Compra) ó
                // (Con Fecha de Cierre y Fecha de Reporte < Fecha de Cierre)

                DateTime FechaDefault = Parser.ToDateTime("1900-01-01");
                DateTime FechaUltimoPago = Parser.ToDateTime(tl.TL_14);
                DateTime FechaUltimaCompra = Parser.ToDateTime(tl.TL_15);
                DateTime FechaCierre = Parser.ToDateTime(tl.TL_16);
                DateTime FechaReporte = Parser.ToDateTime(tl.TL_17);

                if ( (FechaUltimoPago != FechaDefault && FechaReporte < FechaUltimoPago ) ||
                     (FechaUltimaCompra != FechaDefault && FechaReporte < FechaUltimaCompra ) ||
                     (FechaCierre != FechaDefault && FechaReporte < FechaCierre)
                   )
                {
                    string Mensaje = "F. Ultimo Pago: " + FechaUltimoPago.ToString() + " F. Ultima Compra: " + FechaUltimaCompra.ToString() + " F. Cierre: " + FechaCierre.ToString() + " F. Reporte: " + FechaReporte.ToString();
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A02-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 03 - Saldo Actual > Crédito Máximo
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP03(ValidationResults results)
            {
                // Saldo Actual > Crédito Máximo y
                // Crédito Máximo > 0
                
                Double CreditoMaximo = Parser.ToDouble(tl.TL_21);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);

                if (SaldoActual > CreditoMaximo && CreditoMaximo > 0 )
                {
                    string Mensaje = "Saldo Actual: " + SaldoActual.ToString() + " Credito Maximo: " + CreditoMaximo;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A03-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 04 - Crédito Máximo inconsistente
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP04(ValidationResults results)
            {
                // Crédito Máximo <= 0 y
                // Saldo Actual > 0 y
                // (Fecha de Reporte – Fecha de Apertura > 90 ó Fecha de Reporte – Fecha de Apertura < 0)

                Double CreditoMaximo = Parser.ToDouble(tl.TL_21);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                string FechaReporte = tl.TL_17;
                string FechaApertura = tl.TL_13 ;

                if ( CreditoMaximo <= 0 && SaldoActual > 0 &&
                     (Parser.ToDateTime(FechaReporte).Subtract(Parser.ToDateTime(FechaApertura)).Days > 90 ||
                      Parser.ToDateTime(FechaReporte).Subtract(Parser.ToDateTime(FechaApertura)).Days < 0
                     )                    
                   )
                {
                    string Mensaje = "Credito Maximo: " + CreditoMaximo + " Saldo Actual: " + SaldoActual + " F. Apertura: " + FechaApertura + " F. Reporte: " + FechaReporte;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A04-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 05 - Fecha de Cierre menor a la Fecha de Ultima Compra
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP05(ValidationResults results)
            {
                // Con Fecha de Cierre y
                // Fecha de Cierre < Fecha de Ultima Compra               

                string FechaCierre = tl.TL_16;
                string FechaUltimaCompra = tl.TL_15;

                if (!string.IsNullOrWhiteSpace(FechaCierre) && Parser.ToDateTime(FechaCierre) < Parser.ToDateTime(FechaUltimaCompra))
                {
                    string Mensaje = "Fecha de Cierre: " + FechaCierre + " Fecha Ultima Compra: " + FechaUltimaCompra;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A05-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 06 - Frecuencia de Pago faltante
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP06(ValidationResults results)
            {
                // Sin Frecuencia de Pagos reportada ó
                // Frecuencia de pagos no está en catálogo ó
                // (Número de Pagos Vencidos = 0 y MOP <> 00, UR, 01) ó
                // (Número de Pagos Vencidos > 0 y MOP < 02)

                string FrecuenciaPagos = tl.TL_11;
                string FormaPago = tl.TL_26;
                int PagosVencidos = Parser.ToNumber(tl.TL_25);

                if (string.IsNullOrEmpty(FrecuenciaPagos) ||
                    (FrecuenciaPagos !=  "B" && FrecuenciaPagos !=  "D" && FrecuenciaPagos !=  "H" && FrecuenciaPagos !=  "K" && FrecuenciaPagos !=  "M" && FrecuenciaPagos !=  "P" && FrecuenciaPagos !=  "Q" && FrecuenciaPagos !=  "S" && FrecuenciaPagos !=  "V" && FrecuenciaPagos !=  "W") ||
                    (PagosVencidos == 0 && (FormaPago != "00" && FormaPago != "01" && FormaPago != "UR") ) ||
                    (PagosVencidos > 0 && (FormaPago == "00" || FormaPago == "01" || FormaPago == "UR") )
                   )
                {
                    string Mensaje = "Frecuencia de Pagos: " + FrecuenciaPagos + " Forma de Pago (MOP): " + FormaPago + " Pagos Vencidos: " + PagosVencidos;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A06-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 07 - Inconsistencia en Frecuencia de Pago
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP07(ValidationResults results)
            {
                // Tipo de Cuenta = "R", "O" y
                // Frecuencia de Pagos <> "Z"

                string TipoCuenta = tl.TL_06;
                string FrecuenciaPagos = tl.TL_11;

                if ( (TipoCuenta == "R" || TipoCuenta == "O") && FrecuenciaPagos != "Z" )
                {
                    string Mensaje = "Tipo de Cuenta: " + TipoCuenta + " Frecuencia de Pagos: " + FrecuenciaPagos;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A07-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 08 - Saldo Actual faltante o menor a cero
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP08(ValidationResults results)
            {
                // Sin Saldo Actual ó
                // Saldo Actual < 0

                Double? SaldoActual = Parser.ToDouble(tl.TL_22);

                if (SaldoActual.HasValue == false || SaldoActual < 0)
                {
                    string Mensaje = "Saldo Actual: " + SaldoActual;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A08-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 09 - Saldo Vencido faltante o menor a cero
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP09(ValidationResults results)
            {
                // Sin Saldo Vencido ó
                // Saldos Vencidos < 0

                Double? SaldoVencido = Parser.ToDouble(tl.TL_24);

                if (SaldoVencido.HasValue == false || SaldoVencido < 0)
                {
                    string Mensaje = "Saldo Vencido: " + SaldoVencido;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A09-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 10 - Tipo de Responsabilidad no reportado
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP10(ValidationResults results)
            {
                //  Tipo de Responsabilidad no reportada

                string TipoResponsabilidad = tl.TL_05;

                if (TipoResponsabilidad != "I" && TipoResponsabilidad != "J" && TipoResponsabilidad != "C")
                {
                    string Mensaje = "Tipo Responsabilidad: " + TipoResponsabilidad;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A10-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 11 - Saldo Insoluto de Principal faltante o menor a cero
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP11(ValidationResults results)
            {
                // Sin Saldo Insoluto de Principal ó
                // Saldo Insoluto de Principal < 0

                Double? SaldoInsoluto = Parser.ToDouble(tl.TL_44);

                if (SaldoInsoluto.HasValue == false || SaldoInsoluto < 0)
                {
                    string Mensaje = "Saldo Insoluto del Principal: " + SaldoInsoluto;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A11-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 12 - Forma de pago (MOP) inválido de acuerdo a la clave de Observación
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP12(ValidationResults results)
            {
                // Clave de observación: AD, FN, RI, LS y MOP <> "UR"
                // Clave de observación: FD y MOP <> "99"
                // Clave de observación: LC, UP y MOP <> "97"

                string FormaPagoMOP = tl.TL_26;
                string ClaveObservacion = tl.TL_30;

                if ( ((ClaveObservacion == "AD" || ClaveObservacion == "FN" || ClaveObservacion == "RI" || ClaveObservacion == "LS" ) && FormaPagoMOP != "UR") ||
                     (ClaveObservacion == "FD" && FormaPagoMOP != "99") ||
                     ((ClaveObservacion == "LC" || ClaveObservacion == "UP") && FormaPagoMOP != "97")
                   )
                {
                    string Mensaje = "Forma de Pago MOP: " + FormaPagoMOP + " Clave de Observacion: " + ClaveObservacion;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A00-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }

            }

            /// <summary>
            /// Alerta Preventiva 13 - Fecha de primer incumplimiento anterior a 72 meses
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void ValidacionAlertaP13(ValidationResults results)
            {
                // Fecha de primer incumplimiento < (fecha de reporte – 72 meses)

                /*
                string FechaIncumplimiento = tl.TL_43;
                string FechaReporte = tl.TL_17;
                TimeSpan SieteMeses = new TimeSpan(2191, 0, 0, 0, 0);

                if (Parser.ToDateTime(FechaIncumplimiento) < Parser.ToDateTime(FechaReporte).Subtract(SieteMeses))
                {
                    string temp =  Parser.ToDateTime(FechaReporte).Subtract(SieteMeses).ToString();

                    string Mensaje = "F Incumplimiento: " + FechaIncumplimiento + "F Reporte: " + FechaReporte + "F Ajustada: " + temp;
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("A13-F", tl.Validaciones, Mensaje, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                }
                */
            }

            [SelfValidation]
            public void ValidacionObservacion(ValidationResults results)
            {
                Double MontoPagar1 = Parser.ToDouble(tl.TL_12);
                Double SaldoActual = Parser.ToDouble(tl.TL_22);
                Double SaldoVencido = Parser.ToDouble(tl.TL_24);

                DateTime FechaUltimoPago = Parser.ToDateTime(tl.TL_14);
                DateTime FechaUltimaCompra = Parser.ToDateTime(tl.TL_15);

                switch (tl.TL_30)
                {
                    case "AD":
                        if (tl.TL_26 != "UR" || SaldoActual < 0 || SaldoVencido < 0 || MontoPagar1 < 0)
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("AD-F", tl.Validaciones, "", tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "CA":
                        if (tl.TL_26 != "01" || SaldoActual != 0 || SaldoVencido != 0 || MontoPagar1 != 0 || tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("CA-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "CC":
                        if (tl.TL_26 != "01" || SaldoActual != 0 || SaldoVencido != 0 || MontoPagar1 != 0 || tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("CC-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "CL":
                        if (tl.TL_26 != "01" || SaldoActual != 0 || SaldoVencido != 0 || MontoPagar1 != 0 || tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("CL-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "CV":
                        if (!(tl.TL_26 == "UR" || tl.TL_26 == "00" || tl.TL_26 == "00" || tl.TL_26 == "01") || MontoPagar1 != 0 || SaldoVencido < 0 || MontoPagar1 != 0 || tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("CV-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "FD":
                        if (tl.TL_26 != "99" || SaldoActual <= 0 || SaldoVencido <= 0 || MontoPagar1 <= 0)
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("FD-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "FN":
                        if (tl.TL_26 != "UR" || SaldoActual != 0 || SaldoVencido != 0 || MontoPagar1 != 0)
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("FN-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "FP":
                        if (!WebConfig.TL_01_ES.Contains("BB") || tl.TL_26 != "01" || SaldoActual != 0 || SaldoVencido != 0 || MontoPagar1 != 0 || tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("FP-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "FR":
                        if (!(tl.TL_26 == "01" && SaldoVencido == 0) || !(tl.TL_26 == "97" && SaldoVencido >= 0) || SaldoActual != 0 || MontoPagar1 != 0 || tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("FR-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "IA":
                        if (!(tl.TL_26 == "UR" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && tl.TL_16 == "00000000" && (FechaUltimoPago == default(DateTime) || (Parser.ToDateTime(tl.TypedParent.TypedParent.INTF.INTF_35, "ddMMyyyy") - FechaUltimoPago).Days > 30) && (FechaUltimaCompra == default(DateTime) || (Parser.ToDateTime(tl.TypedParent.TypedParent.INTF.INTF_35, "ddMMyyyy") - FechaUltimaCompra).Days > 30)))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("IA-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "IM":
                        if (!(!(tl.TL_26 == "UR" || tl.TL_26 == "00" || tl.TL_26 == "01") && SaldoActual > 0 && SaldoVencido > 0 && MontoPagar1 > 0))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("IM-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "LC":
                        if (!(tl.TL_26 == "97" && SaldoActual == 0 && SaldoVencido >= 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("LC-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "LG":
                        if (!(tl.TL_26 == "01" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && (tl.TL_16 != "00000000" && Parser.ToDateTime(tl.TL_16) != default(DateTime))) || !(SaldoActual > 0 && SaldoVencido == 0 && MontoPagar1 > 0 && (tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("LG-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "LO":
                        if (!(!(tl.TL_26 == "UR" || tl.TL_26 == "00" || tl.TL_26 == "01") && SaldoActual > 0 && SaldoVencido > 0 && MontoPagar1 > 0 && (tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("LO-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "LS":
                        if (!(tl.TL_26 == "UR" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("LS-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "NA":
                        if (!(tl.TL_26 == "01" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("NA-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "NV":
                        if (!(!(tl.TL_26 == "UR" || tl.TL_26 == "00" || tl.TL_26 == "01" || tl.TL_26 == "97" || tl.TL_26 == "99") && MontoPagar1 == 0 && SaldoVencido >= 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("NV-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "PC":
                        if (!(!(tl.TL_26 == "UR" || tl.TL_26 == "00" || tl.TL_26 == "01" || tl.TL_26 == "97" || tl.TL_26 == "99") && (tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("PC-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "RA":
                        if (!(tl.TL_26 == "01" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("RA-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "RI":
                        if (!(tl.TL_26 == "UR" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("RI-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "RF":
                        if (!(SaldoActual >= 0 && SaldoVencido >= 0 && MontoPagar1 >= 0))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("RF-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "RN":
                        if (!(tl.TL_26 == "01" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("RN-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "RV":
                        if (!(tl.TL_26 == "01" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("RV-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "SG":
                        if (!(!(tl.TL_26 == "UR" || tl.TL_26 == "00" || tl.TL_26 == "01" || tl.TL_26 == "97" || tl.TL_26 == "99") && SaldoActual > 0 && SaldoVencido > 0 && MontoPagar1 > 0 && (tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("SG-F", tl.Validaciones, "MOP: " + tl.TL_26 + " Saldo Act: " + tl.TL_22 + " Saldo Venc: " + tl.TL_24 + " Monto " + tl.TL_12 + " Fecha: " + tl.TL_16, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "UP":
                        if (!(tl.TL_26 == "97" && SaldoActual > 0 && SaldoVencido > 0 && MontoPagar1 > 0))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("UP-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;
                    case "VR":
                        if (!(tl.TL_26 == "01" && SaldoActual == 0 && SaldoVencido == 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))) || !(tl.TL_26 == "97" && SaldoActual == 0 && SaldoVencido >= 0 && MontoPagar1 == 0 && !(tl.TL_16 == "00000000" || Parser.ToDateTime(tl.TL_16) == default(DateTime))))
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("VR-F", tl.Validaciones, tl.TL_30, tl, tl.TypedParent.PN_05, tl.TL_04), "", "", null));
                        break;

                }

            }

        #endregion

    }

}
