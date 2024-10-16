using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Business.Rules.PM;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Business.Rules.PF;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules
{
    public class MainRules
    {
        private PM_Cinta_Rules pmCintaRules = null;
        private PF_Cinta_Rules pfCintaRules = null;        
        private NotificacionesRules notifRules = null;
        private ErrorAdvertenciaDataAccess errores = null;

        public MainRules()
        {
            pmCintaRules = new PM_Cinta_Rules();
            pfCintaRules = new PF_Cinta_Rules();
            notifRules = new NotificacionesRules();
            errores = new ErrorAdvertenciaDataAccess();
        }

        public void GeneraArchivos(Enums.Reporte reporte, string grupos)
        {
            errores.ClearErrorAdv();
            errores.ValidCuentasIFRS9();

            PM_Cinta cintaPM = pmCintaRules.GeneraArchivo(reporte, grupos);
            notifRules.EnviaNotificaciones(Enums.Persona.Moral);
            //ConciliacionRules conciliacionRules = new ConciliacionRules(Enums.Persona.Moral);
            //conciliacionRules.Concilia(cintaPM.ArchivoId, reporte);


            //PF_Cinta cintaPF = pfCintaRules.GeneraArchivo(reporte);
            ////notifRules.EnviaNotificaciones(Enums.Persona.Fisica);
            //conciliacionRules = new ConciliacionRules(Enums.Persona.Fisica);
            //conciliacionRules.Concilia(cintaPF, cintaPF.ArchivoId, reporte);
        }

    }
}
