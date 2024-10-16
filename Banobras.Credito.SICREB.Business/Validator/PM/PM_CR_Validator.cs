using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Common.Validator.PM;
using System.IO;

namespace Banobras.Credito.SICREB.Business.Validator.PM
{

    [HasSelfValidation]
    public class PM_CR_Validator
    {

        private PM_CR cr = null;
        public const string IDENTIFICADOR = "CR";

        public PM_CR_Validator(PM_CR cr)
        {
            this.cr = cr;
        }

        public ValidationResults Valida()
        {
            ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
            Validator<PM_CR_Validator> CRValidator = factory.CreateValidator<PM_CR_Validator>();

            return CRValidator.Validate(this);
        }

        /// <summary>
        /// Identificador del Segmento | Requerido | Valor Fijo CR
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiquetaCR(ValidationResults results)
        {
            ValidacionEntry result;

            // 401-M Valor del identificador CR no válido
            if (cr.CR_CR != IDENTIFICADOR)
            {
                result = CommonValidator.GetValidacionEntry("401-M", cr.Validaciones, cr.CR_CR, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
            }
        }

        /// <summary>
        /// RFC del Acreditado | Requerido | Texto
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta00(ValidationResults results)
        {
            ValidacionEntry result;

            // ----------------------------------------------------------
            // Si es Extranjero el RFC es opcional
            // ----------------------------------------------------------
            if (cr.EsExtranjero && String.IsNullOrWhiteSpace(cr.CR_00))
            {
                cr.Correctos++;
                return;
            }

            // ----------------------------------------------------------
            // Si es Mexicano aplicamos validaciones
            // ----------------------------------------------------------

            // 402-M El RFC es obligatorio para Acreditados Mexicanos.
            if (String.IsNullOrWhiteSpace(cr.CR_00))
            {
                result = CommonValidator.GetValidacionEntry("402-M", cr.Validaciones, cr.CR_00, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 403-M Formato inválido de RFC. Debe ser PM:AAANNNNNNZZZ (12 posiciones) ó PF:AAAANNNNNNZZZ (13 posiciones)
            // 404-M RFC reportado como fallecido.
            int codigo;
            if (!CommonValidator.V_Rfc(cr.CR_00, false, out codigo))
            {
                string codigo_error = (codigo == 102) ? "403-M" : "404-M";
                result = CommonValidator.GetValidacionEntry(codigo_error, cr.Validaciones, cr.CR_00, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 405-M El RFC reportado como generico no es aceptado.
            if (CommonValidator.V_RFCGenerico(cr.CR_00))
            {
                result = CommonValidator.GetValidacionEntry("405-M", cr.Validaciones, cr.CR_00, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 406-M El RFC No debe contener caracteres especiales.
            // 407-M El RFC esta incompleto.
            // 408-M El Acreditado se reporta como Fideicomiso, pero el RFC no cumple las validaciones correspondientes.

            // 409-M El RFC del Acreditado no coincide con el reportado en el Segmento EM.
            if (cr.MainRoot.EM_00.Trim() != cr.CR_00.Trim())
            {
                result = CommonValidator.GetValidacionEntry("409-M", cr.Validaciones, cr.CR_00, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Numero de Experiencias Crediticias | No Aplica para Institucion Financiera | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta01(ValidationResults results)
        {
            // Sin validaciones que realizar.
        }

        /// <summary>
        /// Numero de Credito o Contrato | Requerido | Texto
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta02(ValidationResults results)
        {
            ValidacionEntry result;

            // 410-M El dato Numero de Credito o Contrato es requerido.
            if (String.IsNullOrWhiteSpace(cr.CR_02))
            {
                result = CommonValidator.GetValidacionEntry("410-M", cr.Validaciones, cr.CR_02, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Numero de Credito, Cuenta o Contrato Anterior | Opcional | Texto
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta03(ValidationResults results)
        {
            ValidacionEntry result;

            // 411-M El dato Numero de Cuenta o Contrado Anterior se debe reportar cuando es un caso de Reestructura.

            if (String.IsNullOrWhiteSpace(cr.CR_03) && !CommonValidator.EmptyDate(cr.CR_13))
            {
                result = CommonValidator.GetValidacionEntry("411-M", cr.Validaciones, "Credito/Cuenta anterior: " + cr.CR_03 + " Fecha Restructura: " + cr.CR_13, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }
        }

        /// <summary>
        /// Fecha de Apertura del Credito | Requerido | Fecha
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta04(ValidationResults results)
        {

            ValidacionEntry result;
            bool todoCorrecto = true;

            // 412-M La Fecha de Apertura es requerida.
            if (CommonValidator.EmptyDate(cr.CR_04))
            {
                result = CommonValidator.GetValidacionEntry("412-M", cr.Validaciones, cr.CR_04, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                todoCorrecto = false;
                return;
            }

            bool? compareDates = CommonValidator.ComparaFecha(cr.CR_04, cr.PeriodoHD05, 15);
            // 413-M El formato de la Fecha de Apertura es invalido.
            if (compareDates == null)
            {
                result = CommonValidator.GetValidacionEntry("413-M", cr.Validaciones, cr.CR_04, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                todoCorrecto = false;
                return;
            }

            // 414-M La Fecha de Apertura del Credito es mayor a 15 días del periodo reportado.
            if (compareDates == true)
            {
                result = CommonValidator.GetValidacionEntry("414-M", cr.Validaciones, cr.CR_04, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                todoCorrecto = false;
                return;
            }

            if (!String.IsNullOrWhiteSpace(cr.CR_15))
            {
                // 415-M La Fecha de Apertura es Mayor a la Fecha de Liquidación
                if (CommonValidator.ComparaFecha(cr.CR_04, cr.CR_15, 0) == true)
                {
                    result = CommonValidator.GetValidacionEntry("415-M", cr.Validaciones, "Fecha Apertura: " + cr.CR_04 + " Fecha Liquidacion: " + cr.CR_15, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                    return;
                }

                // 416-M La Fecha de Apertura es Igual a la Fecha de Liquidación
                if (cr.CR_04.Trim() == cr.CR_15.Trim())
                {
                    result = CommonValidator.GetValidacionEntry("416-M", cr.Validaciones, "Fecha Apertura: " + cr.CR_04 + " Fecha Liquidacion: " + cr.CR_15, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                    return;
                }
            }

            // 417-M El Credito no cumple los criterios de la circular 17/2014 (Fecha de Apertura > 04/Octubre/2014)
            if (CommonValidator.ComparaFecha(cr.CR_04, "04102014", 0) == true)
            {
                // Validar en esta parte los criterios de la circular.
                // 1) Longitud del campo Plazo en Meses (CR_05) pasa de 5 a 6 posiciones
                // 2) Longitud del campo Frecuencia de Pagos (CR_10) pasa de 3 a 5 posiciones
                // 3) El Plazo debe estar en Meses (redondeado a 2 decimales) aplicando la formula y factor de conversion para pasar de dias a meses.

                // Solo podemos validar el punto 3 confirmando que para la version 4 del archivo PM, el dato Plazo contenga un punto decimal
                // lo cual indicaria que se aplico la conversion a meses.
                if (!cr.CR_05.Contains("."))
                {
                    result = CommonValidator.GetValidacionEntry("417-M", cr.Validaciones, "Fecha de Apertura: " + cr.CR_04 + " Plazo: " + cr.CR_05, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                    return;
                }
            }

            if (todoCorrecto)
            {
                cr.Correctos++;
            }
        }

        /// <summary>
        /// Plazo en Meses | Requerido | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta05(ValidationResults results)
        {

            ValidacionEntry result;
            double plazo = Parser.ToDouble(cr.CR_05);

            // 418-M El dato Plazo en Meses es requerido.
            if (String.IsNullOrWhiteSpace(cr.CR_05))
            {
                result = CommonValidator.GetValidacionEntry("418-M", cr.Validaciones, cr.CR_05, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 419-M La Fecha de Vencimiento debe ser mayor a la Fecha de Apertura.
            if (plazo < 0)
            {
                result = CommonValidator.GetValidacionEntry("419-M", cr.Validaciones, "Fecha Apertura: " + cr.CR_04 + " Plazo: " + cr.CR_05, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 420-M El dato Plazo en Meses solo puede ser = 0, para creditos especiales.
            if (plazo == 0)
            {
                if (!CommonValidator.ValidTipoCreditoEspecial("CR", "05", cr.CR_06))
                {
                    result = CommonValidator.GetValidacionEntry("420-M", cr.Validaciones, " Plazo: " + cr.CR_05 + " Tipo Credito: " + cr.CR_06, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Tipo de Credito | Requerido | Numerico (Catalogo Anexo 2)
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta06(ValidationResults results)
        {
            ValidacionEntry result;

            int creditoId;
            int.TryParse(cr.CR_06, out creditoId);

            // 421-M El Tipo de Crédito no se encontro en el catalogo correspondiente.
            string cred = CommonValidator.GetCredito(cr.CR_06, string.Empty);
            if (String.IsNullOrWhiteSpace(cred))
            {
                /*string lineas = "421-M " + cr.Validaciones + " CR_06 : " + cr.CR_06 + " ad: " + cr.CR_02 ;
                using (StreamWriter mylogs = File.AppendText(@"c:\a\errorsGL.txt"))         //se crea el archivo
                {
                    DateTime dateTime = new DateTime();
                    dateTime = DateTime.Now;
                    string strDate = Convert.ToDateTime(dateTime).ToString("hh:mm:ss dd-MM-yyyy");
                    mylogs.WriteLine(strDate + " --- " + lineas + " -- ");
                    mylogs.Close();
                }*/


                result = CommonValidator.GetValidacionEntry("421-M", cr.Validaciones, cr.CR_06, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 422-M El Tipo de Crédito pertenece a los crédios exceptuados.
            string exeptuado = CommonValidator.GetExceptuados(cr.CR_06, string.Empty);
            if (!String.IsNullOrWhiteSpace(exeptuado))
            {
                result = CommonValidator.GetValidacionEntry("422-M", cr.Validaciones, cr.CR_06, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, cr.CR_03, string.Empty, null));
                return;
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Monto Autorizado del Credito (Saldo Inicial) | Requerido | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta07(ValidationResults results)
        {

            ValidacionEntry result;

            // 423-M El dato Monto Autorizado del Credito (Saldo Inicial) es requerido.
            if (String.IsNullOrWhiteSpace(cr.CR_07))
            {
                result = CommonValidator.GetValidacionEntry("423-M", cr.Validaciones, cr.CR_07, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            double saldo = Parser.ToDouble(cr.CR_07);

            // 424-M El dato Saldo Inicial no puede ser menor a Cero.
            if (saldo < 0)
            {
                result = CommonValidator.GetValidacionEntry("424-M", cr.Validaciones, cr.CR_07, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 425-M Solo se pueden reportar con Saldo Inicial = 0 los creditos especiales con Cuentas Revolventes.
            if (saldo == 0)
            {
                if (!CommonValidator.ValidTipoCreditoEspecial("CR", "07", cr.CR_06))
                {
                    result = CommonValidator.GetValidacionEntry("425-M", cr.Validaciones, "Saldo Inicial: " + cr.CR_07 + " Tipo Credito: " + cr.CR_06, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Moneda | Requerido | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta08(ValidationResults results)
        {

            ValidacionEntry result;

            // 426-M El dato Moneda es requerido.
            if (String.IsNullOrWhiteSpace(cr.CR_08))
            {
                result = CommonValidator.GetValidacionEntry("426-M", cr.Validaciones, cr.CR_08, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 427-M El dato Tipo de Moneda no se encontro en el catalogo correspondiente.

            cr.Correctos++;
        }

        /// <summary>
        /// Numero de Pagos | Opcional | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta09(ValidationResults results)
        {
            ValidacionEntry result;

            // 428-M Solo se pueden reportar el Numero de Pagos = 0 para creditos especiales con Cuentas Revolventes.
            if (!String.IsNullOrWhiteSpace(cr.CR_09))
            {
                Int32 NumPagos = Parser.ToNumber(cr.CR_09);
                if (!CommonValidator.ValidTipoCreditoEspecial("CR", "09", cr.CR_06) && NumPagos == 0)
                {
                    result = CommonValidator.GetValidacionEntry("428-M", cr.Validaciones, "No. Pagos: " + cr.CR_09 + " Tipo Credito: " + cr.CR_06, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    return;
                }
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Frecuencia de Pagos | Opcional | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta10(ValidationResults results)
        {
            ValidacionEntry result;

            // 429-M (Warning) El dato Frecuencia de Pagos no puede estar vacio en un credito Activo.
            if (cr.WarningsRequeridos && String.IsNullOrWhiteSpace(cr.CR_10))
            {
                result = CommonValidator.GetValidacionEntry("429-M", cr.Validaciones, "Frecuencia de Pagos: " + cr.CR_10, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Importe de Pago | Complementario | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta11(ValidationResults results)
        {
            ValidacionEntry result;

            // 430-M (Warning) El dato Importe de Pago no puede estar vacio en un credito Activo.
            if (cr.WarningsRequeridos && String.IsNullOrWhiteSpace(cr.CR_11))
            {
                result = CommonValidator.GetValidacionEntry("430-M", cr.Validaciones, "Importe de Pago: " + cr.CR_11, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Fecha de Ultimo Pago | Opcional | Fecha
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta12(ValidationResults results)
        {

            ValidacionEntry result;
            bool todoCorrecto = true;

            if (!CommonValidator.EmptyDate(cr.CR_12))
            {
                // 431-M El Formato de la Fecha de Ultimo Pago es invalida. Debe usar DDMMAAAA
                if (!CommonValidator.Match(cr.CR_12, CommonValidator.REGX_FECHA_DDMMAAAA, true))
                {
                    result = CommonValidator.GetValidacionEntry("431-M", cr.Validaciones, cr.CR_12, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                }
                // 432-M El crédito tiene una Fecha de Ultimo Pago > a 15 días del periodo reportado.
                else if (CommonValidator.ComparaFecha(cr.CR_12, cr.PeriodoHD05, 15) == true)
                {
                    result = CommonValidator.GetValidacionEntry("432-M", cr.Validaciones, cr.CR_12, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                }
            }

            // 433-M (Warning) La Fecha de Ultimo Pago esta vacia en un credito activo.
            if (cr.WarningsRequeridos && CommonValidator.EmptyDate(cr.CR_12))
            {
                if (cr.DiasVencido <= 0)
                {
                    result = CommonValidator.GetValidacionEntry("433-M", cr.Validaciones, cr.CR_12, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                }
            }

            if (todoCorrecto)
            {
                cr.Correctos++;
            }
        }

        /// <summary>
        /// Fecha de Reestructura | Opcional | Fecha
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta13(ValidationResults results)
        {

            ValidacionEntry result;
            bool todoCorrecto = true;

            if (!CommonValidator.EmptyDate(cr.CR_13))
            {
                // 434-M El Formato de la Fecha de Reestructura es invalido. Debe usar DDMMAAAA
                if (!CommonValidator.Match(cr.CR_13, CommonValidator.REGX_FECHA_DDMMAAAA, false))
                {
                    result = CommonValidator.GetValidacionEntry("434-M", cr.Validaciones, cr.CR_13, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                }
                // 435-M El crédito tiene una Fecha de Reestructura > a 15 días del periodo reportado.
                else if (CommonValidator.ComparaFecha(cr.CR_13, cr.PeriodoHD05, 15) == true)
                {
                    result = CommonValidator.GetValidacionEntry("435-M", cr.Validaciones, cr.CR_13, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                }
            }

            if (todoCorrecto)
            {
                cr.Correctos++;
            }
        }

        /// <summary>
        /// Pago Final para Cierre de Cuenta Moroda (Pago Efectivo) | Opcional | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta14(ValidationResults results)
        {
            // Sin validaciones que realizar.
        }

        /// <summary>
        /// Fecha Liquidacion | Complementario  | Fecha
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta15(ValidationResults results)
        {

            ValidacionEntry result;
            bool todoCorrecto = true;

            if (!CommonValidator.EmptyDate(cr.CR_15))
            {
                // 436-M El Formato de la Fecha de Liquidación es invalido. Debe usar DDMMAAAA
                if (!CommonValidator.Match(cr.CR_15, CommonValidator.REGX_FECHA_DDMMAAAA, false))
                {
                    result = CommonValidator.GetValidacionEntry("436-M", cr.Validaciones, cr.CR_15, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                }
                // 437-M El crédito tiene una Fecha de Liquidacion > a 15 días del periodo reportado.
                else if (CommonValidator.ComparaFecha(cr.CR_15, cr.PeriodoHD05, 15) == true)
                {
                    result = CommonValidator.GetValidacionEntry("437-M", cr.Validaciones, cr.CR_15, cr, cr.MainRoot.EM_00, cr.CR_02);
                    results.AddResult(new ValidationResult("", result, "", "", null));
                    todoCorrecto = false;
                }
            }

            if (todoCorrecto)
            {
                cr.Correctos++;
            }
        }

        /// <summary>
        /// Quita | Complementario | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta16(ValidationResults results)
        {
            // Sin validaciones que realizar.    
        }

        /// <summary>
        /// Dacion de Pago | Complementario | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta17(ValidationResults results)
        {
            // Sin validaciones que realizar.
        }

        /// <summary>
        /// Quebranto o Castigo | Opcional | Texto
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta18(ValidationResults results)
        {
            // Sin validaciones que realizar.
        }

        /// <summary>
        /// Clave de Observacion | Opcional | Texto
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta19(ValidationResults results)
        {
            bool error = false;
            string infoError = "";

            //double Cantidad = cr.MontoPagar; 
            // Se agrega el Monto Vencido al Monto Vigente (Monto a Pagar) debido a que no todos los creditos con Clave de
            // Observacion se encuntran al corriente.
            double Cantidad = cr.MontoPagar + cr.MontoPagarVencido;   
            double ImportePago = Parser.ToDouble(cr.CR_11);
            double SaldoVencido = cr.MontoPagarVencido; 

            DateTime FechaApertura = Parser.ToDateTime(cr.CR_04);
            DateTime FechaRestrcutura = Parser.ToDateTime(cr.CR_13);
            DateTime FechaLiquidacion = Parser.ToDateTime(cr.CR_15);

            //bool conFechaApertura = (cr.Fe != null && cr.Fecha_Cierre != default(DateTime));
            bool conFechaReestructura = (cr.Fecha_Reestructura != null && cr.Fecha_Reestructura != default(DateTime));
            bool conFechaLiquidacion = (cr.Fecha_Cierre != null && cr.Fecha_Cierre != default(DateTime));

            int DiasVencido = cr.DiasVencido;

            double Quita = Parser.ToDouble(cr.CR_16);
            double Dacion = Parser.ToDouble(cr.CR_17);
            double Quebranto = Parser.ToDouble(cr.CR_18);

            string TipoCredito = cr.CR_06;
            string TipoInstitucion = cr.MainRoot.TypedParent.HD.HD_02;

            switch (this.cr.CR_19)
            {
                case "AD": // AD - Cuenta o monto en aclaración directamente con el Usuario.
                    // Cantidad = Saldo actual en el momento de la aclaración
                    // Días de Vencimiento = Días de atraso en el pago al momento de la aclaración
                    // Fecha de liquidación = Con o Sin Fecha de liquidación
                    error = ( Cantidad <= 0 || DiasVencido < 0 );
                    break;

                case "CA": // CA - Cuenta al corriente vendida o cedida a un usuario de una Sociedad de Información Creditica.
                    // Para el Usuario que vende la Cuenta de Credito    
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha en que se celebró la cesión o venta
                    if (conFechaLiquidacion)
                    {
                        // Si trae Fecha de Liquidacion aplicamos las validaciones de la Institucion que Vende la Cuenta
                        error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || conFechaLiquidacion);
                    }
                    
                    // Para la Primera Vez del Usuario que compra la Cuenta de Credito
                    // Fecha de Apertura = La fecha de apertura del Usuario original que otorgó la Cuenta o Crédito
                    // Cantidad = Saldo actual de la Cuenta o Crédito
                    // Días de Vencido = 0  
                    // Importe de Pago = Cantidad que el nuevo Usuario solicite
                    // Fecha de liquidación = Sin fecha
                    if (!conFechaLiquidacion)
                    {
                        // Si no trae Fecha de Liquidacion aplicamos las validaciones de la Institucion que Compra la Cuenta
                        error = (Cantidad == 0 || DiasVencido != 0 || ImportePago == 0 || !conFechaLiquidacion);
                    }
                    break;

                case "CC": // CC - Cuenta cancelada o cerrada.
                    // Cantidad = 0
                    // Saldo vencido = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Con fecha de cierre ó cancelación del crédito
                    error = (Cantidad != 0 || SaldoVencido != 0 || DiasVencido != 0 || !conFechaLiquidacion);
                    break;

                case "CL": // CL - Cuenta en cobranza pagada totalmente, sin causar quebranto.
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Con fecha de cierre ó cancelación del crédito
                    error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion);
                    break;

                case "CO": // CO - Crédito en controversia.
                    // Fecha de Apertura = La fecha de apertura del Usuario original que otorgó la Cuenta o Crédito
                    // Cantidad = Saldo actual considerado
                    // Días de Vencido = Los días que tenga de atraso
                    // Importe de Pago = Monto de acuerdo al Usuario que reporta
                    // Fecha de liquidación = Con fecha de cierre ó cancelación del crédito
                    error = ( Cantidad <= 0 || DiasVencido < 0 || ImportePago<=0 || !conFechaLiquidacion);
                    break;

                case "CV": // CV - Cuenta que no esta al corriente vendida o cedida a un usuario de Buró de Crédito.
                    // Para el Usuario que vende la Cuenta de Credito 
                    // Cantidad = 0
                    // Días de Vencido = Días de atraso en el pago
                    // Importe de Pago = 0
                    // Quebranto > = 0
                    // Fecha de liquidación = Fecha en que se celebró la cesión o venta
                    if (conFechaLiquidacion)
                    {
                        // Si trae Fecha de Liquidacion aplicamos las validaciones de la Institucion que Vende la Cuenta
                        error = (Cantidad != 0 || DiasVencido == 0 || ImportePago != 0 || Quebranto < 0 || !conFechaLiquidacion);
                    }

                    // Para la Primera Vez del Usuario que compra la Cuenta de Credito
                    // Fecha de Apertura = La fecha de apertura del Usuario original que otorgó la Cuenta o Crédito
                    // Cantidad = Saldo actual de la Cuenta o Crédito
                    // Días de Vencido = Los que le corresponda
                    // Importe de Pago = Monto requerido por el Usuario
                    // Fecha de liquidación = Sin fecha
                    if (!conFechaLiquidacion)
                    {
                        // Si no trae Fecha de Liquidacion aplicamos las validaciones de la Institucion que Compra la Cuenta
                        error = (Cantidad == 0 || DiasVencido <= 0 || ImportePago == 0 || conFechaLiquidacion);
                    }
                    break;

                case "FD": // FD - Cuenta con fraude atribuible al cliente.
                    // A) Existe proceso de cobranza
                    // Cantidad = Saldo de la cuenta
                    // Días de Vencido = Días de atraso en el pago
                    // Importe de Pago = Monto requerido por el Usuario
                    // Fecha de liquidación = Sin Fecha de liquidación
                    if (Quebranto == 0)
                    {
                        error = (Cantidad <= 0 || DiasVencido <= 0 || ImportePago <= 0 || conFechaLiquidacion);
                    }

                    // B) Se envía a Quebranto:
                    // Cantidad = 0
                    // Días de Vencido = Días de atraso en el pago
                    // Importe de Pago = 0
                    // Quebranto > 0    
                    // Fecha de liquidación = Con fecha de liquidación
                    if (Quebranto > 0)
                    {
                        error = ( Cantidad != 0 || DiasVencido <= 0 || ImportePago != 0 || Quebranto <= 0 || !conFechaLiquidacion);
                    }
                    break;

                case "FN": // FN - Fraude NO atribuible al cliente.
                    // La cuenta se deberá reportar por última vez como sigue:
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha en que se comprobó el fraude
                    if (conFechaLiquidacion)
                    {
                        // Si trae Fecha de Liquidacion aplicamos las validaciones de la Cuenta que registro el Fraude
                        error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion);
                    }

                    // Se deberá abrir otra cuenta la cual contendrá lo siguiente:
                    // Cantidad = Saldo actual real de la cuenta anterior del Cliente
                    // Días de Vencido = Días de atraso en el pago de acuerdo a la cuenta anterior.
                    // Importe de Pago = Monto requerido por el Usuario
                    // Fecha de liquidación = Sin Fecha de liquidación
                    if (!conFechaLiquidacion)
                    {
                        // Si no trae Fecha de Liquidacion aplicamos las validaciones de la Cuenta nueva que de debe crear
                        error = (Cantidad <= 0 || DiasVencido <= 0 || ImportePago <= 0 || conFechaLiquidacion);
                    }
                    break;

                case "FP": // FP - Fianza pagada.
                    // Tipo de institución = Fianzas (010)
                    // Tipo de crédito = Fianzas (6291)
                    // Días de vencimiento = 0
                    // Cantidad = 0
                    // Fecha de liquidación = Con fecha de liquidación
                    error = (TipoInstitucion != "10" || TipoCredito != "6291" || DiasVencido != 0 || Cantidad != 0 || !conFechaLiquidacion);
                    break;

                case "FR": // FR - Adjudicaón y/o aplicación de garantía
                    // Si la deuda fue cubierta totalmente con la garantía:
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha de adjudicación
                    if (Cantidad == 0)
                    {
                        // Si la Cantidad es igual a cero la deuda fue cubierta totalmente con la garantía
                        error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion);
                    }

                    // Si la deuda NO fue cubierta totalmente con la garantía:
                    // Cantidad = Monto que no se cubrió con la adjudicación o garantía.
                    // Días de Vencido = Días de atraso en el pago
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha de adjudicación
                    if ( Cantidad !=0)
                    {
                        // Si la Cantidad es diferente a cero la deuda NO fue cubierta totalmente con la garantía
                        error = (Cantidad <= 0 || DiasVencido <= 0 || ImportePago != 0 || !conFechaLiquidacion);
                    }
                    break;

                case "GP": // GP - Ejecución de garantía en pago por crédito prendario.
                    // Cantidad: = 0
                    // Días de Vencido: = 0
                    // Importe de Pago: = 0
                    // Con fecha de Liquidación.
                    error = (Cantidad != 0 || DiasVencido != 0 ||ImportePago != 0 || !conFechaLiquidacion);
                    break;

                case "IA": // IA - Cuenta inactiva.
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Sin Fecha de liquidación
                    error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || conFechaLiquidacion);
                    break;

                case "IM": // IM - Integrante causante de mora.
                    // Cantidad = Monto de la deuda
                    // Días de Vencido = Días de atraso en el pago
                    // Importe de Pago = Monto requerido por el Usuario
                    // Fecha de liquidación = Con o sin Fecha de liquidación
                    error = ( Cantidad <= 0  || DiasVencido <= 0 || ImportePago <= 0 );
                    break;

                case "IS": // IS - Integrante que fue subsidiado para evitar mora.
                    // Cantidad: =>0
                    // Días de Vencido: = 0
                    // Importe de Pago: =>0
                    error = (Cantidad < 0 || DiasVencido != 0 || ImportePago < 0 );
                    break;

                case "LC": // LC - Convenio de finiquito con pago menor a la deuda, acordado con el cliente (Quita).
                    // Cantidad = 0
                    // Importe de Pago = 0
                    // Días de Vencido > 0
                    // Quita > 0 Monto de la Quita que se acordó con el Cliente
                    // Fecha de liquidación = Fecha del acuerdo o convenio de finiquito.
                    error = (Cantidad != 0 || DiasVencido <= 0 || Quita <= 0 || !conFechaLiquidacion );
                    break;

                case "LG": // LG - Pago menor de la dueda por programa institucional o de gobierno, incluyeno los apoyos a damnificados por catástrofes naturales (Quita).

                    // Si el adeudo se finiquita totalmente, reportar como sigue:
                    // Cantidad = 0
                    // Importe de Pago = 0
                    // Días de Vencido = 0
                    // Quita > 0 Monto de la Quita que se acordó con el Cliente
                    // Fecha de liquidación = Fecha de acuerdo a convenio
                    if (Cantidad == 0)
                    {
                        // Si la cantidad es igual a cero aplicamos las validaciones del adeudo finiquitado totalmente
                        error = ( Cantidad !=0 || ImportePago != 0 || DiasVencido != 0 || Quita <= 0 || !conFechaLiquidacion );
                    }                   

                    // Si queda saldo remanente después del apoyo que el Cliente seguirá pagando, reportar como sigue:
                    // Cantidad > 0
                    // Importe de Pago = 0
                    // Días de Vencido = 0
                    // Quita > 0 Monto de la Quita que se acordó con el Cliente
                    // Fecha de liquidación = Sin fecha de cierre
                    if (Cantidad > 0)
                    {
                        // Si la Cantidad es mayor a cero aplicamos las validaciones del saldo remanente
                        error  = ( Cantidad <= 0 || ImportePago != 0 || DiasVencido != 0  || Quita <= 0 || !conFechaLiquidacion );
                    }

                    break;

                case "LO": // LO - En localización.
                    // Cantidad = Monto de la deuda
                    // Días de Vencido = Pagos no realizados
                    // Importe de Pago = Monto requerido por el Usuario
                    // Fecha de liquidación = Con o sin Fecha de liquidación
                    error = ( Cantidad <= 0 || DiasVencido <= 0 || ImportePago <= 0 );
                    break;

                case "LS": // LS - Tarjeta de crédito robada ó extraviada.
                    // Reportar el número de tarjeta robada o extraviada como:
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha de reporte de robo o extravío
                    error = ( Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion );

                    // Reportar el nuevo número de tarjeta como sigue:
                    // Cantidad = Saldo actual
                    // Días de Vencido = Días de atraso en el pago
                    // Importe de Pago = Monto que solicite el Usuario
                    // Fecha de liquidación = Sin Fecha de liquidación
                    error = ( Cantidad <= 0 || DiasVencido <= 0 || ImportePago <= 0 || !conFechaLiquidacion );
                    break;

                case "NA": // NA - Cuenta al corriente vendida o cedida a un NO usuario del búro de crédito.
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha en que se celebró la cesión o venta
                    error = ( Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion );
                    break;

                case "NV": // NV - Cuenta vencida o vendida a un NO usuario del búro de crédito de la Sociedad de Información Crediticia.
                    // Cantidad = 0
                    // Días de Vencido = Días de atraso en el pago
                    // Importe de Pago = 0
                    // Quebranto = Lo que no recuperó el Usuario
                    // Fecha de liquidación = Fecha en que se celebró la cesión o venta
                    error = ( Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || Quebranto <= 0 || !conFechaLiquidacion );
                    break;

                case "PC": // PC - Cuenta en cobranza.
                    // Cantidad = Monto del adeudo
                    // Días de Vencido = Días de atraso en el pago
                    // Importe de Pago = Monto requerido por el Usuario
                    // Fecha de liquidación = Sin fecha
                    error = ( Cantidad <= 0 || DiasVencido <= 0 || ImportePago <= 0 || conFechaLiquidacion );
                    break;

                case "RA": // RA - Cuenta reestructurada sin pago menor, por programa institucional o gubernamental, incluyendo los apoyos a damnificados por catástrofes naturales.
                    // Reportar en la cuenta original:
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha en que se celebró la reestructura
                    // Fecha de Reestructura = Fecha en que se celebró la reestructura
                    // Clave de Observación = RA
                    if (conFechaLiquidacion && conFechaReestructura)
                    {
                        // Si tiene Fecha de Liquidacion y Fecha de restructura aplicamos validaciones de la Cuenta Original
                        error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion || !conFechaReestructura);
                    }

                    // La nueva cuenta se reportará por primera vez de la siguiente forma:
                    // Fecha de apertura = Fecha de apertura del crédito original
                    // Cantidad = Saldo actual de la cuenta anterior
                    // Días de Vencido = 0
                    // Importe de Pago = Cantidad que solicite el Usuario
                    // Fecha de liquidación = Sin Fecha de liquidación
                    if (!conFechaLiquidacion)
                    {
                        // Si no tiene fecha de Liquidacion aplicamos las validaciones de la Nueva Cuenta
                        error = (Cantidad <= 0 || DiasVencido != 0 || ImportePago <= 0 || conFechaLiquidacion);
                    }
                    break;

                case "RI": // RI - Robo de identidad.
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha en que se comprobó la identidad falsa
                    error = ( Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion );
                    break;

                case "RF": // RF - Resolucion judicial favorable al cliente.
                    // Cantidad = 0 ó saldo pendiente de pago, sin atraso, dependiendo de los términos acordados
                    // Días de Vencido = Número de días de atraso en su pago acordado con el Cliente
                    // Importe de Pago = 0 ó Monto requerido por el Usuario
                    // Quita = 0 ó Monto que le haya otorgado el Usuario
                    // Dación = 0 ó Monto del valor del bien en “Dación”
                    // Fecha de liquidación = Con o sin fecha dependiendo de los términos acordados con el Cliente
                    error = (Cantidad != 0 || DiasVencido <= 0 || Quita != 0 || Dacion != 0);
                    break;

                case "RN": // RN - Cuenta reestructurada debido a un proceso judicial.
                    // Reportar en la cuenta original:
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha en que se celebró la reestructura
                    // Fecha de Reestructura = Fecha en que se celebró la reestructura
                    // Clave de Observación = RN
                    if (conFechaLiquidacion)
                    {
                        // Si tiene Fecha de Liquidacion aplicamos las validaciones de Cuenta Original
                        error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion || !conFechaReestructura);
                    }                    

                    // La nueva cuenta se reportará por primera vez de la siguiente forma:
                    // Fecha de apertura = Fecha de apertura del crédito original
                    // Cantidad = Saldo actual de la cuenta anterior
                    // Días de Vencido = 0
                    // Importe de Pago = Cantidad que solicite el Usuario
                    // Fecha de liquidación = Sin Fecha de liquidación
                    if (!conFechaLiquidacion)
                    {
                        // Si NO tiene Fecha de Liquidacion aplicamos validaciones de Nueva Cuenta
                        error = (Cantidad <= 0 || DiasVencido != 0 || ImportePago <= 0 || conFechaLiquidacion);
                    }
                    break;

                case "RV": // RV - Cuenta reestructurada sin pago menor por modificación de la situación del cliente, a petición de éste.                 
                    // Reportar en la cuenta original:
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Fecha de liquidación = Fecha en que se celebró la reestructura
                    // Fecha de Reestructura = Fecha en que se celebró la reestructura
                    // Clave de Observación = RV
                    if (conFechaLiquidacion)
                    {
                        // Si tiene Fecha de Liquidacion aplicamos las validaciones de la Cuenta Original
                        error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || !conFechaLiquidacion || !conFechaReestructura);
                    }
                    
                    // La nueva cuenta se reportará por primera vez de la siguiente forma:
                    // Fecha de apertura = Fecha de apertura del crédito original
                    // Cantidad = Saldo actual de la cuenta anterior
                    // Días de Vencido = 0
                    // Importe de Pago = Cantidad que solicite el Usuario
                    // Fecha de liquidación = Sin Fecha de liquidación
                    if (!conFechaLiquidacion)
                    {
                        // Si no tiene Fecha de Liquidacion aplicamos las validaciones de la Cuenta Nueva
                        error = (Cantidad <= 0 || DiasVencido != 0 || ImportePago <= 0 || conFechaLiquidacion);
                    }                   
                    break;

                case "SG": // SG - Demanda por el usuario.
                    // Cantidad = Saldo al momento de la demanda
                    // Días de Vencido = Días de atraso en el pago al momento de la demanda
                    // Importe de Pago = Cantidad que solicite el Usuario
                    // Fecha de liquidación = Sin Fecha de liquidación
                    error = ( Cantidad <= 0 || DiasVencido <= 0 || ImportePago <= 0 || conFechaLiquidacion);
                    infoError = " Cantidad: " + Cantidad + " Dias: " + DiasVencido + " Importe: " + ImportePago + " Fecha: " + cr.Fecha_Cierre + " Con Fecha: " + conFechaLiquidacion;
                    break;

                case "UP": // UP - Cuenta que causa Quebranto o Castigo.
                    // A) Cantidad = Monto del Saldo sin recuperar
                    // Días de Vencido = Días de atraso en el pago.
                    // Importe de Pago = Cantidad que solicite el Usuario
                    // Fecha de liquidación = Sin fecha de cierre si el Usuario continúa tratando de recuperar
                    if (conFechaLiquidacion)
                    {
                        error = (Cantidad <= 0 || DiasVencido <= 0 || ImportePago <= 0 || conFechaLiquidacion);
                    }

                    // B) Cantidad = 0
                    // Días de Vencido = Días de atraso en el pago.
                    // Importe de Pago = 0
                    // Quebranto = Importe que dejó de pagar el Cliente
                    // Fecha de liquidación = Con fecha si el Usuario ya no desea seguir tratando de recuperar
                    if (!conFechaLiquidacion)
                    {
                        error = (Cantidad != 0 || DiasVencido <= 0 || ImportePago != 0  || Quebranto <= 0 || !conFechaLiquidacion);
                    }
                    break;

                case "VR": // VR - Dación en pago o renta.
                    // Si la deuda fue cubierta totalmente:
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Dación = Monto del valor del bien dado en "Dación"
                    // Fecha de liquidación = Fecha de la dación
                    if (Quebranto == 0)
                    {
                        error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || Dacion <= 0 || !conFechaLiquidacion);
                    }

                    // Si la deuda NO fue cubierta totalmente:
                    // Cantidad = 0
                    // Días de Vencido = 0
                    // Importe de Pago = 0
                    // Dación = Monto del valor del bien dado en "Dación"
                    // Quebranto = Monto que no se cubrió con el valor del bien dado en "Dación"
                    // Fecha de liquidación = Fecha de la dación
                    if (Quebranto > 0)
                    {
                        error = (Cantidad != 0 || DiasVencido != 0 || ImportePago != 0 || Dacion <= 0 || Quebranto <= 0 || !conFechaLiquidacion);
                    }
                    break;
            }

            if (error)
            {
                ValidacionEntry result = CommonValidator.GetValidacionEntry(string.Format("{0}-M", cr.CR_19), cr.Validaciones, infoError, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
            }

        }

        /// <summary>
        /// Marca para Credito Especial | Opcional | Texto
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta20(ValidationResults results)
        {
            ValidacionEntry result;

            // 438-M El dato Marca para Crédito Especial contiene un valor invalido.
            if (!CommonValidator.Match(cr.CR_20, "F?", true))
            {
                result = CommonValidator.GetValidacionEntry("438-M", cr.Validaciones, cr.CR_20, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
            }
        }

        /// <summary>
        /// Fecha de Primer Incumplimiento | Requerido | Fecha
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta21(ValidationResults results)
        {
            ValidacionEntry result;

            // 439-M El dato Fecha de Primer Incumplimiento es Requerido.
            if (String.IsNullOrWhiteSpace(cr.CR_21))
            {
                result = CommonValidator.GetValidacionEntry("439-M", cr.Validaciones, cr.CR_21, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 440-M El dato Fecha de Primer Incumplimiento no puede estar vacio debido a que existen Días de Vencido.
            if (cr.DiasVencido > 0 && cr.CR_21.Contains("000000"))
            {
                result = CommonValidator.GetValidacionEntry("440-M", cr.Validaciones, "Fecha Incumplimiento: " + cr.CR_21 + " Dias Vencido: " + cr.DiasVencido, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }
        }

        /// <summary>
        /// Saldo Insoluto del Principal | Requerido | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta22(ValidationResults results)
        {
            ValidacionEntry result;

            // 441-M El dato Saldo Insoluto del Principal es Requerido.
            if (String.IsNullOrWhiteSpace(cr.CR_22))
            {
                result = CommonValidator.GetValidacionEntry("441-M", cr.Validaciones, cr.CR_22, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            // 442-M El Saldo Insoluto del Principal deberá ser menor o igual a "Cantidad" y diferente a 0.
            // Excepción
            // Para los tipos de contrato será aceptado que el Saldo Insoluto del Principal y la "Cantidad" sean igual a 0:
            //  - Tarjeta de Crédito Empresarial (1380)
            //  - Tarjeta de Servicio (6250)
            //  - Línea de Crédito (6280) 
            //  - Créditos en cuenta corriente ( 1305)
            if (!CommonValidator.ValidTipoCreditoEspecial("CR", "22", cr.CR_06))
            {
                
                // Campo Cantidad = Deuda Original + Intereses
                // Investigar de donde optendremos el campo de Cantidad
                // Esta validacion no existia en le Varsion 3


                // DESHABILITADO hasta definir correctamente el campo Cantidad (NO es CR_07)

                //double SaldoInsoluto = Parser.ToDouble(cr.CR_22);
                //double Cantidad = Parser.ToDouble(cr.CR_07);

                //if (SaldoInsoluto == 0 || SaldoInsoluto >= Cantidad)
                //{
                //    result = CommonValidator.GetValidacionEntry("442-M", cr.Validaciones, "Saldo Insoluto: " + cr.CR_22 + " Cantidad: " + cr.CR_07, cr, cr.MainRoot.EM_00, cr.CR_02);
                //    results.AddResult(new ValidationResult("", result, "", "", null));
                //    return;
                //}
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Credito Maximo Utilizado | Requerido | Numerico
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta23(ValidationResults results)
        {
            ValidacionEntry result;

            // 443-M El dato Credito Maximo Utilizado es Requerido.
            if (String.IsNullOrWhiteSpace(cr.CR_23))
            {
                result = CommonValidator.GetValidacionEntry("443-M", cr.Validaciones, cr.CR_23, cr, cr.MainRoot.EM_00, cr.CR_02);
                results.AddResult(new ValidationResult("", result, "", "", null));
                return;
            }

            cr.Correctos++;
        }

        /// <summary>
        /// Filler | Requerido | Texto
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckEtiqueta24(ValidationResults results)
        {
            // Sin validaciones que realizar.
        }

    }
}
