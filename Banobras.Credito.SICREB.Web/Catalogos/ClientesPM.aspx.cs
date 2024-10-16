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


public partial class Catalogos_ClientesPM : System.Web.UI.Page
{
    public const String catalog = "ClientesPM";
    int idUs;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CambiaAtributosRGR();
            if (Session["Facultades"] != null)
            {
                getFacultades("PM");
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");

            }
            if (!this.Page.IsPostBack)
            {
                //CambiaAtributosRGR();
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingresó a Catálogo " + catalog);
            }

        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }

    }

    protected void RgdCuentas_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ClientesPM_rules getRecords = null;
        List<ClientesPM> ClientesPM_Info;
        //List<string> lsCodigo = new List<string>();


        try
        {

            string pPersona = "PM";
            getRecords = new ClientesPM_rules();
            ClientesPM_Info = getRecords.GetRecords();
            RgdCuentas.DataSource = ClientesPM_Info;
            RgdCuentas.VirtualItemCount = ClientesPM_Info.Count;

            //for (int i = 0; i < ClientesPM_Info.Count; i++)
            //{
            //lsCodigo.Add(cuentasInfo[i].Codigo);
            //lsCodigo.Add(ClientesPM_Info[i].Codigo + "-" + ClientesPM_Info[i].IdClasificacion + "-" + ClientesPM_Info[i].Sector + "-" + ClientesPM_Info[i].Grupo); //JAGH 01/03/13
            //}

            //ViewState["Codigo"] = lsCodigo;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdCuentas_CancelCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //RegresarPropiedades();
    }
    protected void RgdCuentas_UpdateCommand(object source, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;


        string Rfc = oldValues["RFC"] == null ? string.Empty : oldValues["RFC"].ToString();
        string Curp = oldValues["CURP"] == null ? string.Empty : oldValues["CURP"].ToString();
        string Nombre = oldValues["NOMBRE"] == null ? string.Empty : oldValues["NOMBRE"].ToString();
        string Apellido_paterno = oldValues["APELLIDO_PATERNO"] == null ? null : oldValues["APELLIDO_PATERNO"].ToString();
        string Apellido_materno = oldValues["APELLIDO_MATERNO"] == null ? null : oldValues["APELLIDO_MATERNO"].ToString();
        string Nacionalidad_clave = oldValues["NACIONALIDAD_CLAVE"] == null ? null : oldValues["NACIONALIDAD_CLAVE"].ToString();
        string Nacionalidad = oldValues["NACIONALIDAD"] == null ? null : oldValues["NACIONALIDAD"].ToString();
        string Calle = oldValues["CALLE"] == null ? null : oldValues["CALLE"].ToString();
        string Num_ext = oldValues["NUM_EXT"] == null ? null : oldValues["NUM_EXT"].ToString();
        string Num_int = oldValues["NUM_INT"] == null ? null : oldValues["NUM_INT"].ToString();
        string Colonia = oldValues["COLONIA"] == null ? null : oldValues["COLONIA"].ToString();
        string Municipio_clave = oldValues["MUNICIPIO_CLAVE"] == null ? null : oldValues["MUNICIPIO_CLAVE"].ToString();
        string Municipio = oldValues["MUNICIPIO"] == null ? null : oldValues["MUNICIPIO"].ToString();
        string Ciudad = oldValues["CIUDAD"] == null ? null : oldValues["CIUDAD"].ToString();
        string Estado_clave = oldValues["ESTADO_CLAVE"] == null ? null : oldValues["ESTADO_CLAVE"].ToString();
        string Estado = oldValues["ESTADO"] == null ? null : oldValues["ESTADO"].ToString();
        string Codigo_postal = oldValues["CODIGO_POSTAL"] == null ? null : oldValues["CODIGO_POSTAL"].ToString();
        string Telefonos = oldValues["TELEFONOS"] == null ? null : oldValues["TELEFONOS"].ToString();
        string Pais_clave = oldValues["PAIS_CLAVE"] == null ? null : oldValues["PAIS_CLAVE"].ToString();
        string Pais = oldValues["PAIS"] == null ? null : oldValues["PAIS"].ToString();
        string Tipo_cliente_clave = oldValues["TIPO_CLIENTE_CLAVE"] == null ? null : oldValues["TIPO_CLIENTE_CLAVE"].ToString();
        string Tipo_cliente = oldValues["TIPO_CLIENTE"] == null ? null : oldValues["TIPO_CLIENTE"].ToString();
        string Compania = oldValues["COMPANIA"] == null ? null : oldValues["COMPANIA"].ToString();
        string Act_eco_clave = oldValues["ACT_ECO_CLAVE"] == null ? null : oldValues["ACT_ECO_CLAVE"].ToString();
        string Act_eco = oldValues["ACT_ECO"] == null ? null : oldValues["ACT_ECO"].ToString();
        string Usuario_alta = oldValues["USUARIO_ALTA"] == null ? null : oldValues["USUARIO_ALTA"].ToString();
        string Consecutivo = null;
        if (oldValues["CONSECUTIVO"] != null)
        {
            Consecutivo = oldValues["CONSECUTIVO"].ToString();
        }
        string Estatus = oldValues["ESTATUS"] == null ? null : oldValues["ESTATUS"].ToString();
        string Id_tipo_cliente = null;
        if (oldValues["ID_TIPO_CLIENTE"] != null)
        {
            Id_tipo_cliente = oldValues["ID_TIPO_CLIENTE"].ToString();
        }

        ClientesPM clienteOld = new ClientesPM(Rfc, Curp, Nombre, Apellido_paterno, Apellido_materno, Nacionalidad_clave, Nacionalidad, Calle, Num_ext, Num_int, Colonia, Municipio_clave, Municipio, Ciudad, Estado_clave, Estado, Codigo_postal, Telefonos, Pais_clave, Pais, Tipo_cliente_clave, Tipo_cliente, Compania, Act_eco_clave, Act_eco, Usuario_alta, Consecutivo, Estatus, Id_tipo_cliente);

        Rfc = newValues["RFC"] == null ? String.Empty : newValues["RFC"].ToString();
        Curp = newValues["CURP"] == null ? null : newValues["CURP"].ToString();
        Nombre = newValues["NOMBRE"] == null ? null : newValues["NOMBRE"].ToString();
        Apellido_paterno = newValues["APELLIDO_PATERNO"] == null ? null : newValues["APELLIDO_PATERNO"].ToString();
        Apellido_materno = newValues["APELLIDO_MATERNO"] == null ? null : newValues["APELLIDO_MATERNO"].ToString();
        Nacionalidad_clave = newValues["NACIONALIDAD_CLAVE"] == null ? null : newValues["NACIONALIDAD_CLAVE"].ToString();
        Nacionalidad = newValues["NACIONALIDAD"] == null ? null : newValues["NACIONALIDAD"].ToString();
        Calle = newValues["CALLE"] == null ? null : newValues["CALLE"].ToString();
        Num_ext = newValues["NUM_EXT"] == null ? null : newValues["NUM_EXT"].ToString();
        Num_int = newValues["NUM_INT"] == null ? null : newValues["NUM_INT"].ToString();
        Colonia = newValues["COLONIA"] == null ? null : newValues["COLONIA"].ToString();
        Municipio_clave = newValues["MUNICIPIO_CLAVE"] == null ? null : newValues["MUNICIPIO_CLAVE"].ToString();
        Municipio = newValues["MUNICIPIO"] == null ? null : newValues["MUNICIPIO"].ToString();
        Ciudad = newValues["CIUDAD"] == null ? null : newValues["CIUDAD"].ToString();
        Estado_clave = newValues["ESTADO_CLAVE"] == null ? null : newValues["ESTADO_CLAVE"].ToString();
        Estado = newValues["ESTADO"] == null ? null : newValues["ESTADO"].ToString();
        Codigo_postal = string.Empty;
        if (newValues["CODIGO_POSTAL"] != null)
        {
            Codigo_postal =  newValues["CODIGO_POSTAL"].ToString();
            if ((Codigo_postal.Length > 5) && (Codigo_postal.IndexOf("'")==-1)) //SI ES UN CODIGO POSTAL MAYOR A 5 POSICIONES PERO NO TIENE APOSTROFO, SOLO SE TOMAN LOS PRIMERO 5 CARACTERES
            {
                Codigo_postal = Codigo_postal.Substring(0, 5);
            }
        }
        Telefonos = newValues["TELEFONOS"] == null ? null : newValues["TELEFONOS"].ToString();
        Pais_clave = newValues["PAIS_CLAVE"] == null ? null : newValues["PAIS_CLAVE"].ToString();
        Pais = newValues["PAIS"] == null ? null : newValues["PAIS"].ToString();
        Tipo_cliente_clave = newValues["TIPO_CLIENTE_CLAVE"] == null ? null : newValues["TIPO_CLIENTE_CLAVE"].ToString();
        Tipo_cliente = newValues["TIPO_CLIENTE"] == null ? null : newValues["TIPO_CLIENTE"].ToString();
        Compania = newValues["COMPANIA"] == null ? null : newValues["COMPANIA"].ToString();
        Act_eco_clave = newValues["ACT_ECO_CLAVE"] == null ? null : newValues["ACT_ECO_CLAVE"].ToString();
        Act_eco = newValues["ACT_ECO"] == null ? null : newValues["ACT_ECO"].ToString();
        Usuario_alta = newValues["USUARIO_ALTA"] == null ? null : newValues["USUARIO_ALTA"].ToString();
        Consecutivo = string.Empty;
        if (newValues["CONSECUTIVO"] != null)
        {
            Consecutivo = newValues["CONSECUTIVO"].ToString();
        }
        Estatus = newValues["ESTATUS"] == null ? null : newValues["ESTATUS"].ToString();
        Id_tipo_cliente = string.Empty;
        if (newValues["ID_TIPO_CLIENTE"] != null)
        {
            Id_tipo_cliente = newValues["ID_TIPO_CLIENTE"].ToString();
        }
        ClientesPM clienteNew = new ClientesPM(Rfc, Curp, Nombre, Apellido_paterno, Apellido_materno, Nacionalidad_clave, Nacionalidad, Calle, Num_ext, Num_int, Colonia, Municipio_clave, Municipio, Ciudad, Estado_clave, Estado, Codigo_postal, Telefonos, Pais_clave, Pais, Tipo_cliente_clave, Tipo_cliente, Compania, Act_eco_clave, Act_eco, Usuario_alta, Consecutivo, Estatus, Id_tipo_cliente);


        try
        {
            if (clienteNew.Rfc != null && clienteNew.Rfc.ToString().Trim() != "")
            {
                if (validaCampoNumerico(1, clienteNew.Consecutivo, clienteNew.Id_tipo_cliente))
                {

                    ClientesPM_rules ClienteValores = new ClientesPM_rules();

                    if (ClienteValores.Update(clienteOld, clienteNew) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog , 1, oldValues, newValues);
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                        //RadAjaxManager1.ResponseScripts.Add("radalert('El RFC es obligatorio.', 320, 100);");
                        //index = e.Item.ItemIndex;

                    }
                }
            }
            else // el RFC viene nulo o vacio
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue insertado debido a que el campo del RFC es obligatorio, por favor verifique y vuelva a intentarlo.");
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
        finally
        {
            //RegresarPropiedades();
        }
    }
    protected void RgdCuentas_DeleteCommand(object source, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        string sRFCborrar = item["RFC"].ToString();

        try
        {

            if (sRFCborrar != null && sRFCborrar.ToString().Trim() != "")
            {

                    ClientesPM_rules ClienteBorrar = new ClientesPM_rules();

                    if (ClienteBorrar.Delete(sRFCborrar) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                        //ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                        //RadAjaxManager1.ResponseScripts.Add("radalert('El RFC es obligatorio.', 320, 100);");
                        //index = e.Item.ItemIndex;

                    }
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }
    protected void RgdCuentas_InsertCommand(object source, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        Hashtable newValues = new Hashtable();
        editedItem.ExtractValues(newValues);
        GridDataItem item = (GridDataItem)e.Item;
        Hashtable oldValues = (Hashtable)item.SavedOldValues;

        string Rfc = newValues["RFC"] == null ? String.Empty : newValues["RFC"].ToString();
        string Curp = newValues["CURP"] == null ? null : newValues["CURP"].ToString();
        string Nombre = newValues["NOMBRE"] == null ? null : newValues["NOMBRE"].ToString();
        string Apellido_paterno = newValues["APELLIDO_PATERNO"] == null ? null : newValues["APELLIDO_PATERNO"].ToString();
        string Apellido_materno = newValues["APELLIDO_MATERNO"] == null ? null : newValues["APELLIDO_MATERNO"].ToString();
        string Nacionalidad_clave = newValues["NACIONALIDAD_CLAVE"] == null ? null : newValues["NACIONALIDAD_CLAVE"].ToString();
        string Nacionalidad = newValues["NACIONALIDAD"] == null ? null : newValues["NACIONALIDAD"].ToString();
        string Calle = newValues["CALLE"] == null ? null : newValues["CALLE"].ToString();
        string Num_ext = newValues["NUM_EXT"] == null ? null : newValues["NUM_EXT"].ToString();
        string Num_int = newValues["NUM_INT"] == null ? null : newValues["NUM_INT"].ToString();
        string Colonia = newValues["COLONIA"] == null ? null : newValues["COLONIA"].ToString();
        string Municipio_clave = newValues["MUNICIPIO_CLAVE"] == null ? null : newValues["MUNICIPIO_CLAVE"].ToString();
        string Municipio = newValues["MUNICIPIO"] == null ? null : newValues["MUNICIPIO"].ToString();
        string Ciudad = newValues["CIUDAD"] == null ? null : newValues["CIUDAD"].ToString();
        string Estado_clave = newValues["ESTADO_CLAVE"] == null ? null : newValues["ESTADO_CLAVE"].ToString();
        string Estado = newValues["ESTADO"] == null ? null : newValues["ESTADO"].ToString();
        string Codigo_postal = string.Empty;
        if (newValues["CODIGO_POSTAL"] != null)
        {
            Codigo_postal = newValues["CODIGO_POSTAL"].ToString();
            if ((Codigo_postal.Length > 5) && (Codigo_postal.IndexOf("'") == -1)) //SI ES UN CODIGO POSTAL MAYOR A 5 POSICIONES PERO NO TIENE APOSTROFO, SOLO SE TOMAN LOS PRIMERO 5 CARACTERES
            {
                Codigo_postal = Codigo_postal.Substring(0, 5);
            }
        }
        string Telefonos = newValues["TELEFONOS"] == null ? null : newValues["TELEFONOS"].ToString();
        string Pais_clave = newValues["PAIS_CLAVE"] == null ? null : newValues["PAIS_CLAVE"].ToString();
        string Pais = newValues["PAIS"] == null ? null : newValues["PAIS"].ToString();
        string Tipo_cliente_clave = newValues["TIPO_CLIENTE_CLAVE"] == null ? null : newValues["TIPO_CLIENTE_CLAVE"].ToString();
        string Tipo_cliente = newValues["TIPO_CLIENTE"] == null ? null : newValues["TIPO_CLIENTE"].ToString();
        string Compania = newValues["COMPANIA"] == null ? null : newValues["COMPANIA"].ToString();
        string Act_eco_clave = newValues["ACT_ECO_CLAVE"] == null ? null : newValues["ACT_ECO_CLAVE"].ToString();
        string Act_eco = newValues["ACT_ECO"] == null ? null : newValues["ACT_ECO"].ToString();
        string Usuario_alta = newValues["USUARIO_ALTA"] == null ? null : newValues["USUARIO_ALTA"].ToString();
        string Consecutivo = string.Empty;
        if (newValues["CONSECUTIVO"] != null)
        {
            Consecutivo = newValues["CONSECUTIVO"].ToString();
        }
        string Estatus = newValues["ESTATUS"] == null ? null : newValues["ESTATUS"].ToString();
        string Id_tipo_cliente = string.Empty;
        if (newValues["ID_TIPO_CLIENTE"] != null)
        {
            Id_tipo_cliente = newValues["ID_TIPO_CLIENTE"].ToString();
        }
        ClientesPM clienteNew = new ClientesPM(Rfc, Curp, Nombre, Apellido_paterno, Apellido_materno, Nacionalidad_clave, Nacionalidad, Calle, Num_ext, Num_int, Colonia, Municipio_clave, Municipio, Ciudad, Estado_clave, Estado, Codigo_postal, Telefonos, Pais_clave, Pais, Tipo_cliente_clave, Tipo_cliente, Compania, Act_eco_clave, Act_eco, Usuario_alta, Consecutivo, Estatus, Id_tipo_cliente);
        try
        {
            if (clienteNew.Rfc != null && clienteNew.Rfc.ToString().Trim() != "")
            {
                if (validaCampoNumerico(1, clienteNew.Consecutivo, clienteNew.Id_tipo_cliente))
                {

                    ClientesPM_rules ClienteValores = new ClientesPM_rules();

                    if (ClienteValores.Insert(clienteNew) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se insertó correctamente");
                        //ActividadRules.GActividadCatalogo(8888, idUs, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue insertado");
                        //RadAjaxManager1.ResponseScripts.Add("radalert('El RFC es obligatorio.', 320, 100);");
                        //index = e.Item.ItemIndex;

                    }
                }
            }
            else // el RFC viene nulo o vacio
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue insertado debido a que el campo del RFC es obligatorio, por favor verifique y vuelva a intentarlo.");
            }

        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);

        }
        finally
        {
            //RegresarPropiedades();
        }

    }
    protected void RgdCuentas_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                //item["TipoPersonaTemp"].Text = item["Persona"].Text;
                //item["Clasificacion"].Text = item["descripcionClasificacion"].Text;
                //item["FormatSicofin"].Text = string.Format("'{0}", item["FormatSicofin"].Text);
                //item["Codigo"].Text = string.Format("'{0}", item["Codigo"].Text);
                ////JAGH 
                //Label lblTipoCuenta = (Label)item.FindControl("lblTipoCuenta");

                //if (item["tipoc"] != null)
                //{

                //    if (item["tipoc"].Text == "C")
                //    {
                //        item["tipoc"].Text = "Consumo";
                //        lblTipoCuenta.Text = "Consumo";
                //    }
                //    else
                //    {
                //        item["tipoc"].Text = "Hipotecario";
                //        lblTipoCuenta.Text = "Hipotecario";
                //    }
                //}

                //this.RgdCuentas.MasterTableView.GetColumn("Clasificacion").Visible = true;
                //this.RgdCuentas.MasterTableView.GetColumn("descripcionClasificacion").Visible = false;


            }
            else if (e.Item.IsInEditMode) //(e.Item.ItemType == GridItemType.CommandItem)
            {
                GridDataItem items;

                items = (GridDataItem)e.Item;
                //RadComboBox comboP;
                //comboP = (RadComboBox)items["TipoPersonaTemp"].FindControl("ComboPersona");
                //if (Request.QueryString["Persona"].ToString() == "PF")
                //    comboP.SelectedIndex = 0;
                //else
                //    comboP.SelectedIndex = 1;
                //comboP.Enabled = false;

                ////JAGH 01/03/13 se modifica para realizar correctamente actualizacion
                //Label lblClasificacion = (Label)items.FindControl("lblClasificacion");
                //RadComboBox ComboClasificacion = (RadComboBox)items.FindControl("ComboClasificacion");
                //lblClasificacion.Visible = false;
                //ComboClasificacion.Visible = true;
                //this.RgdCuentas.MasterTableView.GetColumn("Clasificacion").Visible = true;
                //this.RgdCuentas.MasterTableView.GetColumn("descripcionClasificacion").Visible = false;

                //JAGH 22/02/13 se modifica para mostrar solo combo al editar y deshabilitar para PM
                //Label lblTipoCuenta = (Label)items.FindControl("lblTipoCuenta");
                //RadComboBox ComboTipoCredito = (RadComboBox)items.FindControl("ComboTipoCredito");
                //lblTipoCuenta.Visible = false;
                //ComboTipoCredito.Visible = true;

                //this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = true;
                //this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = true;
                //this.RgdCuentas.MasterTableView.GetColumn("tipoc").Visible = false;

                //MAMR 28/02/13 
                //if (Request.QueryString["Persona"].ToString() == "PM")
                //{
                //    ComboTipoCredito.SelectedIndex = 0;
                //    ComboTipoCredito.Enabled = false;
                //    this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Visible = false;
                //    this.RgdCuentas.MasterTableView.GetColumn("TipoCredito").Display = false;
                //}
                //fin se modifica para mostrar solo combo al editar y deshabilitar para PM

                //ClasificacionRules catalogo;
                //RadComboBox comboClasif;
                GridDataItem item;

                item = (GridDataItem)e.Item;
                //RadComboBox comboPersona;
                //comboPersona = (RadComboBox)item["TipoPersonaTemp"].FindControl("ComboPersona");
                ////comboEstatus = (RadComboBox)item["EstatusTemp"].FindControl("ComboEstatus");
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Hashtable newValues = new Hashtable();
                editedItem.ExtractValues(newValues);

                //catalogo = new ClasificacionRules();
                //comboClasif = (RadComboBox)item["Clasificacion"].FindControl("ComboClasificacion");

                //comboClasif.DataSource = catalogo.GetRecords(false);
                //comboClasif.DataTextField = "Descripcion";
                //comboClasif.DataValueField = "Id";
                //comboClasif.DataBind();

                //if (newValues["Persona"] != null)
                //{
                //    //if (newValues["Estatus"].ToString() == "Activo")
                //    //    comboEstatus.SelectedIndex = 0;
                //    //else
                //    //    comboEstatus.SelectedIndex = 1;
                //    if (newValues["Persona"].ToString() == "Fisica")
                //        comboPersona.SelectedIndex = 0;
                //    else
                //        comboPersona.SelectedIndex = 1;
                //    comboPersona.Enabled = false;

                //    comboClasif.SelectedItem.Text = newValues["descripcionClasificacion"].ToString();
                //    if (newValues["tipoCuenta"] != null)
                //    {
                //        if (newValues["tipoCuenta"].ToString() == "H")
                //            ((RadComboBox)item["TipoCredito"].FindControl("ComboTipoCredito")).SelectedIndex = 0;
                //        else
                //            ((RadComboBox)item["TipoCredito"].FindControl("ComboTipoCredito")).SelectedIndex = 1;
                //    }
                //}
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
    public bool validaCampoNumerico(int type, string txt_1, string txt_2)
    {
        //1  Numerico
        bool valido = false;
        switch (type)
        {
            case 1:
                if (txt_1.Length > 0) //se permiten nulos
                {
                    for (int n = 0; n < txt_1.Length; n++)
                    {
                        if (!Char.IsNumber(txt_1, n))
                        {
                            valido = false;
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en los campos Consecutivo y Tipo de Cliente deben ser numéricos, favor de verificar");
                            break;
                        }
                        else
                        {
                            valido = true;
                        }
                    }
                }
                else
                {
                    valido = true;
                }
                if (valido)
                {
                    if (txt_2.Length > 0) //se permiten nulos
                    {

                        for (int n = 0; n < txt_2.Length; n++)
                        {
                            if (!Char.IsNumber(txt_2, n))
                            {
                                valido = false;
                                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en los campos Consecutivo y Tipo de Cliente deben ser numéricos, favor de verificar");
                                break;
                            }
                            else
                            {
                                valido = true;
                            }
                        }
                    }
                    else
                    {
                        valido = true;
                    }

                }


                break;

            case 2:

                break;
        }
        return valido;
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
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e) // exportar a excel
    {

        Response.ContentType = "application/force-download";
        RgdCuentas.Columns[0].Visible = false;
        RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
        RgdCuentas.MasterTableView.GetColumn("APELLIDO_PATERNO").HeaderText = "APELLIDO_PATERNO";
        RgdCuentas.MasterTableView.GetColumn("APELLIDO_MATERNO").HeaderText = "APELLIDO_MATERNO";
        RgdCuentas.MasterTableView.GetColumn("NACIONALIDAD_CLAVE").HeaderText = "NACIONALIDAD_CLAVE";
        RgdCuentas.MasterTableView.GetColumn("NUM_EXT").HeaderText = "NUM_EXT";
        RgdCuentas.MasterTableView.GetColumn("NUM_INT").HeaderText = "NUM_INT";
        RgdCuentas.MasterTableView.GetColumn("MUNICIPIO_CLAVE").HeaderText = "MUNICIPIO_CLAVE";
        RgdCuentas.MasterTableView.GetColumn("ESTADO_CLAVE").HeaderText = "ESTADO_CLAVE";
        RgdCuentas.MasterTableView.GetColumn("CODIGO_POSTAL").HeaderText = "CODIGO_POSTAL";
        RgdCuentas.MasterTableView.GetColumn("TELEFONOS").HeaderText = "TELEFONOS";
        RgdCuentas.MasterTableView.GetColumn("PAIS_CLAVE").HeaderText = "PAIS_CLAVE";
        RgdCuentas.MasterTableView.GetColumn("PAIS").HeaderText = "PAIS";
        RgdCuentas.MasterTableView.GetColumn("TIPO_CLIENTE_CLAVE").HeaderText = "TIPO_CLIENTE_CLAVE";
        RgdCuentas.MasterTableView.GetColumn("TIPO_CLIENTE").HeaderText = "TIPO_CLIENTE";
        RgdCuentas.MasterTableView.GetColumn("COMPANIA").HeaderText = "COMPANIA";
        RgdCuentas.MasterTableView.GetColumn("ACT_ECO_CLAVE").HeaderText = "ACT_ECO_CLAVE";
        RgdCuentas.MasterTableView.GetColumn("ACT_ECO").HeaderText = "ACT_ECO";
        RgdCuentas.MasterTableView.GetColumn("USUARIO_ALTA").HeaderText = "USUARIO_ALTA";
        RgdCuentas.MasterTableView.GetColumn("ID_TIPO_CLIENTE").HeaderText = "ID_TIPO_CLIENTE";

        RgdCuentas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = false;
        RgdCuentas.ExportSettings.ExportOnlyData = true;
        RgdCuentas.ExportSettings.IgnorePaging = true;
        RgdCuentas.ExportSettings.OpenInNewWindow = true;
        RgdCuentas.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exportó el Catálogo " + catalog);

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

                var columns = RgdCuentas.MasterTableView.RenderColumns;
                foreach (GridColumn column in columns)
                {
                    //TODO: HAY DOS COLUMNAS EXTRAS A LAS BOUND COLUMNS, CHECAR EL TIPO Y NO INCLUIRLAS  
                    if (column.ColumnType != "GridExpandColumn" && column.ColumnType != "GridRowIndicatorColumn" && column.ColumnType != "GridEditCommandColumn" && column.ColumnType != "GridClientSelectColumn" && column.ColumnType != "GridTemplateColumn")
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

                string storeBase = "PACKCLIENTES.SP_INSERTCTESTMP";

                dt_layout_procesado = cargaMasiva.cargaMasiva("clientes tmp", dt_metaDataLayout, ruta_archivo, " * ", parametros.ToArray(), storeBase, "PM");
                int numeros = cargaMasiva.Correctos;

                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdCuentas.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog + " ", total + " Registros", numeros + " Registros", catalog, 1);

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

                ClientesPM_rules ClienteValores = new ClientesPM_rules();

                if (ClienteValores.DeleteAll() > 0)
                {
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Todos los registros del catálogo fueron removidos");
                    ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdCuentas.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los registros no fueron removidos.");
                    //RadAjaxManager1.ResponseScripts.Add("radalert('El RFC es obligatorio.', 320, 100);");
                    //index = e.Item.ItemIndex;
                }

                RgdCuentas.Rebind();
                RgdCuentas.DataSource = null;
                RgdCuentas.AllowPaging = true;
                RgdCuentas.Rebind();
                RgdCuentas.DataBind();

            }//header
            else
            {

                foreach (GridDataItem row in RgdCuentas.Items)
                {

                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;
                    string RFC = row.Cells[3].Text;
                    if (chkResult)
                    {
                        ClientesPM_rules ClienteValores = new ClientesPM_rules();

                        if (ClienteValores.Delete(RFC) > 0)
                        {
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            Mensajes.ShowMessage(this.Page, this.GetType(), "Se removieron correctamente los registros seleccionados.");
                            ActividadRules.GActividadCatalogo(1111, idUs, "Eliminación de Registro con RFC: " + RFC.ToString(), null, null, catalog, 1, null, null);
                        }
                        else
                        {
                            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los registros seleccionados no fueron removidos.");
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
            }
        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }//try-catch


        //ChkTodo_CheckedChanged
    }
 
}