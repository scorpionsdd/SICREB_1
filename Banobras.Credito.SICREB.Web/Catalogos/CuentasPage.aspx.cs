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

public partial class EstadoPage : System.Web.UI.Page
{
    public const String catalog = "Cuentas";
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
            if (Persona.Equals("PM"))
            {
                this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = false;
                this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = false;
            }
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
                //CambiaAtributosRGR();
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó a Catálogo " + catalog + " " + Persona);
                getClasificacion();
            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }

    }

    protected void RgdCuentas_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        Cuentas_Rules getRecords = null;
        List<Cuentas> cuentasInfo;
        List<string> lsCodigo = new List<string>();


        try
        {

            pPersona = (Persona == "PM") ? Enums.Persona.Moral : Enums.Persona.Fisica;
            if (Request.QueryString.Count == 1)
            {
                getRecords = new Cuentas_Rules(pPersona);
                cuentasInfo = getRecords.GetRecords(false);
                RgdCuentas.DataSource = cuentasInfo;
                RgdCuentas.VirtualItemCount = cuentasInfo.Count;

                for (int i = 0; i < cuentasInfo.Count; i++)
                {
                    //lsCodigo.Add(cuentasInfo[i].Codigo);
                    lsCodigo.Add(cuentasInfo[i].Codigo + "-" + cuentasInfo[i].IdClasificacion + "-" + cuentasInfo[i].Sector + "-" + cuentasInfo[i].Grupo); //JAGH 01/03/13
                }

                ViewState["Codigo"] = lsCodigo;
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar su usuario");
            }
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

    protected void RgdCuentas_CancelCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        RegresarPropiedades();
    }

    protected void RgdCuentas_UpdateCommand(object source, GridCommandEventArgs e)
    {

        Int32 id = 0;
        string codigo = string.Empty;
        string descripcion = string.Empty;
        string sector = string.Empty;
        string rol = string.Empty;
        ArrayList idClasificacion;
        string formatoSic = string.Empty;
        string formatoSicofin = string.Empty;
        string tipoCuenta = string.Empty;
        ArrayList tipoPersona;
        //ArrayList estatus;
        Int32 grupo = 0;
        Cuentas cuentaOld;
        Cuentas cuentaNew;
        Enums.Persona persona = new Enums.Persona();
        Enums.Estado estado = new Enums.Estado();
        bool Actualizar = true;


        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);

        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;
        string sCodigoOld = oldValues["Codigo"].ToString().Replace("'", "");
        try
        {
            if (newValues["Codigo"] != null && newValues["Descripcion"] != null && newValues["Rol"] != null && newValues["FormatSic"] != null && newValues["FormatSicofin"] != null && newValues["Grupo"] != null)
            {
                if (validaCampoNumerico(1, "1", newValues["Grupo"].ToString()))
                {
                    codigo = newValues["Codigo"].ToString().Replace("'", "");
                                                        
                    //JAGH 01/03/13 obtener id de clasificacion 
                    Label lblClasificacion = (Label)item.FindControl("lblClasificacion");                    
                    RadComboBox ComboClasificacion = (RadComboBox)editedItem.FindControl("ComboClasificacion");
                    string strIDClasificacion_old  = string.Empty;
                    string strIDClasificacion = string.Empty;
                    int index = 0;

                    if (!lblClasificacion.Text.Equals(ComboClasificacion.SelectedItem.Text))
                    {                        
                        strIDClasificacion = ComboClasificacion.SelectedValue.ToString();
                        //obtener id de clasificacion anterior                        
                        index = ComboClasificacion.FindItemIndexByText(lblClasificacion.Text);
                        ComboClasificacion.SelectedIndex = index;
                        strIDClasificacion_old = ComboClasificacion.SelectedValue.ToString();
                    }
                    else
                    {
                        //obtiene valores almacenados de combo
                        List<string> lClasificacion = new List<string>();
                        lClasificacion = (List<string>)ViewState["Clasificacion"];

                        //recorre lista para encontrar iguales
                        for (int i = 0; i < lClasificacion.Count; i++)
                        {
                            string[]arrayInfo = lClasificacion[i].Split(',');

                            //si es igual obtiene id de clasificacion y sale del for
                            if (arrayInfo[0].Equals(lblClasificacion.Text))
                            {
                                strIDClasificacion = arrayInfo[1];
                                strIDClasificacion_old = strIDClasificacion;
                                break;
                            }
                            
                        }//fin for
                     
                    }
                    // fin obtener id de clasificacion 

                    
                    if (sCodigoOld != codigo)
                    {
                        //JAGH 01/03/13 se agregan campos para efectuar validacion codigo
                        string strCodigo = newValues["Codigo"].ToString() + "-" +  strIDClasificacion + "-" + newValues["Sector"].ToString() + "-" + newValues["Grupo"].ToString();
                        Actualizar = bValidaCodigo(strCodigo.Replace("'", "")); //(codigo);
                    }

                    if (Actualizar) 
                    {
                        //Datos Orginales
                        id = Parser.ToNumber(newValues["Id"]);

                        //Datos Nuevos

                        descripcion = newValues["Descripcion"].ToString();
                        sector = newValues["Sector"] == null ? string.Empty : newValues["Sector"].ToString();
                        rol = newValues["Rol"].ToString();
                        //estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));
                        idClasificacion = Util.RadComboToString(item["Clasificacion"].FindControl("ComboClasificacion")); 
                        String tP = Request.QueryString["Persona"].ToString();                        
                        tipoCuenta = ((RadComboBox)item["TipoCredito"].FindControl("ComboTipoCredito")).SelectedValue;
                        formatoSic = newValues["FormatSic"].ToString();
                        formatoSicofin = newValues["FormatSicofin"].ToString().Replace("'", "");
                        estado = Enums.Estado.Activo;
                        grupo = Parser.ToNumber(newValues["Grupo"]);

                        if (tP == "PM" || tP == "pm") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
                        //if (estatus[0].ToString() == "Activo" || estatus[0].ToString() == "activo") { estado = Enums.Estado.Activo; } else if (estatus[0].ToString() == "Inactivo" || estatus[0].ToString() == "inactivo") { estado = Enums.Estado.Inactivo; } else { estado = Enums.Estado.Test; }

                        cuentaOld = new Cuentas(id, codigo, descripcion, sector, rol, Parser.ToNumber(strIDClasificacion_old), "descripcionClasificacion", formatoSic, formatoSicofin, persona, grupo, estado, tipoCuenta);
                        cuentaNew = new Cuentas(id, codigo, descripcion, sector, rol, Parser.ToNumber(strIDClasificacion), "descripcionClasificacion", formatoSic, formatoSicofin, persona, grupo, estado, tipoCuenta);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        Cuentas_Rules CuentaValores = new Cuentas_Rules(pPersona);

                        if (CuentaValores.Update(cuentaOld, cuentaNew) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                            ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog + " " + persona, 1, oldValues, newValues);
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                        }


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

   
    protected void RgdCuentas_DeleteCommand(object source, GridCommandEventArgs e)
    {
        Cuentas CuentaDelete;
        Int32 idCuenta = 0;
        //Datos para identificar valor

        GridDataItem item = (GridDataItem)e.Item;

        try
        {
            idCuenta = Parser.ToNumber(item["Id"].Text);
            CuentaDelete = new Cuentas(idCuenta, "", "", "", "", 0, "descripcionCalificacion", "", "", Enums.Persona.Moral, 0, Enums.Estado.Activo, string.Empty);

            //Llamar el SP correspondiente con las entidades old y new de Validacion

            Cuentas_Rules CuentaBorrar = new Cuentas_Rules(pPersona);

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
        Cuentas_Rules CuentaInsertar;
       
        try
        {
            Cuentas record = this.ValidaNulos(e.Item as GridEditableItem);
            if (e.Item is GridDataInsertItem)
            {   //si trae informacion valida hara la insercion
                if (record.Codigo != string.Empty && record.Descripcion != string.Empty && record.IdClasificacion != 0)
                {
                    //JAGH 01/03/13 se agregan campos para efectuar validacion codigo
                    string strCodigo = record.Codigo + "-" + record.IdClasificacion + "-" + record.Sector + "-" + record.Grupo;

                    if (bValidaCodigo(strCodigo))  //(record.Codigo))
                    {
                        CuentaInsertar = new Cuentas_Rules(pPersona);
                        if (CuentaInsertar.Insert(record) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
                        }
                    }
                }
                
                if (Request.QueryString["Persona"].ToString() == "PF")
                {
                    this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = true;
                    this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = true;
                    this.RgdCuentas.MasterTableView.GetColumn("tipoc").Visible = false;
                }
                else //MAMR 28/02/13
                {
                    this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = false;
                    this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = false;
                }
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;

            //Mensajes.ShowError(this.Page, this.GetType(), ex);

            string Cadena = "Error en la insercción ID duplicado Nulo o Inexistente, valores ( , , , , , , , , , , )" ;

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

    private Cuentas ValidaNulos(GridEditableItem editedItem)
    {
        Hashtable newValues = new Hashtable();
        Cuentas record;
        ArrayList TipoPersona;
        //ArrayList estatus;
        ArrayList idClasificacion;
        //estatus = Util.RadComboToString(editedItem["EstatusTemp"].FindControl("ComboEstatus"));       
        TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));
        idClasificacion = Util.RadComboToString(editedItem["Clasificacion"].FindControl("ComboClasificacion"));
        ArrayList TipoCuenta = Util.RadComboToString(editedItem["TipoCredito"].FindControl("ComboTipoCredito"));
        // Extrae todos los elementos
        editedItem.ExtractValues(newValues);
        String sector = (newValues["Sector"] == null) ? "00" : (String)newValues["Sector"];
        if (newValues.Count > 0)
        {
            if (newValues["Codigo"] != null && newValues["Descripcion"] != null && newValues["Rol"] != null && newValues["FormatSic"] != null && newValues["FormatSicofin"] != null && newValues["Grupo"] != null)
            {
                if (validaCampoNumerico(1, "1", newValues["Grupo"].ToString()))
                {
                    Enums.Persona persona = new Enums.Persona();
                    Enums.Estado estado = new Enums.Estado();
                    if (TipoPersona[0].ToString() == "Moral" || TipoPersona[0].ToString() == "moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
                    //if (estatus[0].ToString() == "Activo" || estatus[0].ToString() == "activo") { estado = Enums.Estado.Activo; } else if (estatus[0].ToString() == "Inactivo" || estatus[0].ToString() == "inactivo") { estado = Enums.Estado.Inactivo; } else { estado = Enums.Estado.Test; }
                    estado = Enums.Estado.Activo;
                    record = new Cuentas(0, newValues["Codigo"].ToString(), newValues["Descripcion"].ToString(), sector, newValues["Rol"].ToString(), Parser.ToNumber(idClasificacion[1]), "descripcionCalificacion", newValues["FormatSic"].ToString(), newValues["FormatSicofin"].ToString(), persona, Parser.ToNumber(newValues["Grupo"]), estado, TipoCuenta[1].ToString());
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GActividadCatalogo(7777, idUs, "Se Insertó un Registro", null, null, catalog + " " + TipoPersona, 1, null, newValues);
                }
                else
                {
                    record = new Cuentas(0, string.Empty, string.Empty, string.Empty, string.Empty, 0, "descripcionCalificacion", string.Empty, string.Empty, Enums.Persona.Moral, 0, Enums.Estado.Activo, string.Empty);
                }
            }
            else
            {
                record = new Cuentas(0, string.Empty, string.Empty, string.Empty, string.Empty, 0, "descripcionCalificacion", string.Empty, string.Empty, Enums.Persona.Moral, 0, Enums.Estado.Activo, string.Empty);
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
            }
        }
        else
        {
            record = new Cuentas(0, string.Empty, string.Empty, string.Empty, string.Empty, 0, "descripcionCalificacion", string.Empty, string.Empty, Enums.Persona.Moral, 0, Enums.Estado.Activo, string.Empty);
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
                item["TipoPersonaTemp"].Text = item["Persona"].Text;
                item["Clasificacion"].Text = item["descripcionClasificacion"].Text;
                item["FormatSicofin"].Text = string.Format("'{0}", item["FormatSicofin"].Text);
                item["Codigo"].Text = string.Format("'{0}", item["Codigo"].Text);
                //JAGH 
                Label lblTipoCuenta = (Label)item.FindControl("lblTipoCuenta");                
                
                if (item["tipoc"] != null)
                {

                    if (item["tipoc"].Text == "C")
                    {
                        item["tipoc"].Text = "Consumo";
                        lblTipoCuenta.Text = "Consumo";
                    }
                    else
                    {
                        item["tipoc"].Text = "Hipotecario";
                        lblTipoCuenta.Text = "Hipotecario";
                    }
                }

                this.RgdCuentas.MasterTableView.GetColumn("Clasificacion").Visible = true;
                this.RgdCuentas.MasterTableView.GetColumn("descripcionClasificacion").Visible = false;


            }
            else if (e.Item.IsInEditMode) //(e.Item.ItemType == GridItemType.CommandItem)
            {
                GridDataItem items;

                items = (GridDataItem)e.Item;
                RadComboBox comboP;
                comboP = (RadComboBox)items["TipoPersonaTemp"].FindControl("ComboPersona");
                if (Request.QueryString["Persona"].ToString() == "PF")
                    comboP.SelectedIndex = 0;
                else
                    comboP.SelectedIndex = 1;
                comboP.Enabled = false;

                //JAGH 01/03/13 se modifica para realizar correctamente actualizacion
                Label lblClasificacion = (Label)items.FindControl("lblClasificacion");
                RadComboBox ComboClasificacion = (RadComboBox)items.FindControl("ComboClasificacion");
                lblClasificacion.Visible = false;
                ComboClasificacion.Visible = true;        
                //this.RgdCuentas.MasterTableView.GetColumn("Clasificacion").Visible = true;
                //this.RgdCuentas.MasterTableView.GetColumn("descripcionClasificacion").Visible = false;

                //JAGH 22/02/13 se modifica para mostrar solo combo al editar y deshabilitar para PM
                Label lblTipoCuenta = (Label)items.FindControl("lblTipoCuenta");
                RadComboBox ComboTipoCredito = (RadComboBox)items.FindControl("ComboTipoCredito");
                lblTipoCuenta.Visible = false;
                ComboTipoCredito.Visible = true;

                this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = true;
                this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = true;
                this.RgdCuentas.MasterTableView.GetColumn("tipoc").Visible = false;

                //MAMR 28/02/13 
                if (Request.QueryString["Persona"].ToString() == "PM")
               {
                   ComboTipoCredito.SelectedIndex = 0;
                   ComboTipoCredito.Enabled = false;
                   this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = false;
                   this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = false;
                }
                //fin se modifica para mostrar solo combo al editar y deshabilitar para PM

                ClasificacionRules catalogo;
                RadComboBox comboClasif;
                GridDataItem item;

                item = (GridDataItem)e.Item;
                RadComboBox comboPersona;
                comboPersona = (RadComboBox)item["TipoPersonaTemp"].FindControl("ComboPersona");
                //comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);

                catalogo = new ClasificacionRules();
                comboClasif = (RadComboBox)item["Clasificacion"].FindControl("ComboClasificacion");

                comboClasif.DataSource = catalogo.GetRecords(false);
                comboClasif.DataTextField = "Descripcion";
                comboClasif.DataValueField = "Id";
                comboClasif.DataBind();

                if (newValues["Persona"] != null)
                {
                    //if (newValues["Estatus"].ToString() == "Activo")
                    //    comboEstatus.SelectedIndex = 0;
                    //else
                    //    comboEstatus.SelectedIndex = 1;
                    if (newValues["Persona"].ToString() == "Fisica")
                        comboPersona.SelectedIndex = 0;
                    else
                        comboPersona.SelectedIndex = 1;
                    comboPersona.Enabled = false;

                    comboClasif.SelectedItem.Text = newValues["descripcionClasificacion"].ToString();
                    if (newValues["tipoCuenta"] != null)
                    {
                        if (newValues["tipoCuenta"].ToString() == "H")
                            ((RadComboBox)item["TipoCredito"].FindControl("ComboTipoCredito")).SelectedIndex = 0;
                        else
                            ((RadComboBox)item["TipoCredito"].FindControl("ComboTipoCredito")).SelectedIndex = 1;
                    }
                }
            }
           

            //if (e.Item is GridFilteringItem)
            //{
            //List<Clasificacion> classInfo;
            //GridFilteringItem filterItem = (GridFilteringItem)e.Item;
            // RadComboBox en el Filter Template
            //RadComboBox combo = (RadComboBox)filterItem.FindControl("FiltroRadCombo");
            // asigno los Items
            //ClasificacionRules clasifica = new ClasificacionRules();
            //RadComboBoxItem seleccione = new RadComboBoxItem();
            //seleccione.Text = "Seleccione..";
            //seleccione.Value = "0";
            //combo.Items.Add(seleccione);
            //    classInfo = clasifica.GetRecords(false);
            //    combo.DataSource = classInfo;
            //    combo.DataTextField = "Descripcion";
            //    combo.DataValueField = "Id";
            //    combo.DataBind();
            //}

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
    //protected void FiltroRadCombo_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    string filtro = string.Empty;
    //    try
    //    {
    //        filtro = ((RadComboBox)o).Text.ToString();
    //        string filterExpression;
    //        GridColumn column = RgdCuentas.MasterTableView.GetColumnSafe("Clasificacion");
    //        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
    //        filterExpression = "[Clasificacion] = '" + filtro + "'";
    //        RgdCuentas.MasterTableView.FilterExpression = filterExpression;
    //        RgdCuentas.MasterTableView.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        Mensajes.ShowError(this.Page, this.GetType(), ex);
    //    }
    //}
    public bool validaCampoNumerico(int type, string txtIdCalificacion, string txtGrupo)
    {
        //1  Numerico
        bool valido = false;
        switch (type)
        {
            case 1:
                for (int n = 0; n < txtIdCalificacion.Length; n++)
                {
                    if (!Char.IsNumber(txtIdCalificacion, n))
                    {
                        valido = false;
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en los campos id, Clasificación y grupo deben ser numéricos, favor de verificar");
                        break;
                    }
                    else
                        valido = true;
                }
                if (valido)
                {
                    for (int n = 0; n < txtGrupo.Length; n++)
                    {
                        if (!Char.IsNumber(txtGrupo, n))
                        {
                            valido = false;
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en los campos id, Clasificación y grupo deben ser numéricos, favor de verificar");
                            break;
                        }
                        else
                            valido = true;
                    }
                }


                break;
            case 2:

                break;
        }
        return valido;
    }


    public void RegresarPropiedades ()
    {
        this.RgdCuentas.MasterTableView.GetColumn("Clasificacion").Visible = false;
        this.RgdCuentas.MasterTableView.GetColumn("descripcionClasificacion").Visible = true;

        //JAGH 22/02/13
        //if (Request.QueryString["Persona"].ToString() == "PF")
        //{
            this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = true;
            this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = true;
            this.RgdCuentas.MasterTableView.GetColumn("tipoc").Visible = false;
        //}

            //JAGH 22/02/13
            if (Request.QueryString["Persona"].ToString() == "PM")
            {
            this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = false;
            this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = false;
            }
    }
    

    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        this.RgdCuentas.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        //this.RgdCuentas.MasterTableView.GetColumn("DeleteState").Visible = false;
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
                //this.RgdCuentas.MasterTableView.GetColumn("DeleteState").Visible = true;
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
                //this.RgdCuentas.MasterTableView.GetColumn("DeleteState").Visible = true;
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
                row1["nombreColumna"] = "Código";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Descripción";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "2";



                DataRow row4;

                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "Sector";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "30";

                DataRow row5;

                row5 = dt_metaDataLayout.NewRow();
                row5["nombreColumna"] = "Rol";
                row5["tipoDato"] = "VARCHAR2";
                row5["longitud"] = "30";


                DataRow row6;

                row6 = dt_metaDataLayout.NewRow();
                row6["nombreColumna"] = "Clasificación";
                row6["tipoDato"] = "NUMBER";
                row6["longitud"] = "30";


                DataRow row7;

                row7 = dt_metaDataLayout.NewRow();
                row7["nombreColumna"] = "Formato SIC";
                row7["tipoDato"] = "VARCHAR2";
                row7["longitud"] = "30";


                DataRow row8;

                row8 = dt_metaDataLayout.NewRow();
                row8["nombreColumna"] = "Formato SICOFIN";
                row8["tipoDato"] = "VARCHAR2";
                row8["longitud"] = "30";




                DataRow row9;

                row9 = dt_metaDataLayout.NewRow();
                row9["nombreColumna"] = "Persona";
                row9["tipoDato"] = "VARCHAR2";
                row9["longitud"] = "5";

                DataRow row10;

                row10 = dt_metaDataLayout.NewRow();
                row10["nombreColumna"] = "Grupo";
                row10["tipoDato"] = "NUMBER";
                row10["longitud"] = "99999999";


                DataRow row11;

                row11 = dt_metaDataLayout.NewRow();
                row11["nombreColumna"] = "Tipo Cuenta";
                row11["tipoDato"] = "VARCHAR2";
                row11["longitud"] = "100";


                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);

                dt_metaDataLayout.Rows.Add(row4);
                dt_metaDataLayout.Rows.Add(row5);
                dt_metaDataLayout.Rows.Add(row6);
                dt_metaDataLayout.Rows.Add(row7);
                dt_metaDataLayout.Rows.Add(row8);
                dt_metaDataLayout.Rows.Add(row9);
                dt_metaDataLayout.Rows.Add(row10);
                if (Persona == "PF")
                {
                    dt_metaDataLayout.Rows.Add(row11);
                }
                //dt_metaDataLayout.Rows.Add(row12);


                //List<DbParameter> parametros = new List<DbParameter>();

                DbParameter[] parametros;


                parametros = new DbParameter[13];
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
                parametros[3].ParameterName = "codigop";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "descripcionp";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 100;



                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "sectorp";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 200;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "rolp";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 50;

                parametros[7] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].ParameterName = "idclasificacionp";
                parametros[7].DbType = DbType.String;
                parametros[7].Size = 30;


                parametros[8] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[8].Direction = ParameterDirection.Input;
                parametros[8].ParameterName = "formatosicp";
                parametros[8].DbType = DbType.String;
                parametros[8].Size = 100;

                parametros[9] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[9].Direction = ParameterDirection.Input;
                parametros[9].ParameterName = "formatosicofinp";
                parametros[9].DbType = DbType.String;
                parametros[9].Size = 100;

                parametros[10] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[10].Direction = ParameterDirection.Input;
                parametros[10].ParameterName = "personap";
                parametros[10].DbType = DbType.String;
                parametros[10].Size = 30;

                parametros[11] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[11].Direction = ParameterDirection.Input;
                parametros[11].ParameterName = "grupop";
                parametros[11].DbType = DbType.Decimal;
                parametros[11].Size = 30;


                parametros[12] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[12].Direction = ParameterDirection.Input;
                parametros[12].ParameterName = "tipop";
                parametros[12].DbType = DbType.String;
                parametros[12].Size = 30;

                string storeBase = "SP_cargaMasiva_Cuentas";


                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_cuentas", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, Persona);
                int numeros = cargaMasiva.Correctos;

                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdCuentas.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog + " " + Persona, total + " Registros", numeros + " Registros", catalog, 1);

                RgdCuentas.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error En la carga del archivo<br/> {0}</b>",
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
            GridHeaderItem headerItem = RgdCuentas.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdCuentas.AllowPaging = false;
                RgdCuentas.Rebind();
                foreach (GridDataItem row in RgdCuentas.Items)
                {
                    Cuentas CuentasDelete;
                    int pId = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    string pCodigo = row["Codigo"].Text.ToString();
                    string pDescripcion = row["Descripcion"].Text.ToString();
                    string pSector = row["Sector"].Text.ToString();
                    string pRol = row["Rol"].Text.ToString();
                    int pIdClasificacion = Parser.ToNumber(row["Clasificacion"].Text.ToString());
                    string pDescripcionClasif = row["descripcionClasificacion"].Text.ToString();
                    string pFormatSic = row["FormatSic"].Text.ToString();
                    string pFormatSicofin = row["FormatSicofin"].Text.ToString();
                    int pGrupo = Parser.ToNumber(row["Grupo"].Text.ToString());
                    string pTipo = row["TipoCredito"].Text.ToString();



                    CuentasDelete = new Cuentas(pId, pCodigo, pDescripcion, pSector, pRol, pIdClasificacion, pDescripcionClasif, pFormatSic, pFormatSicofin, (Persona == "PF") ? Enums.Persona.Fisica : Enums.Persona.Moral, pGrupo, Enums.Estado.Activo, pTipo);
                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
                    //CuentasDataAccess CuentasBorrar = new CuentasDataAccess();
                    Cuentas_Rules CuentasBorrar = new Cuentas_Rules((Persona == "PF") ? Enums.Persona.Fisica : Enums.Persona.Moral);
                    if (CuentasBorrar.Delete(CuentasDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "Los Registros se han removido correctamente");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }
                }//foreach
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


                        Cuentas CuentaDelete;
                        Int32 idCuenta = 0;
                        //Datos para identificar valor


                        idCuenta = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                        CuentaDelete = new Cuentas(idCuenta, "", "", "", "", 0, "descripcionCalificacion", "", "", Enums.Persona.Moral, 0, Enums.Estado.Activo, string.Empty);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion

                        Cuentas_Rules CuentaBorrar = new Cuentas_Rules(pPersona);

                        if (CuentaBorrar.Delete(CuentaDelete) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                            Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con ID " + idCuenta, null, null, catalog + " " + pPersona, 1, null, null);
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
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch
        //btn_eliminar_Click
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

                    row["TipoPersonaTemp"].Text = row["Persona"].Text;
                    row["Clasificacion"].Text = row["descripcionClasificacion"].Text;
                    row["FormatSicofin"].Text = string.Format("'{0}", row["FormatSicofin"].Text);
                    row["Codigo"].Text = string.Format("'{0}", row["Codigo"].Text);
                    if (row["tipoc"] != null)
                    {

                        if (row["tipoc"].Text == "C")
                            row["tipoc"].Text = "Consumo";
                        else
                            row["tipoc"].Text = "Hipotecario";
                    }
                }
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch


        //ChkTodo_CheckedChanged
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

    //JAGH 01/03/13 llena lista con datos de combo clasificacion
    List<Clasificacion> lClasificacion;
    List<string> descClasificacion = new List<string>();
    private void getClasificacion()
    {
        ClasificacionRules catalogo = new ClasificacionRules();
        lClasificacion = catalogo.GetRecords(false);
        for (int i = 0; i < lClasificacion.Count; i++)
        {
            descClasificacion.Add(lClasificacion[i].Descripcion + "," + lClasificacion[i].Id.ToString()); 
        }
        ViewState["Clasificacion"] = descClasificacion;
    }

}