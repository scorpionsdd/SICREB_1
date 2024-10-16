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
using System.Data;
using System.Text;

public partial class Reportes_ReporteConciliacion : ExportToExcel
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
        catch { ;}

        if (!IsPostBack)
        {
            //JAGH se agregan actividades 17/01/13            
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso al Reporte Conciliacion");

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
        ActividadRules.GuardarActividad(6, this.idUs, "PF Conciliacion");  //JAGH
        this.div_Conciliacion_PF.Visible = true;
        this.div_Conciliacion_PM.Visible = false;

        this.rgConciliacion_PF.DataSource = null;
        this.rgConciliacion_PF.DataBind();
        DataSet dsPF = new DataSet();
        dsPF = ReporteConciliacionesDataAccess.Get_PF();
        LlenarTotal(dsPF.Tables[0], "fisica");
        this.rgConciliacion_PF.DataSource = dsPF;
        this.rgConciliacion_PF.DataBind();
       
    }

    protected void ObtenDatosPersonaMorales()
    {
        ActividadRules.GuardarActividad(7, this.idUs, "PM Conciliacion");  //JAGH
        this.div_Conciliacion_PF.Visible = false;
        this.div_Conciliacion_PM.Visible = true;

        this.rgConciliacion_PM.DataSource = null;
        this.rgConciliacion_PM.DataBind();
        DataSet dsPM = new DataSet();
        dsPM = ReporteConciliacionesDataAccess.Get_PM();
        LlenarTotal(dsPM.Tables[0], "moral");
        this.rgConciliacion_PM.DataSource = dsPM;
        this.rgConciliacion_PM.DataBind();
    }

    private void LlenarTotal(DataTable dtPM, string persona)
    {
        decimal defecto = 0;
        int contadorCredito = 0;
        decimal totalSalVigSicreb = 0;
        decimal totalSalVincSicreb = 0;

        decimal totalSalVigSicofin = 0;
        decimal totalSalVincSicofin = 0;

        decimal totalSalVigDiferencia = 0;
        decimal totalSalVincDiferencia = 0;

        foreach (DataRow renglon in dtPM.Rows)

        {
            //if (renglon["OBSERVACIONES"].ToString() == "Sin observación" || renglon["OBSERVACIONES"].ToString() == "Por redondeo")
            //{
            //    contadorCredito += 1;
            //}
            if (decimal.TryParse(renglon["SALDO_VIGENTE_SICREB"].ToString(), out defecto))
            {
                totalSalVigSicreb += Convert.ToDecimal(renglon["SALDO_VIGENTE_SICREB"]);
            }
            if (decimal.TryParse(renglon["SALDO_VENCIDO_SICREB"].ToString(), out defecto))
            {
                totalSalVincSicreb += Convert.ToDecimal(renglon["SALDO_VENCIDO_SICREB"]);
            }
            if (decimal.TryParse(renglon["SALDO_VIGENTE_SICOFIN"].ToString(), out defecto))
            {
                totalSalVigSicofin += Convert.ToDecimal(renglon["SALDO_VIGENTE_SICOFIN"]);
            }
            if (decimal.TryParse(renglon["SALDO_VENCIDO_SICOFIN"].ToString(), out defecto))
            {
                totalSalVincSicofin += Convert.ToDecimal(renglon["SALDO_VENCIDO_SICOFIN"]);
            }
            if (decimal.TryParse(renglon["SALDO_VIGENTE_DIFERENCIA"].ToString(), out defecto))
            {
                totalSalVigDiferencia += Convert.ToDecimal(renglon["SALDO_VIGENTE_DIFERENCIA"]);
            }
            if (decimal.TryParse(renglon["SALDO_VENCIDO_DIFERENCIA"].ToString(), out defecto))
            {
                totalSalVincDiferencia += Convert.ToDecimal(renglon["SALDO_VENCIDO_DIFERENCIA"]);
            }
            if (decimal.TryParse(renglon["AUXILIAR"].ToString(), out defecto))
            {
                renglon["AUXILIAR"] = "'" + renglon["AUXILIAR"];
            }
        }
        if (persona == "moral")
        {
            lblNoCredito.Text = dtPM.Rows.Count.ToString();
            lblSalVigSicreb.Text = totalSalVigSicreb.ToString();
            lblSalVincSicreb.Text = totalSalVincSicreb.ToString();
            lblSalVigSicofin.Text = totalSalVigSicofin.ToString();
            lblSalVincSicofin.Text = totalSalVincSicofin.ToString();
            lblSalVigDiferencia.Text = totalSalVigDiferencia.ToString();
            lblSalVincDiferencia.Text = totalSalVincDiferencia.ToString();
        }
        else if (persona == "reporteFinal")
        {
            DataRow renglon = dtPM.NewRow();
            renglon["ESTADO"] = "Total:";
            renglon["NO_CREDITO"] = dtPM.Rows.Count.ToString();
            renglon["SALDO_VIGENTE_SICREB"] = totalSalVigSicreb.ToString();
            renglon["SALDO_VENCIDO_SICREB"] = totalSalVincSicreb.ToString();
            renglon["SALDO_VIGENTE_SICOFIN"] = totalSalVigSicofin.ToString();
            renglon["SALDO_VENCIDO_SICOFIN"] = totalSalVincSicofin.ToString();
            renglon["SALDO_VIGENTE_DIFERENCIA"] = totalSalVigDiferencia.ToString();
            renglon["SALDO_VENCIDO_DIFERENCIA"] = totalSalVincDiferencia.ToString();
            dtPM.Rows.Add(renglon);
        }
        else
        {
            lblPFNoCredito.Text = dtPM.Rows.Count.ToString();
            lblPFSalVigSicreb.Text = totalSalVigSicreb.ToString();
            lblPFSalVincSicreb.Text = totalSalVincSicreb.ToString();
            lblSalVigSicofin.Text = totalSalVigSicofin.ToString();
            lblPFSalVincSicofin.Text = totalSalVincSicofin.ToString();
            lblPFSalVigDiferencia.Text = totalSalVigDiferencia.ToString();
            lblPFSalVincDiferencia.Text = totalSalVincDiferencia.ToString();
        }
    }

    protected void imgExportArchivoXLS_Click(object sender, ImageClickEventArgs e)
    {
        if (rbPersona.SelectedValue.ToString() == "PF")
        {
            //rgConciliacion_PF.MasterTableView.HierarchyDefaultExpanded = true;
            //rgConciliacion_PF.ExportSettings.OpenInNewWindow = false;
            //rgConciliacion_PF.MasterTableView.ExportToCSV();
            //idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            //ActividadRules.GuardarActividad(13, idUs, "Exportación a Excel");
            DataSet dsPF = new DataSet();
            dsPF = ReporteConciliacionesDataAccess.Get_PF();
            LlenarTotal(dsPF.Tables[0], "reporteFinal");
            ResponseWriteExcelFile(dsPF.Tables[0], "ReporteConciliacion.xls", new string[] { "ESTADO","TIPO_CARTERA", "RFC", "ACREDITADO", 
                "NO_CREDITO", "SALDO_VIGENTE_SICREB", "SALDO_VENCIDO_SICREB", "AUXILIAR", 
                "SALDO_VIGENTE_SICOFIN", "SALDO_VENCIDO_SICOFIN", "SALDO_VIGENTE_DIFERENCIA", 
                "SALDO_VENCIDO_DIFERENCIA", "OBSERVACIONES"});
        }
        else
        {
            //rgConciliacion_PM.MasterTableView.HierarchyDefaultExpanded = true;
            //rgConciliacion_PM.ExportSettings.OpenInNewWindow = false;
            //rgConciliacion_PM.MasterTableView.ExportToCSV();
            //idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            //ActividadRules.GuardarActividad(13, idUs, "Exportación a Excel");
            DataSet dsPM = new DataSet();
            dsPM = ReporteConciliacionesDataAccess.Get_PM();
            LlenarTotal(dsPM.Tables[0], "reporteFinal");
            ResponseWriteExcelFile(dsPM.Tables[0], "ReporteConciliacion.xls", new string[] { "ESTADO", "RFC", "ACREDITADO", 
                "NO_CREDITO", "SALDO_VIGENTE_SICREB", "SALDO_VENCIDO_SICREB", "AUXILIAR", 
                "SALDO_VIGENTE_SICOFIN", "SALDO_VENCIDO_SICOFIN", "SALDO_VIGENTE_DIFERENCIA", 
                "SALDO_VENCIDO_DIFERENCIA", "OBSERVACIONES" });
        }
    }

    protected void rgConciliacion_PF_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }

    protected void rgConciliacion_PM_ItemDataBound(object sender, GridItemEventArgs e)
    {
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
            rgConciliacion_PF.ExportSettings.Pdf.PageHeight = Unit.Parse("300mm");
            rgConciliacion_PF.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
            rgConciliacion_PF.MasterTableView.ExportToPdf();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó conciliación PF");
        }
        else
        {
            Response.ContentType = "application/force-download";
            rgConciliacion_PM.MasterTableView.HierarchyDefaultExpanded = true;
            rgConciliacion_PM.ExportSettings.OpenInNewWindow = false;
            rgConciliacion_PM.ExportSettings.ExportOnlyData = true;
            rgConciliacion_PM.ExportSettings.IgnorePaging = true;
            rgConciliacion_PM.ExportSettings.OpenInNewWindow = true;
            rgConciliacion_PM.ExportSettings.Pdf.PageHeight = Unit.Parse("300mm");
            rgConciliacion_PM.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
            rgConciliacion_PM.MasterTableView.ExportToPdf();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó conciliación PM");
        }
        //string[] nombres = new string[] { "REPORTE DE CONCILIACIÓN" };
        //List<RadGrid> listaGrids = new List<RadGrid>();

        //try
        //{
        //    this.rgConciliacion_PF.MasterTableView.AllowPaging = false;
        //    this.rgConciliacion_PF.Rebind();
        //    listaGrids.Add(this.rgConciliacion_PF);
        //    string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, "REPORTE DE CONCILIACIÓN", WebConfig.Site);
        //    this.rgConciliacion_PF.MasterTableView.AllowPaging = true;
        //    this.rgConciliacion_PF.Rebind();
        //    ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);
        //    ActividadRules.GuardarActividad(13, this.idUs, "Exportación a PDF Conciliacion"); //JAGH
        //}
        //catch (Exception ex)
        //{
        //    ActividadRules.GuardarActividad(13, this.idUs, "Error Exportación a PDF Conciliacion"); //JAGH
        //    Mensajes.ShowError(this.Page, this.GetType(), ex);
        //}
    }
    protected void rgConciliacion_PF_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.rgConciliacion_PF.DataSource = ReporteConciliacionesDataAccess.Get_PF();
    }
    protected void rgConciliacion_PM_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.rgConciliacion_PM.DataSource = ReporteConciliacionesDataAccess.Get_PM();
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