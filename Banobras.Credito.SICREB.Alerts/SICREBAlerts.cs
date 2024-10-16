using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Business.Rules;

namespace Banobras.Credito.SICREB.Alerts
{
    public partial class SICREBAlerts : ServiceBase
    {
        private List<Request_Estado> estados;
        private RequestsRules reqRules = null;
        private System.Timers.Timer timer;

        public SICREBAlerts()
        {
            InitializeComponent();
            try
            {
                reqRules = new RequestsRules();
                estados = reqRules.GetRequestsEstado();
            }
            catch (Exception ex)
            {
                EscribirLog(ex);
            }
        }


        protected override void OnStart(string[] args)
        {
            
            timer = new System.Timers.Timer(3600000); // cada hora
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool bEnvia = false; 

            //obtiene hora de settings para verificar envio de alerta
            DateTime dtHora1a = Convert.ToDateTime(SetAlerts.Default.strHora1a);
            DateTime dtHora1b = Convert.ToDateTime(SetAlerts.Default.strHora1b);
            DateTime dtHora2a = Convert.ToDateTime(SetAlerts.Default.strHora2a);
            DateTime dtHora2b = Convert.ToDateTime(SetAlerts.Default.strHora2b);

            //valida si se encuentra en el rango de horas de settings
            if (DateTime.Now >= dtHora1a && DateTime.Now <= dtHora1b)
                bEnvia = true;
            else if (DateTime.Now >= dtHora2a && DateTime.Now <= dtHora2b)
                bEnvia = true;

            Request request = reqRules.GetRequestPendiente();

            //si esta en el rango de horas envia alerta
            if(bEnvia)
            {
                if (request.Status == Request_Estado.Estado.INICIO && request.Id > 0)
                {
                    ProcesoAlerts proc = new ProcesoAlerts(request);
                    proc.RunWorkerCompleted += new RunWorkerCompletedEventHandler(proc_RunWorkerCompleted);
                    proc.ProgressChanged += new ProgressChangedEventHandler(proc_ProgressChanged);
                    proc.RunWorkerAsync();
                }
            }            
        }

        void proc_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Request request = e.UserState as Request;

            if (request != null)
            {
                string mensaje = (from est in estados
                                  where est.Status == request.Status
                                  select est.Mensaje).FirstOrDefault();

                if (request.Status == Request_Estado.Estado.ERROR)
                {
                    EventLog.WriteEntry(string.Format("{0}\n\r\n\r{1}", mensaje, request.Mensaje), EventLogEntryType.Error);
                }
                else
                {
                    EventLog.WriteEntry(string.Format("{0} - {1}", request.Status, mensaje), EventLogEntryType.Information);
                }
                try
                {
                    //actualiza el request!
                    reqRules.ActualizaRequest(request);
                }
                catch (Exception ex)
                {
                    EscribirLog(ex);
                }
            }
            System.Threading.Thread.Sleep(2000);
            
        }

        void proc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer.Start();
        }

        protected override void OnStop()
        {
            
        }

        public void EscribirLog(string Mensaje)
        {
            EventLog.WriteEntry(string.Format("Hora del mensaje:{0} \n Mensaje: {1} \n  ", DateTime.Now, Mensaje), EventLogEntryType.Information);
        }

        public void EscribirLog(Exception Ex)
        {
            EventLog.WriteEntry(string.Format("Hora de la Excepcion:{0} \n Mensaje: {1} \n Traza Completa: {2} \n ", DateTime.Now, Ex.InnerException.Message, Ex.InnerException.StackTrace), EventLogEntryType.Error);
        }
        
    }
}
