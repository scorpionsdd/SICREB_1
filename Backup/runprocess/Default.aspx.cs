using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Business.Rules.PM;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Entities.Util;
using System.IO;
using System.Text;

namespace runprocess
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Button1_Click(object sender, EventArgs e)
        {
            PM_Cinta_Rules pmCintaRules = new PM_Cinta_Rules();
            PM_Cinta cintaPM = new PM_Cinta();

            cintaPM = pmCintaRules.GeneraArchivo(Banobras.Credito.SICREB.Entities.Util.Enums.Reporte.Mensual, "13,");
            lblEstado.Text = "Proceso terminado.";            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Archivos_Rules archivos = new Archivos_Rules(Enums.Persona.Moral);
            Archivo ultimoArchivo = archivos.GetUltimoArchivo();
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            sw.Write(encoding.GetString(ultimoArchivo.BytesArchivo));
            sw.Flush();

            Response.Clear();
            Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", ultimoArchivo.Nombre));
            Response.AppendHeader("Content-Length", ms.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(ms.ToArray());
            Response.End();

        }      
    }
}