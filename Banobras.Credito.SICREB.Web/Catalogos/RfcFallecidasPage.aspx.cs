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
using Banobras.Credito.SICREB.Data;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Text.RegularExpressions;

public partial class RfcFallecidasPage : System.Web.UI.Page
{
    public const String catalog = "Defunción";
    int idUs;
    protected void Page_Load(object sender, EventArgs e)
    {
        CambiaAtributosRGR(); 

        try
        {
            if (Session["Facultades"] != null)
            {
                string persona = "PM";
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                    //CambiaAtributosRGR();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó al Catálogo " + catalog);

                    
                    //RgdRfcFallecido.Columns.FindByDataField("Fecha"). = System.DateTime.Now.AddDays(-30);
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

    //JAGH se modifica para mostrar titulos filtros
    public void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdRfcFallecido.FilterMenu;
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
    

    protected void RgdRfcFallecido_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RfcFallecidasRules getRecords;

        List<PersonasFallecidas> RfcFallecidasInfo;

        List<string> lsRFC = new List<string>();
        try
        {
            getRecords = new RfcFallecidasRules();
            RfcFallecidasInfo = getRecords.GetRecords(false);
            RgdRfcFallecido.DataSource = RfcFallecidasInfo;
            RgdRfcFallecido.VirtualItemCount = RfcFallecidasInfo.Count;
            ((Telerik.Web.UI.GridDateTimeColumn)(RgdRfcFallecido.Columns[6])).MaxDate = DateTime.Now;
            for (int i = 0; i < RfcFallecidasInfo.Count; i++)
            {
                lsRFC.Add(RfcFallecidasInfo[i].Rfc);
            }


            ViewState["RFC"] = lsRFC;

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdRfcFallecido_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;

    }

    /*   protected void RgdRfcFallecido_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
       {

           GridDataItem item = (GridDataItem)e.Item;
     Hashtable oldValues = (Hashtable)item.SavedOldValues;

       } */
    protected void RgdRfcFallecido_UpdateCommand(object source, GridCommandEventArgs e)
    {
        string rfc = string.Empty;
        //SICREB-INICIO ACA Sep-2012
        string nombre = string.Empty;
        string credito = string.Empty;
        string auxiliar = string.Empty;
        //SICREB-FIN ACA Sep-2012
        DateTime fecha = DateTime.Now;
        ArrayList estatus;
        Enums.Estado estado;
        Int32 idValue = 0;
        PersonasFallecidas oldRfcFallecidas;
        PersonasFallecidas newRfcFallecidas;
        bool bValida = true;


        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        string RFCold = oldValues["Rfc"].ToString();
        try
        {
            if (newValues["Rfc"] != null && newValues["Fecha"] != null && newValues["Nombre"] != null && newValues["Credito"] != null && newValues["Auxiliar"] != null)            
            {
                if (bValidaCampoVacio(newValues["Rfc"].ToString(), "RFC", e) && bValidaCampoVacio(newValues["Fecha"].ToString(), "Fecha", e) && bValidaCampoVacio(newValues["Credito"].ToString(), "Credito", e) && bValidaCampoVacio(newValues["Auxiliar"].ToString(), "Auxiliar", e) && bValidaCampoVacio(newValues["Nombre"].ToString(), "Nombre", e))
                {
                    //Datos Orginales
                    idValue = Parser.ToNumber(item.SavedOldValues["Id"]);
                    //Datos Nuevos
                    rfc = newValues["Rfc"].ToString().Trim();

                    if (RFCold != rfc)
                    {
                        if (bValidaFormatoRFC(rfc,e))
                        {

                            bValida = bValidaRFC(rfc, e);
                        }
                        else
                        {
                            bValida = false;
                        }
                    }

                    if (bValida)
                    {
                        //SICREB-INICIO ACA Sep-2012
                        nombre = newValues["Nombre"].ToString().Trim();
                        credito = newValues["Credito"].ToString().Trim();
                        auxiliar = newValues["Auxiliar"].ToString().Trim();
                        //SICREB-FIN ACA Sep-2012
                        //dtp = (RadDatePicker)item["FechaTemp"].FindControl("dtpFecha");
                        estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));
                        //fecha = Parser.ToDateTime(dtp.SelectedDate);
                        fecha = Convert.ToDateTime(newValues["Fecha"].ToString());
                        if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else if (estatus[0].ToString() == "Inactivo") { estado = Enums.Estado.Inactivo; } else { estado = Enums.Estado.Test; }


                        //oldRfcFallecidas = new PersonasFallecidas(idValue, rfc, fecha, estado);
                        //newRfcFallecidas = new PersonasFallecidas(idValue, rfc, fecha, estado);

                        //SICREB-INICIO ACA Sep-2012
                        oldRfcFallecidas = new PersonasFallecidas(idValue, rfc, nombre, credito, auxiliar, fecha, estado);
                        newRfcFallecidas = new PersonasFallecidas(idValue, rfc, nombre, credito, auxiliar, fecha, estado);
                        //SICREB-FIN ACA Sep-2012

                        //Llamar el SP correspondiente con las entidades old y new
                        RfcFallecidasRules RfcValores = new RfcFallecidasRules();

                        if (RfcValores.Update(oldRfcFallecidas, newRfcFallecidas) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            //SICREB-INICIO ACA Sep-2012

                            //SICREB-FIN ACA Sep-2012
                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                            //this.RgdRfcFallecido.MasterTableView.GetColumn("FechaTemp").Visible = false;
                            //this.RgdRfcFallecido.MasterTableView.GetColumn("Fecha").Visible = true;
                            ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);

                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                            //this.RgdRfcFallecido.MasterTableView.GetColumn("FechaTemp").Visible = false;
                            //this.RgdRfcFallecido.MasterTableView.GetColumn("Fecha").Visible = true;
                        }
                    }
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
                e.Canceled = true;
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
        estatus = null;
    }

    //protected void RgdRfcFallecido_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    //{


    //}
    protected void RgdRfcFallecido_DeleteCommand(object source, GridCommandEventArgs e)
    {
        PersonasFallecidas RfcDelete;
        Int32 idRfc = 0;

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;

        try
        {
            //Datos para identificar valor
            idRfc = Convert.ToInt32(item["Id"].Text);
            //SICREB-INICIO ACA Sep-2012
            RfcDelete = new PersonasFallecidas(idRfc, string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, Enums.Estado.Activo);  // <--- MOD
            //SICREB-INICIO ACA Sep-2012

            //Llamar el SP correspondiente con las entidades old y new de Validacion
            RfcFallecidasRules RfcBorrar = new RfcFallecidasRules();

            if (RfcBorrar.Delete(RfcDelete) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro fue removido correctamente");
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdRfcFallecido_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertRfcFallecida(e);
    }
    private void InsertRfcFallecida(GridCommandEventArgs e)
    {
        RfcFallecidasRules rfcInsertar;
        try
        {
            PersonasFallecidas record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion
                if (bValidaCampoVacio(record.Rfc, "RFC", e) && bValidaCampoVacio(record.Fecha.ToString(), "Fecha", e) && bValidaCampoVacio(record.Credito, "Credito", e) && bValidaCampoVacio(record.Auxiliar, "Auxiliar", e) && bValidaCampoVacio(record.Nombre, "Nombre", e))
                {
                    if (bValidaFormatoRFC(record.Rfc,e))
                    {

                        if (bValidaRFC(record.Rfc, e))
                        {

                            rfcInsertar = new RfcFallecidasRules();

                            //TMR. verifica tipo de operación
                            int iTipoOperación = rfcInsertar.Insert(record);

                            if (iTipoOperación > 0)
                            {
                                //TMR
                                if (iTipoOperación == 1)
                                {
                                    //SICREB-INICIO ACA Sep-2012
                                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                                }
                                else if (iTipoOperación == 2)
                                {
                                    //TMR
                                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro ya existe y fue actualizado.");
                                }
                            }
                            else
                            {
                                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                            }
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }


    private bool bValidaRFC(string sRFC, Telerik.Web.UI.GridCommandEventArgs e)
    {

        bool Valido = true;

        List<string> lsSic = new List<string>();

        lsSic = (List<string>)ViewState["RFC"];

        if (lsSic.FindIndex(s => s == sRFC) >= 0)
        {
            Valido = false;

            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El RFC : " + sRFC + " ya se encuentra dado de alta.");

            e.Canceled = true;
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

    private PersonasFallecidas ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        PersonasFallecidas record;
        string rfc = string.Empty;
        //SICREB-INICIO ACA Sep-2012
        string nombre = string.Empty;
        string credito = string.Empty;
        string auxiliar = string.Empty;
        //SICREB-FIN ACA Sep-2012
        Enums.Estado estado;
        DateTime fecha;
      

        //dtp = (RadDatePicker)editedItem["FechaTemp"].FindControl("dtpFecha");
        //fecha = Parser.ToDateTime(dtp.SelectedDate);

        estado = Enums.Estado.Activo;
        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);    

        if (newValues.Count > 0)
        {

            if (newValues["Rfc"] != null && newValues["Fecha"] != null && newValues["Nombre"] != null && newValues["Credito"] != null && newValues["Auxiliar"] != null)
            {

                fecha = Convert.ToDateTime(newValues["Fecha"].ToString());
                rfc = newValues["Rfc"].ToString();
                nombre = newValues["Nombre"].ToString();
                credito = newValues["Credito"].ToString();
                auxiliar = newValues["Auxiliar"].ToString();
                record = new PersonasFallecidas(0, rfc, nombre, credito, auxiliar, fecha, estado);    // <--- MOD   < ALEX >
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, newValues);

            }
            else
            {
                record = new PersonasFallecidas(0, string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, Enums.Estado.Activo); // <--- MOD   < ALEX >
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            record = new PersonasFallecidas(0, string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, Enums.Estado.Activo);  // <--- MOD   < ALEX >
        }
        //this.RgdRfcFallecido.MasterTableView.GetColumn("FechaTemp").Visible = false;
        //this.RgdRfcFallecido.MasterTableView.GetColumn("Fecha").Visible = true;
        return record;
    }

    public bool bValidaFormatoRFC(string RFC, Telerik.Web.UI.GridCommandEventArgs e)
    {
        string modelo = @"^[a-zA-Z&]{3,4}(\d{6})((\D|\d){3})?$";

        Regex re = new Regex(modelo);

        if (re.IsMatch(RFC))

            return true;

        else
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de introducir un RFC valido");

            return false;
        }
    }

    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdRfcFallecido.Columns[0].Visible = false;
        RgdRfcFallecido.Columns[RgdRfcFallecido.Columns.Count - 1].Visible = false;
        RgdRfcFallecido.MasterTableView.HierarchyDefaultExpanded = true;
        RgdRfcFallecido.ExportSettings.OpenInNewWindow = false;
        RgdRfcFallecido.ExportSettings.ExportOnlyData = true;
        RgdRfcFallecido.MasterTableView.GridLines = GridLines.Both;
        RgdRfcFallecido.ExportSettings.IgnorePaging = true;
        RgdRfcFallecido.ExportSettings.OpenInNewWindow = true;
        RgdRfcFallecido.ExportSettings.Pdf.PageHeight = Unit.Parse("300mm");
        RgdRfcFallecido.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdRfcFallecido.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);


    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdRfcFallecido.Columns[0].Visible = false;
        RgdRfcFallecido.Columns[RgdRfcFallecido.Columns.Count - 1].Visible = false;
        RgdRfcFallecido.MasterTableView.HierarchyDefaultExpanded = true;
        RgdRfcFallecido.ExportSettings.OpenInNewWindow = false;
        RgdRfcFallecido.ExportSettings.ExportOnlyData = true;

        RgdRfcFallecido.ExportSettings.IgnorePaging = true;
        RgdRfcFallecido.ExportSettings.OpenInNewWindow = true;
        RgdRfcFallecido.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdRfcFallecido_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadDatePicker dtp;
        try
        {

            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {

                GridDataItem item = (GridDataItem)e.Item;
                Hashtable oldValues = (Hashtable)item.SavedOldValues;
                item["EstatusTemp"].Text = item["Estatus"].Text;
                //dtp = (RadDatePicker)item["FechaTemp"].FindControl("dtpFecha");
                //dtp.SelectedDate = Parser.ToDateTime(item["Fecha"].Text);
                item["Auxiliar"].Text = string.Format("{0:0}", item["Auxiliar"].Text);
            }
            else if (e.Item.ItemType == GridItemType.FilteringItem)
            {
                
                ((Telerik.Web.UI.GridDateTimeColumn)(((Telerik.Web.UI.GridItem)(e.Item)).OwnerTableView.Columns[6])).MaxDate = DateTime.Now; 
            }
            
            if (e.Item.IsInEditMode)
            {

                GridDataItem item;
                RadComboBox comboEstatus;
                //this.RgdRfcFallecido.MasterTableView.GetColumn("FechaTemp").Visible = true;
                //this.RgdRfcFallecido.MasterTableView.GetColumn("Fecha").Visible = false;
                //e.Item.Cells[7].Visible = true;
                //e.Item.Cells[6].Visible = false;
                
                item = (GridDataItem)e.Item;
                comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);
                if (newValues["Estatus"] != null && newValues["Fecha"] != null)
                {
                    if (newValues["Estatus"].ToString() == "Activo")
                        comboEstatus.SelectedIndex = 0;
                    else
                        comboEstatus.SelectedIndex = 1;

                    //dtp = (RadDatePicker)item["FechaTemp"].FindControl("dtpFecha");
                    //dtp.SelectedDate = Parser.ToDateTime(newValues["Fecha"].ToString());
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
        this.RgdRfcFallecido.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdRfcFallecido.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_FALLECIDAS")))
            {
                this.RgdRfcFallecido.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_FALLECIDAS")))
            {
                //  this.RgdRfcFallecido.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_FALLECIDAS")))
            {
                this.RgdRfcFallecido.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_FALLECIDAS")))
            {
                // this.RgdRfcFallecido.MasterTableView.GetColumn("DeleteState").Visible = true;
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_FALLECIDAS")))
        {
            valido = true;
        }
        return valido;
    }
    protected void FiltroDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        DateTime fechaFiltro;
        RadDatePicker rdp = (RadDatePicker)sender;
        try
        {
            string filterExpression;
            fechaFiltro = (DateTime)rdp.DbSelectedDate;

            GridColumn column = RgdRfcFallecido.MasterTableView.GetColumnSafe("Fecha");
            column.CurrentFilterFunction = GridKnownFunction.EqualTo;
            filterExpression = "([Fecha] = '" + fechaFiltro.ToShortDateString().ToString() + "')";
            //TMR se agrega esta instrucción para desaparecer un mensaje de dato inesperado
            RgdRfcFallecido.EnableLinqExpressions = false;
            RgdRfcFallecido.MasterTableView.FilterExpression = filterExpression;
            RgdRfcFallecido.MasterTableView.Rebind();
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
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
                row1["nombreColumna"] = "RFC";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "38";

                DataRow row2;
                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "NOMBRE";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "38";

                DataRow row3;
                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "CREDITO";  //JAGH 20/02/13 se elimina acento para coincidir con layout (excel)
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "30";

                DataRow row4;
                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "AUXILIAR";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "30";

                DataRow row5;
                row5 = dt_metaDataLayout.NewRow();
                row5["nombreColumna"] = "FECHA";
                row5["tipoDato"] = "VARCHAR2";
                row5["longitud"] = "30";
                
                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);
                dt_metaDataLayout.Rows.Add(row4);
                dt_metaDataLayout.Rows.Add(row5);

                DbParameter[] parametros = new DbParameter[8];

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
                parametros[2].ParameterName = "ppersona";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "rfcp";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "nombrep";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 30;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "creditop";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 30;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "auxiliarp";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 30;

                parametros[7] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].ParameterName = "fechap";
                parametros[7].DbType = DbType.String;
                parametros[7].Size = 30;

                string storeBase = "SP_CARGAMASIVA_FALLECIDAS";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_fallecidas", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "PF");
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'));
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros, //((numeros > 0) ? (numeros - 1) : numeros),
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdRfcFallecido.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdRfcFallecido.Rebind();

            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'));

            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
            //RgdRfcFallecido.Rebind();
            //   Mensajes.ShowMessage(this.Page,this.GetType(),cargaMasiva.Log.ToString());
        }//try-Catch
    }
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;

            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdRfcFallecido.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdRfcFallecido.AllowPaging = false;
                RgdRfcFallecido.Rebind();
                foreach (GridDataItem row in RgdRfcFallecido.Items)
                {
                    PersonasFallecidas RfcDelete;
                    Int32 idRfc = 0;

                    idRfc = Convert.ToInt32(row["Id"].Text);
                    //SICREB-INICIO ACA Sep-2012
                    RfcDelete = new PersonasFallecidas(idRfc, string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, Enums.Estado.Activo);  // <--- MOD
                    //SICREB-INICIO ACA Sep-2012

                    //Llamar el SP correspondiente con las entidades old y new de Validacion
                    RfcFallecidasRules RfcBorrar = new RfcFallecidasRules();

                    if (RfcBorrar.Delete(RfcDelete) > 0)
                    {


                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }//foreach
                RgdRfcFallecido.Rebind();
                RgdRfcFallecido.DataSource = null;
                RgdRfcFallecido.AllowPaging = true;
                RgdRfcFallecido.Rebind();
                RgdRfcFallecido.DataBind();
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdRfcFallecido.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdRfcFallecido.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {
                        PersonasFallecidas RfcDelete;
                        Int32 idRfc = 0;

                        idRfc = Convert.ToInt32(row["Id"].Text);
                        //SICREB-INICIO ACA Sep-2012
                        RfcDelete = new PersonasFallecidas(idRfc, string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, Enums.Estado.Activo);  // <--- MOD
                        //SICREB-INICIO ACA Sep-2012

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        RfcFallecidasRules RfcBorrar = new RfcFallecidasRules();

                        if (RfcBorrar.Delete(RfcDelete) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con RFC " + idRfc, null, null, catalog, 1, null, null);
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                        }
                    }//checket

                }//foreach Items


            }
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch
        //btn_eliminar_Click
        RgdRfcFallecido.Rebind();


        //btn_eliminar_Click
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
                foreach (GridDataItem row in RgdRfcFallecido.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdRfcFallecido.Items)
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

        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ((Telerik.Web.UI.GridDateTimeColumn)(((Telerik.Web.UI.GridItem)(e.Item)).OwnerTableView.Columns[6])).MaxDate = DateTime.Now;
        }

    }
}
