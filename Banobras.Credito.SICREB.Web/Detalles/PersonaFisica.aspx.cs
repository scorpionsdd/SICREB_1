using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Business.Rules;
using Banobras.Credito.SICREB.Business.Rules.PF;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Entities.Util;
using Telerik.Web.UI;
using System.Text;

public partial class Detalles_PersonaFisica : System.Web.UI.Page
{
    private PF_Cinta cinta;
    private Archivo ultimoArchivo;
    private List<ErrorWarningInfo> erroresInfo;
    int idUs;

    //SICREB-INICIO-VHCC SEP-2012
    //DataTables encargados de contener la informacion de cada tipo Errores y Warnings.
    public System.Data.DataTable dtWarnings;
    public System.Data.DataTable dtErrores;
    //SICREB-FIN-VHCC SEP-2012

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session.Count == 0)
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }
            else
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(6, idUs, "Acceso al Menú detalle de Personas Fisicas");
                
                if (ultimoArchivo == null)
                {
                    Archivos_Rules archivos = new Archivos_Rules(Enums.Persona.Fisica);
                    ultimoArchivo = archivos.GetUltimoArchivo();

                    if (Session["idultimoArchivoPF"] == null)
                        Session.Add("idultimoArchivoPF", ultimoArchivo.Id);
                    else
                    {
                        if (Parser.ToNumber(Session["idultimoArchivoPF"]) != ultimoArchivo.Id)
                        {
                            Session["cintaPF"] = null;
                            Session["erroresInfoPF"] = null;
                        }
                    }
                }
                if (Session["cintaPF"] == null)
                {
                    PF_Cinta_Rules cintaRules = new PF_Cinta_Rules();
                    Session.Add("cintaPF", cintaRules.LoadCintaBD(ultimoArchivo.Id));
                }
                if (!IsPostBack)
                {
                    //JAGH se agregan actividades 16/01/13
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(6, idUs, "Acceso al Menú detalle de Personas Fisicas");
                    
                    if (ultimoArchivo != null)
                    {
                        lblErrores.Text = ultimoArchivo.Reg_Errores.ToString();
                        lblWarnings.Text = ultimoArchivo.Reg_Advertencias.ToString();
                        lblCorrectos.Text = ultimoArchivo.Reg_Correctos.ToString();
                        lblProcesados.Text = (ultimoArchivo.Reg_Correctos + ultimoArchivo.Reg_Errores + ultimoArchivo.Reg_Advertencias).ToString();
                        lblFechaArchivo.Text = ultimoArchivo.Fecha.ToShortDateString();
                    }
                    CambiaAtributosRGR(rgArchivoFisicas.FilterMenu);
                    //SICREB-INICIO-VHCC SEP-2012
                    //Se cambia el atributo de los dos RadGrid Errores y Warnings.
                    CambiaAtributosRGR(rgErrores.FilterMenu);
                    CambiaAtributosRGR(rgWarnings.FilterMenu);
                    //SICREB-FIN-VHCC SEP-2012
                }
                if (Session["erroresInfoPF"] == null)
                {
                    ErroresWarnings_Rules errores = new ErroresWarnings_Rules();
                    //SICREB-INICIO-VHCC SEP-2012
                    // Se Agregaron estas dos sesiones debido a que se realizan dos conultas.

                    //JAGH se llama grupos seleccionados y se modifica metodo 08/01/13
                    string strGrupos = "13,6378";  //Session["GruposSession"].ToString();
                    Session.Add("erroresInfoPF", errores.GetErroresPresentacion(Enums.Persona.Fisica, 1, strGrupos));
                    Session.Add("warningsInfoPF", errores.GetErroresPresentacion(Enums.Persona.Fisica, 0, strGrupos));
                    
                    //SICREB-FIN-VHCC SEP-2012
                }
                getFacultades();
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    

    }

    protected void rgArchivoFisicas_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (Session["cintaPF"] != null)
        {
            this.rgArchivoFisicas.MasterTableView.DataSource = ((PF_Cinta)Session["cintaPF"]).PNs;

            List<PF_PE> pes = new List<PF_PE>();
            List<PF_PA> pas = new List<PF_PA>();
            List<PF_TL> tls = new List<PF_TL>();

            foreach (PF_PN pn in ((PF_Cinta)Session["cintaPF"]).PNs)
            {
                pes.AddRange(pn.PEs);
                pas.AddRange(pn.PAs);
                tls.AddRange(pn.TLs);
            }

            this.rgArchivoFisicas.MasterTableView.DetailTables[0].DataSource = pas;
            this.rgArchivoFisicas.MasterTableView.DetailTables[1].DataSource = pes;
            this.rgArchivoFisicas.MasterTableView.DetailTables[2].DataSource = tls;
        }
    }

    protected void imgExportArchivoXLS_Click(object sender, ImageClickEventArgs e)
    {
        //logica para exportar!
      /*  rgArchivoFisicas.MasterTableView.HierarchyDefaultExpanded = true;
        rgArchivoFisicas.ExportSettings.OpenInNewWindow = false;
        rgArchivoFisicas.MasterTableView.ExportToExcel();*/
        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(8, idUs, "Exportación a Excel");

        rgArchivoFisicas.MasterTableView.AllowPaging = false;
        rgArchivoFisicas.Rebind();

        foreach (GridDataItem dgi in rgArchivoFisicas.MasterTableView.Items)
            dgi.Expanded = true;

        List<RadGrid> rg = new List<RadGrid>();
        rg.Add(rgArchivoFisicas);

        string[] titulares = new string[1];
        titulares[0] = "titulo";
        string excScript = ExportarExcel.GenerarExcelDetallesFisicas(rg, titulares, "Detalle Personas Fisicas", "Banobras");
        string fileName = string.Format("{0}{2}{1}", "PersonasFisicasDetalles", ".xls", DateTime.Now.ToString());

        rgArchivoFisicas.MasterTableView.AllowPaging = true;
        rgArchivoFisicas.Rebind();

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
        Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
        Response.Write(excScript);
        Response.End();
    }

    protected void imgExportArchivoPDF_Click(object sender, ImageClickEventArgs e)
    {
        //logica para exportar PDF
        string[] nombres = new string[] { "PROCESO DE PERSONAS FÍSICAS" };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {

            this.rgArchivoFisicas.MasterTableView.AllowPaging = false;
            this.rgArchivoFisicas.Rebind();
            listaGrids.Add(this.rgArchivoFisicas);
            string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, "PROCESO DE PERSONAS FÍSICAS", WebConfig.Site);
            this.rgArchivoFisicas.MasterTableView.AllowPaging = true;
            this.rgArchivoFisicas.Rebind();
            ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(11, idUs, "Exportación a PDF");
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
        //rgArchivoFisicas.MasterTableView.HierarchyDefaultExpanded = true;
        //rgArchivoFisicas.ExportSettings.OpenInNewWindow = true;
        //rgArchivoFisicas.MasterTableView.ExportToPdf();
    }

    //SICREB-INICIO-VHCC SEP-2012
    protected void rgErrores_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        dtErrores = Session["erroresInfoPF"] as System.Data.DataTable;
        try
        {
            this.rgErrores.DataSource = dtErrores;
        }
        catch (Exception ex) {
            string strMessageError = ex.Message.ToString();
        }
        //this.rgErrores.DataSource = (System.Data.DataTable)Session["erroresInfoPF"];
    }

    protected void rgWarnings_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e) 
    {
        dtWarnings = Session["warningsInfoPF"] as System.Data.DataTable;
        
        try
        {
            this.rgWarnings.DataSource = dtWarnings;
        }
        catch (Exception ex)
        {
            string strMessageError = ex.Message.ToString();
        }
        //this.rgWarnings.DataSource = (System.Data.DataTable)Session["warningsInfoPF"];
    }

    protected void imgExportWarningXLS_Click(object sender, ImageClickEventArgs e)
    {
        rgWarnings.MasterTableView.HierarchyDefaultExpanded = true;
        rgWarnings.ExportSettings.OpenInNewWindow = false;
        rgWarnings.MasterTableView.ExportToExcel();
        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(10, idUs, "Exportación a Excel");
    }

    protected void imgExportWarningPDF_Click(object sender, ImageClickEventArgs e)
    {
        //logica para exportar PDF
        string[] nombres = new string[] { "WARNINGS" };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {

            this.rgWarnings.MasterTableView.AllowPaging = false;
            this.rgWarnings.Rebind();
            listaGrids.Add(this.rgWarnings);
            string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, "WARNINGS", WebConfig.Site);
            this.rgWarnings.MasterTableView.AllowPaging = true;
            this.rgWarnings.Rebind();
            ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(11, idUs, "Exportación a PDF");
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
        //rgErroresAdv.MasterTableView.HierarchyDefaultExpanded = true;
        //rgErroresAdv.ExportSettings.OpenInNewWindow = true;
        //rgErroresAdv.MasterTableView.ExportToPdf();
    }

    protected void imgExportErrorXLS_Click(object sender, ImageClickEventArgs e)
    {
        rgErrores.MasterTableView.HierarchyDefaultExpanded = true;
        rgErrores.ExportSettings.OpenInNewWindow = false;
        rgErrores.MasterTableView.ExportToExcel();
        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(10, idUs, "Exportación a Excel");
    }

    protected void imgExportErrorPDF_Click(object sender, ImageClickEventArgs e)
    {
        //logica para exportar PDF
        string[] nombres = new string[] { "ERRORES" };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {

            this.rgErrores.MasterTableView.AllowPaging = false;
            this.rgErrores.Rebind();
            listaGrids.Add(this.rgErrores);
            string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, "ERRORES", WebConfig.Site);
            this.rgErrores.MasterTableView.AllowPaging = true;
            this.rgErrores.Rebind();
            ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(11, idUs, "Exportación a PDF");
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
        //rgErroresAdv.MasterTableView.HierarchyDefaultExpanded = true;
        //rgErroresAdv.ExportSettings.OpenInNewWindow = true;
        //rgErroresAdv.MasterTableView.ExportToPdf();
    }
    //SICREB-FIN-VHCC SEP-2012

    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();

        if (!Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("CDPF") + "|"))
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario Válido", "~/Login.aspx");
    }

    protected void rgArchivoFisicas_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridDataItem item = e.Item as GridDataItem;
        if (item != null && item.OwnerTableView.Name == "Creditos")
        {  
            // se modifica fecha por default por indicacion de usuario JAGH 01/04/13
            if (!string.IsNullOrWhiteSpace(item["FechaApertura"].Text) && item["FechaApertura"].Text.Trim().Length == 8)
            {
                if (item["FechaApertura"].Text.Equals("00000000") || item["FechaApertura"].Text.Equals("01010001")) 
                    item["FechaApertura"].Text = "01011900";

                item["FechaApertura"].Text = string.Format("{0}/{1}/{2}", item["FechaApertura"].Text.Trim().Substring(0, 2), item["FechaApertura"].Text.Trim().Substring(2, 2), item["FechaApertura"].Text.Trim().Substring(4, 4));
            }

            if (!string.IsNullOrWhiteSpace(item["FechaUltimoPago"].Text) && item["FechaUltimoPago"].Text.Trim().Length == 8)
            {
                if (item["FechaUltimoPago"].Text.Equals("00000000") || item["FechaUltimoPago"].Text.Equals("01010001"))
                    item["FechaUltimoPago"].Text = "01011900";

                item["FechaUltimoPago"].Text = string.Format("{0}/{1}/{2}", item["FechaUltimoPago"].Text.Trim().Substring(0, 2), item["FechaUltimoPago"].Text.Trim().Substring(2, 2), item["FechaUltimoPago"].Text.Trim().Substring(4, 4));
            }

            if (!string.IsNullOrWhiteSpace(item["FechaUltimaCompra"].Text) && item["FechaUltimaCompra"].Text.Trim().Length == 8)
            {
                if (item["FechaUltimaCompra"].Text.Equals("00000000") || item["FechaUltimaCompra"].Text.Equals("01010001"))
                    item["FechaUltimaCompra"].Text = "01011900";

                item["FechaUltimaCompra"].Text = string.Format("{0}/{1}/{2}", item["FechaUltimaCompra"].Text.Trim().Substring(0, 2), item["FechaUltimaCompra"].Text.Trim().Substring(2, 2), item["FechaUltimaCompra"].Text.Trim().Substring(4, 4));
            }

            if (!string.IsNullOrWhiteSpace(item["FechaCierre"].Text) && item["FechaCierre"].Text.Trim().Length == 8)
            {
                if (item["FechaCierre"].Text.Equals("00000000") || item["FechaCierre"].Text.Equals("01010001"))
                    item["FechaCierre"].Text = "01011900";

                item["FechaCierre"].Text = string.Format("{0}/{1}/{2}", item["FechaCierre"].Text.Trim().Substring(0, 2), item["FechaCierre"].Text.Trim().Substring(2, 2), item["FechaCierre"].Text.Trim().Substring(4, 4));
            }
            
            //FechaAmortVencida
            if (!string.IsNullOrWhiteSpace(item["TL_43"].Text) && item["TL_43"].Text.Trim().Length == 8)
            {
                if (item["TL_43"].Text.Equals("00000000") || item["TL_43"].Text.Equals("01010001"))
                    item["TL_43"].Text = "01011900";

                item["TL_43"].Text = string.Format("{0}/{1}/{2}", item["TL_43"].Text.Trim().Substring(0, 2), item["TL_43"].Text.Trim().Substring(2, 2), item["TL_43"].Text.Trim().Substring(4, 4));
            }

            //FechaTraspaso
            if (!string.IsNullOrWhiteSpace(item["TL_46"].Text) && item["TL_46"].Text.Trim().Length == 8)
            {
                if (item["TL_46"].Text.Equals("00000000") || item["TL_46"].Text.Equals("01010001"))
                    item["TL_46"].Text = "01011900";

                item["TL_46"].Text = string.Format("{0}/{1}/{2}", item["TL_46"].Text.Trim().Substring(0, 2), item["TL_46"].Text.Trim().Substring(2, 2), item["TL_46"].Text.Trim().Substring(4, 4));
            }
                                   

            item["Monto"].Text = string.Format("{0:c}", Parser.ToDouble(item["Monto"].Text));
            item["CreditoMaximo"].Text = string.Format("{0:c}", Parser.ToDouble(item["CreditoMaximo"].Text));
            item["SaldoActual"].Text = string.Format("{0:c}", Parser.ToDouble(item["SaldoActual"].Text));
            item["SaldoVencido"].Text = string.Format("{0:c}", Parser.ToDouble(item["SaldoVencido"].Text));
            item["SaldoInsoluto"].Text = string.Format("{0:c}", Parser.ToDouble(item["SaldoInsoluto"].Text));
            item["MontoIntereses"].Text = string.Format("{0:c}", Parser.ToDouble(item["MontoIntereses"].Text));
            
            if (item["TipoContrato"].Text == "RE")
                item["Garantia"].Text = "HIPOTECARIA";
            else
                item["Garantia"].Text = "PAGARE";
        }

        if (item != null && item.OwnerTableView.Name == "Direccion")
        {
            if (!string.IsNullOrWhiteSpace(item["FechaResidencia"].Text) && item["FechaResidencia"].Text.Trim().Length == 8)
            {
                if (item["FechaResidencia"].Text.Equals("00000000") || item["FechaResidencia"].Text.Equals("01010001"))
                    item["FechaResidencia"].Text = "01011900";

                item["FechaResidencia"].Text = string.Format("{0}/{1}/{2}", item["FechaResidencia"].Text.Trim().Substring(0, 2), item["FechaResidencia"].Text.Trim().Substring(2, 2), item["FechaResidencia"].Text.Trim().Substring(4, 4));
            }
        }

        if (item != null && item.OwnerTableView.Name == "Nombre")
        {
            if (!string.IsNullOrWhiteSpace(item["FechaNacimiento"].Text) && item["FechaNacimiento"].Text.Trim().Length == 8)
            {
                if (item["FechaNacimiento"].Text.Equals("00000000") || item["FechaNacimiento"].Text.Equals("01010001"))
                    item["FechaNacimiento"].Text = "01011900";

                item["FechaNacimiento"].Text = string.Format("{0}/{1}/{2}", item["FechaNacimiento"].Text.Trim().Substring(0, 2), item["FechaNacimiento"].Text.Trim().Substring(2, 2), item["FechaNacimiento"].Text.Trim().Substring(4, 4));
            }
            //JAGH 03/04/13 se agrega fecha de fallecimiento
            if (!string.IsNullOrWhiteSpace(item["FechaDefuncion"].Text) && item["FechaDefuncion"].Text.Trim().Length == 8)
            {
                if (item["FechaDefuncion"].Text.Equals("00000000") || item["FechaDefuncion"].Text.Equals("01010001"))
                    item["FechaDefuncion"].Text = "01011900";

                item["FechaDefuncion"].Text = string.Format("{0}/{1}/{2}", item["FechaDefuncion"].Text.Trim().Substring(0, 2), item["FechaDefuncion"].Text.Trim().Substring(2, 2), item["FechaDefuncion"].Text.Trim().Substring(4, 4));
            }
            
        }
    }

    public void CambiaAtributosRGR(GridFilterMenu menu)
    {
        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (menu.Items[i].Text.Equals("NoFilter"))
                menu.Items[i].Text = "Sin Filtro";
            else if (menu.Items[i].Text.Equals("EqualTo"))
                menu.Items[i].Text = "Igual";
            else if (menu.Items[i].Text.Equals("NotEqualTo"))
                menu.Items[i].Text = "Diferente";
            else if (menu.Items[i].Text.Equals("GreaterThan"))
                menu.Items[i].Text = "Mayor que";
            else if (menu.Items[i].Text.Equals("LessThan"))
                menu.Items[i].Text = "Menor que";
            else if (menu.Items[i].Text.Equals("GreaterThanOrEqualTo"))
                menu.Items[i].Text = "Mayor o igual a";
            else if (menu.Items[i].Text.Equals("LessThanOrEqualTo"))
                menu.Items[i].Text = "Menor o igual a";
            else if (menu.Items[i].Text.Equals("Between"))
                menu.Items[i].Text = "Entre";
            else if (menu.Items[i].Text.Equals("NotBetween"))
                menu.Items[i].Text = "No entre";
            else if (menu.Items[i].Text.Equals("IsNull"))
                menu.Items[i].Text = "Es nulo";
            else if (menu.Items[i].Text.Equals("NotIsNull"))
                menu.Items[i].Text = "No es nulo";
            else if (menu.Items[i].Text.Equals("Contains"))
                menu.Items[i].Text = "Contenga";
            else if (menu.Items[i].Text.Equals("DoesNotContain"))
                menu.Items[i].Text = "No Contenga";
            else if (menu.Items[i].Text.Equals("StartsWith"))
                menu.Items[i].Text = "Inicie con";
            else if (menu.Items[i].Text.Equals("EndsWith"))
                menu.Items[i].Text = "Finalice con";
            else if (menu.Items[i].Text.Equals("NotIsEmpty"))
                menu.Items[i].Text = "No es vacio";
            else if (menu.Items[i].Text.Equals("IsEmpty"))
                menu.Items[i].Text = "Vacio";
        }
    }

    // conservar nombres de filtros en español al filtrar dato
    protected void grids_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //sobre filtro y paginacion
        if (e.CommandName == RadGrid.FilterCommandName || e.CommandName == RadGrid.PageCommandName || e.CommandName.Equals("ChangePageSize") ||
            e.CommandName == RadGrid.PrevPageCommandArgument || e.CommandName == RadGrid.NextPageCommandArgument ||
            e.CommandName == RadGrid.FirstPageCommandArgument || e.CommandName == RadGrid.LastPageCommandArgument)
        {
            RadGrid grid = (RadGrid)sender;
            CambiaAtributosRGR(grid.FilterMenu);
        }
       
    }


}
