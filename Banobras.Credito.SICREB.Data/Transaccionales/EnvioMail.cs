using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Mail;
using System.Net.Mail;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// JAGH 18/12/12
/// Clase para envio de correo electonico via smtp de GMail
/// </summary>
namespace Banobras.Credito.SICREB.Data.Transaccionales
{

    public static class EnvioMail //: OracleBase
    {    
    
        //metodo para envio con datos de web.config
        public static void EnviaAlerta(string destinatarios, string MENSAJEALERTA, string TITULOALERTA)
        {
            Hashtable htConfig = obtenConfig();

            System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage()
            {
                Subject = TITULOALERTA,
                Body = MENSAJEALERTA,
                IsBodyHtml = false,
                From = new MailAddress(htConfig["strFrom"].ToString(), "Administrador SICREB")
            };
            
            //Attachment adjuntos = new Attachment("ruta_de_nuestro_archivo/carta.txt");
            //Email.Attachments.Add(adjuntos);
            char[] delimitador = new char[] { ';' };
            string[] pnDestinatarios = destinatarios.ToString().Split(delimitador, StringSplitOptions.RemoveEmptyEntries);
            
            //recorre destinatarios
            for (int i = 0; i < pnDestinatarios.Length; i++)
            {
                Email.CC.Add(pnDestinatarios[i]);

            }
            
            //configura y envia
            System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient(htConfig["strHost"].ToString(), Convert.ToInt32(htConfig["iPort"].ToString()));
            Email.IsBodyHtml = false;            
            smtpMail.UseDefaultCredentials = false;            
            smtpMail.EnableSsl = true;
            smtpMail.Credentials = new System.Net.NetworkCredential(htConfig["strUser"].ToString(), htConfig["strPass"].ToString());
            smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;            
            smtpMail.Send(Email);                   
        }

        //obtiene configuracion de envio de mail
        private static Hashtable obtenConfig()
        {
            Hashtable htInfo = new Hashtable();
            try
            {
                System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
                System.Net.Configuration.MailSettingsSectionGroup mailSection = config.GetSectionGroup("system.net/mailSettings") as System.Net.Configuration.MailSettingsSectionGroup;
                htInfo.Add("strFrom", mailSection.Smtp.From);
                htInfo.Add("strHost", mailSection.Smtp.Network.Host);
                htInfo.Add("strUser", mailSection.Smtp.Network.UserName);
                htInfo.Add("strPass", mailSection.Smtp.Network.Password);
                htInfo.Add("iPort", mailSection.Smtp.Network.Port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                htInfo.Add("Error", "Error" + ex.ToString());
                return htInfo; 
            }

            return htInfo;

        }

        //metodo para envio con datos de settings service
        public static void EnviaServicioAlerta(string destinatarios, string MENSAJEALERTA, string TITULOALERTA, Hashtable htConfig)
        {           
            System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage()
            {
                Subject = TITULOALERTA,
                Body = MENSAJEALERTA,
                IsBodyHtml = false,
                From = new MailAddress(htConfig["strFrom"].ToString(), "Administrador SICREB")
            };

            char[] delimitador = new char[] { ';' };
            string[] pnDestinatarios = destinatarios.ToString().Split(delimitador, StringSplitOptions.RemoveEmptyEntries);
            
            //recorre destinatarios
            for (int i = 0; i < pnDestinatarios.Length; i++)
            {
                Email.CC.Add(pnDestinatarios[i]);

            }
            
            //configura y envia
            System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient(htConfig["strHost"].ToString(), Convert.ToInt32(htConfig["iPort"].ToString()));
            Email.IsBodyHtml = false;
            smtpMail.UseDefaultCredentials = false;
            //smtpMail.Port = 465;
            smtpMail.EnableSsl = true;
            smtpMail.Credentials = new System.Net.NetworkCredential(htConfig["strUser"].ToString(), htConfig["strPass"].ToString());
            smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpMail.Send(Email);

        }
    
    }

}
