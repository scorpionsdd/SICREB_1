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

public partial class ConvertidorActPage : System.Web.UI.Page
{
    //MASS 20/06/13
    public const String catalog = "Convertidor Actual";
    int idUs;

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
        //MASS 20/06/13
        ConvertidorAct_Rules getRecords = null;
        List<ConvertidorAct> cuentasInfo;
        List<string> lsCodigo = new List<string>();

        try
        {
            getRecords = new ConvertidorAct_Rules();
            cuentasInfo = getRecords.GetRecords(false);
            RgdCuentas.DataSource = cuentasInfo;
            RgdCuentas.VirtualItemCount = cuentasInfo.Count;

            for (int i = 0; i < cuentasInfo.Count; i++)
            {
                lsCodigo.Add(cuentasInfo[i].cuenta_act + "-" + cuentasInfo[i].cuenta_ant_vigente + "-" + cuentasInfo[i].cuenta_ant_vencido);
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

    protected void RgdCuentas_CancelCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void RgdCuentas_UpdateCommand(object source, GridCommandEventArgs e)
    {

        Int32 id_convertidor = 0;
        string cuenta_act = string.Empty;
        string cuenta_ant_vigente = string.Empty;
        string cuenta_ant_vencido = string.Empty;
        string cuenta_ant_moratorios = string.Empty;
        string cuenta_capital = string.Empty;

        //MASS 20/06/13
        ConvertidorAct cuentaOld;
        ConvertidorAct cuentaNew;

        Enums.Estado estado = new Enums.Estado();
        bool Actualizar = true;

        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        string sCuentaActOld = oldValues["cuenta_act"].ToString().Replace("'", "");

        try
        {
            if (newValues["cuenta_act"] != null && newValues["cuenta_ant_vigente"] != null && newValues["cuenta_ant_vencido"] != null && newValues["cuenta_ant_moratorios"] != null)
            {
                cuenta_act = newValues["cuenta_act"].ToString().Replace("'", "");
                if (sCuentaActOld != cuenta_act)
                {
                    //campos para efectuar validacion codigo
                    string strCodigo = newValues["cuenta_act"].ToString() + "-" + newValues["cuenta_ant_vigente"].ToString() + "-" + newValues["cuenta_ant_vencido"].ToString();
                    Actualizar = bValidaCodigo(strCodigo.Replace("'", ""));
                }

                if (Actualizar)
                {
                    //Datos Orginales
                    id_convertidor = Parser.ToNumber(newValues["id_convertidor"]);
                    //Datos Nuevos
                    cuenta_ant_vigente = newValues["cuenta_ant_vigente"].ToString();
                    cuenta_ant_vencido = newValues["cuenta_ant_vencido"].ToString();
                    cuenta_ant_moratorios = newValues["cuenta_ant_moratorios"].ToString();
                    cuenta_capital = newValues["cuenta_capital"].ToString();//psl 07 12 2021
                    estado = Enums.Estado.Activo;

                    //MASS 20/06/13
                    cuentaOld = new ConvertidorAct(id_convertidor, cuenta_act, cuenta_ant_vigente, cuenta_ant_vencido, cuenta_ant_moratorios, estado,cuenta_capital); //cuenta_nueva);
                    cuentaNew = new ConvertidorAct(id_convertidor, cuenta_act, cuenta_ant_vigente, cuenta_ant_vencido, cuenta_ant_moratorios, estado,cuenta_capital); //enta_nueva);

                    //Llamar el SP correspondiente con las entidades old y new de Validacion
                    //MASS 20/06/13
                    ConvertidorAct_Rules CuentaValores = new ConvertidorAct_Rules();

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
        //MASS 20/06/13        
        ConvertidorAct CuentaDelete;

        Int32 idCuenta = 0;
        //Datos para identificar valor

        GridDataItem item = (GridDataItem)e.Item;

        try
        {
            idCuenta = Parser.ToNumber(item["id_convertidor"].Text);

            //modif PSL 07/ 12/ 2021
            CuentaDelete = new ConvertidorAct(idCuenta, "", "", "", "", Enums.Estado.Activo,"");

            //Llamar el SP correspondiente con las entidades old y new de Validacion

            //MASS 20/06/13            
            ConvertidorAct_Rules CuentaBorrar = new ConvertidorAct_Rules();

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
        //MASS 20/06/13
        ConvertidorAct_Rules CuentaInsertar;

        try
        {
            //MASS 20/06/13
            ConvertidorAct record = this.ValidaNulos(e.Item as GridEditableItem);
            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion
                if (record.cuenta_act != string.Empty && record.cuenta_ant_vigente != string.Empty && record.cuenta_ant_vencido != string.Empty)
                {
                    //JAGH se agregan campos para efectuar validacion codigo
                    string strCodigo = record.cuenta_act + "-" + record.cuenta_ant_vigente + "-" + record.cuenta_ant_vencido;

                    if (bValidaCodigo(strCodigo))  //(record.Codigo))
                    {
                        //MASS 20/06/13
                        CuentaInsertar = new ConvertidorAct_Rules();
                        int CuentaInsertar_ = CuentaInsertar.Insert(record);
                        if (CuentaInsertar_ > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                        }
                        else if (CuentaInsertar_ == -999)
                        {
                            //this.log.Append(String.Format("Error en la inserción la cuenta actual o cuenta nueva no xistente en el catálogo de cuentas, valores({0})<br/>\n\r", values));
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Error en la inserción la cuenta actual no xistente en el catálogo de cuentas");
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
            string Cadena = "Error en la insercción ID duplicado Nulo o Inexistente, valores ( , , , , , , , , , , )";
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

    //MASS 20/06/13
    private ConvertidorAct ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        ConvertidorAct record;

        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);

        if (newValues.Count > 0)
        {
            if (newValues["cuenta_act"] != null && newValues["cuenta_ant_vigente"] != null && newValues["cuenta_ant_vencido"] != null && newValues["cuenta_ant_moratorios"] != null)
            {
                Enums.Estado estado = new Enums.Estado();
                estado = Enums.Estado.Activo;

                //PSL 07 12 2021
                record = new ConvertidorAct(0, newValues["cuenta_act"].ToString()
                    , newValues["cuenta_ant_vigente"].ToString()
                    , newValues["cuenta_ant_vencido"].ToString()
                    , newValues["cuenta_ant_moratorios"].ToString(), estado
                    , (newValues["cuenta_capital"] == null) ? "" : newValues["cuenta_capital"].ToString());

                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, newValues);
            }
            else
            {
                //PSL 07 12 2021
                record = new ConvertidorAct(0, string.Empty, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo,string.Empty );
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            //MASS 20/06/13
            record = new ConvertidorAct(0, string.Empty, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo, string.Empty);
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
            }
            else if (e.Item.IsInEditMode)
            {
                GridDataItem items;
                items = (GridDataItem)e.Item;
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
                row1["nombreColumna"] = "cuenta actual";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "100";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "cuenta anterior vigente";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "100";

                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "cuenta anterior vencido";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "100";

                DataRow row4;
                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "cuenta anterior moratorios";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "100";

                DataRow row5;
                row5 = dt_metaDataLayout.NewRow();
                row5["nombreColumna"] = "cuenta nueva";
                row5["tipoDato"] = "VARCHAR2";
                row5["longitud"] = "100";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);
                dt_metaDataLayout.Rows.Add(row4);
                dt_metaDataLayout.Rows.Add(row5);

                DbParameter[] parametros;

                parametros = new DbParameter[6]; //regresar a 7
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
                parametros[2].ParameterName = "pCuenta_Act";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 16;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "pCuenta_Ant_Vigente";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 16;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "pCuenta_ant_Vencido";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 16;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "pCuenta_Ant_Moratorios";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 16;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "pCuenta_capital";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 50;

                //MASS 19/06/14
                string storeBase = "PACKCONVERTIDOR_ACT.SP_CARGAMASIVA_CONVERTIDOR_ACT";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_convertidor", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "");
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
                    //MASS 20/06/13
                    ConvertidorAct CuentasDelete;
                    int pId = Parser.ToNumber(row.GetDataKeyValue("id_convertidor").ToString());

                    //PSL 07/12/2021
                    CuentasDelete = new ConvertidorAct(pId, "", "", "", "", Enums.Estado.Activo,"");

                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
                    //CuentasDataAccess CuentasBorrar = new CuentasDataAccess();

                    //MASS 20/06/13
                    ConvertidorAct_Rules CuentasBorrar = new ConvertidorAct_Rules();
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

                        //MASS 20/06/13
                        ConvertidorAct CuentaDelete;
                        Int32 idCuenta = 0;
                        //Datos para identificar valor

                        idCuenta = Parser.ToNumber(row.GetDataKeyValue("id_convertidor").ToString());

                        //PSL 07/12/2021
                        CuentaDelete = new ConvertidorAct(idCuenta, "", "", "", "", Enums.Estado.Activo,"");

                        //Llamar el SP correspondiente con las entidades old y new de Validacion

                        //MASS 20/06/13
                        ConvertidorAct_Rules CuentaBorrar = new ConvertidorAct_Rules();

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
}