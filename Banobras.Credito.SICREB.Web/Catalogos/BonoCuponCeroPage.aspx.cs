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

public partial class BonoCuponCeroPage : System.Web.UI.Page
{

    int idUsuario;
    string persona;
    public const String catalog = "Bono Cupon Cero";

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


    protected void RgdCuponCero_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BonoCuponCeroRules getRecords = null;
        List<BonoCuponCero> BonoCuponCeroInfo;

        try
        {
            getRecords = new BonoCuponCeroRules(Enums.Persona.Moral);
            var s = getRecords.GetRecords(false);
            BonoCuponCeroInfo = s;
            RgdCuponCero.DataSource = BonoCuponCeroInfo;
            RgdCuponCero.VirtualItemCount = s.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdCuponCero_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void RgdCuponCero_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            // Quitamos los nulos para evitar error en el Hashtable
            if (newValues["Id"] == null) { newValues["Id"] = "0"; }
            if (newValues["Credito"] == null) { newValues["Credito"] = string.Empty; }
            if (newValues["Rfc"] == null) { newValues["Rfc"] = string.Empty; }
            if (newValues["NombreAcreditado"] == null) { newValues["NombreAcreditado"] = string.Empty; }
            if (newValues["MontoInversion"] == null) { newValues["MontoInversion"] = 0; }

            BonoCuponCero CuponCeroNew;
            BonoCuponCeroRules CuponCeroAgregar = new BonoCuponCeroRules(Enums.Persona.Moral);
            Enums.Estado estado = Enums.Estado.Activo;

            if (ValidarDatosCuponCero(newValues) == true)
            {

                CuponCeroNew = new BonoCuponCero(0,
                                                 newValues["Credito"].ToString().ToUpper(),
                                                 newValues["Rfc"].ToString().ToUpper(),
                                                 newValues["NombreAcreditado"].ToString().ToUpper(),
                                                 Parser.ToDouble(newValues["MontoInversion"]),
                                                 estado);

                if (CuponCeroAgregar.Insert(CuponCeroNew) > 0)
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

    protected void RgdCuponCero_UpdateCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            // Quitamos los nulos para evitar error en el Hashtable
            if (oldValues["Credito"] == null) { oldValues["Credito"] = string.Empty; }
            if (oldValues["Rfc"] == null) { oldValues["Rfc"] = string.Empty; }
            if (oldValues["NombreAcreditado"] == null) { oldValues["NombreAcreditado"] = string.Empty; }
            if (oldValues["MontoInversion"] == null) { oldValues["MontoInversion"] = string.Empty; }

            Int32 idCupon = Parser.ToNumber(item.SavedOldValues["Id"]);
            newValues["Id"] = idCupon;

            if (newValues["Credito"] == null) { newValues["Credito"] = string.Empty; }
            if (newValues["Rfc"] == null) { newValues["Rfc"] = string.Empty; }
            if (newValues["NombreAcreditado"] == null) { newValues["NombreAcreditado"] = string.Empty; }
            if (newValues["MontoInversion"] == null) { newValues["MontoInversion"] = string.Empty; }

            BonoCuponCero CuponCeroOld;
            BonoCuponCero CuponCeroNew;
            BonoCuponCeroRules CuponCeroActualizar = new BonoCuponCeroRules(Enums.Persona.Moral);
            Enums.Estado estado = Enums.Estado.Activo;

            if (ValidarDatosCuponCero(newValues) == true)
            {

                CuponCeroOld = new BonoCuponCero(idCupon,
                                                 oldValues["Credito"].ToString().ToUpper(),
                                                 oldValues["Rfc"].ToString().ToUpper(),
                                                 oldValues["NombreAcreditado"].ToString().ToUpper(),
                                                 Parser.ToDouble(oldValues["MontoInversion"]),
                                                 estado);

                CuponCeroNew = new BonoCuponCero(idCupon,
                                                 newValues["Credito"].ToString().ToUpper(),
                                                 newValues["Rfc"].ToString().ToUpper(),
                                                 newValues["NombreAcreditado"].ToString().ToUpper(),
                                                 Parser.ToDouble(newValues["MontoInversion"]),
                                                 estado);

                if (CuponCeroActualizar.Update(CuponCeroOld, CuponCeroNew) > 0)
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


    protected void btnCargar_Click(object sender, EventArgs e)
    {
        cls_cargaArchivos cargaMasiva = new cls_cargaArchivos();

        try
        {
            if (fluArchivo.HasFile)
            {
                DataTable dt_metaDataLayout = new DataTable();
                string ruta_archivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + fluArchivo.FileName));
                fluArchivo.SaveAs(ruta_archivo);

                DataTable dt_layout_procesado = new DataTable();

                dt_metaDataLayout.Columns.Add("nombreColumna");
                dt_metaDataLayout.Columns.Add("tipoDato");
                dt_metaDataLayout.Columns.Add("longitud");

                DataRow row1;
                row1 = dt_metaDataLayout.NewRow();
                row1["nombreColumna"] = "Credito";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "25";

                DataRow row2;
                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "RFC";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "15";

                DataRow row3;
                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Nombre del Acreditado";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "150";

                DataRow row4;
                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "Monto de la Inversion";
                row4["tipoDato"] = "NUMBER";
                row4["longitud"] = "14";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);
                dt_metaDataLayout.Rows.Add(row4);

                DbParameter[] parametros = new DbParameter[7];
                parametros[0] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[0].Direction = ParameterDirection.Output;
                parametros[0].ParameterName = "Id_OUT";
                parametros[0].DbType = DbType.Decimal;
                parametros[0].Size = 38;

                parametros[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[1].Direction = ParameterDirection.Output;
                parametros[1].ParameterName = "Tipo_OUT";
                parametros[1].DbType = DbType.Decimal;
                parametros[1].Size = 38;

                parametros[2] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].ParameterName = "PersonaP";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "CreditoP";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 25;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "RFCP";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 15;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "NombreAcreditadoP";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 150;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "MontoInversionP";
                parametros[6].DbType = DbType.Decimal;
                parametros[6].Size = 14;

                string storeBase = "SP_CARGAMASIVA_CUPONCERO";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_CuponCero", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }

                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdCuponCero.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdCuponCero.Rebind();
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
            GridHeaderItem headerItem = RgdCuponCero.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");

            if (chkHeader.Checked == true)
            {
                int RegistrosEliminados = 0;
                RgdCuponCero.AllowPaging = false;
                RgdCuponCero.Rebind();

                foreach (GridDataItem row in RgdCuponCero.Items)
                {
                    BonoCuponCero CuponCeroDelete;
                    Int32 idCupon = 0;
                    string numCredito = string.Empty;
                    string rfcAcreditado = string.Empty;

                    //Datos para identificar valor

                    idCupon = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    numCredito = row["Credito"].Text.ToString();
                    rfcAcreditado = row["Rfc"].Text.ToString();

                    CuponCeroDelete = new BonoCuponCero(idCupon, numCredito, rfcAcreditado, string.Empty, 0, Enums.Estado.Activo);

                    //Llamar el SP correspondiente con las entidades old y new de validacion
                    BonoCuponCeroRules CuponCeroBorrar = new BonoCuponCeroRules(Enums.Persona.Moral);

                    if (CuponCeroBorrar.Delete(CuponCeroDelete) > 0)
                    {
                        RegistrosEliminados++;
                    }
                }

                if (RgdCuponCero.Items.Count == RegistrosEliminados)
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Registros removidos correctamente");
                }
                else
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), RegistrosEliminados + " registros de " + RgdCuponCero.Items.Count + " removidos correctamente");
                }

                RgdCuponCero.Rebind();
                RgdCuponCero.DataSource = null;
                RgdCuponCero.AllowPaging = true;
                RgdCuponCero.Rebind();
                RgdCuponCero.DataBind();
                idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUsuario, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdCuponCero.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
            }
            else
            {
                int RegistrosEliminados = 0;

                foreach (GridDataItem row in RgdCuponCero.Items)
                {
                    CheckBox chkCell;
                    bool chkResult;
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;

                    if (chkResult)
                    {

                        BonoCuponCero CuponCeroDelete;
                        Int32 idCupon = 0;
                        string numCredito = string.Empty;
                        string rfcAcreditado = string.Empty;

                        //Datos para identificar valor
                        idCupon = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                        numCredito = row["Credito"].Text.ToString();
                        rfcAcreditado = row["Rfc"].Text.ToString();

                        CuponCeroDelete = new BonoCuponCero(idCupon, numCredito, rfcAcreditado, string.Empty, 0, Enums.Estado.Activo);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        BonoCuponCeroRules CuponCeroBorrar = new BonoCuponCeroRules(Enums.Persona.Moral);

                        if (CuponCeroBorrar.Delete(CuponCeroDelete) > 0)
                        {
                            RegistrosEliminados++;
                            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + idCupon, null, null, catalog, 1, null, null);
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
            Response.Write(exep.Message.ToString());
        }

        RgdCuponCero.Rebind();
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdCuponCero.Columns[0].Visible = false;
        RgdCuponCero.Columns[RgdCuponCero.Columns.Count - 1].Visible = false;
        RgdCuponCero.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuponCero.ExportSettings.OpenInNewWindow = false;
        RgdCuponCero.ExportSettings.ExportOnlyData = true;
        RgdCuponCero.MasterTableView.GridLines = GridLines.Both;
        RgdCuponCero.ExportSettings.IgnorePaging = true;
        RgdCuponCero.ExportSettings.OpenInNewWindow = true;
        RgdCuponCero.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdCuponCero.ExportSettings.Pdf.PageWidth = Unit.Parse("450mm");
        RgdCuponCero.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catálogo " + catalog);
    }

    protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdCuponCero.Columns[0].Visible = false;
        RgdCuponCero.Columns[RgdCuponCero.Columns.Count - 1].Visible = false;
        RgdCuponCero.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuponCero.ExportSettings.OpenInNewWindow = false;
        RgdCuponCero.ExportSettings.ExportOnlyData = true;
        RgdCuponCero.ExportSettings.IgnorePaging = true;
        RgdCuponCero.ExportSettings.OpenInNewWindow = true;
        RgdCuponCero.MasterTableView.ExportToExcel();

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

            foreach (GridDataItem row in RgdCuponCero.Items)
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


    private bool facultadInsertar()
    {
        bool valido = false;
        UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_BONOCUPONCERO")))
        {
            valido = true;
        }
        return valido;
    }

    private bool ValidarDatosCuponCero(Hashtable DatosCuponCero)
    {

        if (DatosCuponCero["Credito"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El número de credito no puede ser nulo");
            return false;
        }

        if (DatosCuponCero["Credito"].ToString().Trim() == string.Empty)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de ingresar el número de credito");
            return false;
        }

        if (ValidarCampoNumerico(DatosCuponCero["Credito"].ToString().Trim(), "Credito") == false)
        {
            return false;
        }

        if (DatosCuponCero["Rfc"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El RFC del Acreditado no puede ser nulo");
            return false;
        }

        if (DatosCuponCero["MontoInversion"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Monto de la Inversion no puede ser nulo");
            return false;
        }

        if (ValidarCampoNumerico(DatosCuponCero["MontoInversion"].ToString().Trim(), "Monto de la Inversion") == false)
        {
            return false;
        }

        if (ValidarRegistroDuplicado(DatosCuponCero["Id"].ToString(), DatosCuponCero["Credito"].ToString(), DatosCuponCero["Rfc"].ToString()) == true)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Ya existe un registro con los mismos datos RFC Acreditado: " + DatosCuponCero["Rfc"].ToString() + " y Credito: " + DatosCuponCero["Credito"].ToString());
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

    private bool ValidarRegistroDuplicado(string idRegistro, string NumeroCredito, string RFCAcreditado)
    {
        bool Respuesta = false;

        try
        {
            BonoCuponCeroRules getRecords = new BonoCuponCeroRules(Enums.Persona.Moral);
            List<BonoCuponCero> RegistrosCupones = getRecords.GetRecords(false);

            foreach (BonoCuponCero ItemCuponCero in RegistrosCupones)
            {

                if (RFCAcreditado.ToUpper() == ItemCuponCero.Rfc.ToString().ToUpper() &&
                    NumeroCredito.ToUpper() == ItemCuponCero.Credito.ToString().ToUpper())
                {
                    if (idRegistro == ItemCuponCero.Id.ToString() && idRegistro.Trim() != "0")
                    {
                        // Si el id del registro es diferente de 0 se trata de la actualizacion de un registro
                        // Si el id del Cupon encontrado es igual al del registro que esta siendo editado permitimos
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

        this.RgdCuponCero.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        btnExportarPDF.Visible = false;
        btnExportarExcel.Visible = false;

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_BONOCUPONCERO")))
        {
            this.RgdCuponCero.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_BONOCUPONCERO")))
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
        GridFilterMenu menu = RgdCuponCero.FilterMenu;
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