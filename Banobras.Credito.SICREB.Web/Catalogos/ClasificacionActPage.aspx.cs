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

public partial class ClasificacionActPage : System.Web.UI.Page
{
    //MASS 24-jun-2013
    public const String catalog = "Clasificación Actual";        
    int idUs;
    int SeleccionTipo = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CambiaAtributosRGR();

            if (Session["Facultades"] != null)
            {
                getFacultades("n/a");
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }

            if (!this.Page.IsPostBack)
            {                
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó a Catálogo " + catalog);
            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }

    }

    protected void RgdCuentas_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {        
        //MASS 24-jun-2013
        ClasificacionAct_Rules getRecords = null;
        List<ClasificacionAct> cuentasInfo;
        List<string> lsCodigo = new List<string>();
        RgdCuentas.EnableLinqExpressions = false;

        try
        {
            getRecords = new ClasificacionAct_Rules();
            cuentasInfo = getRecords.GetRecords(false);
            RgdCuentas.DataSource = cuentasInfo;
            RgdCuentas.VirtualItemCount = cuentasInfo.Count;

            for (int i = 0; i < cuentasInfo.Count; i++)
            {
                lsCodigo.Add(cuentasInfo[i].descripcion + "-" + cuentasInfo[i].vigente);
            }

            ViewState["Codigo"] = lsCodigo;

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    //JAGH se modifica para mostrar titulos filtros
    public void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdCuentas.FilterMenu;
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
    
    protected void RgdCuentas_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    

    protected void RgdCuentas_UpdateCommand(object source, GridCommandEventArgs e)
    {

        Int32 id = 0;
        string Descripcion = string.Empty;
        string EstatusClas = string.Empty;;      

        //MASS 24-jun-2013
        ClasificacionAct cuentaOld;
        ClasificacionAct cuentaNew;                
        
        bool Actualizar = true;

        GridEditableItem editedItem = e.Item as GridEditableItem;

        try
        {
            RadComboBox ComboTipo = (RadComboBox)editedItem.FindControl("ComboTipo");
            EstatusClas = ComboTipo.SelectedValue.ToString();
        }
        catch
        {
        }

        (editedItem["Vigente"].Controls[0] as TextBox).Text = EstatusClas;


        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;

        string sDescripcionOld = oldValues["descripcion"].ToString().Replace("'", "");
        string sEstatusClasOld = oldValues["vigente"].ToString().Replace("'", "");

        try
        {
            if (newValues["descripcion"] != null && newValues["vigente"] != null)
            {
                Descripcion = newValues["descripcion"].ToString().Replace("'", "");
                EstatusClas = newValues["vigente"].ToString().Replace("'", "");

                if (sDescripcionOld != Descripcion || sEstatusClasOld != EstatusClas)
                {
                    //campos para efectuar validacion codigo
                    string strCodigo = newValues["descripcion"].ToString() + "-" + newValues["vigente"].ToString();
                    Actualizar = bValidaCodigo(strCodigo.Replace("'", ""));
                }

                if (Actualizar)
                {
                    //Datos Orginales
                    id = Parser.ToNumber(newValues["id"]);                    

                    //Datos Nuevos
                    Descripcion = newValues["descripcion"].ToString();                    
                   
                    //MASS 24-jun-2013
                    cuentaOld = new ClasificacionAct(id, Descripcion, sEstatusClasOld);
                    cuentaNew = new ClasificacionAct(id, Descripcion, EstatusClas);

                    //Llamar el SP correspondiente con las entidades old y new de Validacion
                    //MASS 24-jun-2013
                    ClasificacionAct_Rules CuentaValores = new ClasificacionAct_Rules();

                    if (CuentaValores.Update(cuentaOld, cuentaNew) > 0)
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
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
        finally
        {
            RegresarPropiedades();
        }
    }

    protected void RgdCuentas_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {

       
    }
    protected void RgdCuentas_DeleteCommand(object source, GridCommandEventArgs e)
    {
        //MASS 24-jun-2013        
        ClasificacionAct CuentaDelete;

        Int32 idCuenta = 0;
        //Datos para identificar valor

        GridDataItem item = (GridDataItem)e.Item;

        try
        {
            idCuenta = Parser.ToNumber(item["id"].Text);

            //MASS 24-jun-2013                        
            CuentaDelete = new ClasificacionAct(idCuenta, "", "");

            //Llamar el SP correspondiente con las entidades old y new de Validacion

            //MASS 24-jun-2013            
            ClasificacionAct_Rules CuentaBorrar = new ClasificacionAct_Rules();

            if (CuentaBorrar.Delete(CuentaDelete) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
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
    protected void RgdCuentas_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertEstado(e);
        RegresarPropiedades();
    }
    private void InsertEstado(GridCommandEventArgs e)
    {
        //MASS 24-jun-2013
        ClasificacionAct_Rules CuentaInsertar;
       
        try
        {
            //MASS 24/06/13
            ClasificacionAct record = this.ValidaNulos(e.Item as GridEditableItem);
            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion
                if (record.descripcion != string.Empty && record.vigente != string.Empty)
                {
                    //JAGH se agregan campos para efectuar validacion codigo
                    string strCodigo = record.descripcion + "-" + record.vigente;

                    if (bValidaCodigo(strCodigo))  //(record.Codigo))
                    {
                        //MASS 24-jun-2013
                        CuentaInsertar = new ClasificacionAct_Rules();

                        if (CuentaInsertar.Insert(record) > 0)
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

            string Cadena = "Error en la insercción ID duplicado Nulo o Inexistente, valores ( , , , , , , , , , , )" ;

            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), Cadena.Split('\n'), System.Text.Encoding.Unicode);

            Mensajes.ShowMessage(this.Page, this.GetType(), ex.Message + " <br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>");
        }
    }

    private bool bValidaCodigo(string sCodigo)
    {

        bool Valido = true;

        List<string> lsCodigo = new List<string>();

        lsCodigo = (List<string>)ViewState["Codigo"];

        if (lsCodigo.FindIndex(s => s == sCodigo) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "El código: " + sCodigo + " ya se encuentra dado de alta.");
        }

        return Valido;
    } 

    //MASS 24-jun-2013
    private ClasificacionAct ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();

        //MASS 24-jun-2013
        ClasificacionAct record;        
        
        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);
        
        if (newValues.Count > 0)
        {
            if (newValues["descripcion"] != null)
            {
                string estado_clas = "";                

                try
                {
                    RadComboBox ComboTipo = (RadComboBox)editedItem.FindControl("ComboTipo");
                    estado_clas = ComboTipo.SelectedValue.ToString();
                }
                catch
                {
                }

                //MASS 24-jun-2013                
                record = new ClasificacionAct(0, newValues["descripcion"].ToString(), estado_clas);

                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, newValues);

            }
            else
            {
                //MASS 24-jun-2013                
                record = new ClasificacionAct(0, string.Empty, "");
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            //MASS 24-jun-2013            
            record = new ClasificacionAct(0, string.Empty, "");
        }
        return record;

    }
    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdCuentas.Columns[0].Visible = false;
        RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
        RgdCuentas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = false;
        RgdCuentas.ExportSettings.ExportOnlyData = true;

        RgdCuentas.MasterTableView.GridLines = GridLines.Both;
        RgdCuentas.ExportSettings.IgnorePaging = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = true;
        RgdCuentas.ExportSettings.Pdf.PageHeight = Unit.Parse("250mm");
        RgdCuentas.ExportSettings.Pdf.PageWidth = Unit.Parse("550mm");
        RgdCuentas.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdCuentas.Columns[0].Visible = false;
        RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
        RgdCuentas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = false;
        RgdCuentas.ExportSettings.ExportOnlyData = true;

        RgdCuentas.ExportSettings.IgnorePaging = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = true;
        RgdCuentas.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdCuentas_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;                

                if (item["Vigente"] != null)
                {
                    if (item["Vigente"].Text == "1")
                    {
                        item["Vigente"].Text = "Vigente";                        
                    }
                    else
                    {
                        item["Vigente"].Text = "Vencido";                        
                    }

                    item["VigenteTemp"].Text = item["Vigente"].Text;
                }
            }
            else if (e.Item.IsInEditMode)
            {
                GridDataItem items = (GridDataItem)e.Item;
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);

                RadComboBox comboTipo = (RadComboBox)items["VigenteTemp"].FindControl("ComboTipo");

                if (newValues["vigente"] != null)
                {
                    if (newValues["vigente"].ToString() == "1")
                    {
                        comboTipo.SelectedValue = "1";
                    }
                    else
                    {
                        comboTipo.SelectedValue = "0";
                    }                   
                    
                }                

                this.RgdCuentas.MasterTableView.GetColumn("VigenteTemp").Visible = true;
                this.RgdCuentas.MasterTableView.GetColumn("Vigente").Visible = false;                               
                
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                RadComboBox combo = (RadComboBox)filterItem.FindControl("FiltroRadCombo");
                combo.SelectedValue = SeleccionTipo.ToString();
            }

            if (e.Item is GridCommandItem)
            {
                //<MASS 15-jul-2013>
                /*Button addButton = e.Item.FindControl("AddNewRecordButton") as Button;
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
                }*/
                //</MASS>
            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);

        }
    }

    protected void FiltroRadCombo_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (e.Value != "-1")
        {
            SeleccionTipo = Parser.ToNumber(e.Value);                      
            GridColumn column = RgdCuentas.MasterTableView.GetColumnSafe("vigente");
            column.CurrentFilterFunction = GridKnownFunction.EqualTo;
            string filterExpression = "[vigente] = '" + e.Value + "'";            
            RgdCuentas.MasterTableView.FilterExpression = filterExpression;
            RgdCuentas.MasterTableView.Rebind();
        }
        else
        {
            SeleccionTipo = Parser.ToNumber(e.Value);
            GridColumn column = RgdCuentas.MasterTableView.GetColumnSafe("vigente");
            column.CurrentFilterFunction = GridKnownFunction.DoesNotContain;
            RgdCuentas.MasterTableView.FilterExpression = "[vigente] <> ''";
            RgdCuentas.MasterTableView.Rebind();
        }
    }

    public void PosicionaItemRadComboBox(GridDataItem item, string val_estatus)
    {
        RadComboBox FiltroEstatusEdit = (RadComboBox)item["vigenteTemp"].FindControl("FiltroEstatusEdit");       

        if (val_estatus == "1")
        {
            FiltroEstatusEdit.SelectedValue = "1";
        }
        else
        {
            FiltroEstatusEdit.SelectedValue = "0";
        }        
    }


    public void RegresarPropiedades()
    {
        
    }  

    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdCuentas.MasterTableView.GetColumn("EditCommandColumn").Visible = false;        
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MCCTAS")))
            {
                this.RgdCuentas.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ECCTAS")))
            {                
                valido = true;
            }

            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MCCTAS")))
            {
                this.RgdCuentas.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ECCTAS")))
            {                
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AACTAS")))
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
                row1["nombreColumna"] = "descripcion";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "50";

                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "tipo";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "50";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2); 
                
                DbParameter[] parametros;

                parametros = new DbParameter[4];
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
                parametros[2].ParameterName = "pDescripcion";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 50;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "pVigente";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 50;
                

                //MASS 19-jun-2014
                string storeBase = "PACKCLASIFICACION_ACT.SP_CARGAMASIVA";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_clasificacion_act", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "");
                int numeros = cargaMasiva.Correctos;

                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }

                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));

                int total = RgdCuentas.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdCuentas.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error En la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }

    }
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdCuentas.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdCuentas.AllowPaging = false;
                RgdCuentas.Rebind();
                foreach (GridDataItem row in RgdCuentas.Items)
                {
                    //MASS 24-jun-2013
                    ClasificacionAct CuentasDelete;

                    int pId = Parser.ToNumber(row.GetDataKeyValue("id").ToString());

                    //MASS 24-jun-2013                    
                    CuentasDelete = new ClasificacionAct(pId, "", "");

                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
                    //CuentasDataAccess CuentasBorrar = new CuentasDataAccess();

                    //MASS 24-jun-2013
                    ClasificacionAct_Rules CuentasBorrar = new ClasificacionAct_Rules();
                    if (CuentasBorrar.Delete(CuentasDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los Registros se han removido correctamente");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }

                RgdCuentas.Rebind();
                RgdCuentas.DataSource = null;
                RgdCuentas.AllowPaging = true;
                RgdCuentas.Rebind();
                RgdCuentas.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdCuentas.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);


            }//header
            else
            {
                foreach (GridDataItem row in RgdCuentas.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {

                        //MASS 24-jun-2013
                        ClasificacionAct CuentaDelete;
                        Int32 idCuenta = 0;
                        //Datos para identificar valor

                        idCuenta = Parser.ToNumber(row.GetDataKeyValue("id").ToString());

                        //MASS 24-jun-2013                        
                        CuentaDelete = new ClasificacionAct(idCuenta, "", "");

                        //Llamar el SP correspondiente con las entidades old y new de Validacion

                        //MASS 24-jun-2013
                        ClasificacionAct_Rules CuentaBorrar = new ClasificacionAct_Rules();

                        if (CuentaBorrar.Delete(CuentaDelete) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + idCuenta, null, null, catalog, 1, null, null);
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                        }
                    }
                }
            }
        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }

        RgdCuentas.Rebind();

    }
    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            CheckBox chkHeader;

            chkHeader = (CheckBox)sender;

            foreach (GridDataItem row in RgdCuentas.Items)
            {
                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkCell.Checked = chkHeader.Checked;
            }

        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }
        
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

    protected void RgdCuentas_CancelCommand(object source, GridCommandEventArgs e)
    {
        this.RgdCuentas.MasterTableView.GetColumn("VigenteTemp").Visible = false;
        this.RgdCuentas.MasterTableView.GetColumn("Vigente").Visible = true;
    }
}