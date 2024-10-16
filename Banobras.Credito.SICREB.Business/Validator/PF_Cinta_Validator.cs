using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Banobras.Credito.SICREB.Common.Types
{
    [HasSelfValidation]
    public class  PF_Cinta_Validator
    {
        //[SelfValidation]
        //public void ValidacionAvisos01(ValidationResults results)
        //{
        //    DateTime TL_13date = Convert.ToDateTime(SegmentoCuenta.TL_13);
        //    if (string.IsNullOrEmpty(SegmentoNombre.PN_15) && TL_13date > Convert.ToDateTime("1998-01-01") && Convert.ToDateTime(SegmentoCuenta.TL_17).Subtract(TL_13date).Days >= 90)
        //    results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Cuenta con fecha de Apertura mayor a 90 Días y sin RFC reportado", this, "", "", null));
        //    return;
        //}


        //[SelfValidation]
        //public void ValidacionAvisos02(ValidationResults results)
        //{
        //    DateTime TL_17date = Convert.ToDateTime(SegmentoCuenta.TL_17);
        //    DateTime TL_14date = Convert.ToDateTime(SegmentoCuenta.TL_14);
        //    DateTime TL_16date = Convert.ToDateTime(SegmentoCuenta.TL_16);
        //    DateTime TL_15date = Convert.ToDateTime(SegmentoCuenta.TL_15);
        //    DateTime FechaDefault = Convert.ToDateTime("1900-01-01");
        //    if ((TL_17date < TL_14date && TL_14date != FechaDefault) || (TL_17date < TL_15date && TL_15date != FechaDefault) || (TL_17date < TL_16date && TL_16date != FechaDefault) )
        //    {
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Fecha de Reporte Incongruente", this, "", "", null));
        //        return;
        //    }
        //}

        //[SelfValidation]
        //public void ValidacionAvisos03(ValidationResults results)
        //{
        //    if(Convert.ToInt64(SegmentoCuenta.TL_21) < Convert.ToInt64(SegmentoCuenta.TL_22))
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Saldo Actual Mayor a Máximo Crédito", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos04(ValidationResults results)
        //{
        //    if (!string.IsNullOrEmpty(SegmentoCuenta.TL_04) && string.IsNullOrEmpty(SegmentoCuenta.TL_41))
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Cambio del número de cuenta", this, "", "", null));
        //}


        //[SelfValidation]
        //public void ValidacionAvisos05(ValidationResults results)
        //{
        //    int RangoDias = Convert.ToDateTime(SegmentoCuenta.TL_17).Subtract(Convert.ToDateTime(SegmentoCuenta.TL_13)).Days;
        //    if (Convert.ToInt64(SegmentoCuenta.TL_21)<=0 && Convert.ToInt64(SegmentoCuenta.TL_22) > 0 && (RangoDias>90 || RangoDias < 0))
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Crédito Máximo Inconsistente", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos06(ValidationResults results)
        //{
        //    DateTime TL_17date =Convert.ToDateTime(SegmentoCuenta.TL_16);
        //    if (TL_17date != Convert.ToDateTime("1900-01-01") && TL_17date < Convert.ToDateTime(SegmentoCuenta.TL_15)) 
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Fecha de cierre menor a la fecha de última compra", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos07(ValidationResults results)
        //{
        //    int NumeroPagosVencidos =Convert.ToInt32(SegmentoCuenta.TL_25);
        //    if (string.IsNullOrEmpty(SegmentoCuenta.TL_11) &&  ((NumeroPagosVencidos==0 &&  (SegmentoCuenta.TL_26 != "00" || SegmentoCuenta.TL_26 != "UR" || SegmentoCuenta.TL_26 != "01" || SegmentoCuenta.TL_26 != "99" ) )||(NumeroPagosVencidos > 0 && !(SegmentoCuenta.TL_26 != "00" || SegmentoCuenta.TL_26 != "UR" || SegmentoCuenta.TL_26 != "01" || SegmentoCuenta.TL_26 != "99" ) )) )
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Frecuencia de pago faltante", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos08(ValidationResults results)
        //{
            
        //    if (Convert.ToDateTime(SegmentoCuenta.TL_16) == Convert.ToDateTime("1900-02-02"))
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Reapertura de Cuentas", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos09(ValidationResults results)
        //{

        //    if (SegmentoCuenta.TL_30=="EL")
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Eliminación de Clave de Observación", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos10(ValidationResults results)
        //{
        //    if (SegmentoCuenta.TL_11 != "Z" && (SegmentoCuenta.TL_06 == "O" || SegmentoCuenta.TL_06 =="R"))
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Inconsistencia en Frecuencia de Pago", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos11(ValidationResults results)
        //{
        //    if (SegmentoCuenta.TL_22 < 0 )
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Valor de Saldo Actual en cero", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos12(ValidationResults results)
        //{
        //    if (SegmentoCuenta.TL_24 < 0)
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Valor de Saldo Vencido en cero", this, "", "", null));
        //}

        //[SelfValidation]
        //public void ValidacionAvisos13(ValidationResults results)
        //{
        //    if (string.IsNullOrEmpty(SegmentoCuenta.TL_05))
        //        results.AddResult(new Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult("Tipo de responsabilidad no reportado", this, "", "", null));
        //}

    }
}
