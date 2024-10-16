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

public partial class CreditosExceptuadosPage : System.Web.UI.Page
{
    public const String catalog = "Creditos Exceptuados";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {

                CambiaAtributosRGR();
                string persona = Request.QueryString["Persona"].ToString();
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {                   
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó a Catálogo " + catalog);
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario Válido", "~/Login.aspx");

            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);

        }

    }
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdExceptuados.FilterMenu;
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
    protected void RgdExceptuados_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        Exceptuados_Rules getRecords = null;
        List<Exceptuado> ExceptuadosInfo;
        try
        {
            getRecords = new Exceptuados_Rules();
            ExceptuadosInfo = getRecords.GetRecords(false);
            RgdExceptuados.DataSource = ExceptuadosInfo;
            RgdExceptuados.VirtualItemCount = ExceptuadosInfo.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
    }
    protected void RgdExceptuados_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdExceptuados_UpdateCommand(object source, GridCommandEventArgs e)
    {

        int IdValue = 0;
        string Credito = string.Empty;
        string Motivo = string.Empty;
        //ArrayList estatus;
        Enums.Estado estado;
        Exceptuado ExceptuadosOld;
        Exceptuado ExceptuadosNew;


        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        try
        {

            if ((newValues["Credito"] != null) && (newValues["Motivo"] != null))
            {

                if (bValidaCampoVacio(newValues["Motivo"].ToString(), "Motivo", e))
                {

                    //Datos Orginales
                    IdValue = Convert.ToInt32(item.SavedOldValues["Id"]);
                    //Datos para actualizar
                    Credito = newValues["Credito"].ToString();
                    Motivo = newValues["Motivo"].ToString();

                    //estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));
                    estado = Enums.Estado.Activo;
                    //if (estatus[0].ToString() == "Activo" || estatus[0].ToString() == "activo") { estado = Enums.Estado.Activo; } else if (estatus[0].ToString() == ("Inactivo") || estatus[0].ToString() == ("inactivo")) { estado = Enums.Estado.Inactivo; } else { estado = Enums.Estado.Test; }
                    ExceptuadosOld = new Exceptuado(IdValue, Credito, Motivo, estado);
                    ExceptuadosNew = new Exceptuado(IdValue, Credito, Motivo, estado);

                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
                    Exceptuados_Rules ExceptuadosUpdate = new Exceptuados_Rules();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    if (ExceptuadosUpdate.Update(ExceptuadosOld, ExceptuadosNew) > 0)
                    {
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Registro no fue Modificado");
                    }
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de Verificar los Datos Ingresados");
                e.Canceled = true;
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdExceptuados_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdExceptuados_DeleteCommand(object source, GridCommandEventArgs e)
    {
        Exceptuado ExceptuadoDelete;
        int IdExc = 0;
        GridDataItem item = (GridDataItem)e.Item;
        try
        {
            IdExc = Convert.ToInt32(item["Id"].Text);
            ExceptuadoDelete = new Exceptuado(IdExc, "", "", Enums.Estado.Activo);
            //Aqui se llama el SP correspondiente para eliminar
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            Exceptuados_Rules ExceptuadoBorrar = new Exceptuados_Rules();

            if (ExceptuadoBorrar.Delete(ExceptuadoDelete) > 0)
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro", null, null, catalog, 1, null, null);
                //Response.Write("<script> alert('El Registro fue removido Correctamente'); </script>");
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue Removido");
                //Response.Write("<script> alert('El Registro no fue Removido'); </script>");
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }

    }
    protected void RgdExceptuados_InsertCommand(object source, GridCommandEventArgs e)
    {
        InserCreditoExceptuado(e);
    }

    private void InserCreditoExceptuado(GridCommandEventArgs e)
    {
        //ExceptuadosDataAccess ExceptuadoInsertar;
        Exceptuados_Rules ExceptuadoInsertar;
        try
        {
            Exceptuado record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {
                if (bValidaCampoVacio(record.Motivo,"Motivo",e) )
                {
                    ExceptuadoInsertar = new Exceptuados_Rules();
                    if (ExceptuadoInsertar.Insert(record) > 0)
                    {
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
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
    private Exceptuado ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        Exceptuado record;
        //ArrayList estatus;
        Enums.Estado estado;
        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {

            if (newValues["Credito"] != null && newValues["Motivo"] != null)
            {
                estado = new Enums.Estado();
                int Id = Convert.ToInt32(newValues["Id"]);
                string credito = newValues["Credito"].ToString();
                string motivo = newValues["Motivo"].ToString();
                //estatus = Util.RadComboToString(editedItem["EstatusTemp"].FindControl("ComboEstatus"));

                //if (estatus[0].ToString() == "Activo" || estatus[0].ToString() == "activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }
                estado = Enums.Estado.Activo;
                record = new Exceptuado(Id, credito, motivo, estado);
            }
            else
            {
                record = new Exceptuado(0, "", "", Enums.Estado.Inactivo);

                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de Verificar los Datos Ingresados");
                //Response.Write("<script> alert('Favor de Verificar los Datos Ingresados')</script>");
            }
        }
        else
        {
            record = new Exceptuado(0, "", "", Enums.Estado.Inactivo);
        }
        return record;
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

    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdExceptuados.MasterTableView.HierarchyDefaultExpanded = true;
        RgdExceptuados.ExportSettings.OpenInNewWindow = false;
        RgdExceptuados.ExportSettings.ExportOnlyData = true;
        RgdExceptuados.MasterTableView.GridLines = GridLines.Both;
        RgdExceptuados.ExportSettings.IgnorePaging = true;
        RgdExceptuados.ExportSettings.OpenInNewWindow = true;
        RgdExceptuados.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);



    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";

        RgdExceptuados.MasterTableView.HierarchyDefaultExpanded = true;
        RgdExceptuados.ExportSettings.OpenInNewWindow = false;
        RgdExceptuados.ExportSettings.ExportOnlyData = true;

        RgdExceptuados.ExportSettings.IgnorePaging = true;
        RgdExceptuados.ExportSettings.OpenInNewWindow = true;
        RgdExceptuados.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdExceptuados_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                item["EstatusTemp"].Text = item["Estatus"].Text;
            }
            else if (e.Item.IsInEditMode) //(e.Item.ItemType == GridItemType.CommandItem)
            {

                GridDataItem item;
                RadComboBox comboEstatus;

                item = (GridDataItem)e.Item;
                comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);
                if (newValues["Estatus"] != null)
                {
                    if (newValues["Estatus"].ToString() == "Activo")
                        comboEstatus.SelectedIndex = 0;
                    else
                        comboEstatus.SelectedIndex = 1;
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
        this.RgdExceptuados.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        this.RgdExceptuados.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_EXCEPTUADO_F")))
            {
                this.RgdExceptuados.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_EXCEPTUADO_F")))
            {
                this.RgdExceptuados.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_EXCEPTUADO_F")))
            {
                this.RgdExceptuados.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_EXCEPTUADO_F")))
            {
                this.RgdExceptuados.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario Válido", "~/Login.aspx");
        }
    }

    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_EXCEPTUADO_F")))
        {
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_EXCEPTUADO_M")))
        {
            valido = true;
        }


        return valido;
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
                foreach (GridDataItem row in RgdExceptuados.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdExceptuados.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = false;
                }
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

    }
    protected void btn_cargar_Click(object sender, EventArgs e)
    {

    }
}