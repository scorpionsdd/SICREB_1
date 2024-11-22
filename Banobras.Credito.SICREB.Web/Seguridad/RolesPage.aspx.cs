using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Business.Seguridad;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Common.ExceptionHelpers;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class Seguridad_RolesPage : System.Web.UI.Page
{
    int idUs;



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
    /// Nombre completo del usuario de la sesión
    /// </summary>
    private string _sessionUserFullName = string.Empty;

    /// <summary>
    /// Login de usuario de la sesión
    /// </summary>
    private string _sessionUserLogin = string.Empty;

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {
                this.getFacultades();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(4444, idUs, "Ingreso al catalogo de Roles");
                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = "Ingreso al catálogo de roles.";
                bitacora.EventId = (int)BitacoraEventoTipoEnum.UsuarioSesionIniciada;
                bitacora.LogType = BitacoraTipoEstatusEnum.Successful;
                BitacoraRules.AgregarBitacora(bitacora);
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = "Ingreso al catálogo de roles.";
                bitacora.EventId = (int)BitacoraEventoTipoEnum.UsuarioSesionIniciada;
                bitacora.LogType = BitacoraTipoEstatusEnum.NotSuccessful;
                BitacoraRules.AgregarBitacora(bitacora);
            }
            if (!Page.IsPostBack)
            {
                this.CambiaAtributosRGR();
                this.Section_Agregar.Visible = false;
                this.btnRoleAdd.Visible = true;
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }
        
    }

    /// <summary>
    /// Exportar datos a PDF en ruta local
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        string[] nombres = new string[] { this.lblTitle.Text };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {
            this.RgdRoles.MasterTableView.AllowPaging = false;
            this.RgdRoles.Rebind();

            listaGrids.Add(this.RgdRoles);
            string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, this.lblTitle.Text, WebConfig.Site);
            this.RgdRoles.MasterTableView.AllowPaging = true;
            this.RgdRoles.Rebind();
            ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);

            //Guardando bitácora
            Bitacora bitacora = this.GetBitacoraTemplate();
            bitacora.Comments = "Catálogo de roles exportado en PDF.";
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
            RgdRoles.Rebind();
            RgdRoles.MasterTableView.Columns.FindByUniqueName("Facultades").Visible = true;
            RgdRoles.MasterTableView.ExportToExcel();

            //Guardando bitácora
            Bitacora bitacora = this.GetBitacoraTemplate();
            bitacora.Comments = "Catálogo de roles exportado a Excel.";
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
    /// Habilitar sección para agregar Rol
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRoleAdd_Click(object sender, EventArgs e)
    {
        this.ShowControlsOnOff(true);
        FacultadRules objFacultad = new FacultadRules();
        try
        {
            ListFacultadDisponibles.DataSource = objFacultad.Facultades();
            ListFacultadDisponibles.DataValueField = "Id";
            ListFacultadDisponibles.DataTextField = "Descripcion";
            ListFacultadDisponibles.DataBind();
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
    protected void RgdRoles_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            try
            {
                int indexColumnButtons = 10;
                ImageButton btnDelete = (ImageButton)item.Controls[indexColumnButtons].Controls[1];
                ImageButton btnActivate = (ImageButton)item.Controls[indexColumnButtons].Controls[3];
                ImageButton btnEdit = (ImageButton)item.Controls[indexColumnButtons].Controls[5];

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
                        if (Session["Facultades"].ToString().Contains(objUsuario.GetVariable("ELIM_ROL")))
                        {
                            btnActivate.Visible = true;
                        }
                        btnDelete.Visible = false;

                        //Un rol no activo no puede ser modificado
                        btnEdit.Visible = false;
                        break;

                    //Activo
                    case "1":
                        //Pintar de verde la celda y colocar visible el botón de edición
                        item["Estatus"].Style.Add("Color", "White");
                        item["Estatus"].Style.Add("BackGround-Color", "LightGreen");

                        //Habilitar la eliminación
                        btnActivate.Visible = false;
                        if (Session["Facultades"].ToString().Contains(objUsuario.GetVariable("ELIM_ROL")))
                        {
                            btnDelete.Visible = true;
                        }

                        if (Session["Facultades"].ToString().Contains(objUsuario.GetVariable("MOD_ROL")))
                        {
                            btnEdit.Visible = true;
                        }
                        break;
                }

                //Limitando el texto de la lista de facultades
                item["Facultades"].Attributes["title"] = item["Facultades"].Text;
                item["Facultades"].Text = item["Facultades"].Text.Length >= 150 ? item["Facultades"].Text.Substring(0, 150) + "..." : item["Facultades"].Text;
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
    protected void RgdRoles_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //var filtros = this.RgdRoles.MasterTableView.FilterExpression;
            var data = this.GetRolesWithFacultades().OrderBy(x => x.Rol);
            this.RgdRoles.DataSource = data;

            //Volviendo a traducir los filtros
            if (this.RgdRoles.FilterMenu.Items[0].Text == "NoFilter")
            {
                this.CambiaAtributosRGR();
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }


    #endregion



    #region Acciones botones del Grid ...

    /// <summary>
    /// Eliminado lógico de información de rol y facultades asociadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGridDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton Button = (ImageButton)sender;
            GridDataItem item = (GridDataItem)Button.NamingContainer;
            int rolId = Parser.ToNumber(item["idrol"].Text);
            int transactionUserId = Parser.ToNumber(Session["idUsuario"].ToString());

            if (rolId.ToString() != ConfigurationManager.AppSettings["IdRolPermanente"].ToString())
            {
                //Buscando el rol seleccionado
                Rol currentRolData = this.GetRoles().FirstOrDefault(x => x.Id == rolId);
                //Eliminando información del Rol
                Rol rolToRemoveData = new Rol(currentRolData);
                rolToRemoveData.Estatus = Enums.Estado.Inactivo;
                RolRules objRoles = new RolRules();
                int rolRemovedId = objRoles.EliminarRol(rolToRemoveData);

                //Eliminando las asociaciones de las facultades con el rol
                FacultadRol facultadRolesRemoveData = new FacultadRol(0, rolId, 0, Enums.Estado.Inactivo);
                FacultadRolRules objFactultadRol = new FacultadRolRules();
                int facultadRolRemovedId = objFactultadRol.EliminarFacultadRol(facultadRolesRemoveData);

                if (rolRemovedId > 0 && facultadRolRemovedId > 0)
                {
                    //Guardando actividad
                    ActividadRules.GuardarActividad(90, transactionUserId, "Rol: " + item["Rol"].Text);
                    //Guardando bitácora
                    Bitacora bitacora = this.GetBitacoraTemplate();
                    bitacora.Comments = String.Format("Se ha eliminado información del rol. Identificador: {0}, Descripción: {1}.", currentRolData.Id, currentRolData.Descripcion);
                    bitacora.EventId = (int)BitacoraEventoTipoEnum.Rol_Baja;
                    bitacora.Request = JsonConvert.SerializeObject(rolToRemoveData) + "   " + JsonConvert.SerializeObject(facultadRolesRemoveData);
                    BitacoraRules.AgregarBitacora(bitacora);

                    Mensajes.ShowMessage(Page, this.GetType(), "Los datos del rol se han eliminado de forma correcta."); 
                    this.ReloadDataGrid();
                }
                else
                {
                    Mensajes.ShowMessage(Page, this.GetType(), "No se eliminaron los datos");
                }
            }
            else
            {
                Mensajes.ShowMessage(Page, this.GetType(), "(Administrador) - Este rol no se puede eliminar");
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Activando información de rol y facultades asociadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGridActivate_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton Button = (ImageButton)sender;
            GridDataItem item = (GridDataItem)Button.NamingContainer;
            int rolId = Parser.ToNumber(item["idrol"].Text);
            int transactionUserId = Parser.ToNumber(Session["idUsuario"].ToString());

            //Buscando el rol seleccionado
            Rol currentRolData = this.GetRoles().FirstOrDefault(x => x.Id == rolId);
            currentRolData.Estatus = Enums.Estado.Activo;
            currentRolData.TransactionDate = DateTime.Now;
            currentRolData.TransactionLogin = Session["UserLogin"].ToString();

            //Actualizando información del rol
            RolRules objRoles = new RolRules();
            int rolUpdatedId = objRoles.ActualizarRol(new Rol(), currentRolData);
            
            //Buscando las facultades asociadas al rol
            FacultadRolRules objFactultadRol = new FacultadRolRules();
            var facultadRolesList = objFactultadRol.FacultadRoles(false);
            var rolesAssociatedList = facultadRolesList.Where(x => x.IdRol == rolId).ToList();

            //Actualizando estatus de facultades asociadas al rol
            List<object> facultadRolList = new List<object>();
            int facultadUpdatedId = 0;
            if (rolesAssociatedList.Any())
            {
                foreach (var rol in rolesAssociatedList)
                {
                    var facultadRolData = new FacultadRol(rol);
                    facultadRolData.Estatus = Enums.Estado.Activo;
                    facultadUpdatedId += objFactultadRol.ActualizarFacultadRol(new FacultadRol(), facultadRolData);
                    facultadRolList.Add(new { FacultadId = rol.IdFacultad, Descripcion = "" });
                }
            }

            if (rolUpdatedId > 0 && facultadUpdatedId > 0)
            {
                //Guardando actividad
                ActividadRules.GuardarActividad(89, transactionUserId, "Rol: " + item["Rol"].Text);
                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = String.Format("Se ha activado información del rol. Identificador: {0}, Descripción: {1}.", currentRolData.Id, currentRolData.Descripcion);
                bitacora.EventId = (int)BitacoraEventoTipoEnum.Rol_Actualizacion;
                bitacora.Request = JsonConvert.SerializeObject(currentRolData) + "   " + JsonConvert.SerializeObject(facultadRolList);
                BitacoraRules.AgregarBitacora(bitacora);

                Mensajes.ShowMessage(Page, this.GetType(), "La información del rol fué reactivada con éxito.");
                this.ReloadDataGrid();
            }
            else
            {
                Mensajes.ShowMessage(Page, this.GetType(), "No se pudo reactivar la información del rol.");
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Editar información de rol y facultades asociadas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGridEdit_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton Button = (ImageButton)sender;
            GridDataItem item = (GridDataItem)Button.NamingContainer;

            this.ShowControlsOnOff(true);

            int id = Parser.ToNumber(item["idrol"].Text);
            this.txtRolDescription.Text = item["rol"].Text;
            this.txtRolDescription.Enabled = false;
            this.txtRolId.Text = id.ToString();

            //Cargando las facultades
            FacultadRules objFacultad = new FacultadRules();
            List<Facultad> facultadesList = objFacultad.Facultades();

            this.ListFacultadDisponibles.DataSource = facultadesList;
            this.ListFacultadDisponibles.DataValueField = "Id";
            this.ListFacultadDisponibles.DataTextField = "Descripcion";
            this.ListFacultadDisponibles.DataBind();

            //Cargando las facultades asociadas a los roles
            FacultadRolRules objFacultadRol = new FacultadRolRules();
            List<FacultadRol> facultadesRolList = objFacultadRol.FacultadRoles();

            var query = from facultadRol in facultadesRolList
                        where facultadRol.IdRol == id
                        select new
                        {
                            facultadRol.IdRol,
                            Rol = "",
                            facultadRol.IdFacultad,
                            facultadRol.Id,
                            facultadRol.Estatus
                        };

            List<FacultadRolCadena> frc = (from q in query
                                           join facultad in facultadesList on q.IdFacultad equals facultad.Id
                                           select new FacultadRolCadena
                                           {
                                               idRol = q.IdRol,
                                               Rol = q.Rol,
                                               idFacultad = q.IdFacultad,
                                               id = q.Id,
                                               estatus = q.Estatus,
                                               Facultades = facultad.Descripcion
                                           }).ToList<FacultadRolCadena>();

            this.ListFacultadAsginadas.DataSource = frc;
            this.ListFacultadAsginadas.DataValueField = "idFacultad";
            this.ListFacultadAsginadas.DataTextField = "Facultades";
            this.ListFacultadAsginadas.DataBind();

            foreach (ListItem li in this.ListFacultadAsginadas.Items)
            {
                this.ListFacultadDisponibles.Items.Remove(li);
            }

            //e.Canceled = true;
            this.RgdRoles.MasterTableView.ClearEditItems();

            //Guardando bitácora
            Bitacora bitacora = this.GetBitacoraTemplate();
            bitacora.Comments = string.Format("Se consultó la información del rol {0}.", this.txtRolDescription.Text);
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


    #endregion



    #region Acciones botones sección Rol ...

    /// <summary>
    /// Asociar facultad a un Rol
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFacultadAdd_Click(object sender, EventArgs e)
    {
        //Agregar la facultad disponible seleccionado a la lista de asignados
        for (int i = 0; i < ListFacultadDisponibles.Items.Count;i++ )
        {
            if (ListFacultadDisponibles.Items[i].Selected)
            {
                ListFacultadAsginadas.Items.Add(ListFacultadDisponibles.Items[i]);
            }
        }
        //Quitando la facultad seleccionada de la lista de disponibles
        for (int i = 0; i < ListFacultadDisponibles.Items.Count; i++)
        {
            if (ListFacultadDisponibles.Items[i].Selected)
            {
                ListFacultadDisponibles.Items.Remove(ListFacultadDisponibles.Items[i]);
                i = -1;
            }
        }
    }

    /// <summary>
    /// Desasociar Facultad a un Rol
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFacultadRemove_Click(object sender, EventArgs e)
    {
        //Regresar la facultad asignada a la lista de roles disponibles
        for (int i = 0;i< ListFacultadAsginadas.Items.Count;i++ )
        {
            if (ListFacultadAsginadas.Items[i].Selected)
            {
                ListFacultadDisponibles.Items.Add(ListFacultadAsginadas.Items[i]);                
            }
        }
        //Quitar la facultad seleccionada de la lista de asignados
        for (int i = 0; i < ListFacultadAsginadas.Items.Count; i++)
        {
            if (ListFacultadAsginadas.Items[i].Selected)
            {
                ListFacultadAsginadas.Items.Remove(ListFacultadAsginadas.Items[i]);
                i=-1;
            }
        }
    }

    /// <summary>
    /// Guardar rol
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRoleSave_Click(object sender, EventArgs e)
    {
        this._sessionUserId = Parser.ToNumber(Session["idUsuario"].ToString());
        this._sessionUserLogin = Session["UserLogin"].ToString();

        if (string.IsNullOrEmpty(this.txtRolDescription.Text.Trim()) || ListFacultadAsginadas.Items.Count == 0)
        {
            Mensajes.ShowMessage(Page, this.GetType(), "Es necesario ingresar una descripción para el rol y seleccionar una facultad, por lo menos.");
            return;
        }

        try
        {
            RolRules objRoles = new RolRules();
            FacultadRolRules objFacultadRol = new FacultadRolRules();
            List<string> facultadesTextList = new List<string>();

            int rolId = !string.IsNullOrEmpty(this.txtRolId.Text.Trim()) ? Parser.ToNumber(this.txtRolId.Text.Trim()) : 0;
            List<Rol> rolesList = this.GetRoles();
            Rol currentRoleData = rolesList.FirstOrDefault(x => x.Descripcion.ToUpper() == this.txtRolDescription.Text.Trim().ToUpper());

            switch (rolId)
            {
                //Rol nuevo
                case 0:
                    //Verificar primero que no sea un rol registrado en SICREB
                    if (currentRoleData != null)
                    {
                        Mensajes.ShowMessage(Page, this.GetType(), "El rol ya existe en SICREB.");
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(txtRolDescription.Text) && ListFacultadAsginadas.Items.Count > 0)
                    {
                        //Guardando información del rol
                        Rol newRolData = new Rol { 
                            CreationDate = DateTime.Now, 
                            Descripcion = this.txtRolDescription.Text.Trim(), 
                            Estatus = Enums.Estado.Activo, 
                            Id = 0,
                            //TransactionDate = DateTime.Now,
                            TransactionLogin = this._sessionUserLogin
                        };
                        int rolInsertedId = objRoles.InsertarRol(newRolData);

                        //Guardando facultades asociadas
                        foreach (ListItem facultad in ListFacultadAsginadas.Items)
                        {
                            objFacultadRol.InsertarFacultadesRol(new FacultadRol(0, rolInsertedId, Parser.ToNumber(facultad.Value), Enums.Estado.Activo));
                            facultadesTextList.Add(facultad.Text); //Lista temporal para luego convertirla en cadena a guardar en bitácora
                        }

                        //Guardando actividad
                        bool isSuccessActivity = ActividadRules.GuardarActividad(88, this._sessionUserId, "Rol: " + this.txtRolDescription.Text);

                        //Guardando bitácora
                        Bitacora bitacora = this.GetBitacoraTemplate();
                        bitacora.Comments = String.Format("Se ha creado un nuevo rol. Descripción: {0}, Facultades: {1}, Creado por {2}.", this.txtRolDescription.Text.Trim(), string.Join(",", facultadesTextList), this._sessionUserLogin);
                        bitacora.EventId = (int)BitacoraEventoTipoEnum.Rol_Alta;
                        bitacora.Request = JsonConvert.SerializeObject(newRolData);
                        BitacoraRules.AgregarBitacora(bitacora);

                        this.ShowControlsOnOff(false);
                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han guardado de forma correcta");
                        this.ReloadDataGrid();
                    }
                    break;
                //Rol existente
                default:
                    Rol updateRolData = new Rol();
                    if (currentRoleData.Id > 0)
                    {
                        updateRolData = new Rol(currentRoleData);
                        updateRolData.Descripcion = this.txtRolDescription.Text.Trim();
                        updateRolData.TransactionDate = DateTime.Now;
                        updateRolData.TransactionLogin = this._sessionUserLogin;

                        //Actualizando información del rol
                        int updateRolId = objRoles.ActualizarRol(new Rol(), updateRolData);

                        //Determinando la acción a partir del estatus del rol
                        switch (updateRolData.Estatus)
                        {
                            case Enums.Estado.Activo:
                                //Obteniendo el catálogo de facultades asociadas a los roles
                                List<FacultadRol> facultadesRolList = objFacultadRol.FacultadRoles(false);

                                //Obteniendo las facultades originalmente asociadas al rol
                                List<int> originalAssociatedList = facultadesRolList.Where(x => x.IdRol == rolId).Select(y => y.IdFacultad).OrderByDescending(o => o).ToList();

                                //Obteniendo las facultades actualmente asociadas, es decir, las de la lista de asociadas
                                var currentAssociatedList = this.ListFacultadAsginadas.Items.Cast<ListItem>().Select(x => new { Id = x.Value, Descripcion = x.Text }).ToList();
                                List<int> currentAssociatedIdList = currentAssociatedList.Select(x => Parser.ToNumber(x.Id)).OrderByDescending(o => o).ToList();

                                //Removiendo de las facultades originales, las que no estén en la lista actual
                                if (originalAssociatedList.Count > currentAssociatedList.Count)
                                {
                                    var notFoundList_TMP = originalAssociatedList.Where(x => !currentAssociatedIdList.Contains(x)).ToList();

                                    var notFoundList = originalAssociatedList.Where(x => !currentAssociatedList.Select(a => Parser.ToNumber(a.Id)).Contains(x)).ToList();
                                    if (notFoundList.Any())
                                    {
                                        foreach (var item in notFoundList)
                                        {
                                            FacultadRol facultadToUpdate = facultadesRolList.Find(x => x.IdRol == rolId && x.IdFacultad == Parser.ToNumber(item) && x.Estatus == Enums.Estado.Activo);
                                            if (facultadToUpdate != null)
                                            {
                                                facultadToUpdate.Estatus = Enums.Estado.Inactivo;
                                                objFacultadRol.ActualizarFacultadRol(new FacultadRol(), facultadToUpdate);

                                                //Obtener el catálogo de facultades
                                                FacultadRules objFaculties = new FacultadRules();
                                                List<Facultad> facultadesList = objFaculties.Facultades(false);

                                                string facultad = facultadesList.FirstOrDefault(x => x.Id == item).Descripcion;
                                                facultadesTextList.Add(facultad + "(-)"); //Lista temporal para luego convertirla en cadena a guardar en bitácora
                                            }
                                        }
                                    }
                                }

                                //De las facultades de la lista de asociadas al rol existente, insertar únicamente las que previamente no lo estaban
                                if (currentAssociatedList.Any())
                                {
                                    foreach (var item in currentAssociatedList)
                                    {
                                        FacultadRol facultad = facultadesRolList.Find(x => x.IdFacultad == Parser.ToNumber(item.Id) && x.IdRol == rolId);
                                        //Verificar si la facultad ya está asociada
                                        if (facultad != null)
                                        {
                                            //Si está inactiva, activarla
                                            if (facultad.Estatus == Enums.Estado.Inactivo)
                                            {
                                                FacultadRol facultadToUpdate = new FacultadRol(facultad);
                                                facultadToUpdate.Estatus = Enums.Estado.Activo;
                                                objFacultadRol.ActualizarFacultadRol(new FacultadRol(), facultadToUpdate);
                                                facultadesTextList.Add(item.Descripcion + "(+)"); //Lista temporal para luego convertirla en cadena a guardar en bitácora
                                            }
                                        }
                                        else 
                                        {
                                            //No está asocida, insertarla
                                            objFacultadRol.InsertarFacultadesRol(new FacultadRol(0, rolId, Parser.ToNumber(item.Id), Enums.Estado.Activo));
                                            facultadesTextList.Add(item.Descripcion + "(+)"); //Lista temporal para luego convertirla en cadena a guardar en bitácora
                                        }
                                    }
                                }
                                break;
                            case Enums.Estado.Inactivo: //No se podrán agregar o modificar facultades asociadas
                                break;
                        }

                        //Guardando bitácoras
                        ActividadRules.GuardarActividad(89, this._sessionUserId, "Rol: " + txtRolDescription.Text);
                        Bitacora bitacora = this.GetBitacoraTemplate();
                        bitacora.Comments = String.Format("Se ha modificado información del rol. Descripción: {0}, Facultades: {1}, Creado por {2}.", this.txtRolDescription.Text.Trim(), string.Join(",", facultadesTextList), this._sessionUserLogin);
                        bitacora.EventId = (int)BitacoraEventoTipoEnum.Rol_Actualizacion;
                        bitacora.Request = JsonConvert.SerializeObject(updateRolData);
                        BitacoraRules.AgregarBitacora(bitacora);

                        this.ShowControlsOnOff(false);
                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han modificado de forma correcta");
                        this.ReloadDataGrid();
                    }
                    else
                    {
                        Mensajes.ShowMessage(Page, this.GetType(), "No se pudo modifcar los datos");
                    }
                    break;
            }

            RgdRoles.MasterTableView.Rebind();
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Cancelar edición de Rol
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRoleCancel_Click(object sender, EventArgs e)
    {
        this.ShowControlsOnOff(false);
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
            LogType = BitacoraTipoEstatusEnum.Permissions,
            //Request = JsonConvert.SerializeObject(rolToRemoveData) + "   " + JsonConvert.SerializeObject(facultadRolesRemoveData),
            SessionIP = Session["SessionIP"].ToString(),
            UserLogin = Session["UserLogin"].ToString(),
            UserFullName = Session["nombreUser"].ToString()
        };

        return bitacora;
    }

    /// <summary>
    /// Obtener el catálogo de roles
    /// </summary>
    /// <returns></returns>
    private List<Rol> GetRoles()
    {
        List<Rol> rolesList = new List<Rol>();

        try
        {
            RolRules objRoles = new RolRules();
            rolesList = objRoles.Roles(false);
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }

        return rolesList;
    }

    /// <summary>
    /// Obtener el catálogo de roles con facultades asociadas
    /// </summary>
    /// <returns></returns>
    private List<FacultadRolCadena> GetRolesWithFacultades()
    {
        List<FacultadRolCadena> facultiesRoleList = new List<FacultadRolCadena>();
        try
        {
            //Obtener el catálogo de roles
            RolRules objRoles = new RolRules();
            List<Rol> rolesList = objRoles.Roles(false);

            //Obtener el catálogo de facultades
            FacultadRules objFaculties = new FacultadRules();
            List<Facultad> facultadesList = objFaculties.Facultades(true);

            //Obtener las facultades asociadas a los roles
            FacultadRolRules objFacultiesRoles = new FacultadRolRules();
            List<FacultadRol> facultadesRolList = objFacultiesRoles.FacultadRoles(true);

            //Agregando las facultades al rol
            foreach (var item in rolesList)
            {
                var facultadesPorRolIds = facultadesRolList.Where(x => x.IdRol == item.Id).Select(y => y.IdFacultad).ToList();
                var facultadesPorRolText = facultadesList.Where(x => facultadesPorRolIds.Contains(x.Id)).ToList();
                FacultadRolCadena nuevo = new FacultadRolCadena { 
                    CreationDate = item.CreationDate, 
                    estatus = item.Estatus,
                    Facultades = string.Join(", ", facultadesPorRolText.Select(x => x.Descripcion).ToList()), 
                    id = 0, 
                    idFacultad = 0, 
                    idRol = item.Id, 
                    Rol = item.Descripcion, 
                    TransactionDate = item.TransactionDate, 
                    TransactionLogin = item.TransactionLogin
                };
                facultiesRoleList.Add(nuevo);
            }
        }
        catch (Exception ex)
        {
            //Mensajes.ShowError(this.Page, this.GetType(), exc); 
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }

        return facultiesRoleList;
    }

    /// <summary>
    /// Mostrar u ocultar controles
    /// </summary>
    /// <param name="enabled"></param>
    private void ShowControlsOnOff(bool enabled)
    {
        this.btnFacultadAdd.Enabled = enabled;
        this.btnFacultadRemove.Enabled = enabled;
        this.btnRoleAdd.Visible = !enabled;
        this.btnRoleCancel.Enabled = enabled;
        this.btnRoleSave.Enabled = enabled;
        this.ListFacultadAsginadas.Items.Clear();
        this.ListFacultadAsginadas.Enabled = enabled;
        this.ListFacultadDisponibles.Items.Clear();
        this.ListFacultadDisponibles.Enabled = enabled;
        this.Section_Agregar.Visible = enabled;
        this.Section_Botones.Visible = !enabled;
        this.Section_Listado.Visible = !enabled;
        this.txtRolDescription.Enabled = enabled;
        this.txtRolDescription.Text = "";
        this.txtRolId.Text = "";
    }

    /// <summary>
    /// Estableciendo visibilidad de botones de acuerdo a facultades/permisos
    /// </summary>
    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();
        btnExportPDF.Visible = false;
        btnExportExcel.Visible = false;

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_ROL")))
            btnRoleAdd.Visible = false;

        if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDPDF") + "|"))
            btnExportPDF.Visible = true;

        if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDEX")+"|"))
            btnExportExcel.Visible = true;
    }

    /// <summary>
    /// Traduciendo el texto de los filtros
    /// </summary>
    public void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdRoles.FilterMenu;
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
    /// Recargar información del catálogo de roles
    /// </summary>
    private void ReloadDataGrid()
    {
        var data = this.GetRolesWithFacultades();
        this.RgdRoles.DataSource = data.OrderBy(x => x.Rol);
        this.RgdRoles.MasterTableView.Rebind();
    }


    #endregion

}
