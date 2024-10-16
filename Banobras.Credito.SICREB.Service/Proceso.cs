using System;
using System.ComponentModel;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Business.Rules;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Business.Rules.PM;
using Banobras.Credito.SICREB.Business.Rules.PF;

namespace Banobras.Credito.SICREB.Service
{
    public class Proceso : BackgroundWorker
    {
        private readonly Request request;
        private readonly PM_Cinta_Rules pmCintaRules = null;
        private readonly PF_Cinta_Rules pfCintaRules = null;
        private readonly PM_Cinta_Rules pmCintaRules2011 = null;
        private readonly PF_Cinta_Rules pfCintaRules2011 = null;
        private readonly NotificacionesRules notifRules = null;
        private readonly ErroresWarnings_Rules errores = null;

        public Proceso(Request request)
        {
            this.WorkerReportsProgress = true;

            pmCintaRules = new PM_Cinta_Rules();
            pfCintaRules = new PF_Cinta_Rules();

            // Modificacion para gerar cinta solo con registros del 2011
            pmCintaRules2011 = new PM_Cinta_Rules();
            pfCintaRules2011 = new PF_Cinta_Rules();

            notifRules = new NotificacionesRules();
            errores = new ErroresWarnings_Rules();

            this.request = request;
        }

        /// <summary>
        /// Proceso principal del servicio para crear los archivos PM y PF
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            base.OnDoWork(e);
            try
            {
                if (this.request != default(Request) && this.request.Status == Request_Estado.Estado.INICIO)
                {
                    ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.BORRAR_ERRORES));
                    errores.ClearErrors();

                    // Persona Moral
                    ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.GENERAR_PM));
                    PM_Cinta cintaPM = pmCintaRules.GeneraArchivo(this.request.TipoReporte, this.request.Grupos);
                    request.ArchivoPM = cintaPM.ArchivoId;

                    // Persona Física
                    ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.GENERAR_PF));
                    PF_Cinta cintaPF = pfCintaRules.GeneraArchivo(this.request.TipoReporte, this.request.Grupos);
                    request.ArchivoPF = cintaPF.ArchivoId;

                    if (this.request.MandaNotificaciones)
                    {
                        ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.NOTIF_PM));
                        notifRules.EnviaNotificaciones(Enums.Persona.Moral);

                        ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.NOTIF_PF));
                        notifRules.EnviaNotificaciones(Enums.Persona.Fisica);
                    }

                    request.Fecha_Final = DateTime.Now;
                    ReportProgress(100, request.ReportaEstado(Request_Estado.Estado.COMPLETO));
                }
            }
            catch (Exception ex)
            {
                request.Mensaje = "Error: "+ ex.Message;
                if (ex.InnerException != null)
                {
                    request.Mensaje += string.Format("{0} {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }

                request.Fecha_Final = DateTime.Now;
                ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.ERROR));
            }
        }

        /// <summary>
        /// Copia de Proceso principal, se utiliza para pruebas en Desarrollo
        /// </summary>
        public void OnDoWorkSobre()
        {
            try
            {
                if (this.request != default(Request) && this.request.Status == Request_Estado.Estado.INICIO)
                {
                    ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.BORRAR_ERRORES));
                    errores.ClearErrors();

                    this.request.TipoReporte = Enums.Reporte.Mensual;
                    this.request.Grupos = "13,";

                    // Persona Moral Desarrollo
                    ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.GENERAR_PM));
                    PM_Cinta cintaPM = pmCintaRules.GeneraArchivo(this.request.TipoReporte, "13,");
                    request.ArchivoPM = cintaPM.ArchivoId;

                    // Persona Física Desarrollo
                    ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.GENERAR_PF));
                    PF_Cinta cintaPF = pfCintaRules.GeneraArchivo(this.request.TipoReporte, "13,");
                    request.ArchivoPF = cintaPF.ArchivoId;

                    // INICIO Nuevo Desarrollo Garantias
                    ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.GENERA_GPOS_LCCYR));
                    /* Archivo_Personas_Morales_Gpos_Lcryc Archivo = */

                    request.Fecha_Final = DateTime.Now;
                    ReportProgress(100, request.ReportaEstado(Request_Estado.Estado.COMPLETO));
                    // FIN Nuevo Desarrollo Garantias

                }
            }
            catch (Exception ex)
            {
                request.Mensaje = "Error: " +  ex.Message;
                if (ex.InnerException != null)
                {
                    request.Mensaje += string.Format("{0} {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }

                request.Fecha_Final = DateTime.Now;
                ReportProgress(0, request.ReportaEstado(Request_Estado.Estado.ERROR));
            }
        }

    }

}