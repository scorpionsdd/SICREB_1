using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

public partial class ReporteNavegacion : System.Web.UI.Page
{
   
    private ActividadRules act;
    private int idUs; //JAGH
    protected void Page_Load(object sender, EventArgs e)
    {
        //JAGH se agregan actividades 17/01/13
        try
	{
	
		this.idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH
	}
	catch {;}

        ActividadRules.GuardarActividad(4444, this.idUs, "Acceso al Reporte Navegacion");        
    }
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdNavegacion.FilterMenu;
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
    
    protected void grdCatalogo_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (rdpFechaInicial.SelectedDate.HasValue && rdpFechaInicial.SelectedDate.HasValue)
        {

            act = new ActividadRules();
            this.RgdNavegacion.DataSource = act.GetActividades(rdpFechaInicial.SelectedDate.Value, rdpFechaFinal.SelectedDate.Value.AddDays(1));
                        
        }
    }
    
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        act = new ActividadRules();
        CambiaAtributosRGR();
        try
        {
            ActividadRules.GuardarActividad(700, this.idUs, "Búsqueda en Reporte Navegacion");      //JAGH  
            DateTime dimeInicial = rdpFechaInicial.SelectedDate.Value;
            //FIX se le agrega un día para que encuentre lo que seleccionó el cliente
            DateTime dimeFinal = rdpFechaFinal.SelectedDate.Value.AddDays(1);
                this.RgdNavegacion.DataSource = act.GetActividades(dimeInicial, dimeFinal);
                this.RgdNavegacion.DataBind();
                this.RgdNavegacion.Visible = true;
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(700, this.idUs, "Error Búsqueda en Reporte Navegacion");      //JAGH  
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }


    }
   
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.RgdNavegacion.ExportSettings.IgnorePaging = true;
            this.RgdNavegacion.ExportSettings.OpenInNewWindow = true;
            this.RgdNavegacion.ExportSettings.ExportOnlyData = true;
            this.RgdNavegacion.ExportSettings.HideStructureColumns = true;
            this.RgdNavegacion.MasterTableView.ExportToExcel();
            ActividadRules.GuardarActividad(3333, this.idUs, "Excel Reporte Navegacion");      //JAGH  
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(3333, this.idUs, "Error Excel Reporte Navegacion");      //JAGH  
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        string[] nombres = new string[] { this.lblTitle.Text };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {
            this.RgdNavegacion.MasterTableView.AllowPaging = false;
            this.RgdNavegacion.Rebind();
            listaGrids.Add(this.RgdNavegacion);
            string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, this.lblTitle.Text, WebConfig.Site);
            this.RgdNavegacion.MasterTableView.AllowPaging = true;
            this.RgdNavegacion.Rebind();
            ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);
            ActividadRules.GuardarActividad(2222, this.idUs, "PDF Reporte Navegacion");      //JAGH  
        }
        catch (Exception ex)
        {
            ActividadRules.GuardarActividad(2222, this.idUs, "Error PDF Reporte Navegacion");      //JAGH  
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
}