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
    public class PM_AC_Validator
    {

        private PM_AC ac = null;
        public const string IDENTIFICADOR = "AC";

        public PM_AC_Validator(PM_AC ac)
        {
            this.ac = ac;
        }

        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PM_AC_Validator> ACValidator = factory.CreateValidator<PM_AC_Validator>();
            return ACValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Identificador del Segmento | Requerido | Valor Fijo AC
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiquetaAC(ValidationResults results)
            {
                ValidacionEntry result;

                // 301-M Valor del identificador AC no válido
                if (ac.AC_AC != IDENTIFICADOR)
                {
                    result = CommonValidator.GetValidacionEntry("301-M", ac.Validaciones, ac.AC_AC, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// RFC del Accionista | Requerido | Texto | - EX - | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta00(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero el RFC es opcional
                // ----------------------------------------------------------
                if (ac.EsExtranjero && !String.IsNullOrWhiteSpace(ac.AC_00))
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 302-M El RFC es obligatorio para Accionistas Mexicanos.
                if (String.IsNullOrWhiteSpace(ac.AC_00))
                {
                    result = CommonValidator.GetValidacionEntry("302-M", ac.Validaciones, ac.AC_00, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 303-M Formato inválido de RFC. Debe ser PM:AAANNNNNNZZZ (12 posiciones) ó PF:AAAANNNNNNZZZ (13 posiciones)
                // 304-M RFC reportado como fallecido.
                int codigo;
                if (!CommonValidator.V_Rfc(ac.AC_00, false, out codigo))
                {
                    string codigo_error = (codigo == 102) ? "303-M" : "304-M";
                    result = CommonValidator.GetValidacionEntry(codigo_error, ac.Validaciones, ac.AC_00, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 305-M El RFC reportado como generico no es aceptado.
                if (CommonValidator.V_RFCGenerico(ac.AC_00))
                {
                    result = CommonValidator.GetValidacionEntry("305-M", ac.Validaciones, ac.AC_00, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 306-M El RFC No debe contener caracteres especiales.
                // 307-M El RFC esta incompleto.
                // 308-M El Accionista se reporta como Fideicomiso, pero el RFC no cumple las validaciones correspondientes.

                // 309-M El RFC del Accionistas no puede ser el mismo que el del Acreditado.
                if (ac.AC_00.Trim() == ac.MainRoot.EM_00.Trim())
                {
                    result = CommonValidator.GetValidacionEntry("309-M", ac.Validaciones, ac.AC_00, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
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
            /// Nombre de la Compania Accionista | Requerido | texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta03(ValidationResults results)
            {
                ValidacionEntry result;

                // 310-M El Nombre de la Compañía es requerido para 1)Personas Morales, 3)Fondo o Fideicomiso, 4)Gobierno.
                if (ac.AC_19 == "1" || ac.AC_19 == "3" || ac.AC_19 == "4")
                {
                    if (String.IsNullOrWhiteSpace(ac.AC_03))
                    {
                        result = CommonValidator.GetValidacionEntry("310-M", ac.Validaciones, "RFC Accionista: " + ac.AC_00 + ", Nombre: " + ac.AC_03, ac, ac.MainRoot.EM_00);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }
            }

            /// <summary>
            /// Primer Nombre del Accionista | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta04(ValidationResults results)
            {
                ValidacionEntry result;

                // 311-M El Nombre del Accionista es requerido para PF (Persona Fisica) y PFAE.
                if (ac.AC_19 == "2")
                {
                    if (String.IsNullOrWhiteSpace(ac.AC_04))
                    {
                        result = CommonValidator.GetValidacionEntry("311-M", ac.Validaciones, "RFC Accionista: " + ac.AC_00 + ", Nombre: " + ac.AC_04, ac, ac.MainRoot.EM_00);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }
            }

            /// <summary>
            /// Segundo Nombre del Accionista | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta05(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Apellido Paterno del Accionista | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta06(ValidationResults results)
            {
                ValidacionEntry result;

                // 312-M El Apellido Paterno es requerido para PF (Persona Fisica) y PFAE.
                if (ac.AC_19 == "2")
                {
                    if (String.IsNullOrWhiteSpace(ac.AC_06))
                    {
                        result = CommonValidator.GetValidacionEntry("312-M", ac.Validaciones, "RFC Accionista: " + ac.AC_00 + ", Apellido: " + ac.AC_06, ac, ac.MainRoot.EM_00);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }
            }

            /// <summary>
            /// Apellido Materno del Accionista | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta07(ValidationResults results)
            {
                ValidacionEntry result;

                // 313-M El Apellido Materno es requerido para PF y PFAE, sino existe colocar la leyenda "NO PROPORCIONADO".
                if (ac.AC_19 == "2")
                {
                    if (String.IsNullOrWhiteSpace(ac.AC_07))
                    {
                        result = CommonValidator.GetValidacionEntry("313-M", ac.Validaciones, "RFC Accionista: " + ac.AC_00 + ", Apellido: " + ac.AC_07, ac, ac.MainRoot.EM_00);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                    }
                }
            
            }

            /// <summary>
            /// Porcentaje del Accionista | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta08(ValidationResults results)
            {
                ValidacionEntry result;

                // 314-M El Porcentaje del Accionista es requerido.
                if (!CommonValidator.Match(ac.AC_08, CommonValidator.REGX_DIGITOS, false))
                {
                    result = CommonValidator.GetValidacionEntry("314-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_08, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", ac, "", "", null));
                }
            }

            /// <summary>
            /// Primera Dirección del Accionista | Complementario | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta09(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero el Domicilio es opcional
                // ----------------------------------------------------------
                if (ac.EsExtranjero)
                {
                    return;
                }

                if (!String.IsNullOrWhiteSpace(ac.AC_09) && !ac.EsExtranjero)
                {
                    // Si el Accionista es Mexicano y se reporta la Direccion, deberan aplicarse las validaciones de consistencia del domicilio. 
                    // Debe incluirse nombre de la calle, numero exterior e interior, cuando éste último dato exista.
                    return;
                }
                
                //if (String.IsNullOrWhiteSpace(ac.AC_09) && !ac.EsExtranjero)
                //{
                    // El campo de direccion es opcional, si no se incluye se omiten las validaciones de los siguientes campos
                    // ciudad, delegación/municipio, estado, código postal.
                //    return;
                //}

                // 315-M La Direccion del domicilio del accionista es requerida para Extranjeros.
                //if (String.IsNullOrWhiteSpace(ac.AC_09) && ac.EsExtranjero)
                //{
                //    result = CommonValidator.GetValidacionEntry("315-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_09, ac, ac.MainRoot.EM_00);
                //    results.AddResult(new ValidationResult("", result, "", "", null));
                //}                 
                 
            }

            /// <summary>
            /// Segunda Dirección del Accionista | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta10(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Colonia o Población | Complementario | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta11(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Delegación o Municipio | Complementario | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta12(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero la Delegacion/Municipio es opcional
                // ----------------------------------------------------------
                if (ac.EsExtranjero)
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                if (String.IsNullOrWhiteSpace(ac.AC_09) && !ac.EsExtranjero)
                {
                    // Para la version 3.0 del archivo de PM
                    // El campo de direccion es opcional, si no se incluye se omiten las validaciones de los siguientes campos
                    // ciudad, delegación/municipio, estado, código postal.
                    return;
                }

                // Si es Mexicano y reporta domicilio aplicamos validaciones

                // 316-M El dato Delegacion/Municipio es requerido si no se reporta la Ciudad.
                if (String.IsNullOrWhiteSpace(ac.AC_13) && String.IsNullOrWhiteSpace(ac.AC_12))
                {
                    result = CommonValidator.GetValidacionEntry("316-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_12, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }      
            }

            /// <summary>
            /// Ciudad | Complementario | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta13(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero la Ciudad es opcional
                // ----------------------------------------------------------
                if (ac.EsExtranjero)
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                if (String.IsNullOrWhiteSpace(ac.AC_09) && !ac.EsExtranjero)
                {
                    // Para la version 3.0 del archivo de PM
                    // El campo de direccion es opcional, si no se incluye se omiten las validaciones de los siguientes campos
                    // ciudad, delegación/municipio, estado, código postal.
                    return;
                }

                // Si es Mexicano y reporta domicilio aplicamos validaciones

                // 317-M El dato Ciudad es requerido sino se reporta Delegacion/Municipio.
                if (String.IsNullOrWhiteSpace(ac.AC_12) && String.IsNullOrWhiteSpace(ac.AC_13))
                {
                    result = CommonValidator.GetValidacionEntry("317-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_13, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Estado en México | Complementario | texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta14(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero el Estado en México es opcional
                // ----------------------------------------------------------
                if (ac.EsExtranjero)
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano y reporta domicilio aplicamos validaciones
                // ----------------------------------------------------------

                if (String.IsNullOrWhiteSpace(ac.AC_09) && !ac.EsExtranjero)
                {
                    // Para la version 3.0 del archivo de PM
                    // El campo de direccion es opcional, si no se incluye se omiten las validaciones de los siguientes campos
                    // ciudad, delegación/municipio, estado, código postal.
                    return;
                }

                // Si es Mexicano y reporta domicilio aplicamos validaciones

                // 318-M El dato Nombre de Estado para Domicilios en México es requerido para Accionistas Nacionales.
                if (String.IsNullOrWhiteSpace(ac.AC_14))
                {
                    result = CommonValidator.GetValidacionEntry("318-M", ac.Validaciones, ac.AC_14, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 319-M El dato Nombre de Estado para Domicilios en México no se encontro en el catalogo correspondiente.
                if (!CommonValidator.ValidarClaveEstado(ac.AC_14, Enums.Persona.Moral))
                {
                    result = CommonValidator.GetValidacionEntry("319-M", ac.Validaciones, ac.AC_14, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Codigo Postal | Requerido | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta15(ValidationResults results)
            {
                ValidacionEntry result;

                if (ac.EsExtranjero)
                {
                    // Si es extranjero se Omite el CP y sus validaciones.
                    return;
                }

                if (String.IsNullOrWhiteSpace(ac.AC_09) && !ac.EsExtranjero)
                {
                    // Para la version 3.0 del archivo de PM
                    // El campo de direccion es opcional, si no se incluye se omiten las validaciones de los siguientes campos
                    // ciudad, delegación/municipio, estado, código postal.
                    return;
                }

                // Si es Mexicano y reporta domicilio aplicamos validaciones

                // 320-M El dato Codigo Postal es requerido.
                if (String.IsNullOrWhiteSpace(ac.AC_15))
                {
                    result = CommonValidator.GetValidacionEntry("320-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_15, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // Si es Mexicano aplicamos las validaciones de SEPOMEX
                if (!ac.EsExtranjero)
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

                    string CPTemp = CommonValidator.StrCP(ac.AC_15, 5);
                    string ResultadoSEPOMEX = CommonValidator.ValidarCPSEPOMEX_PM(CPTemp, ac.AC_14, ac.AC_12, ac.AC_13);

                    switch (ResultadoSEPOMEX)
                    {
                        case "01": // 321-M El dato Codigo Postal no se encontro en el catalogo de SEPOMEX.
                            result = CommonValidator.GetValidacionEntry("321-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_15, ac, ac.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        case "02": // 322-M El dato Nombre de Estado para Domicilios en México no coincide con el C.P. (SEPOMEX).
                            result = CommonValidator.GetValidacionEntry("322-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_14, ac, ac.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        case "03": // 323-M (Warning) El Municipio y la Ciudad no coinciden con el C.P. (SEPOMEX).
                            result = CommonValidator.GetValidacionEntry("323-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_12 + ", " + ac.AC_13, ac, ac.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        case "04": // 324-M (Warning) El dato Delegacion/Municipio no coincide con el C.P. (SEPOMEX), la Ciuda no fue reportada
                            result = CommonValidator.GetValidacionEntry("324-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_12, ac, ac.MainRoot.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            break;
                        case "05": // 325-M (Warning) El dato Ciudad no coincide con el C.P. (SEPOMEX), la Delegacion/Municipio no fue reportada
                            result = CommonValidator.GetValidacionEntry("325-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_13, ac, ac.MainRoot.EM_00);
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
            public void CheckEtiqueta16(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(ac.AC_16))
                {
                    return;
                }

                // 326-M El dato de Telefono debe contener solo digitos.
                if (!CommonValidator.Match(ac.AC_16, CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("326-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_16, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Extension Telefonica | Opcional | Texto 
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta17(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(ac.AC_17))
                {
                    return;
                }

                // 327-M El dato de Extension Telefonica debe contener solo digitos.
                if (!CommonValidator.Match(ac.AC_17, CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("327-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_17, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Número de Fax | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta18(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(ac.AC_18))
                {
                    return;
                }

                // 328-M El dato de Fax debe contener solo digitos.
                if (!CommonValidator.Match(ac.AC_18, CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("328-M", ac.Validaciones, "Accionista: " + ac.AC_00 + ", " + ac.AC_18, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Tipo de Accionista | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta19(ValidationResults results)
            {
                ValidacionEntry result;

                // 329-M El dato Tipo de Accionista es requerido.
                if (String.IsNullOrWhiteSpace(ac.AC_19))
                {
                    result = CommonValidator.GetValidacionEntry("329-M", ac.Validaciones, "RFC Accionista: " + ac.AC_00 + ", Tipo: " + ac.AC_19, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 330-M El dato Tipo de Accionista debe ser 1) PM, 2) PF/PFAE, 3) Fondo o Fideicomiso y 4) Gobierno
                if (ac.AC_19 != "1" && ac.AC_19 != "2" && ac.AC_19 != "3" && ac.AC_19 != "4")
                {
                    result = CommonValidator.GetValidacionEntry("330-M", ac.Validaciones, "RFC Accionista: " + ac.AC_00 + ", Tipo: " + ac.AC_19, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Estado en el Extranjero | Complementario | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta20(ValidationResults results)
            {
                ValidacionEntry result;

                // 331-M El dato Nombre del Estado en el Pais Extranjero es requerido para los domicilios extranjeros.
                if (ac.EsExtranjero && String.IsNullOrWhiteSpace(ac.AC_20))
                {
                    result = CommonValidator.GetValidacionEntry("331-M", ac.Validaciones, "RFC Accionista: " + ac.AC_00 + ", Estado Extranjero: " + ac.AC_20, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Pais de Origen del Domicilio | Complementario | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta21(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(ac.AC_21))
                {
                    ac.AC_21 = "MX";
                }

                // 332-M El dato Pais de Origen del Domicilio no se encontro en el catalogo correspondiente.
                if (!CommonValidator.ValidarClavePais(ac.AC_21))
                {
                    result = CommonValidator.GetValidacionEntry("332-M", ac.Validaciones, "RFC Accionista: " + ac.AC_00 + ", Pais: " + ac.AC_21, ac, ac.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Filler | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta22(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

        #endregion

    }
}
