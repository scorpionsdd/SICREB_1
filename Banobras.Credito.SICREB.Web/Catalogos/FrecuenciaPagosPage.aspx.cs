using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Common;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Data;
using System.Data;
using System.IO;
using System.Data.Common;
public partial class Catalogos_FrecuenciaPagosPage : System.Web.UI.Page
{
    public const String catalog = "Frecuencias de Pago de Interés";
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

        GridFilterMenu menu = RgdFrecuenciaPagos.FilterMenu;
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
        RgdFrecuenciaPagos.Columns[0].Visible = false;
        RgdFrecuenciaPagos.Columns[RgdFrecuenciaPagos.Columns.Count - 1].Visible = false;
        RgdFrecuenciaPagos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdFrecuenciaPagos.ExportSettings.OpenInNewWindow = false;
        RgdFrecuenciaPagos.ExportSettings.ExportOnlyData = true;
        RgdFrecuenciaPagos.MasterTableView.GridLines = GridLines.Both;
        RgdFrecuenciaPagos.ExportSettings.IgnorePaging = true;
        RgdFrecuenciaPagos.ExportSettings.OpenInNewWindow = true;
        RgdFrecuenciaPagos.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdFrecuenciaPagos.Columns[0].Visible = false;
        RgdFrecuenciaPagos.Columns[RgdFrecuenciaPagos.Columns.Count - 1].Visible = false;
        RgdFrecuenciaPagos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdFrecuenciaPagos.ExportSettings.OpenInNewWindow = false;
        RgdFrecuenciaPagos.ExportSettings.ExportOnlyData = true;

        RgdFrecuenciaPagos.ExportSettings.IgnorePaging = true;
        RgdFrecuenciaPagos.ExportSettings.OpenInNewWindow = true;
        RgdFrecuenciaPagos.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdFrecuenciaPagos_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)e.Item;
            int id = Parser.ToNumber(item["id"].Text);
            FrecuenciasPagoRules fpr = new FrecuenciasPagoRules();
            if (fpr.BorrarFrecuenciaPago(new Frecuencia_Pago(id, "", "", Enums.Estado.Activo)) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
            }

            else
            {
                Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo eliminar los datos");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
            e.Canceled = true;
        }

    }
    protected void RgdFrecuenciaPagos_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdFrecuenciaPagos_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem objetoGrid = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            objetoGrid.ExtractValues(nuevosValores);
            if (nuevosValores["CLAVEBURO"] != null && nuevosValores["CLAVESIC"] != null)
            {
                string claveburo = nuevosValores["CLAVEBURO"].ToString();
                string clavesic = nuevosValores["CLAVESIC"].ToString();

                if (bValidaCampoVacio(claveburo, "Clave Buró", e) && bValidaCampoVacio(clavesic, "Clave SIC", e))
                {

                    if (bValidaBuro(claveburo))
                    {
                                                
                        FrecuenciasPagoRules fpr = new FrecuenciasPagoRules();
                        if (fpr.InsertarFrecuenciaPago(new Frecuencia_Pago(0, claveburo, clavesic, Enums.Estado.Activo)) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowAdvertencia(Page, this.GetType(), "Los datos se han guardado de forma correcta");

                            ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, nuevosValores);

                        }

                        else
                        {
                            Mensajes.ShowMessage(Page, this.GetType(), "No se pudo guardar los datos");
                        }
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
            Mensajes.ShowError(Page, this.GetType(), ex);
            e.Canceled = true;
        }
    }

    private bool bValidaBuro(string sBuro)
    {

        bool Valido = true;

        List<string> lsTiposCredito = new List<string>();

        lsTiposCredito = (List<string>)ViewState["Buro"];
        if (lsTiposCredito.FindIndex(s => s == sBuro) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Clave Buró: " + sBuro + " ya se encuentra dada de alta.");
        }

        return Valido;
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

    protected void RgdFrecuenciaPagos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            FrecuenciasPagoRules fpr = new FrecuenciasPagoRules();

            List<string> lsBuro = new List<string>();

            var s = fpr.FrecuenciasPago();

            RgdFrecuenciaPagos.DataSource = s;
            RgdFrecuenciaPagos.VirtualItemCount = s.Count;

            for (int i = 0; i < s.Count; i++)
            {

                lsBuro.Add(s[i].ClaveBuro);
            }

            ViewState["Buro"] = lsBuro;

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
        }
    }
    protected void RgdFrecuenciaPagos_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            editedItem.ExtractValues(nuevosValores);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;
            string sClaveBuroOld = oldValues["CLAVEBURO"].ToString();
            bool bValida = true;

            if (nuevosValores["CLAVEBURO"] != null && nuevosValores["CLAVESIC"] != null && item.SavedOldValues["ID"] != null)
            {
                  
                string claveburo = nuevosValores["CLAVEBURO"].ToString();
                string clavesic = nuevosValores["CLAVESIC"].ToString();

                if (bValidaCampoVacio(claveburo, "Clave Buró", e) && bValidaCampoVacio(clavesic, "Clave SIC", e))
                {

                    if (sClaveBuroOld != claveburo)
                    {
                       
                        bValida = bValidaBuro(claveburo);
                       
                    }
                }
                else
                {
                    bValida = true;
                }

                if (bValida)
                {
                   
                    int id = Parser.ToNumber(item.SavedOldValues["ID"].ToString());
                    FrecuenciasPagoRules fpr = new FrecuenciasPagoRules();

                    if (fpr.ActualizarFrecuenciaPago(new Frecuencia_Pago(id, claveburo, clavesic, Enums.Estado.Activo)) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han actualizado de forma correcta");
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, nuevosValores);

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo guardar los datos");
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
            Mensajes.ShowError(Page, this.GetType(), ex);
            e.Canceled = true;
        }
    }
    protected void RgdFrecuenciaPagos_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {

    }
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdFrecuenciaPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdFrecuenciaPagos.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_FRECPAGO")))
            {
                this.RgdFrecuenciaPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_FRECPAGO")))
            {
                //	this.RgdFrecuenciaPagos.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_FRECPAGO")))
            {
                this.RgdFrecuenciaPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_FRECPAGO")))
            {
                //	this.RgdFrecuenciaPagos.MasterTableView.GetColumn("DeleteState").Visible = true;
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

    protected void RgdFrecuenciaPagos_ItemDataBound(object sender, GridItemEventArgs e)
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


    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_FRECPAGO")))
        {
            valido = true;
        }
        return valido;
    }
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdFrecuenciaPagos.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdFrecuenciaPagos.AllowPaging = false;
                RgdFrecuenciaPagos.Rebind();
                foreach (GridDataItem row in RgdFrecuenciaPagos.Items)
                {


                    int id = Parser.ToNumber(row["ID"].Text.ToString());
                    FrecuenciasPagoRules fpr = new FrecuenciasPagoRules();
                    if (fpr.BorrarFrecuenciaPago(new Frecuencia_Pago(id, "", "", Enums.Estado.Activo)) > 0)
                    {


                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                    }

                    else
                    {
                        Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo eliminar los datos");
                    }
                }//foreach
                RgdFrecuenciaPagos.Rebind();
                RgdFrecuenciaPagos.DataSource = null;
                RgdFrecuenciaPagos.AllowPaging = true;
                RgdFrecuenciaPagos.Rebind();
                RgdFrecuenciaPagos.DataBind();
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdFrecuenciaPagos.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdFrecuenciaPagos.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {


                        int id = Parser.ToNumber(row["ID"].Text.ToString());
                        FrecuenciasPagoRules fpr = new FrecuenciasPagoRules();
                        if (fpr.BorrarFrecuenciaPago(new Frecuencia_Pago(id, "", "", Enums.Estado.Activo)) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con id " + id, null, null, catalog, 1, null, null);

                        }

                        else
                        {
                            Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo eliminar los datos");
                        }



                    }//checket

                }//FOREACH

            }
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch
        //btn_eliminar_Click
        RgdFrecuenciaPagos.Rebind();

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
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Clave SIC";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "38";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);




                DbParameter[] parametros = new DbParameter[5];


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
                parametros[3].ParameterName = "clavesicp";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "claveburop";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 200;



                string storeBase = "SP_CARGAMASIVA_FRECPAGO";


                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_accionistas", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "PM");
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdFrecuenciaPagos.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdFrecuenciaPagos.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catch
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
                foreach (GridDataItem row in RgdFrecuenciaPagos.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdFrecuenciaPagos.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = false;
                }
            }




        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch


        //ChkTodo_CheckedChanged
    }

}