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
using System.Data;
using System.IO;
using System.Data.Common;
using Banobras.Credito.SICREB.Data;

public partial class FormasPagosPage : System.Web.UI.Page
{
    public const String catalog = "Formas De Pago";
    int idUs;
    string persona;
    protected void Page_Load(object sender, EventArgs e)
    {
        CambiaAtributosRGR();

        try
        {
            if (Session["Facultades"] != null)
            {
                persona = "PF";
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                    //CambiaAtributosRGR();
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
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    //JAGH se modifica para mostrar titulos filtros
    public void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdFormaPagos.FilterMenu;
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
        
    protected void RgdFormaPagos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        FormaPagos_Rules getRecords = null;
        List<FormaPagos> FormaPagoInfo;
        List<string> lsClave = new List<string>();
        try
        {
            getRecords = new FormaPagos_Rules();
            FormaPagoInfo = getRecords.GetRecords(false);
            RgdFormaPagos.DataSource = FormaPagoInfo;
            RgdFormaPagos.VirtualItemCount = FormaPagoInfo.Count;

            for (int i = 0; i < FormaPagoInfo.Count; i++)
            {

                lsClave.Add(FormaPagoInfo[i].Clave);
            }

            ViewState["Clave"] = lsClave;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdFormaPagos_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdFormaPagos_UpdateCommand(object source, GridCommandEventArgs e)
    {

        int IdValue = 0;
        string Comentarios = string.Empty;
        string Descripcion = string.Empty;
        string clave = string.Empty;
        //ArrayList estatus;
        Enums.Estado estado;
        FormaPagos FormaPagosOld;
        FormaPagos FormaPagosNew;
        bool Valida = true;

        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        string ClaveOld = oldValues["Clave"].ToString();
        try
        {

            if (newValues["Descripcion"] != null && newValues["Comentarios"] != null && newValues["Clave"] != null)
            {
                if (bValidaCampoVacio(newValues["Descripcion"].ToString(), "Descripción", e) && bValidaCampoVacio(newValues["Comentarios"].ToString(), "Comentarios", e) && bValidaCampoVacio(newValues["Clave"].ToString(), "Clave", e))
                {
                    //Datos Orginales
                    IdValue = Convert.ToInt32(item.SavedOldValues["Id"]);
                    //Datos para actualizar
                    Comentarios = newValues["Comentarios"].ToString();
                    Descripcion = newValues["Descripcion"].ToString();
                    clave = newValues["Clave"].ToString();

                    if (clave != ClaveOld)
                    {
                        Valida = bValidaClave(clave);
                    }
                }
                else
                {
                    Valida = false;
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
                e.Canceled = true;
                Valida = false;
            }

            if (Valida)
            {
                //estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));
                //if (estatus[0].ToString() == "Activo" || estatus[0].ToString() == "activo") { estado = Enums.Estado.Activo; } else if (estatus[0].ToString() == "Inactivo" || estatus[0].ToString() == "inactivo") { estado = Enums.Estado.Inactivo; } else { estado = Enums.Estado.Test; }
                estado = Enums.Estado.Activo;
                FormaPagosOld = new FormaPagos(IdValue, Descripcion, Comentarios, estado, clave);
                FormaPagosNew = new FormaPagos(IdValue, Descripcion, Comentarios, estado, clave);


                //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
                FormaPagos_Rules FormaPagoUpdate = new FormaPagos_Rules();

                if (FormaPagoUpdate.Update(FormaPagosOld, FormaPagosNew) > 0)
                {
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    //
                    DateTime now = DateTime.Now;
                    // ActividadRules.GuardarActividad(idUs, "Catálogo de Forma de pago", now.ToString("d"), now.ToString("T"), "Actualiza", 38, FormaPagosNew.Clave.ToString(), FormaPagosOld.Descripcion.ToString());
                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                    ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro ", null, null, catalog, 1, oldValues, newValues);
                }
                else
                {
                    //JAGH se agrega actividad 20/01/13
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(800, idUs, "El registro no fue modificado catálogo formas pago (MOP)");
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                }
            }
        }
        catch (Exception exc)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue modificado catálogo formas pago (MOP)");
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdFormaPagos_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdFormaPagos_DeleteCommand(object source, GridCommandEventArgs e)
    {

        FormaPagos FormaPagoDelete;
        int IdFormaPago = 0;
        GridDataItem item = (GridDataItem)e.Item;
        try
        {
            IdFormaPago = Convert.ToInt32(item["Id"].Text);
            FormaPagoDelete = new FormaPagos(IdFormaPago, "Descripcion", "Comentario", Enums.Estado.Activo, "Clave");
            //Aqui se llama el SP correspondiente para eliminar
            //FormasPagoDataAccess FormaPagoBorrar = new FormasPagoDataAccess();
            FormaPagos_Rules FormaPagoBorrar = new FormaPagos_Rules();

            if (FormaPagoBorrar.Delete(FormaPagoDelete) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro ", null, null, catalog, 1, null, null);
            }
            else
            {
                //JAGH se agrega actividad 20/01/13
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(800, idUs, "El registro no fue eliminado catálogo formas pago (MOP)");
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
            }
        }
        catch (Exception exc)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error: El registro no fue eliminado catálogo formas pago (MOP)");
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }

    }
    protected void RgdFormaPagos_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertFormaPago(e);
    }

    private void InsertFormaPago(GridCommandEventArgs e)
    {
        //FormasPagoDataAccess FormaPagoInsertar;
        FormaPagos_Rules FormaPagoInsertar;
        try
        {
            FormaPagos record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {
                if (record.Id != 0 || record.Descripcion != "")
                {

                    if (bValidaCampoVacio(record.Descripcion, "Descripción", e) && bValidaCampoVacio(record.Comentarios, "Comentarios", e) && bValidaCampoVacio(record.Clave, "Clave", e))
                    {
                        if (bValidaClave(record.Clave))
                        {
                            FormaPagoInsertar = new FormaPagos_Rules();
                            if (FormaPagoInsertar.Insert(record) > 0)
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                                //
                                DateTime now = DateTime.Now;
                                ActividadRules.GuardarActividad(idUs, "Catálogo de Forma de pago ", now.ToString("d"), now.ToString("T"), " Agrega ", 37, record.Descripcion.ToString(), "N/A");
                                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                            }
                            else
                            {
                                //JAGH se agrega actividad 20/01/13
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                                ActividadRules.GuardarActividad(800, idUs, "El registro no fue guardado catálogo formas pago (MOP)");
                                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue guardado catálogo formas pago (MOP)");
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    private FormaPagos ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        FormaPagos record;
        //ArrayList estatus;
        // Extrae todos los elementos

        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {
            GridDataItem item = (GridDataItem)editedItem;
            if (newValues["Descripcion"] != null && newValues["Comentarios"] != null && newValues["Clave"]!= null)
            {
                Enums.Estado estado = Enums.Estado.Activo;
                //estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));
                //if (estatus[0].ToString() == "Activo" || estatus[0].ToString() == "activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }
                record = new FormaPagos(0, newValues["Descripcion"].ToString(), newValues["Comentarios"].ToString(), estado, newValues["Clave"].ToString());
                idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH se agrega linea 22/01/2013
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro ", null, null, catalog, 1, null, newValues);

            }
            else
            {
                record = new FormaPagos(0, "", "", Enums.Estado.Activo, "");
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            record = new FormaPagos(0, "", "", Enums.Estado.Activo, "");
        }
        //estatus = null;
        return record;
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

    private bool bValidaClave(string sClave)
    {

        bool Valido = true;

        List<string> lsTiposCredito = new List<string>();

        lsTiposCredito = (List<string>)ViewState["Clave"];

        if (lsTiposCredito.FindIndex(s => s == sClave) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Clave: " + sClave + " ya se encuentra dada de alta.");
        }

        return Valido;
    }

    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdFormaPagos.Columns[0].Visible = false;
        RgdFormaPagos.Columns[RgdFormaPagos.Columns.Count - 1].Visible = false;
        RgdFormaPagos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdFormaPagos.ExportSettings.OpenInNewWindow = false;
        RgdFormaPagos.ExportSettings.ExportOnlyData = true;
        RgdFormaPagos.MasterTableView.GridLines = GridLines.Both;
        RgdFormaPagos.ExportSettings.IgnorePaging = true;
        RgdFormaPagos.ExportSettings.OpenInNewWindow = true;
        RgdFormaPagos.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el Catálogo " + catalog);

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdFormaPagos.Columns[0].Visible = false;
        RgdFormaPagos.Columns[RgdFormaPagos.Columns.Count - 1].Visible = false;
        RgdFormaPagos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdFormaPagos.ExportSettings.OpenInNewWindow = false;
        RgdFormaPagos.ExportSettings.ExportOnlyData = true;

        RgdFormaPagos.ExportSettings.IgnorePaging = true;
        RgdFormaPagos.ExportSettings.OpenInNewWindow = true;
        RgdFormaPagos.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el Catálogo " + catalog);

    }
    protected void RgdFormaPagos_ItemDataBound(object sender, GridItemEventArgs e)
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
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdFormaPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdFormaPagos.MasterTableView.GetColumn("DeleteState").Visible = false;
        ImageButton1.Visible = false;
        btnExportPDF.Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_FORMPAGO_F")))
            {
                this.RgdFormaPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_FORMPAGO_F")))
            {
                //this.RgdFormaPagos.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_FORMPAGO_F")))
            {
                this.RgdFormaPagos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_FORMPAGO_F")))
            {
                //this.RgdFormaPagos.MasterTableView.GetColumn("DeleteState").Visible = true;
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
    private bool facultadInsertar()
    {
        bool valido = false;
        try
        {

            UsuarioRules facultad = new UsuarioRules();
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_FORMPAGO_F")))
            {
                valido = true;
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
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

                //JAGH se modifica orden de renglones para coincidir con layout (excel) 20/02/13
                DataRow row1;
                row1 = dt_metaDataLayout.NewRow();
                row1["nombreColumna"] = "Clave";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "30";

                DataRow row2;
                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Descripción";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "200";

                DataRow row3;
                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Comentarios";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "200";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);              


                //List<DbParameter> parametros = new List<DbParameter>();

                //JAGH se modifica orden de renglones para coincidir con layout (STORE) 20/02/13
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
                parametros[2].ParameterName = "personap";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "clavep";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "descripcionp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 200;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "comentariosp";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 200;                

                string storeBase = "SP_cargaMasiva_ForPago";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_formasPago", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdFormaPagos.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros ", numeros + " Registros", catalog, 1);

                RgdFormaPagos.Rebind();
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
            GridHeaderItem headerItem = RgdFormaPagos.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdFormaPagos.AllowPaging = false;
                RgdFormaPagos.Rebind();
                foreach (GridDataItem row in RgdFormaPagos.Items)
                {

                    //Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
                    int id = Parser.ToNumber(row["Id"].Text.ToString());
                    FormaPagos_Rules pr = new FormaPagos_Rules();

                    if (pr.Delete(new FormaPagos(id, "", "", Enums.Estado.Activo, "")) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        //JAGH se agrega actividad 20/01/13
                        ActividadRules.GuardarActividad(7777, idUs, "Los datos se han eliminado de forma correcta catálogo formas pago (MOP)");

                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                    }
                    else
                    {
                        //JAGH se agrega actividad 20/01/13
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        ActividadRules.GuardarActividad(800, idUs, "Los datos no se han eliminado catálogo formas pago (MOP)");

                        Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo eliminar los datos");
                    }

                }//foreach
                RgdFormaPagos.Rebind();
                RgdFormaPagos.DataSource = null;
                RgdFormaPagos.AllowPaging = true;
                RgdFormaPagos.Rebind();
                RgdFormaPagos.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdFormaPagos.VirtualItemCount) + " Registros", " 0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdFormaPagos.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {

                        //Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
                        int id = Parser.ToNumber(row["Id"].Text.ToString());
                        FormaPagos_Rules pr = new FormaPagos_Rules();

                        if (pr.Delete(new FormaPagos(id, "", "", Enums.Estado.Activo, "")) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + id, null, null, catalog + " " + persona, 1, null, null);

                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");


                        }
                        else
                        {
                            //JAGH se agrega actividad 20/01/13
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(800, idUs, "Los datos no se han eliminado catálogo formas pago (MOP)");
                            Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo eliminar los datos");
                        }

                    }//checket

                }//FOREACH


            }
        }
        catch (Exception exep)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error los datos no se han eliminado catálogo formas pago (MOP)");

            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch
        //btn_eliminar_Click
        RgdFormaPagos.Rebind();
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
                foreach (GridDataItem row in RgdFormaPagos.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdFormaPagos.Items)
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

    // JAGH conservar nombres de filtros en español al filtrar dato
    protected void grids_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //sobre filtro y paginacion
        if (e.CommandName == RadGrid.FilterCommandName || e.CommandName == RadGrid.PageCommandName || e.CommandName.Equals("ChangePageSize") ||
             e.CommandName == RadGrid.PrevPageCommandArgument || e.CommandName == RadGrid.NextPageCommandArgument ||
             e.CommandName == RadGrid.FirstPageCommandArgument || e.CommandName == RadGrid.LastPageCommandArgument)
        {
            CambiaAtributosRGR(); 
        }
    }

}