using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Business.Repositorios;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities.Util;

public partial class Catalogos_Bitacora : System.Web.UI.Page
{

    public const String catalog = "Bitácora";
    private ActividadRules act;
    protected void Page_Load(object sender, EventArgs e)
    {
       // this.RgdBitacora.Visible = true;
        //this.RgdBitacoraDatos.Visible = true;
        CambiaAtributosRGR();
            
    }
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdBitacora.FilterMenu;

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

        GridFilterMenu menu2 = RgdBitacoraDatos.FilterMenu;

        for (int i = menu2.Items.Count - 1; i >= 0; i--)
        {
            if (menu2.Items[i].Text == "NoFilter")
            {
                menu2.Items[i].Text = "Sin Filtro";
            }
            else if (menu2.Items[i].Text == "EqualTo")
            {
                menu2.Items[i].Text = "Igual";
            }
            else if (menu2.Items[i].Text == "NotEqualTo")
            {
                menu2.Items[i].Text = "Diferente";
            }
            else if (menu2.Items[i].Text == "GreaterThan")
            {
                menu2.Items[i].Text = "Mayor que";
            }
            else if (menu2.Items[i].Text == "LessThan")
            {
                menu2.Items[i].Text = "Menor que";
            }
            else if (menu2.Items[i].Text == "GreaterThanOrEqualTo")
            {
                menu2.Items[i].Text = "Mayor o igual a";
            }
            else if (menu2.Items[i].Text == "LessThanOrEqualTo")
            {
                menu2.Items[i].Text = "Menor o igual a";
            }
            else if (menu2.Items[i].Text == "Between")
            {
                menu2.Items[i].Text = "Entre";
            }
            else if (menu2.Items[i].Text == "NotBetween")
            {
                menu2.Items[i].Text = "No entre";
            }
            else if (menu2.Items[i].Text == "IsNull")
            {
                menu2.Items[i].Text = "Es nulo";
            }
            else if (menu2.Items[i].Text == "NotIsNull")
            {
                menu2.Items[i].Text = "No es nulo";
            }
            else if (menu2.Items[i].Text == "Contains")
            {
                menu2.Items[i].Text = "Contenga";
            }
            else if (menu2.Items[i].Text == "DoesNotContain")
            {
                menu2.Items[i].Text = "No Contenga";
            }
            else if (menu2.Items[i].Text == "StartsWith")
            {
                menu2.Items[i].Text = "Inicie con";
            }
            else if (menu2.Items[i].Text == "EndsWith")
            {
                menu2.Items[i].Text = "Finalice con";
            }
            else if (menu2.Items[i].Text == "IsEmpty")
            {
                // item.Text = "Finalice con";
                menu2.Items[i].Visible = false;
            }
            else if (menu2.Items[i].Text == "NotIsEmpty")
            {
                // item.Text = "Finalice con";
                menu2.Items[i].Visible = false;
            }
        }

    }
    protected void txtFechaInicial_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {

    }
    protected void txtFechaFinal_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (txtFechaFinal.SelectedDate < txtFechaInicial.SelectedDate)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La fecha final debe ser mayor a la fecha inicial.", "~/Login.aspx");
            txtFechaFinal.SelectedDate = null;
        }
    }
    protected void btnExportPDFBitacora_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdBitacora.MasterTableView.HierarchyDefaultExpanded = true;
        RgdBitacora.ExportSettings.OpenInNewWindow = false;
        RgdBitacora.ExportSettings.ExportOnlyData = true;
        RgdBitacora.MasterTableView.BorderWidth = 2;
        RgdBitacora.MasterTableView.GridLines = GridLines.Both;
        RgdBitacora.ExportSettings.IgnorePaging = true;
        RgdBitacora.ExportSettings.OpenInNewWindow = true;
        RgdBitacora.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdBitacora.ExportSettings.Pdf.PageWidth = Unit.Parse("700mm");
        RgdBitacora.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }

    protected void btnExportPDFDatos_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdBitacoraDatos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdBitacoraDatos.ExportSettings.OpenInNewWindow = false;
        RgdBitacoraDatos.ExportSettings.ExportOnlyData = true;


        RgdBitacoraDatos.ExportSettings.IgnorePaging = true;
        RgdBitacoraDatos.ExportSettings.OpenInNewWindow = true;
        RgdBitacoraDatos.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdBitacoraDatos.ExportSettings.Pdf.PageWidth = Unit.Parse("700mm");
        RgdBitacoraDatos.MasterTableView.BorderStyle = BorderStyle.Solid;

        RgdBitacoraDatos.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void btnExportExcelDatos_Click1(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";

        RgdBitacoraDatos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdBitacoraDatos.ExportSettings.OpenInNewWindow = false;
        RgdBitacoraDatos.ExportSettings.ExportOnlyData = true;

        RgdBitacoraDatos.ExportSettings.IgnorePaging = true;
        RgdBitacoraDatos.ExportSettings.OpenInNewWindow = true;
        RgdBitacoraDatos.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);


    }

    protected void btnExportExcelBitacora_Click1(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";

        RgdBitacora.MasterTableView.HierarchyDefaultExpanded = true;
        RgdBitacora.ExportSettings.OpenInNewWindow = false;
        RgdBitacora.ExportSettings.ExportOnlyData = true;

        RgdBitacora.ExportSettings.IgnorePaging = true;
        RgdBitacora.ExportSettings.OpenInNewWindow = true;
        RgdBitacora.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);


    }
    protected void RgdBitacora_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (txtFechaInicial.SelectedDate.HasValue && txtFechaFinal.SelectedDate.HasValue)
        {
            if (txtFechaFinal.SelectedDate > txtFechaInicial.SelectedDate)
            {
                act = new ActividadRules();
                this.RgdBitacora.DataSource = act.GetActividades(txtFechaInicial.SelectedDate.Value.ToString(), txtFechaFinal.SelectedDate.Value.AddDays(1).ToString());
                this.RgdBitacoraDatos.DataSource = act.GetActividadesDatos(txtFechaInicial.SelectedDate.Value.ToString(), txtFechaFinal.SelectedDate.Value.AddDays(1).ToString());
            }
        }
    }
    protected void RgdBitacora_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }
    //protected void btnBuscar_Click(object sender, EventArgs e)
    //{
    //    act = new ActividadRules();
    //     CambiaAtributosRGR();
    //int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
    // ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingreso a Catalogo " + catalog);
    //    try
    //    {
    //        DateTime dimeInicial = txtFechaInicial.SelectedDate.Value;
    //        //FIX se le agrega un día para que encuentre lo que seleccionó el cliente
    //        DateTime dimeFinal = txtFechaFinal.SelectedDate.Value.AddDays(1);
    //        this.RgdBitacora.DataSource = act.GetActividades(dimeInicial.ToString(), dimeFinal.ToString());
    //        this.RgdBitacora.DataBind();
    //        this.RgdBitacoraDatos.DataSource = act.GetActividadesDatos(dimeInicial.ToString(), dimeFinal.ToString());
    //        this.RgdBitacoraDatos.DataBind();

    //    }
    //    catch (Exception ex)
    //    {
    //        Mensajes.ShowError(this.Page, this.GetType(), ex);
    //    }

    //}
    protected void RgdBitacora_ItemCreated(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridFilteringItem)
        //{
        //    GridFilteringItem filteringItem = e.Item as GridFilteringItem;
        //    //set dimensions for the filter textbox  
        //    TextBox boxIdUser = filteringItem["ID_USUARIO"].Controls[0] as TextBox;
        //    TextBox boxRol = filteringItem["ROL"].Controls[0] as TextBox;
        //    boxIdUser.Width = Unit.Pixel(50);
        //    boxRol.Width = Unit.Pixel(60); 
        //}
    }
}