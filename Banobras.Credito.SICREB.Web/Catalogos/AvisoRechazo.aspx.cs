using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Common;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Business.Repositorios;

public partial class AvisoRechazo : System.Web.UI.Page
{
    public const String catalog = "Validaciones";
    
    public bool oldAplicable
    {
        get;
        set;
    }

    public string oldMensaje
    {
        get
        {
            return ViewState["oldMensaje"] != null ? Convert.ToString(ViewState["oldMensaje"]) : string.Empty;
        }
        set
        {
            ViewState["oldMensaje"] = value;
        }
    }

    public string oldTipo
    {
        get
        {
            return ViewState["oldTipo"] != null ? Convert.ToString(ViewState["oldTipo"]) : string.Empty;
        }
        set
        {
            ViewState["oldTipo"] = value;
        }
    }
    int idUs;
    int SeleccionTipo = -1;
    int SeleccionActivo = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {
                string persona = Request.QueryString["Persona"].ToString();
                getFacultades(persona);

                CambiaAtributosRGR();
               
                if (!this.Page.IsPostBack)
                {
                    
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingreso a Catálogo " + catalog + " " + persona);
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);

        }

    }
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdAvisoRechazos.FilterMenu;
        foreach (RadMenuItem item in menu.Items)
        {    //change the text for the "StartsWith" menu item  
            if (item.Text == "NoFilter")
            {
                item.Text = "Sin Filtro";
            }
            else if (item.Text == "EqualTo")
            {
                item.Text = "Igual";
            }
            else if (item.Text == "NotEqualTo")
            {
                item.Text = "Diferente";
            }
            else if (item.Text == "GreaterThan")
            {
                item.Text = "Mayor que";
            }
            else if (item.Text == "LessThan")
            {
                item.Text = "Menor que";
            }
            else if (item.Text == "GreaterThanOrEqualTo")
            {
                item.Text = "Mayor o igual a";
            }
            else if (item.Text == "LessThanOrEqualTo")
            {
                item.Text = "Menor o igual a";
            }
            else if (item.Text == "Between")
            {
                item.Text = "Entre";
            }
            else if (item.Text == "NotBetween")
            {
                item.Text = "No entre";
            }
            else if (item.Text == "IsNull")
            {
                item.Text = "Es nulo";
            }
            else if (item.Text == "NotIsNull")
            {
                item.Text = "No es nulo";
            }
            else if (item.Text == "Contains")
            {
                item.Text = "Contenga";
            }
            else if (item.Text == "DoesNotContain")
            {
                item.Text = "No Contenga";
            }
            else if (item.Text == "StartsWith")
            {
                item.Text = "Inicie con";
            }
            else if (item.Text == "EndsWith")
            {
                item.Text = "Finalice con";
            }
            else if (item.Text == "IsEmpty")
            {
               // item.Text = "Finalice con";
                item.Visible = false;
            }
            else if (item.Text == "NotIsEmpty")
            {
               // item.Text = "Finalice con";
                item.Visible = false;
            }                
        }


        //GridFilterMenu menu = RgdAvisoRechazos.FilterMenu;
        //for (int i = menu.Items.Count - 1; i >= 0; i--)
        //{
        //    if (menu.Items[i].Text == "NoFilter")
        //    {
        //        menu.Items[i].Text = "Sin Filtro";
        //    }
        //    else if (menu.Items[i].Text == "EqualTo")
        //    {
        //        menu.Items[i].Text = "Igual";
        //    }
        //    else if (menu.Items[i].Text == "NotEqualTo")
        //    {
        //        menu.Items[i].Text = "Diferente";
        //    }
        //    else if (menu.Items[i].Text == "GreaterThan")
        //    {
        //        menu.Items[i].Remove();
        //       // menu.Items[i].Text = "Mayor que";
        //    }
        //    else if (menu.Items[i].Text == "LessThan")
        //    {
        //        menu.Items[i].Remove();
        //       // menu.Items[i].Text = "Menor que";
        //    }
        //    else if (menu.Items[i].Text == "GreaterThanOrEqualTo")
        //    {
        //        menu.Items[i].Remove();
        //       // menu.Items[i].Text = "Mayor o igual a";
        //    }
        //    else if (menu.Items[i].Text == "LessThanOrEqualTo")
        //    {
        //        menu.Items[i].Remove();
        //        //menu.Items[i].Text = "Menor o igual a";
        //    }
        //    else if (menu.Items[i].Text == "Between")
        //    {
        //        menu.Items[i].Text = "Entre";
        //    }
        //    else if (menu.Items[i].Text == "NotBetween")
        //    {
        //        menu.Items[i].Text = "No entre";
        //    }
        //    else if (menu.Items[i].Text == "IsNull")
        //    {
        //        menu.Items[i].Text = "Es nulo";
        //    }
        //    else if (menu.Items[i].Text == "NotIsNull")
        //    {
        //        menu.Items[i].Text = "No es nulo";
        //    }
        //    else if (menu.Items[i].Text == "Contains")
        //    {
        //        menu.Items[i].Text = "Contenga";
        //    }
        //    else if (menu.Items[i].Text == "DoesNotContain")
        //    {
        //        menu.Items[i].Text = "No Contenga";
        //    }
        //    else if (menu.Items[i].Text == "StartsWith")
        //    {
        //        menu.Items[i].Text = "Inicie con";
        //    }
        //    else if (menu.Items[i].Text == "EndsWith")
        //    {
        //        menu.Items[i].Text = "Finalice con";
        //    }
        //    else
        //        menu.Items.RemoveAt(i);
        //}

    }
    protected void RgdAvisoRechazos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ValidacionesDataAccess getRecords = null;
        List<Validacion> DatosValidacion;
        RgdAvisoRechazos.EnableLinqExpressions = false;
        string Persona = string.Empty;
        try
        {
            Persona = Request.QueryString["Persona"].ToString();
            Enums.Persona pPersona = (Persona == "PM") ? Enums.Persona.Moral : Enums.Persona.Fisica;
            getRecords = new ValidacionesDataAccess(pPersona);
            DatosValidacion = getRecords.GetRecords(false);
            RgdAvisoRechazos.DataSource = DatosValidacion;
            RgdAvisoRechazos.VirtualItemCount = DatosValidacion.Count;

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void CargarGrid()
    {
        ValidacionesDataAccess getRecords = null;
        List<Validacion> DatosValidacion;
        RgdAvisoRechazos.EnableLinqExpressions = false;
        string Persona = string.Empty;
        try
        {
            Persona = Request.QueryString["Persona"].ToString();
            Enums.Persona pPersona = (Persona == "PM") ? Enums.Persona.Moral : Enums.Persona.Fisica;
            getRecords = new ValidacionesDataAccess(pPersona);
            DatosValidacion = getRecords.GetRecords(false);
            RgdAvisoRechazos.DataSource = DatosValidacion;
            RgdAvisoRechazos.VirtualItemCount = DatosValidacion.Count;

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdAvisoRechazos_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {


    }
    protected void RgdAvisoRechazos_UpdateCommand(object source, GridCommandEventArgs e)
    {
        string CodeError = string.Empty;
        bool newAplicable = false;
        string newMensaje = string.Empty;
        ArrayList newTipo;
        Validacion oldValidacion;
        Validacion newValidacion;
        Enums.Rechazo Tipo;
        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        GridDataItem item = (GridDataItem)e.Item;
        editedItem.ExtractValues(newValues);
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        try
        {

            //Datos Orginales
            int id = Parser.ToNumber(item.SavedOldValues["Id"]);
            oldMensaje = item.SavedOldValues["Mensaje"].ToString();
            oldTipo = item.SavedOldValues["Tipo"].ToString();
            oldAplicable = (Boolean)item.SavedOldValues["Aplicable"];

            //Datos Nuevos
            newMensaje = newValues["Mensaje"].ToString();
            CheckBox ChkBx = (CheckBox)item["Aplicable"].Controls[0];
            newTipo = Util.RadComboToString(item["TipoTemp"].FindControl("ComboTipo"));
            newAplicable = ChkBx.Checked;
            string etiqueta = "ETIQUETA";
            string campo = "CAMPO";
            Enums.Estado estado = Enums.Estado.Activo;
            Tipo = (newTipo[0].ToString() == "Error" ? Enums.Rechazo.Error : Enums.Rechazo.Warning);
            oldValidacion = new Validacion(id, Tipo, oldAplicable, Enums.Persona.Moral, oldMensaje, estado, CodeError, 1, etiqueta, campo);
            newValidacion = new Validacion(id, Tipo, newAplicable, Enums.Persona.Moral, newMensaje, estado, CodeError, 1, etiqueta, campo);

            //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
            ValidacionesDataAccess AvisoValidacion = new ValidacionesDataAccess(Enums.Persona.Moral);
            if (AvisoValidacion.UpdateRecord(oldValidacion, newValidacion) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);

            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
            }
        }
        catch (Exception x)
        {
            Mensajes.ShowError(this.Page, this.GetType(), x);
        }
        finally
        {
            RestablecerValores();
        }

    }

    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdAvisoRechazos.Columns[0].Visible = false;
        RgdAvisoRechazos.Columns[2].Visible = false;
        RgdAvisoRechazos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAvisoRechazos.ExportSettings.OpenInNewWindow = false;
        RgdAvisoRechazos.ExportSettings.ExportOnlyData = true;
        RgdAvisoRechazos.MasterTableView.GridLines = GridLines.Both;
        RgdAvisoRechazos.ExportSettings.IgnorePaging = true;
        RgdAvisoRechazos.ExportSettings.OpenInNewWindow = true;
        RgdAvisoRechazos.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdAvisoRechazos.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdAvisoRechazos.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdAvisoRechazos.Columns[0].Visible = false;
        RgdAvisoRechazos.Columns[2].Visible = false;
        RgdAvisoRechazos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAvisoRechazos.ExportSettings.OpenInNewWindow = false;
        RgdAvisoRechazos.ExportSettings.ExportOnlyData = true;

        RgdAvisoRechazos.ExportSettings.IgnorePaging = true;
        RgdAvisoRechazos.ExportSettings.OpenInNewWindow = true;
        RgdAvisoRechazos.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }
    protected void RgdAvisoRechazos_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            item["TipoTemp"].Text = item["Tipo"].Text;
        }
        else if (e.Item.IsInEditMode)
        {

            GridDataItem item;
            RadComboBox comboTipo;

            this.RgdAvisoRechazos.MasterTableView.GetColumn("TipoTemp").Visible = true;
            this.RgdAvisoRechazos.MasterTableView.GetColumn("Tipo").Visible = false;
            item = (GridDataItem)e.Item;
            comboTipo = (RadComboBox)item["TipoTemp"].FindControl("ComboTipo");
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);
            if (newValues["TipoPersona"] != null)
            {
                if (newValues["TipoPersona"].ToString() == "Fisica")
                    comboTipo.SelectedIndex = 0;
                else
                    comboTipo.SelectedIndex = 1;
            }
        }
        if (e.Item is GridFilteringItem)
        {
            GridFilteringItem filterItem = (GridFilteringItem)e.Item;
            // RadComboBox en el Filter Template
            RadComboBox combo = (RadComboBox)filterItem.FindControl("FiltroRadCombo");
            RadComboBox comboPersona = (RadComboBox)filterItem.FindControl("FiltroPersona");
            RadComboBox comboEstatus = (RadComboBox)filterItem.FindControl("FiltroEstatus");
            // asigno los Items
            RadComboBoxItem Error = new RadComboBoxItem();
            RadComboBoxItem Warning = new RadComboBoxItem();
            RadComboBoxItem Todos = new RadComboBoxItem();
            RadComboBoxItem Todo = new RadComboBoxItem();
            RadComboBoxItem Fisica = new RadComboBoxItem();
            RadComboBoxItem Moral = new RadComboBoxItem();
            RadComboBoxItem Activo = new RadComboBoxItem();
            RadComboBoxItem Inactivo = new RadComboBoxItem();

            Todo.Value = "0";
            Todo.Text = "Todos";
            Todos.Value = "0";
            Todos.Text = "Todos";
            Error.Value = "1";
            Error.Text = "Error";
            Warning.Value = "2";
            Warning.Text = "Warning";
            Fisica.Value = "1";
            Fisica.Text = "Fisica";           
            Moral.Value = "2";
            Moral.Text = "Moral";
            Activo.Value = "1";
            Activo.Text = "Activo";          
            Inactivo.Value = "2";
            Inactivo.Text = "Inactivo";

            combo.Items.Add(Todos);
            combo.Items.Add(Error);
            combo.Items.Add(Warning);

            if (SeleccionTipo > -1)
                combo.SelectedIndex = SeleccionTipo;
            SeleccionTipo = -1;

            if (comboPersona != null)
            {
                comboPersona.Items.Add(Todo);
                comboPersona.Items.Add(Fisica);
                comboPersona.Items.Add(Moral);
            }

            if (comboEstatus != null)
            {
                comboEstatus.Items.Add(Todo);
                comboEstatus.Items.Add(Activo);
                comboEstatus.Items.Add(Inactivo);
            }

            if (SeleccionActivo > -1)
            {
                comboEstatus.SelectedIndex = SeleccionActivo;
            }

            SeleccionTipo = -1;

        }
    }

    protected void FiltroRadCombo_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Enums.Rechazo filtro = new Enums.Rechazo();
        try
        {

            if (e.Value != "0")
            {
                string filterExpression;
                if (e.Value == "1")
                    filtro = Enums.Rechazo.Error;
                else if (e.Value == "2")
                    filtro = Enums.Rechazo.Warning;

                SeleccionTipo = Parser.ToNumber(e.Value);

                GridColumn column = RgdAvisoRechazos.MasterTableView.GetColumnSafe("Tipo");
                column.CurrentFilterFunction = GridKnownFunction.EqualTo;
                filterExpression = "[Tipo] = '" + filtro + "'";
                RgdAvisoRechazos.MasterTableView.FilterExpression = filterExpression;
                RgdAvisoRechazos.MasterTableView.Rebind();
            }
            else
            {
                GridColumn column = RgdAvisoRechazos.MasterTableView.GetColumnSafe("Tipo");
                column.CurrentFilterFunction = GridKnownFunction.DoesNotContain;               
                RgdAvisoRechazos.MasterTableView.FilterExpression = "[Tipo] <> '0'";
                RgdAvisoRechazos.MasterTableView.Rebind();
                SeleccionTipo = Parser.ToNumber(e.Value);              
            }                        
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    protected void FiltroPersona_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Enums.Persona filtro = new Enums.Persona();
        try
        {
                string filterExpression;
                if (e.Value == "1")
                    filtro = Enums.Persona.Fisica;
                else if (e.Value == "2")
                    filtro = Enums.Persona.Moral;

                SeleccionActivo = Parser.ToNumber(e.Value);

                GridColumn column = RgdAvisoRechazos.MasterTableView.GetColumnSafe("Persona");
                column.CurrentFilterFunction = GridKnownFunction.EqualTo;
                filterExpression = "[Persona] = '" + filtro + "'";
                RgdAvisoRechazos.MasterTableView.FilterExpression = filterExpression;
                RgdAvisoRechazos.MasterTableView.Rebind();
          
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    protected void FiltroEstatus_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Enums.Estado filtro = new Enums.Estado();
        try
        {

            if (e.Value != "0")
            {

                string filterExpression;
                if (e.Value == "1")
                    filtro = Enums.Estado.Activo;
                else if (e.Value == "2")
                    filtro = Enums.Estado.Inactivo;

                SeleccionActivo = Parser.ToNumber(e.Value);

                GridColumn column = RgdAvisoRechazos.MasterTableView.GetColumnSafe("Estatus");
                column.CurrentFilterFunction = GridKnownFunction.EqualTo;
                filterExpression = "[Estatus] = '" + filtro + "'";
                RgdAvisoRechazos.MasterTableView.FilterExpression = filterExpression;
                RgdAvisoRechazos.MasterTableView.Rebind();
            }
            else
            {
                GridColumn column = RgdAvisoRechazos.MasterTableView.GetColumnSafe("Estatus");
                column.CurrentFilterFunction = GridKnownFunction.DoesNotContain;
                RgdAvisoRechazos.MasterTableView.FilterExpression = "[Estatus] <> '0'";
                RgdAvisoRechazos.MasterTableView.Rebind();

                SeleccionActivo = Parser.ToNumber(e.Value);
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    protected void FiltroCheck_CheckedChange(object sender, EventArgs e)
    {
        string filtro = string.Empty;
        try
        {
            string filterExpression;
            CheckBox chk;
            chk = (CheckBox)sender;
            filtro = (chk.Checked ? "1" : "0");

            GridColumn column = RgdAvisoRechazos.MasterTableView.GetColumnSafe("Aplicable");
            column.CurrentFilterFunction = GridKnownFunction.EqualTo;
            filterExpression = "[Aplicable] = '" + filtro + "'";
            RgdAvisoRechazos.MasterTableView.FilterExpression = filterExpression;
            RgdAvisoRechazos.MasterTableView.Rebind();
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }

    }

    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdAvisoRechazos.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MCV")))
            {
                this.RgdAvisoRechazos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EEWPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EEWEX") + "|"))
                ImageButton1.Visible = true;

        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MCV")))
            {
                this.RgdAvisoRechazos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EEWPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EEWEX") + "|"))
                ImageButton1.Visible = true;

        }
        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }

    }

    protected void TipoSelectedIndexChange(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //Mensajes.ShowError("aca estoy ",this);

    }

    protected void RgdAvisoRechazos_ItemCommand(object source, GridCommandEventArgs e)
    {

    }

    public void RestablecerValores()
    {
        this.RgdAvisoRechazos.MasterTableView.GetColumn("TipoTemp").Visible = false;
        this.RgdAvisoRechazos.MasterTableView.GetColumn("Tipo").Visible = true;
    }

    protected void RgdAvisoRechazos_CancelCommand(object source, GridCommandEventArgs e)
    {
        RestablecerValores();
    }
    
}
