<%@ Application Language="C#" %>
<%@ Import Namespace="Banobras.Credito.SICREB.Business.Repositorios" %>
<%@ Import Namespace="Banobras.Credito.SICREB.Business.Seguridad" %>
<%@ Import Namespace="Banobras.Credito.SICREB.Common.HttpUnity" %>
<%@ Import Namespace="Banobras.Credito.SICREB.Common.ExceptionMng" %>
<%@ Import Namespace="Banobras.Credito.SICREB.Entities" %>
<%@ Import Namespace="Banobras.Credito.SICREB.Entities.Util" %>


<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        Microsoft.Practices.Unity.IUnityContainer myContainer = Application.GetContainer();
        myContainer.AddExtension(new Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity.EnterpriseLibraryCoreExtension());
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
    }
        
    void Application_Error(object sender, EventArgs e) 
    {         
        //Registrar la excepción utilizando algún framework como log4
        Exception ex = Server.GetLastError();
        
        //Redirigir a página de error personalizada
        if (ex != null)
        {
            Server.ClearError();
            Response.Redirect("~/Error.aspx");
        }
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        try
        {
            this.SaveSessionUser();
            this.SaveCloseSession();
            Session.RemoveAll();
            
            System.IO.File.Delete(Server.MapPath("../Logs/log" + Session.SessionID + ".txt"));
        }
        catch (Exception ex) 
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(new Page(), this.GetType(), message);
        }
    }
    
    void Application_BeginRequest(object sender, EventArgs e)
    {
        //Protección del Clickjacking, permitiendo que el contenido sea embebido sólo en páginas del mismo dominio.
        //Esto aplicará la cabecera "X-Frame-Options" a todas las respuestas HTTP de la aplicación.
        HttpContext.Current.Response.AddHeader("X-Frame-Options", "SAMEORIGIN");

        HttpContext.Current.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        HttpContext.Current.Response.Headers.Add("Content-Security-Policy", "default-src 'self' 'unsafe-inline'; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://ajax.googleapis.com; style-src 'self' 'unsafe-inline';");
        if (HttpContext.Current.Request.IsSecureConnection)
        {
            HttpContext.Current.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        }
    }

    void Application_EndRequest(object sender, EventArgs e)
    {
    }
        
    /// <summary>
    /// Limpiando el campo de IP del usuario
    /// </summary>
    private void SaveSessionUser()
    {
        int userId = Convert.ToInt32(Session["idUsuario"].ToString());
        
        //Ubicando usuario
        UsuarioEntidadRules objUsuario = new UsuarioEntidadRules();
        List<Usuario> userList = objUsuario.Usuarios();
        Usuario currentUserData = userList.ToList().FirstOrDefault(x => x.Id == userId);
        currentUserData.SessionIP = " ";

        //Eliminando sesión de usuario en T_Usuarios
        int userUpdated = objUsuario.ActualizarUsuario(new Usuario(), currentUserData);
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
       
</script>
