using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Reportes;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Business.Rules;
using Banobras.Credito.SICREB.Business.Repositorios;

using Telerik.Web.UI;

public partial class HistorialGL : System.Web.UI.Page
{

    private int idUs;

    protected void Page_Load(object sender, EventArgs e)
    {
        CambiaAtributosRGR(RgdHistorialGL.FilterMenu);

        try
        {
            this.idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        }
        catch { ; }

        if (!IsPostBack)
        {
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso al Historial GL");

            if (Session["Facultades"] != null)
            {
                getFacultades();
                ObtenDatosResumenConciliacionGL();
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }
        }
    }

    protected void RgdHistorialGL_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.RgdHistorialGL.DataSource = ReporteConciliacionesDataAccess.Get_HistorialGL();
    }


    protected void btnExportarXLS_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RgdHistorialGL.MasterTableView.HierarchyDefaultExpanded = true;
            RgdHistorialGL.ExportSettings.OpenInNewWindow = false;
            RgdHistorialGL.MasterTableView.ExportToCSV();
            ActividadRules.GuardarActividad(13, this.idUs, "Exportación a Excel del Historial GL");
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(13, this.idUs, "Error Exportación a Excel del Historial GL");
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdHistorialGL.MasterTableView.HierarchyDefaultExpanded = true;
        RgdHistorialGL.ExportSettings.OpenInNewWindow = false;
        RgdHistorialGL.ExportSettings.ExportOnlyData = true;
        RgdHistorialGL.ExportSettings.IgnorePaging = true;
        RgdHistorialGL.ExportSettings.OpenInNewWindow = true;
        RgdHistorialGL.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
        RgdHistorialGL.ExportSettings.Pdf.PageWidth = Unit.Parse("300mm");
        RgdHistorialGL.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Historial GL");
    }


    private void ObtenDatosResumenConciliacionGL()
    {
        ActividadRules.GuardarActividad(7, this.idUs, "Historial GL");
        this.RgdHistorialGL.DataSource = null;
        this.RgdHistorialGL.DataBind();

        this.RgdHistorialGL.DataSource = ReporteConciliacionesDataAccess.Get_HistorialGL();
        this.RgdHistorialGL.DataBind();
    }

    private void CambiaAtributosRGR(GridFilterMenu menu)
    {
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

    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();
        btnExportarXLS.Visible = false;
        btnExportarPDF.Visible = false;

        if (!Session["Facultades"].ToString().Contains(facultad.GetVariable("GRC")))
        {
            Response.Redirect("../inicio.aspx");
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ERC")))
        {
            btnExportarXLS.Visible = true;
            btnExportarPDF.Visible = true;
        }
    }

}