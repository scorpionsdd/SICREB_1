using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Banobras.Credito.SICREB.Common.Data;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Banobras.Credito.SICREB.Business.Validators.Common;
using Banobras.Credito.SICREB.Common.Types;
using Banobras.Credito.SICREB.Common.Types.PF;
namespace Banobras.Credito.SICREB.Business.Validators.PF
{
     [ConfigurationElementType(typeof(CustomValidatorData))]
    public class SegmentoINTFValidator:Validator<PF_INTF>
    {
         public SegmentoINTFValidator(string messageTemplate, string tag)
             : base(String.Empty, String.Empty)
         {
             base.MessageTemplate = messageTemplate;
                 base.Tag=tag;
         }

         protected override string DefaultMessageTemplate
         {
             get { return base.MessageTemplate; }
         }


         protected override void DoValidate(PF_INTF objectToValidate, object currentTarget, string key, ValidationResults validationResults)
         {
             string result = DefaultMessageTemplate;
             if (objectToValidate != null)
             {
                 
             }
         }
    }
}
