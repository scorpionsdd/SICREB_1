using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities
{
    public class Request_Estado
    {
        public enum Estado { 
            INICIO, 
            BORRAR_ERRORES, 
            GENERAR_PM, 
            NOTIF_PM, 
            CONCILIACION_PM, 
            GENERAR_PF_2011, 
            NOTIF_PF_2011, 
            GENERAR_PF, 
            NOTIF_PF, 
            CONCILIACION_PF, 
            COMPLETO,
            ERROR,
            GENERA_GPOS_LCCYR};

        public Request_Estado.Estado Status { get; private set; }
        public String Mensaje { get; private set; }

        public Request_Estado(string status, string mensaje)
        {
            this.Status = EstadoFromString(status);
            this.Mensaje = mensaje;
        }

        public static Request_Estado.Estado EstadoFromString(string estado)
        {
            Request_Estado.Estado toRetun;
            switch (estado.ToUpper())
            {
                case "INICIO":
                    toRetun = Request_Estado.Estado.INICIO; break;
                case "BORRAR_ERRORES":
                    toRetun = Request_Estado.Estado.BORRAR_ERRORES; break;
                case "GENERAR_PM":
                    toRetun = Request_Estado.Estado.GENERAR_PM; break;
                case "NOTIF_PM":
                    toRetun = Request_Estado.Estado.NOTIF_PM; break;
                case "CONCILIACION_PM":
                    toRetun = Request_Estado.Estado.CONCILIACION_PM; break;
                case "GENERAR_PF":
                    toRetun = Request_Estado.Estado.GENERAR_PM; break;
                case "NOTIF_PF":
                    toRetun = Request_Estado.Estado.NOTIF_PM; break;
                case "CONCILIACION_PF":
                    toRetun = Request_Estado.Estado.CONCILIACION_PM; break;
                case "COMPLETO":
                    toRetun = Request_Estado.Estado.COMPLETO; break;
                case "ERROR":
                    toRetun = Request_Estado.Estado.ERROR; break;
                case "GENERA_GPOS_LCCYR":
                    toRetun = Request_Estado.Estado.GENERA_GPOS_LCCYR; break;
                default:
                    toRetun = Request_Estado.Estado.INICIO; break;

            }
            return toRetun;
        }

    }
}
