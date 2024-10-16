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
using Banobras.Credito.SICREB.Data;
using System.Data;
using System.IO;
using System.Data.Common;
public partial class Catalogos_MonedasPage : System.Web.UI.Page
{
    public const String catalog = "Tipo de Moneda";
    int idUs;
    string persona;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {
                persona = "PM";
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                    CambiaAtributosRGR();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó al Catálogo " + catalog);
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

        GridFilterMenu menu = RgdMONEDAS.FilterMenu;
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
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdMONEDAS.Columns[0].Visible = false;
        RgdMONEDAS.Columns[RgdMONEDAS.Columns.Count - 1].Visible = false;
        RgdMONEDAS.MasterTableView.HierarchyDefaultExpanded = true;
        RgdMONEDAS.ExportSettings.OpenInNewWindow = false;
        RgdMONEDAS.ExportSettings.ExportOnlyData = true;
        RgdMONEDAS.MasterTableView.GridLines = GridLines.Both;
        RgdMONEDAS.ExportSettings.IgnorePaging = true;
        RgdMONEDAS.ExportSettings.OpenInNewWindow = true;
        RgdMONEDAS.ExportSettings.Pdf.PageHeight = Unit.Parse("300mm");
        RgdMONEDAS.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdMONEDAS.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdMONEDAS.Columns[0].Visible = false;
        RgdMONEDAS.Columns[RgdMONEDAS.Columns.Count - 1].Visible = false;
        RgdMONEDAS.MasterTableView.HierarchyDefaultExpanded = true;
        RgdMONEDAS.ExportSettings.OpenInNewWindow = false;
        RgdMONEDAS.ExportSettings.ExportOnlyData = true;

        RgdMONEDAS.ExportSettings.IgnorePaging = true;
        RgdMONEDAS.ExportSettings.OpenInNewWindow = true;
        RgdMONEDAS.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdMONEDAS_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;
            int id = Parser.ToNumber(item["id"].Text);
            MonedasRules mr = new MonedasRules();
            if (mr.BorrarMoneda(new Moneda(id, 0, 0, "", Enums.Estado.Inactivo)) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
            }

            else
            {
                Mensajes.ShowAdvertencia(Page, this.GetType(), "No se eliminaron los datos");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
        }
    }
    protected void RgdMONEDAS_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdMONEDAS_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem objetoGrid = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            objetoGrid.ExtractValues(nuevosValores);           

            if (bValidaCampos(nuevosValores, e))
            {
                              
                    int claveburo = Parser.ToNumber(nuevosValores["CLAVEBURO"].ToString());
                    int claveclic = Parser.ToNumber(nuevosValores["CLAVECLIC"].ToString());
                    string descripcion = nuevosValores["DESCRIPCION"].ToString();

                    MonedasRules mr = new MonedasRules();
                    List<Moneda> mon = mr.Monedas();

                    if (mon.FindIndex(delegate(Moneda mo) { return mo.ClaveBuro == claveburo; }) < 0)
                    {
                        Moneda m = new Moneda(0, claveburo, claveclic, descripcion, Enums.Estado.Activo);
                        if (mr.InsertarMoneda(m) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han guardado de forma correcta");
                            ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, nuevosValores);
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo guardar los datos");
                        }
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(Page, this.GetType(), "La Clave Buro: " + claveburo + " ya se encuentra dada de alta.");
                        e.Canceled = true;
                    }

                }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
        }
    }
    protected void RgdMONEDAS_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            MonedasRules mr = new MonedasRules();
            var s = mr.Monedas();
            RgdMONEDAS.DataSource = s;
            RgdMONEDAS.VirtualItemCount = s.Count;
        }

        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
        }
    }
    protected void RgdMONEDAS_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            editedItem.ExtractValues(nuevosValores);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            if (bValidaCampos(nuevosValores,e))
            {
                int id = Parser.ToNumber(nuevosValores["ID"].ToString());
                int claveburo = Parser.ToNumber(nuevosValores["CLAVEBURO"].ToString());
                int claveclic = Parser.ToNumber(nuevosValores["CLAVECLIC"].ToString());
                string descripcion = nuevosValores["DESCRIPCION"].ToString();
                MonedasRules mr = new MonedasRules();
                List<Moneda> mon = mr.Monedas();
                if (mon.FindIndex(delegate(Moneda mo) { return (mo.ClaveBuro == claveburo && mo.Id != id); }) < 0)
                {
                    Moneda m = new Moneda(id, claveburo, claveclic, descripcion, Enums.Estado.Activo);
                    if (mr.ActulizarMoneda(m) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han actualizado de forma correcta");
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, nuevosValores);

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(Page, this.GetType(), "No se guardaron los datos");
                    }
                }
                else
                    Mensajes.ShowAdvertencia(Page, this.GetType(), "La Clave Buró: " + claveburo + " ya se encuentra dada de alta.");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
        }
    }


    public bool bValidaCampos(Hashtable Valores, Telerik.Web.UI.GridCommandEventArgs e)
    {
        bool bValidacion = true;
        string sCadena = string.Empty;     

        if (Valores["CLAVEBURO"] == null)
        {
            sCadena = sCadena + "El campo Clave Buró debe tener datos <br>";
        }

        if (Valores["CLAVECLIC"] == null)
        {
            sCadena = sCadena + "El campo Clave Clic debe tener datos <br>";
        }

        if (Valores["DESCRIPCION"] == null)
        {
      
            sCadena = sCadena + "El campo Descripción debe tener datos <br>";
        }

        if (sCadena.Length > 0)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), sCadena);
            e.Canceled = true;
            bValidacion = false;
        }

        return bValidacion;
    }

    protected void RgdMONEDAS_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {

    }
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdMONEDAS.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdMONEDAS.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TIPOMONEDA")))
            {
                this.RgdMONEDAS.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPOMONEDA")))
            {
                //this.RgdMONEDAS.MasterTableView.GetColumn("DeleteState").Visible = true;
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
                this.RgdMONEDAS.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPOMONEDA")))
            {
                //this.RgdMONEDAS.MasterTableView.GetColumn("DeleteState").Visible = true;
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

    protected void RgdMONEDAS_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
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
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);

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
                row1["nombreColumna"] = "Clave Buró";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "5";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Clave SIC";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "10";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Descripción";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "30";



                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);


                //List<DbParameter> parametros = new List<DbParameter>();

                DbParameter[] parametros = new DbParameter[6];


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
                parametros[3].ParameterName = "claveburop";
                parametros[3].DbType = DbType.Decimal;
                parametros[3].Size = 5;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "claveclicp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 5;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "descripcionp";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 30;

                string storeBase = "SP_cargaMasiva_moneda";


                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_moneda", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;

                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));

                int total = RgdMONEDAS.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdMONEDAS.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catch
    }
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdMONEDAS.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdMONEDAS.AllowPaging = false;
                RgdMONEDAS.Rebind();
                foreach (GridDataItem row in RgdMONEDAS.Items)
                {
                    int id = Parser.ToNumber(row["ID"].Text.ToString());
                    MonedasRules mr = new MonedasRules();
                    if (mr.BorrarMoneda(new Moneda(id, 0, 0, "", Enums.Estado.Inactivo)) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                    }

                    else
                    {
                        Mensajes.ShowAdvertencia(Page, this.GetType(), "No se eliminaron los datos");
                    }
                }//foreach
                RgdMONEDAS.Rebind();
                RgdMONEDAS.DataSource = null;
                RgdMONEDAS.AllowPaging = true;
                RgdMONEDAS.Rebind();
                RgdMONEDAS.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdMONEDAS.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdMONEDAS.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {


                        int id = Parser.ToNumber(row["ID"].Text.ToString());
                        MonedasRules mr = new MonedasRules();
                        if (mr.BorrarMoneda(new Moneda(id, 0, 0, "", Enums.Estado.Inactivo)) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con id " + id, null, null, catalog, 1, null, null);

                        }

                        else
                        {
                            Mensajes.ShowAdvertencia(Page, this.GetType(), "No se eliminaron los datos");
                        }





                    }//checket

                }//FOREACH

            }
        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }//try-catch
        //btn_eliminar_Click
        RgdMONEDAS.Rebind();
        //btn_eliminar_Click
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;

            CheckBox chkHeader;

            chkHeader = (CheckBox)sender;


            if (chkHeader.Checked == true)
            {
                foreach (GridDataItem row in RgdMONEDAS.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdMONEDAS.Items)
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
