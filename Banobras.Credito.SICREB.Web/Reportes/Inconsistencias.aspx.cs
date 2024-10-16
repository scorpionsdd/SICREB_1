using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using System.Data;

public partial class Reportes_Inconsistencias : System.Web.UI.Page
{
    Enums.Persona TipoPersona;    
    private int idUs; //JAGH

    protected void Page_Load(object sender, EventArgs e)
    {
        try
	{
	
		this.idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH
	}
	catch {;}

        //JAGH
        if (!this.Page.IsPostBack)
        {
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso a Reporte Inconsistencias");
        }

        CambioTipoPersona();
        if (lblAux.Text.Trim().Equals(String.Empty))
        {
            CargaInconsistencias();
            lblAux.Text = "1";
        }
    }

    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.rgvInconsistencias.ExportSettings.IgnorePaging = true;
            this.rgvInconsistencias.ExportSettings.OpenInNewWindow = true;
            this.rgvInconsistencias.ExportSettings.ExportOnlyData = true;
            this.rgvInconsistencias.ExportSettings.HideStructureColumns = true;
            this.rgvInconsistencias.MasterTableView.ExportToExcel();
            this.rgvInconsistencias.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            ActividadRules.GuardarActividad(3333, this.idUs, "Excel Reporte Inconsistencias");      //JAGH  
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(3333, this.idUs, "Error Excel Reporte Inconsistencias");      //JAGH  
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    protected void rgvInconsistencias_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }

    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        rgvInconsistencias.MasterTableView.HierarchyDefaultExpanded = true;
        rgvInconsistencias.ExportSettings.OpenInNewWindow = false;
        rgvInconsistencias.ExportSettings.ExportOnlyData = true;
        rgvInconsistencias.ExportSettings.IgnorePaging = true;
        rgvInconsistencias.ExportSettings.OpenInNewWindow = true;
        rgvInconsistencias.ExportSettings.Pdf.PageHeight = Unit.Parse("300mm");
        rgvInconsistencias.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        rgvInconsistencias.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó inconsistencias");
    }
    protected void rgvInconsistencias_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {        
        try
        {
            CambioTipoPersona();
            this.rgvInconsistencias.DataSource = CargaInconsistencias();
        }
        catch (Exception ex)
        {
            string strMessageError = ex.Message.ToString();
        }
    }

    private void CambioTipoPersona()
    {
        if (Session["TipoPersona"] != null)
            TipoPersona = (Enums.Persona)Session["TipoPersona"];
        if (TipoPersona != (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica))
            lblAux.Text = string.Empty;
        TipoPersona = (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica);
        //Session.Add("erroresInfoPF", errores.GetErroresPresentacion(Enums.Persona.Fisica, 1));
        Session.Add("TipoPersona", TipoPersona);
    }

    private DataTable CargaInconsistencias()
    {       
        //Modificacion Mariana Gonzalez Amigon
        if (Session["TipoPersona"] != null)
            TipoPersona = (Enums.Persona)Session["TipoPersona"];
        System.Data.DataTable dtInconsistencias = new System.Data.DataTable();
        cls_ReporteInconsistencias clsReporteInconsistencias = new cls_ReporteInconsistencias();
        clsReporteInconsistencias = new cls_ReporteInconsistencias(TipoPersona);               
        return clsReporteInconsistencias.GetTablaInconsistencia();                  
    }
    protected void rbPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
          //recorre elementos de la lista JAGH 17/01/13
        CambioTipoPersona();
        this.rgvInconsistencias.DataSource = CargaInconsistencias();
        this.rgvInconsistencias.DataBind();
        foreach (ListItem li in rbPersona.Items)
        {
            if (li.Selected)
            {
                int iAct = 6;
                if (li.Value.Equals("PM"))
                    iAct = 7;

                ActividadRules.GuardarActividad(iAct, this.idUs, "Seleccionado Cambios Reportes " + li.Text);  //JAGH
            }
            else
            {
                int iAct = 6;
                if (li.Value.Equals("PM"))
                    iAct = 7;
                ActividadRules.GuardarActividad(iAct, this.idUs, "Deseleccionado Cambios Reportes " + li.Text);  //JAGH
            }
        }
    }
}