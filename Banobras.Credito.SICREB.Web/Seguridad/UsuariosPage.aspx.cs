using Banobras.Credito.SICREB.Business.DTOs;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Business.Seguridad;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Common.ExceptionHelpers;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Seguridad_UsuariosPage : System.Web.UI.Page
{


    #region Propiedades locales ...

    /// <summary>
    /// Lista de Usuarios
    /// </summary>
    private List<Usuario> userList = new List<Usuario>();

    /// <summary>
    /// Identificador de usuario de la sesión
    /// </summary>
    private int _sessionUserId = 0;

    /// <summary>
    /// Login de usuario de la sesión
    /// </summary>
    private string _sessionUserLogin = string.Empty;

    /// <summary>
    /// Dirección de correo de usuario
    /// </summary>
    private string _userSectionEmail = string.Empty;

    /// <summary>
    /// Nombre de completo de usuario
    /// </summary>
    private string _userSectionFullName = string.Empty;

    /// <summary>
    /// Número de empleado en nómina o directorio activo
    /// </summary>
    private string _userSectionEmployeeId = string.Empty;

    /// <summary>
    /// Login de usuario
    /// </summary>
    private string _userSectionLogin = string.Empty;

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                var sessionData = this.GetSessionVariablesValues();
                if (Session["Facultades"] != null)
                {
                    //this.GetFacultades();
                    this.LoadInitData();
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
                }

                string sessionUserId = Session["idUsuario"].ToString().Trim();
                this._sessionUserId = !string.IsNullOrEmpty(sessionUserId) ? Parser.ToNumber(sessionUserId) : 0;
                ActividadRules.GuardarActividad(4444, Parser.ToNumber(sessionUserId), "Ingreso al catalogo de Usuarios");
                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = "Ingreso al catálogo de usuarios.";
                bitacora.EventId = (int)BitacoraEventoTipoEnum.UsuarioSesionIniciada;
                bitacora.LogType = BitacoraTipoEstatusEnum.Successful;
                BitacoraRules.AgregarBitacora(bitacora);

                this.Section_User.Visible = false;
                //CambiaAtributosRGR();

                //Usuarios
                this.userList = this.GetUsers();
            }            
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
            //Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    /// <summary>
    /// Habilitar sección para agregar Usuario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        this.ShowControlsOnOff(true);
        this.imgbSearchDA.Visible = true;
        this.txtUserLogin.Enabled = true;
        this.txtEmpleadoId.Enabled = true;
        this.txtName.Enabled = true;
        this.txtEmail.Enabled = true;

        try
        {
            //Cargando catálogo de Roles, sólo los que estén activos
            RolRules rr = new RolRules();
            listRolesDisponibles.DataSource = rr.Roles(true);
            listRolesDisponibles.DataValueField = "Id";
            listRolesDisponibles.DataTextField = "Descripcion";
            listRolesDisponibles.DataBind();
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
            //Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    /// <summary>
    /// Exportar datos a PDF en ruta local
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        string title = "ADMINISTRACIÓN DE USUARIOS";
        string[] nombres = new string[] { title };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {
            this.RgdUsuarios.MasterTableView.AllowPaging = false;
            this.RgdUsuarios.Rebind();

            listaGrids.Add(this.RgdUsuarios);
            string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, title, WebConfig.Site);
            this.RgdUsuarios.MasterTableView.AllowPaging = true;
            this.RgdUsuarios.Rebind();
            ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);

            //Guardando bitácora
            Bitacora bitacora = this.GetBitacoraTemplate();
            bitacora.Comments = "Catálogo de usuarios exportado en PDF.";
            bitacora.EventId = (int)BitacoraEventoTipoEnum.UsuarioSesionIniciada;
            bitacora.LogType = BitacoraTipoEstatusEnum.Successful;
            BitacoraRules.AgregarBitacora(bitacora);
        }
        catch (Exception ex)
        {
            //Mensajes.ShowError(this.Page, this.GetType(), ex);
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Exportar datos a Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.RgdUsuarios.MasterTableView.Columns.FindByUniqueName("Acciones").Visible = false;
            this.RgdUsuarios.ExportSettings.ExportOnlyData = true;
            this.RgdUsuarios.MasterTableView.ExportToExcel();

            //Guardando bitácora
            Bitacora bitacora = this.GetBitacoraTemplate();
            bitacora.Comments = "Catálogo de usuarios exportado a Excel.";
            bitacora.EventId = (int)BitacoraEventoTipoEnum.UsuarioSesionIniciada;
            bitacora.LogType = BitacoraTipoEstatusEnum.Successful;
            BitacoraRules.AgregarBitacora(bitacora);
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Buscar información del login en Directorio Activo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearchDA_Click(object sender, EventArgs e)
    {
        this.btnSaveUser.Enabled = false;
        this._userSectionLogin = txtUserLogin.Text.Trim();
        if (string.IsNullOrEmpty(this._userSectionLogin))
        {
            Mensajes.ShowMessage(Page, this.GetType(), "No ha ingresado un login para el usuario.");
            return;
        }

        try
        {
            //Buscando usuario localmente
            this.userList = this.GetUsers();
            Usuario userData = this.userList.ToList().FirstOrDefault(x => x.Login.ToLower() == this._userSectionLogin.ToLower());
            if (userData != null && userData.Id > 0)
            {
                Mensajes.ShowMessage(Page, this.GetType(), "El login capturado ya está asociado a un usuario en SICREB.");
                return;            
            }

            //Consultando información en el directorio activo
            var userDA = ActiveDir.GetUserDataByUserName(this._userSectionLogin.ToLower());
            if (userDA != null && !string.IsNullOrEmpty(userDA.SamAccountName))
            {
                this.txtEmail.Text = userDA.Mail;
                this.txtEmpleadoId.Text = userDA.Initials;
                this.txtName.Text = userDA.DisplayName;
                //this.txtUserId.Text = userDA.Initials;
                this.btnSaveUser.Enabled = true;
            }
            else
            {
                Mensajes.ShowMessage(Page, this.GetType(), "El usuario no existe en el directorio activo");
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }


    #region Eventos del Grid ...
    
    /// <summary>
    /// Al bindear los datos a la cuadrícula, mostrar u ocultar botones
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RgdUsuarios_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            try
            {
                int indexColumnButtons = 13;
                ImageButton btnDelete = (ImageButton)item.Controls[indexColumnButtons].Controls[1];
                ImageButton btnActivate = (ImageButton)item.Controls[indexColumnButtons].Controls[3];
                ImageButton btnEdit = (ImageButton)item.Controls[indexColumnButtons].Controls[5];
                ImageButton btnSession = (ImageButton)item.Controls[indexColumnButtons].Controls[7];

                UsuarioRules objUsuario = new UsuarioRules();

                //Evaluando el estatus para visualizar botones
                switch (Util.SetEstado(item["Estatus"].Text))
                {
                    //NO activo
                    case "0":
                        //Pintar de rojo la celda
                        item["Estatus"].Style.Add("Color", "White");
                        item["Estatus"].Style.Add("BackGround-Color", "Red");

                        //Habilitar la activación
                        if (Session["Facultades"].ToString().Contains(objUsuario.GetVariable("ELIM_USUARIO")))
                        {
                            btnActivate.Visible = true;
                        }
                        btnDelete.Visible = false;

                        //Un usuario no activo no puede ser modificado
                        btnEdit.Visible = false;
                        break;

                    //Activo
                    case "1":
                        //Pintar de verde la celda
                        item["Estatus"].Style.Add("Color", "White");
                        item["Estatus"].Style.Add("BackGround-Color", "LightGreen");

                        //Habilitar la eliminación
                        btnActivate.Visible = false;
                        if (Session["Facultades"].ToString().Contains(objUsuario.GetVariable("ELIM_USUARIO")))
                        {
                            btnDelete.Visible = true;
                        }

                        if (Session["Facultades"].ToString().Contains(objUsuario.GetVariable("MOD_USUARIO")))
                        {
                            btnEdit.Visible = true;
                        }
                        break;
                }

                //Evaluando sessión iniciada
                if (string.IsNullOrEmpty(item["SessionIP"].Text.Trim()) || item["SessionIP"].Text.Trim() == "&nbsp;")
                {
                    btnSession.ImageUrl = "../App_Themes/Banobras2011/Grid/cerrar_sesion_off.png";
                    btnSession.Enabled = false;
                    btnSession.ToolTip = "Sesión cerrada.";
                }
                else
                {
                    btnSession.ImageUrl = "../App_Themes/Banobras2011/Grid/cerrar_sesion_on.png";
                    btnSession.Enabled = true;
                    btnSession.ToolTip = "Cerrar sesión activa.";
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
    /// Conectando datos con la cuadrícula
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void RgdUsuarios_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            //UsuarioRolRules urr = new UsuarioRolRules();
            //RgdUsuarios.DataSource = urr.UsuarioRolesCadena().OrderBy(x => x.Usuario);

            //this.userList = JsonConvert.DeserializeObject<List<Usuario>>(Session["CatalogoUsuarios"].ToString());
            //RgdUsuarios.DataSource = this.userList.OrderBy(x => x.Login);

            this.RgdUsuarios.DataSource = this.GetUsersWithRoles().OrderBy(x => x.Login);
            //Volviendo a traducir los filtros
            if (this.RgdUsuarios.FilterMenu.Items[0].Text == "NoFilter")
            {
                this.CambiaAtributosRGR();            
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }
    }


    #endregion


    #region Acciones botones del Grid ...

    /// <summary>
    /// Eliminado lógico de información de usuario y roles asociados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGridDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton Button = (ImageButton)sender;
            GridDataItem item = (GridDataItem)Button.NamingContainer;
            int userId = Parser.ToNumber(item.GetDataKeyValue("Id").ToString());
            this._userSectionLogin = item.GetDataKeyValue("Login").ToString();
            this._userSectionFullName = item["FullName"].Text;

            //Eliminado lógico de usuario en T_Usuarios
            this.userList = JsonConvert.DeserializeObject<List<Usuario>>(Session["CatalogoUsuarios"].ToString());
            Usuario currentUserData = this.userList.ToList().FirstOrDefault(x => x.Id == userId);
            currentUserData.Estatus = Enums.Estado.Inactivo;
            currentUserData.TransactionDate = DateTime.Now;
            currentUserData.TransactionLogin = Session["UserLogin"].ToString();
            UsuarioEntidadRules uer = new UsuarioEntidadRules();
            int userDeleted = uer.EliminarUsuario(currentUserData);

            //Eliminado lógico de roles asociados a un usuario en T_Usuario_Rol
            var usuarioRolData = new UsuarioRol(0, 0, userId, Enums.Estado.Inactivo);
            UsuarioRolRules urr = new UsuarioRolRules();
            int userRolDeleted = urr.EliminarUsuarioRol(usuarioRolData);
            bool success = userDeleted > 0 && userRolDeleted > 0;
            if (success)
            {
                this._sessionUserId = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(87, this._sessionUserId, "Usuario: " + this._userSectionLogin);
                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = String.Format("Se ha eliminado información del usuario. Login: {0}, Nombre: {1}.", this._userSectionLogin, this._userSectionFullName);
                bitacora.EventId = (int)BitacoraEventoTipoEnum.Usuario_Baja;
                bitacora.Request = JsonConvert.SerializeObject(currentUserData) + "   " + JsonConvert.SerializeObject(usuarioRolData);
                BitacoraRules.AgregarBitacora(bitacora);

                Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos del usuario han sido eliminados satisfactoriamente.");
                this.ReloadDataGrid();
            }
            else {
                Mensajes.ShowMessage(this.Page, this.GetType(), "No fué posible eliminar la información del usuario.");
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }
    
    /// <summary>
    /// Activando información de usuario y roles asociados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGridActivate_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton Button = (ImageButton)sender;
            GridDataItem item = (GridDataItem)Button.NamingContainer;
            int userId = Parser.ToNumber(item.GetDataKeyValue("Id").ToString());
            this._userSectionLogin = item.GetDataKeyValue("Login").ToString();
            this._userSectionFullName = item.Cells[5].Text;

            //Activado lógico de usuario en T_Usuarios
            this.userList = JsonConvert.DeserializeObject<List<Usuario>>(Session["CatalogoUsuarios"].ToString());
            Usuario currentUserData = this.userList.ToList().FirstOrDefault(x => x.Id == userId);
            currentUserData.Estatus = Enums.Estado.Activo;
            currentUserData.TransactionDate = DateTime.Now;
            currentUserData.TransactionLogin = Session["UserLogin"].ToString();

            UsuarioEntidadRules uer = new UsuarioEntidadRules();
            int userUpdated = uer.ActualizarUsuario(new Usuario(), currentUserData);

            //Buscando los roles asociados al usuario
            UsuarioRolRules objUsuarioRol = new UsuarioRolRules();
            var rolesList = objUsuarioRol.UsuariosRol(false);
            var rolesAssociatedList = rolesList.Where(x => x.IdUsuario == userId).ToList();

            //Activado lógico de roles asociados a un usuario en T_Usuario_Rol
            int userRolUpdated = 0;
            List<object> usuarioRolesList = new List<object>();
            if (rolesAssociatedList.Any())
            {
                foreach (var rol in rolesAssociatedList)
                {
                    var usuarioRolData = new UsuarioRol(rol);
                    usuarioRolData.Estatus = Enums.Estado.Activo;
                    userRolUpdated += objUsuarioRol.ActualizarUsuarioRol(new UsuarioRol(), usuarioRolData);
                    usuarioRolesList.Add(new { RolId = rol.IdRol, Rol = "" });
                }
            }
            
            bool success = userUpdated > 0 && userRolUpdated > 0;
            if (success)
            {
                this._sessionUserId = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(86, this._sessionUserId, "Usuario: " + this._userSectionLogin);
                ActividadRules.GuardarActividad(89, this._sessionUserId, "Usuario: " + this._userSectionLogin);
                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = String.Format("Se ha activado información del usuario. Login: {0}, Nombre: {1}.", this._userSectionLogin, this._userSectionFullName);
                bitacora.EventId = (int)BitacoraEventoTipoEnum.Usuario_Reactivacion;
                bitacora.Request = JsonConvert.SerializeObject(currentUserData) + " | " + JsonConvert.SerializeObject(usuarioRolesList);
                BitacoraRules.AgregarBitacora(bitacora);

                Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos del usuario se han activado satisfactoriamente.");
            }
            else
            {
                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = String.Format("No fué posible activar la información del usuario. Login: {0}, Nombre: {1}.", this._userSectionLogin, this._userSectionFullName);
                bitacora.EventId = (int)BitacoraEventoTipoEnum.Usuario_Reactivacion;
                bitacora.LogType = BitacoraTipoEstatusEnum.NotSuccessful;
                bitacora.Request = JsonConvert.SerializeObject(currentUserData) + " | " + JsonConvert.SerializeObject(usuarioRolesList);
                BitacoraRules.AgregarBitacora(bitacora);

                Mensajes.ShowMessage(this.Page, this.GetType(), "No fué posible activar la información del usuario.");
            }
            this.ReloadDataGrid();
        }
        catch (Exception ex)
        {
            //Guardando bitácora
            Bitacora bitacora = this.GetBitacoraTemplate();
            bitacora.Comments = String.Format("No fué posible activar la información del usuario. Login: {0}, Nombre: {1}.", this._userSectionLogin, this._userSectionFullName);
            bitacora.EventId = (int)BitacoraEventoTipoEnum.Usuario_Reactivacion;
            BitacoraRules.AgregarBitacora(bitacora);

            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Editar información de usuario y roles asociados
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGridEdit_Click(object sender, EventArgs e)
    {
        try
        {
            this.ShowControlsOnOff(true);
            this.imgbSearchDA.Visible = false;

            ImageButton Button = (ImageButton)sender;
            GridDataItem item = (GridDataItem)Button.NamingContainer;
            int userSectionId = Parser.ToNumber(item["Id"].Text);

            this.txtUserLogin.Text = item["Login"].Text;
            this.txtUserLogin.Enabled = false;
            this.txtUserId.Text = userSectionId.ToString();

            this.txtEmpleadoId.Text = item["EmployeeNumber"].Text;
            this.txtEmpleadoId.Enabled = false;
            this.txtName.Text = item["FullName"].Text;
            this.txtName.Enabled = false;
            this.txtEmail.Text = item["Email"].Text;
            this.txtEmail.Enabled = false;

            //Si el estatus es Inactivo, anular operaciones con roles
            if (item["Estatus"].Text.Trim() == "Inactivo")
            {
                this.btnAddRole.Visible = false;
                this.btnRemoveRole.Visible = false;
            }
            else
            {
                this.btnAddRole.Visible = true;
                this.btnRemoveRole.Visible = true;
            }
            this.btnSaveUser.Enabled = true;

            //Cargando información a la lista de roles no asociados
            RolRules rr = new RolRules();
            List<Rol> roles = rr.Roles(true);
            this.listRolesDisponibles.DataSource = roles;
            this.listRolesDisponibles.DataValueField = "Id";
            this.listRolesDisponibles.DataTextField = "Descripcion";
            this.listRolesDisponibles.DataBind();

            //Obteniendo el catálogo de roles asociados a usuarios
            UsuarioRolRules urr = new UsuarioRolRules();
            List<UsuarioRol> usuarioRol = urr.UsuariosRol(false);

            //Obteniendo únicamente los roles asociados el usuario
            var query = from ur in usuarioRol
                        where ur.IdUsuario == userSectionId
                        select new { ur.IdRol, usuario = "", ur.IdUsuario, ur.Id, ur.Estatus };

            List<UsuarioRolCadena> frc = (from q in query
                                          join rol in roles on q.IdRol equals rol.Id
                                          select new UsuarioRolCadena
                                          {
                                              idRol = q.IdRol,
                                              Rol = rol.Descripcion,
                                              idUsuario = q.IdUsuario,
                                              id = q.Id,
                                              estatus = q.Estatus,
                                              Usuario = txtUserLogin.Text
                                          }).ToList<UsuarioRolCadena>();

            //Cargando la lista de roles asociados
            this.listRolesAsignados.DataSource = frc;
            this.listRolesAsignados.DataValueField = "idRol";
            this.listRolesAsignados.DataTextField = "Rol";
            this.listRolesAsignados.DataBind();

            //Removiendo de los roles disponibles, los asociados
            for (int i = 0; i < listRolesAsignados.Items.Count; i++)
            {
                this.listRolesDisponibles.Items.Remove(this.listRolesAsignados.Items[i]);
            }

            //e.Canceled = true;
            this.RgdUsuarios.MasterTableView.ClearEditItems();
            
            //Guardando bitácora
            Bitacora bitacora = this.GetBitacoraTemplate();
            bitacora.Comments = string.Format("Se consultó la información del usuario {0}.", this.txtUserLogin.Text);
            bitacora.EventId = (int)BitacoraEventoTipoEnum.UsuarioSesionIniciada;
            bitacora.LogType = BitacoraTipoEstatusEnum.Successful;
            BitacoraRules.AgregarBitacora(bitacora);
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Cerrando sessión
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGridSession_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton Button = (ImageButton)sender;
            GridDataItem item = (GridDataItem)Button.NamingContainer;
            int userId = Parser.ToNumber(item.GetDataKeyValue("Id").ToString());
            this._userSectionLogin = item.GetDataKeyValue("Login").ToString();
            this._userSectionFullName = item["FullName"].Text;

            //Ubicando usuario
            this.userList = JsonConvert.DeserializeObject<List<Usuario>>(Session["CatalogoUsuarios"].ToString());
            Usuario currentUserData = this.userList.ToList().FirstOrDefault(x => x.Id == userId);
            currentUserData.SessionIP = " ";
            currentUserData.TransactionDate = DateTime.Now;
            currentUserData.TransactionLogin = Session["UserLogin"].ToString();

            //Eliminando sesión de usuario en T_Usuarios
            UsuarioEntidadRules uer = new UsuarioEntidadRules();
            int userUpdated = uer.ActualizarUsuario(new Usuario(), currentUserData);
            if (userUpdated > 0)
            {
                this._sessionUserId = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(86, this._sessionUserId, "Usuario: " + this._userSectionLogin);

                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = String.Format("Se ha cerrado la sesión del usuario. Login: {0}, Nombre: {1}.", this._userSectionLogin, this._userSectionFullName);
                bitacora.EventId = (int)BitacoraEventoTipoEnum.Usuario_CerrarSesion;
                bitacora.Request = JsonConvert.SerializeObject(currentUserData);
                BitacoraRules.AgregarBitacora(bitacora);

                Mensajes.ShowMessage(this.Page, this.GetType(), "La sesión del usuario se ha cerrado satisfactoriamente.");
                this.ReloadDataGrid();
            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "No fué posible cerrar la sesión del usuario.");
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    #endregion
    

    #region Acciones botones sección Usuario ...

    /// <summary>
    /// Guardar información de usuario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveUser_Click(object sender, EventArgs e)    
    {
        this._userSectionLogin = this.txtUserLogin.Text.Trim().ToLower();
        this._userSectionFullName = this.txtName.Text.Trim();
        this._userSectionEmail = this.txtEmail.Text.Trim();
        this._userSectionEmployeeId = this.txtEmpleadoId.Text.Trim();
        this._sessionUserId = Parser.ToNumber(Session["idUsuario"].ToString());
        this._sessionUserLogin = Session["UserLogin"].ToString();

        if (string.IsNullOrEmpty(this._userSectionLogin) || listRolesAsignados.Items.Count == 0)
        {
            Mensajes.ShowMessage(Page, this.GetType(), "Es necesario ingresar un login y asignar un rol.");
            return;
        }

        try
        {
            int userId = !string.IsNullOrEmpty(this.txtUserId.Text.Trim()) ? Parser.ToNumber(this.txtUserId.Text.Trim()) : 0;
            this.userList = this.GetUsers();
            Usuario transactionUserData = this.userList.ToList().FirstOrDefault(x => x.Id == this._sessionUserId);
            Usuario currentUserData = this.userList.ToList().FirstOrDefault(x => x.Login == _userSectionLogin);
            List<string> rolesTextList = new List<string>();
            UsuarioRolRules objUsuarioRol = new UsuarioRolRules();

            switch (userId)
            {
                //Usuario nuevo
                case 0:
                    //Verificar primero que no sea un usuario registrado en SICREB
                    if (currentUserData != null)
                    {
                        Mensajes.ShowMessage(Page, this.GetType(), "El usuario ya existe en SICREB.");
                        return;
                    }

                    //Consultando información en el directorio activo
                    var userDA = ActiveDir.GetUserDataByUserName(this._userSectionLogin);
                    if (userDA != null)
                    {                        
                        //Guardando información de usuario
                        Usuario newUserData = new Usuario { 
                            CreationDate = DateTime.Now,
                            Email = this._userSectionEmail,
                            EmployeeNumber = !string.IsNullOrEmpty(this._userSectionEmployeeId) ? Parser.ToNumber(this._userSectionEmployeeId) : 0, 
                            Estatus = Enums.Estado.Activo,
                            FullName = this._userSectionFullName,
                            //Id = 0,
                            Login = this._userSectionLogin, 
                            //SessionDate = DateTime.MinValue,
                            //SessionIP = null,
                            TransactionDate = DateTime.Now,
                            TransactionLogin = transactionUserData.Login
                        };

                        UsuarioEntidadRules ur = new UsuarioEntidadRules();
                        int createdUserId = ur.InsertarUsuario(newUserData);

                        //Guardando información de roles asociados
                        foreach (ListItem li in listRolesAsignados.Items)
                        {
                            objUsuarioRol.InsertarUsuarioRol(new UsuarioRol(0, Parser.ToNumber(li.Value), createdUserId, Enums.Estado.Activo));
                            rolesTextList.Add(li.Text); //Lista temporal para luego convertirla en cadena a guardar en bitácora
                        }

                        //Guardando bitácora
                        bool isSuccessActivity = ActividadRules.GuardarActividad(85, this._sessionUserId, "Usuario creado: " + createdUserId);

                        //Guardando bitácora
                        Bitacora bitacora = this.GetBitacoraTemplate();
                        bitacora.Comments = String.Format("Se ha creado un nuevo usuario. Login: {0}, Nombre: {1}, Rol: {2}. Creado por {3}.", this._userSectionLogin, this._userSectionFullName, string.Join(",", rolesTextList), this._sessionUserLogin);
                        bitacora.EventId = (int)BitacoraEventoTipoEnum.Usuario_Alta;
                        bitacora.Request = JsonConvert.SerializeObject(newUserData);
                        BitacoraRules.AgregarBitacora(bitacora);

                        this.ShowControlsOnOff(false);
                        this.ReloadDataGrid();
                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han guardado de forma correcta");
                    }
                    else
                    {
                        Mensajes.ShowMessage(Page, this.GetType(), "El usuario no existe en el directorio activo");
                    }
                    break;
                //Usuario existente
                default:
                    Usuario updateUserData = new Usuario();
                    if (currentUserData.Id > 0)
                    {
                        updateUserData = new Usuario(currentUserData);
                        //updateUserData.CreationDate = currentUserData.CreationDate != DateTime.MinValue ? currentUserData.CreationDate : DateTime.Now;
                        updateUserData.Email = this._userSectionEmail;
                        updateUserData.EmployeeNumber = !string.IsNullOrEmpty(this._userSectionEmployeeId) ? Convert.ToInt32(this._userSectionEmployeeId) : 0;
                        ////updateUserData.Estatus = currentUserData.Estatus;
                        updateUserData.FullName = this._userSectionFullName;
                        updateUserData.Login = currentUserData.Login;
                        //updateUserData.SessionDate = currentUserData.SessionDate;
                        //updateUserData.SessionIP = Session["SessionIP"].ToString();
                        updateUserData.TransactionDate = DateTime.Now;
                        updateUserData.TransactionLogin = transactionUserData.Login;

                        //Actualizando información de Usuarios
                        UsuarioEntidadRules uer = new UsuarioEntidadRules();
                        int actualizado = uer.ActualizarUsuario(currentUserData, updateUserData);

                        //Determinando la acción a partir del estatus del usuario
                        switch (updateUserData.Estatus)
                        {
                            case Enums.Estado.Activo:
                                //Obteniendo el catálogo de roles asociads a los usuarios
                                List<UsuarioRol> usuarioRolesList = objUsuarioRol.UsuariosRol(false);

                                //Obteniendo los roles originalmente asociados al usuario
                                var originalAssociatedList = usuarioRolesList.Where(x => x.IdUsuario == userId).Select(y => y.IdRol).ToList();

                                //Obteniendo los roles actualmente asociados, es decir, las de la lista de asociados
                                var currentAssociatedList = this.listRolesAsignados.Items.Cast<ListItem>().Select(x => Parser.ToNumber(x.Value)).ToList();

                                //Removiendo de los roles originales, los que no estén en la lista actual
                                if (originalAssociatedList.Count > currentAssociatedList.Count)
                                {
                                    var notFoundList = originalAssociatedList.Where(x => !currentAssociatedList.Contains(x)).ToList();
                                    if (notFoundList.Any())
                                    {
                                        foreach (var item in notFoundList)
                                        {
                                            UsuarioRol usuarioRolToUpdate = usuarioRolesList.Find(x => x.IdUsuario == userId && x.IdRol == Parser.ToNumber(item));
                                            usuarioRolToUpdate.Estatus = Enums.Estado.Inactivo;
                                            objUsuarioRol.ActualizarUsuarioRol(new UsuarioRol(), usuarioRolToUpdate);
                                        }
                                    }
                                }

                                //De los roles de la lista de asociados al usuario existente, insertar únicamente los que previamente no lo estaban
                                if (currentAssociatedList.Any())
                                {
                                    foreach (var item in currentAssociatedList)
                                    {
                                        UsuarioRol usuarioRolData = usuarioRolesList.Find(x => x.IdRol == Parser.ToNumber(item) && x.IdUsuario == userId);
                                        //Verificar si la facultad ya está asociada;
                                        if (usuarioRolData != null)
                                        {
                                            //Si está inactivo, activarlo
                                            if (usuarioRolData.Estatus == Enums.Estado.Inactivo)
                                            {
                                                UsuarioRol facultadToUpdate = new UsuarioRol(usuarioRolData); // usuarioRolesList.Find(x => x.IdUsuario == userId && x.IdRol == Parser.ToNumber(item));
                                                facultadToUpdate.Estatus = Enums.Estado.Activo;
                                                objUsuarioRol.ActualizarUsuarioRol(new UsuarioRol(), facultadToUpdate);
                                            }
                                        }
                                        else
                                        {
                                            //No está asocida, insertarla
                                            objUsuarioRol.InsertarUsuarioRol(new UsuarioRol(0, Parser.ToNumber(item), userId, Enums.Estado.Activo));
                                        }
                                        rolesTextList.Add(item.ToString()); //Lista temporal para luego convertirla en cadena a guardar en bitácora
                                    }
                                }
                                break;
                            case Enums.Estado.Inactivo: //No se podrán agregar o modificar roles asociados
                                break;
                        }

                        //Guardando actividad
                        ActividadRules.GuardarActividad(86, this._sessionUserId, "Usuario actualizado: " + userId);

                        //Guardando bitácora
                        Bitacora bitacora = this.GetBitacoraTemplate();
                        bitacora.Comments = String.Format("Se ha modificado información del usuario. Login: {0}, Nombre: {1}, Rol: {2}. Actualizado por {3}.", this._userSectionLogin, this._userSectionFullName, string.Join(",", rolesTextList), this._sessionUserLogin);
                        bitacora.EventId = (int)BitacoraEventoTipoEnum.Usuario_Actualizacion;
                        bitacora.Request = JsonConvert.SerializeObject(updateUserData);
                        BitacoraRules.AgregarBitacora(bitacora);
                                                
                        this.ShowControlsOnOff(false);
                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han modificado de forma correcta");
                        this.ReloadDataGrid();
                    }
                    else
                    {
                        Mensajes.ShowMessage(Page, this.GetType(), "No se encontró información de usuario para modificar.");
                    }                    
                    break;
            }

            RgdUsuarios.MasterTableView.Rebind();
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
            //Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    /// <summary>
    /// Cancelando edición de registro de usuario.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelUser_Click(object sender, EventArgs e)
    {
        this.txtEmail.Text = string.Empty;
        this.txtEmpleadoId.Text = string.Empty;
        this.txtName.Text = string.Empty;
        this.txtUserId.Text = string.Empty;
        this.txtUserLogin.Text = string.Empty;
        this.ShowControlsOnOff(false);
    }

    /// <summary>
    /// Asociar rol a un usuario
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddRole_Click(object sender, EventArgs e)
    {
        //Agregar el rol disponible seleccionado a la lista de asignados
        for (int i = 0; i < listRolesDisponibles.Items.Count; i++)
        {
            if (listRolesDisponibles.Items[i].Selected)
            {
                listRolesAsignados.Items.Add(listRolesDisponibles.Items[i]);
            }
        }
        //Quitando el rol seleccionado de la lista de disponibles
        for (int i = 0; i < listRolesDisponibles.Items.Count; i++)
        {
            if (listRolesDisponibles.Items[i].Selected)
            {
                listRolesDisponibles.Items.Remove(listRolesDisponibles.Items[i]);
                i = -1;
            }
        }
    }

    /// <summary>
    /// Desasociar rol a un usuario.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRemoveRole_Click(object sender, EventArgs e)
    {
        //Regresar el rol asignado a la lista de roles disponibles
        for (int i = 0; i < listRolesAsignados.Items.Count; i++)
        {
            if (listRolesAsignados.Items[i].Selected)
            {
                listRolesDisponibles.Items.Add(listRolesAsignados.Items[i]);
            }
        }
        //Quitar el rol seleccionado de la lista de asignados
        for (int i = 0; i < listRolesAsignados.Items.Count; i++)
        {
            if (listRolesAsignados.Items[i].Selected)
            {
                listRolesAsignados.Items.Remove(listRolesAsignados.Items[i]);
                i = -1;
            }
        }
    }
    

    #endregion



    #region Métodos ...


    /// <summary>
    /// Obtener la información inicial para guardar la bitácora.
    /// Los campos faltantes a definir son: Comments, EventId, Request
    /// </summary>
    /// <returns></returns>
    private Bitacora GetBitacoraTemplate()
    {
        int transactionEmployeeId = !string.IsNullOrEmpty(Session["EmpleadoId"].ToString().Trim()) ? Parser.ToNumber(Session["EmpleadoId"].ToString()) : 0;
        Bitacora bitacora = new Bitacora
        {
            CreationDate = DateTime.Now,
            //Comments = String.Format("Se ha eliminado información del rol. Identificador: {0}, Descripción: {1}.", currentRolData.Id, currentRolData.Descripcion),
            EmployeeNumber = transactionEmployeeId,
            //EventId = (int)BitacoraEventoTipoEnum.Rol_Baja,
            LogType = BitacoraTipoEstatusEnum.AccountManagement,
            //Request = JsonConvert.SerializeObject(rolToRemoveData) + "   " + JsonConvert.SerializeObject(facultadRolesRemoveData),
            SessionIP = Session["SessionIP"].ToString(),
            UserLogin = Session["UserLogin"].ToString(),
            UserFullName = Session["nombreUser"].ToString()
        };

        return bitacora;
    }

    private SessionVariables GetSessionVariablesValues()
    {
        var data = (SessionVariables)Session["VariablesSesion"];
        return data;
    }

    /// <summary>
    /// Traduciendo texto de los filtros
    /// </summary>
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdUsuarios.FilterMenu;
        for (int i = menu.Items.Count - 1; i >= 0; i--)
        {
            switch (menu.Items[i].Text)
            {
                case "NoFilter":
                    menu.Items[i].Text = "Sin Filtro";
                    break;
                case "EqualTo":
                    menu.Items[i].Text = "Igual";
                    break;
                case "NotEqualTo":
                    menu.Items[i].Text = "Diferente";
                    break;
                case "GreaterThan":
                    menu.Items[i].Text = "Mayor que";
                    break;
                case "LessThan":
                    menu.Items[i].Text = "Menor que";
                    break;
                case "GreaterThanOrEqualTo":
                    menu.Items[i].Text = "Mayor o igual a";
                    break;
                case "LessThanOrEqualTo":
                    menu.Items[i].Text = "Menor o igual a";
                    break;
                case "Between":
                    menu.Items[i].Text = "Entre";
                    break;
                case "NotBetween":
                    menu.Items[i].Text = "No entre";
                    break;
                case "IsNull":
                    menu.Items[i].Text = "Es nulo";
                    break;
                case "NotIsNull":
                    menu.Items[i].Text = "No es nulo";
                    break;
                case "Contains":
                    menu.Items[i].Text = "Contenga";
                    break;
                case "DoesNotContain":
                    menu.Items[i].Text = "No contenga";
                    break;
                case "StartsWith":
                    menu.Items[i].Text = "Inicie con";
                    break;
                case "EndsWith":
                    menu.Items[i].Text = "Finalice con";
                    break;
                default:
                    menu.Items.RemoveAt(i);
                    break;
            }//switch
        }//for

    }

    /// <summary>
    /// Mostrar u ocultar controles
    /// </summary>
    /// <param name="enabled">true | false</param>
    protected void ShowControlsOnOff(bool enabled)
    {
        this.btnAddRole.Enabled = enabled;
        this.btnCancelUser.Enabled = enabled;
        this.btnRemoveRole.Enabled = enabled;
        this.btnSaveUser.Enabled = enabled;
        this.listRolesAsignados.Enabled = enabled;
        this.listRolesAsignados.Items.Clear();
        this.listRolesDisponibles.Enabled = enabled;
        this.listRolesDisponibles.Items.Clear();
        this.txtUserId.Text = "";
        this.txtUserLogin.Enabled = enabled;
        this.txtUserLogin.Text = "";
        this.Section_Filters.Visible = !enabled;
        this.Section_UserList.Visible = !enabled;
        this.Section_User.Visible = enabled;
    }
        
    /// <summary>
    /// Estableciendo visibilidad de botones de acuerdo a facultades/permisos
    /// </summary>
    private void GetFacultades()
    {
        try
        {
            UsuarioRules facultad = new UsuarioRules();
            //this.RgdUsuarios.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
            //this.RgdUsuarios.MasterTableView.GetColumn("DeleteState").Visible = false;
            btnAddUser.Visible = false;
            btnExportPDF.Visible = false;
            btnExportExcel.Visible = false;

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_USUARIO")))
            {
                btnAddUser.Visible = true;
            }

            //if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_USUARIO")))
            //{
            //    this.RgdUsuarios.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
            //}

            //if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_USUARIO")))
            //{
            //    this.RgdUsuarios.MasterTableView.GetColumn("DeleteState").Visible = true;
            //}

            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
            {
                btnExportPDF.Visible = true;
            }

            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
            {
                btnExportExcel.Visible = true;
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), "Verificar su usuario");
        }
    }

    /// <summary>
    /// Obtener lista de usuarios
    /// </summary>
    /// <returns></returns>
    private List<Usuario> GetUsers()
    {
        List<Usuario> userList = new List<Usuario>();
        try
        {
            UsuarioEntidadRules uer = new UsuarioEntidadRules();
            userList = uer.Usuarios(false);
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }

        Session.Add("CatalogoUsuarios", JsonConvert.SerializeObject(userList));
        return userList;
    }

    /// <summary>
    /// Obtener el catálogo de usuarios con roles asociados
    /// </summary>
    /// <returns></returns>
    private List<UsuarioDTO> GetUsersWithRoles()
    {
        List<UsuarioDTO> userRolesList = new List<UsuarioDTO>();

        try
        {
            //Obteniendo el catálogo de usuarios
            UsuarioEntidadRules uer = new UsuarioEntidadRules();
            userList = uer.Usuarios(false);

            //Obteniendo los roles asociados a los usuarios
            UsuarioRolRules urr = new UsuarioRolRules();
            List<UsuarioRol> data = urr.UsuariosRol(false);

            //Obteniendo el catálogo de roles
            RolRules rr = new RolRules();
            List<Rol> rolList = rr.Roles(false);

            //Asociando los roles a cada usuario
            foreach (var item in userList)
            {
                UsuarioDTO row = new UsuarioDTO(item);
                row.Roles = data.Where(x => x.IdUsuario == item.Id).ToList();
                List<string> roles = rolList.Where(x => row.Roles.Select(y => y.IdRol).Contains(x.Id)).Select(z => z.Descripcion).ToList();
                row.RolesNameList = string.Join(", ", roles);
                userRolesList.Add(row);
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }

        return userRolesList;
    }


    private void LoadInitData()
    {
        this.GetFacultades();
        this.CambiaAtributosRGR();       
    }

    /// <summary>
    /// Recargar información del catálogo de usuarios
    /// </summary>
    private void ReloadDataGrid()
    {
        //this.userList = this.GetUsers();
        //Session.Add("CatalogoUsuarios", JsonConvert.SerializeObject(this.userList));
        //RgdUsuarios.DataSource = this.userList.OrderBy(x => x.Login);
        //RgdUsuarios.MasterTableView.Rebind();

        var data = this.GetUsersWithRoles();
        RgdUsuarios.DataSource = data.OrderBy(x => x.Login);
        RgdUsuarios.MasterTableView.Rebind();
    }


    #endregion
    
}
