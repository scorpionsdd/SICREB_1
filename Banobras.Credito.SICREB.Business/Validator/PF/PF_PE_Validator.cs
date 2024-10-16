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

namespace Banobras.Credito.SICREB.Business.Validator.PF
{

    [HasSelfValidation]
    public class PF_PE_Validator
    {

        private PF_PE pe = null;

        public PF_PE_Validator(PF_PE pe)
        {
            this.pe = pe;
        }
        
        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PF_PE_Validator> HDValidator = factory.CreateValidator<PF_PE_Validator>();
            return HDValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Nombre o Razon Social del Empleador | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_PE(ValidationResults results)
            {

                // 401-F El dato Nombre o Razon Social del empleador es requerido
                if (string.IsNullOrEmpty(pe.PE_PE))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("401-F", pe.Validaciones, "Empleador: " + pe.PE_PE, pe), "", "", null));
                    return;
                }

                // 402-F La Longitud del dato Nombre o Razon Social del empleado es mayor a lo permitido (99 Posiciones)
                if (pe.PE_PE.Length > 99)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("402-F", pe.Validaciones, "Empleador: " + pe.PE_PE, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Primer Linea de Direccion | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_00(ValidationResults results)
            {

                // La direccion del empleador es opcional
                if (string.IsNullOrEmpty(pe.PE_00))
                {
                    pe.Correctos++;
                    return;
                }

                // Si se reporta una direccion aplicamos las validaciones

                // 403-F La longitud del dato Primer Linea de Direccion es mayor a lo permitido (40 posiciones)
                if (pe.PE_00.Length > 40)
                {

                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("403-F", pe.Validaciones, pe.PE_00, pe), "", "", null));
                    return;
                }

                // 404-F La Direccion unicamente contiene la calle sin el numero.
                string Direccion = pe.PE_00 + pe.PE_01;
                if (!Direccion.Contains("SN") &&
                    !Direccion.Contains("0") &&
                    !Direccion.Contains("1") &&
                    !Direccion.Contains("2") &&
                    !Direccion.Contains("3") &&
                    !Direccion.Contains("4") &&
                    !Direccion.Contains("5") &&
                    !Direccion.Contains("6") &&
                    !Direccion.Contains("7") &&
                    !Direccion.Contains("8") &&
                    !Direccion.Contains("9")) 
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("404-F", pe.Validaciones, pe.PE_00, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Segunda Linea de Direccion | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_01(ValidationResults results)
            {
                if (string.IsNullOrEmpty(pe.PE_01))
                {
                    pe.Correctos++;
                    return;
                }

                // 405-F La longitud del dato Seguna Linea de Direccion es mayor a lo permitido (40 posiciones)
                if (pe.PE_01.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("405-F", pe.Validaciones, pe.PE_01, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Colonia o Poblacion | Texto | Complementario
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_02(ValidationResults results)
            {
                  
                // 406-F El dato Colonia/Poblacion es requerido cuando se reporta la direccion
                if (string.IsNullOrEmpty(pe.PE_02) && !string.IsNullOrEmpty(pe.PE_00))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("406-F", pe.Validaciones, pe.PE_02, pe), "", "", null));
                    return;
                }

                // 407-F La longitud del dato Colonia/Poblacion es mayor a lo permitido (40 posiciones)
                if (pe.PE_02.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("407-F", pe.Validaciones, pe.PE_02, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Delegacion o Municipio | Texto | Complementario
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_03(ValidationResults results)
            {

                // Si no se reporta la direccion, el dato Delegacion/Municipio se vuelve opcional
                if (string.IsNullOrEmpty(pe.PE_00) && string.IsNullOrEmpty(pe.PE_03))
                {
                    pe.Correctos++;
                    return;
                }

                // 408-F Si no se reporta la Ciudad, el dato Delegacion/Municipio es requerido
                if (string.IsNullOrEmpty(pe.PE_03) && string.IsNullOrEmpty(pe.PE_04))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("408-F", pe.Validaciones, "", pe), "", "", null));
                    return;
                }

                // 409-F La longitud del dato Delegacion/Municipio es mayor a lo permitido (40 posiciones)
                if (pe.PE_03.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("409-F", pe.Validaciones, pe.PE_03, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Ciudad | Texto | Complementario
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_04(ValidationResults results)
            {
                // Si no se reporta la direccion, el dato Ciudad se vuelve opcional
                if (string.IsNullOrEmpty(pe.PE_00) && string.IsNullOrEmpty(pe.PE_04))
                {
                    pe.Correctos++;
                    return;
                }

                // 410-F Si no se reporta Delegacion/Municipio, el dato Ciudad es requerido
                if (string.IsNullOrEmpty(pe.PE_04) && string.IsNullOrEmpty(pe.PE_03))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("410-F", pe.Validaciones, pe.PE_04, pe), "", "", null));
                    return;
                }

                // 411-F La longitud del dato Ciudad es mayor de lo permitido (40 posiciones)
                if (pe.PE_04.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("411-F", pe.Validaciones, pe.PE_04, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Estado | Texto | Complementario
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_05(ValidationResults results)
            {

                // ----------------------------------------------------------
                // Si es Extranjero el Estado es opcional
                // ----------------------------------------------------------
                if (string.IsNullOrEmpty(pe.PE_05) && pe.EsExtranjero )
                {
                    pe.Correctos++;
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano y reporta domicilio aplicamos validaciones
                // ----------------------------------------------------------

                // Si no se reporta el domicilio, el estado se vuelve opcional
                if (String.IsNullOrWhiteSpace(pe.PE_00) && String.IsNullOrWhiteSpace(pe.PE_05))
                {
                    pe.Correctos++;
                    return;
                }

                // 412-F El dato Clave de Estado es requerido para validar la consistencia con el domicilio
                if (String.IsNullOrWhiteSpace(pe.PE_05) && !pe.EsExtranjero)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("412-F", pe.Validaciones, pe.PE_05, pe), "", "", null));
                    return;
                }

                // 413-F La longitud del dato Clave de Estado es mayor a lo permitido (4 posiciones)
                if (pe.PE_05.Length > 4)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("413-F", pe.Validaciones, pe.PE_05, pe), "", "", null));
                    return;
                }

                // 414-F La Clave de Estado reportada no se localizo en el catalogo correspondiente
                if (!CommonValidator.ValidarClaveEstado(pe.PE_05, Enums.Persona.Fisica))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("414-F", pe.Validaciones, pe.PE_05, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Codigo Postal | Numerico | Complementario
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_06(ValidationResults results)
            {

                // 415-M El dato Codigo Postal es requerido.
                if (String.IsNullOrWhiteSpace(pe.PE_06))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("415-F", pe.Validaciones, pe.PE_06, pe, pe.TypedParent.PN_05), "", "", null));
                    return;
                }

                // Si es Mexicano aplicamos las validaciones de SEPOMEX
                if (!pe.EsExtranjero)
                {
                    // 00) Si no existe es la Clave del Estado en el catalogo se rechaza el Registro.           Rechazado
                    // 01) El Codigo Postal no existe en el catalogo de SEPOMEX.                                Rechazado
                    // 02) Si el Estado del catalogo y el Estado del CP no coinciden se rechaza el Registro.    Rechazado
                    // 03) EstadoCP = EstadoR       MunicipioCP != MunicipioR       CiudadCP != CiudadR         Rechazado
                    // 04) EstadoCP = EstadoR       MunicipioCP != MunicipioR                                   Rechazado
                    // 05) EstadoCP = EstadoR                                       CiudadCP != CiudadR         Rechazado
                    // 06) EstadoCP = EstadoR       MunicipioCP = MunicipioR        CiudadCP = CiudadR          Aceptado
                    // 07) EstadoCP = EstadoR       MunicipioCP = MunicipioR                                    Aceptado
                    // 08) EstadoCP = EstadoR                                       CiudadCP = CiudadR          Aceptado
                    // 09) EstadoCP = EstadoR       MunicipioCP != MunicipioR       CiudadCP = CiudadR          Aceptado
                    // 10) EstadoCP = EstadoR       MunicipioCP = MunicipioR        CiudadCP != CiudadR         Aceptado

                    string CPTemp = CommonValidator.StrCP(pe.PE_06, 5);
                    string ResultadoSEPOMEX = CommonValidator.ValidarCPSEPOMEX_PF(CPTemp, pe.PE_05, pe.PE_03, pe.PE_04);

                    switch (ResultadoSEPOMEX)
                    {
                        case "01": // 416-F El dato Codigo Postal no se encontro en el catalogo de SEPOMEX.
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("416-F", pe.Validaciones, "Empleador: " + pe.PE_PE + ", " + pe.PE_06, pe, pe.TypedParent.PN_05), "", "", null));
                            break;
                        case "02": // 417-F El dato Nombre de Estado para Domicilios en México no coincide con el C.P. (SEPOMEX).
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("417-F", pe.Validaciones, "Empleador: " + pe.PE_PE + ", " + pe.PE_06, pe, pe.TypedParent.PN_05), "", "", null));
                            break;
                        case "03": // 418-F El Municipio y la Ciudad no coinciden con el C.P. (SEPOMEX).
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("418-F", pe.Validaciones, "Empleador: " + pe.PE_PE + ", " + pe.PE_06, pe, pe.TypedParent.PN_05), "", "", null));
                            break;
                        case "04": // 419-F El dato Delegacion/Municipio no coincide con el C.P. (SEPOMEX), la Ciuda no fue reportada
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("419-F", pe.Validaciones, "Empleador: " + pe.PE_PE + ", " + pe.PE_06, pe, pe.TypedParent.PN_05), "", "", null));
                            break;
                        case "05": // 420-F El dato Ciudad no coincide con el C.P. (SEPOMEX), la Delegacion/Municipio no fue reportada
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("420-F", pe.Validaciones, "Empleador: " + pe.PE_PE + ", " + pe.PE_06, pe, pe.TypedParent.PN_05), "", "", null));
                            break;                     
                    }
                }

                // 421-F La longitud del dato Codigo Postal debe ser de 5 posiciones
                if (pe.PE_06.Length != 5)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("421-F", pe.Validaciones, pe.PE_06, pe, pe.TypedParent.PN_05), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Numero de Telefono | Numerico | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_07(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_07))
                {
                    return;
                }

                // 422-F El dato de Telefono debe contener solo digitos
                if (!CommonValidator.Match(pe.PE_07, CommonValidator.REGX_DIGITOS, true))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("422-F", pe.Validaciones, pe.PE_07, pe), "", "", null));
                    return;
                }

                // 423-F La longitud del dato Telefono es mayor a lo permitido (11 posiciones)
                if (pe.PE_07.Length > 11)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("423-F", pe.Validaciones, pe.PE_07, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Extension Telefonica | Numerico | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_08(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_08))
                {
                    return;
                }

                // 424-F El dato de Extension Telefonica debe contener solo digitos
                if (!CommonValidator.Match(pe.PE_08, CommonValidator.REGX_DIGITOS, true))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("424-F", pe.Validaciones, pe.PE_08, pe), "", "", null));
                    return;
                }

                // 425-F La longitud del dato Extension Telefonica es mayor a lo permitido (8 posiciones)
                if (pe.PE_08.Length > 8 )
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("425-F", pe.Validaciones, pe.PE_08, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Numero de Fax en esta Direccion | Numerico | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_09(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_09))
                {
                    return;
                }

                // 426-F El dato de Numero de Fax debe contener solo digitos
                if (!CommonValidator.Match(pe.PE_08, CommonValidator.REGX_DIGITOS, true))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("426-F", pe.Validaciones, pe.PE_09, pe), "", "", null));
                    return;
                }

                // 427-F La longitud del dato Numero de Fax es mayor de lo permitido (11 posiciones)
                if (pe.PE_09.Length > 11)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("427-F", pe.Validaciones, pe.PE_09, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Cargo u Ocupacion | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_10(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_10))
                {
                    return;
                }

                // 428-F La longitud del dato Cargo u Ocupacion es mayor a lo permitido (30 posiciones)
                if (pe.PE_10.Length > 30)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("428-F", pe.Validaciones, pe.PE_10, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Fecha de Contratacion | Fecha | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_11(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_11))
                {
                    return;
                }

                // 429-F La longitud del dato Fecha de Contratacion debe ser de 8 posiciones
                if (pe.PE_11.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("429-F", pe.Validaciones, pe.PE_11, pe), "", "", null));
                    return;
                }

                // 430-F El formato del Fecha de Contratacion es incorrecto (Formato DDMYYYY)
                Regex re = new Regex(WebConfig.PatronDeFecha);
                if (!re.IsMatch(pe.PE_11))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("430-F", pe.Validaciones, pe.PE_11, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Clave de la Moneda de Pago del Sueldo | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_12(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_12))
                {
                    return;
                }

                // 431-F La longitud del dato Clave de la Moneda de Pago debe ser de 2 posiciones

                if (pe.PE_12.Length != 2)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("431-F", pe.Validaciones, pe.PE_12, pe), "", "", null));
                    return;
                }

                // 432-F El dato Clave de la Moneda de Pago no se localizo en el catalogo correspondiente

                pe.Correctos++;
            }

            /// <summary>
            /// Monto de Sueldo o Salario | Numerico | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_13(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_12) && string.IsNullOrEmpty(pe.PE_13))
                {
                    return;
                }

                // 433-F El dato Monto del Sueldo es requerido cuando se reporta la Clave de la Moneda de Pago
                if (!string.IsNullOrEmpty(pe.PE_12) && string.IsNullOrEmpty(pe.PE_13))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("433-F", pe.Validaciones, pe.PE_13, pe), "", "", null));
                    return;
                }

                // 434-F La longitud del dato Monto del Sueldo es mayor a lo permitido (9 posiciones)
                if (pe.PE_13.Length > 9)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("434-F", pe.Validaciones, pe.PE_13, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Periodo de Pago o Base Salarial | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_14(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_12) && string.IsNullOrEmpty(pe.PE_14))
                {
                    return;
                }

                // 435-F El dato Periodo de Pago es requerido cuando se reporta la Clave de la Moneda de Pago
                if (!string.IsNullOrEmpty(pe.PE_12) && string.IsNullOrEmpty(pe.PE_14))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("435-F", pe.Validaciones, pe.PE_14, pe), "", "", null));
                    return;
                }

                // 436-F El dato Periodo de Pago no se localizo en el catalogo correspondiente
                if (pe.PE_14.Length != 1 || !(pe.PE_14[0] == 'B' || pe.PE_14[0] == 'D' || pe.PE_14[0] == 'H' || pe.PE_14[0] == 'K' || pe.PE_14[0] == 'M' || pe.PE_14[0] == 'S' || pe.PE_14[0] == 'W' || pe.PE_14[0] == 'Y'))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("436-F", pe.Validaciones, pe.PE_14, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Numero de Empleado | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_15(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_15))
                {
                    return;
                }

                // 437-F La longitud del dato Numero de Empleado es mayor de lo permitido (15 posiciones)
                if (pe.PE_15.Length > 15) 
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("437-F", pe.Validaciones, pe.PE_15, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Fecha de Ultimo Dia de Empleo | Fecha | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_16(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_16))
                {
                    return;
                }

                // 438-F La longitud del dato Fecha de Ultimo dia de Empleo debe ser de 8 posiciones
                if (pe.PE_16.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("438-F", pe.Validaciones, pe.PE_16, pe), "", "", null));
                    return;
                }
                
                // 439-F El formato de la Fecha de Ultimo Dia de Empleo es incorrecto (Formato correcto DDMMYYYY)
                Regex re = new Regex(WebConfig.PatronDeFecha);
                if (!re.IsMatch(pe.PE_16))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("439-F", pe.Validaciones, pe.PE_16, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Fecha de Verificacion de Empleo | Fecha | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_17(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_17))
                {
                    return;
                }

                // 440-F La longitud del dato Fecha de Verificacion de Empleo debe ser de 8 posiciones
                if (pe.PE_17.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("440-F", pe.Validaciones, pe.PE_17, pe), "", "", null));
                    return;
                }

                // 441-F El formato de la Fecha de Verificacion de Empleo es incorrecto (Formato correcto DDMMYYYY)
                Regex re = new Regex(WebConfig.PatronDeFecha);
                if (!re.IsMatch(pe.PE_17))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("441-F", pe.Validaciones, pe.PE_17, pe), "", "", null));
                    return;
                }

                pe.Correctos++;
            }

            /// <summary>
            /// Origen de la Razon Social y Domicilio (Pais) | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_18(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pe.PE_18))
                {
                    pe.PE_18 = "MX";
                }

                // 442-F La longitud del dato Origen de la Razon Social y Domicilio debe ser de 2 posiciones
                if (pe.PE_17.Length != 2)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("442-F", pe.Validaciones, pe.PE_17, pe), "", "", null));
                    return;
                }

                // 443-F El dato Origen de la Razon Social y Domicilio no se encontro en el catalogo correspondiente

                pe.Correctos++;
            }

        #endregion Metodos Validadores

    }

}
