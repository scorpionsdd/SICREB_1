using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Business;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Business.Repositorios;
using System.Data;
using Banobras.Credito.SICREB.Data;
using System.IO;
using System.Data.Common;
using Banobras.Credito.SICREB.Common;
using iTextSharp.text.html;
using iTextSharp.text;
using System.Text;


using iTextSharp.text.pdf;
using System.Web.UI.HtmlControls;
public partial class Catalogos_SEPOMEXPage : System.Web.UI.Page
{

    public const String catalog = "SEPOMEX";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["Facultades"] != null)
            {
                getFacultades();
                if (!this.Page.IsPostBack)
                {
                    CambiaAtributosRGR();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó a Catálogo " + catalog);

                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");

            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);

        }
    }
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdSEPOMEX.FilterMenu;
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
    protected void RgdSEPOMEX_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {


        try
        {
            int idUs;
            SEPOMEXRules smda = new SEPOMEXRules();
            var s = smda.GetSepomex();
            RgdSEPOMEX.DataSource = s;
            RgdSEPOMEX.VirtualItemCount = s.Count;



            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdSEPOMEX_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        RgdSEPOMEX.AllowPaging = false;
        RgdSEPOMEX.Rebind();
        string attachment = "attachment; filename=Export.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        // Create a form to contain the grid
        HtmlForm frm = new HtmlForm();
        RgdSEPOMEX.Parent.Controls.Add(frm);
        frm.Attributes["runat"] = "server";

        frm.Controls.Add(RgdSEPOMEX.MasterTableView);

        frm.RenderControl(htw);
        Response.Write(sw.ToString());
        RgdSEPOMEX.AllowPaging = true;
        RgdSEPOMEX.Rebind();
        Response.End();


    }
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        string[] nombres = new string[] { "REPORTE DE CONCILIACIÓN" };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {
            //this.RgdSEPOMEX.MasterTableView.AllowPaging = false;
            //this.RgdSEPOMEX.Rebind();
            //listaGrids.Add(this.RgdSEPOMEX);
            //string pdfScript = ExportarAPdf.ImprimirPdf(listaGrids, nombres, "REPORTE sepomex", WebConfig.Site);
            //this.RgdSEPOMEX.MasterTableView.AllowPaging = true;
            //this.RgdSEPOMEX.Rebind();
            //ClientScript.RegisterStartupScript(this.GetType(), "mykey", pdfScript, true);

            Response.ContentType = "application/force-download";

            RgdSEPOMEX.MasterTableView.HierarchyDefaultExpanded = true;
            RgdSEPOMEX.ExportSettings.OpenInNewWindow = false;
            RgdSEPOMEX.ExportSettings.ExportOnlyData = true;
            RgdSEPOMEX.MasterTableView.GridLines = GridLines.Both;
            RgdSEPOMEX.ExportSettings.IgnorePaging = true;
            RgdSEPOMEX.ExportSettings.OpenInNewWindow = true;
            RgdSEPOMEX.MasterTableView.ExportToPdf();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo SEPOMEX");
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        if (!Session["Facultades"].ToString().Contains(facultad.GetVariable("CCSEPO")))
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No tiene permiso para consultar catálogo SEPOMEX");
        }
        else
            valido = true;

        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }

    }

    protected void btn_cargar_sepomex_Click(object sender, EventArgs e)
    {
        cls_cargaSEPOMEX cargaMassiva = new cls_cargaSEPOMEX();
        try
        {

            if (file_txt_sepomex.HasFile)
            {


                DataTable dt_metaDataLayout = new DataTable();
                string ruta_archivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + file_txt_sepomex.FileName));
                file_txt_sepomex.SaveAs(ruta_archivo);
                DataTable dt_layout_procesado = new DataTable();

                DbTypeEquiva equiLE = new DbTypeEquiva();
                //      RgdSEPOMEX.MasterTableView.AllowPaging = false;


                //  RgdSEPOMEX.MasterTableView.AllowPaging = true;



                dt_layout_procesado = cargaMassiva.cargaSEPOMEX(ruta_archivo, "SP_CARGAMASIVA_SEPOMEX");
                int numeros = cargaMassiva.Correctos;

                if (cargaMassiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMassiva.Log.ToString().Split('\n'), Encoding.UTF8);
                } 
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMassiva.Errores > 0) ? "" + (cargaMassiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));

                int total = RgdSEPOMEX.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);
                RgdSEPOMEX.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMassiva.Log.ToString().Split('\n'), Encoding.UTF8);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMassiva.Errores > 0) ? "" + (cargaMassiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catchtry-Catch
    }
}
