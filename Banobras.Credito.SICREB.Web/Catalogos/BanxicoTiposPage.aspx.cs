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

public partial class BanxicoTiposPage : System.Web.UI.Page
{
    public const String catalog = "Tipos de Actividad Económica Banxico";
    string persona;
    int idUs;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {
                persona = "PM";
                getFacultades(persona);
                CambiaAtributosRGR();
                if (!this.Page.IsPostBack)
                {
                   
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

        GridFilterMenu menu = RgdBanxicoTipo.FilterMenu;
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
                  menu.Items.RemoveAt(i);
            }
            else if (menu.Items[i].Text == "NotIsEmpty")
            {
                // item.Text = "Finalice con";
                 menu.Items.RemoveAt(i);
            }                
              
        }

    }
    protected void RgdBanxicoTipo_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BanxicoTiposRules getRecords = null;
        string Persona = string.Empty;
        List<BanxicoTipo> BanxicoTipoInfo;
        try
        {
            getRecords = new BanxicoTiposRules();
            BanxicoTipoInfo = getRecords.GetRecords(false);
            RgdBanxicoTipo.DataSource = BanxicoTipoInfo;
            RgdBanxicoTipo.VirtualItemCount = BanxicoTipoInfo.Count;
        }
        catch (Exception exc)
        {
            //Mensajes.ShowError(exc, this.Page);
        }
    }
    protected void RgdBanxicoTipo_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

        GridDataItem item = (GridDataItem)e.Item;

    }
    protected void RgdBanxicoTipo_UpdateCommand(object source, GridCommandEventArgs e)
    {
        string descripcion = string.Empty;
        ArrayList estatus;
        Enums.Estado estado;
        Int32 idTipo = 0;
        BanxicoTipo oldTipo;
        BanxicoTipo newTipo;

        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        try
        {
            //Datos Orginales
            idTipo = Convert.ToInt32(item.SavedOldValues["Id"]);

            if (newValues["Descripcion"] != null && bValidaCampoVacio(newValues["Descripcion"].ToString(), "Descripción", e))
            {

                //Datos Nuevos
                descripcion = newValues["Descripcion"].ToString();

                estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));
                Enums.Persona persona = new Enums.Persona();

                if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }

                oldTipo = new BanxicoTipo(idTipo, descripcion, estado);
                newTipo = new BanxicoTipo(idTipo, descripcion, estado);

                //Llamar el SP correspondiente con las entidades old y new de Validacion
                BanxicoTiposRules bnaxicoTipo = new BanxicoTiposRules();

                if (bnaxicoTipo.Update(oldTipo, newTipo) > 0)
                {
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro ", null, null, catalog, 1, oldValues, newValues);
                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                }
                else
                {
                    //JAGH se agrega actividad 20/01/13
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(800, idUs, "El registro no fue modificado catálogo Banxico tipos");
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Registro no fue Modificado");
                }
            }
        }
        catch (Exception exc)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue modificado catálogo Banxico tipos");

            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdBanxicoTipo_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdBanxicoTipo_DeleteCommand(object source, GridCommandEventArgs e)
    {
        BanxicoTipo TipoDelete;
        Int32 idTipo = 0;

        //Datos para identificar valor
        GridDataItem item = (GridDataItem)e.Item;

        try
        {
            idTipo = Convert.ToInt32(item["Id"].Text);

            Enums.Persona persona = new Enums.Persona();
            TipoDelete = new BanxicoTipo(idTipo, string.Empty, Enums.Estado.Activo);

            //Llamar el SP correspondiente con las entidades old y new de Validacion
            BanxicoTiposRules TipoBorrar = new BanxicoTiposRules();

            if (TipoBorrar.Delete(TipoDelete) > 0)
            {
                //JAGH se agrega actividad 20/01/13
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(1111, idUs, "El registro fue eliminado catálogo Banxico tipos");
                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
            }
            else
            {
                //JAGH se agrega actividad 20/01/13
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(800, idUs, "El registro no fue eliminado catálogo Banxico tipos");
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Registro no fue Removido");
            }

        }
        catch (Exception exc)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue modificado catálogo Banxico tipos");
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdBanxicoTipo_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertEstado(e);
    }
    private void InsertEstado(GridCommandEventArgs e)
    {
        BanxicoTiposRules TipoInsertar;
        try
        {
            BanxicoTipo record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion
                if (bValidaCampoVacio(record.Descripcion, "Descripcion",e))
                {
                    TipoInsertar = new BanxicoTiposRules();
                    if (TipoInsertar.Insert(record) > 0)
                    {
                        //JAGH se agrega actividad 20/01/13
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        ActividadRules.GuardarActividad(7777, idUs, "El registro fue agregado catálogo Banxico tipos");
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                    }
                    else
                    {
                        //JAGH se agrega actividad 20/01/13
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        ActividadRules.GuardarActividad(800, idUs, "El registro no fue agregado catálogo Banxico tipos");
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //JAGH se agrega actividad 20/01/13
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue agregado catálogo Banxico tipos");
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    private BanxicoTipo ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        BanxicoTipo record;
        ArrayList estatus;
        Enums.Estado estado;

        estatus = Util.RadComboToString(editedItem["EstatusTemp"].FindControl("ComboEstatus"));
        if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else { estado = Enums.Estado.Inactivo; }
        // Extrae todos los elementos

        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {
            if (newValues["Descripcion"] != null)
            {
                record = new BanxicoTipo(0, newValues["Descripcion"].ToString(), estado);
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro ", null, null, catalog, 1, null, newValues);
            }
            else
            {
                record = new BanxicoTipo(0, string.Empty, Enums.Estado.Activo);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de Verificar los Datos Ingresados");
            }
        }
        else
        {
            record = new BanxicoTipo(0, string.Empty, Enums.Estado.Activo);
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
        RgdBanxicoTipo.Columns[0].Visible = false;
        RgdBanxicoTipo.Columns[RgdBanxicoTipo.Columns.Count - 1].Visible = false;
        RgdBanxicoTipo.MasterTableView.HierarchyDefaultExpanded = true;
        RgdBanxicoTipo.ExportSettings.OpenInNewWindow = false;
        RgdBanxicoTipo.ExportSettings.ExportOnlyData = true;
        RgdBanxicoTipo.MasterTableView.GridLines = GridLines.Both;
        RgdBanxicoTipo.ExportSettings.IgnorePaging = true;
        RgdBanxicoTipo.ExportSettings.OpenInNewWindow = true;
        RgdBanxicoTipo.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el Catálogo " + catalog);

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdBanxicoTipo.Columns[0].Visible = false;
        RgdBanxicoTipo.Columns[RgdBanxicoTipo.Columns.Count - 1].Visible = false;
        RgdBanxicoTipo.MasterTableView.HierarchyDefaultExpanded = true;
        RgdBanxicoTipo.ExportSettings.OpenInNewWindow = false;
        RgdBanxicoTipo.ExportSettings.ExportOnlyData = true;

        RgdBanxicoTipo.ExportSettings.IgnorePaging = true;
        RgdBanxicoTipo.ExportSettings.OpenInNewWindow = true;
        RgdBanxicoTipo.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el Catálogo " + catalog);

    }
    protected void RgdBanxicoTipo_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                item["EstatusTemp"].Text = item["Estatus"].Text;
            }
            else if (e.Item.IsInEditMode)
            {

                GridDataItem item;
                item = (GridDataItem)e.Item;
                RadComboBox comboEstatus;               
               
                item["Id"].Enabled = false;           
                
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
        this.RgdBanxicoTipo.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdBanxicoTipo.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;
        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TMOD_AEBA")))
            {
                this.RgdBanxicoTipo.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TMOD_AEBA")))
            {
                //this.RgdBanxicoTipo.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TMOD_AEBA")))
            {
                this.RgdBanxicoTipo.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TMOD_AEBA")))
            {
                //this.RgdBanxicoTipo.MasterTableView.GetColumn("DeleteState").Visible = true;
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_TMOD_AEBA")))
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
                row1["nombreColumna"] = "Id";
                row1["tipoDato"] = "DECIMAL";
                row1["longitud"] = "30";

                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Descripción";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "30";

                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Estatus";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "30";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);

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
                parametros[3].ParameterName = "idp";
                parametros[3].DbType = DbType.Decimal;
                parametros[3].Size = 5;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "descripcionp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 250;

                string storeBase = "SP_CM_ACTIBANXICO";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_tipos_actividad_banxico", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);

                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdBanxicoTipo.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros ", numeros + " Registros ", catalog, 1);

                RgdBanxicoTipo.Rebind();
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
            GridHeaderItem headerItem = RgdBanxicoTipo.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdBanxicoTipo.AllowPaging = false;
                RgdBanxicoTipo.Rebind();
                foreach (GridDataItem row in RgdBanxicoTipo.Items)
                {
                    BanxicoTipo TipoDelete;
                    Int32 idTipo = 0;

                    idTipo = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());

                    Enums.Persona persona = new Enums.Persona();
                    TipoDelete = new BanxicoTipo(idTipo, string.Empty, Enums.Estado.Activo);

                    //Llamar el SP correspondiente con las entidades old y new de Validacion
                    BanxicoTiposRules TipoBorrar = new BanxicoTiposRules();

                    if (TipoBorrar.Delete(TipoDelete) > 0)
                    {
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                        //ActividadRules.GActividadCatalogo(1111, idUs, "Eliminacion de Registro", null, null, catalog, 1,null, null);
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Registro no fue Removido");
                    }


                }//foreach
                RgdBanxicoTipo.Rebind();
                RgdBanxicoTipo.DataSource = null;
                RgdBanxicoTipo.AllowPaging = true;
                RgdBanxicoTipo.Rebind();
                RgdBanxicoTipo.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdBanxicoTipo.VirtualItemCount) + " Registros ", "0 registros ", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdBanxicoTipo.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {

                        BanxicoTipo TipoDelete;
                        Int32 idTipo = 0;



                        idTipo = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());

                        Enums.Persona persona = new Enums.Persona();
                        TipoDelete = new BanxicoTipo(idTipo, string.Empty, Enums.Estado.Activo);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        BanxicoTiposRules TipoBorrar = new BanxicoTiposRules();

                        if (TipoBorrar.Delete(TipoDelete) > 0)
                        {
                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con id " + idTipo, null, null, catalog, 1, null, null);

                        }
                        else
                        {
                            //JAGH se agrega actividad 20/01/13
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(800, idUs, "El registro " + idTipo + " no fue eliminado catálogo Banxico tipos");
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Registro no fue Removido");
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
        RgdBanxicoTipo.Rebind();

    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;          
            CheckBox chkHeader;
            chkHeader = (CheckBox)sender;

            foreach (GridDataItem row in RgdBanxicoTipo.Items)
            {
                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkCell.Checked = chkHeader.Checked;

                row["EstatusTemp"].Text = row["Estatus"].Text;
            }
        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }//try-catch


        //ChkTodo_CheckedChanged
    }

}