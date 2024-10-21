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
public partial class CalificacionCarteraPage : System.Web.UI.Page
{
    public const String catalog = "Calificación de Cartera";
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
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingreso a Catálogo " + catalog);
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

        GridFilterMenu menu = RgdCalifCartera.FilterMenu;
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
    protected void RgdCalifCartera_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        CalificacionesRules getRecords = null;
        List<string> lstClaveBuro = new List<string>();

        List<Calificacion> CalificacionesInfo;
        try
        {
            getRecords = new CalificacionesRules();
            CalificacionesInfo = getRecords.GetRecords(false);
            RgdCalifCartera.DataSource = CalificacionesInfo;
            RgdCalifCartera.VirtualItemCount = CalificacionesInfo.Count;

            ///BMS
            for (int i = 0; i < CalificacionesInfo.Count; i++)
            {

                lstClaveBuro.Add(CalificacionesInfo[i].ClaveBuro);
            }

            ViewState["ClaveBuro"] = lstClaveBuro;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdCalifCartera_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

        GridDataItem item = (GridDataItem)e.Item;

    }
    protected void RgdCalifCartera_UpdateCommand(object source, GridCommandEventArgs e)
    {
        string ClaveOld = string.Empty;
        string claveBuro = string.Empty;
        string claveBuroValidar = string.Empty;
        string descripcion = string.Empty;
        bool bvalida = false;
        ArrayList estatus;
        Int32 idCalif = 0;
        Calificacion calificacionOld;
        Calificacion calificacionNew;
        Enums.Estado estado;

        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        try
        {
            ///BMS
            claveBuroValidar = newValues["ClaveBuro"].ToString();
            ClaveOld = item.SavedOldValues["ClaveBuro"].ToString();

            if (ClaveOld != claveBuroValidar)
            {
                if (bValidaClave(claveBuroValidar))
                {
                    bvalida = true;
                }
            }
            else
                bvalida = true;

            if (bvalida)
            {

                //Datos Orginales
                idCalif = Convert.ToInt32(item.SavedOldValues["Id"]);
                //Datos Nuevos
                claveBuro = newValues["ClaveBuro"].ToString();
                descripcion = newValues["Descripcion"].ToString();
                estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));

                if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }

                calificacionOld = new Calificacion(idCalif, claveBuro, descripcion, estado);
                calificacionNew = new Calificacion(idCalif, claveBuro, descripcion, estado);


                //Llamar el SP correspondiente con las entidades old y new
                CalificacionesRules CalificacionesValores = new CalificacionesRules();

                if (CalificacionesValores.Update(calificacionOld, calificacionNew) > 0)
                {
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                    ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);

                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                }
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
        estatus = null;
    }

    protected void RgdCalifCartera_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdCalifCartera_DeleteCommand(object source, GridCommandEventArgs e)
    {
        Calificacion CalificacionDelete;
        string claveBuro = string.Empty;
        Int32 idCalificacion = 0;
        GridDataItem item = (GridDataItem)e.Item;

        try
        {
            //Datos para identificar valor
            idCalificacion = Parser.ToNumber(item["Id"].Text);
            claveBuro = item["ClaveBuro"].Text;

            CalificacionDelete = new Calificacion(idCalificacion, claveBuro, string.Empty, Enums.Estado.Activo);
            //Llamar el SP correspondiente con las entidades old y new
            CalificacionesRules CalificacionBorrar = new CalificacionesRules();

            if (CalificacionBorrar.Delete(CalificacionDelete) > 0)
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
    protected void RgdCalifCartera_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertCalificacion(e);
    }
    private void InsertCalificacion(GridCommandEventArgs e)
    {
        CalificacionesRules CalificacionInsert;
        try
        {
            Calificacion record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion
                if (record.Id != 0 || record.ClaveBuro != "")
                {
                    if (bValidaClave(record.ClaveBuro))
                    {
                        CalificacionInsert = new CalificacionesRules();
                        if (CalificacionInsert.Insert(record) > 0)
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
        catch (Exception ex)
        {
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }


    private bool bValidaClave(string sClaveBuro)
    {
        bool Valido = true;

        List<string> lsSic = new List<string>();

        lsSic = (List<string>)ViewState["ClaveBuro"];

        if (lsSic.FindIndex(s => s == sClaveBuro) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Clave Buró : " + sClaveBuro + " ya se encuentra dada de alta.");
        }

        return Valido;
    }

    private Calificacion ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        Calificacion record;
        ArrayList estatus;
        Enums.Estado estado;

        estatus = Util.RadComboToString(editedItem["EstatusTemp"].FindControl("ComboEstatus"));
        if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }

        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {

            if (newValues["ClaveBuro"] != null && newValues["Descripcion"] != null && estatus != null)
            {

                record = new Calificacion(0, newValues["ClaveBuro"].ToString(), newValues["Descripcion"].ToString(), estado);
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, newValues);

            }
            else
            {
                record = new Calificacion(0, string.Empty, string.Empty, Enums.Estado.Activo);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            record = new Calificacion(0, string.Empty, string.Empty, Enums.Estado.Activo);
        }
        estatus = null;
        return record;
    }
    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdCalifCartera.Columns[0].Visible = false;
        RgdCalifCartera.Columns[RgdCalifCartera.Columns.Count - 1].Visible = false;
        RgdCalifCartera.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCalifCartera.ExportSettings.OpenInNewWindow = false;
        RgdCalifCartera.ExportSettings.ExportOnlyData = true;
        RgdCalifCartera.MasterTableView.GridLines = GridLines.Both;
        RgdCalifCartera.ExportSettings.IgnorePaging = true;
        RgdCalifCartera.ExportSettings.OpenInNewWindow = true;
        RgdCalifCartera.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdCalifCartera.Columns[0].Visible = false;
        RgdCalifCartera.Columns[RgdCalifCartera.Columns.Count - 1].Visible = false;
        RgdCalifCartera.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCalifCartera.ExportSettings.OpenInNewWindow = false;
        RgdCalifCartera.ExportSettings.ExportOnlyData = true;

        RgdCalifCartera.ExportSettings.IgnorePaging = true;
        RgdCalifCartera.ExportSettings.OpenInNewWindow = true;
        RgdCalifCartera.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);


    }
    protected void RgdCalifCartera_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            RadComboBox comboEstatus;

            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                item["EstatusTemp"].Text = item["Estatus"].Text;
            }
            else if (e.Item.IsInEditMode) //(e.Item.ItemType == GridItemType.CommandItem)
            {

                GridDataItem item;
                //BMS omboBox comboEstatus;

                item = (GridDataItem)e.Item;
                comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
                
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);


                comboEstatus.SelectedIndex = 0;
                //BMS comboEstatus.Enabled = false;
                comboEstatus.Enabled = true;

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

    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdCalifCartera.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdCalifCartera.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CALIFCARTERA")))
            {
                this.RgdCalifCartera.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CALIFCARTERA")))
            {
                //this.RgdCalifCartera.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CALIFCARTERA")))
            {
                this.RgdCalifCartera.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CALIFCARTERA")))
            {
                //this.RgdCalifCartera.MasterTableView.GetColumn("DeleteState").Visible = true;
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
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CALIFCARTERA")))
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
            GridHeaderItem headerItem = RgdCalifCartera.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdCalifCartera.AllowPaging = false;
                RgdCalifCartera.Rebind();
                foreach (GridDataItem row in RgdCalifCartera.Items)
                {

                    Calificacion CalificacionDelete;
                    string claveBuro = string.Empty;
                    Int32 idCalificacion = 0;


                    //Datos para identificar valor
                    idCalificacion = Parser.ToNumber(row["Id"].Text.ToString());
                    claveBuro = row["ClaveBuro"].Text;

                    CalificacionDelete = new Calificacion(idCalificacion, claveBuro, string.Empty, Enums.Estado.Activo);
                    //Llamar el SP correspondiente con las entidades old y new
                    CalificacionesRules CalificacionBorrar = new CalificacionesRules();

                    if (CalificacionBorrar.Delete(CalificacionDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                        ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro", null, null, catalog, 1, null, null);

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }//foreach
                RgdCalifCartera.Rebind();
                RgdCalifCartera.DataSource = null;
                RgdCalifCartera.AllowPaging = true;
                RgdCalifCartera.Rebind();
                RgdCalifCartera.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdCalifCartera.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdCalifCartera.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {

                        Calificacion CalificacionDelete;
                        string claveBuro = string.Empty;
                        Int32 idCalificacion = 0;


                        //Datos para identificar valor
                        idCalificacion = Parser.ToNumber(row["Id"].Text.ToString());
                        claveBuro = row["ClaveBuro"].Text;


                        CalificacionDelete = new Calificacion(idCalificacion, claveBuro, string.Empty, Enums.Estado.Activo);
                        //Llamar el SP correspondiente con las entidades old y new
                        CalificacionesRules CalificacionBorrar = new CalificacionesRules();

                        if (CalificacionBorrar.Delete(CalificacionDelete) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con id " + idCalificacion, null, null, catalog, 1, null, null);

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
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch
        //btn_eliminar_Click
        RgdCalifCartera.Rebind();


    }
    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            CheckBox chkHeader;

            chkHeader = (CheckBox)sender;
         
            foreach (GridDataItem row in RgdCalifCartera.Items)
            {
                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkCell.Checked = chkHeader.Checked;
                row["EstatusTemp"].Text = row["Estatus"].Text;
            }
          
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
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
                row1["nombreColumna"] = "Clave Buró";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Descripción";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "200";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Estatus";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "30";




                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);



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
                parametros[3].ParameterName = "clave_burop";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "descripcionp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 200;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "estatusp";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 30;





                string storeBase = "SP_CARGAMASIVA_CALIFICACIONES";


                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_accionistas", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "PM");
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdCalifCartera.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdCalifCartera.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catch
    }
}