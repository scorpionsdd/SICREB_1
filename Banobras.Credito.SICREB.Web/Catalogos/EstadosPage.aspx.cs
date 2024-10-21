using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Common;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Business.Repositorios;
using System.Data;
using System.IO;
using Banobras.Credito.SICREB.Data;
using System.Data.Common;

public partial class EstadoPage : System.Web.UI.Page
{
    public const String catalog = "Estados";
    int idUs;
    string persona;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {
                persona = Request.QueryString["Persona"].ToString();
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                    CambiaAtributosRGR();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó a Catálogo " + catalog + " " + persona);
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

        GridFilterMenu menu = RgdEstados.FilterMenu;
        for (int i = menu.Items.Count - 1; i >= 0; i--)
        {
            if (menu.Items[i].Text == "NoFilter")
            {
                menu.Items[i].Text = "Sin Filtro";
            }
            else if (menu.Items[i].Text == "EqualTo")
            {
                menu.Items[i].Text = "Igual";
            }
            else if (menu.Items[i].Text == "NotEqualTo")
            {
                menu.Items[i].Text = "Diferente";
            }
            else if (menu.Items[i].Text == "GreaterThan")
            {
                menu.Items[i].Text = "Mayor que";
            }
            else if (menu.Items[i].Text == "LessThan")
            {
                menu.Items[i].Text = "Menor que";
            }
            else if (menu.Items[i].Text == "GreaterThanOrEqualTo")
            {
                menu.Items[i].Text = "Mayor o igual a";
            }
            else if (menu.Items[i].Text == "LessThanOrEqualTo")
            {
                menu.Items[i].Text = "Menor o igual a";
            }
            else if (menu.Items[i].Text == "Between")
            {
                menu.Items[i].Text = "Entre";
            }
            else if (menu.Items[i].Text == "NotBetween")
            {
                menu.Items[i].Text = "No entre";
            }
            else if (menu.Items[i].Text == "IsNull")
            {
                menu.Items[i].Text = "Es nulo";
            }
            else if (menu.Items[i].Text == "NotIsNull")
            {
                menu.Items[i].Text = "No es nulo";
            }
            else if (menu.Items[i].Text == "Contains")
            {
                menu.Items[i].Text = "Contenga";
            }
            else if (menu.Items[i].Text == "DoesNotContain")
            {
                menu.Items[i].Text = "No Contenga";
            }
            else if (menu.Items[i].Text == "StartsWith")
            {
                menu.Items[i].Text = "Inicie con";
            }
            else if (menu.Items[i].Text == "EndsWith")
            {
                menu.Items[i].Text = "Finalice con";
            }
            else
                menu.Items.RemoveAt(i);
        }

    }
    protected void RgdEstados_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        Estados_Rules getRecords = null;
        string Persona = string.Empty;
        List<Estado> estadosInfo;
        List<int> lsClic = new List<int>();
        try
        {
            Persona = Request.QueryString["Persona"].ToString();
            Enums.Persona pPersona = (Persona == "PM") ? Enums.Persona.Moral : Enums.Persona.Fisica;
            if (Request.QueryString.Count == 1)
            {
                getRecords = new Estados_Rules(pPersona);
                estadosInfo = getRecords.GetRecords(false);
                RgdEstados.DataSource = estadosInfo;
                RgdEstados.VirtualItemCount = estadosInfo.Count;

                for (int i = 0; i < estadosInfo.Count; i++)
                {

                    lsClic.Add(estadosInfo[i].ClaveClic);
                }

                ViewState["ClaveClic"] = lsClic;
            }
            else
            {
                Mensajes.ShowError(this.Page, this.GetType(), new Exception("Error en la página"));
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdEstados_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

       
       
    }

    protected void RgdEstados_UpdateCommand(object source, GridCommandEventArgs e)
    {
        string DescripcionOld = string.Empty;
        string DescripcionNew = string.Empty;
        string ClaveBuroNew = string.Empty;
        string ClaveBuroOld = string.Empty;
        string oldPersona = string.Empty;
        //ArrayList estatus;
        ArrayList TipoPersonaNew;
        Enums.Estado estado;
        Int32 ClaveClicOld = 0;
        Int32 ClaveClicNew = 0;
        Estado oldEstado;
        Estado newEstado;
        bool Valida = true;


        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        try
        {

            estado = Enums.Estado.Activo;               

            Enums.Persona persona = new Enums.Persona();
            Enums.Persona personaold = new Enums.Persona();

            if (newValues["ClaveClic"] != null && newValues["ClaveBuro"] != null && newValues["Descripcion"] != null )
            {

                if (validaCampoNumerico(newValues["ClaveClic"].ToString()) && bValidaCampoVacio(newValues["ClaveBuro"].ToString(), "Clave Buró", e) && bValidaCampoVacio(newValues["Descripcion"].ToString(), "Descripción", e))
                {
                    ClaveClicOld = Convert.ToInt32(item.SavedOldValues["ClaveClic"]);
                    ClaveBuroOld = item.SavedOldValues["ClaveBuro"].ToString();

                    //Datos Nuevos
                    DescripcionNew = newValues["Descripcion"].ToString();
                    ClaveBuroNew = newValues["ClaveBuro"].ToString();

                    ClaveClicNew = Convert.ToInt32(newValues["ClaveClic"]);

                    if (ClaveClicOld != ClaveClicNew)
                    {
                        Valida = bValidaClic(ClaveClicNew);
                    }
                }
                else
                    Valida = false;
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
                e.Canceled = true;                   
                Valida = false;
            }

            if (Valida)
            {

                if (Request.QueryString["Persona"].ToString() == "PM") { personaold = Enums.Persona.Moral; } else { personaold = Enums.Persona.Fisica; }
                if (Request.QueryString["Persona"].ToString() == "PM") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
                //if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }

                oldEstado = new Estado(1, ClaveClicOld, ClaveBuroOld, DescripcionOld, personaold, Enums.Estado.Activo);
                newEstado = new Estado(1, ClaveClicNew, ClaveBuroNew, DescripcionNew, persona, estado);

                //Llamar el SP correspondiente con las entidades old y new de Validacion

                Estados_Rules estadoValores = new Estados_Rules(Enums.Persona.Moral);

                if (estadoValores.Update(oldEstado, newEstado) > 0)
                {
                    if (oldPersona == "Moral")
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                    }
                    else
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                    }

                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                    ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro ", null, null, catalog + " " + persona, 1, oldValues, newValues);

                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                    //Response.Write("<script> alert('El Registro no fue modificado'); </script>");

                    //JAGH se inserta actividad 20/01/13
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(800, idUs, "El registro no fue modificado Catálogo " + catalog + " " + persona);
                }
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
            //JAGH  se inserta actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue modificado Catálogo " + catalog + " " + persona);
        }
    }

    protected void RgdEstados_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdEstados_DeleteCommand(object source, GridCommandEventArgs e)
    {
        Estado EstadoDelete;
        string claveBuro = string.Empty;
        Int32 claveClic = 0;
        string tipoPersona = string.Empty;
        //Datos para identificar valor

        GridDataItem item = (GridDataItem)e.Item;

        try
        {

            claveClic = Convert.ToInt32(item["ClaveClic"].Text);
            claveBuro = item["ClaveBuro"].Text.ToString();
            tipoPersona = item["TipoPersonaTemp"].Text.ToString();

            Enums.Persona persona = new Enums.Persona();

            if (tipoPersona == "Moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
            if (tipoPersona == "Moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
            EstadoDelete = new Estado(999, claveClic, claveBuro, "Descripcion", persona, Enums.Estado.Activo);
            //Llamar el SP correspondiente con las entidades old y new de Validacion

            Estados_Rules EstadoBorrar = new Estados_Rules(Enums.Persona.Moral);

            if (EstadoBorrar.Delete(EstadoDelete) > 0)
            {
                if (tipoPersona == "Moral")
                {
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                }
                else
                {
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                }
                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                //JAGH se inserta actividad 20/01/13
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro ", null, null, catalog + " " + Request.QueryString["Persona"].ToString(), 1, null, null);

            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                //JAGH se inserta actividad 20/01/13
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(800, idUs, "El registro no fue eliminado " + catalog + " " + Request.QueryString["Persona"].ToString());
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
            //JAGH se inserta actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue eliminado " + catalog + " " + Request.QueryString["Persona"].ToString());
        }
    }

    protected void RgdEstados_InsertCommand(object source, GridCommandEventArgs e)
    {
        this.RgdEstados.MasterTableView.GetColumn("TipoPersonaTemp").Visible = true;
        this.RgdEstados.MasterTableView.GetColumn("TipoPersonaTemp").Visible = false;
        InsertEstado(e);
    }
    private void InsertEstado(GridCommandEventArgs e)
    {
        Estados_Rules EstadoInsertar;
        try
        {
            Estado record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion
                if (record.Id != 0 || record.ClaveClic != 0)
                {

                    if (bValidaClic(record.ClaveClic) && bValidaCampoVacio(record.ClaveBuro,"Clave Buró",e)&&bValidaCampoVacio(record.Descripcion,"Descripción",e))
                    {

                        EstadoInsertar = new Estados_Rules((Request.QueryString["Persona"].ToString() == "PM") ? Enums.Persona.Moral : Enums.Persona.Fisica);
                        if (EstadoInsertar.Insert(record) > 0)
                        {
                            if (Convert.ToString(record.TipoPersona) == "Moral")
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            }
                            else
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            }

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                            //JAGH se inserta actividad 20/01/13
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(7777, idUs, "El registro se guardó exitosamente " + catalog + " " + Request.QueryString["Persona"].ToString());
                            //this.lblMsg.Text = "El registro se guardó exitosamente";
                            //this.lblMsg.Visible = true;
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                            //JAGH se inserta actividad 20/01/13
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(800, idUs, "El registro no fue guardado " + catalog + " " + Request.QueryString["Persona"].ToString());
                        }

                    }

                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores Ingresados no Válidos");
                }
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
            //JAGH se inserta actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue guardado " + catalog + " " + Request.QueryString["Persona"].ToString());
        }
    }

    private bool bValidaClic(int iClic)
    {

        bool Valido = true;

        List<int> lsTiposCredito = new List<int>();

        lsTiposCredito = (List<int>)ViewState["ClaveClic"];

        if (lsTiposCredito.FindIndex(s => s == iClic) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Clave CLIC: " + iClic + " ya se encuentra dada de alta.");
        }

        return Valido;
    }

    public bool bValidaCampoVacio(string Campo, string NombreCampo, Telerik.Web.UI.GridCommandEventArgs e)
    {

        int longitud = 0;

        for (int n = 0; n < Campo.Length; n++)
        {

            if (Char.IsWhiteSpace(Campo, n))
            {

                longitud++;

            }
        }

        if (longitud == Campo.Length)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El campo " + NombreCampo + " debe tener datos, favor de verificar.");

            e.Canceled = true;

            return false;
        }

        return true;
    }

    private Estado ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        Estado record;
        ArrayList TipoPersona;
        //ArrayList estatus;
        Enums.Persona persona;
        Enums.Estado estado = Enums.Estado.Activo;       
        string txtClic;

        //estatus = Util.RadComboToString(editedItem["EstatusTemp"].FindControl("ComboEstatus"));
        TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));
        if ((Request.QueryString["Persona"].ToString() == "PM")) { persona = Enums.Persona.Moral; } else  { persona = Enums.Persona.Fisica; }
        //if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }
        // Extrae todos los elementos

        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {

            if (newValues["ClaveClic"] != null && newValues["ClaveBuro"] != null && newValues["Descripcion"] != null && TipoPersona != null)
            {
                
                txtClic = newValues["ClaveClic"].ToString();               

                if (validaCampoNumerico(txtClic))
                {

                    record = new Estado(0, Convert.ToInt32(newValues["ClaveClic"]), newValues["ClaveBuro"].ToString(), newValues["Descripcion"].ToString(), persona, estado);
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH se agrega linea 22/01/2013
                    ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog + " " + persona, 1, null, newValues);

                }
                else
                {
                    record = new Estado(0, 0, "CveBuro", "Descripcion", Enums.Persona.Moral, Enums.Estado.Activo);
                }
            }
            else
            {
                record = new Estado(0, 0, "CveBuro", "Descripcion", Enums.Persona.Moral, Enums.Estado.Activo);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            record = new Estado(0, 0, "CveBuro", "Descripcion", Enums.Persona.Moral, Enums.Estado.Activo);
        }
        return record;
    }

    public bool validaCampoNumerico(string Campo)
    {
        //1  Numerico
        bool valido = true;


        for (int n = 0; n < Campo.Length; n++)
                {

                    if (!Char.IsNumber(Campo, n))
                    {

                        valido = false;
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en el campo Clave CLIC deben ser numéricos, favor de verificar");
                        break;
                    }                    
                }
               
       
        return valido;
    }


    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdEstados.Columns[0].Visible = false;
        RgdEstados.Columns[RgdEstados.Columns.Count - 1].Visible = false;
        RgdEstados.MasterTableView.HierarchyDefaultExpanded = true;
        RgdEstados.ExportSettings.OpenInNewWindow = false;
        RgdEstados.ExportSettings.ExportOnlyData = true;
        RgdEstados.MasterTableView.GridLines = GridLines.Both;
        RgdEstados.ExportSettings.IgnorePaging = true;
        RgdEstados.ExportSettings.OpenInNewWindow = true;
        RgdEstados.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog + " " + persona);

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdEstados.Columns[0].Visible = false;
        RgdEstados.Columns[RgdEstados.Columns.Count - 1].Visible = false;
        RgdEstados.MasterTableView.HierarchyDefaultExpanded = true;
        RgdEstados.ExportSettings.OpenInNewWindow = false;
        RgdEstados.ExportSettings.ExportOnlyData = true;

        RgdEstados.ExportSettings.IgnorePaging = true;
        RgdEstados.ExportSettings.OpenInNewWindow = true;
        RgdEstados.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog + " " + persona);

    }
    protected void RgdEstados_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                item["TipoPersonaTemp"].Text = item["TipoPersonaTemp"].Text;
                item["EstatusTemp"].Text = item["Estatus"].Text;
            }
            else if (e.Item.IsInEditMode)
            {

                GridDataItem item;
                RadComboBox comboPersona;
                RadComboBox comboEstatus;
                this.RgdEstados.MasterTableView.GetColumn("TipoPersonaTemp").Visible = true;
                this.RgdEstados.MasterTableView.GetColumn("TipoPersonaTemp").Visible = false;
                item = (GridDataItem)e.Item;
                comboPersona = (RadComboBox)item["TipoPersonaTemp"].FindControl("ComboPersona");
                comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);
                if (newValues["Estatus"] != null && newValues["TipoPersonaTemp"] != null)
                {
                    if (newValues["Estatus"].ToString() == "Activo")
                        comboEstatus.SelectedIndex = 0;
                    else
                        comboEstatus.SelectedIndex = 1;
                    if (newValues["TipoPersonaTemp"].ToString() == "Fisica")
                        comboPersona.SelectedIndex = 0;
                    else
                        comboPersona.SelectedIndex = 1;
                }

            }
            if (e.Item is GridCommandItem)
            {
                Button addButton = e.Item.FindControl("AddNewRecordButton") as Button;
                LinkButton lnkButton = (LinkButton)e.Item.FindControl("InitInsertButton");
                if (facultadInsertar())
                {
                    addButton.Visible = true;
                    lnkButton.Visible = true;
                }
                else
                {
                    addButton.Visible = false;
                    lnkButton.Visible = false;
                }
            }

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
        this.RgdEstados.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdEstados.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_EDOS_M")))
            {
                this.RgdEstados.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_EDOS_M")))
            {
                //this.RgdEstados.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_EDOS_F")))
            {
                this.RgdEstados.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_EDOS_F")))
            {
                //this.RgdEstados.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }
    }

    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGAR_EDOS_F")))
        {
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_EDOS_M")))
        {
            valido = true;
        }
        return valido;
    }
    protected void btn_cargar_Click(object sender, EventArgs e)
    {
        cls_cargaArchivos cargaMasiva = new cls_cargaArchivos();
        try
        {

            if (file_txt_layout.HasFile)
            {


                DataTable dt_metaDataLayout = new DataTable();
                string ruta_archivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + file_txt_layout.FileName));

                file_txt_layout.SaveAs(ruta_archivo);

                DataTable dt_layout_procesado = new DataTable();


                dt_metaDataLayout.Columns.Add("nombreColumna");
                dt_metaDataLayout.Columns.Add("tipoDato");
                dt_metaDataLayout.Columns.Add("longitud");



                DataRow row1;

                row1 = dt_metaDataLayout.NewRow();
                row1["nombreColumna"] = "Clave CLIC";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Clave Buró";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "2";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Descripción";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "30";



                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);


                //List<DbParameter> parametros = new List<DbParameter>();

                DbParameter[] parametros = new DbParameter[6];


                parametros[0] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[0].Direction = ParameterDirection.Output;
                parametros[0].ParameterName = "id_OUT";
                parametros[0].DbType = DbType.Decimal;
                parametros[0].Size = 38;

                parametros[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[1].Direction = ParameterDirection.Output;
                parametros[1].ParameterName = "tipo_OUT";
                parametros[1].DbType = DbType.Decimal;
                parametros[1].Size = 38;


                parametros[2] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].ParameterName = "personap";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;


                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "clave_clicp";
                parametros[3].DbType = DbType.Decimal;
                parametros[3].Size = 5;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "clave_burop";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 5;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "descripcionp";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 30;

                string storeBase = "SP_cargaMasiva_estados";


                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_estados", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;

                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }

                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("  {0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdEstados.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog + " " + persona, total + " Registros", numeros + " Registros", catalog, 1);

                RgdEstados.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catch
    }
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdEstados.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdEstados.AllowPaging = false;
                RgdEstados.Rebind();
                foreach (GridDataItem row in RgdEstados.Items)
                {
                    Estado EstadoDelete;
                    string claveBuro = string.Empty;
                    Int32 claveClic = 0;
                    string tipoPersona = string.Empty;




                    claveClic = Parser.ToNumber(row.GetDataKeyValue("ClaveClic").ToString());
                    claveBuro = row["ClaveBuro"].Text.ToString();
                    tipoPersona = row["TipoPersonaTemp"].Text.ToString();

                    Enums.Persona persona = new Enums.Persona();

                    if (tipoPersona == "Moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
                    if (tipoPersona == "Moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
                    EstadoDelete = new Estado(999, claveClic, claveBuro, "Descripcion", persona, Enums.Estado.Activo);
                    //Llamar el SP correspondiente con las entidades old y new de Validacion

                    Estados_Rules EstadoBorrar = new Estados_Rules(Enums.Persona.Moral);

                    if (EstadoBorrar.Delete(EstadoDelete) > 0)
                    {
                        if (tipoPersona == "Moral")
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        }
                        else
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        }
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }//foreach
                RgdEstados.Rebind();
                RgdEstados.DataSource = null;
                RgdEstados.AllowPaging = true;
                RgdEstados.Rebind();
                RgdEstados.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdEstados.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdEstados.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {

                        Estado EstadoDelete;
                        string claveBuro = string.Empty;
                        Int32 claveClic = 0;
                        string tipoPersona = string.Empty;




                        claveClic = Parser.ToNumber(row.GetDataKeyValue("ClaveClic").ToString());
                        claveBuro = row["ClaveBuro"].Text.ToString();
                        tipoPersona = row["TipoPersonaTemp"].Text.ToString();

                        Enums.Persona persona = new Enums.Persona();

                        if (tipoPersona == "Moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
                        if (tipoPersona == "Moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
                        EstadoDelete = new Estado(999, claveClic, claveBuro, "Descripcion", persona, Enums.Estado.Activo);
                        //Llamar el SP correspondiente con las entidades old y new de Validacion

                        Estados_Rules EstadoBorrar = new Estados_Rules(Enums.Persona.Moral);

                        if (EstadoBorrar.Delete(EstadoDelete) > 0)
                        {
                            if (tipoPersona == "Moral")
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            }
                            else
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            }
                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + claveClic, null, null, catalog + " " + persona, 1, null, null);

                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");

                        }




                    }//checket

                }//FOREACH

            }
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch
        //btn_eliminar_Click
        RgdEstados.Rebind();
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;

            CheckBox chkHeader;

            chkHeader = (CheckBox)sender;


            if (chkHeader.Checked == true)
            {
                foreach (GridDataItem row in RgdEstados.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdEstados.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = false;
                }
            }




        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch


        //ChkTodo_CheckedChanged
    }

    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }



}
