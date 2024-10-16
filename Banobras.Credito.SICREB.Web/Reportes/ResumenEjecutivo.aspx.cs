using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Business.Rules;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Business.Repositorios;
using System.IO;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Data.Reportes;

public partial class Reportes_ResumenEjecutivo : System.Web.UI.Page
{
    private int idUs; //JAGH
    protected void Page_Load(object sender, EventArgs e)
    {
        CambiaAtributosRGR(rgConciliacion_PM.FilterMenu);
        CambiaAtributosRGR(rgConciliacion_PF.FilterMenu);
        
	try
	{
	
		this.idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH
	}
	catch {;}

        if (!IsPostBack)
        {
            //JAGH se agregan actividades 17/01/13            
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso al Resumen Ejecutivo");    

            if (Session["Facultades"] != null)
            {
                getFacultades();
                rbPersona.SelectedValue = "PM";
                ObtenDatosPersonaMorales();
            }
            else
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }
    }

    protected void ObtenDatosPersonaFisica()
    {
        ActividadRules.GuardarActividad(6, this.idUs, "PF Resumen Ejecutivo");  //JAGH

        this.div_Conciliacion_PF.Visible = true;
        this.div_Conciliacion_PM.Visible = false;

        this.rgConciliacion_PF.DataSource = null;
        this.rgConciliacion_PF.DataBind();

        this.rgConciliacion_PF.DataSource = ReporteConciliacionesDataAccess.GetResumen_PF();
        this.rgConciliacion_PF.DataBind();
    }

    protected void ObtenDatosPersonaMorales()
    {
        ActividadRules.GuardarActividad(7, this.idUs, "PM Resumen Ejecutivo");  //JAGH

        this.div_Conciliacion_PF.Visible = false;
        this.div_Conciliacion_PM.Visible = true;

        this.rgConciliacion_PM.DataSource = null;
        this.rgConciliacion_PM.DataBind();

        this.rgConciliacion_PM.DataSource = ReporteConciliacionesDataAccess.GetResumen_PM();
        this.rgConciliacion_PM.DataBind();
    }

    protected void rgConciliacion_PF_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }

    protected void rgConciliacion_PM_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }

    protected void imgExportArchivoXLS_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            rgConciliacion_PF.MasterTableView.HierarchyDefaultExpanded = true;
            rgConciliacion_PF.ExportSettings.OpenInNewWindow = false;
            rgConciliacion_PF.MasterTableView.ExportToCSV();
            ActividadRules.GuardarActividad(13, this.idUs, "Exportación a Excel Resumen Ejecutivo");
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(13, this.idUs, "Error Exportación a Excel Resumen Ejecutivo"); //JAGH
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    protected void imgExportArchivoPDF_Click(object sender, ImageClickEventArgs e)
    {
        if (rbPersona.SelectedValue.ToString() == "PF")
        {
            Response.ContentType = "application/force-download";
            rgConciliacion_PF.MasterTableView.HierarchyDefaultExpanded = true;
            rgConciliacion_PF.ExportSettings.OpenInNewWindow = false;
            rgConciliacion_PF.ExportSettings.ExportOnlyData = true;
            rgConciliacion_PF.ExportSettings.IgnorePaging = true;
            rgConciliacion_PF.ExportSettings.OpenInNewWindow = true;
            rgConciliacion_PF.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
            rgConciliacion_PF.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
            rgConciliacion_PF.MasterTableView.ExportToPdf();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó resumen PF");
        }
        else
        {
            Response.ContentType = "application/force-download";
            rgConciliacion_PM.MasterTableView.HierarchyDefaultExpanded = true;
            rgConciliacion_PM.ExportSettings.OpenInNewWindow = false;
            rgConciliacion_PM.ExportSettings.ExportOnlyData = true;
            rgConciliacion_PM.ExportSettings.IgnorePaging = true;
            rgConciliacion_PM.ExportSettings.OpenInNewWindow = true;
            rgConciliacion_PM.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
            rgConciliacion_PM.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
            rgConciliacion_PM.MasterTableView.ExportToPdf();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó resumen PM");
        }
    }
    protected void rgConciliacion_PF_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.rgConciliacion_PF.DataSource = ReporteConciliacionesDataAccess.GetResumen_PF();
        this.div_Conciliacion_PF.Visible = true;
    }
    protected void rgConciliacion_PM_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.rgConciliacion_PM.DataSource = ReporteConciliacionesDataAccess.GetResumen_PM();
        this.div_Conciliacion_PM.Visible = true;
    }

    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();
        imgExportArchivoXLS.Visible = false;
        imgExportArchivoPDF.Visible = false;

        if (!Session["Facultades"].ToString().Contains(facultad.GetVariable("GRC")))
            Response.Redirect("../inicio.aspx");

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ERC")))
        {
            imgExportArchivoXLS.Visible = true;
            imgExportArchivoPDF.Visible = true;
        }

    }

    protected void rbPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbPersona.SelectedValue.ToString() == "PF")
            ObtenDatosPersonaFisica();
        else
            ObtenDatosPersonaMorales();
    }


    public void CambiaAtributosRGR(GridFilterMenu menu)
    {
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
            else if (menu.Items[i].Text == "IsEmpty")
            {
                // item.Text = "Finalice con";
                menu.Items[i].Visible = false;
            }
            else if (menu.Items[i].Text == "NotIsEmpty")
            {
                // item.Text = "Finalice con";
                menu.Items[i].Visible = false;
            } 
        }
    }
}