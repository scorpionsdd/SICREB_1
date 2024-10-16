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
    public partial class PM_AV_Validator
    {

        private PM_AV av = null;
        public const string IDENTIFICADOR = "AV";

        public PM_AV_Validator(PM_AV av)
        {
            this.av = av;
        }
       
        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PM_AV_Validator> AVValidator = factory.CreateValidator<PM_AV_Validator>();
            return AVValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Identificador del Segmento | Requerido | Valor Fijo AV
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiquetaAV(ValidationResults results)
            {
                ValidacionEntry result;

                // 601-M Valor del identificador AV no válido
                if (av.AV_AV != IDENTIFICADOR)
                {
                    result = CommonValidator.GetValidacionEntry("601-M", av.Validaciones, av.AV_AV, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// RFC del Aval | Requerido | Texto | - EX - | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta00(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero el RFC es opcional
                // ----------------------------------------------------------
                if (av.EsExtranjero && !String.IsNullOrWhiteSpace(av.AV_00))
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 602-M El RFC es obligatorio para Avales Mexicanos.
                if (String.IsNullOrWhiteSpace(av.AV_00))
                {
                    result = CommonValidator.GetValidacionEntry("602-M", av.Validaciones, av.AV_00, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 603-M Formato inválido de RFC. Debe ser PM:AAANNNNNNZZZ (12 posiciones) ó PF:AAAANNNNNNZZZ (13 posiciones)
                // 604-M RFC reportado como fallecido.
                int codigo;
                if (!CommonValidator.V_Rfc(av.AV_00, false, out codigo))
                {
                    string codigo_error = (codigo == 102) ? "603-M" : "604-M";
                    result = CommonValidator.GetValidacionEntry(codigo_error, av.Validaciones, av.AV_00, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 605-M El RFC reportado como generico no es aceptado.
                if (CommonValidator.V_RFCGenerico(av.AV_00))
                {
                    result = CommonValidator.GetValidacionEntry("605-M", av.Validaciones, av.AV_00, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 606-M El RFC No debe contener caracteres especiales.
                // 607-M El RFC esta incompleto.
                // 608-M El cliente se reporta como Fideicomiso, pero el RFC no cumple las validaciones correspondientes.

                // 609-M El RFC del Aval no puede ser el mismo que el del Acreditado.
                if (av.AV_00.Trim() == av.MainRoot.EM_00.Trim())
                {
                    result = CommonValidator.GetValidacionEntry("609-M", av.Validaciones, av.AV_00, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

            }

            /// <summary>
            /// Codigo de Ciudadano | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta01(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Campo Reservado | Requerido | Valor Fijo 10 Ceros
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta02(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Nombre de la Compania Aval | Requerido | texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta03(ValidationResults results)
            {
                ValidacionEntry result;

                // 610-M El Nombre de la Compañía es requerido para 1)Personas Morales, 3)Fondo o Fideicomiso, 4)Gobierno.
                if (av.AV_18 == "1" || av.AV_18 == "3" || av.AV_18 == "4")
                {
                    if (String.IsNullOrWhiteSpace(av.AV_03))
                    {
                        result = CommonValidator.GetValidacionEntry("610-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_03, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }
            }

            /// <summary>
            /// Primer Nombre del Aval | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta04(ValidationResults results)
            {
                ValidacionEntry result;

                // 611-M El Nombre del Aval es requerido para PF (Persona Fisica) y PFAE.
                if (av.AV_18 == "2")
                {
                    if (String.IsNullOrWhiteSpace(av.AV_04))
                    {
                        result = CommonValidator.GetValidacionEntry("611-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_04, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }
            }

            /// <summary>
            /// Segundo Nombre del Aval | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta05(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Apellido Paterno del Aval | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta06(ValidationResults results)
            {
                ValidacionEntry result;

                if (av.AV_18 == "2")
                {
                    // 612-M El Apellido Paterno es requerido para PF (Persona Fisica) y PFAE.
                    if (String.IsNullOrWhiteSpace(av.AV_06))
                    {
                        result = CommonValidator.GetValidacionEntry("612-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_06, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }

                    // 613-M El Apellido Paterno no puede incluir caracteres especiales.
                    if (CommonValidator.V_CaracteresEspeciales(av.AV_06))
                    {
                        result = CommonValidator.GetValidacionEntry("613-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_06, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }
            }

            /// <summary>
            /// Apellido Materno del Aval | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta07(ValidationResults results)
            {
                ValidacionEntry result;

                // 614-M El Apellido Materno es requerido para PF y PFAE, sino existe colocar la leyenda "NO PROPORCIONADO".
                if (av.AV_18 == "2")
                {
                    if (String.IsNullOrWhiteSpace(av.AV_07))
                    {
                        result = CommonValidator.GetValidacionEntry("614-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_07, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }
            }

            /// <summary>
            /// Primera Dirección del Aval | Requerido | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta08(ValidationResults results)
            {
                ValidacionEntry result;

                // 615-M La Direccion del domicilio del aval es requerida.
                if (String.IsNullOrWhiteSpace(av.AV_08))
                {
                    result = CommonValidator.GetValidacionEntry("615-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_08, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Segunda Dirección del Aval | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta09(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Colonia o Población | Complementario | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta10(ValidationResults results)
            {
                ValidacionEntry result;

                // 616-M (Warning) Se requiere el campo de Colonia o Poblacion si se dispone de él
                if (String.IsNullOrWhiteSpace(av.AV_10))
                {
                    result = CommonValidator.GetValidacionEntry("616-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_10, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Delegación o Municipio | Complementario | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta11(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero la Delegacion/Municipio es opcional
                // ----------------------------------------------------------
                if (av.EsExtranjero)
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 617-M El dato Delegacion/Municipio es requerido si no se reporta la Ciudad.
                if (String.IsNullOrWhiteSpace(av.AV_12) && String.IsNullOrWhiteSpace(av.AV_11))
                {
                    result = CommonValidator.GetValidacionEntry("617-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_11, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }      
            }

            /// <summary>
            /// Ciudad | Complementario | Texto | -- EX -- | Opcional | 
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta12(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero la Ciudad es opcional
                // ----------------------------------------------------------
                if (av.EsExtranjero)
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 618-M El dato Ciudad es requerido sino se reporta Delegacion/Municipio.
                if (String.IsNullOrWhiteSpace(av.AV_11) && String.IsNullOrWhiteSpace(av.AV_12))
                {
                    result = CommonValidator.GetValidacionEntry("618-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_12, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Estado en México | Complementario | texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta13(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero el Estado en México es opcional
                // ----------------------------------------------------------
                if (av.EsExtranjero)
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 619-M El dato Nombre de Estado para Domicilios en México es requerido para Avales Nacionales.
                if (String.IsNullOrWhiteSpace(av.AV_13))
                {
                    result = CommonValidator.GetValidacionEntry("619-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_13, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 620-M El dato Nombre de Estado para Domicilios en México no se encontro en el catalogo correspondiente.
                if (!CommonValidator.ValidarClaveEstado(av.AV_13, Enums.Persona.Moral ))
                {
                    result = CommonValidator.GetValidacionEntry("620-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_13, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Codigo Postal | Requerido | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta14(ValidationResults results)
            {
                ValidacionEntry result;

                // 621-M El dato Codigo Postal es requerido.
                if (String.IsNullOrWhiteSpace(av.AV_14))
                {
                    result = CommonValidator.GetValidacionEntry("621-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_14, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

                // Si es Mexicano aplicamos las validaciones de SEPOMEX
                if (!av.EsExtranjero)
                {
                    // 00) Si no existe es la Clave del Estado en el catalogo se rechaza el Registro.           Rechazado
                    // 01) El Codigo Postal no existe en el catalogo de SEPOMEX.                                Rechazado
                    // 02) Si el Estado del catalogo y el Estado del CP no coinciden se rechaza el Registro.    Rechazado
                    // 03) EstadoCP = EstadoR       MunicipioCP != MunicipioR       CiudadCP != CiudadR         Warning
                    // 04) EstadoCP = EstadoR       MunicipioCP != MunicipioR                                   Warning
                    // 05) EstadoCP = EstadoR                                       CiudadCP != CiudadR         Warning
                    // 06) EstadoCP = EstadoR       MunicipioCP = MunicipioR        CiudadCP = CiudadR          Aceptado
                    // 07) EstadoCP = EstadoR       MunicipioCP = MunicipioR                                    Aceptado
                    // 08) EstadoCP = EstadoR                                       CiudadCP = CiudadR          Aceptado
                    // 09) EstadoCP = EstadoR       MunicipioCP != MunicipioR       CiudadCP = CiudadR          Aceptado
                    // 10) EstadoCP = EstadoR       MunicipioCP = MunicipioR        CiudadCP != CiudadR         Rechazado

                    string CPTemp = CommonValidator.StrCP(av.AV_14, 5);
                    string ResultadoSEPOMEX = CommonValidator.ValidarCPSEPOMEX_PM(CPTemp, av.AV_13, av.AV_11, av.AV_12);

                    switch (ResultadoSEPOMEX)
                    {
                        case "01": // 622-M El dato Codigo Postal no se encontro en el catalogo de SEPOMEX.
                            result = CommonValidator.GetValidacionEntry("622-M", av.Validaciones, "Aval: " + av.AV_00 + " CP: " + av.AV_14, av, av.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        case "02": // 623-M El dato Nombre de Estado para Domicilios en México no coincide con el C.P. (SEPOMEX).
                            result = CommonValidator.GetValidacionEntry("623-M", av.Validaciones, "Aval: " + av.AV_00 + " CP: " + av.AV_14 + " Estado: " + av.AV_13, av, av.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        case "03": // 624-M (Warning) El Municipio y la Ciudad no coinciden con el C.P. (SEPOMEX).
                            result = CommonValidator.GetValidacionEntry("624-M", av.Validaciones, "Aval: " + av.AV_00 + " CP: " + av.AV_14 + " Municipio: " + av.AV_11 + " Ciudad: " + av.AV_12, av, av.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        case "04": // 625-M (Warning) El dato Delegacion/Municipio no coincide con el C.P. (SEPOMEX), la Ciuda no fue reportada
                            result = CommonValidator.GetValidacionEntry("625-M", av.Validaciones, "Aval: " + av.AV_00 + " CP: " + av.AV_14 + " Municipio: " + av.AV_11, av, av.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        case "05": // 626-M (Warning) El dato Ciudad no coincide con el C.P. (SEPOMEX), la Delegacion/Municipio no fue reportada
                            result = CommonValidator.GetValidacionEntry("626-M", av.Validaciones, "Aval: " + av.AV_00 + " CP: " + av.AV_14 + " Ciudad: " + av.AV_12, av, av.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        //case "10": TipoErrorSEPOMEX = "00-M"; break;  // El Estado no coincide con el Codigo Postal                       
                    }
                }
            }

            /// <summary>
            /// Número de Telefono | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta15(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(av.AV_15))
                {
                    return;
                }

                // 627-M El dato de Telefono debe contener solo digitos.
                if (!CommonValidator.Match(av.AV_15, CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("627-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_15, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Extension Telefonica | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta16(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(av.AV_16))
                {
                    return;
                }

                // 628-M El dato de Extension Telefonica debe contener solo digitos.
                if (!CommonValidator.Match(av.AV_16, CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("628-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_16, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Número de Fax | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta17(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(av.AV_17))
                {
                    return;
                }

                // 629-M El dato de Fax debe contener solo digitos.
                if (!CommonValidator.Match(av.AV_17, CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("629-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_17, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Tipo de Aval | Requerido | 1 = Persona Moral
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta18(ValidationResults results)
            {
                ValidacionEntry result;

                // 630-M El dato Tipo de Aval es requerido.
                if (String.IsNullOrWhiteSpace(av.AV_18))
                {
                    result = CommonValidator.GetValidacionEntry("630-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_18, av, av.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 631-M El dato Tipo de Aval debe ser 1) PM, 2) PF/PFAE, 3) Fondo o Fideicomiso y 4) Gobierno
                if (av.AV_18 != "1" && av.AV_18 != "2" && av.AV_18 != "3" && av.AV_18 != "4")
                {
                    result = CommonValidator.GetValidacionEntry("631-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_18, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Estado en el Extranjero | Complementario | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta19(ValidationResults results)
            {
                ValidacionEntry result;

                // 632-M El dato Nombre del Estado en el Pais Extranjero es requerido para los domicilios extranjeros.
                if (av.EsExtranjero && String.IsNullOrWhiteSpace(av.AV_19))
                {
                    result = CommonValidator.GetValidacionEntry("632-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_19, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Pais de Origen del Domicilio | Complementario | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta20(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(av.AV_20))
                {
                    av.AV_20 = "MX";
                }

                // 633-M El dato Pais de Origen del Domicilio no se encontro en el catalogo correspondiente.
                if (!CommonValidator.ValidarClavePais(av.AV_20))
                {
                    result = CommonValidator.GetValidacionEntry("633-M", av.Validaciones, "Aval: " + av.AV_00 + ", " + av.AV_20, av, av.MainRoot.EM_00, av.ParentCR.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Filler | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta21(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

        #endregion

    }
}
