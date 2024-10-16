using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Banobras.Credito.SICREB.Common.Types;
using Banobras.Credito.SICREB.Business.Validators.Common;
using Banobras.Credito.SICREB.Common.Data.Catalogos;
using Banobras.Credito.SICREB.Common.Entidades;
using Banobras.Credito.SICREB.Business.Repositorios.Catalogos;

namespace Banobras.Credito.SICREB.Business.Validators.PF
{
    public static class SegmentoINTF
    {
        /// <summary>
        /// version del archivo obligatorio 10
        /// </summary>
        /// <param name="toValidate"></param>
        public static void V_05(string toValidate)
        {
            if(toValidate==null)
                throw new PF_INTF_Exception(String.Format("La version del archivo es obligatoria"));
            if (toValidate != "10")
                throw new PF_INTF_Exception(String.Format("Version del archivo incorrecta:{0} Correcta:{1} ", toValidate, "10"));
        }

        public static void V_07(string toValidate)
        {
            if(toValidate==null)
                throw new PF_INTF_Exception(string.Format("La clave de usuario es obligatoria", toValidate));
            if (toValidate.Trim().Length == 0)
                throw new PF_INTF_Exception(string.Format("La clave de usuario es obligatoria", toValidate));
            if (toValidate.Length > 10)
                throw new PF_INTF_Exception(string.Format("La clave de usuario mas grande lo permitido {0}", toValidate));
        }

        public static void V_17(string toValidate)
        {
            if (toValidate.Length == 16)
                throw new PF_INTF_Exception(string.Format("Clave de usuario mas grande lo permitido {0}", toValidate));
        }
            /*
             *         
        public string INTF_17 { get; set; }

        public string INTF_33 { get; set; }
        
        public string INTF_35 { get; set; }

        public string INTF_43 { get; set; }

       
        public string INTF_53 { get; set; }*/
    }
}
