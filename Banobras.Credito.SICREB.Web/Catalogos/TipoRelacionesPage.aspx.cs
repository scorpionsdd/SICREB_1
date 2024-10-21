using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Telerik.Web.UI;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Business.Repositorios;

public partial class TipoRelacionesPage : System.Web.UI.Page
{

    int idUsuario;
    string persona;
    public const String catalog = "Relaciones";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
            if (Session["Facultades"] != null)
            {
                persona = "PM";
                getFacultades(persona);
                CambiaAtributosRGR();
                if (!this.Page.IsPostBack)
                {
                    ActividadRules.GuardarActividad(4444, idUsuario, "El Usuario " + Session["nombreUser"] + "Ingreso a Catálogo " + catalog);
                }
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario Válido", "~/Login.aspx");
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }


    protected void RgdRelaciones_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        TipoRelacionRules getRecords = null;
        List<TipoRelacion> RelacionesInfo;

        try
        {
            getRecords = new TipoRelacionRules();
            var s = getRecords.GetRecords(false);
            RelacionesInfo = s;
            RgdRelaciones.DataSource = RelacionesInfo;
            RgdRelaciones.VirtualItemCount = s.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdRelaciones_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (item["Ocupada"].Text.ToUpper() == "0")
                { item["OcupadaTemp"].Text = "NO"; }
                else
                { item["OcupadaTemp"].Text = "SI"; }
            }

            if (e.Item.IsInEditMode)
            {
                GridDataItem item = (GridDataItem)e.Item;
                TipoRelacion ObjTipoRelacion = (TipoRelacion)item.DataItem;

                RadComboBox ComboOcupada;
                ComboOcupada = (RadComboBox)item["OcupadaTemp"].FindControl("ComboOcupada");

                RadComboBox comboOperacion;
                comboOperacion = (RadComboBox)item["OcupadaTemp"].FindControl("ComboOcupada");

                if (ObjTipoRelacion.Ocupada == "0")
                { ComboOcupada.SelectedIndex = 0; }
                else
                { ComboOcupada.SelectedIndex = 1; }
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

    protected void RgdRelaciones_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            // Quitamos los nulos para evitar error en el Hashtable
            if (newValues["Id"] == null) { newValues["Id"] = "0"; }
            if (newValues["ClaveRelacion"] == null) { newValues["ClaveRelacion"] = string.Empty; }
            if (newValues["TipoRelaciones"] == null) { newValues["TipoRelaciones"] = string.Empty; }
            if (newValues["Descripcion"] == null) { newValues["Descripcion"] = string.Empty; }
            if (newValues["Ocupada"] == null) { newValues["Ocupada"] = string.Empty; }

            TipoRelacion TipoRelacionNew;
            TipoRelacionRules TipoRelacionAgregar = new TipoRelacionRules();
            Enums.Estado estado = Enums.Estado.Activo;
            ArrayList TipoOcupada = Util.RadComboToString(editedItem["OcupadaTemp"].FindControl("ComboOcupada"));

            if (TipoOcupada[0].ToString().ToUpper() == "SI" || TipoOcupada[0].ToString() == "si")
            {
                newValues["Ocupada"] = "1";
            }
            else
            {
                newValues["Ocupada"] = "0";
            }

            if (ValidarDatosTipoRelacion(newValues) == true)
            {

                TipoRelacionNew = new TipoRelacion(0,
                                    newValues["ClaveRelacion"].ToString(),
                                    newValues["TipoRelaciones"].ToString(),
                                    newValues["Descripcion"].ToString(),
                                    newValues["Ocupada"].ToString(),
                                    estado);

                if (TipoRelacionAgregar.Insert(TipoRelacionNew) > 0)
                {
                    idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                    Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                }
            }
            else
            {
                e.Canceled = true;
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdRelaciones_UpdateCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            // Quitamos los nulos para evitar error en el Hashtable
            if (oldValues["ClaveRelacion"] == null) { oldValues["ClaveRelacion"] = string.Empty; }
            if (oldValues["TipoRelaciones"] == null) { oldValues["TipoRelaciones"] = string.Empty; }
            if (oldValues["Descripcion"] == null) { oldValues["Descripcion"] = string.Empty; }
            if (oldValues["Ocupada"] == null) { oldValues["Ocupada"] = string.Empty; }

            Int32 idTipoRelacion = Parser.ToNumber(item.SavedOldValues["Id"]);
            newValues["Id"] = idTipoRelacion;

            if (newValues["ClaveRelacion"] == null) { newValues["ClaveRelacion"] = string.Empty; }
            if (newValues["TipoRelaciones"] == null) { newValues["TipoRelaciones"] = string.Empty; }
            if (newValues["Descripcion"] == null) { newValues["Descripcion"] = string.Empty; }
            if (newValues["Ocupada"] == null) { newValues["Ocupada"] = string.Empty; }

            TipoRelacion TipoRelacionOld;
            TipoRelacion TipoRelacionNew;
            TipoRelacionRules TipoRelacionActualizar = new TipoRelacionRules();
            Enums.Estado estado = Enums.Estado.Activo;
            ArrayList TipoOcupada = Util.RadComboToString(editedItem["OcupadaTemp"].FindControl("ComboOcupada"));

            if (TipoOcupada[0].ToString().ToUpper() == "SI" || TipoOcupada[0].ToString() == "si")
            {
                newValues["Ocupada"] = "1";
            }
            else
            {
                newValues["Ocupada"] = "0";
            }

            if (ValidarDatosTipoRelacion(newValues) == true)
            {

                TipoRelacionOld = new TipoRelacion(idTipoRelacion,
                                    oldValues["ClaveRelacion"].ToString(),
                                    oldValues["TipoRelaciones"].ToString(),
                                    oldValues["Descripcion"].ToString(),
                                    oldValues["Ocupada"].ToString(),
                                    estado);

                TipoRelacionNew = new TipoRelacion(idTipoRelacion,
                                    newValues["ClaveRelacion"].ToString(),
                                    newValues["TipoRelaciones"].ToString(),
                                    newValues["Descripcion"].ToString(),
                                    newValues["Ocupada"].ToString(),
                                    estado);

                if (TipoRelacionActualizar.Update(TipoRelacionOld, TipoRelacionNew) > 0)
                {
                    idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GActividadCatalogo(8888, idUsuario, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Registro no fue Modificado");
                }
            }
            else
            {
                e.Canceled = true;
            }
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdRelaciones_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        // Se requiere la definicion del metodo, pero no lleva codigo
    }

    protected void RgdRelaciones_DeleteCommand(object source, GridCommandEventArgs e)
    {
        // Se requiere la definicion del metodo, pero no lleva codigo
    }

    protected void RgdRelaciones_ItemCreated(object sender, GridItemEventArgs e)
    {
        // Se requiere la definicion del metodo, pero no lleva codigo
    }

    protected void RgdRelaciones_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        // Se requiere la definicion del metodo, pero no lleva codigo
    }


    protected void btnCargar_Click(object sender, EventArgs e)
    {
        cls_cargaArchivos cargaMasiva = new cls_cargaArchivos();

        try
        {
            if (fluRelaciones.HasFile)
            {
                DataTable dt_metaDataLayout = new DataTable();
                string ruta_archivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + fluRelaciones.FileName));
                fluRelaciones.SaveAs(ruta_archivo);

                DataTable dt_layout_procesado = new DataTable();

                dt_metaDataLayout.Columns.Add("nombreColumna");
                dt_metaDataLayout.Columns.Add("tipoDato");
                dt_metaDataLayout.Columns.Add("longitud");

                DataRow row1;
                row1 = dt_metaDataLayout.NewRow();
                row1["nombreColumna"] = "CLAVE_REL";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "2";

                DataRow row2;
                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "TIPO_REL";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "150";

                DataRow row3;
                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "DESCRIPCION";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "400";

                DataRow row4;
                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "OCUPADA";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "1";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);
                dt_metaDataLayout.Rows.Add(row4);

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
                parametros[2].ParameterName = "personaP";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "ClaveRelacionP";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 2;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "TipoRelacionP";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 150;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "DescripcionP";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 400;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "OcupadaP";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 1;

                string storeBase = "SP_CARGAMASIVA_TIPOSRELACIONES";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_Relaciones", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }

                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdRelaciones.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdRelaciones.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
                                ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
        }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdRelaciones.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");

            if (chkHeader.Checked == true)
            {
                int RegistrosEliminados = 0;
                RgdRelaciones.AllowPaging = false;
                RgdRelaciones.Rebind();

                foreach (GridDataItem row in RgdRelaciones.Items)
                {
                    TipoRelacion TipoRelacionDelete;
                    Int32 idTipoRelacion = 0;
                    string numClaveRelacion = string.Empty;

                    //Datos para identificar valor

                    idTipoRelacion = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    numClaveRelacion = row["ClaveRelacion"].Text.ToString();

                    TipoRelacionDelete = new TipoRelacion(idTipoRelacion, numClaveRelacion, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo);

                    //Llamar el SP correspondiente con la entidad new de validacion
                    TipoRelacionRules TipoRelacionBorrar = new TipoRelacionRules();

                    if (TipoRelacionBorrar.Delete(TipoRelacionDelete) > 0)
                    {
                        RegistrosEliminados++;
                    }
                }

                if (RgdRelaciones.Items.Count == RegistrosEliminados)
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Registros removidos correctamente");
                }
                else
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), RegistrosEliminados + " registros de " + RgdRelaciones.Items.Count + " removidos correctamente");
                }

                RgdRelaciones.Rebind();
                RgdRelaciones.DataSource = null;
                RgdRelaciones.AllowPaging = true;
                RgdRelaciones.Rebind();
                RgdRelaciones.DataBind();
                idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUsuario, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdRelaciones.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
            }
            else
            {
                int RegistrosEliminados = 0;

                foreach (GridDataItem row in RgdRelaciones.Items)
                {
                    CheckBox chkCell;
                    bool chkResult;
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;

                    if (chkResult)
                    {

                        TipoRelacion TipoRelacionDelete;
                        Int32 idTipoRelacion = 0;
                        string numClaveRelacion = string.Empty;

                        //Datos para identificar valor

                        idTipoRelacion = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                        numClaveRelacion = row["ClaveRelacion"].Text.ToString();

                        TipoRelacionDelete = new TipoRelacion(idTipoRelacion, numClaveRelacion, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        TipoRelacionRules TipoRelacionBorrar = new TipoRelacionRules();

                        if (TipoRelacionBorrar.Delete(TipoRelacionDelete) > 0)
                        {
                            RegistrosEliminados++;
                            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + idTipoRelacion, null, null, catalog, 1, null, null);
                        }
                    }
                }

                if (RegistrosEliminados > 0)
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), RegistrosEliminados + " registro(s) removido(s) correctamente");
                }

            }
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }

        RgdRelaciones.Rebind();
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdRelaciones.Columns[0].Visible = false;
        RgdRelaciones.Columns[RgdRelaciones.Columns.Count - 1].Visible = false;
        RgdRelaciones.MasterTableView.HierarchyDefaultExpanded = true;
        RgdRelaciones.ExportSettings.OpenInNewWindow = false;
        RgdRelaciones.ExportSettings.ExportOnlyData = true;
        RgdRelaciones.MasterTableView.GridLines = GridLines.Both;
        RgdRelaciones.ExportSettings.IgnorePaging = true;
        RgdRelaciones.ExportSettings.OpenInNewWindow = true;
        RgdRelaciones.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdRelaciones.ExportSettings.Pdf.PageWidth = Unit.Parse("700mm");
        RgdRelaciones.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catálogo " + catalog);
    }

    protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdRelaciones.Columns[0].Visible = false;
        RgdRelaciones.Columns[RgdRelaciones.Columns.Count - 1].Visible = false;
        RgdRelaciones.MasterTableView.HierarchyDefaultExpanded = true;
        RgdRelaciones.ExportSettings.OpenInNewWindow = false;
        RgdRelaciones.ExportSettings.ExportOnlyData = true;
        RgdRelaciones.ExportSettings.IgnorePaging = true;
        RgdRelaciones.ExportSettings.OpenInNewWindow = true;
        RgdRelaciones.MasterTableView.ExportToExcel();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catálogo " + catalog);
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkCell;
            CheckBox chkHeader;
            chkHeader = (CheckBox)sender;

            foreach (GridDataItem row in RgdRelaciones.Items)
            {
                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkCell.Checked = chkHeader.Checked;

                if (row["Ocupada"].Text.ToUpper() == "0")
                { row["OcupadaTemp"].Text = "NO"; }
                else
                { row["OcupadaTemp"].Text = "SI"; }
            }
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }
    }


    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_TIPORELACION")))
        {
            valido = true;
        }
        return valido;
    }

    private bool ValidarDatosTipoRelacion(Hashtable DatosTipoRelacion)
    {

        if (DatosTipoRelacion["ClaveRelacion"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La clave de relación no puede ser nula");
            return false;
        }

        if (DatosTipoRelacion["ClaveRelacion"].ToString().Trim() == string.Empty)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de ingresar la clave de relación");
            return false;
        }

        if (ValidarCampoNumerico(DatosTipoRelacion["ClaveRelacion"].ToString().Trim(), "Clave de Relación") == false)
        {
            return false;
        }

        if ( ValidarRegistroDuplicado(DatosTipoRelacion["Id"].ToString(), DatosTipoRelacion["ClaveRelacion"].ToString()) )
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Ya existe un registro con la misma clave de relación: " + DatosTipoRelacion["ClaveRelacion"].ToString());
            return false;
        }

        return true;
    }

    private bool ValidarCampoNumerico(string ValorCampo, string NombreCampo)
    {
        bool valido = true;

        for (int n = 0; n < ValorCampo.Length; n++)
        {
            if (!Char.IsNumber(ValorCampo, n))
            {
                valido = false;
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en el campo " + NombreCampo + " deben ser numéricos, favor de verificar.");
                break;
            }
        }

        return valido;
    }

    private bool ValidarRegistroDuplicado(string idRegistro, string ClaveRelacion)
    {
        bool Respuesta = false;

        try
        {
            TipoRelacionRules getRecords = new TipoRelacionRules();
            List<TipoRelacion> RegistrosRelaciones = getRecords.GetRecords(false);

            foreach (TipoRelacion ItemRelacion in RegistrosRelaciones)
            {

                if (ClaveRelacion.ToUpper() == ItemRelacion.ClaveRelacion.ToString().ToUpper())
                {
                    if (idRegistro == ItemRelacion.Id.ToString() && idRegistro.Trim() != "0")
                    {
                        // Si el id del registro es diferente de 0 se trata de la actualizacion de un registro
                        // Si el id del registro encontrado es igual al del registro que esta siendo editado permitimos
                        // guardar el registro por tratarse del mismo; si los id's son diferentes se trata de registros
                        // duplicados por lo tanto no permitimos guardar
                        Respuesta = false;
                        break;
                    }

                    Respuesta = true;
                    break;
                }
            }

            return Respuesta;
        }
        catch
        {
            return Respuesta;
        }
    }

    private void getFacultades(string persona)
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();

        this.RgdRelaciones.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        btnExportarPDF.Visible = false;
        btnExportarExcel.Visible = false;

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_TIPORELACION")))
        {
            this.RgdRelaciones.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_TIPORELACION")))
        {
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDPDF") + "|"))
        {
            btnExportarPDF.Visible = true;
        }

        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("EDEX") + "|"))
        {
            btnExportarExcel.Visible = true;
        }

        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario Válido", "~/Login.aspx");
        }
    }

    private void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdRelaciones.FilterMenu;
        for (int i = menu.Items.Count - 1; i >= 0; i--)
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


}