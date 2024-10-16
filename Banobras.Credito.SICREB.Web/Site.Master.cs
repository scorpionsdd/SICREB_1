using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Business.Seguridad;
using Banobras.Credito.SICREB.Common.ExceptionMng;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;

public partial class Site : System.Web.UI.MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;


    protected void Page_Load(object sender, EventArgs e)
    {

        string pathActual = this.Request.Path;
        int diagonal = pathActual.LastIndexOf('/');
        int punto = pathActual.LastIndexOf('.');
        int inicio = pathActual.LastIndexOf('~');
        string pageActual = pathActual.Substring(inicio + diagonal + 1, punto - diagonal);

        //verificas si page viene de raiz  para ubicar nivel de busqueda de imagenes (JAGH 13/12/12)        
        string[] arrayInfo = pathActual.Split('/');

        string strIndicador = string.Empty;
        if (arrayInfo.Length > 2)
            strIndicador = "../";

        //creas estilos 
        string strStyle = " body{background-image:url('" + strIndicador + "ResourcesSICREB/Images/FondoDegradado.png');}  ";
        strStyle += " table.Footer01 tr { width:100%;background:url('" + strIndicador + "ResourcesSICREB/Images/HeadersFooters/BNB-SIC-Footer01.png'); background-repeat:repeat-x; }  ";
        strStyle += " table.FooterPage { width:100%; height:66px; background:url('" + strIndicador + "ResourcesSICREB/Images/HeadersFooters/BNB-SIC-Footer01.png'); padding-bottom:0px; padding-top:0px; }  ";
        strStyle += " table.BKGDTable {background:url('" + strIndicador + "ResourcesSICREB/Images/FondoDegradado.png') repeat-x; top: 0px; left: 0px; height:89px;} ";

        //creas objeto estilo para aplicarlo a los elementos del master
        System.Web.UI.HtmlControls.HtmlGenericControl style = new System.Web.UI.HtmlControls.HtmlGenericControl();
        style.TagName = "style";
        style.Attributes.Add("type", "text/css");
        style.InnerHtml = strStyle;
        Page.Header.Controls.Add(style);
        //fin verificas si page viene raiz

        linkAyuda.Attributes.Add("onclick", "javascript:if(window.open('Ayuda.aspx?NombreParent=" + pageActual + "','Ayuda','height=555, Width=700, Top=100, Left=200, center=yes, help=no, resizable=no, status=no,scrollbars=yes')==false)return false;");

        if (Session["UserLogin"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
            this.lblUser.Text = Session["nombreUser"].ToString();
        }

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        //First, check for the existence of the Anti-XSS cookie
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;

        //If the CSRF cookie is found, parse the token from the cookie.
        //Then, set the global page variable and view state user key. The global variable will be used to validate that it matches 
        //in the view state form field in the Page.PreLoad method.
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            //Set the global token variable so the cookie value can be validated against the value in the view state form field in the Page.PreLoad method.
            _antiXsrfTokenValue = requestCookie.Value;

            //Set the view state user key, which will be validated by the framework during each request
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        //If the CSRF cookie is not found, then this is a new session.
        else
        {
            //Generate a new Anti-XSRF token
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

            //Set the view state user key, which will be validated by the framework during each request
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            //Create the non-persistent CSRF cookie
            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                //Set the HttpOnly property to prevent the cookie from being accessed by client side script
                HttpOnly = true,

                //Add the Anti-XSRF token to the cookie value
                Value = _antiXsrfTokenValue
            };

            //If we are using SSL, the cookie should be set to secure to prevent it from being sent over HTTP connections
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            
            //Add the CSRF cookie to the response
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;    
    }


    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        //During the initial page load, add the Anti-XSRF token and user name to the ViewState
        if (!IsPostBack)
        {
            //Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

            //If a user name is assigned, set the user name
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        //During all subsequent post backs to the page, the token value from the cookie should be validated against the token in the view state
        //form field. Additionally user name should be compared to the authenticated users name
        else
        {
            //Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of " + "Anti-XSRF token failed.");
            }
        }
    }

    
    protected void Salir1_Click(object sender, EventArgs e)
    {
        try
        {
            int userId = Convert.ToInt32(Session["idUsuario"].ToString());
            //Ubicando usuario
            UsuarioEntidadRules uer = new UsuarioEntidadRules();
            List<Usuario> userList = uer.Usuarios();
            Usuario currentUserData = userList.ToList().FirstOrDefault(x => x.Id == userId);
            currentUserData.SessionIP = " ";

            //Eliminando sesión de usuario en T_Usuarios
            int userUpdated = uer.ActualizarUsuario(new Usuario(), currentUserData);

            //Agregando bítácora de cierre
            this.SaveCloseSession();

            Session.RemoveAll();
            Response.Redirect("~/Login.aspx", false);
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Guardando sesión
    /// </summary>
    protected void SaveCloseSession()
    {
        //Agregando bítácora de cierre
        Bitacora bitacora = new Bitacora
        {
            CreationDate = DateTime.Now,
            Comments = String.Format("Cierre de sessión. Login: {0}.", Session["UserLogin"]),
            EmployeeNumber = Parser.ToNumber(Session["EmpleadoId"]),
            EventId = (int)BitacoraEventoTipoEnum.Usuario_CerrarSesion,
            LogType = BitacoraTipoEstatusEnum.Permissions,
            //Request = JsonConvert.SerializeObject(userData) + "   " + JsonConvert.SerializeObject(usuarioRolData),
            SessionIP = Session["SessionIP"].ToString(),
            UserLogin = Session["UserLogin"].ToString(),
            UserFullName = Session["nombreUser"].ToString(),
        };
        bool isSuccess = BitacoraRules.AgregarBitacora(bitacora);
    }

}


