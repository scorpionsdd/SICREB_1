using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Banobras.Credito.SICREB.Common.Validaciones
{
    public class Validaciones
    {
        public bool Val_Fecha( string Fecha)
        {
            try
            {
                Regex re = new Regex("^(0?[1-9]|1[0-9]|2|2[0-9]|3[0-1])(0?[1-9]|1[0-2])(d{2}|d{4})$");
                if (re.IsMatch(Fecha))                       
                           return true;                       
                    else                       
                           return false;                       
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public bool Val_RFC_PM(string RFC_PM)
        {
            Regex re = new Regex(@"^[a-zA-Z&]{3,4}(\d{6})((\D|\d){3})?$");
            if (re.IsMatch(RFC_PM))
                return true;
            else
                return false;  

        }
    }
}
