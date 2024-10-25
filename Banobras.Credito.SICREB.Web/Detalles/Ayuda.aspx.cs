using Banobras.Credito.SICREB.Common.ExceptionHelpers;
using System;
using System.Web.UI;


public partial class Ayuda : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
			    string url = string.Format("./Ayuda{0}.htm", Request.Params["NombreParent"]);		
                string strScript = string.Format("<script type='text/javascript'>window.location.href='{0}';</script>", url);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", strScript, false);
            }
            catch (Exception ex)
            {
                string message = ExceptionMessageHelper.GetExceptionMessage(ex);
                Mensajes.ShowMessage(this.Page, this.GetType(), message);
            }
        }
	}
}