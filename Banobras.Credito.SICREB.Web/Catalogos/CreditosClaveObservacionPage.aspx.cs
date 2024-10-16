using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Telerik.Web.UI;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Business.Repositorios;

public partial class CreditosClaveObservacionPage : System.Web.UI.Page
{

    int idUs;
    public const String catalog = "Créditos Con Clave De Observación";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Facultades"] != null)
            {
                string persona = Request.QueryString["Persona"].ToString();
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                    CambiaAtributosRGR();
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + " Ingresó a Catálogo " + catalog);
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
    
    
    protected void RgdCredCveObs_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        CredCveObs_Rules getRecords = null;
        List<CreditoObservacion> CredCveObsInfo;
        try
        {
            getRecords = new CredCveObs_Rules();
            CredCveObsInfo = getRecords.GetRecords(false);
            RgdCredCveObs.DataSource = CredCveObsInfo;
            RgdCredCveObs.VirtualItemCount = CredCveObsInfo.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdCredCveObs_InsertCommand(object source, GridCommandEventArgs e)
    {
        InsertCredCvObs(e);
    }

    protected void RgdCredCveObs_DeleteCommand(object source, GridCommandEventArgs e)
    {
        CreditoObservacion CredCveObsDelete;
        int Id = 0;
        string Credito = string.Empty;
        int IdCredCveObs = 0;

        GridDataItem item = (GridDataItem)e.Item;

        try
        {

            Id = Convert.ToInt32(item["Id"].Text);
            Credito = item["Credito"].Text;
            CredCveObsDelete = new CreditoObservacion(Id, Credito, IdCredCveObs, "0", Enums.Estado.Activo);

            //Aqui se llama el SP correspondiente para eliminar
            CredCveObs_Rules CreditoObsBorrar = new CredCveObs_Rules();

            if (CreditoObsBorrar.Delete(CredCveObsDelete) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminacion de Registro ", null, null, catalog, 1, null, null);
                //Response.Write("<script> alert('El Registro fue removido Correctamente'); </script>");
            }
            else
            {
                //JAGH 21/01/13 se agrega actividad
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(800, idUs, "El registro no fue eliminado " + catalog);

                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                //Response.Write("<script> alert('El Registro no fue Removido'); </script>");
            }
        }
        catch (Exception ex)
        {
            //JAGH 21/01/13 se agrega actividad
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue eliminado " + catalog);

            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }

    }

    protected void RgdCredCveObs_UpdateCommand(object source, GridCommandEventArgs e)
    {
        int IdValue = 0;
        string Credito = string.Empty;
        ArrayList IdCveObs;
        //ArrayList estatus;

        Enums.Estado estado;
        CreditoObservacion CredObsOld;
        CreditoObservacion CredObsNew;

        try
        {
            this.RgdCredCveObs.MasterTableView.GetColumn("IdCvesObservacion").Visible = false;
            this.RgdCredCveObs.MasterTableView.GetColumn("CveObservacion").Visible = true;
            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            //Datos Orginales
            IdValue = Convert.ToInt32(item.SavedOldValues["Id"]);
            //Datos para actualizar
            Credito = newValues["Credito"].ToString();
            //estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));
            IdCveObs = Util.RadComboToString(item["CveObservacion"].FindControl("ComboObservacion"));


            //if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else if (estatus[0].ToString() == "Inactivo") { estado = Enums.Estado.Inactivo; } else { estado = Enums.Estado.Test; }
            estado = Enums.Estado.Activo;

            CredObsOld = new CreditoObservacion(IdValue, Credito, Parser.ToNumber(IdCveObs[1]), "0", estado);
            CredObsNew = new CreditoObservacion(IdValue, Credito, Parser.ToNumber(IdCveObs[1]), "0", estado);

            //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
            CredCveObs_Rules CredCveObsUpdate = new CredCveObs_Rules();

            if (CredCveObsUpdate.Update(CredObsOld, CredObsNew) > 0)
            {
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro ", null, null, catalog, 1, oldValues, newValues);

            }
            else
            {
                //JAGH 21/01/13 se agrega actividad
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(800, idUs, "El registro no fue actualizado " + catalog);

                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                //Response.Write("<script> alert('El Registro no fue Modificado'); </script>");
            }
        }
        catch (Exception ex)
        {
            //JAGH 21/01/13 se agrega actividad
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue actualizado " + catalog);
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
         
    protected void RgdCredCveObs_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {

            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                item["EstatusTemp"].Text = item["Estatus"].Text;
                item["CveObservacion"].Text = item["IdCvesObservacion"].Text;
            }
            else if (e.Item.IsInEditMode)
            {

                GridDataItem item;
                RadComboBox comboEstatus;
                ClavesObservacion_Rules catalogs;
                RadComboBox combo;
                this.RgdCredCveObs.MasterTableView.GetColumn("IdCvesObservacion").Visible = false;
                this.RgdCredCveObs.MasterTableView.GetColumn("CveObservacion").Visible = true;
                item = (GridDataItem)e.Item;
                comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);

                catalogs = new ClavesObservacion_Rules(Enums.Persona.Moral);
                combo = (RadComboBox)item["CveObservacion"].FindControl("ComboObservacion");
                combo.DataTextField = "Clave";
                combo.DataValueField = "Id";
                combo.DataSource = catalogs.GetRecords(false);
                combo.DataBind();

                if (newValues["CveExterna"] != null)
                {
                    combo.SelectedItem.Text = newValues["CveExterna"].ToString();


                }
                if (e.Item is GridCommandItem)
                {
                    Button addButton = e.Item.FindControl("AddNewRecordButton") as Button;
                    LinkButton lnkButton = (LinkButton)e.Item.FindControl("InitInsertButton");
                    if (facultadInsertar())
                    {

                    }
                    else
                    {

                    }
                }


            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);

        }

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
                row2["longitud"] = "200";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Nombre";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "200";
                DataRow row4;

                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "Clave de Observación";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "200";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);
                dt_metaDataLayout.Rows.Add(row4);


                //List<DbParameter> parametros = new List<DbParameter>();

                DbParameter[] parametros = new DbParameter[7];


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
                parametros[2].ParameterName = "personaApp";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;


                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "pCREDITO";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 5;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "pRFC";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 5;


                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "pNombre";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 5;


                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "pID_CLAVES_OBSERVACION";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 200;

                string storeBase = "SP_CM_CREDCOBS";

                string persona = Request.QueryString["Persona"].ToString();
                dt_layout_procesado = cargaMasiva.cargaMasiva("RadGridExport 2", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'));
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente <br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga. <br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores </a>" : "")));

                int total = RgdCredCveObs.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros ", numeros + " Registros ", catalog, 1);

                RgdCredCveObs.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'));
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo <br/> {0}</b>",
            ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados. <br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores </a>" : "")));
        }//try-Catch
        //
    }
    
    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;
            bool chkResult;
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdCredCveObs.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdCredCveObs.AllowPaging = false;
                RgdCredCveObs.Rebind();
                foreach (GridDataItem row in RgdCredCveObs.Items)
                {
                    CreditoObservacion CredCveObsDelete;
                    int Id = 0;
                    string Credito = string.Empty;
                    int IdCredCveObs = 0;
                    Id = Convert.ToInt32(row["Id"].Text);
                    Credito = row["Credito"].Text;
                    CredCveObsDelete = new CreditoObservacion(Id, Credito, IdCredCveObs, "0", Enums.Estado.Activo);

                    //Aqui se llama el SP correspondiente para eliminar
                    CredCveObs_Rules CreditoObsBorrar = new CredCveObs_Rules();

                    if (CreditoObsBorrar.Delete(CredCveObsDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");

                        //Response.Write("<script> alert('El Registro fue removido Correctamente'); </script>");
                    }
                    else
                    {
                        //JAGH 21/01/13 se agrega actividad
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        ActividadRules.GuardarActividad(800, idUs, "El registro no fue eliminado " + catalog);

                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                        //Response.Write("<script> alert('El Registro no fue Removido'); </script>");
                    }

                }//foreach
                RgdCredCveObs.Rebind();
                RgdCredCveObs.DataSource = null;
                RgdCredCveObs.AllowPaging = true;
                RgdCredCveObs.Rebind();
                RgdCredCveObs.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdCredCveObs.VirtualItemCount) + " Registros ", " 0 registros ", catalog, 1, null, null);

            }//header
            else
            {
                foreach (GridDataItem row in RgdCredCveObs.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;


                    if (chkResult)
                    {
                        CreditoObservacion CredCveObsDelete;
                        int Id = 0;
                        string Credito = string.Empty;
                        int IdCredCveObs = 0;
                        Id = Convert.ToInt32(row["Id"].Text);
                        Credito = row["Credito"].Text;
                        CredCveObsDelete = new CreditoObservacion(Id, Credito, IdCredCveObs, "0", Enums.Estado.Activo);

                        //Aqui se llama el SP correspondiente para eliminar
                        CredCveObs_Rules CreditoObsBorrar = new CredCveObs_Rules();

                        if (CreditoObsBorrar.Delete(CredCveObsDelete) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + Id, null, null, catalog, 1, null, null);
                            //Response.Write("<script> alert('El Registro fue removido Correctamente'); </script>");

                        }
                        else
                        {
                            //JAGH 21/01/13 se agrega actividad
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(800, idUs, "El registro " + Id + " no fue eliminado " + catalog);

                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                            //Response.Write("<script> alert('El Registro no fue Removido'); </script>");
                        }

                    }//checket

                }//FOREACH
            }
        }
        catch (Exception exep)
        {
            //JAGH 21/01/13 se agrega actividad
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "El registro no fue eliminado " + catalog);

            Response.Write(exep.Message.ToString());
        }//try-catch
        //btn_eliminar_Click
        RgdCredCveObs.Rebind();

    }

    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdCredCveObs.Columns[0].Visible = false;
        RgdCredCveObs.Columns[RgdCredCveObs.Columns.Count - 1].Visible = false;
        RgdCredCveObs.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCredCveObs.ExportSettings.OpenInNewWindow = false;
        RgdCredCveObs.ExportSettings.ExportOnlyData = true;
        RgdCredCveObs.MasterTableView.GridLines = GridLines.Both;
        RgdCredCveObs.ExportSettings.IgnorePaging = true;
        RgdCredCveObs.ExportSettings.OpenInNewWindow = true;
        RgdCredCveObs.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el Catálogo " + catalog);
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        RgdCredCveObs.Columns[0].Visible = false;
        RgdCredCveObs.Columns[RgdCredCveObs.Columns.Count - 1].Visible = false;
        RgdCredCveObs.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCredCveObs.ExportSettings.OpenInNewWindow = false;
        RgdCredCveObs.ExportSettings.ExportOnlyData = true;

        RgdCredCveObs.ExportSettings.IgnorePaging = true;
        RgdCredCveObs.ExportSettings.OpenInNewWindow = true;
        RgdCredCveObs.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + " Exportó el Catálogo " + catalog);
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
                foreach (GridDataItem row in RgdCredCveObs.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdCredCveObs.Items)
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


    private void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdCredCveObs.FilterMenu;
        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (menu.Items[i].Text == "NoFilter") { menu.Items[i].Text = "Sin Filtro"; }
            if (menu.Items[i].Text == "EqualTo") { menu.Items[i].Text = "Igual"; }
            if (menu.Items[i].Text == "NotEqualTo") { menu.Items[i].Text = "Diferente"; }
            if (menu.Items[i].Text == "GreaterThan") { menu.Items[i].Text = "Mayor que"; }
            if (menu.Items[i].Text == "LessThan") { menu.Items[i].Text = "Menor que"; }
            if (menu.Items[i].Text == "GreaterThanOrEqualTo") { menu.Items[i].Text = "Mayor o igual a"; }
            if (menu.Items[i].Text == "LessThanOrEqualTo") { menu.Items[i].Text = "Menor o igual a"; }
            if (menu.Items[i].Text == "Between") { menu.Items[i].Text = "Entre"; }
            if (menu.Items[i].Text == "NotBetween") { menu.Items[i].Text = "No entre"; }
            if (menu.Items[i].Text == "IsNull") { menu.Items[i].Text = "Es nulo"; }
            if (menu.Items[i].Text == "NotIsNull") { menu.Items[i].Text = "No es nulo"; }
            if (menu.Items[i].Text == "Contains") { menu.Items[i].Text = "Contenga"; }
            if (menu.Items[i].Text == "DoesNotContain") { menu.Items[i].Text = "No Contenga"; }
            if (menu.Items[i].Text == "StartsWith") { menu.Items[i].Text = "Inicie con"; }
            if (menu.Items[i].Text == "EndsWith") { menu.Items[i].Text = "Finalice con"; }
            if (menu.Items[i].Text == "IsEmpty") { menu.Items[i].Visible = false; }
            if (menu.Items[i].Text == "NotIsEmpty") { menu.Items[i].Visible = false; }
        }
    }

    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdCredCveObs.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        if (persona == "PM")
        {

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CREDOBS_M")))
            {
                this.RgdCredCveObs.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CREDOBS_M")))
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
            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CREDOBS_F")))
            {
                this.RgdCredCveObs.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
                valido = true;
            }

            if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CREDOBS_F")))
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
    
    private void InsertCredCvObs(GridCommandEventArgs e)
    {

        CredCveObs_Rules CredCveObsInsertar;
        try
        {
            CreditoObservacion record = this.ValidaNulos(e.Item as GridEditableItem);

            if (e.Item is GridDataInsertItem)
            {
                if (record.Id != 0 || record.IdCvesObservacion != 0)
                {
                    CredCveObsInsertar = new CredCveObs_Rules();

                    //TMR. Válida q no exista ese registro
                    List<CreditoObservacion> listaRegistros = CredCveObsInsertar.GetRecords(false);
                    var odato = from item in listaRegistros
                                where item.Id.Equals(record.Id)
                                select item;
                    bool bExiste = false;
                    foreach (CreditoObservacion oCredito in odato)
                    {
                        bExiste = true;
                    }

                    if (bExiste)
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro que intenta dar de alta ya existe.");
                    }
                    else
                    {
                        if (CredCveObsInsertar.Insert(record) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                        }
                        else
                        {
                            //JAGH 21/01/13 se agrega actividad
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(800, idUs, "El registro no fue agregado " + catalog);

                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //JAGH 21/01/13 se agrega actividad
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(800, idUs, "Error el registro no fue agregado " + catalog);

            e.Canceled = true;
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    
    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CREDOBS_F")))
        {
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CREDOBS_M")))
        {
            valido = true;
        }
        return valido;
    }

    private CreditoObservacion ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        CreditoObservacion record;
        ArrayList clave;
        // Extrae todos los elementos

        clave = Util.RadComboToString(editedItem["CveObservacion"].FindControl("ComboObservacion"));
        editedItem.ExtractValues(newValues);
        if (newValues.Count > 0)
        {

            if (newValues["Credito"] != null && Parser.ToNumber(clave[1]) != 0)
            {
                Enums.Estado estado = Enums.Estado.Activo;


                record = new CreditoObservacion(0, newValues["Credito"].ToString(), Parser.ToNumber(clave[1]), "0", estado);
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro ", null, null, catalog, 1, null, newValues);

            }
            else
            {
                record = new CreditoObservacion(0, "0", 0, "0", Enums.Estado.Activo);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
                //Response.Write("<script> alert('Favor de Verificar los Datos Ingresados')</script>");
            }
        }
        else
        {
            record = new CreditoObservacion(0, "0", 0, "0", Enums.Estado.Activo);
        }
        return record;
    }

}
