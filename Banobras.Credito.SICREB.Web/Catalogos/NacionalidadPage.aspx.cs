using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Common;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Business.Repositorios;
using System.Collections;
using System.Data;
using System.IO;
using System.Data.Common;
using Banobras.Credito.SICREB.Data;
public partial class NacionalidadPage : System.Web.UI.Page
{
    public const String catalog = "Nacionalidad";
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

        GridFilterMenu menu = RgdNacionalidad.FilterMenu;
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
        RgdNacionalidad.Columns[0].Visible = false;
        RgdNacionalidad.Columns[RgdNacionalidad.Columns.Count - 1].Visible = false;
        RgdNacionalidad.MasterTableView.HierarchyDefaultExpanded = true;
        RgdNacionalidad.ExportSettings.OpenInNewWindow = false;
        RgdNacionalidad.ExportSettings.ExportOnlyData = true;
        RgdNacionalidad.MasterTableView.GridLines = GridLines.Both;
        RgdNacionalidad.ExportSettings.IgnorePaging = true;
        RgdNacionalidad.ExportSettings.OpenInNewWindow = true;
        RgdNacionalidad.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
    {

        Response.ContentType = "application/force-download";
        RgdNacionalidad.Columns[0].Visible = false;
        RgdNacionalidad.Columns[RgdNacionalidad.Columns.Count - 1].Visible = false;
        RgdNacionalidad.MasterTableView.HierarchyDefaultExpanded = true;
        RgdNacionalidad.ExportSettings.OpenInNewWindow = false;
        RgdNacionalidad.ExportSettings.ExportOnlyData = true;

        RgdNacionalidad.ExportSettings.IgnorePaging = true;
        RgdNacionalidad.ExportSettings.OpenInNewWindow = true;
        RgdNacionalidad.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdNacionalidad_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
            GridDataItem item = (GridDataItem)e.Item;
            int id = Parser.ToNumber(item["id"].Text);
            NacionalidadRules NacRules = new NacionalidadRules();
            Nacionalidad nac = new Nacionalidad(id, 0, "", "", persona, Enums.Estado.Inactivo);
            if (NacRules.BorrarNacionalidad(nac, persona) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                if (Convert.ToString(persona) == "m")
                {

                }
                else
                {

                }
                Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han eliminado de forma correcta");
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo eliminar los datos");
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdNacionalidad_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

        GridEditableItem objetoGrid = e.Item as GridEditableItem;
        Hashtable nuevosValores = new Hashtable();
        objetoGrid.ExtractValues(nuevosValores);

        ViewState["CLAVESIC"] = nuevosValores["CLAVESIC"].ToString();      
    }

    protected void RgdNacionalidad_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
            GridEditableItem objetoGrid = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            objetoGrid.ExtractValues(nuevosValores);
            if (nuevosValores["CLAVESIC"] != null && nuevosValores["CLAVEBURO"] != null && nuevosValores["DESCRIPCION"] != null && bValidaCampoVacio(nuevosValores["CLAVEBURO"].ToString(), "Clave BURO",e))
            {
                int clavesic = Parser.ToNumber(nuevosValores["CLAVESIC"].ToString());

                if (bValidaClic(clavesic))
                {

                    string claveburo = nuevosValores["CLAVEBURO"].ToString();
                    string descripcion = nuevosValores["DESCRIPCION"].ToString();
                    Nacionalidad nac = new Nacionalidad(0, clavesic, claveburo, descripcion, persona, Enums.Estado.Activo);
                    NacionalidadRules NacRules = new NacionalidadRules();

                    if (NacRules.InsertarNacionalidad(nac, persona) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        
                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han guardado de forma correcta");
                        ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, nuevosValores);

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo guardar los datos");
                    }
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores ingresados no válidos");
                e.Canceled = true;
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }

    }
    protected void RgdNacionalidad_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
            NacionalidadRules nacRules = new NacionalidadRules();
            List<int> lsClic = new List<int>();
            var s = nacRules.GetNacionalidades(persona);
            RgdNacionalidad.DataSource = s;
            RgdNacionalidad.VirtualItemCount = s.Count;

            for (int i = 0; i < s.Count; i++)
            {

                lsClic.Add(s[i].ClaveSIC);
            }

            ViewState["ClaveClic"] = lsClic;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }

    }
    protected void RgdNacionalidad_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable nuevosValores = new Hashtable();
            editedItem.ExtractValues(nuevosValores);
            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            int ClaveSicOriginal = Parser.ToNumber(ViewState["CLAVESIC"].ToString());
            bool bValidacion = true;

            if (nuevosValores["CLAVESIC"] != null && nuevosValores["CLAVEBURO"] != null && nuevosValores["DESCRIPCION"] != null  && bValidaCampoVacio(nuevosValores["CLAVEBURO"].ToString(), "Clave BURO",e))
             {
                              
                int clavesic = Parser.ToNumber(nuevosValores["CLAVESIC"].ToString());

                if (clavesic != ClaveSicOriginal)
                {
                    bValidacion = bValidaClic(clavesic); 
                }

                if (bValidacion)
                {

                    string claveburo = nuevosValores["CLAVEBURO"].ToString();
                    string descripcion = nuevosValores["DESCRIPCION"].ToString();
                    int id = Parser.ToNumber(nuevosValores["ID"].ToString());
                    Nacionalidad Nac = new Nacionalidad(id, clavesic, claveburo, descripcion, persona, Enums.Estado.Activo);
                    NacionalidadRules NacRules = new NacionalidadRules();

                    if (NacRules.ActualizaNacionalidad(Nac, persona) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                     
                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han actualizado de forma correcta");
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, nuevosValores);

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo guardar los datos");
                    }
                }
               
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Valores ingresados no válidos");
                e.Canceled = true;
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

 
    private bool bValidaClic(int iClic)
    {

        bool Valido = true;

        List<int> lsTiposCredito = new List<int>();

        lsTiposCredito = (List<int>)ViewState["ClaveClic"];

        if (lsTiposCredito.FindIndex(s => s == iClic) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Clave CLIC: " + iClic + " ya se encuentra dada de alta.");
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


    protected void RgdNacionalidad_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {

    }
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdNacionalidad.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdNacionalidad.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;
        if (persona == "m")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_NAC_M")))
            {
                this.RgdNacionalidad.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_NAC_M")))
            {
                //this.RgdNacionalidad.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_NAC_F")))
            {
                this.RgdNacionalidad.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_NAC_F")))
            {
                //	this.RgdNacionalidad.MasterTableView.GetColumn("DeleteState").Visible = true;
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

    protected void RgdNacionalidad_ItemDataBound(object sender, GridItemEventArgs e)
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
    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_NAC_F")))
        {
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_NAC_M")))
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
                row1["nombreColumna"] = "Clave CLIC";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Clave Buró";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "100";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Descripción";
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
                parametros[3].ParameterName = "pclavesic";
                parametros[3].DbType = DbType.String;
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


                string storeBase = "SP_cargaMasiva_nacion";


                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_nacionalidad", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;

                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdNacionalidad.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdNacionalidad.Rebind();
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
            GridHeaderItem headerItem = RgdNacionalidad.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdNacionalidad.AllowPaging = false;
                RgdNacionalidad.Rebind();
                foreach (GridDataItem row in RgdNacionalidad.Items)
                {

                    Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);

                    int id = Parser.ToNumber(row["id"].Text);
                    NacionalidadRules NacRules = new NacionalidadRules();
                    Nacionalidad nac = new Nacionalidad(id, 0, "", "", persona, Enums.Estado.Inactivo);
                    if (NacRules.BorrarNacionalidad(nac, persona) > 0)
                    {
                        //int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        if (Convert.ToString(persona) == "m")
                        {

                        }
                        else
                        {

                        }
                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo eliminar los datos");
                    }

                }//foreach
                RgdNacionalidad.Rebind();
                RgdNacionalidad.DataSource = null;
                RgdNacionalidad.AllowPaging = true;
                RgdNacionalidad.Rebind();
                RgdNacionalidad.DataBind();
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdNacionalidad.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdNacionalidad.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {

                        //Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);
                        Enums.Persona persona = Util.GetPersona(Request.QueryString["Persona"].ToCharArray()[0]);

                        int id = Parser.ToNumber(row["id"].Text);
                        NacionalidadRules NacRules = new NacionalidadRules();
                        Nacionalidad nac = new Nacionalidad(id, 0, "", "", persona, Enums.Estado.Inactivo);
                        if (NacRules.BorrarNacionalidad(nac, persona) > 0)
                        {
                            //  idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            if (Convert.ToString(persona) == "m")
                            {

                            }
                            else
                            {

                            }
                            Mensajes.ShowMessage(this.Page, this.GetType(), "Los datos se han eliminado de forma correcta");
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + id, null, null, catalog + " " + persona, 1, null, null);

                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "No se pudo eliminar los datos");
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
        RgdNacionalidad.Rebind();

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
                foreach (GridDataItem row in RgdNacionalidad.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdNacionalidad.Items)
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