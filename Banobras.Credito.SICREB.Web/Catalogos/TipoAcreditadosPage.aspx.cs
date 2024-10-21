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

public partial class TipoAcreditadosPage : System.Web.UI.Page
{

    int idUsuario;
    string persona;
    public const String catalog = "Acreditados";

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


    protected void RgdAcreditados_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        TipoAcreditado_Rules getRecords = null;
        List<TipoAcreditado> TipoAcreditadoInfo;

        try
        {
            getRecords = new TipoAcreditado_Rules(Enums.Persona.Moral);
            var s = getRecords.GetRecords(false);
            TipoAcreditadoInfo = s;
            RgdAcreditados.DataSource = TipoAcreditadoInfo;
            RgdAcreditados.VirtualItemCount = s.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdAcreditados_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                item["TipoPersonaTemp"].Text = item["Persona"].Text;
            }
            
            if (e.Item.IsInEditMode)
            {
                GridDataItem item = (GridDataItem)e.Item;
                TipoAcreditado ObjTipoAcreditado = (TipoAcreditado)item.DataItem;

                RadComboBox comboPersona;
                comboPersona = (RadComboBox)item["TipoPersonaTemp"].FindControl("ComboPersona");

                if (ObjTipoAcreditado.Tipo_Acreditado == Enums.Persona.Moral )
                { comboPersona.SelectedIndex = 0; }
                if (ObjTipoAcreditado.Tipo_Acreditado == Enums.Persona.Fisica)
                { comboPersona.SelectedIndex = 1; }
                if (ObjTipoAcreditado.Tipo_Acreditado == Enums.Persona.Fideicomiso)
                { comboPersona.SelectedIndex = 2; }
                if (ObjTipoAcreditado.Tipo_Acreditado == Enums.Persona.Gobierno)
                { comboPersona.SelectedIndex = 3; } 
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

    protected void RgdAcreditados_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            // Quitamos los nulos para evitar error en el Hashtable
            if (newValues["Id"] == null) { newValues["Id"] = "0"; }
            if (newValues["RfcAcreditado"] == null) { newValues["RfcAcreditado"] = string.Empty; }            
            if (newValues["NombreAcreditado"] == null) { newValues["NombreAcreditado"] = string.Empty; }            
            if (newValues["Tipo_Acreditado"] == null) { newValues["Tipo_Acreditado"] = string.Empty; }

            TipoAcreditado_Rules TipoAcreditadoAgregar = new TipoAcreditado_Rules(Enums.Persona.Moral);
            Enums.Estado estatus = Enums.Estado.Activo;
            Enums.Persona persona = Enums.Persona.Moral;
            ArrayList TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));

            switch (TipoPersona[0].ToString().ToUpper().Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U'))
            {
                case "MORAL":
                    persona = Enums.Persona.Moral;
                    newValues["Tipo_Acreditado"] = "MORAL";
                    break;
                case "FISICA":
                    persona = Enums.Persona.Fisica;
                    newValues["Tipo_Acreditado"] = "FISICA";
                    break;
                case "FONDO O FIDEICOMISO":
                    persona = Enums.Persona.Fideicomiso;
                    newValues["Tipo_Acreditado"] = "FONDO O FIDEICOMISO";
                    break;
                case "GOBIERNO":
                    persona = Enums.Persona.Gobierno;
                    newValues["Tipo_Acreditado"] = "GOBIERNO";
                    break;
                default:
                    persona = Enums.Persona.Moral;
                    newValues["Tipo_Acreditado"] = "MORAL";
                    break;
            }

            if (ValidarDatosTipoAcreditado(newValues) == true)
            {

                TipoAcreditado TipoAcreditadoNew = new TipoAcreditado(0,
                                                                       newValues["RfcAcreditado"].ToString().ToUpper(),
                                                                       newValues["NombreAcreditado"].ToString().ToUpper(),
                                                                       persona,
                                                                       estatus);

                if (TipoAcreditadoAgregar.Insert(TipoAcreditadoNew) > 0)
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

    protected void RgdAcreditados_UpdateCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            // Quitamos los nulos para evitar error en el Hashtable
            if (oldValues["RfcAcreditado"] == null) { oldValues["RfcAcreditado"] = string.Empty; }           
            if (oldValues["NombreAcreditado"] == null) { oldValues["NombreAcreditado"] = string.Empty; }            
            if (oldValues["Tipo_Acreditado"] == null) { oldValues["Tipo_Acreditado"] = string.Empty; }
        
            Int32 idTipoAcreditado = Parser.ToNumber(item.SavedOldValues["Id"]);
            newValues["Id"] = idTipoAcreditado;

            if (newValues["RfcAcreditado"] == null) { newValues["RfcAcreditado"] = string.Empty; }            
            if (newValues["NombreAcreditado"] == null) { newValues["NombreAcreditado"] = string.Empty; }
            if (newValues["Tipo_Acreditado"] == null) { newValues["Tipo_Acreditado"] = string.Empty; }

            TipoAcreditado TipoAcreditadoOld;
            TipoAcreditado TipoAcreditadoNew;
            TipoAcreditado_Rules TipoAcreditadoActualizar = new TipoAcreditado_Rules(Enums.Persona.Moral);
            Enums.Estado estado = Enums.Estado.Activo;
            Enums.Persona persona = Enums.Persona.Moral;            
            ArrayList TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));

            switch (TipoPersona[0].ToString().ToUpper().Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U'))
            {
                case "MORAL":
                    persona = Enums.Persona.Moral;
                    newValues["Tipo_Acreditado"] = "MORAL";
                    break;
                case "FISICA":
                    persona = Enums.Persona.Fisica;
                    newValues["Tipo_Acreditado"] = "FISICA";
                    break;
                case "FONDO O FIDEICOMISO":
                    persona = Enums.Persona.Fideicomiso;
                    newValues["Tipo_Acreditado"] = "FONDO O FIDEICOMISO";
                    break;
                case "GOBIERNO":
                    persona = Enums.Persona.Gobierno;
                    newValues["Tipo_Acreditado"] = "GOBIERNO";
                    break;
                default:
                    persona = Enums.Persona.Moral;
                    newValues["Tipo_Acreditado"] = "MORAL";
                    break;
            }

            if (ValidarDatosTipoAcreditado(newValues) == true)
            {

                TipoAcreditadoOld = new TipoAcreditado(idTipoAcreditado,                                    
                                    oldValues["RfcAcreditado"].ToString().ToUpper(),                                    
                                    oldValues["NombreAcreditado"].ToString().ToUpper(),                                    
                                    persona,
                                    estado);

                TipoAcreditadoNew = new TipoAcreditado(idTipoAcreditado,                                    
                                    newValues["RfcAcreditado"].ToString().ToUpper(),                                  
                                    newValues["NombreAcreditado"].ToString().ToUpper(),                                    
                                    persona,                                    
                                    estado);

                if (TipoAcreditadoActualizar.Update(TipoAcreditadoOld, TipoAcreditadoNew) > 0)
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
                row1["nombreColumna"] = "RFC Acreditado";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "15";

                DataRow row2;
                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Nombre Acreditado";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "150";

                DataRow row3;
                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Tipo Acreditado";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "20";

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
                parametros[3].ParameterName = "rfcAcreditadoP";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 15;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "NombreAcreditadoP";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 150;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "TipoAcreditadoP";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 20;

                string storeBase = "SP_CARGAMASIVA_TIPOACREDITADO";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_acreditados", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }

                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdAcreditados.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdAcreditados.Rebind();
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
            GridHeaderItem headerItem = RgdAcreditados.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");

            if (chkHeader.Checked == true)
            {
                int RegistrosEliminados = 0;
                RgdAcreditados.AllowPaging = false;
                RgdAcreditados.Rebind();

                foreach (GridDataItem row in RgdAcreditados.Items)
                {
                    TipoAcreditado TipoAcreditadoDelete;
                    Int32 idTipoAcreditado = 0;
                    string rfcAcreditado = string.Empty;

                    //Datos para identificar valor

                    idTipoAcreditado = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    rfcAcreditado = row["RfcAcreditado"].Text.ToString();

                    TipoAcreditadoDelete = new TipoAcreditado(idTipoAcreditado, rfcAcreditado, string.Empty, Enums.Persona.Moral, Enums.Estado.Activo);

                    //Llamar el SP correspondiente con las entidades old y new de validacion
                    TipoAcreditado_Rules TipoAcreditadoBorrar = new TipoAcreditado_Rules(Enums.Persona.Moral);

                    if (TipoAcreditadoBorrar.Delete(TipoAcreditadoDelete) > 0)
                    {
                        RegistrosEliminados++;  
                    }
                }

                if (RgdAcreditados.Items.Count == RegistrosEliminados) 
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Registros removidos correctamente");
                }
                else
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), RegistrosEliminados + " registros de " + RgdAcreditados.Items.Count + " removidos correctamente");
                }

                RgdAcreditados.Rebind();
                RgdAcreditados.DataSource = null;
                RgdAcreditados.AllowPaging = true;
                RgdAcreditados.Rebind();
                RgdAcreditados.DataBind();
                idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUsuario, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdAcreditados.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
            }
            else
            {
                int RegistrosEliminados = 0;

                foreach (GridDataItem row in RgdAcreditados.Items)
                {
                    CheckBox chkCell;
                    bool chkResult;
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;

                    if (chkResult)
                    {

                        TipoAcreditado TipoAcreditadoDelete;
                        Int32 idTipoAcreditado = 0;
                        string rfcAcreditado = string.Empty;

                        //Datos para identificar valor

                        idTipoAcreditado = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                        rfcAcreditado = row["RfcAcreditado"].Text.ToString();

                        TipoAcreditadoDelete = new TipoAcreditado(idTipoAcreditado, rfcAcreditado, string.Empty, Enums.Persona.Moral, Enums.Estado.Activo);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        TipoAcreditado_Rules TipoAcreditadoBorrar = new TipoAcreditado_Rules(Enums.Persona.Moral);

                        if (TipoAcreditadoBorrar.Delete(TipoAcreditadoDelete) > 0)
                        {
                            RegistrosEliminados++;
                            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + idTipoAcreditado, null, null, catalog, 1, null, null);
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

        RgdAcreditados.Rebind();
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdAcreditados.Columns[0].Visible = false;
        RgdAcreditados.Columns[RgdAcreditados.Columns.Count - 1].Visible = false;
        RgdAcreditados.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAcreditados.ExportSettings.OpenInNewWindow = false;
        RgdAcreditados.ExportSettings.ExportOnlyData = true;
        RgdAcreditados.MasterTableView.GridLines = GridLines.Both;
        RgdAcreditados.ExportSettings.IgnorePaging = true;
        RgdAcreditados.ExportSettings.OpenInNewWindow = true;
        RgdAcreditados.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdAcreditados.ExportSettings.Pdf.PageWidth = Unit.Parse("985mm");
        RgdAcreditados.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catálogo " + catalog);
    }

    protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdAcreditados.Columns[0].Visible = false;
        RgdAcreditados.Columns[RgdAcreditados.Columns.Count - 1].Visible = false;
        RgdAcreditados.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAcreditados.ExportSettings.OpenInNewWindow = false;
        RgdAcreditados.ExportSettings.ExportOnlyData = true;
        RgdAcreditados.ExportSettings.IgnorePaging = true;
        RgdAcreditados.ExportSettings.OpenInNewWindow = true;
        RgdAcreditados.MasterTableView.ExportToExcel();

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

            foreach (GridDataItem row in RgdAcreditados.Items)
            {
                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkCell.Checked = chkHeader.Checked;

                row["TipoPersonaTemp"].Text = row["Persona"].Text;
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_TIPOACREDITADO")))
        {
            valido = true;
        }
        return valido;
    }

    private bool ValidarDatosTipoAcreditado(Hashtable DatosTipoAcreditado)
    {

        // Solo se contemplan las validaciones mas basicas de los datos del catalogo.
        // Las validaciones generales se realizan a la hora de crear el Segmento AV al procesar la informacion de Personas Morales.

        if (DatosTipoAcreditado["RfcAcreditado"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El RFC del acreditado no puede ser nulo");
            return false;
        }

        if (ValidarRegistroDuplicado(DatosTipoAcreditado["Id"].ToString(), DatosTipoAcreditado["RfcAcreditado"].ToString()) == true)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Ya existe un registro con el mismo RFC Acreditado: " + DatosTipoAcreditado["RfcAcreditado"].ToString() );
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

    private bool ValidarRegistroDuplicado(string idRegistro, string RegistroAcreditado)
    {
        bool Respuesta = false;

        try
        {
            TipoAcreditado_Rules getRecords = new TipoAcreditado_Rules(Enums.Persona.Moral);
            List<TipoAcreditado> RegistrosAcreditados = getRecords.GetRecords(false);

            foreach (TipoAcreditado ItemAcreditado in RegistrosAcreditados)
            {

                if (RegistroAcreditado.ToUpper() == ItemAcreditado.RfcAcreditado.ToString().ToUpper() )
                {
                    if (idRegistro == ItemAcreditado.Id.ToString() && idRegistro.Trim() != "0")
                    {
                        // Si el id del registro es diferente de 0 se trata de la actualizacion de un registro
                        // Si el id del Acreditado encontrado es igual al del registro que esta siendo editado permitimos
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

        this.RgdAcreditados.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        btnExportarPDF.Visible = false;
        btnExportarExcel.Visible = false;

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_ACREDITADO")))
        {
            this.RgdAcreditados.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_ACREDITADO")))
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
        GridFilterMenu menu = RgdAcreditados.FilterMenu;
        for (int i = menu.Items.Count - 1; i >= 0; i--)
        {
            if (menu.Items[i].Text == "NoFilter"){ menu.Items[i].Text = "Sin Filtro"; }
            if (menu.Items[i].Text == "EqualTo"){ menu.Items[i].Text = "Igual"; }
            if (menu.Items[i].Text == "NotEqualTo"){ menu.Items[i].Text = "Diferente"; }
            if (menu.Items[i].Text == "GreaterThan"){ menu.Items[i].Text = "Mayor que"; }
            if (menu.Items[i].Text == "LessThan"){ menu.Items[i].Text = "Menor que"; }
            if (menu.Items[i].Text == "GreaterThanOrEqualTo"){ menu.Items[i].Text = "Mayor o igual a"; }
            if (menu.Items[i].Text == "LessThanOrEqualTo"){ menu.Items[i].Text = "Menor o igual a"; }
            if (menu.Items[i].Text == "Between"){ menu.Items[i].Text = "Entre"; }
            if (menu.Items[i].Text == "NotBetween"){ menu.Items[i].Text = "No entre"; }
            if (menu.Items[i].Text == "IsNull"){ menu.Items[i].Text = "Es nulo"; }
            if (menu.Items[i].Text == "NotIsNull"){ menu.Items[i].Text = "No es nulo"; }
            if (menu.Items[i].Text == "Contains"){ menu.Items[i].Text = "Contenga"; }
            if (menu.Items[i].Text == "DoesNotContain"){ menu.Items[i].Text = "No Contenga"; }
            if (menu.Items[i].Text == "StartsWith"){ menu.Items[i].Text = "Inicie con"; }
            if (menu.Items[i].Text == "EndsWith"){ menu.Items[i].Text = "Finalice con"; }
            if (menu.Items[i].Text == "IsEmpty"){ menu.Items[i].Visible = false; }
            if (menu.Items[i].Text == "NotIsEmpty"){ menu.Items[i].Visible = false; }
        }
    }


}