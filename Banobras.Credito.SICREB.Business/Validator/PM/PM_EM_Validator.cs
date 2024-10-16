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
    public partial class PM_EM_Validator
    {

        private PM_EM em = null;
        public const string IDENTIFICADOR = "EM";

        public PM_EM_Validator(PM_EM em)
        {
            this.em = em;
        }

        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PM_EM_Validator> EMValidator = factory.CreateValidator<PM_EM_Validator>();
            return EMValidator.Validate(this);
        }

        #region Metodos Validadores

            /// <summary>
            /// Identificador del Segmento | Requerido | Valor Fijo EM
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiquetaEM(ValidationResults results)
            {

                // 201-M Valor de identificador EM inválido.
                if (em.EM_EM != IDENTIFICADOR)
                {
                    ValidacionEntry result = CommonValidator.GetValidacionEntry("201-M", em.Validaciones, em.EM_EM, em, em.EM_00);
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
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero el RFC es opcional
                // ----------------------------------------------------------
                if (em.EsExtranjero && String.IsNullOrWhiteSpace(em.EM_00))
                {
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 202-M El RFC es obligatorio para Clientes/Acreditados Mexicanos.
                if (!em.EsExtranjero && String.IsNullOrWhiteSpace(em.EM_00))
                {
                    result = CommonValidator.GetValidacionEntry("202-M", em.Validaciones, em.EM_00, em, em.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 203-M Formato inválido de RFC. Debe ser PM:AAANNNNNNZZZ (12 posiciones) ó PF:AAAANNNNNNZZZ (13 posiciones)
                // 204-M RFC reportado como fallecido.
                int codigo;
                if (!CommonValidator.V_Rfc(em.EM_00, false, out codigo))
                {
                    string codigo_error = (codigo == 102) ? "203-M" : "204-M";
                    result = CommonValidator.GetValidacionEntry(codigo_error, em.Validaciones, em.EM_00, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 205-M El RFC reportado como generico no es aceptado.
                if (CommonValidator.V_RFCGenerico(em.EM_00))
                {
                    result = CommonValidator.GetValidacionEntry("205-M", em.Validaciones, em.EM_00, em, em.MainRoot.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 206-M El RFC No debe contener caracteres especiales.
                // 207-M El RFC esta incompleto.
                // 208-M El cliente se reporta como Fideicomiso, pero el RFC no cumple las validaciones correspondientes.

                em.Correctos++;
            }

            /// <summary>
            /// Codigo de Ciudadano (CURP en México) | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta01(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Reservado | Requerido | Valor Fijo 10 Ceros
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta02(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Nombre de la Comañia (PM) | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta03(ValidationResults results)
            {
                ValidacionEntry result;

                // 209-M El Nombre de la Compañía es requerido para 1)Personas Morales, 3)Fondo o Fideicomiso, 4)Gobierno.
                if (em.EM_23 == "1" || em.EM_23 == "3" || em.EM_23 == "4")
                {
                    if (String.IsNullOrWhiteSpace(em.EM_03))
                    {
                        result = CommonValidator.GetValidacionEntry("209-M", em.Validaciones, em.EM_03, em, em.EM_00);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                        return;
                    }
                }

                em.Correctos++;
            }

            /// <summary>
            /// Primer Nombre (PFAE) | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta04(ValidationResults results)
            {
                ValidacionEntry result;

                // 210-M El Nombre del Acreditado es requerido para PF (Persona Fisica) y PFAE.
                if (em.EM_23 == "2")
                {
                    if (String.IsNullOrWhiteSpace(em.EM_04))
                    {
                        result = CommonValidator.GetValidacionEntry("210-M", em.Validaciones, em.EM_04, em, em.EM_00);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                        return;
                    }
                }

                em.Correctos++;
            }

            /// <summary>
            /// Segundo Nombre (PFAE) | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta05(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Apellido Paterno (PFAE) | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta06(ValidationResults results)
            {
                ValidacionEntry result;

                // 211-M El Apellido Paterno es requerido para PF (Persona Fisica) y PFAE.
                if (em.EM_23 == "2")
                {
                    if (String.IsNullOrWhiteSpace(em.EM_06))
                    {
                        result = CommonValidator.GetValidacionEntry("211-M", em.Validaciones, em.EM_06, em, em.EM_00);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                        return;
                    }
                }

                em.Correctos++;
            }

            /// <summary>
            /// Apellido Materno (PFAE) | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta07(ValidationResults results)
            {
                ValidacionEntry result;

                // 212-M El Apellido Materno es requerido para PF y PFAE, sino existe colocar la leyenda "NO PROPORCIONADO".
                if (em.EM_23 == "2")
                {
                    if (String.IsNullOrWhiteSpace(em.EM_07))
                    {
                        result = CommonValidator.GetValidacionEntry("212-M", em.Validaciones, em.EM_07, em, em.EM_00);
                        results.AddResult(new ValidationResult("", result, "", "", null));
                        return;
                    }
                }

                em.Correctos++;
            }

            /// <summary>
            /// Nacionalidad | Requerido | Texto (Catalogo Anexo 6)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta08(ValidationResults results)
            {
                ValidacionEntry result;

                // 213-M La Nacionalidad es requerida.
                if (String.IsNullOrWhiteSpace(em.EM_08))
                {
                    result = CommonValidator.GetValidacionEntry("213-M", em.Validaciones, em.EM_08, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 214-M No se encontro la Clave de Nacionalidad en el catalogo correspondiente.
                // *** REVISAR *** Por que en SICREB existe un catalogo de NACIONALIDAD y otro de PAIS si en el manual del buro
                // de credito se marca que se debe utilizar el mismo catalogo (Anexo 6)
                if (!CommonValidator.ValidarClavePais(em.EM_08))
                {
                    result = CommonValidator.GetValidacionEntry("214-M", em.Validaciones, em.EM_08, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

                em.Correctos++;
            }

            /// <summary>
            /// Calificacion de Cartera | Requerido | Texto (Catalogo ?)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta09(ValidationResults results)
            {
                ValidacionEntry result;

                // Alejandro Badillo - Se obtiene la calificacion mas alta
                string CalificacionCredito = CommonValidator.GetCalificacionAltoRiesgo(em.EM_00);

                // JAGH - Se da valor virtual para evitar entre a la validacion
                if (CalificacionCredito.Length == 0) CalificacionCredito = "NC";

                // 215-M El dato de Calificacion de Cartera no se encontro en el catalogo correspondiente.
                if (string.IsNullOrWhiteSpace(CalificacionCredito))
                {
                    result = CommonValidator.GetValidacionEntry("215-M", em.Validaciones, em.EM_09, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                em.Correctos++;
            }

            /// <summary>
            /// Actividad Economica 1 BANXICO/SCIAN | Requerido | Numerico (Catalogo)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta10(ValidationResults results)
            {
                ValidacionEntry result;

                // 216-M El dato Activida Economica 1 BANXICO/SCIAN es requerido.
                if (String.IsNullOrEmpty(em.EM_10))
                {
                    result = CommonValidator.GetValidacionEntry("216-M", em.Validaciones, em.EM_10, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 217-M Clave Banxico 1 incorrecta (No se localizo en el catalogo Correspondiente).
                if (!CommonValidator.V_Banxico(em.EM_10, false))
                {
                    result = CommonValidator.GetValidacionEntry("217-M", em.Validaciones, em.EM_10, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }

                em.Correctos++;
            }

            /// <summary>
            /// Actividad Economica 2 BANXICO/SCIAN | Requerido | Numerico (Catalogo)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta11(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(em.EM_11))
                { return; }

                // 218-M Clave Banxico 2 incorrecta (No se localizo en el catalogo Correspondiente).
                if (!CommonValidator.V_Banxico(em.EM_11, true))
                {
                    result = CommonValidator.GetValidacionEntry("218-M", em.Validaciones, em.EM_11, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Actividad Economica 3 BANXICO/SCIAN | Requerido | Numerico (Catalogo)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta12(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(em.EM_11))
                { return; }

                // 219-M Clave Banxico 3 incorrecta (No se localizo en el catalogo Correspondiente).
                if (!CommonValidator.V_Banxico(em.EM_12, true))
                {
                    result = CommonValidator.GetValidacionEntry("219-M", em.Validaciones, em.EM_12, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Primera Linea de Dirección | Complementario | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta13(ValidationResults results)
            {
                ValidacionEntry result;

                // 220-M El dato Primera Linea de Direccion es requerido.
                if (String.IsNullOrWhiteSpace(em.EM_13))
                {
                    result = CommonValidator.GetValidacionEntry("220-M", em.Validaciones, em.EM_13, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                em.Correctos++;
            }

            /// <summary>
            /// Segunda Linea de Dirección | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta14(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Colonia o Población | Requerido | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta15(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero la Colonia/Poblacion es opcional
                // ----------------------------------------------------------
                if (em.EsExtranjero)
                {
                    em.Correctos++;
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 221-M (Warning) El dato Colonia/Poblacion es requerido para Clientes Nacionales si se dispone de él.
                if (string.IsNullOrWhiteSpace(em.EM_15))
                {
                    result = CommonValidator.GetValidacionEntry("221-M", em.Validaciones, em.EM_15, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                em.Correctos++;
            }

            /// <summary>
            /// Delegación o Municipio | Complementario | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta16(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero la Delegacion/Municipio es opcional
                // ----------------------------------------------------------
                if (em.EsExtranjero)
                {
                    em.Correctos++;
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 222-M El dato Delegacion/Municipio es requerido si no se reporta la Ciudad.
                if (String.IsNullOrWhiteSpace(em.EM_16) && String.IsNullOrWhiteSpace(em.EM_17))
                {
                    result = CommonValidator.GetValidacionEntry("222-M", em.Validaciones, em.EM_16, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                em.Correctos++;
            }

            /// <summary>
            /// Ciudad | Complementario | Texto | -- EX -- | Opcional |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta17(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero la Delegacion/Municipio es opcional
                // ----------------------------------------------------------
                if (em.EsExtranjero)
                {
                    em.Correctos++;
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones
                // ----------------------------------------------------------

                // 223-M El dato Ciudad es requerido sino se reporta Delegacion/Municipio.
                if (String.IsNullOrWhiteSpace(em.EM_16) && String.IsNullOrWhiteSpace(em.EM_17))
                {
                    result = CommonValidator.GetValidacionEntry("223-M", em.Validaciones, em.EM_17, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }
             
                em.Correctos++;
            }

            /// <summary>
            /// Estado en México | Complementario | Texto (Catalogo Anexo 7)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta18(ValidationResults results)
            {
                ValidacionEntry result;

                // ----------------------------------------------------------
                // Si es Extranjero el Estado en México es opcional
                // ----------------------------------------------------------
                if (em.EsExtranjero)
                {
                    em.Correctos++;
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano y reporta domicilio aplicamos validaciones
                // ----------------------------------------------------------

                // 224-M El dato Nombre de Estado para Domicilios en México es requerido para clientes Nacionales.
                if (String.IsNullOrWhiteSpace(em.EM_18))
                {
                    result = CommonValidator.GetValidacionEntry("224-M", em.Validaciones, em.EM_18, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 225-M El dato Nombre de Estado para Domicilios en México no se encontro en el catalogo correspondiente.
                if (!CommonValidator.ValidarClaveEstado(em.EM_18, Enums.Persona.Moral))
                {
                    result = CommonValidator.GetValidacionEntry("225-M", em.Validaciones, em.EM_18, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                em.Correctos++;
            }

            /// <summary>
            /// Codigo Postal | Requerido | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta19(ValidationResults results)
            {
                ValidacionEntry result;

                // 226-M El dato Codigo Postal es requerido.
                if (String.IsNullOrWhiteSpace(em.EM_19))
                {
                    result = CommonValidator.GetValidacionEntry("226-M", em.Validaciones, em.EM_19, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // ----------------------------------------------------------
                // Si es Mexicano aplicamos validaciones de SEPOMEX
                // ----------------------------------------------------------

                if (!em.EsExtranjero)
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

                    string CPTemp = CommonValidator.StrCP(em.EM_19, 5);
                    string ResultadoSEPOMEX = CommonValidator.ValidarCPSEPOMEX_PM(CPTemp, em.EM_18, em.EM_16, em.EM_17);

                    switch (ResultadoSEPOMEX)
                    {
                        case "01": // 227-M El dato Codigo Postal no se encontro en el catalogo de SEPOMEX.
                            result = CommonValidator.GetValidacionEntry("227-M", em.Validaciones, em.EM_19, em, em.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            return;
                            //break;
                        case "02": // 228-M El dato Nombre de Estado para Domicilios en México no coincide con el C.P. (SEPOMEX).
                            result = CommonValidator.GetValidacionEntry("228-M", em.Validaciones, "C.P.: " + em.EM_19 + " Estado reportado: " + em.EM_18, em, em.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            // Como es Warning no aplicamos return ya que el registro es valido
                            break;
                        case "03": // 229-M (Warning) El Municipio y la Ciudad no coinciden con el C.P. (SEPOMEX).
                            result = CommonValidator.GetValidacionEntry("229-M", em.Validaciones, "C.P.: " + em.EM_19 + " Municipio: " + em.EM_16 + " Ciudad: " + em.EM_17, em, em.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            // Como es Warning no aplicamos return ya que el registro es valido
                            break;
                        case "04": // 230-M (Warning) El dato Delegacion/Municipio no coincide con el C.P. (SEPOMEX), la Ciuda no fue reportada
                            result = CommonValidator.GetValidacionEntry("230-M", em.Validaciones, "C.P.: " + em.EM_19 + " Municipio: " + em.EM_16, em, em.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));                           
                            // Como es Warning no aplicamos return ya que el registro es valido
                            break;
                        case "05": // 231-M (Warning) El dato Ciudad no coincide con el C.P. (SEPOMEX), la Delegacion/Municipio no fue reportada
                            result = CommonValidator.GetValidacionEntry("231-M", em.Validaciones, "C.P.: " + em.EM_19 + " Ciudad: " + em.EM_17, em, em.EM_00);
                            results.AddResult(new ValidationResult("", result, "", "", null));
                            return;
                            //break;
                        //case "10": TipoErrorSEPOMEX = "00-M"; break;  // El Estado no coincide con el Codigo Postal                       
                    }
                }

                em.Correctos++;
            }

            /// <summary>
            /// Numero de Telefono | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta20(ValidationResults results)
            {
                ValidacionEntry result;
             
                //ultrasist 20211108 campos telefono y mail obligatorios
                //el dato de telefono es requerido
                if (String.IsNullOrWhiteSpace(em.EM_20))
                {
                    em.Correctos++;
                    return;
                }
                //if (String.IsNullOrWhiteSpace(em.EM_20))
                //{
                //    result = CommonValidator.GetValidacionEntry("634-M", em.Validaciones, em.EM_20, em, em.EM_00);
                //    results.AddResult(new ValidationResult("", result, "", "", null));
                //    return;
                //}

                // 232-M El dato de Telefono debe contener solo digitos.
                if (!CommonValidator.Match(em.EM_20.Replace(" ", string.Empty), CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("232-M", em.Validaciones, em.EM_20, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                em.Correctos++;
            }

            /// <summary>
            /// Extensión Telefonica | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta21(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(em.EM_21))
                {
                    em.Correctos++;
                    return;
                }

                // 233-M El dato de Extension Telefonica debe contener solo digitos.
                if (!CommonValidator.Match(em.EM_21.Replace(" ", string.Empty), CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("233-M", em.Validaciones, em.EM_21, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Número de Fax | Opcional | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta22(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(em.EM_22))
                {
                    em.Correctos++;
                    return;
                }

                // 234-M El dato de Fax debe contener solo digitos.
                if (!CommonValidator.Match(em.EM_22.Replace(" ", string.Empty), CommonValidator.REGX_DIGITOS, true))
                {
                    result = CommonValidator.GetValidacionEntry("234-M", em.Validaciones, em.EM_22, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Tipo de Cliente | Requerido | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta23(ValidationResults results)
            {
                ValidacionEntry result;

                // 235-M El dato Tipo de Cliente es requerido.
                if (String.IsNullOrWhiteSpace(em.EM_23))
                {
                    result = CommonValidator.GetValidacionEntry("235-M", em.Validaciones, em.EM_23, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }

                // 236-M El dato Tipo de Cliente debe ser 1) PM, 2) PF/PFAE, 3) Fondo o Fideicomiso y 4) Gobierno
                if (em.EM_23 != "1" && em.EM_23 != "2" && em.EM_23 != "3" && em.EM_23 != "4")
                {
                    result = CommonValidator.GetValidacionEntry("236-M", em.Validaciones, "Cliente: " + em.EM_00 + ", " + em.EM_23, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Estado en el Extranjero | Complementario | Texto | -- EX -- | Requerido |
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta24(ValidationResults results)
            {
                ValidacionEntry result;

                // 237-M El dato Nombre del Estado en el Pais Extranjero es requerido para los domicilios extranjeros.
                if (em.EsExtranjero && String.IsNullOrWhiteSpace(em.EM_24))
                {
                    result = CommonValidator.GetValidacionEntry("237-M", em.Validaciones, "Cliente: " + em.EM_00 + ", " + em.EM_24, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Pais de Origen del Domicilio | Complementario | Texto | -- EX -- | Requerido |  (Catalogo Anexo 6)
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta25(ValidationResults results)
            {
                ValidacionEntry result;

                if (String.IsNullOrWhiteSpace(em.EM_25))
                {
                    em.EM_25 = "MX";
                }

                // 238-M El dato Pais de Origen del Domicilio no se encontro en el catalogo correspondiente. 
                if (!CommonValidator.ValidarClavePais(em.EM_25))
                {
                    result = CommonValidator.GetValidacionEntry("238-M", em.Validaciones, "Cliente: " + em.EM_00 + ", " + em.EM_25, em, em.EM_00);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                }
            }

            /// <summary>
            /// Clave de Consolidacion | Opcional | Numerico
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta26(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

            /// <summary>
            /// Filler | Requerido | Texto
            /// </summary>
            /// <param name="results"></param>
            [SelfValidation]
            public void CheckEtiqueta27(ValidationResults results)
            {
                // Sin validaciones que realizar.
            }

        #endregion

    }

}
