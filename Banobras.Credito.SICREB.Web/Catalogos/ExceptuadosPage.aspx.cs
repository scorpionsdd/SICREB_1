using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Entities.Util;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Common;
using System.Data.Common;
using System.Data;
using Banobras.Credito.SICREB.Data;
public partial class Catalogos_ExceptuadosPage : System.Web.UI.Page
{
    int idUs;
    public const String catalog = "Créditos Exceptuados";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {
                CambiaAtributosRGR();
                string persona = "PF";
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                   
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

        GridFilterMenu menu = RgdExceptuados.FilterMenu;
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
    protected void RgdExceptuados_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        int id = Parser.ToNumber(item["id"].Text);
        CreditosExceptuadosRules cer = new CreditosExceptuadosRules();
        if (cer.CreditosExceptuadosBorrar(new CreditoExceptuado(id, "", "", Enums.Estado.Inactivo)) > 0)
        {

            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            //
            DateTime now = DateTime.Now;
            ActividadRules.GuardarActividad(idUs, "Catálogo de Créditos Exceptuados", now.ToString("d"), now.ToString("T"), "Elimina", 51, item["CREDITO"].Text.ToString(), "N/A");
            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
        }
        else
        {
            Mensajes.ShowAdvertencia(this.Page, GetType(), "No se eliminaron los datos, favor de verificar");
        }
    }
    protected void RgdExceptuados_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdExceptuados_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        CreditoExceptuado ce;
        GridEditableItem objetoGrid = e.Item as GridEditableItem;
        Hashtable nuevosValores = new Hashtable();
        objetoGrid.ExtractValues(nuevosValores);
        if (nuevosValores["CREDITO"] != null && nuevosValores["MOTIVO"] != null)
        {
            string credito = nuevosValores["CREDITO"].ToString();

           if (bValidaCredito(credito))
            {
                if (bValidaCampoVacio(nuevosValores["MOTIVO"].ToString(), "Motivo", e))
                {

                    string motivo = nuevosValores["MOTIVO"].ToString();
                    ce = new CreditoExceptuado(0, credito, motivo, Enums.Estado.Activo);
                    CreditosExceptuadosRules cer = new CreditosExceptuadosRules();

                    if (cer.CreditosExceptuadosInsertar(ce) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        //
                        DateTime now = DateTime.Now;
                        //  ActividadRules.GuardarActividad(idUs, "Catálogo de Créditos Exceptuados", now.ToString("d"), now.ToString("T"), "Agrega", 49, credito.ToString(), "N/A");
                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han guardado de forma correcta");
                        ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, nuevosValores);

                    }
                }
                else
                {
                    Mensajes.ShowAdvertencia(Page, this.GetType(), "No se guardaron los datos, favor de verificar");
                }


            }
        }
        else
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores ingresados no válidos");
            e.Canceled = true;
        }
    }

    private bool bValidaCredito(string sCredito)
    {

        bool Valido = true;

        List<string> lsCredito = new List<string>();

        lsCredito = (List<string>)ViewState["Credito"];

        if (lsCredito.FindIndex(s => s == sCredito) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "El Crédito : " + sCredito + " ya se encuentra dado de alta.");
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

    protected void RgdExceptuados_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            CreditosExceptuadosRules cer = new CreditosExceptuadosRules();
            var s = cer.CreditosExceptuados();
            List<string> lsCredito = new List<string>();
            RgdExceptuados.DataSource = s;
            RgdExceptuados.VirtualItemCount = s.Count;

            for (int i = 0; i < s.Count; i++)
            {

                lsCredito.Add(s[i].Credito);
            }

            ViewState["Credito"] = lsCredito;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdExceptuados_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable nuevosValores = new Hashtable();
        editedItem.ExtractValues(nuevosValores);
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        bool bValida = true;

        if (nuevosValores["CREDITO"] != null && nuevosValores["MOTIVO"] != null && item.SavedOldValues["ID"] != null)
        {
            if (bValidaCampoVacio(nuevosValores["MOTIVO"].ToString(), "Motivo", e))
            {

                string credito = nuevosValores["CREDITO"].ToString();

                if (credito != oldValues["CREDITO"].ToString())
                {
                    bValida = bValidaCredito(credito);
                }


                if (bValida)
                {
                    string motivo = nuevosValores["MOTIVO"].ToString();
                    int id = Parser.ToNumber(item.SavedOldValues["ID"].ToString());
                    CreditosExceptuadosRules cer = new CreditosExceptuadosRules();
                    if (cer.CreditosExceptuadosActualizar(new CreditoExceptuado(id, credito, motivo, Enums.Estado.Activo)) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        DateTime now = DateTime.Now;
                        // ActividadRules.GuardarActividad(idUs, "Catálogo de Créditos Exceptuados", now.ToString("d"), now.ToString("T"), "Agrega", 49, credito.ToString(), "N/A");
                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han actualizado de forma correcta");
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, nuevosValores);


                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo guardar los datos");
                    }
                }
            }

        }
        else
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores no validos ingresados");
            e.Canceled = true;
        }
    }
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdExceptuados.Columns[0].Visible = false;
        RgdExceptuados.Columns[RgdExceptuados.Columns.Count - 1].Visible = false;
        RgdExceptuados.MasterTableView.HierarchyDefaultExpanded = true;
        RgdExceptuados.ExportSettings.OpenInNewWindow = false;
        RgdExceptuados.ExportSettings.ExportOnlyData = true;
        RgdExceptuados.MasterTableView.GridLines = GridLines.Both;
        RgdExceptuados.ExportSettings.IgnorePaging = true;
        RgdExceptuados.ExportSettings.OpenInNewWindow = true;
        RgdExceptuados.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);


    }
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdExceptuados.Columns[0].Visible = false;
        RgdExceptuados.Columns[RgdExceptuados.Columns.Count - 1].Visible = false;
        RgdExceptuados.MasterTableView.HierarchyDefaultExpanded = true;
        RgdExceptuados.ExportSettings.OpenInNewWindow = false;
        RgdExceptuados.ExportSettings.ExportOnlyData = true;

        RgdExceptuados.ExportSettings.IgnorePaging = true;
        RgdExceptuados.ExportSettings.OpenInNewWindow = true;
        RgdExceptuados.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);


    }
    protected void RgdExceptuados_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {

    }
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdExceptuados.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdExceptuados.MasterTableView.GetColumn("DeleteState").Visible = false;
        ImageButton1.Visible = false;
        btnExportPDF.Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_EXCEPTUADO_F")))
            {
                this.RgdExceptuados.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_EXCEPTUADO_F")))
            {
                //this.RgdExceptuados.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;

        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_EXCEPTUADO_F")))
            {
                this.RgdExceptuados.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_EXCEPTUADO_F")))
            {
                //this.RgdExceptuados.MasterTableView.GetColumn("DeleteState").Visible = true;
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
    protected void RgdExceptuados_ItemDataBound(object sender, GridItemEventArgs e)
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_EXCEPTUADO_F")))
        {
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_EXCEPTUADO_M")))
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
            GridHeaderItem headerItem = RgdExceptuados.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");

            if (chkHeader.Checked == true)
            {
                RgdExceptuados.AllowPaging = false;
                RgdExceptuados.Rebind();
                foreach (GridDataItem row in RgdExceptuados.Items)
                {
                    Exceptuado exceptudodelete;
                    string claveBuro = string.Empty;
                    Int32 claveClic = 0;
                    string Descripcion = string.Empty;


                    claveClic = Parser.ToNumber(row.GetDataKeyValue("ID").ToString());
                    claveBuro = row["Credito"].Text.ToString();
                    Descripcion = row["Motivo"].Text.ToString();

                    exceptudodelete = new Exceptuado(claveClic, claveBuro, Descripcion, Enums.Estado.Activo);

                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
                    //EstadoCivilDataAccess EstadoCivilBorrar = new EstadoCivilDataAccess();
                    Exceptuados_Rules exborrar = new Exceptuados_Rules();

                    if (exborrar.Delete(exceptudodelete) > 0)
                    {

                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los Registros se han removido correctamente");

                    }
                    else
                    {

                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }//foreach
                RgdExceptuados.Rebind();
                RgdExceptuados.DataSource = null;
                RgdExceptuados.AllowPaging = true;
                RgdExceptuados.Rebind();
                RgdExceptuados.DataBind();

                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdExceptuados.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header

            else
            {

                foreach (GridDataItem row in RgdExceptuados.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {



                        int id = Parser.ToNumber(row.GetDataKeyValue("ID").ToString());
                        CreditosExceptuadosRules cer = new CreditosExceptuadosRules();
                        if (cer.CreditosExceptuadosBorrar(new CreditoExceptuado(id, "", "", Enums.Estado.Inactivo)) > 0)
                        {

                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());



                            DateTime now = DateTime.Now;
                            //ActividadRules.GuardarActividad(idUs, "Catálogo de Créditos Exceptuados", now.ToString("d"), now.ToString("T"), "Elimina", 51, item["CREDITO"].Text.ToString(), "N/A");
                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + id, null, null, catalog, 1, null, null);

                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, GetType(), "No se eliminaron los datos, favor de verificar");
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
        RgdExceptuados.Rebind();
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
                foreach (GridDataItem row in RgdExceptuados.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdExceptuados.Items)
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
                row1["nombreColumna"] = "Crédito";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Motivo";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "100";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);

                //List<DbParameter> parametros = new List<DbParameter>();

                DbParameter[] parametros = new DbParameter[4];


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
                parametros[2].ParameterName = "credito";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 38;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "motivo";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 100;

                string storeBase = "SP_CARGAMASIVA_ECEPTUADOS";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_edo_civil", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "");
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                RgdExceptuados.Rebind();
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));

                int total = RgdExceptuados.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);


                RgdExceptuados.Rebind();

            }
        }
        catch (Exception ex)
        {


            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);

            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
            //RgdAccionistas.Rebind();
            //   Mensajes.ShowMessage(this.Page,this.GetType(),cargaMasiva.Log.ToString());
        }//try-Catch
    }
}