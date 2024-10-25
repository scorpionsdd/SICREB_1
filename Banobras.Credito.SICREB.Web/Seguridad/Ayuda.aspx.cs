using Banobras.Credito.SICREB.Common.ExceptionHelpers;
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
                string url = string.Format("./Ayuda{0}.html", Request.Params["NombreParent"]);
                string strScript = string.Format("<script type='text/javascript'>window.location.href='{0}';</script>", url);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", strScript, false);

                //string url = string.Format("Seguridad/Ayuda{0}.html", Request.Params["NombreParent"]);
                //string urlComplete = this.GetUrl(url);
                //bool isValid = this.IsValidRedirectUrl(urlComplete);
                //Page.Response.Write("<script>console.log('" + "Ruta de ayuda: " + urlComplete + "');</script>");
                //Page.Response.Write("<script>console.log('" + "isValid: " + isValid.ToString() + "');</script>");

                //if (isValid)
                //{
                //    Response.Redirect(urlComplete, false);
                //}
                //else
                //{
                //    //string strScript = "<script type='text/javascript'>window.opener.location.href='../Login.aspx';self.close();</script>";
                //    //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", strScript, false);

                //    Page.Response.Write("<script>console.log('" + "Ruta de ayuda: " + urlComplete + "');</script>");
                //}
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
        string a = pathArray[1]; //Banobras.Credito.SICREB.Web
        string b = pathArray[2]; //Seguridad
#if DEBUG
               urlComplete = string.Format("{0}://{1}:{2}/{3}/{4}/{5}", protocol, host, port, a, b, url);
#elif RELEASE
                urlComplete = string.Format("{0}://{1}:{2}/{3}", protocol, host, port, url);
#endif
        
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
               if (result.Host.Contains("localhost")) isValid = true;
            #elif RELEASE
                if (result.Host.Contains("banobras.gob.mx")) isValid = true;
            #endif
        }
        Page.Response.Write("<script>console.log('" + "Host: " + result.Host + "');</script>");

        return isValid;
    }

    private bool IsValirURL(string url)
    {
        Uri result;
        var obj = Uri.TryCreate(url, UriKind.Absolute, out result);
        Uri result2;
        var obj2 = Uri.TryCreate(url, UriKind.Relative, out result2);


        var uriToVerify = new Uri(url);
        var isValidUri = uriToVerify.IsWellFormedOriginalString();
        var isValidScheme = uriToVerify.Scheme == "http" || uriToVerify.Scheme == "https";

        return true;
    }

}