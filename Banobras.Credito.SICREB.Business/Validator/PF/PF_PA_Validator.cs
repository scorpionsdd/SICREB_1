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
using Banobras.Credito.SICREB.Business.Repositorios;

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Banobras.Credito.SICREB.Business.Validator.PF
{

    [HasSelfValidation]
    public class PF_PA_Validator
    {

        private PF_PA pa = null;

        public PF_PA_Validator(PF_PA pa)
        {
            this.pa = pa;
        }

        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PF_PA_Validator> HDValidator = factory.CreateValidator<PF_PA_Validator>();
            return HDValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Primer Linea de Direccion | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_PA(ValidationResults results)
            {

                // 301-F El dato Primera Linea de Direccion es requerido
                if (string.IsNullOrWhiteSpace(pa.PA_PA))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("301-F", pa.Validaciones, pa.PA_PA, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 302-F La longitud del dato Primer Linea de Direccion es mayor a lo permitido (40 posiciones)
                if (pa.PA_PA.Length > 40 || pa.PA_PA.Length < 3)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("302-F", pa.Validaciones, pa.PA_PA, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 303-F La Direccion unicamente contiene la calle sin el numero.
                string Direccion = pa.PA_PA + pa.PA_00;
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
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("303-F", pa.Validaciones, pa.PA_PA, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Segunda Linea de Direccion | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_00(ValidationResults results)
            {
                if (string.IsNullOrWhiteSpace(pa.PA_00))
                {
                    return;
                }

                // 304-F La longitud del dato Segunda Linea de Direccion es mayor a lo permitido (40 posiciones)
                if (pa.PA_00.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("304-F", pa.Validaciones, pa.PA_00, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Colonia o Poblacion | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_01(ValidationResults results)
            {

                // 305-F El dato Colonia/Poblacion es requerido
                if (string.IsNullOrWhiteSpace(pa.PA_01))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("305-F", pa.Validaciones, pa.PA_01, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 306-F La longitud del dato Colonia/Poblacion es mayor de lo permitido (40 posiciones)
                if (pa.PA_01.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("306-F", pa.Validaciones, pa.PA_01, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Delegacion o Municipio | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_02(ValidationResults results)
            {

                // 307-F Si no  se reporta el dato de Ciudad, el dato Delegacion/Municipio es requerido
                if (string.IsNullOrWhiteSpace(pa.PA_02) && string.IsNullOrWhiteSpace(pa.PA_03))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("307-F", pa.Validaciones, pa.PA_02, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 308-F La longitud del dato Delegacion/Municipio es mayor de lo permitido (40 posiciones)
                if (pa.PA_02.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("308-F", pa.Validaciones, pa.PA_02, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Ciudad | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_03(ValidationResults results)
            {
                // 309-F Si no se reporta la Delegacion/Municipio, el dato de Ciudad es requerido
                if (string.IsNullOrWhiteSpace(pa.PA_02) && string.IsNullOrWhiteSpace(pa.PA_03))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("309-F", pa.Validaciones, pa.PA_03, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 310-F La longitud del dato Ciudad es mayor de lo permitido (40 posiciones)
                if (pa.PA_03.Length > 40)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("310-F", pa.Validaciones, pa.PA_03, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Estado | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_04(ValidationResults results)
            {

                // ----------------------------------------------------------
                // Si es Extranjero el Estado es opcional
                // ----------------------------------------------------------
                if (string.IsNullOrWhiteSpace(pa.PA_04) && pa.EsExtranjero)
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos las validaciones
                // ----------------------------------------------------------

                // 311-F El dato Estado es requerido
                if (string.IsNullOrWhiteSpace(pa.PA_04))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("311-F", pa.Validaciones, pa.PA_04, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 312-F La longitud del dato Estado es mayor de lo permitido (4 posiciones)
                if (pa.PA_04.Length > 4)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("312-F", pa.Validaciones, pa.PA_04, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 313-F El dato Estado no se localizo en el catalogo correspondiente
                if (!CommonValidator.ValidarClaveEstado(pa.PA_04, Enums.Persona.Fisica))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("313-F", pa.Validaciones, pa.PA_04, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }


                pa.Correctos++;
            }

            /// <summary>
            /// Codigo Postal | Numerico | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_05(ValidationResults results)
            {

                // 314-M El dato Codigo Postal es requerido.
                if (String.IsNullOrWhiteSpace(pa.PA_05))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("314-F", pa.Validaciones, pa.PA_05, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // Si es Mexicano aplicamos las validaciones de SEPOMEX
                if (!pa.EsExtranjero)
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

                    string CPTemp = CommonValidator.StrCP(pa.PA_05, 5);
                    string ResultadoSEPOMEX = CommonValidator.ValidarCPSEPOMEX_PF(CPTemp, pa.PA_04, pa.PA_02, pa.PA_03);

                    switch (ResultadoSEPOMEX)
                    {
                        case "01": // 315-F El dato Codigo Postal no se encontro en el catalogo de SEPOMEX.
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("315-F", pa.Validaciones, pa.PA_05, pa, pa.TypedParent.PN_05), "", "", null));
                            break;
                        case "02": // 316-F El dato Nombre de Estado para Domicilios en México no coincide con el C.P. (SEPOMEX).
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("316-F", pa.Validaciones, pa.PA_05, pa, pa.TypedParent.PN_05), "", "", null));
                            break;
                        case "03": // 317-F (Warning) El Municipio y la Ciudad no coinciden con el C.P. (SEPOMEX).
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("317-F", pa.Validaciones, pa.PA_05, pa, pa.TypedParent.PN_05), "", "", null));
                            break;
                        case "04": // 318-F (Warning) El dato Delegacion/Municipio no coincide con el C.P. (SEPOMEX), la Ciuda no fue reportada
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("318-F", pa.Validaciones, pa.PA_05, pa, pa.TypedParent.PN_05), "", "", null));
                            break;
                        case "05": // 319-F (Warning) El dato Ciudad no coincide con el C.P. (SEPOMEX), la Delegacion/Municipio no fue reportada
                            results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("319-F", pa.Validaciones, pa.PA_05, pa, pa.TypedParent.PN_05), "", "", null));
                            break;                      
                    }
                }

                // 320-F La longitud del dato Codigo Postal debe ser de 5 posiciones
                if (pa.PA_05.Length != 5)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("320-F", pa.Validaciones, pa.PA_05, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Fecha de Residencia | Fecha | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_06(ValidationResults results)
            {

                if (string.IsNullOrWhiteSpace(pa.PA_06))
                {
                    return;
                }

                // 321-F La longitud del dato Fecha de Residencia debe ser de 8 posiciones
                if (pa.PA_06.Length != 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("321-F", pa.Validaciones, pa.PA_06, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 322-F El formato del dato Fecha de Residencia es incorrecto (Formato correcto DDMMYYYY)
                Regex re = new Regex(WebConfig.PatronDeFecha);
                if (!re.IsMatch(pa.PA_06))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("322-F", pa.Validaciones, pa.PA_06, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Numero de Telefono | Numerico | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_07(ValidationResults results)
            {
                //ultrasist 20211108 campos telefono y mail obligatorios
                // 577-F El dato telefono es requerido.
                if (string.IsNullOrWhiteSpace(pa.PA_07))
                {
                    //results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("577-F", pa.Validaciones, pa.PA_07, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 323-F La longitud del dato Numero de Telefono es mayor de lo permitido (11 posiciones)
                if (pa.PA_07.Length > 11)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("323-F", pa.Validaciones, pa.PA_07, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 324-F El dato de Telefono debe contener solo digitos
                if (!CommonValidator.Match(pa.PA_07, CommonValidator.REGX_DIGITOS, true))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("324-F", pa.Validaciones, pa.PA_07, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Extension Telefonica | Numerico | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_08(ValidationResults results)
            {

                if (string.IsNullOrWhiteSpace(pa.PA_08))
                {
                    return;
                }

                // 325-F La longitud del dato Extension Telefonico es mayor de lo permitido (8 posiciones)
                if (pa.PA_08.Length > 8)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("325-F", pa.Validaciones, pa.PA_08, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 326-F El dato de Extension Telefonica debe contener solo digitos
                if (!CommonValidator.Match(pa.PA_09, CommonValidator.REGX_DIGITOS, true))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("326-F", pa.Validaciones, pa.PA_08, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Numero de Fax en esta Direccion | Numerico | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_09(ValidationResults results)
            {

                if (string.IsNullOrWhiteSpace(pa.PA_09))
                {
                    return;
                }

                // 327-F La longitud del dato Numero de Fax es mayor de lo permitido (11 posiciones)
                if (pa.PA_09.Length > 11)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("327-F", pa.Validaciones, pa.PA_09, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                 // 328-F El dato de Numero de Fax debe contener solo digitos
                if (!CommonValidator.Match(pa.PA_09, CommonValidator.REGX_DIGITOS, true))
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("328-F", pa.Validaciones, pa.PA_09, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Tipo de Domicilio | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_10(ValidationResults results)
            {

                if (string.IsNullOrWhiteSpace(pa.PA_10))
                {
                    return;
                }

                // 329-F La longitud del dato Tipo de Domicilio debe ser de 1 posicion
                if (pa.PA_10.Length != 1)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("329-F", pa.Validaciones, pa.PA_10, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 330-F El dato Tipo de Domicilio no se encontro en el catalogo correspondiente
                if (pa.PA_10 != "B" && pa.PA_10 != "C" && pa.PA_10 != "H" && pa.PA_10 != "P")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("330-F", pa.Validaciones, pa.PA_10, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Indicador Especial de Domicilio | Texto | Opcional
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_11(ValidationResults results)
            {

                if (string.IsNullOrWhiteSpace(pa.PA_11))
                {
                    return;
                }

                // 331-F La longitud del dato Indicador Especial de Domicilio debe ser de 1 posicion
                if (pa.PA_11.Length != 1)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("331-F", pa.Validaciones, pa.PA_11, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 332-F El dato Indicador Especial de Domicilio no se encontro en el catalogo correspondiente
                if (pa.PA_11 != "M" && pa.PA_11 != "R" && pa.PA_11 != "K")
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("332-F", pa.Validaciones, pa.PA_11, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                pa.Correctos++;
            }

            /// <summary>
            /// Origen del Domicilio (Pais) | Texto | Requerido
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void V_12(ValidationResults results)
            {

                if (string.IsNullOrEmpty(pa.PA_12))
                {
                    pa.PA_12 = "MX";
                }

                // 333-F La longitud del dato Origen del Domicilio debe ser de 2 posiciones
                if (pa.PA_12.Length != 2)
                {
                    results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("333-F", pa.Validaciones, pa.PA_12, pa, pa.TypedParent.PN_05), "", "", null));
                    return;
                }

                // 334-F El dato  Origen del Domicilio no se encontro en el catalogo correspondiente

                pa.Correctos++;
            }

        #endregion Metodos Validadores

    }

}
