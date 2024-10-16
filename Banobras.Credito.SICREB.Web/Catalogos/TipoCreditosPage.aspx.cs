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
using Banobras.Credito.SICREB.Data;
using System.Data.Common;

public partial class TipoCreditosPage : System.Web.UI.Page
{
    public const String catalog = "Tipo de Crédito";
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

        GridFilterMenu menu = RgdTipoCreditos.FilterMenu;
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
    protected void RgdTipoCreditos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        TipoCreditos_Rules getRecords = null;
        List<TipoCredito> tipoCreditosInfo;
        List<string> lsID = new List<string>();
        try
        {
            getRecords = new TipoCreditos_Rules();
            tipoCreditosInfo = getRecords.GetRecords(false);
            RgdTipoCreditos.DataSource = tipoCreditosInfo;
            RgdTipoCreditos.VirtualItemCount = tipoCreditosInfo.Count;

            for (int i = 0; i < tipoCreditosInfo.Count; i++)
            {

                lsID.Add(tipoCreditosInfo[i].tipoCredito);
            }


            ViewState["TipoCreditos"] = lsID;
         
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdTipoCreditos_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

        GridDataItem item = (GridDataItem)e.Item;

    }
    protected void RgdTipoCreditos_UpdateCommand(object source, GridCommandEventArgs e)
    {

        string tipoCredito = string.Empty;
        string descripcion = string.Empty;
        string nombreGenerico = string.Empty;
        Enums.Estado estado;
        Int32 idCredito = 0;
        TipoCredito creditoOld;
        TipoCredito creditoNew;
        bool bValidar = true;

        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);
       
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        string TipoCreditoOld = oldValues["tipoCredito"].ToString();

        try
        {
            //Datos Orginales
            idCredito = Convert.ToInt32(item.SavedOldValues["Id"]);

            if (newValues["tipoCredito"] != null && newValues["Descripcion"] != null && newValues["NombreGenerico"] != null)
            {

                if (bValidaCampoVacio(newValues["tipoCredito"].ToString(), "Tipo Crédito", e) && bValidaCampoVacio(newValues["NombreGenerico"].ToString(), "Nombre Genérico", e) && bValidaCampoVacio(newValues["Descripcion"].ToString(), "Descripción", e))
                {

                    //Datos Nuevos
                    tipoCredito = newValues["tipoCredito"].ToString();

                    if (tipoCredito != TipoCreditoOld)
                    {
                        bValidar = bValidaTipoCredito(tipoCredito);
                    }

                    if (bValidar)
                    {

                        descripcion = newValues["Descripcion"].ToString();
                        nombreGenerico = newValues["NombreGenerico"].ToString();
                        estado = Enums.Estado.Activo;

                        creditoOld = new TipoCredito(idCredito, tipoCredito, descripcion, nombreGenerico, estado);
                        creditoNew = new TipoCredito(idCredito, tipoCredito, descripcion, nombreGenerico, estado);


                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        TipoCreditos_Rules TipoCreditosValores = new TipoCreditos_Rules();

                        if (TipoCreditosValores.Update(creditoOld, creditoNew) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                            ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);

                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                            e.Canceled = true;

                        }
                    }
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
                e.Canceled = true;
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
    }

    protected void RgdTipoCreditos_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdTipoCreditos_DeleteCommand(object source, GridCommandEventArgs e)
    {
        TipoCredito tipoCreditoDelete;
        string tipoCredito = string.Empty;
        Int32 idCredito = 0;
        GridDataItem item = (GridDataItem)e.Item;

        try
        {
            //Datos para identificar valor
            idCredito = Parser.ToNumber(item["Id"].Text);
            tipoCredito = item["tipoCredito"].Text;

            tipoCreditoDelete = new TipoCredito(idCredito, tipoCredito, string.Empty, string.Empty, Enums.Estado.Activo);
            //Llamar el SP correspondiente con las entidades old y new de Validacion
            TipoCreditos_Rules TipoCredBorrar = new TipoCreditos_Rules();

            if (TipoCredBorrar.Delete(tipoCreditoDelete) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro", null, null, catalog, 1, null, null);
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdTipoCreditos_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertTipoCredito(e);
    }
    private void InsertTipoCredito(GridCommandEventArgs e)
    {

        TipoCreditos_Rules TipoCeditoInsertar;
        try
        {          

            TipoCredito record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion
                if (record.Id != 0 || record.tipoCredito != "")
                {

                    if (bValidaCampoVacio(record.tipoCredito, "Tipo Crédito", e) && bValidaCampoVacio(record.NombreGenerico, "Nombre Genérico", e) && bValidaCampoVacio(record.Descripcion, "Descripción", e))
                    {

                        if (bValidaTipoCredito(record.tipoCredito))
                        {

                            TipoCeditoInsertar = new TipoCreditos_Rules();

                            if (TipoCeditoInsertar.Insert(record) > 0)
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());


                                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                            }
                            else
                            {
                                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    private TipoCredito ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        TipoCredito record;
        RadComboBox estatusCombo;
        string estatus = string.Empty;
        Enums.Estado estado;
        estatusCombo = (RadComboBox)editedItem["EstatusTemp"].FindControl("ComboEstatus");
        estatus = estatusCombo.Text;

        // Extrae todos los elementos
        estado = Enums.Estado.Activo;
        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {

            if (newValues["tipoCredito"] != null && newValues["Descripcion"] != null && newValues["NombreGenerico"] != null)
            {

                record = new TipoCredito(0, newValues["tipoCredito"].ToString(), newValues["Descripcion"].ToString(), newValues["NombreGenerico"].ToString(), estado);
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, newValues);

            }
            else
            {
                record = new TipoCredito(0, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            record = new TipoCredito(0, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo);
        }
        return record;
    }

    private bool bValidaTipoCredito(string sTipoCredito)
    {

        bool Valido = true;

        List<string> lsTiposCredito = new  List<string>();
        
        lsTiposCredito = (List<string>) ViewState["TipoCreditos"];

        if (lsTiposCredito.FindIndex(s => s == sTipoCredito) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "El Tipo de Crédito: " + sTipoCredito + " ya se encuentra dado de alta.");
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


    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdTipoCreditos.Columns[0].Visible = false;
        RgdTipoCreditos.Columns[RgdTipoCreditos.Columns.Count - 1].Visible = false;
        RgdTipoCreditos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdTipoCreditos.ExportSettings.OpenInNewWindow = false;
        RgdTipoCreditos.ExportSettings.ExportOnlyData = true;
        RgdTipoCreditos.MasterTableView.GridLines = GridLines.Both;
        RgdTipoCreditos.ExportSettings.IgnorePaging = true;
        RgdTipoCreditos.ExportSettings.OpenInNewWindow = true;
        RgdTipoCreditos.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdTipoCreditos.Columns[0].Visible = false;
        RgdTipoCreditos.Columns[RgdTipoCreditos.Columns.Count - 1].Visible = false;
        RgdTipoCreditos.MasterTableView.HierarchyDefaultExpanded = true;
        RgdTipoCreditos.ExportSettings.OpenInNewWindow = false;
        RgdTipoCreditos.ExportSettings.ExportOnlyData = true;

        RgdTipoCreditos.ExportSettings.IgnorePaging = true;
        RgdTipoCreditos.ExportSettings.OpenInNewWindow = true;
        RgdTipoCreditos.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdTipoCreditos_ItemDataBound(object sender, GridItemEventArgs e)
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
    //protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    //{

    //}
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdTipoCreditos.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdTipoCreditos.MasterTableView.GetColumn("DeleteState").Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TIPOCRED")))
            {
                this.RgdTipoCreditos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPOCRED")))
            {
                //	this.RgdTipoCreditos.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TIPOCRED")))
            {
                this.RgdTipoCreditos.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPOCRED")))
            {
                //this.RgdTipoCreditos.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
        }
        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }
    }
    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_TIPOCRED")))
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
                row1["nombreColumna"] = "Tipo de crédito";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Descripción";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "200";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Nombre Genérico";
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
                parametros[3].ParameterName = "tipocreditop";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 5;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "descripcionp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 200;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "nomgenericop";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 30;

                string storeBase = "SP_cargaMasiva_tipoCred";


                dt_layout_procesado = cargaMasiva.cargaMasiva("Pais", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));

                int total = RgdTipoCreditos.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdTipoCreditos.Rebind();
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
            Int32 idCredito = 0;
            string tipoCredito = string.Empty;
            string descripcion = string.Empty;
            string nombreGenerico = string.Empty;
            GridHeaderItem headerItem = RgdTipoCreditos.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdTipoCreditos.AllowPaging = false;
                RgdTipoCreditos.Rebind();
                foreach (GridDataItem row in RgdTipoCreditos.Items)
                {
                    TipoCredito tipoCreditoDelete;
                    idCredito = Parser.ToNumber(row["Id"].Text);
                    tipoCredito = row["tipoCredito"].Text;

                    tipoCreditoDelete = new TipoCredito(idCredito, tipoCredito, string.Empty, string.Empty, Enums.Estado.Activo);
                    //Llamar el SP correspondiente con las entidades old y new de Validacion
                    TipoCreditos_Rules TipoCredBorrar = new TipoCreditos_Rules();

                    if (TipoCredBorrar.Delete(tipoCreditoDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }

                }//foreach
                RgdTipoCreditos.Rebind();
                RgdTipoCreditos.DataSource = null;
                RgdTipoCreditos.AllowPaging = true;
                RgdTipoCreditos.Rebind();
                RgdTipoCreditos.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdTipoCreditos.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdTipoCreditos.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {
                        TipoCredito tipoCreditoDelete;
                        idCredito = Parser.ToNumber(row["Id"].Text);
                        tipoCredito = row["tipoCredito"].Text;

                        tipoCreditoDelete = new TipoCredito(idCredito, tipoCredito, string.Empty, string.Empty, Enums.Estado.Activo);
                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        TipoCreditos_Rules TipoCredBorrar = new TipoCreditos_Rules();

                        if (TipoCredBorrar.Delete(tipoCreditoDelete) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con id " + idCredito, null, null, catalog, 1, null, null);
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
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
        RgdTipoCreditos.Rebind();


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
                foreach (GridDataItem row in RgdTipoCreditos.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdTipoCreditos.Items)
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