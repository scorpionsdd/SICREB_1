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
using Oracle.DataAccess.Client;

public partial class RequestEditPage : System.Web.UI.Page
{
    //MASS 05/07/13
    public const String catalog = "Request Edit";        
    int idUs;
    string SeleccionTipo = "0";    

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CambiaAtributosRGR();

            if (Session["Facultades"] != null)
            {
                getFacultades("n/a");
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }

            if (!this.Page.IsPostBack)
            {                
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó a Catálogo " + catalog);
            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }

    }

    protected void RgdCuentas_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {        
        Request_Edit_Rules getRecords = null;
        List<Request_Edit> cuentasInfo;
        List<string> lsCodigo = new List<string>();
        RgdCuentas.EnableLinqExpressions = false;

        try
        {
            getRecords = new Request_Edit_Rules();
            cuentasInfo = getRecords.GetRecords(false);
            RgdCuentas.DataSource = cuentasInfo;
            RgdCuentas.VirtualItemCount = cuentasInfo.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }

        Request_EditDataAccess r_edit = new Request_EditDataAccess();
        string estado_actual = r_edit.Estado_actual();

        if (estado_actual != "COMPLETO" && estado_actual != "ERROR")
        {
            btn_resetear.Visible = true;
        }
        else
        {
            btn_resetear.Visible = false;
        }
    }

    //JAGH se modifica para mostrar titulos filtros
    public void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdCuentas.FilterMenu;
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

    protected void RgdCuentas_CancelCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {        
    }    

    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        //RgdCuentas.Columns[0].Visible = false;
        //RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
        RgdCuentas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = false;
        RgdCuentas.ExportSettings.ExportOnlyData = true;

        RgdCuentas.MasterTableView.GridLines = GridLines.Both;
        RgdCuentas.ExportSettings.IgnorePaging = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = true;
        RgdCuentas.ExportSettings.Pdf.PageHeight = Unit.Parse("250mm");
        RgdCuentas.ExportSettings.Pdf.PageWidth = Unit.Parse("550mm");
        RgdCuentas.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        //RgdCuentas.Columns[0].Visible = false;
        //RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
        RgdCuentas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = false;
        RgdCuentas.ExportSettings.ExportOnlyData = true;

        RgdCuentas.ExportSettings.IgnorePaging = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = true;
        RgdCuentas.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdCuentas_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
            }
            else if (e.Item.IsInEditMode)
            {
                GridDataItem items;
                items = (GridDataItem)e.Item;               
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                RadComboBox combo = (RadComboBox)filterItem.FindControl("FiltroRadCombo");
                combo.SelectedValue = SeleccionTipo;
            }

            if (e.Item is GridCommandItem)
            {                
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
        this.RgdCuentas.MasterTableView.GetColumn("EditCommandColumn").Visible = false;        
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MCCTAS")))
            {
                this.RgdCuentas.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ECCTAS")))
            {                
                valido = true;
            }

            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MCCTAS")))
            {
                this.RgdCuentas.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ECCTAS")))
            {                
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AACTAS")))
        {
            valido = true;
        }
        return valido;
    }    

    protected void btn_resetear_Click(object sender, EventArgs e)
    {
        try
        {
            Request_Edit_Rules CuentaValores = new Request_Edit_Rules();

            Request_Edit cuentaOld = new Request_Edit(0, "", "", "", 0, 0, "", "", "");
            Request_Edit cuentaNew = new Request_Edit(0, "", "", "ERROR", 0, 0, "", "", "");

            Hashtable oldValues = new Hashtable();
            Hashtable newValues = new Hashtable();

            Request_EditDataAccess r_edit = new Request_EditDataAccess();
            string estado_actual = r_edit.Estado_actual();
            
            if (CuentaValores.Update(cuentaOld, cuentaNew) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se reseteo correctamente");
                ActividadRules.GActividadCatalogo(8888, idUs, "Reseteo de Registro", "ESTADO=" + estado_actual, "ESTADO=ERROR", catalog, 1, oldValues, newValues);
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
            }            

        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }

        RgdCuentas.Rebind();

    }

    protected void FiltroRadCombo_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (e.Value.ToString() != "0")
        {
            SeleccionTipo = e.Value.ToString();
            GridColumn column = RgdCuentas.MasterTableView.GetColumnSafe("estado");
            column.CurrentFilterFunction = GridKnownFunction.EqualTo;
            string filterExpression = "[estado] = '" + e.Value + "'";
            RgdCuentas.MasterTableView.FilterExpression = filterExpression;
            RgdCuentas.MasterTableView.Rebind();
        }
        else
        {
            SeleccionTipo = e.Value.ToString();
            GridColumn column = RgdCuentas.MasterTableView.GetColumnSafe("estado");
            column.CurrentFilterFunction = GridKnownFunction.DoesNotContain;
            RgdCuentas.MasterTableView.FilterExpression = "[estado] <> 'undefined'";
            RgdCuentas.MasterTableView.Rebind();
        }
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