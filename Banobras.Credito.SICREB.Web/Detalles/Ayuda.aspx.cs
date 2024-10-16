using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Common.ExceptionMng;
using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Web;
using System.Web.UI;


public partial class Ayuda : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                //JAGH se agregan actividades 16/01/13
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(600, idUs, "Se ha ejecutado la ayuda del SICREB desde " + Request.Params["NombreParent"].ToString());
                ////Response.Redirect(string.Format("./Ayuda/{0}.htm", Request.Params["NombreParent"]));

                string url = string.Format("Ayuda{0}.htm", Request.Params["NombreParent"]);
                string urlComplete = this.GetUrl(url);
                bool isValid = this.IsValidRedirectUrl(urlComplete);
                if (isValid)
                {
                    Response.Redirect(urlComplete, false);
                }
                else
                {
                    string strScript = "<script type='text/javascript'>window.opener.location.href='~/Login.aspx';self.close();</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", strScript, false);
                }
            }
            catch (Exception ex)
            {
                string message = ExceptionMessageHelper.GetExceptionMessage(ex);
                Mensajes.ShowMessage(this.Page, this.GetType(), message);
            }
        }
    }



    /// <summary>
    /// Obtener la url completa
    /// </summary>
    /// <param name="url">Url a validar</param>
    /// <returns></returns>
    private string GetUrl(string url)
    {
        string urlComplete = string.Empty;

        string protocol = HttpContext.Current.Request.Url.Scheme;
        string host = HttpContext.Current.Request.Url.Host;
        string port = HttpContext.Current.Request.Url.Port.ToString();
        var pathArray = HttpContext.Current.Request.Url.AbsolutePath.Split('/'); // => /Banobras.Credito.SICREB.Web/Seguridad/Ayuda.aspx
        string a = pathArray[1];
        string b = pathArray[2];
        urlComplete = string.Format("{0}://{1}:{2}/{3}/{4}/{5}", protocol, host, port, a, b, url);

        return urlComplete;
    }

    /// <summary>
    /// Validar si la url está dentro del dominio, para lo cual debe ser una url absoluta.
    /// Absoluta: http://localhost:1234/Banobras.Credito.SICREB.Web/Seguridad/Ayuda/UsuariosPage.htm
    /// Relativa: ./Ayuda/UsuariosPage.htm
    /// </summary>
    /// <param name="url">Url a validar</param>
    /// <returns></returns>
    private bool IsValidRedirectUrl(string url)
    {
        bool isValid = false;
        Uri result;
        Uri.TryCreate(url, UriKind.Absolute, out result);
        // Check if the URL is within your domain
        if (result != null) // && (result.Host == "banobras.gob.mx" || result.Host == "localhost")
        {
#if DEBUG
               if (result.Host == "localhost") isValid = true;
#elif RELEASE
                if (result.Host == "banobras.gob.mx") isValid = true;
#endif
        }

        return isValid;
    }




}