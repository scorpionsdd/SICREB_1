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
using System.Text;

public partial class DiasInhabilesPage : System.Web.UI.Page
{
    public const String catalog = "Días Inhábiles";
    //variables a emplear
    string persona;
    System.Globalization.CultureInfo CultureMX = new System.Globalization.CultureInfo("es-MX", true);

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureMX;
        System.Threading.Thread.CurrentThread.CurrentUICulture = CultureMX;
        CambiaAtributosRGR();

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

    //JAGH se modifica para mostrar titulos filtros
    public void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdDiaInhabil.FilterMenu;
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

   
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdDiaInhabil.Columns[0].Visible = false;
        RgdDiaInhabil.Columns[2].HeaderStyle.Width = Unit.Parse("250mm");

        RgdDiaInhabil.Columns[RgdDiaInhabil.Columns.Count - 1].Visible = false;
        RgdDiaInhabil.MasterTableView.HierarchyDefaultExpanded = true;
        RgdDiaInhabil.ExportSettings.OpenInNewWindow = false;
        RgdDiaInhabil.ExportSettings.ExportOnlyData = true;
        RgdDiaInhabil.MasterTableView.GridLines = GridLines.Both;//TMR.- Nombre del archivo
        RgdDiaInhabil.ExportSettings.FileName = "Dias_Inhabiles";
        RgdDiaInhabil.ExportSettings.IgnorePaging = true;
        RgdDiaInhabil.ExportSettings.OpenInNewWindow = true;
        RgdDiaInhabil.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
        RgdDiaInhabil.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdDiaInhabil.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }

    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdDiaInhabil.Columns[0].Visible = false;
        RgdDiaInhabil.Columns[RgdDiaInhabil.Columns.Count - 1].Visible = false;
        RgdDiaInhabil.MasterTableView.HierarchyDefaultExpanded = true;
        RgdDiaInhabil.ExportSettings.OpenInNewWindow = false;
        RgdDiaInhabil.ExportSettings.ExportOnlyData = true;
        //TMR.- Nombre del archivo
        RgdDiaInhabil.ExportSettings.FileName = "Dias_Inhabiles";
        RgdDiaInhabil.ExportSettings.IgnorePaging = true;
        RgdDiaInhabil.ExportSettings.OpenInNewWindow = true;
        RgdDiaInhabil.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);
    }

    protected void RgdDiaInhabil_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DiaRules getRecords = null;
        List<Dia> DiaInfo;
        List<string> DiaInhabil = new List<string>();
        List<string> Dia = new List<string>();
        try
        {
            getRecords = new DiaRules();
            DiaInfo = getRecords.Dias();
            RgdDiaInhabil.DataSource = DiaInfo;
            RgdDiaInhabil.VirtualItemCount = DiaInfo.Count;

            for (int i = 0; i < DiaInfo.Count; i++)
            {

                DiaInhabil.Add(DiaInfo[i].dtFechaInhabil.ToShortDateString());
                Dia.Add(DiaInfo[i].dtFecha.ToShortDateString());
            }

            ViewState["FechaInhabil"] = DiaInhabil;
            ViewState["Fecha"] = Dia;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
    }

    protected void RgdDiaInhabil_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void RgdDiaInhabil_UpdateCommand(object source, GridCommandEventArgs e)
    {
        int idDia = 0;
        string idententicadorDia = string.Empty;
        DateTime dateTime;
        DateTime dtFecha = new DateTime();
        DateTime dtFechaInhabil = new DateTime();
        DateTime dtFechaInhabilOld = new DateTime();
        DateTime dtFechaOld = new DateTime();

        Dia record = new Dia(idDia, idententicadorDia, dtFecha, dtFechaInhabil);
        Dia diaOld;
        Dia diaNew;

        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
       
        bool bValida = true;

        if (DateTime.TryParse(oldValues["dtFechaInhabil"].ToString(), out dateTime))
            dtFechaInhabilOld = dateTime;

        if (DateTime.TryParse(oldValues["dtFecha"].ToString(), out dateTime))
            dtFechaOld = dateTime;
        
        try
        {

            if (newValues["idententicadorDia"] != null && newValues["dtFecha"] != null && newValues["dtFechaInhabil"] != null)
            {
                //Datos Orginales
                idDia = Convert.ToInt32(item.SavedOldValues["Id"]);
                //Datos para actualizar
                idententicadorDia = newValues["idententicadorDia"].ToString();
                if (DateTime.TryParse(newValues["dtFecha"].ToString(), out dateTime))
                    dtFecha = dateTime;
                if (DateTime.TryParse(newValues["dtFechaInhabil"].ToString(), out dateTime))
                    dtFechaInhabil = dateTime;

                if (dtFechaInhabil != dtFechaInhabilOld)
                {
                    bValida = bValidaFechaInhabil(dtFechaInhabil.ToShortDateString(), e);
                }

                if (bValida && dtFecha != dtFechaOld)
                {
                    bValida = bValidaFecha(dtFecha.ToShortDateString(), e);
                }


                if (bValida && bValidaCampoVacio(idententicadorDia, "Identificador de Día", e))
                {

                    diaOld = new Dia(idDia, idententicadorDia, dtFecha, dtFechaInhabil);
                    diaNew = new Dia(idDia, idententicadorDia, dtFecha, dtFechaInhabil);

                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
                    DiaRules DiaUpdate = new DiaRules();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    if (DiaUpdate.ActulizarDia(diaNew) > 0)
                    {
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro se modificó con éxito");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Registro no fue modificado");
                    }
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de Verificar los Datos Ingresados");
            }
           
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdDiaInhabil_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }

    protected void RgdDiaInhabil_DeleteCommand(object source, GridCommandEventArgs e)
    {
        Dia diaDelete;
        int IdDia = 0;
        GridDataItem item = (GridDataItem)e.Item;
        try
        {
            IdDia = Convert.ToInt32(item["Id"].Text);
            diaDelete = new Dia(IdDia, string.Empty, DateTime.Now, DateTime.Now);
            //Aqui se llama el SP correspondiente para eliminar
            DiaRules DiaBorrar = new DiaRules();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            if (DiaBorrar.BorrarDia(diaDelete) > 0)
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro", null, null, catalog, 1, null, null);
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue Removido");
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }

    }

    protected void RgdDiaInhabil_InsertCommand(object source, GridCommandEventArgs e)
    {
        insertDiaInhabil(e);
    }

    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdDiaInhabil.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdDiaInhabil.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        btnExportExcel.Visible = false;

        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TIPOMONEDA")))
            {
                this.RgdDiaInhabil.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPOMONEDA")))
            {
                //this.RgdDiaInhabil.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                btnExportExcel.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TIPOMONEDA")))
            {
                this.RgdDiaInhabil.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPOMONEDA")))
            {
                //this.RgdDiaInhabil.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                btnExportExcel.Visible = true;
        }
        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }
    }

    protected void RgdDiaInhabil_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            GridDataItem item;
            CheckBox chkDiaInhabil;

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

            //dar formato a fecha de columnas filtro
            if (e.Item is GridFilteringItem)
            {
                RadDatePicker pickerFecha = ((GridFilteringItem)e.Item)["Fecha"].Controls[0] as RadDatePicker;
                if (pickerFecha != null)
                {
                    pickerFecha.Culture = CultureMX;
                    pickerFecha.DateInput.DateFormat = "dd MMMM yyyy";
                    pickerFecha.DateInput.DisplayDateFormat = "dd MMMM yyyy";
                    pickerFecha.Width = 210;
                }

                RadDatePicker pickerFechaInhabil = ((GridFilteringItem)e.Item)["FechaInhabil"].Controls[0] as RadDatePicker;
                if (pickerFechaInhabil != null)
                {
                    pickerFechaInhabil.Culture = CultureMX;
                    pickerFechaInhabil.DateInput.DateFormat = "dddd, dd MMMM yyyy";
                    pickerFechaInhabil.DateInput.DisplayDateFormat = "dddd, dd MMMM yyyy";
                    pickerFechaInhabil.Width = 210;
                }

            }//fin dar formato a fecha de columnas filtro 

            //dar formato al insertar/editar fechas
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                RadDatePicker pickerFecha = ((GridEditableItem)e.Item)["Fecha"].Controls[0] as RadDatePicker;
                if (pickerFecha != null)
                {
                    pickerFecha.Culture = CultureMX;
                    pickerFecha.DateInput.DateFormat = "dd MMMM yyyy";
                    pickerFecha.DateInput.DisplayDateFormat = "dd MMMM yyyy";
                    pickerFecha.Width = 210;
                }

                RadDatePicker pickerFechaInhabil = ((GridEditableItem)e.Item)["FechaInhabil"].Controls[0] as RadDatePicker;
                if (pickerFechaInhabil != null)
                {
                    pickerFechaInhabil.Culture = CultureMX;
                    pickerFechaInhabil.DateInput.DateFormat = "dddd, dd MMMM yyyy";
                    pickerFechaInhabil.DateInput.DisplayDateFormat = "dddd, dd MMMM yyyy";
                    pickerFechaInhabil.Width = 210;
                }

            }//findar formato al insertar/editar fechas


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
                row1["nombreColumna"] = "Identificador de Día";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Fecha";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "20";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Fecha Inhábil";
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
                parametros[3].ParameterName = "pclavpIdententicadorDia";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 50;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "pdtFecha";
                parametros[4].DbType = DbType.String;  //JAGH 20/02/13 se modifica para coincidir con layout (store)


                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "pdtFechaInhabil";
                parametros[5].DbType = DbType.String;  //JAGH 20/02/13 se modifica para coincidir con layout (store)


                string storeBase = "SP_CARGAMASIVA_DIAS";


                dt_layout_procesado = cargaMasiva.cargaMasiva("DIAS", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'),Encoding.UTF8);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));

                int total = RgdDiaInhabil.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);


                RgdDiaInhabil.Rebind();

            }
        }
        catch (Exception ex)
        {


            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), Encoding.UTF8);

            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
            //RgdAccionistas.Rebind();
            //   Mensajes.ShowMessage(this.Page,this.GetType(),cargaMasiva.Log.ToString());
        }//try-Catch
        //
    }

    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        //elimina logicamente los registros 
        try
        {

            //elimina todos los registros
            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdDiaInhabil.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");

            if (chkHeader.Checked == true)
            {
                RgdDiaInhabil.AllowPaging = false;
                RgdDiaInhabil.Rebind();
                foreach (GridDataItem row in RgdDiaInhabil.Items)
                {
                    int idDia = Convert.ToInt32(row["ID"].Text.ToString());
                    DiaRules diaDelete = new DiaRules();
                    if (diaDelete.BorrarDia(new Dia(idDia, string.Empty, DateTime.Now, DateTime.Now)) > 0)
                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                    else
                        Mensajes.ShowAdvertencia(this.Page, GetType(), "No se eliminaron los datos, favor de verificar");

                }//foreach
                RgdDiaInhabil.Rebind();
                RgdDiaInhabil.DataSource = null;
                RgdDiaInhabil.AllowPaging = true;
                RgdDiaInhabil.Rebind();
                RgdDiaInhabil.DataBind();
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdDiaInhabil.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);


            }//header
            else
            {
                //registros seleccionados manualmente                
                foreach (GridDataItem row in RgdDiaInhabil.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;

                    if (chkResult)
                    {
                        int idDia = Convert.ToInt32(row["ID"].Text.ToString());
                        DiaRules diaDelete = new DiaRules();

                        if (diaDelete.BorrarDia(new Dia(idDia, string.Empty, DateTime.Now, DateTime.Now)) > 0)
                        {
                            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + idDia, null, null, catalog, 1, null, null);

                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, GetType(), "No se eliminaron los datos, favor de verificar");
                        }

                    }//checket
                }

                RgdDiaInhabil.Rebind();
            }
        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }//try-catch o
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
                foreach (GridDataItem row in RgdDiaInhabil.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdDiaInhabil.Items)
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

    private void insertDiaInhabil(GridCommandEventArgs e)
    {
        //ExceptuadosDataAccess ExceptuadoInsertar;
        DiaRules diInsertar;
        try
        {
            Dia record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {
                if (record.IdententicadorDia.Length > 0 || record.dtFecha.ToString().Length > 0 || record.dtFechaInhabil.ToString().Length > 0)
                {
                    if (bValidaCampoVacio(record.IdententicadorDia, "Identificador de Día", e) && bValidaFechaInhabil(record.dtFechaInhabil.ToShortDateString(), e) && bValidaFecha(record.dtFecha.ToShortDateString(), e))
                    {                    
                       diInsertar = new DiaRules();

                       if (diInsertar.InsertarDia(record) > 0)

                         Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");

                       else

                         Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                    }
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de Ingresar Todos los Datros");
                }
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    
    private bool bValidaFechaInhabil(string sFechaInhabil, Telerik.Web.UI.GridCommandEventArgs e)
    {

        bool Valido = true;

        List<string> lsFechaInhabil = new List<string>();

        lsFechaInhabil = (List<string>)ViewState["FechaInhabil"];

        if (lsFechaInhabil.FindIndex(s => s == sFechaInhabil) >= 0)
        {
            Valido = false;

            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La Fecha Inhábil: " + sFechaInhabil + " ya se encuentra dada de alta.");

            e.Canceled = true;
        }

        return Valido;
    }

    private bool bValidaFecha(string sFecha, Telerik.Web.UI.GridCommandEventArgs e)
    {

        bool Valido = true;

        List<string> lsFecha = new List<string>();

        lsFecha = (List<string>)ViewState["Fecha"];

        if (lsFecha.FindIndex(s => s == sFecha) >= 0)
        {
            Valido = false;

            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La Fecha : " + sFecha + " ya se encuentra dada de alta.");

            e.Canceled = true;
        }

        return Valido;
    }


    private Dia ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        Dia record;
        //ArrayList estatus;
        Enums.Estado estado;

        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {
            if (newValues["idententicadorDia"] != null && newValues["dtFecha"] != null && newValues["dtFechaInhabil"] != null)
            {

                if (newValues["idententicadorDia"].ToString().Length > 0 && newValues["dtFecha"].ToString().Length > 0 && newValues["dtFechaInhabil"].ToString().Length > 0)
                {
                    estado = new Enums.Estado();

                    int idDia = Convert.ToInt32(newValues["Id"]);
                    string idententicadorDia = newValues["idententicadorDia"].ToString();
                    DateTime dateTime;
                    DateTime dtFecha = new DateTime();
                    DateTime dtFechaInhabil = new DateTime();

                    if (DateTime.TryParse(newValues["dtFecha"].ToString(), out dateTime))
                        dtFecha = dateTime;
                    if (DateTime.TryParse(newValues["dtFechaInhabil"].ToString(), out dateTime))
                        dtFechaInhabil = dateTime;

                    estado = Enums.Estado.Activo;
                    record = new Dia(idDia, idententicadorDia, dtFecha, dtFechaInhabil);
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, newValues);

                }
                else
                {
                    record = new Dia(0, "", "", "");

                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de Verificar los Datos Ingresados");
                    //Response.Write("<script> alert('Favor de Verificar los Datos Ingresados')</script>");
                }
            }
            else
            {
                record = new Dia(0, "", "", "");

                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de Verificar los Datos Ingresados");
                //Response.Write("<script> alert('Favor de Verificar los Datos Ingresados')</script>");
            }
        }
        else
        {
            record = new Dia(0, "", "", "");
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
