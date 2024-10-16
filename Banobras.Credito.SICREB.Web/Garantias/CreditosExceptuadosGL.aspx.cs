using System;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Datos.GL;
using Banobras.Credito.SICREB.Entities.GL;
using Banobras.Credito.SICREB.Business.Repositorios;

using Telerik.Web.UI;

public partial class CreditosExceptuadosGL : System.Web.UI.Page
{

    public const String catalog = "Créditos Exceptuados";
    Mapper_GL_CreditosExceptuados ObjCreditos = new Mapper_GL_CreditosExceptuados();

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

    protected void RgdCreditos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            List<GLCreditosExceptuados> lCreditos = new List<GLCreditosExceptuados>();
            lCreditos = ObjCreditos.GL_ObtenerCreditosExeptuados();
            this.RgdCreditos.DataSource = lCreditos;
            this.RgdCreditos.VirtualItemCount = lCreditos.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdCreditos_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = (GridEditableItem)e.Item;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);
            if (newValues["MOTIVO"] != null & newValues["CREDITO"] != null)
            {
                GLCreditosExceptuados CreditoNew = new GLCreditosExceptuados();
                CreditoNew.CREDITO = newValues["CREDITO"].ToString();
                CreditoNew.MOTIVO = newValues["MOTIVO"].ToString();
                ObjCreditos.GL_InsertarCreditoExceptuado(CreditoNew);
            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "El campo de Credito y Motivo son obligatorios.");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowMessage(this.Page, this.GetType(), ex.Message.ToString());
        }
    }

    protected void RgdCreditos_UpdateCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            GLCreditosExceptuados creditoNew = new GLCreditosExceptuados();
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;
            creditoNew.Id = Int64.Parse(oldValues["ID"].ToString());
            creditoNew.CREDITO = newValues["CREDITO"].ToString();
            creditoNew.MOTIVO = newValues["MOTIVO"].ToString();
            creditoNew.ESTATUS = "1";
            if (!(creditoNew.CREDITO.Contains("=") || (creditoNew.CREDITO.Contains("+")) || (creditoNew.CREDITO.Contains("-")) || creditoNew.CREDITO.Contains("@") || creditoNew.CREDITO.Contains("0x09") || creditoNew.CREDITO.Contains("0x0D")))
            {
                if ((!(creditoNew.MOTIVO.Contains("=") || (creditoNew.MOTIVO.Contains("+")) || (creditoNew.MOTIVO.Contains("-")) || creditoNew.MOTIVO.Contains("@") || creditoNew.MOTIVO.Contains("0x09") || creditoNew.MOTIVO.Contains("0x0D"))))
                {
                    ObjCreditos.GL_ActualizarCreditoExceptuado(creditoNew);
                }
                else
                {
                    //   Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se permite el uso de carácteres especiales como:=,+,-");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alertMessage", "alert('No se permite el uso de carácteres especiales como:=, +, -')", true);
                }
            }
            else
            {
                //   Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se permite el uso de carácteres especiales como:=,+,-");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alertMessage", "alert('No se permite el uso de carácteres especiales como:=, +, -')", true);


            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void RgdCreditos_DeleteCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            GLCreditosExceptuados CreditoDelete = new GLCreditosExceptuados();
            GridDataItem item = (GridDataItem)e.Item;
            CreditoDelete.Id = Parser.ToNumber(item["ID"].Text);
            ObjCreditos.GL_BorrarCreditoExceptuado(CreditoDelete);
        }
        catch (Exception ex)
        {
            throw ex;
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
            GridHeaderItem headerItem = RgdCreditos.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
            
            if (chkHeader.Checked == true)
            {
                ObjCreditos.GL_BorrarALLCreditosExceptuados();
                ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación Todos los registros del catalogo ", null, null, catalog, 1, null, null);
            }
            else
            {
                foreach (GridDataItem row in RgdCreditos.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;
                    if (chkResult)
                    {
                        GLCreditosExceptuados CreditoDelete = new GLCreditosExceptuados();
                        CreditoDelete.Id = Parser.ToNumber(row.GetDataKeyValue("ID").ToString());
                        ObjCreditos.GL_BorrarCreditoExceptuado(CreditoDelete);
                        idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                        ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + CreditoDelete.Id, null, null, catalog, 1, null, null);
                    }
                }
            }
        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }

        RgdCreditos.Rebind();
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkCell;
        CheckBox chkHeader;
        chkHeader = (CheckBox)sender;

        foreach (GridDataItem row in RgdCreditos.Items)
        {
            chkCell = (CheckBox)row.Cells[0].FindControl("chk");
            chkCell.Checked = chkHeader.Checked;
        }
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdCreditos.Columns[0].Visible = false;
        RgdCreditos.Columns[RgdCreditos.Columns.Count - 1].Visible = false;
        RgdCreditos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCreditos.ExportSettings.OpenInNewWindow = false;
        RgdCreditos.ExportSettings.ExportOnlyData = true;
        RgdCreditos.MasterTableView.GridLines = GridLines.Both;
        RgdCreditos.ExportSettings.IgnorePaging = true;
        RgdCreditos.ExportSettings.OpenInNewWindow = true;
        RgdCreditos.ExportSettings.Pdf.PageHeight = Unit.Parse("250mm");
        RgdCreditos.ExportSettings.Pdf.PageWidth = Unit.Parse("350mm");
        RgdCreditos.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }

    protected void btnExportarXLS_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdCreditos.Columns[0].Visible = false;
        RgdCreditos.Columns[RgdCreditos.Columns.Count - 1].Visible = false;
        RgdCreditos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCreditos.ExportSettings.OpenInNewWindow = false;
        RgdCreditos.ExportSettings.ExportOnlyData = true;
        RgdCreditos.ExportSettings.IgnorePaging = true;
        RgdCreditos.ExportSettings.OpenInNewWindow = true;
        RgdCreditos.MasterTableView.ExportToExcel();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }

    private void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdCreditos.FilterMenu;
        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (menu.Items[i].Text == "NoFilter") { menu.Items[i].Text = "Sin Filtro"; }
            if (menu.Items[i].Text == "EqualTo") { menu.Items[i].Text = "Igual"; }
            if (menu.Items[i].Text == "NotEqualTo") { menu.Items[i].Text = "Diferente"; }
            if (menu.Items[i].Text == "GreaterThan") { menu.Items[i].Text = "Mayor que"; }
            if (menu.Items[i].Text == "LessThan") { menu.Items[i].Text = "Menor que"; }
            if (menu.Items[i].Text == "GreaterThanOrEqualTo") { menu.Items[i].Text = "Mayor o igual a"; }
            if (menu.Items[i].Text == "LessThanOrEqualTo") { menu.Items[i].Text = "Menor o igual a"; }
            if (menu.Items[i].Text == "Between") { menu.Items[i].Text = "Entre"; }
            if (menu.Items[i].Text == "NotBetween") { menu.Items[i].Text = "No entre"; }
            if (menu.Items[i].Text == "IsNull") { menu.Items[i].Text = "Es nulo"; }
            if (menu.Items[i].Text == "NotIsNull") { menu.Items[i].Text = "No es nulo"; }
            if (menu.Items[i].Text == "Contains") { menu.Items[i].Text = "Contenga"; }
            if (menu.Items[i].Text == "DoesNotContain") { menu.Items[i].Text = "No Contenga"; }
            if (menu.Items[i].Text == "StartsWith") { menu.Items[i].Text = "Inicie con"; }
            if (menu.Items[i].Text == "EndsWith") { menu.Items[i].Text = "Finalice con"; }
            if (menu.Items[i].Text == "IsEmpty") { menu.Items[i].Visible = false; }
            if (menu.Items[i].Text == "NotIsEmpty") { menu.Items[i].Visible = false; }
        }
    }


}