using Banobras.Credito.SICREB.Common;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Mail;

/// <summary>
/// Descripción breve de cls_EnvioAlertas
/// </summary>
namespace Banobras.Credito.SICREB.Data.Transaccionales
{
    public class cls_EnvioAlertas : OracleBase
    {

        public cls_EnvioAlertas()
        {
        }

        public enum Periocidad
        {
            Mensual,
            Semanal
        }

        //verifica si el dia de hoy es habil    
        public bool ValidaFecha(DateTime hoy)
        {
            cls_Mod_ValidaFechaHabil obj = new cls_Mod_ValidaFechaHabil();
            //DateTime hoy = DateTime.Now;
            DateTime fecha = obj.ValidaFechaHabil(hoy, cls_Mod_ValidaFechaHabil.Dir.Adelante);
            string vIDENTIFICADORALERTA, vMENSAJEALERTA, vTITULOALERTA, vPERIODICIDAD, vFECHAAPLICACIONPERIODO, vdestinatario;
            char vESTATUS, vACTIVADA;
            bool bandera = false;

            if (hoy.Equals(fecha))
            {
                if (Int32.Parse(fecha.Day.ToString()).Equals(8))
                {
                    string query = string.Format("SP_ENVIOALERTAS");

                    DbCommand cmd = DB.GetStoredProcCommand(query);
                    DB.AddInParameter(cmd, "vPeriodicidad", DbType.String, Periocidad.Mensual);

                    using (IDataReader reader = DB.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            vIDENTIFICADORALERTA = reader["IDENTIFICADORALERTA"].ToString();
                            vTITULOALERTA = reader["TITULOALERTA"].ToString();
                            vMENSAJEALERTA = reader["MENSAJEALERTA"].ToString();
                            vPERIODICIDAD = reader["PERIODICIDAD"].ToString();
                            vFECHAAPLICACIONPERIODO = reader["FECHAAPLICACIONPERIODO"].ToString();
                            vESTATUS = char.Parse(reader["ESTATUS"].ToString());
                            vACTIVADA = char.Parse(reader["ACTIVADA"].ToString());
                            vdestinatario = reader["destinatario"].ToString();
                            this.EnviaAlerta(vdestinatario, vMENSAJEALERTA, vTITULOALERTA);
                        }
                    }

                    bandera = true;
                }
                else if (Convert.ToDateTime(fecha).DayOfWeek == DayOfWeek.Monday)
                {
                    string query = string.Format("SP_ENVIOALERTAS");

                    DbCommand cmd = DB.GetStoredProcCommand(query);
                    DB.AddInParameter(cmd, "vPeriodicidad", DbType.String, Periocidad.Semanal);

                    using (IDataReader reader = DB.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            vIDENTIFICADORALERTA = reader["IDENTIFICADORALERTA"].ToString();
                            vTITULOALERTA = reader["TITULOALERTA"].ToString();
                            vMENSAJEALERTA = reader["MENSAJEALERTA"].ToString();
                            vPERIODICIDAD = reader["PERIODICIDAD"].ToString();
                            vFECHAAPLICACIONPERIODO = reader["FECHAAPLICACIONPERIODO"].ToString();
                            vESTATUS = char.Parse(reader["ESTATUS"].ToString());
                            vACTIVADA = char.Parse(reader["ACTIVADA"].ToString());
                            vdestinatario = reader["destinatario"].ToString();
                            this.EnviaAlerta(vdestinatario, vMENSAJEALERTA, vTITULOALERTA);
                        }
                    }
                    bandera = true;
                }
            }

            return bandera;
        }

        public void EnviaAlerta(string destinatarios, string MENSAJEALERTA, string TITULOALERTA)
        {
            string de = "ww.facebook.com@live.com.mx";
            System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage()
            {
                Subject = TITULOALERTA,
                Body = MENSAJEALERTA,
                IsBodyHtml = false,
                From = new MailAddress(de.ToString(), "Dulce Maria Martinez")
            };

            char[] delimitador = new char[] { ';' };
            string[] pnDestinatarios = destinatarios.ToString().Split(delimitador, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < pnDestinatarios.Length; i++)
            {
                Email.CC.Add(pnDestinatarios[i]);
            }

            System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("smtp.live.com");
            Email.IsBodyHtml = false;
            smtpMail.UseDefaultCredentials = false;
            smtpMail.Port = 25;
            smtpMail.EnableSsl = true;
            //smtpMail.Credentials = new System.Net.NetworkCredential("ww.facebook.com@live.com.mx", "patitas");

            var environmentVariableValues = Util.GetEnvironmentVariableValues();
            var valuesAD = environmentVariableValues.Emails.FirstOrDefault(x => x.Scheme == "SendAlerts");
            smtpMail.Credentials = new System.Net.NetworkCredential(valuesAD.UserId, valuesAD.Password);
            smtpMail.Send(Email);

            //Email.CC.Add(new MailAddress(destinatarios));
        }


    }
}