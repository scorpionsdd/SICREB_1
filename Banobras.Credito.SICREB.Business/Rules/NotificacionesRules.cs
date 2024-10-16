using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Common;
using System.Drawing;

namespace Banobras.Credito.SICREB.Business.Rules
{
    public class NotificacionesRules
    {
        private List<Validacion> validacionesPM = null;
        private List<Validacion> validacionesPF = null;

        public NotificacionesRules()
        {
            ValidacionesDataAccess valDataPM = new ValidacionesDataAccess(Enums.Persona.Moral);
            validacionesPM = valDataPM.GetRecords(true);

            ValidacionesDataAccess valDataPF = new ValidacionesDataAccess(Enums.Persona.Fisica);
            validacionesPF = valDataPF.GetRecords(true);
        }

        public void EnviaNotificaciones(Enums.Persona persona)
        {
            ErroresWarnings_Rules erroresRules = new ErroresWarnings_Rules();

            List<ErrorWarningInfo> errores = (from err in erroresRules.GetErroresPresentacion(persona)
                                              orderby err.Error.UsuarioAlta
                                              orderby err.Error.Rfc
                                              orderby err.Validacion.Codigo
                                              select err).ToList();
            
            if (errores != null && errores.Count > 0)
            {
                var usuarios = (from er in errores
                                where er.Error != null && er.Error.UsuarioAlta.Trim() != String.Empty
                                select er.Error.UsuarioAlta.ToUpper().Trim()).Distinct();

                foreach (string usuario in usuarios)
                {
                    var misErrores = (from er in errores
                                      where er.Error != null && er.Error.UsuarioAlta.ToUpper().Trim().Equals(usuario)
                                      select er).ToList();
                    EnviaNotificaciones(usuario, misErrores, (persona == Enums.Persona.Moral ? validacionesPM : validacionesPF), persona);
                }
            }
        }
        private bool EnviaNotificaciones(string usuario, List<ErrorWarningInfo> errores, List<Validacion> validaciones, Enums.Persona persona)
        {
            //formar la tablita que muestra los errores:)
            if(errores == null || errores.Count == 0)
            {
                //nada que reportar! Todo esta bien
                return true;
            }

            try
            {
                //ActiveDir active = new ActiveDir();
                //usuario = active.GetEmail(usuario);
                usuario = ActiveDir.GetEmail(usuario);

                if (String.IsNullOrWhiteSpace(usuario))
                {
                    return false;
                }

                string color1 = "1C446C";
                string color2 = "E2E2EC";
                string color3 = "F2F2F2";
                bool mainColor = false;

                StringBuilder builder = new StringBuilder();
                builder.Append("<html><body>");
                
                
                var rfcs = (from er in errores
                            where er.Error != null && er.Error.Rfc.Trim() != String.Empty
                            select er.Error.Rfc.ToUpper().Trim()).Distinct();

                foreach (string rfc in rfcs)
                {
                    mainColor = false;
                    builder.AppendFormat("<span style='font-size:x-large; color:{0};'>{1}</span><br />", color1, rfc);
                    builder.AppendFormat("<table><tr style='font-weight:bold; color:#ffffff; background-color:#{0};'>", color1);
                    builder.Append("<td>Etiqueta</td><td>Campo</td><td>Rfc</td><td>Credito</td><td>Dato</td><td>Codigo Error</td><td>Tipo</td><td>Mensaje</td><td>Aplica</td>");
                    builder.Append("</tr>");
                    var reporte = (from er in errores
                                   where er.Error != null && er.Error.Rfc.ToUpper().Trim().Equals(rfc)
                                   orderby er.Error.Credito
                                   select er).ToList();
                    foreach (ErrorWarningInfo error in reporte)
                    {
                        mainColor = !mainColor;

                        builder.AppendFormat("<tr style='background-color:#{0}; color:#{1};'>", (mainColor ? color2 : color3), color1);
                        builder.AppendFormat("<td>{0}</td>", error.SegEtiq);
                        builder.AppendFormat("<td>{0}</td>", error.Etiqueta.Descripcion);
                        builder.AppendFormat("<td>{0}</td>", error.Error.Rfc);
                        builder.AppendFormat("<td>{0}</td>", error.Error.Credito);
                        builder.AppendFormat("<td>{0}</td>", error.Error.Dato);
                        builder.AppendFormat("<td><b>{0}</b></td>", error.Validacion.Codigo);
                        builder.AppendFormat("<td>{0}</td>", error.Validacion.Tipo);
                        builder.AppendFormat("<td>{0}</td>", error.Validacion.Mensaje);
                        builder.AppendFormat("<td><input type='checkbox' disabled='disabled' {0} /></td>", (error.Validacion.Aplicable ? "checked='checked'" : string.Empty));
                        builder.Append("</tr>");
                    }
                    builder.Append("</table>");
                }
                
                builder.Append("</body></html>");

                MailMessage msg = new MailMessage(WebConfig.MailFrom, usuario);
                msg.IsBodyHtml = true;
                msg.Subject = string.Format("Reporte de errores Persona {0}", persona);
                msg.Body = builder.ToString();

                SmtpClient client = new SmtpClient(WebConfig.SmtpClient);
                client.Send(msg);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
