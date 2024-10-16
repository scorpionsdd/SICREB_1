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
    private ReestructuradoRules reesRules = null;
    private int idUs; //JAGH

    protected void Page_Load(object sender, EventArgs e)
    {
        try
	{
	
		this.idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH
	}
	catch {;}
        
        if (!this.Page.IsPostBack)
        {            
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso al reporte de reestructurados");
        }

        if (Session["Facultades"] != null)
        {
            getFacultades();
            reesRules = new ReestructuradoRules(Enums.Persona.Moral);           
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
            rgConciliacion.MasterTableView.ExportToExcel();
            ActividadRules.GuardarActividad(13, this.idUs, "Exportación a Excel Reestructurados");//JAGH
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(13, this.idUs, "Error Exportación a Excel Reestructurados"); //JAGH
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    protected void imgExportArchivoPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        rgConciliacion.MasterTableView.HierarchyDefaultExpanded = true;
        rgConciliacion.ExportSettings.OpenInNewWindow = false;
        rgConciliacion.ExportSettings.ExportOnlyData = true;
        rgConciliacion.ExportSettings.IgnorePaging = true;
        rgConciliacion.ExportSettings.OpenInNewWindow = true;
        rgConciliacion.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        rgConciliacion.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        rgConciliacion.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó restructurados");
    }
    protected void rgConciliacion_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        try
        {
        //    String Persona = rbPersona.SelectedValue;
            rgConciliacion.DataSource = reesRules.GetReestructuradosPresentacion();
        }
        catch (Exception ex)    
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }


    }
    protected void rbPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        
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



    protected void rgConciliacion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridDataItem item = e.Item as GridDataItem;
        if (item != null)
        {
            item["SVencido"].Text = string.Format("{0}", Math.Round(Parser.ToDouble(item["SVencido"].Text), 2));
            item["SVigente"].Text = string.Format("{0}", Math.Round(Parser.ToDouble(item["SVigente"].Text), 2));
        }
    }

    protected void rgConciliacion_ItemCommand(object source, GridCommandEventArgs e)
    {


    }
    protected void RadToolBar_ButtonClick(object sender, RadToolBarEventArgs e)
    {

    }
}
