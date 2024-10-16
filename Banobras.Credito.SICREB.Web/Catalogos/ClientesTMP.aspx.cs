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

public partial class ClienteTMP : System.Web.UI.Page
{
    //MASS 14/06/13
    public const String catalog = "Clientes Temporales";
    Enums.Persona pPersona;
    string Persona = string.Empty;
    int idUs;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CambiaAtributosRGR();
            Persona = Request.QueryString["Persona"].ToString();
            //MAMR 28/02/13
 
            //if (Persona.Equals("PF"))
            //{
            //    SqlDataSource1.SelectCommand = "select * from temp_clientes_pf";
            //}
            //else
            //{
            //    SqlDataSource1.SelectCommand = "select * from temp_clientes_pm";
            //}

            if (Session["Facultades"] != null)
            {
                getFacultades(Persona);
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");

            }
            if (!this.Page.IsPostBack)
            {
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + " Ingresó a Catálogo " + catalog + " " + (Persona == "PF" ? "Persona física" : "Persona Moral"));                
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
        GridFilterMenu menu = RgdClientes.FilterMenu;
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
    
    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdClientes.Columns[0].Visible = false;
        RgdClientes.Columns[RgdClientes.Columns.Count - 1].Visible = false;
        RgdClientes.MasterTableView.HierarchyDefaultExpanded = true;
        RgdClientes.ExportSettings.OpenInNewWindow = false;
        RgdClientes.ExportSettings.ExportOnlyData = true;

        RgdClientes.MasterTableView.GridLines = GridLines.Both;
        RgdClientes.ExportSettings.IgnorePaging = true;
        RgdClientes.ExportSettings.OpenInNewWindow = true;
        RgdClientes.ExportSettings.Pdf.PageHeight = Unit.Parse("250mm");
        RgdClientes.ExportSettings.Pdf.PageWidth = Unit.Parse("550mm");
        RgdClientes.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdClientes.Columns[0].Visible = false;
        RgdClientes.Columns[RgdClientes.Columns.Count - 1].Visible = false;
        RgdClientes.MasterTableView.HierarchyDefaultExpanded = true;
        RgdClientes.ExportSettings.OpenInNewWindow = false;
        RgdClientes.ExportSettings.ExportOnlyData = true;

        RgdClientes.ExportSettings.IgnorePaging = true;
        RgdClientes.ExportSettings.OpenInNewWindow = true;
        RgdClientes.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }

    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdClientes.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdClientes.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MCCTAS")))
            {
                this.RgdClientes.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ECCTAS")))
            {
                //this.RgdClientes.MasterTableView.GetColumn("DeleteState").Visible = true;
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
                this.RgdClientes.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ECCTAS")))
            {
                //this.RgdClientes.MasterTableView.GetColumn("DeleteState").Visible = true;
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
                string ruta_archivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + file_txt_layout.FileName));
                file_txt_layout.SaveAs(ruta_archivo);
                DataTable dt_layout_procesado;

                DataTable dt_metaDataLayout = new DataTable();
                dt_metaDataLayout.Columns.Add("nombreColumna");
                dt_metaDataLayout.Columns.Add("tipoDato");
                dt_metaDataLayout.Columns.Add("longitud");
                DataRow row1;

                DbParameter parametro;
                List<DbParameter> parametros = new List<DbParameter>();
                parametro = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametro.Direction = ParameterDirection.Output;
                parametro.ParameterName = "id_OUT";
                parametro.DbType = DbType.Int32;
                parametro.Size = 38;
                parametros.Add(parametro);
                parametro = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametro.Direction = ParameterDirection.Output;
                parametro.ParameterName = "tipo_OUT";
                parametro.DbType = DbType.Int32;
                parametro.Size = 38;
                parametros.Add(parametro);
                parametro = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametro.Direction = ParameterDirection.Input;
                parametro.ParameterName = "pPersona";
                parametro.DbType = DbType.String;
                parametro.Size = 5;
                parametros.Add(parametro);
                
                var columns = RgdClientes.MasterTableView.RenderColumns;
                foreach (GridColumn column in columns)
                {
                    if (column.ColumnType != "GridExpandColumn" && column.ColumnType != "GridRowIndicatorColumn" && column.ColumnType != "GridEditCommandColumn" && column.ColumnType != "GridClientSelectColumn")
                    {
                        parametro = OracleBase.DB.DbProviderFactory.CreateParameter();
                        parametro.Direction = ParameterDirection.Input;
                        parametro.ParameterName = "p" + column.UniqueName;
                        if (column.UniqueName.ToUpper() == "FECHA_NAC" || column.UniqueName.ToUpper() == "ESTADO_CIVIL_CLAVE" || column.UniqueName.ToUpper() == "CONSECUTIVO" || column.UniqueName.ToUpper() == "ID_TIPO_CLIENTE")
                            parametro.DbType = DbType.String;
                        else
                            parametro.DbType = GetDBType(column.DataType);

                        parametros.Add(parametro);

                        row1 = dt_metaDataLayout.NewRow();
                        row1["nombreColumna"] = column.UniqueName;
                        row1["tipoDato"] = GetDBType(column.DataType);
                        row1["longitud"] = 0;
                        dt_metaDataLayout.Rows.Add(row1);

                    }
                }

                parametro = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametro.Direction = ParameterDirection.Output;
                parametro.ParameterName = "returnvalue";
                parametro.DbType = DbType.String;
                parametro.Size = 100;
                parametros.Add(parametro);

                string storeBase = "SP_INSERTCTESTMP";

                dt_layout_procesado = cargaMasiva.cargaMasiva("clientes tmp", dt_metaDataLayout, ruta_archivo, " * ", parametros.ToArray(), storeBase, Persona);
                int numeros = cargaMasiva.Correctos;

                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdClientes.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog + " " + Persona, total + " Registros", numeros + " Registros", catalog, 1);

                RgdClientes.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error En la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catch

    }

    protected void RgdClientes_UpdateCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            var columns = ((RadGrid)source).MasterTableView.RenderColumns;
            GridColumn column;

            //Define el stored procedure
            DbCommand cmd = OracleBase.DB.GetStoredProcCommand("SP_UPDATECTESTMP");

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            index = -1;
            if (newValues["RFC"] != null && newValues["RFC"].ToString().Trim() != "")
            {
                OracleBase.DB.AddInParameter(cmd, "pPersona", DbType.String, Persona);
                OracleBase.DB.AddInParameter(cmd, "prfcold", DbType.String, editedItem.SavedOldValues["RFC"]);
                foreach (string key in newValues.Keys)
                {
                    column = columns.First(col => col.UniqueName == key);
                    OracleBase.DB.AddInParameter(cmd, "p" + key, GetDBType(column.DataType), newValues[key]);
                }
                OracleBase.DB.AddOutParameter(cmd, "returnvalue", DbType.String, 100);

                cmd.CommandType = CommandType.StoredProcedure;
                //ejecutas el stored procedure
                OracleBase.DB.ExecuteNonQuery(cmd);
                string returnvalue = (string)OracleBase.DB.GetParameterValue(cmd, "returnvalue");
                RadAjaxManager1.ResponseScripts.Add("radalert('" + returnvalue + "', 320, 100);");
            }
            else
            {
                RadAjaxManager1.ResponseScripts.Add("radalert('El RFC es obligatorio.', 320, 100);");
                index = e.Item.ItemIndex;
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex.Message);
        }

    }

    private DbType GetDBType(System.Type theType)
    {
        System.Data.SqlClient.SqlParameter p1;
        System.ComponentModel.TypeConverter tc;
        p1 = new System.Data.SqlClient.SqlParameter();
        tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.DbType);
        if (tc.CanConvertFrom(theType))
        {
            p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
        }
        else
        {
            //Try brute force
            try
            {
                p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
            }
            catch (Exception)
            {
                //Do Nothing; will return NVarChar as default
            }
        }
        return p1.DbType;
    }

    protected void RgdClientes_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            var columns = ((RadGrid)source).MasterTableView.RenderColumns;
            GridColumn column;
            string value;

            //Define el stored procedure
            DbCommand cmd = OracleBase.DB.GetStoredProcCommand("SP_INSERTCTESTMP");

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            OracleBase.DB.AddInParameter(cmd, "pPersona", DbType.String, Persona);
            foreach (string key in newValues.Keys)
            {
                column = columns.First(col => col.UniqueName == key);
                value = (newValues[key] == null ? null : newValues[key].ToString().ToUpper());
                OracleBase.DB.AddInParameter(cmd, "p" + key, GetDBType(column.DataType), value);
            }
            OracleBase.DB.AddOutParameter(cmd, "returnvalue", DbType.String, 100);
            OracleBase.DB.AddOutParameter(cmd, "id_OUT", DbType.Int32, 10);
            OracleBase.DB.AddOutParameter(cmd, "tipo_OUT", DbType.Int32, 10);

            cmd.CommandType = CommandType.StoredProcedure;
            //ejecutas el stored procedure
            OracleBase.DB.ExecuteNonQuery(cmd);
            string returnvalue = (string)OracleBase.DB.GetParameterValue(cmd, "returnvalue");
            RadAjaxManager1.ResponseScripts.Add("radalert('" + returnvalue + "', 320, 100);");              


        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex.Message);
        }
    }
    protected void RgdClientes_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        RgdClientes.DataSource = datasource();
    }

    private DataTable datasource() 
    {
        DbCommand cmd = OracleBase.DB.GetStoredProcCommand("SP_SELECTCTESTMP");
        OracleBase.DB.AddInParameter(cmd, "pPersona", DbType.String, Persona);
        cmd.CommandType = CommandType.StoredProcedure;
        return OracleBase.DB.ExecuteDataSet(cmd).Tables[0];
    }
    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkCell;
            CheckBox chkHeader;
            chkHeader = (CheckBox)sender;

            foreach (GridDataItem row in RgdClientes.Items)
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

    [System.Web.Services.WebMethod]
    public static string Elimina(bool todo, string persona, string[] rfcs)
    {
        try
        {

            //Define el stored procedure
            DbCommand cmd = OracleBase.DB.GetStoredProcCommand("SP_DELETESTMP");

            if (todo)
            {
                OracleBase.DB.AddInParameter(cmd, "pPersona", DbType.String, persona);
                OracleBase.DB.AddInParameter(cmd, "pAll", DbType.Int32, 1);
                cmd.CommandType = CommandType.StoredProcedure;
                //ejecutas el stored procedure
                OracleBase.DB.ExecuteNonQuery(cmd);
            }
            else
            {
                foreach (string rfc in rfcs)
                {
                    cmd.Parameters.Clear();
                    OracleBase.DB.AddInParameter(cmd, "pPersona", DbType.String, persona);
                    OracleBase.DB.AddInParameter(cmd, "pAll", DbType.Int32, 0);
                    OracleBase.DB.AddInParameter(cmd, "pRFC", DbType.String, rfc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //ejecutas el stored procedure
                    OracleBase.DB.ExecuteNonQuery(cmd);
                }
            }
            return "Los registros se han eliminado correctamente";
        }
        catch (Exception exep)
        {
            return exep.Message;
        }
    }

    int index = -1;
    protected void RgdClientes_PreRender(object sender, EventArgs e)
    {
        if (index > -1)
        {
            RgdClientes.MasterTableView.Items[index].Edit = true;
            RgdClientes.MasterTableView.Rebind();
        }
    }
}