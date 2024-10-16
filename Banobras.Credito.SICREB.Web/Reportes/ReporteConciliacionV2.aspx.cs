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

public partial class ReporteConciliacionV2 : ExportToExcel //System.Web.UI.Page
{

    private int idUs;

    protected void Page_Load(object sender, EventArgs e)
    {
        CambiaAtributosRGR(rgConciliacionPM.FilterMenu);
        CambiaAtributosRGR(rgConciliacionPF.FilterMenu);

        try
        {
            this.idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        }
        catch
        {
        }

        if (!IsPostBack)
        {                      
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso al Reporte de Conciliación");

            if (Session["Facultades"] != null)
            {
                getFacultades();
                rbPersona.SelectedValue = "PM";
                ObtenDatosPersonaMorales();
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }
        }
    }

    protected void rbPersona_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (rbPersona.SelectedValue.ToString() == "PF")
        {
            ObtenDatosPersonaFisica();
        }
        else
        {
            ObtenDatosPersonaMorales();
        }         
    }

    protected void rgConciliacion_PM_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {        
        this.rgConciliacionPM.DataSource = ReporteConciliacionesDataAccess.Get_PMActualizado();        
    }

    protected void rgConciliacion_PF_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.rgConciliacionPF.DataSource = ReporteConciliacionesDataAccess.Get_PFActualizado();
    }

    protected void imgExportArchivoXLS_Click(object sender, ImageClickEventArgs e)
    {

        if (rbPersona.SelectedValue.ToString() == "PM")
        {
            DataSet dsPM = new DataSet();
            dsPM = ReporteConciliacionesDataAccess.Get_PMActualizado();
            LlenarTotal(dsPM.Tables[0], "REPORTE");

            ResponseWriteExcelFile(dsPM.Tables[0], "ReporteConciliacionPM.xls", new string[] {"RFC_ACREDITADO", "NOMBRE_ACREDITADO", 
                "NUMERO_CREDITO", "SALDO_VIGENTE_SICREB", "SALDO_VENCIDO_SICREB","SALDO_BONO_CUPON_CERO", "SALDO_TOTAL_SICREB",
                "AUXILIAR", "SALDO_VIGENTE_SICOFIN", "SALDO_VENCIDO_SICOFIN", "SALDO_DIVERSO_SICOFIN", "SALDO_TOTAL_SICOFIN", "SALDO_TOTAL_DIFERENCIA",
                "SALDO_REDONDEO", "SALDO_TOTAL_CONCILIACION", "OBSERVACIONES", "DETALLE_OBSERVACIONES", "ESTADO" });
        }

        if (rbPersona.SelectedValue.ToString() == "PF")
        {
            DataSet dsPF = new DataSet();
            dsPF = ReporteConciliacionesDataAccess.Get_PFActualizado();
            LlenarTotal(dsPF.Tables[0], "REPORTE");

            ResponseWriteExcelFile(dsPF.Tables[0], "ReporteConciliacionPF.xls", new string[] { "RFC_ACREDITADO", "NOMBRE_ACREDITADO", 
                "NUMERO_CREDITO", "TIPO_CREDITO", "SALDO_VIGENTE_SICREB", "SALDO_VENCIDO_SICREB", "SALDO_TOTAL_SICREB", 
                "AUXILIAR", "SALDO_VIGENTE_SICOFIN", "SALDO_VENCIDO_SICOFIN",  "SALDO_DIVERSO_SICOFIN", "SALDO_TOTAL_SICOFIN", "SALDO_TOTAL_DIFERENCIA", 
                "SALDO_REDONDEO", "SALDO_TOTAL_CONCILIACION", "OBSERVACIONES", "DETALLE_OBSERVACIONES", "ESTADO" });
        }

    }

    protected void imgExportArchivoPDF_Click(object sender, ImageClickEventArgs e)
    {

        if (rbPersona.SelectedValue.ToString() == "PM")
        {
            Response.ContentType = "application/force-download";
            rgConciliacionPM.MasterTableView.HierarchyDefaultExpanded = true;            
            rgConciliacionPM.ExportSettings.FileName = "ConciliacionPM";            
            rgConciliacionPM.ExportSettings.OpenInNewWindow = false;
            rgConciliacionPM.ExportSettings.ExportOnlyData = true;
            rgConciliacionPM.ExportSettings.IgnorePaging = true;
            rgConciliacionPM.ExportSettings.OpenInNewWindow = true;
            rgConciliacionPM.ExportSettings.Pdf.PageHeight = Unit.Parse("500mm");
            rgConciliacionPM.ExportSettings.Pdf.PageWidth = Unit.Parse("900mm");
            rgConciliacionPM.MasterTableView.ExportToPdf();

            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó conciliación PM");
        }

        if (rbPersona.SelectedValue.ToString() == "PF")
        {
            Response.ContentType = "application/force-download";
            rgConciliacionPF.MasterTableView.HierarchyDefaultExpanded = true;
            rgConciliacionPF.ExportSettings.FileName = "ConciliacionPF";
            rgConciliacionPF.ExportSettings.OpenInNewWindow = false;
            rgConciliacionPF.ExportSettings.ExportOnlyData = true;
            rgConciliacionPF.ExportSettings.IgnorePaging = true;
            rgConciliacionPF.ExportSettings.OpenInNewWindow = true;
            rgConciliacionPF.ExportSettings.Pdf.PageHeight = Unit.Parse("500mm");
            rgConciliacionPF.ExportSettings.Pdf.PageWidth = Unit.Parse("900mm");
            rgConciliacionPF.MasterTableView.ExportToPdf();

            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó conciliación PF");
        }

    }
    
    private void ObtenDatosPersonaMorales()
    {
        ActividadRules.GuardarActividad(7, this.idUs, "PM Conciliacion");        
        this.div_Conciliacion_PM.Visible = true;
        this.div_Conciliacion_PF.Visible = false;

        this.rgConciliacionPM.DataSource = null;
        this.rgConciliacionPM.DataBind();
        
        DataSet dsPM = new DataSet();        
        dsPM = ReporteConciliacionesDataAccess.Get_PMActualizado();
        
        LlenarTotal(dsPM.Tables[0], "MORAL");
        this.rgConciliacionPM.DataSource = dsPM;
        this.rgConciliacionPM.DataBind();
    }

    private void ObtenDatosPersonaFisica()
    {
        ActividadRules.GuardarActividad(6, this.idUs, "PF Conciliacion");      
        this.div_Conciliacion_PM.Visible = false;
        this.div_Conciliacion_PF.Visible = true;

        this.rgConciliacionPF.DataSource = null;
        this.rgConciliacionPF.DataBind();

        DataSet dsPF = new DataSet();
        //dsPF = ReporteConciliacionesDataAccess.Get_PFTest();
        dsPF = ReporteConciliacionesDataAccess.Get_PFActualizado();

        LlenarTotal(dsPF.Tables[0], "FISICA");
        this.rgConciliacionPF.DataSource = dsPF;
        this.rgConciliacionPF.DataBind();
    }

    private void LlenarTotal(DataTable dtPM, string persona)
    {
        decimal Defecto = 0;
        decimal SaldoVigenteSicreb = 0;
        decimal SaldoVencidoSicreb = 0;
        decimal SaldoBonoCuponCero = 0;
        decimal SaldoDiversoSicreb = 0;
        decimal SaldoTotalSICREB = 0;
        decimal SaldoVigenteSicofin = 0;
        decimal SaldoVencidoSicofin = 0;
        decimal SaldoDiversoSicofin = 0;
        decimal SaldoTotalSICOFIN = 0;
        decimal SaldoTotalDiferencia = 0;
        decimal SaldoRedondeo = 0;
        decimal SaldoConciliacion = 0;

        foreach (DataRow renglon in dtPM.Rows)
        {

            if (decimal.TryParse(renglon["SALDO_VIGENTE_SICREB"].ToString(), out Defecto))
            {
                SaldoVigenteSicreb += Convert.ToDecimal(renglon["SALDO_VIGENTE_SICREB"]);
            }

            if (decimal.TryParse(renglon["SALDO_VENCIDO_SICREB"].ToString(), out Defecto))
            {
                SaldoVencidoSicreb += Convert.ToDecimal(renglon["SALDO_VENCIDO_SICREB"]);
            }

            if (decimal.TryParse(renglon["SALDO_BONO_CUPON_CERO"].ToString(), out Defecto))
            {
                SaldoBonoCuponCero += Convert.ToDecimal(renglon["SALDO_BONO_CUPON_CERO"]);
            }

            if (decimal.TryParse(renglon["SALDO_TOTAL_SICREB"].ToString(), out Defecto))
            {
                SaldoTotalSICREB += Convert.ToDecimal(renglon["SALDO_TOTAL_SICREB"]);
            }

            if (decimal.TryParse(renglon["SALDO_VIGENTE_SICOFIN"].ToString(), out Defecto))
            {
                SaldoVigenteSicofin += Convert.ToDecimal(renglon["SALDO_VIGENTE_SICOFIN"]);
            }

            if (decimal.TryParse(renglon["SALDO_VENCIDO_SICOFIN"].ToString(), out Defecto))
            {
                SaldoVencidoSicofin += Convert.ToDecimal(renglon["SALDO_VENCIDO_SICOFIN"]);
            }

            if (decimal.TryParse(renglon["SALDO_DIVERSO_SICOFIN"].ToString(), out Defecto))
            {
                SaldoDiversoSicofin += Convert.ToDecimal(renglon["SALDO_DIVERSO_SICOFIN"]);
            }

            if (decimal.TryParse(renglon["SALDO_TOTAL_SICOFIN"].ToString(), out Defecto))
            {
                SaldoTotalSICOFIN += Convert.ToDecimal(renglon["SALDO_TOTAL_SICOFIN"]);
            }

            if (decimal.TryParse(renglon["SALDO_TOTAL_DIFERENCIA"].ToString(), out Defecto))
            {
                SaldoTotalDiferencia += Convert.ToDecimal(renglon["SALDO_TOTAL_DIFERENCIA"]);
            }

            if (decimal.TryParse(renglon["SALDO_REDONDEO"].ToString(), out Defecto))
            {
                SaldoRedondeo += Convert.ToDecimal(renglon["SALDO_REDONDEO"]);
            }

            if (decimal.TryParse(renglon["SALDO_TOTAL_CONCILIACION"].ToString(), out Defecto))
            {
                SaldoConciliacion += Convert.ToDecimal(renglon["SALDO_TOTAL_CONCILIACION"]);
            }

            if (decimal.TryParse(renglon["AUXILIAR"].ToString(), out Defecto))
            {
                renglon["AUXILIAR"] = "'" + renglon["AUXILIAR"];
            }

        }

        if (persona == "MORAL")
        {
            lblPMNumeroCreditos.Text = dtPM.Rows.Count.ToString();
            lblPMSaldoVigenteSicreb.Text = String.Format("{0:C2}", SaldoVigenteSicreb);
            lblPMSaldoVencidoSicreb.Text = String.Format("{0:C2}", SaldoVencidoSicreb);
            lblPMBonoCuponCero.Text = String.Format("{0:C2}", SaldoBonoCuponCero);
            lblPMSaldoTotalSICREB.Text = String.Format("{0:C2}", SaldoTotalSICREB);
            lblPMSaldoVigenteSicofin.Text = String.Format("{0:C2}", SaldoVigenteSicofin);
            lblPMSaldoVencidoSicofin.Text = String.Format("{0:C2}", SaldoVencidoSicofin);
            lblPMSaldoDiversoSicofin.Text = String.Format("{0:C2}", SaldoDiversoSicofin);
            lblPMSaldoTotalSICOFIN.Text = String.Format("{0:C2}", SaldoTotalSICOFIN);
            lblPMSaldoTotalDiferencia.Text = String.Format("{0:C2}", SaldoTotalDiferencia);
            lblPMSaldoRedondeo.Text = String.Format("{0:C2}", SaldoRedondeo);
            lblPMSaldoConciliacion.Text = String.Format("{0:C2}", SaldoConciliacion);
        }

        if (persona == "FISICA")
        {
            lblPFNumeroCreditos.Text = dtPM.Rows.Count.ToString();
            lblPFSaldoVigenteSicreb.Text =  String.Format("{0:C2}", SaldoVigenteSicreb);
            lblPFSaldoVencidoSicreb.Text =  String.Format("{0:C2}", SaldoVencidoSicreb);
            lblPFSaldoTotalSICREB.Text =  String.Format("{0:C2}", SaldoTotalSICREB);
            lblPFSaldoVigenteSicofin.Text = String.Format("{0:C2}", SaldoVigenteSicofin);
            lblPFSaldoVencidoSicofin.Text = String.Format("{0:C2}", SaldoVencidoSicofin);
            lblPFSaldoDiversoSicofin.Text = String.Format("{0:C2}", SaldoDiversoSicofin);
            lblPFSaldoTotalSICOFIN.Text =  String.Format("{0:C2}", SaldoTotalSICOFIN);
            lblPFSaldoTotalDiferencia.Text =  String.Format("{0:C2}", SaldoTotalDiferencia);
            lblPFSaldoRedondeo.Text = String.Format("{0:C2}", SaldoRedondeo);
            lblPFSaldoConciliacion.Text = String.Format("{0:C2}", SaldoConciliacion);
        }
        
        if (persona == "REPORTE")
        {
            DataRow renglon = dtPM.NewRow();
            renglon["NOMBRE_ACREDITADO"] = "Sumas Totales:";
            renglon["NUMERO_CREDITO"] =  dtPM.Rows.Count.ToString();
            renglon["SALDO_VIGENTE_SICREB"] =  SaldoVigenteSicreb.ToString();
            renglon["SALDO_VENCIDO_SICREB"] =  SaldoVencidoSicreb.ToString();
            renglon["SALDO_BONO_CUPON_CERO"] = SaldoBonoCuponCero.ToString();
            renglon["SALDO_TOTAL_SICREB"] =  SaldoTotalSICREB.ToString();
            renglon["SALDO_VIGENTE_SICOFIN"] = SaldoVigenteSicofin.ToString();
            renglon["SALDO_VENCIDO_SICOFIN"] = SaldoVencidoSicofin.ToString();
            renglon["SALDO_DIVERSO_SICOFIN"] = SaldoDiversoSicofin.ToString();
            renglon["SALDO_TOTAL_SICOFIN"] =  SaldoTotalSICOFIN.ToString();
            renglon["SALDO_TOTAL_DIFERENCIA"] = SaldoTotalDiferencia.ToString();
            renglon["SALDO_REDONDEO"] = SaldoRedondeo.ToString();
            renglon["SALDO_TOTAL_CONCILIACION"] = SaldoConciliacion.ToString(); 
            dtPM.Rows.Add(renglon);
        }

    }

    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();
        imgExportArchivoXLS.Visible = false;
        imgExportArchivoPDF.Visible = false;

        if (!Session["Facultades"].ToString().Contains(facultad.GetVariable("GRC")))
        {
            Response.Redirect("../inicio.aspx");
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ERC")))
        {
            imgExportArchivoXLS.Visible = true;
            imgExportArchivoPDF.Visible = true;
        }
    }

    private void CambiaAtributosRGR(GridFilterMenu menu)
    {
        for (int i = menu.Items.Count - 1; i >= 0; i--)
        {
            if (menu.Items[i].Text == "NoFilter")             { menu.Items[i].Text = "Sin Filtro"; }
            if (menu.Items[i].Text == "EqualTo")              { menu.Items[i].Text = "Igual"; }
            if (menu.Items[i].Text == "NotEqualTo")           { menu.Items[i].Text = "Diferente"; }
            if (menu.Items[i].Text == "GreaterThan")          { menu.Items[i].Text = "Mayor que"; }
            if (menu.Items[i].Text == "LessThan")             { menu.Items[i].Text = "Menor que"; }
            if (menu.Items[i].Text == "GreaterThanOrEqualTo") { menu.Items[i].Text = "Mayor o igual a"; }
            if (menu.Items[i].Text == "LessThanOrEqualTo")    { menu.Items[i].Text = "Menor o igual a"; }
            if (menu.Items[i].Text == "Between")              { menu.Items[i].Text = "Entre"; }
            if (menu.Items[i].Text == "NotBetween")           { menu.Items[i].Text = "No entre"; }
            if (menu.Items[i].Text == "IsNull")               { menu.Items[i].Text = "Es nulo"; }
            if (menu.Items[i].Text == "NotIsNull")            { menu.Items[i].Text = "No es nulo"; }
            if (menu.Items[i].Text == "Contains")             { menu.Items[i].Text = "Contenga"; }
            if (menu.Items[i].Text == "DoesNotContain")       { menu.Items[i].Text = "No Contenga"; }
            if (menu.Items[i].Text == "StartsWith")           { menu.Items[i].Text = "Inicie con"; }
            if (menu.Items[i].Text == "EndsWith")             { menu.Items[i].Text = "Finalice con"; }
            if (menu.Items[i].Text == "IsEmpty")              { menu.Items[i].Visible = false; }
            if (menu.Items[i].Text == "NotIsEmpty")           { menu.Items[i].Visible = false; }
        }
    }

}