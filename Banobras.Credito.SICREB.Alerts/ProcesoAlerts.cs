using System;
using System.Collections;
using System.ComponentModel;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Alerts
{
   public  class ProcesoAlerts : BackgroundWorker
    {
        private Request request;
        private string strMsgRequest = string.Empty;

        public ProcesoAlerts(Request request)
       {
           this.WorkerReportsProgress = true;          
        
           this.request = request;

       }
       protected override void OnDoWork(DoWorkEventArgs e)
       {
            base.OnDoWork(e);
            try
            {
                //llenas configuracion
                Hashtable htConfig = new Hashtable();
                htConfig.Add("strFrom", SetAlerts.Default.strFrom);
                htConfig.Add("strHost", SetAlerts.Default.strHost);
                htConfig.Add("strUser", SetAlerts.Default.strUser);
                htConfig.Add("strPass", SetAlerts.Default.strPass);
                htConfig.Add("iPort", SetAlerts.Default.iPort);
                
                this.strMsgRequest = string.Empty;
                ExecSystemAlerts exeSA = new ExecSystemAlerts(DateTime.Now, htConfig);
                string strMsg = exeSA.ejecutaSystemAlerts();
                this.strMsgRequest = strMsg;

                request.Mensaje = strMsgRequest;

            }           
            catch (Exception ex)
            {
                request.Mensaje = ex.Message;
                if (ex.InnerException != null)
                {
                    request.Mensaje += string.Format("{0} {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                request.Fecha_Final = DateTime.Now;
                ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.ERROR));
            }//try-catch
       }//OnDoWork

       public void OnDoWorkSobre()
       {
           try
           {
               this.strMsgRequest = string.Empty;
               ExecSystemAlerts exeSA = new ExecSystemAlerts(DateTime.Now);
               string strMsg = exeSA.ejecutaSystemAlerts();
               this.strMsgRequest = strMsg;

               request.Mensaje = strMsgRequest;
           }           
           catch (Exception ex)
           {
               request.Mensaje = ex.Message;
               if (ex.InnerException != null)
               {
                   request.Mensaje += string.Format("{0} {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
               }
               request.Fecha_Final = DateTime.Now;
               ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.ERROR));
           }//try-catch
       }//OnDoWork

    }
}
