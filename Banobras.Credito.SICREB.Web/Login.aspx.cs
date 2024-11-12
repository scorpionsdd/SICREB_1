using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Business.Seguridad;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Collections.Generic;
using System.Web.UI;

public partial class Loginx : System.Web.UI.Page
{
    public Double Result { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SessionIP"] == null)
        {
            var ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            Session.Add("SessionIP", ipAddress);
            #region Cross Site Request Forgery (WSTG-SESS-05)
            // Genera el token CSRF y guárdalo en la sesión
            string csrfToken = Guid.NewGuid().ToString();
            Session["CsrfToken"] = csrfToken;

            // Asigna el token al campo oculto en el formulario
            csrfToken.Value = csrfToken; 
            #endregion
        }
    }

    protected void ImgBtnIngresar_Click(object sender, ImageClickEventArgs e)
    {
        UsuarioRules usuarioValido = new UsuarioRules();
        UserRolRules usuarioRoles;
        List<CruceUsuarioRol> usuarioRolInfo;
        int userSessionId = 0;

        string facultad = string.Empty;
        string facultades = string.Empty;

        #region Cross Site Request Forgery (WSTG-SESS-05)
        string sessionToken = Session["CsrfToken"] as string;
        string formToken = csrfToken.Value;

        if (sessionToken == null || formToken != sessionToken)
        {
            // La validación ha fallado, bloquea la solicitud
            Response.StatusCode = 403;
            Response.End();
        }
        else
        {
            // Limpia el token para prevenir el uso repetido
            Session["CsrfToken"] = null;

            // Procesa la solicitud
            // ... Lógica del formulario
        } 
        #endregion
        #region DOS attack possible
        if (Session["UserLogin"] != null && Session["UserLogin"].ToString() == txtUsuario.Text.Trim().ToLower())
        {
            if (Session["Intento"] != null)
            {
                Session["Intento"] = Convert.ToInt32(Session["Intento"]) + 1;
                if (Convert.ToInt32(Session["Intento"]) == 3)
                {
                    Session["TiempoDesbloqueo"] = DateTime.Now.AddMinutes(2);
                }

                if (Convert.ToInt32(Session["Intento"]) > 3)
                {
                    DateTime dt = DateTime.Now;
                    var minutosT = (Convert.ToDateTime(Session["TiempoDesbloqueo"]) - dt);
                    if (minutosT.TotalMinutes > 0)
                    {
                        this.SaveLog(String.Format("Número de intentos fallidos. Login: {0}.", this.txtUsuario.Text), BitacoraEventoTipoEnum.Sesion_InicioFallido, BitacoraTipoEstatusEnum.Permissions, string.Empty);
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Usuario sin acceso, alcanzó el límite de intentos. Vuelva a Intentar en: " + minutosT.ToString(@"mm\:ss") + " minutos");
                        return;
                    }
                    else
                    {
                        Session["UserLogin"] = txtUsuario.Text.Trim().ToLower();
                        Session["Intento"] = 1;
                        Session["TiempoDesbloqueo"] = 0;
                    }
                }
            }
            else
            {
                Session["Intento"] = 1;
            }
        }
        else
        {
            Session["UserLogin"] = txtUsuario.Text.Trim().ToLower();
            Session["Intento"] = 1;
            Session["TiempoDesbloqueo"] = null;
        } 
        #endregion

        //Verificando que los campos de ingreso tengan información
        if (!string.IsNullOrEmpty(txtUsuario.Text.Trim()) && !string.IsNullOrEmpty(txtContrasenia.Text.Trim()))
        {
            try
            {
                //Verificar que las credenciales ingresadas existan en el DA
                if (usuarioValido.UsuarioValido(txtUsuario.Text.Trim(), txtContrasenia.Text.Trim()))
                {
                    //Session.Add("usuario", txtUsuario.Text.Trim().ToLower());
                    //Verificar Existencia en BD y sus respectivos Roles
                    usuarioRoles = new UserRolRules(txtUsuario.Text.Trim().ToLower());

                    usuarioRolInfo = usuarioRoles.GetRecords(false);

                    if (usuarioRolInfo.Count > 0)
                    {
                        userSessionId = Parser.ToNumber(usuarioRolInfo[0].IdUsuario);

                        //Ubicando usuario
                        UsuarioEntidadRules objUsuario = new UsuarioEntidadRules();
                        List<Usuario> userList = objUsuario.Usuarios();
                        Usuario currentUserData = userList.ToList().FirstOrDefault(x => x.Id == userSessionId);

                        //Verificando si tiene sesión iniciada
                        //if (!string.IsNullOrEmpty(currentUserData.SessionIP.Trim()))
                        //{
                        //    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Usuario con sesión abierta. Consulte al administrador.");
                        //    return;
                        //}

                        //Guardando sesión de usuario en T_Usuarios
                        currentUserData.SessionDate = DateTime.Now;
                        currentUserData.SessionIP = Session["SessionIP"].ToString();
                        int userUpdated = objUsuario.ActualizarUsuario(new Usuario(), currentUserData);
                        bool isSuccess = ActividadRules.GuardarActividad(100, userSessionId, "Login Correcto en SICREB");

                        facultades = "|" + string.Join("|", usuarioRolInfo.OfType<CruceUsuarioRol>().ToList().Select(x => x.IdFacultad)) + "|";
                        Session.Add("Facultades", facultades);
                        Session.Add("idUsuario", userSessionId);
                        Session.Add("nombreUser", currentUserData.FullName);
                        Session.Add("EmpleadoId", currentUserData.EmployeeNumber);
                    }
                    else
                    {
                        Session.Add("Facultades", facultades);
                    }

                    //Mandar a pagina inicial
                    if (Session["Facultades"].ToString() == "")
                    {
                        this.SaveLog(
                            String.Format("Inicio de sessión fallida. Login: {0}. Sin facultades", this.txtUsuario.Text), 
                            BitacoraEventoTipoEnum.Sesion_InicioFallido, 
                            BitacoraTipoEstatusEnum.NotSuccessful, 
                            string.Empty
                        );
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Usuario sin acceso pongase en contacto con el Administrador de la Aplicación.");
                    }
                    else
                    {
                        this.SaveLog(
                            String.Format("Inicio de sessión. Login: {0}.", this.txtUsuario.Text),
                            BitacoraEventoTipoEnum.Sesion_Inicio,
                            BitacoraTipoEstatusEnum.Successful, 
                            Session["nombreUser"].ToString(), 
                            Parser.ToNumber(Session["EmpleadoId"])
                        );
                        this.SetSessionVariablesValues();
                        Response.Redirect("~/Inicio.aspx", false);
                    }
                } //UsuarioValido
                else
                {
                    this.SaveLog(
                        String.Format("Inicio de sessión fallida. Login: {0}. Usuario sin acceso.", this.txtUsuario.Text), 
                        BitacoraEventoTipoEnum.UsuarioNoRegistrado, 
                        BitacoraTipoEstatusEnum.NotSuccessful, 
                        string.Empty
                    );
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Usuario sin acceso pongase en contacto con el Administrador de la Aplicación.");
                    return;
                } //UsuarioValido
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("1039"))
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Usuario sin acceso pongase en contacto con el Administrador de la Aplicación");
                }
                //string message = ExceptionMessageHelper.GetExceptionMessage(ex);
                //Mensajes.ShowAdvertencia(this.Page, this.GetType(), message);
            }
        }
        else
        {
            Mensajes.ShowMessage(this.Page, this.GetType(), "Debe de ingresar Usuario y Password");
        }
    }

    /// <summary>
    /// Guardando bitácora
    /// </summary>
    /// <param name="message">Mensaje de bitácora</param>
    /// <param name="eventType">Tipo de evento</param>
    /// <param name="eventType">Tipo de bitácora</param>
    /// <returns></returns>
    private bool SaveLog(string message, BitacoraEventoTipoEnum eventType, string logType, string sessionUserFullName, int transactionEmployeeId = 0)
    {
        Bitacora bitacora = new Bitacora
        {
            CreationDate = DateTime.Now,
            Comments = message,
            EmployeeNumber = transactionEmployeeId,
            EventId = (int)eventType,
            LogType = logType,
            //Request = JsonConvert.SerializeObject(userData),
            SessionIP = Session["SessionIP"].ToString(),
            UserLogin = Session["UserLogin"].ToString(),
            UserFullName = sessionUserFullName,
        };
        bool isSuccess = BitacoraRules.AgregarBitacora(bitacora);

        return isSuccess;
    }

    /// <summary>
    /// Seteando los valores de todas las variables de sesión en una sola clase
    /// </summary>
    private void SetSessionVariablesValues()
    {
        SessionVariables data = new SessionVariables
        {
            Faculties = Session["Facultades"].ToString(),
            SessionIP = Session["SessionIP"].ToString(),
            SessionUserEmployeeId = Session["EmpleadoId"].ToString(),
            SessionUserFullName = Session["nombreUser"].ToString(),
            SessionUserId = Session["idUsuario"].ToString(),
            SessionUserLogin = Session["UserLogin"].ToString(),
        };

        Session.Add("VariablesSesion", data);
    }

}