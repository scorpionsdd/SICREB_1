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

public partial class Reportes_Conciliacion : System.Web.UI.Page
{
    private ConciliacionRules concRules = null;
    private int idUs; //JAGH
    protected void Page_Load(object sender, EventArgs e)
    {
        //JAGH se agregan actividades 17/01/13
        try
	{
	
		this.idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH
	}
	catch {;}

        ActividadRules.GuardarActividad(4444, this.idUs, "Acceso al Reporte Conciliacion");        

        if (Session["Facultades"] != null)
        {
            getFacultades();
            concRules = new ConciliacionRules((rbPersona.SelectedValue == "PM" ? Enums.Persona.Moral : Enums.Persona.Fisica));

            if (cbAnios.Items.Count == 0)
            {
                cbAnios.DataSource = concRules.GetAniosDeArchivos();
                cbAnios.DataBind();

                cbAnios_SelectedIndexChanged(sender, null);
                ActividadRules.GuardarActividad(12, idUs, "Conciliación: " + Convert.ToString(rbPersona.SelectedValue));
            }
        }
        else
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");        
    }
    protected void imgExportArchivoXLS_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            rgConciliacion.MasterTableView.HierarchyDefaultExpanded = true;
            rgConciliacion.ExportSettings.OpenInNewWindow = false;
            rgConciliacion.MasterTableView.ExportToCSV();
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(13, this.idUs, "Exportación a Excel Conciliacion");//JAGH
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(13, this.idUs, "Error Exportación a Excel Conciliacion"); //JAGH
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
        
    }
    protected void imgExportArchivoPDF_Click(object sender, ImageClickEventArgs e)
    {
        string[] nombres = new string[] { "REPORTE DE CONCILIACIÓN" };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {
            this.rgConciliacion.MasterTableView.AllowPaging = false;
            this.rgConciliacion.Rebind();
            listaGrids.Add(this.rgConciliacion);
            string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, "REPORTE DE CONCILIACIÓN", WebConfig.Site);
            this.rgConciliacion.MasterTableView.AllowPaging = true;
            this.rgConciliacion.Rebind();
            ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);
            ActividadRules.GuardarActividad(13, this.idUs, "Exportación a PDF Conciliacion"); //JAGH
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(13, this.idUs, "Error Exportación a PDF Conciliacion"); //JAGH
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
       
       
    }
    protected void rgConciliacion_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            rgConciliacion.DataSource = concRules.GetConciliacionPresentacion(Parser.ToNumber(cbArchivos.SelectedValue));
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
        

    }
    protected void cbAnios_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        FilterArchivos();
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
    protected void cbArchivos_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        rgConciliacion.Rebind();
    }
    protected void rbPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        FilterArchivos();
    }

    private void FilterArchivos()
    {
        try
        {

            List<Archivo> archivos = concRules.GetArchivosPorAnio(Parser.ToNumber(cbAnios.SelectedValue));

            cbArchivos.DataSource = archivos;

            cbArchivos.DataTextField = "Nombre";
            cbArchivos.DataValueField = "Id";
            cbArchivos.DataBind();

            rgConciliacion.Rebind();
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    protected void rgConciliacion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridDataItem item = e.Item as GridDataItem;
        if (item != null)
        {
            item["SVencido"].Text = string.Format("{0}", Math.Round(Parser.ToDouble(item["SVencido"].Text),2));
            item["SVigente"].Text = string.Format("{0}", Math.Round(Parser.ToDouble(item["SVigente"].Text), 2));
        } 
    }

    protected void rgConciliacion_ItemCommand(object source, GridCommandEventArgs e)
    {
       

        if (e.CommandName == "FilterRadGrid")
        {
            RadFilter.FireApplyCommand();
        }
    }
    protected void RadToolBar_ButtonClick(object sender, RadToolBarEventArgs e)
    {

    }
}
