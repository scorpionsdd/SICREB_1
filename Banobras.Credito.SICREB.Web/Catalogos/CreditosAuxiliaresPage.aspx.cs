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
using Banobras.Credito.SICREB.Data;
using System.IO;
using System.Data.Common;

public partial class CreditosAuxiliaresPage : System.Web.UI.Page
{
    public const String catalog = "Créditos con Auxiliar Diferente en SIC y SICOFIN";
    string persona = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Facultades"] != null)
        {
            persona = "PM";
            getFacultades();
            if (!this.Page.IsPostBack)
            {
                CambiaAtributosRGR();
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingreso a Catálogo " + catalog);
            }
        }
        else
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }
    }
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdAuxiliares.FilterMenu;
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
    protected void RgdAuxiliares_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        AuxiliaresRules getRecords = null;
        List<Auxiliares> AuxiliaresInfo;
        List<string> lsCredito = new List<string>();
        try
        {
            getRecords = new AuxiliaresRules();
            AuxiliaresInfo = getRecords.GetRecords(false);
            RgdAuxiliares.DataSource = AuxiliaresInfo;
            RgdAuxiliares.VirtualItemCount = AuxiliaresInfo.Count;

            for (int i = 0; i < AuxiliaresInfo.Count; i++)
            {

                lsCredito.Add(AuxiliaresInfo[i].Credito);
            }

            ViewState["Credito"] = lsCredito;


        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdAuxiliares_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

        GridDataItem item = (GridDataItem)e.Item;

    }
    protected void RgdAuxiliares_UpdateCommand(object source, GridCommandEventArgs e)
    {
        Int32 idValor = 0;
        string credito = string.Empty;
        string auxiliar = string.Empty;
        string sCreditoOld;
        bool bValidar = true;

        ArrayList estatus;
        Enums.Estado estado;
        Auxiliares AuxiliarOld;
        Auxiliares AuxiliarNew;


        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);
       
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        sCreditoOld = oldValues["Credito"].ToString();

        try
        {

            if (newValues["Credito"] != null && newValues["Auxiliar"] != null)
            {
                if (bValidaCampoVacio(newValues["Credito"].ToString(), "Crédito", e) && bValidaCampoVacio(newValues["Auxiliar"].ToString(), "Auxiliar", e))
                {

                    credito = newValues["Credito"].ToString();

                    if (sCreditoOld != credito)
                    {
                        bValidar = bValidaCredito(credito);
                    }

                        

                    if (bValidar)
                    {
                        //Datos Orginales
                        idValor = Convert.ToInt32(item.SavedOldValues["Id"]);
                        //Datos Nuevos
                        
                        auxiliar = newValues["Auxiliar"].ToString();
                        estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));

                        if (estatus[1].ToString() == "1") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }

                        AuxiliarOld = new Auxiliares(idValor, credito, auxiliar, estado);
                        AuxiliarNew = new Auxiliares(idValor, credito, auxiliar, estado);


                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        AuxiliaresRules AuxiliarActualiza = new AuxiliaresRules();

                        if (AuxiliarActualiza.Update(AuxiliarOld, AuxiliarNew) > 0)
                        {
                            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                            ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);

                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
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

    protected void RgdAuxiliares_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdAuxiliares_DeleteCommand(object source, GridCommandEventArgs e)
    {
        Auxiliares AuxiliarDelete;
        Int32 idAuxiliar = 0;

        GridDataItem item = (GridDataItem)e.Item;

        try
        {
            //Datos para identificar valor
            idAuxiliar = Convert.ToInt32(item["Id"].Text);

            AuxiliarDelete = new Auxiliares(idAuxiliar, string.Empty, string.Empty, Enums.Estado.Activo);

            //Llamar el SP correspondiente con las entidades old y new de Validacion
            AuxiliaresRules AuxiliarBorrar = new AuxiliaresRules();

            if (AuxiliarBorrar.Delete(AuxiliarDelete) > 0)
            {
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());

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
    protected void RgdAuxiliares_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertAux(e);
    }
    private void InsertAux(GridCommandEventArgs e)
    {
        AuxiliaresRules AuxiliarInsertar;
        try
        {
            Auxiliares record = this.ValidaNulos(e.Item as GridEditableItem);           

            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion

                if ( bValidaCampoVacio(record.Credito,"Crédito",e)&& bValidaCampoVacio(record.Auxiliar,"Auxiliar",e))
                {

                    if (bValidaCredito(record.Credito))
                    {

                        AuxiliarInsertar = new AuxiliaresRules();
                        if (AuxiliarInsertar.Insert(record) > 0)
                        {
                            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());

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
        catch (Exception ex)
        {
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    private bool bValidaCredito(string sCredito )
    {
        bool Valido = true;

        List<string> lsCredito = new List<string>();

        lsCredito = (List<string>)ViewState["Credito"];

        if (lsCredito.FindIndex(s => s == sCredito) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Crédito: " + sCredito + " ya se encuentra dado de alta.");
        }

        return Valido;
    }


    private Auxiliares ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        Auxiliares record;
        ArrayList estatus;
        Enums.Estado estado;

        estatus = Util.RadComboToString(editedItem["EstatusTemp"].FindControl("ComboEstatus"));

        if (estatus[1].ToString() == "1") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }

        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {

            if (newValues["Credito"] != null && newValues["Auxiliar"] != null)
            {

                record = new Auxiliares(0, newValues["Credito"].ToString(), newValues["Auxiliar"].ToString(), estado);
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, newValues);

            }
            else
            {
                record = new Auxiliares(0, string.Empty, string.Empty, Enums.Estado.Activo);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            record = new Auxiliares(0, string.Empty, string.Empty, Enums.Estado.Activo);
        }
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

    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdAuxiliares.Columns[0].Visible = false;
        RgdAuxiliares.Columns[RgdAuxiliares.Columns.Count - 1].Visible = false;
        RgdAuxiliares.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAuxiliares.ExportSettings.OpenInNewWindow = false;
        RgdAuxiliares.ExportSettings.ExportOnlyData = true;
        RgdAuxiliares.MasterTableView.GridLines = GridLines.Both;
        RgdAuxiliares.ExportSettings.IgnorePaging = true;
        RgdAuxiliares.ExportSettings.OpenInNewWindow = true;
        RgdAuxiliares.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdAuxiliares.Columns[0].Visible = false;
        RgdAuxiliares.Columns[RgdAuxiliares.Columns.Count - 1].Visible = false;
        RgdAuxiliares.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAuxiliares.ExportSettings.OpenInNewWindow = false;
        RgdAuxiliares.ExportSettings.ExportOnlyData = true;

        RgdAuxiliares.ExportSettings.IgnorePaging = true;
        RgdAuxiliares.ExportSettings.OpenInNewWindow = true;
        RgdAuxiliares.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdAuxiliares_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                item["EstatusTemp"].Text = item["Estatus"].Text;
                item["Auxiliar"].Text = string.Format("'{0}", item["Auxiliar"].Text);
            }
            else if (e.Item.IsInEditMode)
            {

                GridDataItem item;
                RadComboBox comboEstatus;

                item = (GridDataItem)e.Item;
                comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);
                if (newValues["Estatus"] != null)
                {

                    if (newValues["Estatus"].ToString() == "Activo")
                        comboEstatus.SelectedIndex = 0;
                    else
                        comboEstatus.SelectedIndex = 1;

                }
            }
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

    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdAuxiliares.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdAuxiliares.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;


        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CRED_REG_MANUAL")))
        {
            this.RgdAuxiliares.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELI_CRED_REG_MANUAL")))
        {
            //this.RgdAuxiliares.MasterTableView.GetColumn("DeleteState").Visible = true;
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
            btnExportPDF.Visible = true;
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
            ImageButton1.Visible = true;

        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }
    }
    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (persona == "PM")
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CRED_REG_MANUAL")))
            {
                valido = true;
            }
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CRED_REG_MANUAL")))
            {
                valido = true;
            }
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
                row1["nombreColumna"] = "Auxiliar";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Crédito";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "2";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "RFC";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "30";

                DataRow row4;

                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "Nombre";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "30";


                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);


                //List<DbParameter> parametros = new List<DbParameter>();

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
                parametros[3].ParameterName = "auxiliarp";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "creditop";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 30;

                string storeBase = "SP_cargaMasiva_credAux";


                dt_layout_procesado = cargaMasiva.cargaMasiva("Pais", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdAuxiliares.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdAuxiliares.Rebind();
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
            GridHeaderItem headerItem = RgdAuxiliares.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdAuxiliares.AllowPaging = false;
                RgdAuxiliares.Rebind();
                foreach (GridDataItem row in RgdAuxiliares.Items)
                {
                    Auxiliares AuxiliarDelete;
                    Int32 idAuxiliar = 0;
                    //Datos para identificar valor
                    idAuxiliar = Parser.ToNumber(row["Id"].Text.ToString());

                    AuxiliarDelete = new Auxiliares(idAuxiliar, string.Empty, string.Empty, Enums.Estado.Activo);

                    //Llamar el SP correspondiente con las entidades old y new de Validacion
                    AuxiliaresRules AuxiliarBorrar = new AuxiliaresRules();

                    if (AuxiliarBorrar.Delete(AuxiliarDelete) > 0)
                    {

                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }//foreach
                RgdAuxiliares.Rebind();
                RgdAuxiliares.DataSource = null;
                RgdAuxiliares.AllowPaging = true;
                RgdAuxiliares.Rebind();
                RgdAuxiliares.DataBind();
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdAuxiliares.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdAuxiliares.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {

                        Auxiliares AuxiliarDelete;
                        Int32 idAuxiliar = 0;
                        //Datos para identificar valor
                        idAuxiliar = Parser.ToNumber(row["Id"].Text.ToString());

                        AuxiliarDelete = new Auxiliares(idAuxiliar, string.Empty, string.Empty, Enums.Estado.Activo);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        AuxiliaresRules AuxiliarBorrar = new AuxiliaresRules();

                        if (AuxiliarBorrar.Delete(AuxiliarDelete) > 0)
                        {
                            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con id " + idAuxiliar, null, null, catalog, 1, null, null);
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
        RgdAuxiliares.Rebind();

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
                foreach (GridDataItem row in RgdAuxiliares.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdAuxiliares.Items)
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