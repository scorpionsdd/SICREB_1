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
using System.Data.Common;
using Banobras.Credito.SICREB.Data;

public partial class ClaveObservacionPage : System.Web.UI.Page
{
    public const String catalog = "Claves de Observación";
    string persona;
    int idUs;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CambiaAtributosRGR();

            if (Session["Facultades"] != null)
            {
                persona = Request.QueryString["Persona"].ToString();
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                    //CambiaAtributosRGR();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + " Ingreso a Catálogo " + catalog + " " + Request.QueryString["Persona"].ToString());
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
    Enums.Persona pPersona;

    //JAGH se modifica para mostrar titulos filtros
    public void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdObservacion.FilterMenu;
        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (menu.Items[i].Text.Equals("NoFilter"))
                menu.Items[i].Text = "Sin Filtro";
            else if (menu.Items[i].Text.Equals("EqualTo"))
                menu.Items[i].Text = "Igual";
            else if (menu.Items[i].Text.Equals("NotEqualTo"))
                menu.Items[i].Text = "Diferente";
            else if (menu.Items[i].Text.Equals("GreaterThan"))
                menu.Items[i].Text = "Mayor que";
            else if (menu.Items[i].Text.Equals("LessThan"))
                menu.Items[i].Text = "Menor que";
            else if (menu.Items[i].Text.Equals("GreaterThanOrEqualTo"))
                menu.Items[i].Text = "Mayor o igual a";
            else if (menu.Items[i].Text.Equals("LessThanOrEqualTo"))
                menu.Items[i].Text = "Menor o igual a";
            else if (menu.Items[i].Text.Equals("Between"))
                menu.Items[i].Text = "Entre";
            else if (menu.Items[i].Text.Equals("NotBetween"))
                menu.Items[i].Text = "No entre";
            else if (menu.Items[i].Text.Equals("IsNull"))
                menu.Items[i].Text = "Es nulo";
            else if (menu.Items[i].Text.Equals("NotIsNull"))
                menu.Items[i].Text = "No es nulo";
            else if (menu.Items[i].Text.Equals("Contains"))
                menu.Items[i].Text = "Contenga";
            else if (menu.Items[i].Text.Equals("DoesNotContain"))
                menu.Items[i].Text = "No Contenga";
            else if (menu.Items[i].Text.Equals("StartsWith"))
                menu.Items[i].Text = "Inicie con";
            else if (menu.Items[i].Text.Equals("EndsWith"))
                menu.Items[i].Text = "Finalice con";
            else if (menu.Items[i].Text.Equals("NotIsEmpty"))
                menu.Items[i].Text = "No es vacio";
            else if (menu.Items[i].Text.Equals("IsEmpty"))
                menu.Items[i].Text = "Vacio";
        }

    }
       
    protected void RgdObservacion_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ClavesObservacion_Rules getRecords = null;
        List<Observacion> ClaveObservacion;
        List<string> lsClave = new List<string>();
        string Persona = string.Empty;
        try
        {
            Persona = Request.QueryString["Persona"].ToString();
            pPersona = (Persona == "PM") ? Enums.Persona.Moral : Enums.Persona.Fisica;

            if (Request.QueryString.Count == 1)
            {
                getRecords = new ClavesObservacion_Rules(pPersona);
                ClaveObservacion = getRecords.GetRecords(false);
                RgdObservacion.DataSource = ClaveObservacion;
                RgdObservacion.VirtualItemCount = ClaveObservacion.Count;

                for (int i = 0; i < ClaveObservacion.Count; i++)
                {

                    lsClave.Add(ClaveObservacion[i].Clave);
                }

                ViewState["Clave"] = lsClave;
            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "Error en página");
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdObservacion_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //JAGH se inserta actividad 18/01/13
        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(4444, idUs, "se ha editado claves observación " + " " + Request.QueryString["Persona"].ToString());
    }

    protected void RgdObservacion_UpdateCommand(object source, GridCommandEventArgs e)
    {

        string ClaveOld = string.Empty;
        string DescripcionOld = string.Empty;
        string ComentarioOld = string.Empty;
        string TipoPersonaOld = string.Empty;
        string ClaveNew = string.Empty;
        string DescripcionNew = string.Empty;
        string ComentarioNew = string.Empty;
        ArrayList TipoPersonaNew;
        ArrayList estatus;
        Int32 id = 0;
        bool Valida = true;
        Observacion oldObservacion;
        Observacion newObservacion;

        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        try
        {
            //Datos Orginales
            id = Convert.ToInt32(item.SavedOldValues["id"]); // se modifica nombre clave errónea JAGH 18/01/13
            ClaveOld = item.SavedOldValues["Clave"].ToString();  

            if (newValues["Clave"] != null && newValues["descripcion"] != null && newValues["comentario"] != null)
            {

                if (bValidaCampoVacio(newValues["Clave"].ToString(), "Clave") && bValidaCampoVacio(newValues["descripcion"].ToString(), "Descripción") && bValidaCampoVacio(newValues["comentario"].ToString(), "Comentario"))
                {
                    ClaveNew = newValues["Clave"].ToString();

                    if (ClaveOld != ClaveNew)
                    {
                        Valida = bValidaClave(ClaveNew);
                    }
                }
                else
                {
                    Valida = false;
                }
            }
            else
            {
                Valida = false;
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Todos los campos deben tener datos, favor de verificar.");          
            }

                if (Valida)
                {
                    
                    DescripcionOld = item.SavedOldValues["descripcion"].ToString();

                    if (item.SavedOldValues["comentario"] != null)
                        ComentarioOld = item.SavedOldValues["comentario"].ToString();

                    TipoPersonaOld = item.SavedOldValues["Persona"].ToString();

                    //Datos Nuevos


                    DescripcionNew = newValues["descripcion"].ToString();

                    if (newValues["comentario"] != null)
                        ComentarioNew = newValues["comentario"].ToString();

                    TipoPersonaNew = Util.RadComboToString(item["TipoPersonaTemp"].FindControl("ComboPersona"));

                    Enums.Persona persona = new Enums.Persona();
                    Enums.Estado estado = new Enums.Estado();

                    if (TipoPersonaOld == "Moral") { persona = Enums.Persona.Moral; } else persona = Enums.Persona.Fisica;
                    if (TipoPersonaNew[0].ToString() == "Moral") { persona = Enums.Persona.Moral; } else persona = Enums.Persona.Fisica;
                    estado = Enums.Estado.Activo;

                    oldObservacion = new Observacion(id, ClaveOld, DescripcionOld, ComentarioOld, estado, persona);
                    newObservacion = new Observacion(id, ClaveNew, DescripcionNew, ComentarioNew, estado, persona);

                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new
                    ClavesObservacion_Rules ObservacionesUpdate = new ClavesObservacion_Rules(pPersona);

                    if (ObservacionesUpdate.Update(oldObservacion, newObservacion) > 0)
                    {
                        if (item["Persona"].Text == "M")
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        }
                        else
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        }

                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro se modificó correctamente");
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro claves observación", null, null, catalog + " " + persona, 1, oldValues, newValues); //JAGH

                    }
                    else
                    {
                        //JAGH se inserta actividad 18/01/13
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        ActividadRules.GuardarActividad(800, idUs, "El registro no fue modificado en el catálogo " + catalog + " " + persona);

                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                    }
                }
        }
        catch (Exception ex)
        {
            //JAGH se inserta actividad 18/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue modificado en el catálogo " + catalog + " " + persona);

            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }

    }

    protected void RgdObservacion_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdObservacion_DeleteCommand(object source, GridCommandEventArgs e)
    {
        string clave = string.Empty;
        Int32 IdObservacion = 0;
        GridDataItem item = (GridDataItem)e.Item;
        try
        {
            clave = item["Clave"].Text;
            IdObservacion = Convert.ToInt32(item["Id"].Text);
            Observacion ObservacionDelete = new Observacion(IdObservacion, clave, item["Descripcion"].Text, item["Comentario"].Text, Enums.Estado.Activo, Enums.Persona.Moral);
            //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
            ClavesObservacion_Rules observacionesBorrar = new ClavesObservacion_Rules(pPersona);
            if (observacionesBorrar.Delete(ObservacionDelete) > 0)
            {
                if (item["Persona"].Text == "M")
                {
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                }
                else
                {
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                }
                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro ", null, null, catalog + " " + Request.QueryString["Persona"].ToString(), 1, null, null);
            }
            else
            {
                //JAGH se inserta actividad 18/01/13
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(800, idUs, "El registro no fue removido en el catálogo " + catalog + " " + Request.QueryString["Persona"].ToString());

                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
            }
        }
        catch (Exception exc)
        {
            //JAGH se inserta actividad 18/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue removido en el catálogo " + catalog + " " + Request.QueryString["Persona"].ToString());

            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdObservacion_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertCveObs(e);
    }

    private void InsertCveObs(GridCommandEventArgs e)
    {
        ClavesObservacion_Rules ObservacionInsertar;
        try
        {
            Observacion record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {
                if (record.Id != 0 || record.Clave != "0")
                {

                   if (bValidaClave(record.Clave))
                    {

                        ObservacionInsertar = new ClavesObservacion_Rules(pPersona);
                        if (ObservacionInsertar.Insert(record) > 0)
                        {
                            if (Convert.ToString(record.Persona) == "Moral")
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            }
                            else
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            }

                            //JAGH se inserta actividad 18/01/13
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(34, idUs, "El registro se guardó exitosamente claves observación " + " " + Request.QueryString["Persona"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");

                        }
                        else
                        {
                            //JAGH se inserta actividad 18/01/13
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(800, idUs, "El registro no fue guardado claves observación " + " " + Request.QueryString["Persona"].ToString());

                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            e.Canceled = true;
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

            if (ex.Message.ToString().Contains("not in a correct format"))
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El tipo de Dato ingresado es incorrecto, favor de verificar");
                //Response.Write("<script>alert('El tipo de Dato ingresado es Incorrecto, Favor de Verificar')</script>"); ;
                //JAGH se inserta actividad 18/01/13               
                ActividadRules.GuardarActividad(800, idUs, "El tipo de Dato ingresado es incorrecto, favor de verificar claves observación " + " " + Request.QueryString["Persona"].ToString());
            }

            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue guardado claves observación " + " " + Request.QueryString["Persona"].ToString());
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }


    public bool bValidaCampoVacio(string Campo, string NombreCampo)
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

            return false;
        }

        return true;
    }

    private bool bValidaClave(string sClave)
    {
        bool Valido = true;

        List<string> lsSic = new List<string>();

        lsSic = (List<string>)ViewState["Clave"];

        if (lsSic.FindIndex(s => s == sClave) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Clave : " + sClave + " ya se encuentra dada de alta.");
        }

        return Valido;
    }

    private Observacion ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        Observacion record;
        ArrayList TipoPersona;
        ArrayList estatus;
        Enums.Persona persona;
        Enums.Estado estado;

        estatus = Util.RadComboToString(editedItem["EstatusTemp"].FindControl("ComboEstatus"));
        TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));
        if (TipoPersona[0].ToString() == "Moral" || TipoPersona[0].ToString() == "moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
        //if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }
        estado = Enums.Estado.Activo;
        // Extrae todos los elementos

        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {

            if (newValues["Clave"] != null && newValues["descripcion"] != null && newValues["comentario"] != null)
            {
                if (bValidaCampoVacio(newValues["Clave"].ToString(), "Clave") && bValidaCampoVacio(newValues["descripcion"].ToString(), "Descripción") && bValidaCampoVacio(newValues["comentario"].ToString(), "Comentario"))
                {

                    record = new Observacion(0, newValues["Clave"].ToString(), newValues["descripcion"].ToString(), newValues["comentario"].ToString(), estado, persona);
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH se agrega linea 22/01/2013
                    ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog + " " + persona, 1, null, newValues);
                }
                else
                {
                    record = new Observacion(0, "0", "", "", Enums.Estado.Activo, Enums.Persona.Moral);
                }
            }
            else
            {
                record = new Observacion(0, "0", "", "", Enums.Estado.Activo, Enums.Persona.Moral);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
                //Response.Write("<script> alert('Favor de Verificar los Datos Ingresados')</script>");
            }
        }
        else
        {
            record = new Observacion(0, "0", "", "", Enums.Estado.Activo, Enums.Persona.Moral);
        }
        estatus = null;
        TipoPersona = null;
        return record;
    }
    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdObservacion.Columns[0].Visible = false;
        RgdObservacion.Columns[RgdObservacion.Columns.Count - 1].Visible = false;
        RgdObservacion.MasterTableView.HierarchyDefaultExpanded = true;
        RgdObservacion.ExportSettings.OpenInNewWindow = false;
        RgdObservacion.ExportSettings.ExportOnlyData = true;
        RgdObservacion.MasterTableView.GridLines = GridLines.Both;
        RgdObservacion.ExportSettings.IgnorePaging = true;
        RgdObservacion.ExportSettings.OpenInNewWindow = true;
        RgdObservacion.ExportSettings.Pdf.PageHeight = Unit.Parse("250mm");
        RgdObservacion.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdObservacion.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el Catálogo " + catalog + " " + Request.QueryString["Persona"].ToString());

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdObservacion.Columns[0].Visible = false;
        RgdObservacion.Columns[RgdObservacion.Columns.Count - 1].Visible = false;
        RgdObservacion.MasterTableView.HierarchyDefaultExpanded = true;
        RgdObservacion.ExportSettings.OpenInNewWindow = false;
        RgdObservacion.ExportSettings.ExportOnlyData = true;

        RgdObservacion.ExportSettings.IgnorePaging = true;
        RgdObservacion.ExportSettings.OpenInNewWindow = true;
        RgdObservacion.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el Catálogo " + catalog + " " + Request.QueryString["Persona"].ToString());

    }

    protected void RgdObservacion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            item["TipoPersonaTemp"].Text = item["Persona"].Text;
            item["EstatusTemp"].Text = item["Estatus"].Text;
            //item["descripcion"].Text = item["descripcion"].Text;   // se elimina apostrofe JAGH 21/02/13
        }
        else if (e.Item.IsInEditMode)
        {

            GridDataItem items;

            items = (GridDataItem)e.Item;
            RadComboBox comboP;
            comboP = (RadComboBox)items["TipoPersonaTemp"].FindControl("ComboPersona");
            if (Request.QueryString["Persona"].ToString() == "PF")
                comboP.SelectedIndex = 0;
            else
                comboP.SelectedIndex = 1;
            comboP.Enabled = false;

            GridDataItem item;
            RadComboBox comboEstatus;
            RadComboBox comboPersona;
            this.RgdObservacion.MasterTableView.GetColumn("TipoPersonaTemp").Display = true;
            this.RgdObservacion.MasterTableView.GetColumn("Persona").Display = false;
            item = (GridDataItem)e.Item;
            comboPersona = (RadComboBox)item["TipoPersonaTemp"].FindControl("ComboPersona");
            comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);
            if (newValues["Estatus"] != null && newValues["Persona"] != null)
            {
                if (newValues["Estatus"].ToString() == "Activo")
                    comboEstatus.SelectedIndex = 0;
                else
                    comboEstatus.SelectedIndex = 1;
                if (newValues["Persona"].ToString() == "Fisica")
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
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;

        //btnExportPDF.Visible = false;
        //ImageButton1.Visible = false;

        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CVEOBS_M")))
            {
                this.RgdObservacion.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CVEOBS_M")))
            {
                //this.RgdObservacion.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CVEOBS_F")))
            {
                this.RgdObservacion.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CVEOBS_F")))
            {
                //	this.RgdObservacion.MasterTableView.GetColumn("DeleteState").Visible = true;
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
        if (persona == "PF")
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CVEOBS_F")))
            {
                valido = true;
            }
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CVEOBS_M")))
            {
                valido = true;
            }
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
                row1["nombreColumna"] = "Clave";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "5";

                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Descripción";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "200";

                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Comentario";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "200";

                DataRow row4;

                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "Persona";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "20";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);
                dt_metaDataLayout.Rows.Add(row4);


                //List<DbParameter> parametros = new List<DbParameter>();

                DbParameter[] parametros = new DbParameter[7];
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
                parametros[2].ParameterName = "personaApp";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "clvexterna";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 5;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "descripcionp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 200;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "comentariop";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 200;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "personap";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 5;

                string storeBase = "SP_cargaMasiva_clvosb_F";

                dt_layout_procesado = cargaMasiva.cargaMasiva("RadGridExport 2", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));

                int total = RgdObservacion.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog + " " + persona, total + " Registros", numeros + " Registros", catalog, 1);

                RgdObservacion.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catch
        //
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            CheckBox chkHeader;

            chkHeader = (CheckBox)sender;


          
                foreach (GridDataItem row in RgdObservacion.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = chkHeader.Checked;

                    row["TipoPersonaTemp"].Text = row["Persona"].Text;
                    row["EstatusTemp"].Text = row["Estatus"].Text;
                    row["descripcion"].Text = "'" + row["descripcion"].Text;
                }        

        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }//try-catch


        //ChkTodo_CheckedChanged
    }
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdObservacion.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdObservacion.AllowPaging = false;
                RgdObservacion.Rebind();
                foreach (GridDataItem row in RgdObservacion.Items)
                {
                    Observacion ObservacionDelete;
                    int pId = Parser.ToNumber(row["Id"].Text.ToString());
                    string pclave = row["Clave"].Text.ToString();
                    string pDescripcion = row["descripcion"].Text.ToString();
                    string pComentario = row["comentario"].Text.ToString();



                    ObservacionDelete = new Observacion(pId, pclave, pDescripcion, pComentario, Enums.Estado.Activo, (persona == "PF") ? Enums.Persona.Fisica : Enums.Persona.Moral);
                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
                    //ObservacionDataAccess ObservacionBorrar = new ObservacionDataAccess();
                    ClavesObservacion_Rules ObservacionBorrar = new ClavesObservacion_Rules((persona == "PF") ? Enums.Persona.Fisica : Enums.Persona.Moral);
                    if (ObservacionBorrar.Delete(ObservacionDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los Registros se han removido correctamente");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }//foreach
                RgdObservacion.Rebind();
                RgdObservacion.DataSource = null;
                RgdObservacion.AllowPaging = true;
                RgdObservacion.Rebind();
                RgdObservacion.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminacion Masiva del catálogo: "+ catalog, Convert.ToString(RgdObservacion.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdObservacion.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {


                        Observacion ObservacionDelete;
                        Int32 idCuenta = 0;
                        //Datos para identificar valor


                        int pId = Parser.ToNumber(row["Id"].Text.ToString());
                        string pclave = row["Clave"].Text.ToString();
                        string pDescripcion = row["descripcion"].Text.ToString();
                        string pComentario = row["comentario"].Text.ToString();



                        ObservacionDelete = new Observacion(pId, pclave, pDescripcion, pComentario, Enums.Estado.Activo, (persona == "PF") ? Enums.Persona.Fisica : Enums.Persona.Moral);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion

                        ClavesObservacion_Rules ObservacionBorrar = new ClavesObservacion_Rules((persona == "PF") ? Enums.Persona.Fisica : Enums.Persona.Moral);
                        if (ObservacionBorrar.Delete(ObservacionDelete) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + pId, null, null, catalog + " " + persona, 1, null, null);
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
            Response.Write(exep.Message.ToString());
        }//try-catch
        //btn_eliminar_Click
        RgdObservacion.Rebind();

    }

    // JAGH conservar nombres de filtros en español al filtrar dato
    protected void grids_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //sobre filtro y paginacion
        if (e.CommandName == RadGrid.FilterCommandName || e.CommandName == RadGrid.PageCommandName || e.CommandName.Equals("ChangePageSize") ||
             e.CommandName == RadGrid.PrevPageCommandArgument || e.CommandName == RadGrid.NextPageCommandArgument ||
             e.CommandName == RadGrid.FirstPageCommandArgument || e.CommandName == RadGrid.LastPageCommandArgument)
        {
            CambiaAtributosRGR();
        }

    }

}
