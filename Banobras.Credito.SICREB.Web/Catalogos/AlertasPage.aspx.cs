using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Entities.Util;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Business.Repositorios;
using System.Collections;
using Banobras.Credito.SICREB.Common;
using System.Data;
using Banobras.Credito.SICREB.Data;
using System.Data.Common;
using System.IO;
public partial class Catalogos_AlertasPage : System.Web.UI.Page
{
    public const String catalog = "Alertas";
    int idUs;
    string persona;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (Session["Facultades"] != null)
            {
                CambiaAtributosRGR();
                persona = "PM";
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                   
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + " Ingresó a Catálogo " + catalog);
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");

            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), "Load :"+ ex.Message);
        }
    }
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdALERTAS.FilterMenu;
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
                menu.Items[i].Visible = false;
            }
            else if (menu.Items[i].Text == "NotIsEmpty")
            {
                menu.Items[i].Visible = false;
            }         
        }

    }
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdALERTAS.Columns[0].Visible = false;
        RgdALERTAS.Columns[RgdALERTAS.Columns.Count - 1].Visible = false;
        RgdALERTAS.MasterTableView.HierarchyDefaultExpanded = true;
        RgdALERTAS.ExportSettings.OpenInNewWindow = false;
        RgdALERTAS.ExportSettings.ExportOnlyData = true;
        RgdALERTAS.MasterTableView.GridLines = GridLines.Both;
        RgdALERTAS.ExportSettings.IgnorePaging = true;
        RgdALERTAS.ExportSettings.OpenInNewWindow = true;
        RgdALERTAS.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdALERTAS.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdALERTAS.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el catálogo " + catalog);

    }
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdALERTAS.Columns[0].Visible = false;
        RgdALERTAS.Columns[RgdALERTAS.Columns.Count - 1].Visible = false;
        RgdALERTAS.MasterTableView.HierarchyDefaultExpanded = true;
        RgdALERTAS.ExportSettings.OpenInNewWindow = false;
        RgdALERTAS.ExportSettings.ExportOnlyData = true;

        RgdALERTAS.ExportSettings.IgnorePaging = true;
        RgdALERTAS.ExportSettings.OpenInNewWindow = true;
        RgdALERTAS.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el catálogo " + catalog);

    }
    protected void RgdALERTAS_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;
            int id = Parser.ToNumber(item["id"].Text);
            //MonedasRules mr = new MonedasRules();            
            //if (mr.BorrarMoneda(new Moneda(id, 0, 0, "", Enums.Estado.Inactivo)) > 0)

            AlertasRules mr = new AlertasRules();
            if (mr.BorrarAlerta(new Alerta(id, "", "", "", "", "", "")) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                //JAGH se agrega actividad 20/01/13
                ActividadRules.GuardarActividad(1111, idUs, "El registro fue eliminado catálogo alertas");

                Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
            }

            else
            {
                //JAGH se agrega actividad 20/01/13
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(800, idUs, "El registro no fue eliminado catálogo alertas");

                Mensajes.ShowAdvertencia(Page, this.GetType(), "No se eliminaron los datos");
            }
        }
        catch (Exception ex)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue eliminado catálogo alertas");

            Mensajes.ShowError(Page, this.GetType(), ex);
        }
    }
    protected void RgdALERTAS_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdALERTAS_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem objetoGrid = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            objetoGrid.ExtractValues(nuevosValores);
            GridDataItem item;
            item = (GridDataItem)e.Item;
            RadComboBox cper;
            cper = (RadComboBox)item["DescripcionPeriodo"].FindControl("ComboPeriodo");
            RadComboBox cap;
            cap = (RadComboBox)item["descripcionAplicaPeriodo"].FindControl("ComboDiaPeriodo");
            CheckBox chek;
            chek = (CheckBox)item["DescripcionActivadaSINO"].FindControl("chkAlerta");

            if (nuevosValores["IdententifadorAlerta"] != null && nuevosValores["TituloAlerta"] != null)
            {
                if (bValidaCampoVacio(nuevosValores["IdententifadorAlerta"].ToString(), "Idententifador Alerta", e) && bValidaCampoVacio(nuevosValores["TituloAlerta"].ToString(), "Titulo Alerta", e) && bValidaCampoVacio(nuevosValores["Mensaje"].ToString(), "Mensaje", e))
                {
                    string idententifadorAlerta = nuevosValores["IdententifadorAlerta"].ToString();
                    string tituloAlerta = nuevosValores["TituloAlerta"].ToString();
                    string mensaje = nuevosValores["Mensaje"].ToString();
                    string periodicidad = cper.SelectedItem.Text;
                    string aplicacionDePeriodicidad = cap.SelectedItem.Text;
                    string alarmaActivada = "0";

                    if (chek.Checked)
                    {
                        alarmaActivada = "1";
                    }

                    AlertasRules ar = new AlertasRules();
                    List<Alerta> al = ar.Alertas();
                    if (al.FindIndex(delegate(Alerta alrt) { return alrt.IdententifadorAlerta == idententifadorAlerta; }) < 0)
                    {

                        Alerta alerta = new Alerta(0, idententifadorAlerta, tituloAlerta, mensaje, periodicidad, aplicacionDePeriodicidad, alarmaActivada);
                        if (ar.InsertarAlerta(alerta) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han guardado de forma correcta");
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro ", null, null, catalog, 1, null, nuevosValores);
                        }
                        else
                        {
                            //JAGH se agrega actividad 20/01/13
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(800, idUs, "El registro no fue agregado catálogo alertas");

                            Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo guardar los datos");
                        }
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(Page, this.GetType(), "Ya existe el Identificador de Alerta.");
                        e.Canceled = true;
                    }
                    
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores ingresados no válidos");
                e.Canceled = true;
            }

        }
        catch (Exception ex)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue agregado catálogo alertas");

            Mensajes.ShowError(Page, this.GetType(), ex);
        }
    }

    public bool bValidaCampoVacio(string Campo, string NombreCampo, Telerik.Web.UI.GridCommandEventArgs e)
    {

        int longitud = 0;

        for (int n = 0; n < Campo.Length; n++)
        {

            if (Char.IsWhiteSpace(Campo, n))
            {

                longitud++;

            }
        }

        if (longitud == Campo.Length)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El campo " + NombreCampo + " debe tener datos, favor de verificar.");

            e.Canceled = true;

            return false;
        }

        return true;
    }

    protected void RgdALERTAS_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            AlertasRules mr = new AlertasRules();
            var s = mr.Alertas();
            RgdALERTAS.DataSource = s;
            RgdALERTAS.VirtualItemCount = s.Count;
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), "RgdALERTAS_NeedDataSource: " + ex.Message);
        }
    }
    protected void RgdALERTAS_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            GridEditableItem objetoGrid = e.Item as GridEditableItem;
            bool bValidar = true;

            objetoGrid.ExtractValues(nuevosValores);
            GridDataItem items;
            items = (GridDataItem)e.Item;
            RadComboBox cper;
            cper = (RadComboBox)items["DescripcionPeriodo"].FindControl("ComboPeriodo");
            RadComboBox cap;
            cap = (RadComboBox)items["descripcionAplicaPeriodo"].FindControl("ComboDiaPeriodo");
            CheckBox chek;
            chek = (CheckBox)items["DescripcionActivadaSINO"].FindControl("chkAlerta");

            editedItem.ExtractValues(nuevosValores);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;
            string AlertaOld = oldValues["IdententifadorAlerta"].ToString();

            if (nuevosValores["IdententifadorAlerta"] != null && nuevosValores["TituloAlerta"] != null)
            {
                if (bValidaCampoVacio(nuevosValores["IdententifadorAlerta"].ToString(), "Idententifador Alerta", e) && bValidaCampoVacio(nuevosValores["TituloAlerta"].ToString(), "Titulo Alerta", e)&& bValidaCampoVacio(nuevosValores["Mensaje"].ToString(), "Mensaje", e))
                {
                    string idententifadorAlerta = nuevosValores["IdententifadorAlerta"].ToString();

                    if ( AlertaOld != idententifadorAlerta)
                    {
                        AlertasRules ar = new AlertasRules();
                        List<Alerta> al = ar.Alertas();

                        if (al.FindIndex(delegate(Alerta alrt) { return alrt.IdententifadorAlerta == idententifadorAlerta; }) >= 0)
                        {
                            bValidar = false;
                            Mensajes.ShowAdvertencia(Page, this.GetType(), "Ya existe el Identificador de Alerta.");
                            e.Canceled = true;
                        }
                    }                 


                          if (bValidar)
                          {
                              int id = Parser.ToNumber(nuevosValores["ID"].ToString());

                              string tituloAlerta = nuevosValores["TituloAlerta"].ToString();
                              string mensaje = nuevosValores["Mensaje"].ToString();
                              string periodicidad = cper.SelectedItem.Text;
                              string aplicacionDePeriodicidad = cap.SelectedItem.Text;
                              string alarmaActivada = "0";

                              if (chek.Checked)
                              {
                                  alarmaActivada = "1";
                              }

                              AlertasRules ar = new AlertasRules();
                              //List<Alerta> al = ar.Alertas();
                              //if (al.FindIndex(delegate(Alerta alrt) { return (alrt.IdententifadorAlerta == idententifadorAlerta && alrt.Id != id); }) < 0)
                              //{
                              Alerta alerta = new Alerta(id, idententifadorAlerta, tituloAlerta, mensaje, periodicidad, aplicacionDePeriodicidad, alarmaActivada);
                              if (ar.ActulizarAlerta(alerta) > 0)
                              {
                                  idUs = Parser.ToNumber(Session["idUsuario"].ToString());


                                  ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro ", null, null, catalog, 1, oldValues, nuevosValores);
                                  RgdALERTAS.Columns[6].Visible = false; //PERIODICIDAD

                                  Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han actualizado de forma correcta");

                              }
                              else
                              {
                                  //JAGH se agrega actividad 20/01/13
                                  idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                                  ActividadRules.GuardarActividad(800, idUs, "El registro no fue modificado catálogo alertas");

                                  Mensajes.ShowAdvertencia(Page, this.GetType(), "No se guardaron los datos");
                              }
                          }
                    //}
                    //else
                    //    Mensajes.ShowAdvertencia(Page, this.GetType(), "Ya existe la clave");
                }

            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores ingresados no válidos");
                e.Canceled = true;
            }
        }
        catch (Exception ex)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue modificado catálogo alertas");

            Mensajes.ShowError(Page, this.GetType(), ex);
        }
    }
    protected void RgdALERTAS_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {

    }
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdALERTAS.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdALERTAS.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TIPOMONEDA")))
            {
                this.RgdALERTAS.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPOMONEDA")))
            {
                //this.RgdALERTAS.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TIPOMONEDA")))
            {
                this.RgdALERTAS.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPOMONEDA")))
            {
                //this.RgdALERTAS.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }
    }

    protected void RgdALERTAS_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            GridDataItem item;
            RadComboBox ComboDiaPeriodo;
            CheckBox chkAlerta;
            if (e.Item is GridCommandItem)
            {
                Button addButton = e.Item.FindControl("AddNewRecordButton") as Button;
                LinkButton lnkButton = (LinkButton)e.Item.FindControl("InitInsertButton");
                if (facultadInsertar())
                {
                    addButton.Visible = true;
                    lnkButton.Visible = true;
                }
                else
                {
                    addButton.Visible = false;
                    lnkButton.Visible = false;
                }

                CheckBox chk = e.Item.FindControl("chk") as CheckBox;


            }//IF e.item

            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                item = (GridDataItem)e.Item;
                ComboDiaPeriodo = (RadComboBox)item["AplicaPeriodo"].FindControl("ComboDiaPeriodo");

                GridDataItem dataItem = e.Item as GridDataItem;

                int bool_activada = 0;
                int.TryParse(dataItem["DescripcionActivadaSINO"].Text, out bool_activada);
                chkAlerta = (CheckBox)item["ActivadaSINO"].FindControl("chkAlerta");

                chkAlerta.Checked = (bool_activada == 1);

            }
            else if (e.Item.IsInEditMode) //(e.Item.ItemType == GridItemType.CommandItem)
            {
                item = (GridDataItem)e.Item;
                //Periodicidad
                RadComboBox ComboPeriodo = (RadComboBox)item["Periodo"].FindControl("ComboPeriodo");
                Label lblPeriodicidad = (Label)item["Periodo"].FindControl("lblPeriodicidad");
                ComboPeriodo.Visible = true;
                lblPeriodicidad.Visible = false;
                //aplicacion de periodo
                ComboDiaPeriodo = (RadComboBox)item["AplicaPeriodo"].FindControl("ComboDiaPeriodo");
                Label lblDiaPeriodo = (Label)item["AplicaPeriodo"].FindControl("lblDiaPeriodo");
                ComboDiaPeriodo.Visible = true;
                lblDiaPeriodo.Visible = false;

                this.RgdALERTAS.MasterTableView.GetColumn("AplicaPeriodo").Visible = true;
                this.RgdALERTAS.MasterTableView.GetColumn("descripcionAplicaPeriodo").Visible = false;

                this.RgdALERTAS.MasterTableView.GetColumn("Periodo").Visible = true;
                this.RgdALERTAS.MasterTableView.GetColumn("DescripcionPeriodo").Visible = false;

            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), "RgdALERTAS_ItemDataBound" + ex.Message);

        }
    }
    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_TIPOMONEDA")))
        {
            valido = true;
        }
        return valido;
    }



    protected void ComboDiaPeriodo_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {

            RadComboBox ComboPeriodo = (RadComboBox)sender;
            string periodoStr = ComboPeriodo.SelectedValue;

            GridDataItem item = (GridDataItem)ComboPeriodo.NamingContainer;

            string keyvalue = item.GetDataKeyValue("ID").ToString();

            if (1 == int.Parse(periodoStr))
            {

                Response.Write(periodoStr);

            }
            else if (2 == int.Parse(periodoStr))
            {

                Response.Write(periodoStr);

            }

        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }//try catch

        //ComboPeriodo_SelectedIndexChanged
    }

    protected void ComboPeriodo_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {

            RadComboBox ComboPeriodo = (RadComboBox)sender;
            string periodoStr = ComboPeriodo.SelectedValue;

            GridDataItem item = (GridDataItem)ComboPeriodo.NamingContainer;

            RadComboBox ComboDiaPeriodo = (RadComboBox)item.FindControl("ComboDiaPeriodo");

            string diaSeleccionado = ComboDiaPeriodo.SelectedItem.ToString();

            DataTable dt_dias = new DataTable();
            dt_dias.Columns.Add("Id");
            dt_dias.Columns.Add("Dia");

            if (2 == int.Parse(periodoStr))
            {


                DataRow[] rowDia_mes = new DataRow[32];


                for (int dia_i = 1; dia_i <= 31; dia_i++)
                {


                    rowDia_mes[dia_i] = dt_dias.NewRow();
                    rowDia_mes[dia_i]["Id"] = dia_i.ToString();
                    rowDia_mes[dia_i]["Dia"] = dia_i.ToString();

                    dt_dias.Rows.Add(rowDia_mes[dia_i]);


                }//for dia

                ComboDiaPeriodo.DataSource = dt_dias;
                ComboDiaPeriodo.DataTextField = "Dia";
                ComboDiaPeriodo.DataValueField = "Id";
                ComboDiaPeriodo.DataBind();


            }
            else if (1 == int.Parse(periodoStr))
            {



                DataRow rowDia1 = dt_dias.NewRow();

                rowDia1["Id"] = "1";
                rowDia1["Dia"] = "Lunes";

                DataRow rowDia2 = dt_dias.NewRow();

                rowDia2["Id"] = "2";
                rowDia2["Dia"] = "Martes";

                DataRow rowDia3 = dt_dias.NewRow();

                rowDia3["Id"] = "3";
                rowDia3["Dia"] = "Miercoles";

                DataRow rowDia4 = dt_dias.NewRow();

                rowDia4["Id"] = "4";
                rowDia4["Dia"] = "Jueves";

                DataRow rowDia5 = dt_dias.NewRow();

                rowDia5["Id"] = "5";
                rowDia5["Dia"] = "Viernes";

                DataRow rowDia6 = dt_dias.NewRow();

                rowDia6["Id"] = "6";
                rowDia6["Dia"] = "Sabado";

                DataRow rowDia7 = dt_dias.NewRow();

                rowDia7["Id"] = "7";
                rowDia7["Dia"] = "Domingo";

                dt_dias.Rows.Add(rowDia1);
                dt_dias.Rows.Add(rowDia2);
                dt_dias.Rows.Add(rowDia3);
                dt_dias.Rows.Add(rowDia4);
                dt_dias.Rows.Add(rowDia5);
                dt_dias.Rows.Add(rowDia6);
                dt_dias.Rows.Add(rowDia7);



                ComboDiaPeriodo = (RadComboBox)item["AplicaPeriodo"].FindControl("ComboDiaPeriodo");



                ComboDiaPeriodo.DataSource = dt_dias;
                ComboDiaPeriodo.DataTextField = "Dia";
                ComboDiaPeriodo.DataValueField = "Id";
                ComboDiaPeriodo.DataBind();





            }




        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }//try catch

        //ComboPeriodo_SelectedIndexChanged
    }


    protected void btn_cargar_Click(object sender, EventArgs e)
    {
        cls_cargaArchivos cargaMasiva = new cls_cargaArchivos();
        try
        {

            if (file_txt_layout.HasFile)
            {


                DataTable dt_metaDataLayout = new DataTable();
                string ruta_archivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + file_txt_layout.FileName));

                file_txt_layout.SaveAs(ruta_archivo);





                DataTable dt_layout_procesado = new DataTable();


                dt_metaDataLayout.Columns.Add("nombreColumna");
                dt_metaDataLayout.Columns.Add("tipoDato");
                dt_metaDataLayout.Columns.Add("longitud");



                DataRow row1;

                row1 = dt_metaDataLayout.NewRow();
                row1["nombreColumna"] = "Identificador de Alerta";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Título Alerta";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "2";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Mensaje";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "30";

                DataRow row4;

                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "Periodicidad";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "30";


                DataRow row5;

                row5 = dt_metaDataLayout.NewRow();
                row5["nombreColumna"] = "Aplicación de periodicidad";
                row5["tipoDato"] = "VARCHAR2";
                row5["longitud"] = "30";

                DataRow row6;

                row6 = dt_metaDataLayout.NewRow();
                row6["nombreColumna"] = "Activar Alerta";
                row6["tipoDato"] = "VARCHAR2";
                row6["longitud"] = "30";


                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);

                dt_metaDataLayout.Rows.Add(row4);
                dt_metaDataLayout.Rows.Add(row5);
                dt_metaDataLayout.Rows.Add(row6);

                //List<DbParameter> parametros = new List<DbParameter>();

                DbParameter[] parametros = new DbParameter[9];

                parametros[0] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[0].Direction = ParameterDirection.Output;
                parametros[0].ParameterName = "id_OUT";
                parametros[0].DbType = DbType.Decimal;
                parametros[0].Size = 38;

                parametros[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[1].Direction = ParameterDirection.Output;
                parametros[1].ParameterName = "tipo_OUT";
                parametros[1].DbType = DbType.Decimal;
                parametros[1].Size = 38;

                parametros[2] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].ParameterName = "ppersona";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "identificadorp";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 1000;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "titulop";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 100;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "mensajep";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 4000;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "periodicidadp";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 100;

                parametros[7] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].ParameterName = "fechap";
                parametros[7].DbType = DbType.String;
                parametros[7].Size = 50;

                parametros[8] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[8].Direction = ParameterDirection.Input;
                parametros[8].ParameterName = "activop";
                parametros[8].DbType = DbType.String;
                parametros[8].Size = 5;

                string storeBase = "SP_cargaMasiva_Alerta";

                dt_layout_procesado = cargaMasiva.cargaMasiva("alertas", dt_metaDataLayout, ruta_archivo, " [Identificador de Alerta], [Titulo Alerta], [Mensaje], [Periodicidad], [Aplicación de periodicidad], [Activar Alerta] ", parametros, storeBase, persona, " WHERE Len([Identificador de Alerta]) > 1 ");
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                    //TODO: aplicar el session.id para el nombrado de archivos LOG! ya que en un momento pueden 2 o mas usuarios acceder al ultimo error y no necesariamente al suyo!!!
                    //System.IO.File.WriteAllLines(Server.MapPath("../Logs/log"+Session.SessionID+".txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdALERTAS.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros ", numeros + " Registros ", catalog, 1);


                RgdALERTAS.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catch

        //
    }
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {

        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdALERTAS.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdALERTAS.AllowPaging = false;
                RgdALERTAS.Rebind();
                foreach (GridDataItem row in RgdALERTAS.Items)
                {

                    int id = Parser.ToNumber(row["ID"].Text);
                    AlertasRules mr = new AlertasRules();
                    if (mr.BorrarAlerta(new Alerta(id, "", "", "", "", "", "")) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        //JAGH se agrega actividad 20/01/13
                        ActividadRules.GuardarActividad(1111, idUs, "Los datos se han eliminado de forma correcta catálogo alertas");

                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                    }

                    else
                    {
                        //JAGH se agrega actividad 20/01/13
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        ActividadRules.GuardarActividad(800, idUs, "Los datos se han eliminado de forma correcta catálogo alertas");

                        Mensajes.ShowAdvertencia(Page, this.GetType(), "No se eliminaron los datos");
                    }
                }//foreach
                RgdALERTAS.Rebind();
                RgdALERTAS.DataSource = null;
                RgdALERTAS.AllowPaging = true;
                RgdALERTAS.Rebind();
                RgdALERTAS.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdALERTAS.VirtualItemCount) + " Registros ", "0 registros ", catalog, 1, null, null);


            }//header
            else
            {
                foreach (GridDataItem row in RgdALERTAS.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {


                        int id = Parser.ToNumber(row["ID"].Text);
                        AlertasRules mr = new AlertasRules();
                        if (mr.BorrarAlerta(new Alerta(id, "", "", "", "", "", "")) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + id, null, null, catalog, 1, null, null);

                        }

                        else
                        {
                            //JAGH se agrega actividad 20/01/13
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(800, idUs, "No se eliminaron los datos catálogo alertas");

                            Mensajes.ShowAdvertencia(Page, this.GetType(), "No se eliminaron los datos");
                        }
                    }//checket

                }//FOREACH

            }
        }
        catch (Exception exep)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error no se eliminaron los datos catálogo alertas");

            Response.Write(exep.Message.ToString());
        }//try-catch
        //btn_eliminar_Click
        RgdALERTAS.Rebind();


        //btn_eliminar_Click
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;

            CheckBox chkHeader;

            chkHeader = (CheckBox)sender;


            if (chkHeader.Checked == true)
            {
                foreach (GridDataItem row in RgdALERTAS.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdALERTAS.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = false;
                }
            }




        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }//try-catch


        //ChkTodo_CheckedChanged
    }


}
