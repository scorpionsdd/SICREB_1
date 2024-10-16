using System;
using System.IO;
using System.Web;
using System.Data;
using System.Text;
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

public partial class ConciliacionGL : ExportToExcel
{

    private int idUs;

    protected void Page_Load(object sender, EventArgs e)
    {
        CambiaAtributosRGR(RgdConciliacionGL.FilterMenu);

        try
        {
            this.idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        }
        catch
        {
        }

        if (!IsPostBack)
        {         
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso al Reporte de Conciliacion GL");

            if (Session["Facultades"] != null)
            {
                getFacultades();
                ObtenDatosConciliacion();
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }                
        }
    }

    protected void RgdConciliacionGL_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.RgdConciliacionGL.DataSource = ReporteConciliacionesDataAccess.Get_ConciliacionGL();       
    }


    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdConciliacionGL.MasterTableView.HierarchyDefaultExpanded = true;
        RgdConciliacionGL.ExportSettings.FileName = "ConciliacionGL";
        RgdConciliacionGL.ExportSettings.OpenInNewWindow = false;
        RgdConciliacionGL.ExportSettings.ExportOnlyData = true;
        RgdConciliacionGL.ExportSettings.IgnorePaging = true;
        RgdConciliacionGL.ExportSettings.OpenInNewWindow = true;
        RgdConciliacionGL.ExportSettings.Pdf.PageHeight = Unit.Parse("300mm");
        RgdConciliacionGL.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdConciliacionGL.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó conciliación GL");
    }

    protected void btnExportarXLS_Click(object sender, ImageClickEventArgs e)
    {
        DataSet dsPM = new DataSet();
        dsPM = ReporteConciliacionesDataAccess.Get_ConciliacionGL();

        CalcularSumasTotales(dsPM.Tables[0], "Reporte");

        ResponseWriteExcelFile(dsPM.Tables[0], "ConciliacionGL.xls", new string[] { "TIPO", "ESTADO", "RFC", "ACREDITADO", 
        "NO_CREDITO", "SALDO_SICREB", "AUXILIAR", "SALDO_SICOFIN", "SALDO_DIFERENCIA", "OBSERVACIONES" });
    }


    private void ObtenDatosConciliacion()
    {
        ActividadRules.GuardarActividad(7, this.idUs, "Conciliacion GL");
        this.RgdConciliacionGL.DataSource = null;
        this.RgdConciliacionGL.DataBind();
        
        DataSet dsPM = new DataSet();
        dsPM = ReporteConciliacionesDataAccess.Get_ConciliacionGL();
        CalcularSumasTotales(dsPM.Tables[0], "Visualizacion");

        this.RgdConciliacionGL.DataSource = dsPM;
        this.RgdConciliacionGL.DataBind();
    }

    private void CalcularSumasTotales(DataTable dtPM, string persona)
    {
        decimal Defecto = 0;
        decimal TotalSicreb = 0;
        decimal TotalSicofin = 0;
        decimal TotalDiferencia = 0;

        foreach (DataRow renglon in dtPM.Rows)
        {
            if (decimal.TryParse(renglon["SALDO_SICREB"].ToString(), out Defecto))
            {
                TotalSicreb += Convert.ToDecimal(renglon["SALDO_SICREB"]);
            }

            if (decimal.TryParse(renglon["SALDO_SICOFIN"].ToString(), out Defecto))
            {
                TotalSicofin += Convert.ToDecimal(renglon["SALDO_SICOFIN"]);
            }

            if (decimal.TryParse(renglon["SALDO_DIFERENCIA"].ToString(), out Defecto))
            {
                TotalDiferencia += Convert.ToDecimal(renglon["SALDO_DIFERENCIA"]);
            }
        }

        if (persona == "Visualizacion")
        {
            lblCreditos.Text = dtPM.Rows.Count.ToString();
            lblTotalSicreb.Text = string.Format("{0:C2}",TotalSicreb);
            lblTotalSicofin.Text = string.Format("{0:C2}", TotalSicofin);
            lblTotalDiferencia.Text = string.Format("{0:C2}", TotalDiferencia);
        }
        
        if (persona == "Reporte")
        {
            DataRow renglon = dtPM.NewRow();

            renglon["ESTADO"] = "Total:";
            renglon["NO_CREDITO"] = dtPM.Rows.Count.ToString();
            renglon["SALDO_SICREB"] = TotalSicreb.ToString();
            renglon["SALDO_SICOFIN"] = TotalSicofin.ToString();
            renglon["SALDO_DIFERENCIA"] = TotalDiferencia.ToString();

            dtPM.Rows.Add(renglon);
        }       
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