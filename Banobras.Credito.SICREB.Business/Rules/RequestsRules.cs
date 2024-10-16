using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Entities;
using System.ServiceProcess;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Business.Rules
{
    public class RequestsRules
    {
        private RequestsDataAccess requestData;

        public RequestsRules()
        {
            requestData = new RequestsDataAccess();
        }

        public void IniciaProceso(Enums.Reporte reporte, bool notificaciones, string grupos)
        {
            requestData.AddRequest(reporte, notificaciones, grupos);

            StartService("SICREBService", 10000);
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);               
            }
            catch(Exception ex)
            {
                throw new Exception("Error al inicializar proceso", ex);
            }
        }

        public void ActualizaRequest(Request request)
        {
            requestData.ActualizaRequest(request);
        }

        public Request GetRequestPendiente()
        {
            return requestData.GetRequestPendiente();
        }

        public List<Request_Estado> GetRequestsEstado()
        {
            return requestData.GetRequestsEstado();
        }


    }
    
}
