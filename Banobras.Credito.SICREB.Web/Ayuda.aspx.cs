using System;
using System.Web.UI;
using System.Web;

public partial class Ayuda : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {  
        if (!Page.IsPostBack)
        {
            //JAGH se agregan actividades 16/01/13
            ////int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ////ActividadRules.GuardarActividad(600, idUs, "Se ha ejecutado la ayuda del SICREB desde " + Request.Params["NombreParent"].ToString());
            string url = string.Format("./Ayuda{0}.htm", Request.Params["NombreParent"]);
            ////Response.Redirect(url);
            ////var xxx = this.IsLocalUrl(url);
            string strScript = string.Format("<script type='text/javascript'>window.location.href='{0}';</script>", url);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", strScript, false);
        }       
    }

    private bool IsLocalUrl(string url)
    {
        var httpRequestBase = new HttpRequestWrapper(HttpContext.Current.Request) as HttpRequestBase;
        var xxx = httpRequestBase.IsLocal; //.IsUrlLocalToHost(url);

        return xxx;
    }

}