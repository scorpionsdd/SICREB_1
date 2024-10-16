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

public partial class EstadoCivilPage : System.Web.UI.Page
{

    int idUsuario;
    string persona;
    public const String catalog = "Estado Civil";

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            CambiaAtributosRGR();

            if (Session["Facultades"] != null)
            {
                persona = "PF";
                getFacultades(persona);
                if (!this.Page.IsPostBack)
                {
                    int idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUsuario, "El Usuario " + Session["nombreUser"] + " Ingresó a Catálogo " + catalog);
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


    protected void RgdEstadoCivil_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        EstadoCivil_Rules getRecords = null;
        List<EstadoCivil> EstadoCivilInfo;

        try
        {
            getRecords = new EstadoCivil_Rules();
            var s = getRecords.GetRecords(false);
            EstadoCivilInfo = s;
            RgdEstadoCivil.DataSource = EstadoCivilInfo;
            RgdEstadoCivil.VirtualItemCount = s.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    
    protected void RgdEstadoCivil_ItemDataBound(object sender, GridItemEventArgs e)
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
            else if (e.Item.IsInEditMode)
            {

                GridDataItem item;
                item = (GridDataItem)e.Item;
                item["Id"].Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    protected void RgdEstadoCivil_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            // Quitamos los nulos para evitar error en el Hashtable
            if (newValues["Id"] == null) { newValues["Id"] = "0"; }
            if (newValues["IdClic"] == null) { newValues["IdClic"] = "0"; }
            if (newValues["DescripcionClic"] == null) { newValues["DescripcionClic"] = string.Empty; }
            if (newValues["ClaveBuro"] == null) { newValues["ClaveBuro"] = string.Empty; }

            EstadoCivil estadoCivilNew;
            EstadoCivil_Rules EstadoCivilAgregar = new EstadoCivil_Rules();

            if (ValidarDatosEstadoCivil(newValues) == true)
            {

                estadoCivilNew = new EstadoCivil(0,
                                    Parser.ToNumber(newValues["IdClic"].ToString().ToUpper()),
                                    newValues["DescripcionClic"].ToString().ToUpper(),
                                    newValues["ClaveBuro"].ToString().ToUpper()
                                    );

                if (EstadoCivilAgregar.Insert(estadoCivilNew) > 0)
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

    protected void RgdEstadoCivil_UpdateCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;

            // Quitamos los nulos para evitar error en el Hashtable
            if (oldValues["IdClic"] == null) { oldValues["IdClic"] = "0"; }
            if (oldValues["DescripcionClic"] == null) { oldValues["DescripcionClic"] = string.Empty; }
            if (oldValues["ClaveBuro"] == null) { oldValues["ClaveBuro"] = string.Empty; }

            Int32 idEstadoCivil = Parser.ToNumber(item.SavedOldValues["Id"]);
            newValues["Id"] = idEstadoCivil;

            if (newValues["IdClic"] == null) { newValues["IdClic"] = "0"; }
            if (newValues["DescripcionClic"] == null) { newValues["DescripcionClic"] = string.Empty; }
            if (newValues["ClaveBuro"] == null) { newValues["ClaveBuro"] = string.Empty; }

            EstadoCivil estadoCivilOld;
            EstadoCivil estadoCivilNew;
            EstadoCivil_Rules EstadoCivilActualizar = new EstadoCivil_Rules();
            
            if (ValidarDatosEstadoCivil(newValues) == true)
            {

                estadoCivilOld = new EstadoCivil(idEstadoCivil,
                                    Parser.ToNumber(oldValues["IdClic"].ToString().ToUpper()),
                                    oldValues["DescripcionClic"].ToString().ToUpper(),
                                    oldValues["ClaveBuro"].ToString().ToUpper()
                                    );

                estadoCivilNew = new EstadoCivil(idEstadoCivil,
                                    Parser.ToNumber(newValues["IdClic"].ToString().ToUpper()),
                                    newValues["DescripcionClic"].ToString().ToUpper(),
                                    newValues["ClaveBuro"].ToString().ToUpper()
                                    );

                if (EstadoCivilActualizar.Update(estadoCivilOld, estadoCivilNew) > 0)
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
                row1["nombreColumna"] = "Id CLIC";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "5";

                DataRow row2;
                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Descripción CLIC";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "100";

                DataRow row3;
                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Clave Buró";
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
                parametros[0].Size = 5;

                parametros[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[1].Direction = ParameterDirection.Output;
                parametros[1].ParameterName = "tipo_OUT";
                parametros[1].DbType = DbType.Decimal;
                parametros[1].Size = 5;

                parametros[2] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].ParameterName = "PersonaP";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "IdClicP";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 5;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "DescripcionClicP";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 100;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "ClaveBuroP";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 5;

                string storeBase = "SP_CARGAMASIVA_ESTADOCIVIL";
                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_edo_civil", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }

                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdEstadoCivil.VirtualItemCount;
                int idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUsuario, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdEstadoCivil.Rebind();
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
            GridHeaderItem headerItem = RgdEstadoCivil.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");

            if (chkHeader.Checked == true)
            {
                int RegistrosEliminados = 0;
                RgdEstadoCivil.AllowPaging = false;
                RgdEstadoCivil.Rebind();

                foreach (GridDataItem row in RgdEstadoCivil.Items)
                {
                    EstadoCivil EstadoCivilDelete;
                    Int32 IdEstadoCivil = 0;
                    Int32 ClaveClic = 0;
                    string Descripcion = string.Empty;
                    string ClaveBuro = string.Empty;

                    //Datos para identificar valor

                    IdEstadoCivil = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    ClaveClic = Parser.ToNumber(row["IdClic"].Text);
                    Descripcion = row["DescripcionClic"].Text;
                    ClaveBuro = row["ClaveBuro"].Text;

                    EstadoCivilDelete = new EstadoCivil(IdEstadoCivil, ClaveClic, Descripcion, ClaveBuro);

                    //Llamar el SP correspondiente con las entidades old y new de validacion
                    EstadoCivil_Rules EstadoCivilBorrar = new EstadoCivil_Rules();

                    if (EstadoCivilBorrar.Delete(EstadoCivilDelete) > 0)
                    {
                        RegistrosEliminados++;
                    }
                }
                    
                if (RgdEstadoCivil.Items.Count == RegistrosEliminados)
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Registros removidos correctamente");
                }
                else
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), RegistrosEliminados + " registros de " + RgdEstadoCivil.Items.Count + " removidos correctamente");
                }

                RgdEstadoCivil.Rebind();
                RgdEstadoCivil.DataSource = null;
                RgdEstadoCivil.AllowPaging = true;
                RgdEstadoCivil.Rebind();
                RgdEstadoCivil.DataBind();
                idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUsuario, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdEstadoCivil.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
            }
            else
            {
                int RegistrosEliminados = 0;

                foreach (GridDataItem row in RgdEstadoCivil.Items)
                {
                    CheckBox chkCell;
                    bool chkResult;
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;

                    if (chkResult)
                    {

                        EstadoCivil EstadoCivilDelete;
                        Int32 IdEstadoCivil = 0;
                        Int32 ClaveClic = 0;
                        string Descripcion = string.Empty;
                        string ClaveBuro = string.Empty;

                        //Datos para identificar valor

                        IdEstadoCivil = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                        ClaveClic = Parser.ToNumber(row["IdClic"].Text);
                        Descripcion = row["DescripcionClic"].Text;
                        ClaveBuro = row["ClaveBuro"].Text;

                        EstadoCivilDelete = new EstadoCivil(IdEstadoCivil, ClaveClic, Descripcion, ClaveBuro);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        EstadoCivil_Rules EstadoCivilBorrar = new EstadoCivil_Rules();

                        if (EstadoCivilBorrar.Delete(EstadoCivilDelete) > 0)
                        {
                            RegistrosEliminados++;
                            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + IdEstadoCivil, null, null, catalog, 1, null, null);
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

        RgdEstadoCivil.Rebind();
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdEstadoCivil.Columns[0].Visible = false;
        RgdEstadoCivil.Columns[RgdEstadoCivil.Columns.Count - 1].Visible = false;
        RgdEstadoCivil.MasterTableView.HierarchyDefaultExpanded = true;
        RgdEstadoCivil.ExportSettings.OpenInNewWindow = false;
        RgdEstadoCivil.ExportSettings.ExportOnlyData = true;
        RgdEstadoCivil.MasterTableView.GridLines = GridLines.Both;
        RgdEstadoCivil.ExportSettings.IgnorePaging = true;
        RgdEstadoCivil.ExportSettings.OpenInNewWindow = true;
        RgdEstadoCivil.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
        RgdEstadoCivil.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdEstadoCivil.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catálogo " + catalog);
    }
    
    protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdEstadoCivil.Columns[0].Visible = false;
        RgdEstadoCivil.Columns[RgdEstadoCivil.Columns.Count - 1].Visible = false;
        RgdEstadoCivil.MasterTableView.HierarchyDefaultExpanded = true;
        RgdEstadoCivil.ExportSettings.OpenInNewWindow = false;
        RgdEstadoCivil.ExportSettings.ExportOnlyData = true;
        RgdEstadoCivil.ExportSettings.IgnorePaging = true;
        RgdEstadoCivil.ExportSettings.OpenInNewWindow = true;
        RgdEstadoCivil.MasterTableView.ExportToExcel();

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

            foreach (GridDataItem row in RgdEstadoCivil.Items)
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CIVIL_F")))
        {
            valido = true;
        }
        return valido;
    }

    private bool ValidarDatosEstadoCivil(Hashtable DatosEstadoCivil)
    {

        // Solo se contemplan las validaciones mas basicas de los datos del catalogo.

        if (DatosEstadoCivil["IdClic"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El ID del Clic no puede ser nulo");
            return false;
        }

        if (DatosEstadoCivil["ClaveBuro"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La Clave del Buro no puede ser nula");
            return false;
        }

       if (ValidarCampoNumerico(DatosEstadoCivil["IdClic"].ToString().Trim(), "Id Clic") == false)
        {
            return false;
        }

        if (ValidarRegistroDuplicado(DatosEstadoCivil["Id"].ToString(), DatosEstadoCivil["IdClic"].ToString()) == true)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Ya existe un registro con el mismo Id Clave Clic: " + DatosEstadoCivil["IdClic"].ToString());
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

    private bool ValidarRegistroDuplicado(string idRegistro, string IdClic)
    {
        bool Respuesta = false;

        try
        {
            EstadoCivil_Rules getRecords = new EstadoCivil_Rules();
            List<EstadoCivil> RegistrosEstadoCivil = getRecords.GetRecords(false);

            foreach (EstadoCivil ItemEstadoCivil in RegistrosEstadoCivil)
            {

                if (IdClic.ToUpper() == ItemEstadoCivil.IdClic.ToString().ToUpper())
                {
                    if (idRegistro == ItemEstadoCivil.Id.ToString() && idRegistro.Trim() != "0")
                    {
                        // Si el id del registro es diferente de 0 se trata de la actualizacion de un registro
                        // Si el id del Estado Civil encontrado es igual al del registro que esta siendo editado permitimos
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

        this.RgdEstadoCivil.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        btnExportarPDF.Visible = false;
        btnExportarExcel.Visible = false;

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CIVIL_F")))
        {
            this.RgdEstadoCivil.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CIVIL_F")))
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
        GridFilterMenu menu = RgdEstadoCivil.FilterMenu;
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