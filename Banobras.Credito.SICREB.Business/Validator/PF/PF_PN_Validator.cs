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

namespace Banobras.Credito.SICREB.Business.Validator.PF
{

    [HasSelfValidation]
    public class PF_PN_Validator
    {

        private PF_PN pn = null;

        public PF_PN_Validator(PF_PN pn)
        {
            this.pn = pn;
        }

        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PF_PN_Validator> HDValidator = factory.CreateValidator<PF_PN_Validator>();
            return HDValidator.Validate(this);
        }

        #region Metodos Validadores

        /// <summary>
        /// Apellido Paterno | Texto | Requerido
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_PN(ValidationResults results)
        {
            File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_PN + "" + pn.PN_PN + "  " + "\n");
            // 201-F El dato Apellido Paterno es requerido
            if (string.IsNullOrEmpty(pn.PN_PN))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("201-F", pn.Validaciones, pn.PN_PN, pn, pn.PN_05), "", "", null));
                return;
            }

            // 202-F La longitud del dato Apellido Paterno es mayor a lo permitido (26 posiciones)
            if (pn.PN_PN.Length > 26)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("202-F", pn.Validaciones, pn.PN_PN, pn, pn.PN_05), "", "", null));
                return;
            }
            pn.Correctos++;
        }

        /// <summary>
        /// Apellido Materno | Texto | Requerido
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_00(ValidationResults results)
        {

            // 203-F El dato Apellido Materno es requerido
            if (string.IsNullOrEmpty(pn.PN_00))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("203-F", pn.Validaciones, pn.PN_00, pn, pn.PN_05), "", "", null));
                return;
            }

            // 204-F La longitud de dato Apellido Materno mayor a lo permitido (26 posiciones)
            if (pn.PN_00.Length > 26 || pn.PN_00.Length == 1)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("204-F", pn.Validaciones, pn.PN_00, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Apellido Adicional | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_01(ValidationResults results)
        {

            if (string.IsNullOrEmpty(pn.PN_01))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_01 + " / " + pn.TLs + "\n");
                return;
            }

            // 205-F La longitud del dato Apellido Adicional mayor a lo permitido (26 posiciones)
            if (pn.PN_01.Length > 26 || pn.PN_01.Length == 1)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("205-F", pn.Validaciones, pn.PN_01, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Primer Nombre | Texto | Requerido
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_02(ValidationResults results)
        {

            // 206-F El dato Primer Nombre es requerido
            if (string.IsNullOrEmpty(pn.PN_02))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("206-F", pn.Validaciones, pn.PN_02, pn, pn.PN_05), "", "", null));
                return;
            }

            // 207-F La longitud del dato Primer Nombre mayor a lo permitido (26 posiciones)
            if (pn.PN_02.Length > 26 || pn.PN_02.Length == 1)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("207-F", pn.Validaciones, pn.PN_02, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Segundo Nombre | Texto | Requerido
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_03(ValidationResults results)
        {

            if (string.IsNullOrEmpty(pn.PN_03))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_03 + " / " + pn.TLs + "\n");
                return;
            }

            // 208-F La longitud del dato Segundo Nombre mayor a lo permitido (26 posiciones)
            if (pn.PN_03.Length > 26)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("208-F", pn.Validaciones, pn.PN_03, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Fecha de Nacimiento | Fecha | Requerido
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_04(ValidationResults results)
        {

            // 209-F El dato Fecha de Nacimiento es requerido
            if (string.IsNullOrEmpty(pn.PN_04))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("209-F", pn.Validaciones, pn.PN_04, pn, pn.PN_05), "", "", null));
                return;
            }

            // 210-F La longitud del dato Fecha de Nacimiento debe ser de 8 posiciones
            if (pn.PN_04.Length != 8)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("210-F", pn.Validaciones, pn.PN_04, pn, pn.PN_05), "", "", null));
                return;
            }

            // 211-F El formato de la fecha de Nacimiento no es correcto (Formato correcto DDMMYYYY)
            Regex re = new Regex(WebConfig.PatronDeFecha);
            if (!re.IsMatch(pn.PN_04))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("211-F", pn.Validaciones, pn.PN_04, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// RFC | Texto | Requerido
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_05(ValidationResults results)
        {

            // ----------------------------------------------------------
            // Si es Extranjero el RFC es opcional
            // ----------------------------------------------------------
            if (pn.EsExtranjero && String.IsNullOrWhiteSpace(pn.PN_00))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_00 + "  " + pn.TLs + "\n");
                return;
            }

            // ----------------------------------------------------------
            // Si es Mexicano aplicamos validaciones
            // ----------------------------------------------------------

            // 212-F El RFC es requerido para mexicanos
            if (string.IsNullOrEmpty(pn.PN_05))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("212-F", pn.Validaciones, pn.PN_05, pn), "", "", null));
                return;
            }

            // 213-F La longitud de dato RFC es mas largo de lo permitido (13 posiciones)
            if (pn.PN_05.Length > 13)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("213-F", pn.Validaciones, pn.PN_05, pn, pn.PN_05), "", "", null));
                return;
            }

            // 214-F El formato del RFC no es valido
            Regex re = new Regex(WebConfig.PatronDeRFCFisicas);
            if (!re.IsMatch(pn.PN_05))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("214-F", pn.Validaciones, pn.PN_05, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Prefijo Personal o Profesional | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_06(ValidationResults results)
        {

            if (string.IsNullOrEmpty(pn.PN_06))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_06 + " / " + pn.TLs + "\n");
                return;
            }

            // 215-F La longitud del dato Prefijo Personal  es mas largo de lo permitido (4 posiciones)
            if (pn.PN_06.Length > 4)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("215-F", pn.Validaciones, pn.PN_06, pn, pn.PN_05), "", "", null));
                return;
            }

            // 216-F El dato Prefjo Personal no se localizo en el catalogo correspondiente

            pn.Correctos++;
        }

        /// <summary>
        /// Sufijo Personal del Cliente | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_07(ValidationResults results)
        {

            if (string.IsNullOrEmpty(pn.PN_07))
            {
                File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_07 + "  " + "\n");
                return;
            }

            // 217-F El dato Sufijo Personal es mas largo de lo permitido (4 posiciones)
            if (pn.PN_07.Length > 4)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("217-F", pn.Validaciones, pn.PN_07, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Nacionalidad del Acreditado | Texto | Requerido
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_08(ValidationResults results)
        {

            // 218-F El dato Nacionalida del Acreditado es requerido
            if (string.IsNullOrEmpty(pn.PN_08))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("218-F", pn.Validaciones, pn.PN_08, pn, pn.PN_05), "", "", null));
                return;
            }

            // 219-F La longitud del dato Nacionalidad debe ser de 2 posiciones
            if (pn.PN_08.Length != 2)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("219-F", pn.Validaciones, pn.PN_08, pn, pn.PN_05), "", "", null));
                return;
            }

            // 220-F El dato Nacionalidad no se localizo en el catalogo correspondiente

            pn.Correctos++;
        }

        /// <summary>
        /// Tipo de Residencia | Numerico | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_09(ValidationResults results)
        {

            if (string.IsNullOrEmpty(pn.PN_09))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_09 + "  " + "\n");
                return;
            }

            // 221-F El dato Tipo de Residencia no es valido
            if (pn.PN_09 != "1" && pn.PN_09 != "2" && pn.PN_09 != "3")
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("221-F", pn.Validaciones, pn.PN_09, pn, pn.PN_05), "", "", null));
                return;
            }

            // 222-F La longitud del dato Tipo de Residencia es mayor a lo permitido (1 posicion)
            if (pn.PN_09.Length > 1)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("222-F", pn.Validaciones, pn.PN_09, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Numero de Licencia de Conducir | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_10(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_10))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_10 + "  " + "\n");
                return;
            }

            // 223-F La longitud del dato Numero de Licencia de Conducir es mayor a lo permitido (20 posiciones)
            if (pn.PN_10.Length > 20)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("223-F", pn.Validaciones, pn.PN_10, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Estado Civil | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_11(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_11))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_11 + "  " + "\n");
                return;
            }

            // 224-F La longitud del dato Estado Civil debe ser de 1 posicion
            if (pn.PN_11.Length != 1)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("224-F", pn.Validaciones, pn.PN_11, pn, pn.PN_05), "", "", null));
                return;
            }

            // 225-F El dato Estado Civil no se localizo en el catalogo correspondiente
            if (pn.PN_11 != "D" && pn.PN_11 != "F" && pn.PN_11 != "M" && pn.PN_11 != "S" && pn.PN_11 != "W")
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("225-F", pn.Validaciones, pn.PN_11, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Sexo | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_12(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_12))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_12 + "  " + "\n");
                return;
            }

            // 226-F La longitud del dato Sexo debe ser de 1 posicion
            if (pn.PN_12.Length != 1)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("226-F", pn.Validaciones, pn.PN_12, pn, pn.PN_05), "", "", null));
                return;
            }

            // 227-F El dato Sexo no se localizo en el catalogo correspondiente
            if (pn.PN_12 != "F" && pn.PN_12 != "M")
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("227-F", pn.Validaciones, pn.PN_12, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Numero de Cedula Profesional | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_13(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_13))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_13 + "  " + "\n");
                return;
            }

            // 228-F La longitud del dato Numero de Cedula es mayor a lo permitido (20 posiciones)
            if (pn.PN_13.Length > 20)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("228-F", pn.Validaciones, pn.PN_13, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Numero de Resgistro Electoral (IFE, INE) | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_14(ValidationResults results)
        {

            /*if (string.IsNullOrWhiteSpace(pn.PN_14))
            {
                File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_14 + "  " + "\n");
                return;
            }*/

            // 229-F La longitud del dato Numero de IFE es mayor a lo permitido (20 posiciones)
            if (pn.PN_14.Length > 20)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("229-F", pn.Validaciones, pn.PN_14, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Clave de Identificacion Unica (CURP en México) | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_15(ValidationResults results)
        {
            if (string.IsNullOrWhiteSpace(pn.PN_15))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_15 + "  " + "\n");
                return;
            }

            // 230-F La longitud del dato Clave de identificacion es mayor a lo permitido (20 posiciones)
            if (pn.PN_15.Length > 20)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("230-F", pn.Validaciones, pn.PN_15, pn, pn.PN_05), "", "", null));
                return;
            }

            // 231-F Si reporta el dato Clave de Identificacion debe reportar tambien el dato Clave de Pais
            if (!string.IsNullOrWhiteSpace(pn.PN_15) && string.IsNullOrWhiteSpace(pn.PN_16))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_01 + "  " + "\n");
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("231-F", pn.Validaciones, "Clave de Identificacion: " + pn.PN_15 + " Clave de Pais: " + pn.PN_16, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Clave de Pais | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_16(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_16))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_16 + "  " + "\n");
                return;
            }

            // 232-F La longitud del dato Clave de Pais debe ser de 2 posiciones
            if (pn.PN_16.Length != 2)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("232-F", pn.Validaciones, pn.PN_16, pn, pn.PN_05), "", "", null));
                return;
            }

            // 233-F El dato Clave de Pais no se localizo en el catalogo correspondiente

            pn.Correctos++;
        }

        /// <summary>
        /// Numero de Dependientes | Numerico | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_17(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_17))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_17 + "  " + "\n");
                return;
            }

            // 234-F La longitud del dato Numero de Dependientes debe ser de 2 posiciones
            if (pn.PN_17.Length != 2)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("234-F", pn.Validaciones, pn.PN_17, pn, pn.PN_05), "", "", null));
                return;
            }

            // 235-F El dato Numero de Dependientes debe contener solo digitos
            if (!CommonValidator.Match(pn.PN_17, CommonValidator.REGX_DIGITOS, true))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("235-F", pn.Validaciones, pn.PN_17, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Edades de los Dependientes | Numerico | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_18(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_18))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_18 + " " + "\n");
                return;
            }

            // 236-F La longitud del dato Edades de los Dependientes es mayor a lo permitido (30 posiciones)
            if (pn.PN_18.Length > 30)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("236-F", pn.Validaciones, pn.PN_18, pn, pn.PN_05), "", "", null));
                return;
            }

            // 237-F El dato Edades de los Dependientes debe contener solo digitos
            if (!CommonValidator.Match(pn.PN_18, CommonValidator.REGX_DIGITOS, true))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("237-F", pn.Validaciones, pn.PN_18, pn, pn.PN_05), "", "", null));
                return;
            }

            // 238-F La longitud del dato Edades de los Dependientes no puede ser inpar (todas las edades deben tener 2 cifras 01-99)
            if (pn.PN_18.Length % 2 != 0)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("238-F", pn.Validaciones, pn.PN_18, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Fecha de Defuncion | Fecha | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_20(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_20))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_20 + "  " + "\n");
                return;
            }

            // 239-F La longitud del dato Fecha de Defuncion debe ser de 8 digitos
            if (pn.PN_20.Length != 8)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("239-F", pn.Validaciones, pn.PN_20, pn, pn.PN_05), "", "", null));
                return;
            }

            // 240-F El formato de la Fecha de Defuncion no es correcto (Formato correcto DDMMYYYY)
            Regex re = new Regex(WebConfig.PatronDeFecha);
            if (!re.IsMatch(pn.PN_20))
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("240-F", pn.Validaciones, pn.PN_20, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        /// <summary>
        /// Indicador de Defuncion | Texto | Opcional
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void V_21(ValidationResults results)
        {

            if (string.IsNullOrWhiteSpace(pn.PN_21))
            {
                //File.WriteAllText(@"c:\a\errorsPN_PROCESO.txt", pn.PN_05 + "" + pn.PN_21 + "  " + "\n");
                return;
            }

            // 241-F La longitud del Indicador de Defuncion debe ser de 1 posicion
            if (pn.PN_21.Length != 1)
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("241-F", pn.Validaciones, pn.PN_21, pn, pn.PN_05), "", "", null));
                return;
            }

            // 242-F El Indicador de Defuncion no es valido
            if (pn.PN_21 != "Y")
            {
                results.AddResult(new ValidationResult("", CommonValidator.GetValidacionEntry("242-F", pn.Validaciones, pn.PN_21, pn, pn.PN_05), "", "", null));
                return;
            }

            pn.Correctos++;
        }

        #endregion Metodos Validadores

    }

}
