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
using Banobras.Credito.SICREB.Business;
public partial class Catalogos_NumeroPagosPage : System.Web.UI.Page
{
    public const String catalog = "Número de Pagos";
    int idUs;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {
                string persona = "PM";
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

        GridFilterMenu menu = RgdNumeroPagos.FilterMenu;
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
        RgdNumeroPagos.Columns[0].Visible = false;
        RgdNumeroPagos.Columns[RgdNumeroPagos.Columns.Count - 1].Visible = false;
        RgdNumeroPagos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdNumeroPagos.ExportSettings.OpenInNewWindow = false;
        RgdNumeroPagos.ExportSettings.ExportOnlyData = true;
        RgdNumeroPagos.MasterTableView.GridLines = GridLines.Both;
        RgdNumeroPagos.ExportSettings.IgnorePaging = true;
        RgdNumeroPagos.ExportSettings.OpenInNewWindow = true;
        RgdNumeroPagos.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdNumeroPagos.Columns[0].Visible = false;
        RgdNumeroPagos.Columns[RgdNumeroPagos.Columns.Count - 1].Visible = false;
        RgdNumeroPagos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdNumeroPagos.ExportSettings.OpenInNewWindow = false;
        RgdNumeroPagos.ExportSettings.ExportOnlyData = true;

        RgdNumeroPagos.ExportSettings.IgnorePaging = true;
        RgdNumeroPagos.ExportSettings.OpenInNewWindow = true;
        RgdNumeroPagos.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdNumeroPagos_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        int id = Parser.ToNumber(item["id"].Text);
        NumeroPagosRules npr = new NumeroPagosRules();
        if (npr.BorrarNumeroPagos(new Num_Pago(id, "", "", Enums.Estado.Activo)) > 0)
        {
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

            Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han eliminado de forma correcta");
        }
        else
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo eliminar los datos");
        }
    }
    protected void RgdNumeroPagos_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdNumeroPagos_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Num_Pago numpago;
        GridEditableItem objetoGrid = e.Item as GridEditableItem;
        Hashtable nuevosValores = new Hashtable();
        objetoGrid.ExtractValues(nuevosValores);
        if (nuevosValores["CLAVEBURO"] != null && nuevosValores["CLAVESIC"] != null)
        {

            if (bValidaCampoVacio(nuevosValores["CLAVEBURO"].ToString(), "Clave BURO", e) && bValidaCampoVacio(nuevosValores["CLAVESIC"].ToString(), "Clave SIC", e))
            {
                numpago = new Num_Pago(0, nuevosValores["CLAVEBURO"].ToString(), nuevosValores["CLAVESIC"].ToString(), Enums.Estado.Activo);

                //TMR. Valida que no exista previamente
                bool bExiste = false;
                bExiste = ValidaExistente(nuevosValores["CLAVEBURO"]);

                if (bExiste)
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La clave de buró  que intenta dar de alta ya existe.");
                }
                else
                {
                    NumeroPagosRules npr = new NumeroPagosRules();
                    if (npr.InsertarNumPagos(numpago) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han guardado de forma correcta");
                        ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, nuevosValores);
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo guardar los datos");
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

    /// <summary>
    /// TMR. Valida en los datos del grid que no exista
    /// </summary>
    /// <param name="p">clave buro</param>
    /// <returns></returns>
    private bool ValidaExistente(object p)
    {
        bool existe = false;
        NumeroPagosRules npr = new NumeroPagosRules();
        var listaNumeroPagos = npr.NumeroPagos();

        var dato = from item in listaNumeroPagos
                   where item.ClaveBuro.ToString().Equals(p.ToString())
                   select item;
        foreach (Num_Pago item in dato)
        {
            existe = true;
        }

        return existe;
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

    protected void RgdNumeroPagos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            NumeroPagosRules npr = new NumeroPagosRules();
            var s = npr.NumeroPagos();
            RgdNumeroPagos.DataSource = s;
            RgdNumeroPagos.VirtualItemCount = s.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdNumeroPagos_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        bool bValidar = true;

        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable nuevosValores = new Hashtable();
        editedItem.ExtractValues(nuevosValores);
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        string sClaveBuroOld = oldValues["CLAVEBURO"].ToString();
        if (nuevosValores["CLAVEBURO"] != null && nuevosValores["CLAVESIC"] != null)
        {

            if (bValidaCampoVacio(nuevosValores["CLAVEBURO"].ToString(), "Clave BURO", e) && bValidaCampoVacio(nuevosValores["CLAVESIC"].ToString(), "Clave SIC", e))
            {
                
                int id = Parser.ToNumber(item.SavedOldValues["ID"].ToString());
                string claveburo = nuevosValores["CLAVEBURO"].ToString();
                string clavesic = nuevosValores["CLAVESIC"].ToString();

                if (claveburo != sClaveBuroOld)
                {
                    bValidar = ! ValidaExistente(nuevosValores["CLAVEBURO"]);
                }

                if (bValidar)
                {

                    NumeroPagosRules npr = new NumeroPagosRules();
                    if (npr.ActualizarNumeroPagos(new Num_Pago(id, claveburo, clavesic, Enums.Estado.Activo)) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han actualizado de forma correcta");
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, nuevosValores);

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo guardar los datos");
                    }
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La clave de buró  que intenta dar de alta ya existe.");
                }
            }
        }
        else
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores no válidos ingresados");
            e.Canceled = true;
        }
    }
    protected void RgdNumeroPagos_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {

    }
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdNumeroPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        // this.RgdNumeroPagos.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_NUMPAGO")))
            {
                this.RgdNumeroPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_NUMPAGO")))
            {
                //  this.RgdNumeroPagos.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_NUMPAGO")))
            {
                this.RgdNumeroPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_NUMPAGO")))
            {
                // this.RgdNumeroPagos.MasterTableView.GetColumn("DeleteState").Visible = true;
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

    protected void RgdNumeroPagos_ItemDataBound(object sender, GridItemEventArgs e)
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_NUMPAGO")))
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
                parametros[3].ParameterName = "claveburop";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "clavesicp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 200;


                string storeBase = "SP_CARGAMASIVA_NUMPAGO";


                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_accionistas", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "PM");
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdNumeroPagos.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);


                RgdNumeroPagos.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catchtry-Catch
    }
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdNumeroPagos.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdNumeroPagos.AllowPaging = false;
                RgdNumeroPagos.Rebind();
                foreach (GridDataItem row in RgdNumeroPagos.Items)
                {
                    int id = Parser.ToNumber(row["id"].Text);
                    NumeroPagosRules npr = new NumeroPagosRules();

                    if (npr.BorrarNumeroPagos(new Num_Pago(id, "", "", Enums.Estado.Activo)) > 0)
                    {

                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo eliminar los datos");
                    }
                }//foreach
                RgdNumeroPagos.Rebind();
                RgdNumeroPagos.DataSource = null;
                RgdNumeroPagos.AllowPaging = true;
                RgdNumeroPagos.Rebind();
                RgdNumeroPagos.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdNumeroPagos.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdNumeroPagos.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {
                        int id = Parser.ToNumber(row["id"].Text);
                        NumeroPagosRules npr = new NumeroPagosRules();
                        if (npr.BorrarNumeroPagos(new Num_Pago(id, "", "", Enums.Estado.Activo)) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con id " + id, null, null, catalog, 1, null, null);
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo eliminar los datos");
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
        RgdNumeroPagos.Rebind();


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
                foreach (GridDataItem row in RgdNumeroPagos.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdNumeroPagos.Items)
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