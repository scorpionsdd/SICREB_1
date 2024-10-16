using System;
using System.Web;
using Telerik.Web.UI;
using System.Web.UI;
using System.Text;
using Banobras.Credito.SICREB.Business;
using Banobras.Credito.SICREB.Common.Block;
using Banobras.Credito.SICREB.Common;

/// <summary>
/// Descripción breve de Mensajes
/// </summary>
public class Mensajes
{
    enum Tipo { ERROR, SUCCESS, ADVERTENCIA };

    public Mensajes()
    {
    }

    /// <summary>
    /// Muestra error en pantalla y loguea la informacion en el LOG
    /// </summary>
    /// <param name="page">Desde web: Page</param>
    /// <param name="type">Desde web: this.GetType()</param>
    /// <param name="ex">Excepcion que ocurrió</param>
    public static void ShowError(Page page, Type type, Exception ex)
    {
        ShowError(page, type, ex, string.Empty);
    }

    /// <summary>
    /// Muestra error en pantalla y loguea la informacion en el LOG
    /// </summary>
    /// <param name="page">Desde web: Page</param>
    /// <param name="type">Desde web: this.GetType()</param>
    /// <param name="error">Mensaje de error a mostrar</param>
    public static void ShowError(Page page, Type type, string error)
    {
        ShowError(page, type, error, string.Empty);
    }

    /// <summary>
    /// Muestra error en pantalla y loguea la informacion en el LOG
    /// </summary>
    /// <param name="page">Desde web: Page</param>
    /// <param name="type">Desde web: this.GetType()</param>
    /// <param name="error">Mensaje de error a mostrar</param>
    /// <param name="urlToRedirect">Redirige a esta url después de hacer click en el botón</param>
    public static void ShowError(Page page, Type type, string error, string urlToRedirect)
    {
        ShowError(page, type, new Exception(error), urlToRedirect);
    }

    /// <summary>
    /// Muestra error en pantalla y loguea la informacion en el LOG
    /// </summary>
    /// <param name="page">Desde web: Page</param>
    /// <param name="type">Desde web: this.GetType()</param>
    /// <param name="ex">Excepcion que ocurrió</param>
    /// <param name="urlToRedirect">Redirige a esta url después de hacer click en el botón</param>
    public static void ShowError(Page page, Type type, Exception ex, string urlToRedirect)
    {
        ShowMessage(page, type, ex.Message, Tipo.ERROR, urlToRedirect);

        if (ex.InnerException != null)
        {
            BLog log = BLog.Current;
            if (log != null)
            {
                log.LogWrtMrg.Write(string.Format(@"{0} ({1}) STACK: {2}", ex.Message, ex.InnerException.Message, ex.InnerException.StackTrace), Definitions.CATEGORY_WC, 1, 1000);
            }
        }

    }

    /// <summary>
    /// Muestra un mensaje en pantalla.
    /// </summary>
    /// <param name="page">Desde web: Page</param>
    /// <param name="type">Desde web: this.GetType()</param>
    /// <param name="mensaje">Mensaje a mostrar</param>
    public static void ShowMessage(Page page, Type type, string mensaje)
    {
        ShowMessage(page, type, mensaje, string.Empty);
    }

    /// <summary>
    /// Muestra un mensaje en pantalla.
    /// </summary>
    /// <param name="page">Desde web: Page</param>
    /// <param name="type">Desde web: this.GetType()</param>
    /// <param name="mensaje">Mensaje a mostrar</param>
    /// <param name="urlToRedirect">Redirige a esta url después de hacer click en el botón</param>
    public static void ShowMessage(Page page, Type type, string mensaje, string urlToRedirect)
    {
        ShowMessage(page, type, mensaje, Tipo.SUCCESS, urlToRedirect);
    }

    /// <summary>
    /// Muestra un mensaje de advertencia en la pantalla
    /// </summary>
    /// <param name="page">Desde web: Page</param>
    /// <param name="type">Desde web: this.GetType()</param>
    /// <param name="mensaje">Mensaje a mostrar</param>
    public static void ShowAdvertencia(Page page, Type type, string mensaje)
    {
        ShowAdvertencia(page, type, mensaje, string.Empty);
    }

    /// <summary>
    /// Muestra un mensaje de advertencia en la pantalla y redirecciona a otro url
    /// </summary>
    /// <param name="page">Desde web: Page</param>
    /// <param name="type">Desde web: this.GetType()</param>
    /// <param name="mensaje">Mensaje a mostrar</param>
    /// <param name="urlToRedirect">Redirige a esta url después de hacer click en el botón</param>
    public static void ShowAdvertencia(Page page, Type type, string mensaje, string urlToRedirect)
    {
        ShowMessage(page, type, mensaje, Tipo.ADVERTENCIA, urlToRedirect);
    }

    private static void ShowMessage(Page page, Type type, string message, Tipo tipoMsg, string urlToRedirect)
    {
        if (!String.IsNullOrWhiteSpace(urlToRedirect))
        {
            urlToRedirect = page.ResolveClientUrl(urlToRedirect);
        }

        StringBuilder radAlertScript = new StringBuilder();
        radAlertScript.Append(@"<script language='javascript'>function f(){radalert('");
        radAlertScript.Append(message);
        radAlertScript.Append(@"','400', 'Error');");
        radAlertScript.Append(@"Sys.Application.remove_load(f);}; Sys.Application.add_load(f); </script>");


        page.ClientScript.RegisterStartupScript(type, "radalert", radAlertScript.ToString());

    }

    ///// <summary>
    ///// Muestra un error en pantalla.
    ///// </summary>
    ///// <param name="Message"></param>
    ///// <param name="page">Indica la pagina donde se buscará el control AjaxManager.</param>
    //public static void ShowError(string Message, Page page)
    //{
    //    Mensajes.ErrorMessage("KeyErrorMessage", Message, false, page, string.Empty);
    //}

    ///// <summary>
    ///// Muestra un texto en un alert de JavaScript.
    ///// </summary>
    ///// <param name="Message">Indica el mensaje que se mostrará.</param>
    ///// <param name="bLog">Indica si el mensaje es Almacenado.</param>
    ///// <param name="page">Indica la pagina donde se buscará el control AjaxManager.</param>
    //public static void ShowError(string Message, bool bLog, Page page)
    //{
    //    Mensajes.ErrorMessage("KeyErrorMessage", Message, bLog, page, string.Empty);
    //}

    ///// <summary>
    ///// Muestra un texto en un alert de JavaScript.
    ///// </summary>
    ///// <param name="Message">Indica el mensaje que se mostrará.</param>
    ///// <param name="bLog">Indica si el mensaje es Almacenado.</param>
    ///// <param name="page">Indica la pagina donde se buscará el control AjaxManager.</param>
    ///// <param name="script">Indica el script adicional que se agregrará.</param>
    //public static void ShowError(string Message, bool bLog, Page page, string script)
    //{
    //    Mensajes.ErrorMessage("KeyErrorMessage", Message, bLog, page, script);
    //}

    ///// <summary>
    ///// Genera un script para mostrar un mensaje en JavaScript.
    ///// </summary>
    ///// <param name="oEx">Indica la excepción con el mensaje que se mostrará.</param>
    ///// <param name="page">Indica la pagina donde se buscará el control AjaxManager.</param>
    //public static void ShowError(Exception oEx, Page page)
    //{
    //    Mensajes.ErrorMessage(oEx, "KeyErrorMessage", false, page, string.Empty);
    //}

    ///// <summary>
    ///// Genera un script para mostrar un mensaje en JavaScript.
    ///// </summary>
    ///// <param name="oEx">Indica la excepción con el mensaje que se mostrará.</param>
    ///// <param name="bLog">Indica si el mensaje es Almacenado.</param>
    ///// <param name="page">Indica la pagina donde se buscará el control AjaxManager.</param>
    //public static void ShowError(Exception oEx, bool bLog, Page page)
    //{
    //    Mensajes.ErrorMessage(oEx, "KeyErrorMessage", bLog, page, string.Empty);
    //}

    ///// <summary>
    ///// Genera un script para mostrar un mensaje en JavaScript.
    ///// </summary>
    ///// <param name="oEx">Indica la excepción con el mensaje que se mostrará.</param>
    ///// <param name="bLog">Indica si el mensaje es Almacenado.</param>
    ///// <param name="page">Indica la pagina donde se buscará el control AjaxManager.</param>
    ///// <param name="script">Indica el script adicional que se ejecutará.</param>
    //public static void ShowError(Exception oEx, bool bLog, Page page, string script)
    //{
    //    Mensajes.ErrorMessage(oEx, "KeyErrorMessage", bLog, page, script);
    //}

    ///// <summary>
    ///// Genera un script para mostrar un mensaje en JavaScript.
    ///// </summary>
    ///// <param name="oEx">Indica la excepción con el mensaje que se mostrará.</param>
    ///// <param name="bLog">Indica si el mensaje es Almacenado.</param>
    ///// <param name="page">Indica la pagina donde se buscará el control AjaxManager.</param>
    //private static void ErrorMessage(Exception oEx, string key, bool bLog, Page page, string script)
    //{

    //    string prefix = "Source .";

    //    while (oEx.InnerException != null && oEx.InnerException.Message.Length > 0)
    //    {
    //        oEx = oEx.InnerException;
    //    }

    //    string msg = oEx.Message.Replace("\"", "\\\"").Replace("\'", "\\\"").Replace(Environment.NewLine, "\\r\\n").Replace("\n", "\\n");

    //    if (msg.StartsWith(prefix))
    //    {
    //        msg = msg.Substring(prefix.Length);
    //    }

    //    Mensajes.ErrorMessage(page, key, msg, script);

    //}

    ///// <summary>
    ///// Genera un script para mostrar un mensaje en JavaScript.
    ///// </summary>
    ///// <param name="Message">Indica el mensaje que se mostrará.</param>
    ///// <param name="bLog">Indica si el mensaje es Almacenado.</param>
    ///// <param name="page">Indica la pagina donde se buscará el control AjaxManager.</param>
    //private static void ErrorMessage(string key, string Message, bool bLog, Page page, string script)
    //{

    //    string msg = Message.Replace("\"", "\\\"").Replace("\'", "\\\"").Replace(Environment.NewLine, "\\r\\n");

    //    if (bLog)
    //    {

    //        //FCLogHandling.WriteError(Message, 1, "");
    //        //Loger.LogMessage(Message, true);
    //    }

    //    Mensajes.ErrorMessage(page, key, msg, script);

    //}

    //private static void ErrorMessage(Page page, string key, string msg, string script)
    //{
    //    if (page == null)
    //    {
    //        HttpContext.Current.Response.Write(GeneraScript(string.Format("window.alert('{0}'); {1}", msg, script)));
    //    }
    //    else
    //    {
    //        // if(window.radalert)
    //        string jsScript = string.Format("if(false){0} var oWin = window.radalert('{2}', 330, 100);    if( oWin ){0}  oWin.OnClientClose = function(){0} {3} {1}  {1} {1}   else{0} window.alert('{2}'); {3} {1} ", "{", "}", msg, script);

    //        RadAjaxManager ajaxCtrl = (RadAjaxManager)FindInSubcontrols(page, typeof(RadAjaxManager));
    //        if (ajaxCtrl != null)
    //        {
    //            ajaxCtrl.ResponseScripts.Add(jsScript);
    //        }
    //        else
    //        {
    //            page.ClientScript.RegisterClientScriptBlock(page.GetType(), key, jsScript, true);
    //        }
    //    }
    //}




    //// Mensaje.RegisterScript(string.Format("top.location='/RedApp/VistaSitio.aspx?idSitio={0}&idCandidato={1}';", this.IdSitio, candidato.Id), "Activate", this.Page, false);
    //public static void RegisterScript(string script, string key, Page page, bool executeOnLoad)
    //{

    //    if (executeOnLoad)
    //    {
    //        ScriptManager.RegisterStartupScript(page, page.GetType(), key, script, true);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterClientScriptBlock(page, page.GetType(), key, script, true);
    //    }



    //}


    ////public static string GeneraScript(string codigo)
    ////{
    ////    StringBuilder sbScript = new StringBuilder();
    ////    sbScript.AppendLine("<script type =\"text/javascript\">");
    ////    sbScript.AppendLine("<!--");
    ////    sbScript.AppendLine(codigo);
    ////    sbScript.AppendLine("//-->");
    ////    sbScript.AppendLine("</script>");

    ////    return sbScript.ToString();
    ////}

    /////// <summary>
    /////// Busca un control(objeto) mediante su tipo dentro de otro control.
    /////// </summary>
    /////// <param name="control">Indica el control donde se buscará el objeto.</param>
    /////// <param name="id">Indica el Tipo del objeto que se desea buscar.</param>
    /////// <returns>La referencia al objeto o <code>null</code> sí el objeto no existe.</returns>
    ////public static object FindInSubcontrols(Control control, Type type)
    ////{

    //    object objectSearch = null;

    //    if (type != null)
    //    {
    //        for (int i = 0; i < control.Controls.Count; i++)
    //        {
    //            if (control.Controls[i].GetType() == type)
    //            {
    //                objectSearch = control.Controls[i];
    //            }
    //            if (objectSearch != null) break;
    //        }
    //    }

    //    if (objectSearch == null && control.Controls.Count > 0)
    //    {

    //        foreach (Control ctrl in control.Controls)
    //        {
    //            objectSearch = FindInSubcontrols(ctrl, type);
    //            if (objectSearch != null) break;
    //        }

    //    }

    //    return objectSearch;
    //}


}
