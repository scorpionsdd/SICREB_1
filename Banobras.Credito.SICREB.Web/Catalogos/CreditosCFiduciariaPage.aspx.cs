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

public partial class CreditosCFiduciariaPage : System.Web.UI.Page
{
    public const String catalog = "Créditos en Contabilidad Fiduciaria";
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

        GridFilterMenu menu = RgdFiduciaria.FilterMenu;
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
    protected void RgdFiduciaria_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        FiduciariaRules getRecords = null;
        List<CreditoFiduciario> credFiducuariaInfo;
        try
        {
            getRecords = new FiduciariaRules();
            credFiducuariaInfo = getRecords.GetRecords(false);
            RgdFiduciaria.DataSource = credFiducuariaInfo;
            RgdFiduciaria.VirtualItemCount = credFiducuariaInfo.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
    }
    protected void RgdFiduciaria_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    protected void RgdFiduciaria_UpdateCommand(object source, GridCommandEventArgs e)
    {

        int IdValue = 0;
        string Credito = string.Empty;
        Enums.Estado estado;
        CreditoFiduciario FiduciariaOld;
        CreditoFiduciario FiduciariaNew;


        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        try
        {

            if (newValues["Credito"] != null)
            {
                if (bValidaCampoVacio(newValues["Credito"].ToString(), "Crédito", e))
                {

                    //Datos Orginales
                    IdValue = Convert.ToInt32(item.SavedOldValues["Id"]);
                    //Datos para actualizar
                    Credito = newValues["Credito"].ToString();
                    estado = Enums.Estado.Activo;
                    FiduciariaOld = new CreditoFiduciario(IdValue, Credito, estado);
                    FiduciariaNew = new CreditoFiduciario(IdValue, Credito, estado);

                    //llamar el SP correspondiente con las entidades old y new
                    FiduciariaRules FiduciariaUpdate = new FiduciariaRules();

                    if (FiduciariaUpdate.Update(FiduciariaOld, FiduciariaNew) > 0)
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
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El campo Crédito debe tener datos, favor de verificar.");

                e.Canceled = true;
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdFiduciaria_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {


    }
    protected void RgdFiduciaria_DeleteCommand(object source, GridCommandEventArgs e)
    {
        CreditoFiduciario FiduciariaDelete;
        int IdFidu = 0;
        string credito = string.Empty;
        GridDataItem item = (GridDataItem)e.Item;
        try
        {
            IdFidu = Convert.ToInt32(item["Id"].Text);
            credito = item["Credito"].Text;
            FiduciariaDelete = new CreditoFiduciario(IdFidu, credito, Enums.Estado.Activo);
            //Aqui se llama el SP correspondiente para eliminar
            FiduciariaRules FiduciariaBorrar = new FiduciariaRules();

            if (FiduciariaBorrar.Delete(FiduciariaDelete) > 0)
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
    protected void RgdFiduciaria_InsertCommand(object source, GridCommandEventArgs e)
    {
        InserCreditoCFiduciaria(e);
    }

    private void InserCreditoCFiduciaria(GridCommandEventArgs e)
    {

        FiduciariaRules FiduciariaInsertar;
        try
        {
            CreditoFiduciario record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {
                if (bValidaCampoVacio(record.Credito,"Credito",e))
                {
                    FiduciariaInsertar = new FiduciariaRules();
                    if (FiduciariaInsertar.Insert(record) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                    }
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de ingresar todos los datos");
                }
            }

        }
        catch (Exception ex)
        {
            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    private CreditoFiduciario ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        CreditoFiduciario record;
        ArrayList estatus;
        Enums.Estado estado = new Enums.Estado();

        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {
            double algo = 0;
            if (newValues["Credito"] != null && double.TryParse(newValues["Credito"].ToString(), out  algo))
            {

                string credito = newValues["Credito"].ToString();
                string RFC = (String)newValues["RFC"];
                string Nombre = (String)newValues["Nombre"];
                estado = Enums.Estado.Activo;

                record = new CreditoFiduciario(0, credito, RFC, Nombre, estado);
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog, 1, null, newValues);

            }
            else
            {
                record = new CreditoFiduciario(0, "", Enums.Estado.Inactivo);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            record = new CreditoFiduciario(0, "", Enums.Estado.Inactivo);
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
        RgdFiduciaria.Columns[0].Visible = false;
        RgdFiduciaria.Columns[RgdFiduciaria.Columns.Count - 1].Visible = false;
        RgdFiduciaria.MasterTableView.HierarchyDefaultExpanded = true;
        RgdFiduciaria.ExportSettings.OpenInNewWindow = false;
        RgdFiduciaria.ExportSettings.ExportOnlyData = true;
        RgdFiduciaria.MasterTableView.GridLines = GridLines.Both;
        RgdFiduciaria.ExportSettings.IgnorePaging = true;
        RgdFiduciaria.ExportSettings.OpenInNewWindow = true;
        RgdFiduciaria.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {


        Response.ContentType = "application/force-download";
        RgdFiduciaria.Columns[0].Visible = false;
        RgdFiduciaria.Columns[RgdFiduciaria.Columns.Count - 1].Visible = false;
        RgdFiduciaria.MasterTableView.HierarchyDefaultExpanded = true;
        RgdFiduciaria.ExportSettings.OpenInNewWindow = false;
        RgdFiduciaria.ExportSettings.ExportOnlyData = true;

        RgdFiduciaria.ExportSettings.IgnorePaging = true;
        RgdFiduciaria.ExportSettings.OpenInNewWindow = true;
        RgdFiduciaria.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

    }
    protected void RgdFiduciaria_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);

        }

    }
    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdFiduciaria.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdFiduciaria.MasterTableView.GetColumn("DeleteState").Visible = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_FIDUCIARIA")))
            {
                this.RgdFiduciaria.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_FIDUCIARIA")))
            {
                //this.RgdFiduciaria.MasterTableView.GetColumn("DeleteState").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
                btnExportPDF.Visible = true;
            if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
                ImageButton1.Visible = true;
        }
        else
        {
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_FIDUCIARIA")))
            {
                this.RgdFiduciaria.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_FIDUCIARIA")))
            {
                //this.RgdFiduciaria.MasterTableView.GetColumn("DeleteState").Visible = true;
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

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_FIDUCIARIA")))
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
            GridHeaderItem headerItem = RgdFiduciaria.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdFiduciaria.AllowPaging = false;
                RgdFiduciaria.Rebind();
                foreach (GridDataItem row in RgdFiduciaria.Items)
                {
                    CreditoFiduciario FiduciariaDelete;
                    int IdFidu = 0;
                    string credito = string.Empty;
                    IdFidu = Parser.ToNumber(row["ID"].Text.ToString());
                    credito = row["Credito"].Text;
                    FiduciariaDelete = new CreditoFiduciario(IdFidu, credito, Enums.Estado.Activo);
                    //Aqui se llama el SP correspondiente para eliminar
                    FiduciariaRules FiduciariaBorrar = new FiduciariaRules();

                    if (FiduciariaBorrar.Delete(FiduciariaDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                        // ActividadRules.GActividadCatalogo(1111, idUs, "Eliminacion de Registro", null, null, catalog, 1,null, null);
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }//foreach
                RgdFiduciaria.Rebind();
                RgdFiduciaria.DataSource = null;
                RgdFiduciaria.AllowPaging = true;
                RgdFiduciaria.Rebind();
                RgdFiduciaria.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdFiduciaria.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdFiduciaria.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {
                        CreditoFiduciario FiduciariaDelete;
                        int IdFidu = 0;
                        string credito = string.Empty;
                        IdFidu = Parser.ToNumber(row["ID"].Text.ToString());
                        credito = row["Credito"].Text;
                        FiduciariaDelete = new CreditoFiduciario(IdFidu, credito, Enums.Estado.Activo);
                        //Aqui se llama el SP correspondiente para eliminar
                        FiduciariaRules FiduciariaBorrar = new FiduciariaRules();

                        if (FiduciariaBorrar.Delete(FiduciariaDelete) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con id " + IdFidu, null, null, catalog, 1, null, null);
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
        RgdFiduciaria.Rebind();


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
                foreach (GridDataItem row in RgdFiduciaria.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdFiduciaria.Items)
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
                row1["nombreColumna"] = "Crédito";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "RFC";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "38";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Nombre";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "38";




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
                parametros[3].ParameterName = "creditop";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "rfcp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 200;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "nombrep";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 30;

                string storeBase = "SP_CM_CFIDUCTARIA";


                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_accionistas", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "PM");
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdFiduciaria.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdFiduciaria.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }//try-Catch
    }
}