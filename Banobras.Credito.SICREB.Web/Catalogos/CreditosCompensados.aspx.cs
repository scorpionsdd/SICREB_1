using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Telerik.Web.UI;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Business.Repositorios;

public partial class CreditosCompensados : System.Web.UI.Page
{

    public const String catalog = "Créditos Compensados";  
    CreditoCompensado_Rules ObjCreditos = new CreditoCompensado_Rules();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CambiaAtributosRGR();
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

    protected void RgdCreditosC_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        CreditoCompensado_Rules getRecords = null;
        List<CreditoCompensado> CreditoCompensadoInfo;
        try
        {
            getRecords = new CreditoCompensado_Rules();
            CreditoCompensadoInfo = getRecords.GetRecords(false);
            RgdCreditosC.DataSource = CreditoCompensadoInfo;
            RgdCreditosC.VirtualItemCount = CreditoCompensadoInfo.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }

    }

    protected void RgdCreditosC_InsertCommand(object source, GridCommandEventArgs e)
    {

        try
        {
            GridEditableItem editedItem = (GridEditableItem)e.Item;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);
            if (newValues["CREDITO"] != null)
            {
                CreditoCompensado CreditoNew = new CreditoCompensado(0, newValues["CREDITO"].ToString(),
                                                                        newValues["RFC"].ToString(),
                                                                        newValues["ACREDITADO"].ToString(),
                                                                        newValues["INFORMACION"].ToString(),
                                                                        Enums.Estado.Activo);

                ObjCreditos.Insert(CreditoNew);
                Mensajes.ShowMessage(this.Page, this.GetType(), "El Credito se agrego correctamente.");
            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "El campo de Credito es obligatorio.");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowMessage(this.Page, this.GetType(), ex.Message.ToString());
        }

    }

    protected void RgdCreditosC_UpdateCommand(object source, GridCommandEventArgs e)
    {

        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            if (oldValues["CREDITO"].ToString() == newValues["CREDITO"].ToString())
            {

                CreditoCompensado CreditoOld = new CreditoCompensado( Int32.Parse(oldValues["ID_CREDITO"].ToString()),
                                                                      oldValues["CREDITO"].ToString(),
                                                                      oldValues["RFC"].ToString(),
                                                                      oldValues["ACREDITADO"].ToString(),
                                                                      oldValues["INFORMACION"].ToString(),
                                                                      Enums.Estado.Activo);

                CreditoCompensado CreditoNew = new CreditoCompensado( Int32.Parse(oldValues["ID_CREDITO"].ToString()),
                                                                      newValues["CREDITO"].ToString(),
                                                                      newValues["RFC"].ToString(),
                                                                      newValues["ACREDITADO"].ToString(),
                                                                      newValues["INFORMACION"].ToString(),
                                                                      Enums.Estado.Activo);

                ObjCreditos.Update(CreditoOld, CreditoNew);
                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se actualizo correctamente");

            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "No se debe de modificar el número de Crédito");
            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowMessage(this.Page, this.GetType(), ex.Message.ToString());
        }

    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 idUsuario;
            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdCreditosC.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());

            foreach (GridDataItem row in RgdCreditosC.Items)
            {
                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkResult = (bool)chkCell.Checked;
                if (chkResult)
                {
                    CreditoCompensado CreditoDelete = new CreditoCompensado(Int32.Parse(row.GetDataKeyValue("ID_CREDITO").ToString()), "", "", "", "", Enums.Estado.Inactivo);
                    ObjCreditos.Delete(CreditoDelete);

                    idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                    if (chkHeader.Checked != true)
                    {
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido correctamente");
                        ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + CreditoDelete.Id_Credito, null, null, catalog, 1, null, null);
                    }

                }
            }

            if (chkHeader.Checked == true)
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "Los Registros fueron removidos correctamente.");
            }

        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }

        RgdCreditosC.Rebind();
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkCell;
        CheckBox chkHeader;
        chkHeader = (CheckBox)sender;

        foreach (GridDataItem row in RgdCreditosC.Items)
        {
            chkCell = (CheckBox)row.Cells[0].FindControl("chk");
            chkCell.Checked = chkHeader.Checked;
        }
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdCreditosC.Columns[0].Visible = false;
        RgdCreditosC.Columns[RgdCreditosC.Columns.Count - 1].Visible = false;
        RgdCreditosC.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCreditosC.ExportSettings.OpenInNewWindow = false;
        RgdCreditosC.ExportSettings.ExportOnlyData = true;
        RgdCreditosC.MasterTableView.GridLines = GridLines.Both;
        RgdCreditosC.ExportSettings.IgnorePaging = true;
        RgdCreditosC.ExportSettings.OpenInNewWindow = true;
        RgdCreditosC.ExportSettings.Pdf.PageHeight = Unit.Parse("250mm");
        RgdCreditosC.ExportSettings.Pdf.PageWidth = Unit.Parse("350mm");
        RgdCreditosC.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }

    protected void btnExportarXLS_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdCreditosC.Columns[0].Visible = false;
        RgdCreditosC.Columns[RgdCreditosC.Columns.Count - 1].Visible = false;
        RgdCreditosC.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCreditosC.ExportSettings.OpenInNewWindow = false;
        RgdCreditosC.ExportSettings.ExportOnlyData = true;
        RgdCreditosC.ExportSettings.IgnorePaging = true;
        RgdCreditosC.ExportSettings.OpenInNewWindow = true;
        RgdCreditosC.MasterTableView.ExportToExcel();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }

    private void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdCreditosC.FilterMenu;
        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (menu.Items[i].Text == "NoFilter")             { menu.Items[i].Text = "Sin Filtro"; }
            if (menu.Items[i].Text == "EqualTo")              { menu.Items[i].Text = "Igual"; }
            if (menu.Items[i].Text == "NotEqualTo")           { menu.Items[i].Text = "Diferente"; }
            if (menu.Items[i].Text == "GreaterThan")          { menu.Items[i].Text = "Mayor que"; }
            if (menu.Items[i].Text == "LessThan")             { menu.Items[i].Text = "Menor que"; }
            if (menu.Items[i].Text == "GreaterThanOrEqualTo") { menu.Items[i].Text = "Mayor o igual a"; }
            if (menu.Items[i].Text == "LessThanOrEqualTo")    { menu.Items[i].Text = "Menor o igual a"; }
            if (menu.Items[i].Text == "Between")              { menu.Items[i].Text = "Entre"; }
            if (menu.Items[i].Text == "NotBetween")           { menu.Items[i].Text = "No entre"; }
            if (menu.Items[i].Text == "IsNull")               { menu.Items[i].Text = "Es nulo"; }
            if (menu.Items[i].Text == "NotIsNull")            { menu.Items[i].Text = "No es nulo"; }
            if (menu.Items[i].Text == "Contains")             { menu.Items[i].Text = "Contenga"; }
            if (menu.Items[i].Text == "DoesNotContain")       { menu.Items[i].Text = "No Contenga"; }
            if (menu.Items[i].Text == "StartsWith")           { menu.Items[i].Text = "Inicie con"; }
            if (menu.Items[i].Text == "EndsWith")             { menu.Items[i].Text = "Finalice con"; }
            if (menu.Items[i].Text == "IsEmpty")              { menu.Items[i].Visible = false; }
            if (menu.Items[i].Text == "NotIsEmpty")           { menu.Items[i].Visible = false; }
        }
    }

}