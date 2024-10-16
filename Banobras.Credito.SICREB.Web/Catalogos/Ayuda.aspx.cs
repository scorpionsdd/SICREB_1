using System;
using System.Web.UI;

public partial class Ayuda : System.Web.UI.Page
{ 
  public const String catalog = "SEPOMEX";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            //JAGH se agregan actividades 16/01/13
            ////int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ////ActividadRules.GuardarActividad(600, idUs, "Se ha ejecutado la ayuda del SICREB desde " + Request.Params["NombreParent"].ToString());
            string url = string.Format("./Ayuda{0}.htm", Request.Params["NombreParent"]);
            ////Response.Redirect(url);
            string strScript = string.Format("<script type='text/javascript'>window.location.href='{0}';</script>", url);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", strScript, false);
                
        }
    }
}