using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Business.Rules;

namespace Banobras.Credito.SICREB.Service
{
    public partial class SICREBService : ServiceBase
    {
        private List<Request_Estado> estados;
        private RequestsRules reqRules = null;
        private System.Timers.Timer timer;

        public SICREBService()
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

        public void depurando_servicio()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {            
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();            
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Request request = reqRules.GetRequestPendiente();
            if (request.Status == Request_Estado.Estado.INICIO && request.Id > 0)
            {
                Proceso proc = new Proceso(request);
                proc.RunWorkerCompleted += new RunWorkerCompletedEventHandler(proc_RunWorkerCompleted);
                proc.ProgressChanged += new ProgressChangedEventHandler(proc_ProgressChanged);

                proc.RunWorkerAsync();
                timer.Stop();
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

        public void EscribirLog(string Mensaje)
        {
            EventLog.WriteEntry(string.Format("Hora del mensaje:{0} \n Mensaje: {1} \n  ", DateTime.Now, Mensaje), EventLogEntryType.Information);
        }

        public void EscribirLog(Exception Ex)
        {
            EventLog.WriteEntry(string.Format("Hora de la Excepcion:{0} \n Mensaje: {1} \n Traza Completa: {2} \n ", DateTime.Now, Ex.InnerException.Message, Ex.InnerException.StackTrace), EventLogEntryType.Error);
        }
        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
        }
                
    }
}
