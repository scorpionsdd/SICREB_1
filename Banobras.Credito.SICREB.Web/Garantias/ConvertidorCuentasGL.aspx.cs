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

public partial class ConvertidorCuentasGL : System.Web.UI.Page
{

    public const String catalog = "Convertidor de Cuentas Lineas y Garantias";
    Mapper_GL_ConvertidorCuentas ObjCuentas = new Mapper_GL_ConvertidorCuentas();

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

    protected void RgdCuentas_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            List<GLConvertidorCuentas> lCuentas = new List<GLConvertidorCuentas>();
            lCuentas = ObjCuentas.GL_ObtenerConvertidorCuentas();
            this.RgdCuentas.DataSource = lCuentas;
            this.RgdCuentas.VirtualItemCount = lCuentas.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdCuentas_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = (GridEditableItem)e.Item;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);
            if (newValues["CUENTA_ANT"] != null & newValues["CUENTA_ACT"] != null)
            {
                GLConvertidorCuentas CuentaNew = new GLConvertidorCuentas();
                CuentaNew.CUENTA_ANT = newValues["CUENTA_ANT"].ToString();
                CuentaNew.CUENTA_ACT = newValues["CUENTA_ACT"].ToString();
                ObjCuentas.GL_InsertarConvertidorCuentas(CuentaNew);
            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "Los campo de Cuenta Actual y Cuenta Anterior son obligatorios.");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowMessage(this.Page, this.GetType(), ex.Message.ToString());
        }
    }

    protected void RgdCuentas_UpdateCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            GLConvertidorCuentas cuentaNew = new GLConvertidorCuentas();
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;
            cuentaNew.ID_CONVERTIDOR = Int32.Parse(oldValues["ID_CONVERTIDOR"].ToString());
            cuentaNew.CUENTA_ACT = newValues["CUENTA_ACT"].ToString();
            cuentaNew.CUENTA_ANT = newValues["CUENTA_ANT"].ToString();
            cuentaNew.ESTATUS = "1";
            if (!(cuentaNew.CUENTA_ACT.Contains("=") || (cuentaNew.CUENTA_ACT.Contains("+")) || (cuentaNew.CUENTA_ACT.Contains("-")) || cuentaNew.CUENTA_ACT.Contains("@") || cuentaNew.CUENTA_ACT.Contains("0x09") || cuentaNew.CUENTA_ACT.Contains("0x0D")))
            {
                if ((!( cuentaNew.CUENTA_ANT.Contains("=") || ( cuentaNew.CUENTA_ANT.Contains("+")) || ( cuentaNew.CUENTA_ANT.Contains("-")) ||  cuentaNew.CUENTA_ANT.Contains("@") ||  cuentaNew.CUENTA_ANT.Contains("0x09") ||  cuentaNew.CUENTA_ANT.Contains("0x0D"))))
                {
                    ObjCuentas.GL_ActualizarConvertidorCuentas(cuentaNew);
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

    protected void RgdCuentas_DeleteCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            GLConvertidorCuentas cuentaDelete = new GLConvertidorCuentas();
            GridDataItem item = (GridDataItem)e.Item;
            cuentaDelete.ID_CONVERTIDOR = Parser.ToNumber(item["ID_CONVERTIDOR"].Text);
            ObjCuentas.GL_BorrarConvertidorCuentas(cuentaDelete);
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
            GridHeaderItem headerItem = RgdCuentas.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());

            if (chkHeader.Checked == true)
            {
                ObjCuentas.GL_BorrarALLConvertidorCuentas();
                ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación Todos los registros del catalogo ", null, null, catalog, 1, null, null);
            }
            else
            {
                foreach (GridDataItem row in RgdCuentas.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;
                    if (chkResult)
                    {
                        GLConvertidorCuentas cuentaDelete = new GLConvertidorCuentas();
                        cuentaDelete.ID_CONVERTIDOR = Parser.ToNumber(row.GetDataKeyValue("ID_CONVERTIDOR").ToString());
                        ObjCuentas.GL_BorrarConvertidorCuentas(cuentaDelete);
                        idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                        ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + cuentaDelete.ID_CONVERTIDOR, null, null, catalog, 1, null, null);
                    }
                }
            }
        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }

        RgdCuentas.Rebind();
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkCell;
        CheckBox chkHeader;
        chkHeader = (CheckBox)sender;

        foreach (GridDataItem row in RgdCuentas.Items)
        {
            chkCell = (CheckBox)row.Cells[0].FindControl("chk");
            chkCell.Checked = chkHeader.Checked;
        }
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdCuentas.Columns[0].Visible = false;
        RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
        RgdCuentas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = false;
        RgdCuentas.ExportSettings.ExportOnlyData = true;
        RgdCuentas.MasterTableView.GridLines = GridLines.Both;
        RgdCuentas.ExportSettings.IgnorePaging = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = true;
        RgdCuentas.ExportSettings.Pdf.PageHeight = Unit.Parse("250mm");
        RgdCuentas.ExportSettings.Pdf.PageWidth = Unit.Parse("350mm");
        RgdCuentas.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }

    protected void btnExportarXLS_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdCuentas.Columns[0].Visible = false;
        RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
        RgdCuentas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = false;
        RgdCuentas.ExportSettings.ExportOnlyData = true;
        RgdCuentas.ExportSettings.IgnorePaging = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = true;
        RgdCuentas.MasterTableView.ExportToExcel();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }

    private void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdCuentas.FilterMenu;
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