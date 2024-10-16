using System;
using System.Collections.Generic;
using System.Web.UI;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Common;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Business.Repositorios;
using System.Collections;
using System.Data;
using System.IO;
using Oracle.DataAccess.Client;
using System.Data.Common;
using Banobras.Credito.SICREB.Data;
using System.Web.UI.WebControls;

public partial class Catalogos_ClavesPaisPage : System.Web.UI.Page
{
    public const String catalog = "Claves de País";
    int idUs;
    string persona;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["Facultades"] != null)
            {
                

                persona = Request.QueryString["Persona"].ToString();
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                    CambiaAtributosRGR();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó al Catálogo " + catalog);
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

        GridFilterMenu menu = RgdClavesPais.FilterMenu;
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
    protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdClavesPais.Columns[0].Visible = false;
        RgdClavesPais.Columns[RgdClavesPais.Columns.Count - 1].Visible = false;
        RgdClavesPais.MasterTableView.HierarchyDefaultExpanded = true;
        RgdClavesPais.ExportSettings.OpenInNewWindow = false;
        RgdClavesPais.ExportSettings.ExportOnlyData = true;
        RgdClavesPais.MasterTableView.GridLines = GridLines.Both;
        RgdClavesPais.ExportSettings.IgnorePaging = true;
        RgdClavesPais.ExportSettings.OpenInNewWindow = true;
        RgdClavesPais.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdClavesPais.Columns[0].Visible = false;
        RgdClavesPais.Columns[RgdClavesPais.Columns.Count - 1].Visible = false;
        RgdClavesPais.MasterTableView.HierarchyDefaultExpanded = true;
        RgdClavesPais.ExportSettings.OpenInNewWindow = false;
        RgdClavesPais.ExportSettings.ExportOnlyData = true;

        RgdClavesPais.ExportSettings.IgnorePaging = true;
        RgdClavesPais.ExportSettings.OpenInNewWindow = true;
        RgdClavesPais.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdClavesPais_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;
            int id = Parser.ToNumber(item["id"].Text);
            PaisRules pr = new PaisRules();
            if (pr.BorrarPais(new Pais(id, 0, "", "", persona, Enums.Estado.Inactivo), persona) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
            }
            else
            {
                Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo eliminar los datos");
            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
            e.Canceled = true;
        }
    }
    protected void RgdClavesPais_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdClavesPais_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
            GridEditableItem objetoGrid = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            objetoGrid.ExtractValues(nuevosValores);

            if (nuevosValores["CLAVESIC"] != null && nuevosValores["CLAVEBURO"] != null && nuevosValores["DESCRIPCION"] != null)
            {

                if (bValidaCampoNumerico(nuevosValores["CLAVESIC"].ToString(), e) && bValidaCampoVacio(nuevosValores["CLAVEBURO"].ToString(), "Clave Buró", e) && bValidaCampoVacio(nuevosValores["DESCRIPCION"].ToString(), "Descripción", e))
                {
                    int clavesic = Parser.ToNumber(nuevosValores["CLAVESIC"].ToString());

                    if (bValidaSic(clavesic) && bValidaClaveCB(nuevosValores["CLAVEBURO"].ToString())) ///, e
                    {
                        
                        string claveburo = nuevosValores["CLAVEBURO"].ToString();
                        string descripcion = nuevosValores["DESCRIPCION"].ToString();
                        Pais p = new Pais(0, clavesic, claveburo, descripcion, persona, Enums.Estado.Activo);
                        PaisRules pr = new PaisRules();
                        if (pr.InsertarPais(p, persona) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han guardado de forma correcta");

                            ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, nuevosValores);

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
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores ingresados no válidos");
                e.Canceled = true;
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
            e.Canceled = true;
        }

    }

    private bool bValidaSic(int iSic)///BMS Comento, Telerik.Web.UI.GridCommandEventArgs e)
    {

        bool Valido = true;

        List<int> lsSic = new List<int>();

        lsSic = (List<int>)ViewState["ClaveSic"];

        if (lsSic.FindIndex(s => s == iSic) >= 0)
        {
            Valido = false;

            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La Clave SIC: " + iSic + " ya se encuentra dada de alta.");

            ///BMS e.Canceled = true;
        }


        return Valido;
    }

    private bool bValidaClaveCB(string sClaveBuro)
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

    public bool bValidaCampoNumerico(string Campo, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //1  Numerico
        bool valido = true;

        for (int n = 0; n < Campo.Length; n++)
        {

            if (!Char.IsNumber(Campo, n))
            {
                valido = false;
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en el campo Clave SIC deben ser numéricos, favor de verificar.");

                e.Canceled = true;

                break;
            }
        }

        return valido;
    }

    public bool bValidaCampoVacio(string Campo, string NombreCampo, Telerik.Web.UI.GridCommandEventArgs e)
    {
       
        int longitud = 0;

        for (int n = 0; n < Campo.Length; n++)
        {

            if (Char.IsWhiteSpace(Campo, n))
            {

                longitud ++;              
               
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



    protected void RgdClavesPais_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
            PaisRules pr = new PaisRules();
            List<int> lsSic = new List<int>();
            List<string> lsClaveBuro = new List<string>();

            var s = pr.Paises(persona);
            RgdClavesPais.DataSource = s;
            RgdClavesPais.VirtualItemCount = s.Count;

            for (int i = 0; i < s.Count; i++)
            {
                lsSic.Add(s[i].ClaveSIC);
                lsClaveBuro.Add(s[i].ClaveBuro);
            }

            ViewState["ClaveSic"] = lsSic;
            ViewState["ClaveBuro"] = lsClaveBuro;

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
        }

    }
    protected void RgdClavesPais_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            ///BMS
            string claveClicOld = string.Empty;
            string claveClic = string.Empty;
            string claveBuroOld = string.Empty;
            string claveBuro = string.Empty;
            bool bvalidaCB = false;
            bool bvalidaCl = false;

            bool Valida = true;
            Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            editedItem.ExtractValues(nuevosValores);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            ///BMS
            claveClic = nuevosValores["CLAVESIC"].ToString();
            claveClicOld = item.SavedOldValues["CLAVESIC"].ToString();
            claveBuro = nuevosValores["CLAVEBURO"].ToString();
            claveBuroOld = item.SavedOldValues["CLAVEBURO"].ToString();

            if (claveClicOld != claveClic)
            {
                if (bValidaSic(Convert.ToInt32(claveClic)))
                {
                    bvalidaCl = true;
                }
            }
            else
                bvalidaCl = true;

            if (claveBuroOld != claveBuro)
            {
                if (bValidaClaveCB(claveBuro))
                {
                    bvalidaCB = true;
                }
            }
            else
                bvalidaCB = true;

            if (bvalidaCl && bvalidaCB)
            {

                if (nuevosValores["CLAVESIC"] != null && nuevosValores["CLAVEBURO"] != null && nuevosValores["DESCRIPCION"] != null && nuevosValores["ID"] != null)
                {
                    if (bValidaCampoNumerico(nuevosValores["CLAVESIC"].ToString(), e) && bValidaCampoVacio(nuevosValores["CLAVEBURO"].ToString(), "Clave Buró", e) && bValidaCampoVacio(nuevosValores["DESCRIPCION"].ToString(), "Descripción", e))
                    {

                        int clavesic = Parser.ToNumber(nuevosValores["CLAVESIC"].ToString());
                        int claveSicOld = Parser.ToNumber(oldValues["CLAVESIC"].ToString());

                        if (clavesic != claveSicOld)
                        {
                            Valida = bValidaSic(clavesic); ///BMS , e
                        }

                        if (Valida)
                        {
                            string claveburo = nuevosValores["CLAVEBURO"].ToString();
                            string descripcion = nuevosValores["DESCRIPCION"].ToString();
                            int id = Parser.ToNumber(nuevosValores["ID"].ToString());
                            Pais p = new Pais(id, clavesic, claveburo, descripcion, persona, Enums.Estado.Activo);
                            PaisRules pr = new PaisRules();
                            if (pr.ActualizaPais(p, persona) > 0)
                            {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                                Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han actualizado de forma correcta");
                                ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, nuevosValores);

                            }
                            else
                            {
                                Mensajes.ShowMessage(Page, this.GetType(), "No se pudo guardar los datos");
                            }
                        }
                    }
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores ingresados no válidos");
                    e.Canceled = true;
                }
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(Page, this.GetType(), ex);
            e.Canceled = true;
        }
    }
    protected void RgdClavesPais_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {

    }
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdClavesPais.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        // this.RgdClavesPais.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "m")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_PA")))
            {
                this.RgdClavesPais.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_PA")))
            {
                //this.RgdClavesPais.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_PA")))
            {
                this.RgdClavesPais.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_PA")))
            {
                // this.RgdClavesPais.MasterTableView.GetColumn("DeleteState").Visible = true;
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_PA")))
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
                row1["nombreColumna"] = "Clave SIC";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Clave Buró";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "2";

                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Descripción";
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
                parametros[3].ParameterName = "pclavesic";
                parametros[3].DbType = DbType.Decimal;
                parametros[3].Size = 5;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "pclaveburo";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 5;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "pdescripcion";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 30;

                string storeBase = "SP_cargaMasiva_Pais";


                dt_layout_procesado = cargaMasiva.cargaMasiva("Pais", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdClavesPais.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdClavesPais.Rebind();
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

    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdClavesPais.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdClavesPais.AllowPaging = false;
                RgdClavesPais.Rebind();
                foreach (GridDataItem row in RgdClavesPais.Items)
                {

                    Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
                    int id = Parser.ToNumber(row.GetDataKeyValue("ID").ToString());
                    PaisRules pr = new PaisRules();

                    if (pr.BorrarPais(new Pais(id, 0, "", "", persona, Enums.Estado.Inactivo), persona) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo eliminar los datos");
                    }

                }//foreach
                RgdClavesPais.Rebind();
                RgdClavesPais.DataSource = null;
                RgdClavesPais.AllowPaging = true;
                RgdClavesPais.Rebind();
                RgdClavesPais.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdClavesPais.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdClavesPais.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {

                        Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
                        int id = Parser.ToNumber(row.GetDataKeyValue("ID").ToString());
                        PaisRules pr = new PaisRules();

                        if (pr.BorrarPais(new Pais(id, 0, "", "", persona, Enums.Estado.Inactivo), persona) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con RFC " + id, null, null, catalog, 1, null, null);

                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(Page, this.GetType(), "No se pudo eliminar los datos");
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
        RgdClavesPais.Rebind();
    }



    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            CheckBox chkHeader;

            chkHeader = (CheckBox)sender;


            if (chkHeader.Checked == true)
            {
                foreach (GridDataItem row in RgdClavesPais.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdClavesPais.Items)
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