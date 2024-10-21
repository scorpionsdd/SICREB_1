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

public partial class AvalesPage : System.Web.UI.Page
{

    int idUsuario;
    string persona;
    public const String catalog = "Avales";

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


    protected void RgdAvales_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        AvalRules getRecords = null;
        List<Aval> AvalesInfo;

        try
        {
            getRecords = new AvalRules(Enums.Persona.Moral);
            var s = getRecords.GetRecords(false);
            AvalesInfo = s;
            RgdAvales.DataSource = AvalesInfo;
            RgdAvales.VirtualItemCount = s.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdAvales_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                item["TipoPersonaTemp"].Text = item["Persona"].Text;
                item["TipoOperacionTemp"].Text = item["Operacion"].Text;
            }
            
            if (e.Item.IsInEditMode)
            {
                GridDataItem item = (GridDataItem)e.Item;
                Aval ObjAval = (Aval)item.DataItem;

                RadComboBox comboPersona;
                comboPersona = (RadComboBox)item["TipoPersonaTemp"].FindControl("ComboPersona");

                RadComboBox comboOperacion;
                comboOperacion = (RadComboBox)item["TipoOperacionTemp"].FindControl("ComboOperacion");

                if (ObjAval.TipoAval == Enums.Persona.Moral )
                { comboPersona.SelectedIndex = 0; }
                if (ObjAval.TipoAval == Enums.Persona.Fisica)
                { comboPersona.SelectedIndex = 1; }
                if (ObjAval.TipoAval == Enums.Persona.Fideicomiso)
                { comboPersona.SelectedIndex = 2; }
                if (ObjAval.TipoAval == Enums.Persona.Gobierno)
                { comboPersona.SelectedIndex = 3; }

                if (ObjAval.TipoOperacion == Enums.TipoOperacion.Credito)
                { comboOperacion.SelectedIndex = 1; }
                else
                { comboOperacion.SelectedIndex = 0; }  
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

    protected void RgdAvales_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            // Quitamos los nulos para evitar error en el Hashtable
            if (newValues["Id"] == null) { newValues["Id"] = "0"; }
            if (newValues["RfcAcreditado"] == null) { newValues["RfcAcreditado"] = string.Empty; }
            if (newValues["Credito"] == null) { newValues["Credito"] = string.Empty; }
            if (newValues["RfcAval"] == null) { newValues["RfcAval"] = string.Empty; }
            if (newValues["NombreCompania"] == null) { newValues["NombreCompania"] = string.Empty; }
            if (newValues["Nombre"] == null) { newValues["Nombre"] = string.Empty; }
            if (newValues["SNombre"] == null) { newValues["SNombre"] = string.Empty; }
            if (newValues["ApellidoP"] == null) { newValues["ApellidoP"] = string.Empty; }
            if (newValues["ApellidoM"] == null) { newValues["ApellidoM"] = string.Empty; }
            if (newValues["Direccion"] == null) { newValues["Direccion"] = string.Empty; }
            if (newValues["ColoniaPoblacion"] == null) { newValues["ColoniaPoblacion"] = string.Empty; }
            if (newValues["DelegacionMunicipio"] == null) { newValues["DelegacionMunicipio"] = string.Empty; }
            if (newValues["Ciudad"] == null) { newValues["Ciudad"] = string.Empty; }
            if (newValues["EstadoMexico"] == null) { newValues["EstadoMexico"] = string.Empty; }
            if (newValues["CodigoPostal"] == null) { newValues["CodigoPostal"] = string.Empty; }
            if (newValues["EstadoExtranjero"] == null) { newValues["EstadoExtranjero"] = string.Empty; }
            if (newValues["PaisOrigenDomicilio"] == null) { newValues["PaisOrigenDomicilio"] = "MX"; }
            string cadenaTempN = newValues["PaisOrigenDomicilio"].ToString();
            if (cadenaTempN == string.Empty) { newValues["PaisOrigenDomicilio"] = "MX"; }
            if (newValues["TipoAval"] == null) { newValues["TipoAval"] = string.Empty; }
            if (newValues["TipoOperacion"] == null) { newValues["TipoOperacion"] = string.Empty; }

            Aval avalNew;
            AvalRules AvalAgregar = new AvalRules(Enums.Persona.Moral);
            Enums.Estado estado = Enums.Estado.Activo;
            Enums.Persona persona = Enums.Persona.Moral;
            Enums.TipoOperacion operacion = Enums.TipoOperacion.Credito;
            ArrayList TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));
            ArrayList TipoOperacion = Util.RadComboToString(editedItem["TipoOperacionTemp"].FindControl("ComboOperacion"));

            switch (TipoPersona[0].ToString().ToUpper().Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U'))
            {
                case "MORAL":
                    persona = Enums.Persona.Moral;
                    newValues["TipoAval"] = "MORAL";
                    break;
                case "FISICA":
                    persona = Enums.Persona.Fisica;
                    newValues["TipoAval"] = "FISICA";
                    break;
                case "FONDO O FIDEICOMISO":
                    persona = Enums.Persona.Fideicomiso;
                    newValues["TipoAval"] = "FONDO O FIDEICOMISO";
                    break;
                case "GOBIERNO":
                    persona = Enums.Persona.Gobierno;
                    newValues["TipoAval"] = "GOBIERNO";
                    break;
                default:
                    persona = Enums.Persona.Moral;
                    newValues["TipoAval"] = "MORAL";
                    break;
            }

            switch (TipoOperacion[0].ToString().ToUpper())
            {
                case "CREDITO":
                    operacion = Enums.TipoOperacion.Credito;
                    newValues["TipoOperacion"] = "CREDITO";
                    break;
                case "LINEA":
                    operacion = Enums.TipoOperacion.Linea;
                    newValues["TipoOperacion"] = "LINEA";
                    break;
                default:
                    operacion = Enums.TipoOperacion.Credito;
                    newValues["TipoOperacion"] = "CREDITO";
                    break;
            }

            if (ValidarDatosAval(newValues) == true)
            {

                avalNew = new Aval(0,
                                    newValues["Credito"].ToString().ToUpper(),
                                    newValues["RfcAcreditado"].ToString().ToUpper(),
                                    newValues["RfcAval"].ToString().ToUpper(),
                                    newValues["NombreCompania"].ToString().ToUpper(),
                                    newValues["Nombre"].ToString().ToUpper(),
                                    newValues["SNombre"].ToString().ToUpper(),
                                    newValues["ApellidoP"].ToString().ToUpper(),
                                    newValues["ApellidoM"].ToString().ToUpper(),
                                    newValues["Direccion"].ToString().ToUpper(),
                                    newValues["ColoniaPoblacion"].ToString().ToUpper(),
                                    newValues["DelegacionMunicipio"].ToString().ToUpper(),
                                    newValues["Ciudad"].ToString().ToUpper(),
                                    newValues["EstadoMexico"].ToString().ToUpper(),
                                    newValues["CodigoPostal"].ToString().ToUpper(),
                                    persona,
                                    newValues["EstadoExtranjero"].ToString().ToUpper(),
                                    newValues["PaisOrigenDomicilio"].ToString().ToUpper(),
                                    operacion,
                                    estado);

                if (AvalAgregar.Insert(avalNew) > 0)
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

    protected void RgdAvales_UpdateCommand(object source, GridCommandEventArgs e)
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
            if (oldValues["Credito"] == null) { oldValues["Credito"] = string.Empty; }
            if (oldValues["RfcAval"] == null) { oldValues["RfcAval"] = string.Empty; }
            if (oldValues["NombreCompania"] == null) { oldValues["NombreCompania"] = string.Empty; }
            if (oldValues["Nombre"] == null) { oldValues["Nombre"] = string.Empty; }
            if (oldValues["SNombre"] == null) { oldValues["SNombre"] = string.Empty; }
            if (oldValues["ApellidoP"] == null) { oldValues["ApellidoP"] = string.Empty; }
            if (oldValues["ApellidoM"] == null) { oldValues["ApellidoM"] = string.Empty; }
            if (oldValues["Direccion"] == null) { oldValues["Direccion"] = string.Empty; }
            if (oldValues["ColoniaPoblacion"] == null) { oldValues["ColoniaPoblacion"] = string.Empty; }
            if (oldValues["DelegacionMunicipio"] == null) { oldValues["DelegacionMunicipio"] = string.Empty; }
            if (oldValues["Ciudad"] == null) { oldValues["Ciudad"] = string.Empty; }
            if (oldValues["EstadoMexico"] == null) { oldValues["EstadoMexico"] = string.Empty; }
            if (oldValues["CodigoPostal"] == null) { oldValues["CodigoPostal"] = string.Empty; }
            if (oldValues["EstadoExtranjero"] == null) { oldValues["EstadoExtranjero"] = string.Empty; }
            if (oldValues["PaisOrigenDomicilio"] == null) { oldValues["PaisOrigenDomicilio"] = string.Empty; }
            string cadenaTempO = oldValues["PaisOrigenDomicilio"].ToString();
            if (cadenaTempO == string.Empty) { oldValues["PaisOrigenDomicilio"] = "MX"; }
            if (oldValues["TipoAval"] == null) { oldValues["TipoAval"] = string.Empty; }
            if (oldValues["TipoOperacion"] == null) { oldValues["TipoOperacion"] = string.Empty; }
        
            Int32 idAval = Parser.ToNumber(item.SavedOldValues["Id"]);
            newValues["Id"] = idAval;

            if (newValues["RfcAcreditado"] == null) { newValues["RfcAcreditado"] = string.Empty; }
            if (newValues["Credito"] == null) { newValues["Credito"] = string.Empty; }
            if (newValues["RfcAval"] == null) { newValues["RfcAval"] = string.Empty; }
            if (newValues["NombreCompania"] == null) { newValues["NombreCompania"] = string.Empty; }
            if (newValues["Nombre"] == null) { newValues["Nombre"] = string.Empty; }
            if (newValues["SNombre"] == null) { newValues["SNombre"] = string.Empty; }
            if (newValues["ApellidoP"] == null) { newValues["ApellidoP"] = string.Empty; }
            if (newValues["ApellidoM"] == null) { newValues["ApellidoM"] = string.Empty; }
            if (newValues["Direccion"] == null) { newValues["Direccion"] = string.Empty; }
            if (newValues["ColoniaPoblacion"] == null) { newValues["ColoniaPoblacion"] = string.Empty; }
            if (newValues["DelegacionMunicipio"] == null) { newValues["DelegacionMunicipio"] = string.Empty; }
            if (newValues["Ciudad"] == null) { newValues["Ciudad"] = string.Empty; }
            if (newValues["EstadoMexico"] == null) { newValues["EstadoMexico"] = string.Empty; }
            if (newValues["CodigoPostal"] == null) { newValues["CodigoPostal"] = string.Empty; }
            if (newValues["EstadoExtranjero"] == null) { newValues["EstadoExtranjero"] = string.Empty; }
            if (newValues["PaisOrigenDomicilio"] == null) { newValues["PaisOrigenDomicilio"] = string.Empty; }
            string cadenaTempN = newValues["PaisOrigenDomicilio"].ToString();
            if (cadenaTempN == string.Empty) { newValues["PaisOrigenDomicilio"] = "MX"; }
            if (newValues["TipoAval"] == null) { newValues["TipoAval"] = string.Empty; }
            if (newValues["TipoOperacion"] == null) { newValues["TipoOperacion"] = string.Empty; }

            Aval avalOld;
            Aval avalNew;
            AvalRules AvalActualizar = new AvalRules(Enums.Persona.Moral);
            Enums.Estado estado = Enums.Estado.Activo;
            Enums.Persona persona = Enums.Persona.Moral;
            Enums.TipoOperacion operacion = Enums.TipoOperacion.Credito;
            ArrayList TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));
            ArrayList TipoOperacion = Util.RadComboToString(editedItem["TipoOperacionTemp"].FindControl("ComboOperacion"));

            //if (TipoPersona[0].ToString() == "Moral" || TipoPersona[0].ToString() == "moral")
            //{
            //    persona = Enums.Persona.Moral;
            //    newValues["TipoAval"] = "MORAL";
            //}
            //else
            //{
            //    persona = Enums.Persona.Fisica;
            //    newValues["TipoAval"] = "FISICA";
            //}

            switch (TipoPersona[0].ToString().ToUpper().Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U'))
            {
                case "MORAL":
                    persona = Enums.Persona.Moral;
                    newValues["TipoAval"] = "MORAL";
                    break;
                case "FISICA":
                    persona = Enums.Persona.Fisica;
                    newValues["TipoAval"] = "FISICA";
                    break;
                case "FONDO O FIDEICOMISO":
                    persona = Enums.Persona.Fideicomiso;
                    newValues["TipoAval"] = "FONDO O FIDEICOMISO";
                    break;
                case "GOBIERNO":
                    persona = Enums.Persona.Gobierno;
                    newValues["TipoAval"] = "GOBIERNO";
                    break;
                default:
                    persona = Enums.Persona.Moral;
                    newValues["TipoAval"] = "MORAL";
                    break;
            }

            switch (TipoOperacion[0].ToString().ToUpper())
            {
                case "CREDITO":
                    operacion = Enums.TipoOperacion.Credito;
                    newValues["TipoOperacion"] = "CREDITO"; 
                    break;
                case "LINEA":
                    operacion = Enums.TipoOperacion.Linea;
                    newValues["TipoOperacion"] = "LINEA";
                    break;
                default:
                    operacion = Enums.TipoOperacion.Credito;
                    newValues["TipoOperacion"] = "CREDITO"; 
                    break;
            }

            if (ValidarDatosAval(newValues) == true)
            {

                avalOld = new Aval(idAval,
                                    oldValues["Credito"].ToString().ToUpper(),
                                    oldValues["RfcAcreditado"].ToString().ToUpper(),
                                    oldValues["RfcAval"].ToString().ToUpper(),
                                    oldValues["NombreCompania"].ToString().ToUpper(),
                                    oldValues["Nombre"].ToString().ToUpper(),
                                    oldValues["SNombre"].ToString().ToUpper(),
                                    oldValues["ApellidoP"].ToString().ToUpper(),
                                    oldValues["ApellidoM"].ToString().ToUpper(),
                                    oldValues["Direccion"].ToString().ToUpper(),
                                    oldValues["ColoniaPoblacion"].ToString().ToUpper(),
                                    oldValues["DelegacionMunicipio"].ToString().ToUpper(),
                                    oldValues["Ciudad"].ToString().ToUpper(),
                                    oldValues["EstadoMexico"].ToString().ToUpper(),
                                    oldValues["CodigoPostal"].ToString().ToUpper(),
                                    persona,
                                    oldValues["EstadoExtranjero"].ToString().ToUpper(),
                                    oldValues["PaisOrigenDomicilio"].ToString().ToUpper(),
                                    operacion,
                                    estado);

                avalNew = new Aval(idAval,
                                    newValues["Credito"].ToString().ToUpper(),
                                    newValues["RfcAcreditado"].ToString().ToUpper(),
                                    newValues["RfcAval"].ToString().ToUpper(),
                                    newValues["NombreCompania"].ToString().ToUpper(),
                                    newValues["Nombre"].ToString().ToUpper(),
                                    newValues["SNombre"].ToString().ToUpper(),
                                    newValues["ApellidoP"].ToString().ToUpper(),
                                    newValues["ApellidoM"].ToString().ToUpper(),
                                    newValues["Direccion"].ToString().ToUpper(),
                                    newValues["ColoniaPoblacion"].ToString().ToUpper(),
                                    newValues["DelegacionMunicipio"].ToString().ToUpper(),
                                    newValues["Ciudad"].ToString().ToUpper(),
                                    newValues["EstadoMexico"].ToString().ToUpper(),
                                    newValues["CodigoPostal"].ToString().ToUpper(),
                                    persona,
                                    newValues["EstadoExtranjero"].ToString().ToUpper(),
                                    newValues["PaisOrigenDomicilio"].ToString().ToUpper(),
                                    operacion,
                                    estado);

                if (AvalActualizar.Update(avalOld, avalNew) > 0)
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
                row1["nombreColumna"] = "Tipo de Operacion";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "25";

                DataRow row2;
                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Credito";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "25";

                DataRow row3;
                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "RFC Acreditado";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "15";

                DataRow row4;
                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "RFC Aval";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "15";

                DataRow row5;
                row5 = dt_metaDataLayout.NewRow();
                row5["nombreColumna"] = "Nombre Compañia";
                row5["tipoDato"] = "VARCHAR2";
                row5["longitud"] = "150";

                DataRow row6;
                row6 = dt_metaDataLayout.NewRow();
                row6["nombreColumna"] = "Nombre";
                row6["tipoDato"] = "VARCHAR2";
                row6["longitud"] = "50";

                DataRow row7;
                row7 = dt_metaDataLayout.NewRow();
                row7["nombreColumna"] = "Segundo Nombre";
                row7["tipoDato"] = "VARCHAR2";
                row7["longitud"] = "50";

                DataRow row8;
                row8 = dt_metaDataLayout.NewRow();
                row8["nombreColumna"] = "Apellido Paterno";
                row8["tipoDato"] = "VARCHAR2";
                row8["longitud"] = "50";

                DataRow row9;
                row9 = dt_metaDataLayout.NewRow();
                row9["nombreColumna"] = "Apellido Materno";
                row9["tipoDato"] = "VARCHAR2";
                row9["longitud"] = "50";

                DataRow row10;
                row10 = dt_metaDataLayout.NewRow();
                row10["nombreColumna"] = "Direccion";
                row10["tipoDato"] = "VARCHAR2";
                row10["longitud"] = "50";

                DataRow row11;
                row11 = dt_metaDataLayout.NewRow();
                row11["nombreColumna"] = "Colonia o Poblacion";
                row11["tipoDato"] = "VARCHAR2";
                row11["longitud"] = "60";

                DataRow row12;
                row12 = dt_metaDataLayout.NewRow();
                row12["nombreColumna"] = "Delegacion o Municipio";
                row12["tipoDato"] = "VARCHAR2";
                row12["longitud"] = "50";

                DataRow row13;
                row13 = dt_metaDataLayout.NewRow();
                row13["nombreColumna"] = "Ciudad";
                row13["tipoDato"] = "VARCHAR2";
                row13["longitud"] = "50";

                DataRow row14;
                row14 = dt_metaDataLayout.NewRow();
                row14["nombreColumna"] = "Estado (En Mexico)";
                row14["tipoDato"] = "VARCHAR2";
                row14["longitud"] = "5";

                DataRow row15;
                row15 = dt_metaDataLayout.NewRow();
                row15["nombreColumna"] = "Codigo Postal";
                row15["tipoDato"] = "VARCHAR2";
                row15["longitud"] = "10";

                DataRow row16;
                row16 = dt_metaDataLayout.NewRow();
                row16["nombreColumna"] = "Tipo Aval";
                row16["tipoDato"] = "VARCHAR2";
                row16["longitud"] = "15";

                DataRow row17;
                row17 = dt_metaDataLayout.NewRow();
                row17["nombreColumna"] = "Estado (En el extranjero)";
                row17["tipoDato"] = "VARCHAR2";
                row17["longitud"] = "50";

                DataRow row18;
                row18 = dt_metaDataLayout.NewRow();
                row18["nombreColumna"] = "Pais de origen del domicilio";
                row18["tipoDato"] = "VARCHAR2";
                row18["longitud"] = "2";

                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);
                dt_metaDataLayout.Rows.Add(row4);
                dt_metaDataLayout.Rows.Add(row5);
                dt_metaDataLayout.Rows.Add(row6);
                dt_metaDataLayout.Rows.Add(row7);
                dt_metaDataLayout.Rows.Add(row8);
                dt_metaDataLayout.Rows.Add(row9);
                dt_metaDataLayout.Rows.Add(row10);
                dt_metaDataLayout.Rows.Add(row11);
                dt_metaDataLayout.Rows.Add(row12);
                dt_metaDataLayout.Rows.Add(row13);
                dt_metaDataLayout.Rows.Add(row14);
                dt_metaDataLayout.Rows.Add(row15);
                dt_metaDataLayout.Rows.Add(row16);
                dt_metaDataLayout.Rows.Add(row17);
                dt_metaDataLayout.Rows.Add(row18);

                DbParameter[] parametros = new DbParameter[21];
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
                parametros[3].ParameterName = "TipoOperacionP";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 25;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "CreditoP";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 25;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "rfcAcreditadoP";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 15;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "rfcAvalP";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 15;

                parametros[7] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].ParameterName = "NombreCompaniaP";
                parametros[7].DbType = DbType.String;
                parametros[7].Size = 150;

                parametros[8] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[8].Direction = ParameterDirection.Input;
                parametros[8].ParameterName = "NombreP";
                parametros[8].DbType = DbType.String;
                parametros[8].Size = 50;

                parametros[9] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[9].Direction = ParameterDirection.Input;
                parametros[9].ParameterName = "SNombreP";
                parametros[9].DbType = DbType.String;
                parametros[9].Size = 50;

                parametros[10] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[10].Direction = ParameterDirection.Input;
                parametros[10].ParameterName = "APaternoP";
                parametros[10].DbType = DbType.String;
                parametros[10].Size = 50;

                parametros[11] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[11].Direction = ParameterDirection.Input;
                parametros[11].ParameterName = "AMaternoP";
                parametros[11].DbType = DbType.String;
                parametros[11].Size = 50;

                parametros[12] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[12].Direction = ParameterDirection.Input;
                parametros[12].ParameterName = "DireccionP";
                parametros[12].DbType = DbType.String;
                parametros[12].Size = 50;

                parametros[13] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[13].Direction = ParameterDirection.Input;
                parametros[13].ParameterName = "ColoniaPoblacionP";
                parametros[13].DbType = DbType.String;
                parametros[13].Size = 60;

                parametros[14] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[14].Direction = ParameterDirection.Input;
                parametros[14].ParameterName = "DelegacionMunicipioP";
                parametros[14].DbType = DbType.String;
                parametros[14].Size = 50;

                parametros[15] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[15].Direction = ParameterDirection.Input;
                parametros[15].ParameterName = "CiudadP";
                parametros[15].DbType = DbType.String;
                parametros[15].Size = 50;

                parametros[16] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[16].Direction = ParameterDirection.Input;
                parametros[16].ParameterName = "EstadoMexicoP";
                parametros[16].DbType = DbType.String;
                parametros[16].Size = 5;

                parametros[17] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[17].Direction = ParameterDirection.Input;
                parametros[17].ParameterName = "CodigoPostalP";
                parametros[17].DbType = DbType.String;
                parametros[17].Size = 10;

                parametros[18] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[18].Direction = ParameterDirection.Input;
                parametros[18].ParameterName = "TipoAvalP";
                parametros[18].DbType = DbType.String;
                parametros[18].Size = 10;

                parametros[19] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[19].Direction = ParameterDirection.Input;
                parametros[19].ParameterName = "EstadoExtranjeroP";
                parametros[19].DbType = DbType.String;
                parametros[19].Size = 50;

                parametros[20] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[20].Direction = ParameterDirection.Input;
                parametros[20].ParameterName = "PaisOrigenDomicilioP";
                parametros[20].DbType = DbType.String;
                parametros[20].Size = 2;

                string storeBase = "SP_CARGAMASIVA_AVAL";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_avales", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                }

                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdAvales.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                RgdAvales.Rebind();
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
            GridHeaderItem headerItem = RgdAvales.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");

            if (chkHeader.Checked == true)
            {
                int RegistrosEliminados = 0;
                RgdAvales.AllowPaging = false;
                RgdAvales.Rebind();

                foreach (GridDataItem row in RgdAvales.Items)
                {
                    Aval AvalDelete;
                    Int32 idAval = 0;
                    string numCredito = string.Empty;
                    string rfcAcreditado = string.Empty;
                    string rfcAval = string.Empty;

                    //Datos para identificar valor

                    idAval = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    numCredito = row["Credito"].Text.ToString();
                    rfcAcreditado = row["RfcAcreditado"].Text.ToString();
                    rfcAval = row["RfcAval"].Text.ToString();

                    AvalDelete = new Aval(idAval, numCredito, rfcAcreditado, rfcAval, string.Empty, string.Empty, string.Empty, string.Empty,
                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Enums.Persona.Moral, string.Empty, string.Empty, Enums.TipoOperacion.Credito, Enums.Estado.Activo);

                    //Llamar el SP correspondiente con las entidades old y new de validacion
                    AvalRules AvalBorrar = new AvalRules(Enums.Persona.Moral);

                    if (AvalBorrar.Delete(AvalDelete) > 0)
                    {
                        RegistrosEliminados++;  
                    }
                }

                if (RgdAvales.Items.Count == RegistrosEliminados) 
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Registros removidos correctamente");
                }
                else
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), RegistrosEliminados + " registros de " + RgdAvales.Items.Count + " removidos correctamente");
                }

                RgdAvales.Rebind();
                RgdAvales.DataSource = null;
                RgdAvales.AllowPaging = true;
                RgdAvales.Rebind();
                RgdAvales.DataBind();
                idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUsuario, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdAvales.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
            }
            else
            {
                int RegistrosEliminados = 0;

                foreach (GridDataItem row in RgdAvales.Items)
                {
                    CheckBox chkCell;
                    bool chkResult;
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;

                    if (chkResult)
                    {

                        Aval AvalDelete;
                        Int32 idAval = 0;
                        string numCredito = string.Empty;
                        string rfcAcreditado = string.Empty;
                        string rfcAval = string.Empty;

                        //Datos para identificar valor

                        idAval = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                        numCredito = row["Credito"].Text.ToString();
                        rfcAcreditado = row["RfcAcreditado"].Text.ToString();
                        rfcAval = row["RfcAval"].Text.ToString();

                        AvalDelete = new Aval(idAval, numCredito, rfcAcreditado, rfcAval, string.Empty, string.Empty, string.Empty, string.Empty,
                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Enums.Persona.Moral, string.Empty, string.Empty, Enums.TipoOperacion.Credito, Enums.Estado.Activo);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        AvalRules AvalBorrar = new AvalRules(Enums.Persona.Moral);

                        if (AvalBorrar.Delete(AvalDelete) > 0)
                        {
                            RegistrosEliminados++;
                            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + idAval, null, null, catalog, 1, null, null);
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

        RgdAvales.Rebind();
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdAvales.Columns[0].Visible = false;
        RgdAvales.Columns[RgdAvales.Columns.Count - 1].Visible = false;
        RgdAvales.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAvales.ExportSettings.OpenInNewWindow = false;
        RgdAvales.ExportSettings.ExportOnlyData = true;
        RgdAvales.MasterTableView.GridLines = GridLines.Both;
        RgdAvales.ExportSettings.IgnorePaging = true;
        RgdAvales.ExportSettings.OpenInNewWindow = true;
        RgdAvales.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdAvales.ExportSettings.Pdf.PageWidth = Unit.Parse("985mm");
        RgdAvales.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catálogo " + catalog);
    }

    protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdAvales.Columns[0].Visible = false;
        RgdAvales.Columns[RgdAvales.Columns.Count - 1].Visible = false;
        RgdAvales.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAvales.ExportSettings.OpenInNewWindow = false;
        RgdAvales.ExportSettings.ExportOnlyData = true;
        RgdAvales.ExportSettings.IgnorePaging = true;
        RgdAvales.ExportSettings.OpenInNewWindow = true;
        RgdAvales.MasterTableView.ExportToExcel();

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

            foreach (GridDataItem row in RgdAvales.Items)
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_AVAL")))
        {
            valido = true;
        }
        return valido;
    }

    private bool ValidarDatosAval(Hashtable DatosAval)
    {

        // Solo se contemplan las validaciones mas basicas de los datos del catalogo.
        // Las validaciones generales se realizan a la hora de crear el Segmento AV al procesar la informacion de Personas Morales.

        if (DatosAval["RfcAcreditado"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El RFC del acreditado no puede ser nulo");
            return false;
        }

        if (DatosAval["Credito"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El número de credito no puede ser nulo");
            return false;
        }

        if (DatosAval["Credito"].ToString().Trim() == string.Empty)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de ingresar el número de credito");
            return false;
        }

        if (ValidarCampoNumerico(DatosAval["Credito"].ToString().Trim(), "Credito") == false)
        {
            return false;
        }

        if (DatosAval["RfcAval"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El RFC del Aval no puede ser nulo");
            return false;
        }

        if (ValidarRegistroDuplicado(DatosAval["Id"].ToString(), DatosAval["RfcAcreditado"].ToString(), DatosAval["Credito"].ToString(), DatosAval["RfcAval"].ToString(), DatosAval["TipoOperacion"].ToString()) == true)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Ya existe un registro con los mismos datos RFC Acreditado: " + DatosAval["RfcAcreditado"].ToString() + ", RFC Aval: " + DatosAval["RfcAval"].ToString() + ", " + DatosAval["TipoOperacion"].ToString() + ": " + DatosAval["Credito"].ToString());
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

    private bool ValidarRegistroDuplicado(string idRegistro, string RegistroAcreditado, string NumeroCredito, string RegistroAval, string OperacionRegistrada)
    {
        bool Respuesta = false;

        try
        {
            AvalRules getRecords = new AvalRules(Enums.Persona.Moral);
            List<Aval> RegistrosAvales = getRecords.GetRecords(false);

            foreach (Aval ItemAval in RegistrosAvales)
            {

                if (RegistroAcreditado.ToUpper() == ItemAval.RfcAcreditado.ToString().ToUpper() &&
                    NumeroCredito.ToUpper() == ItemAval.Credito.ToString().ToUpper() &&
                    RegistroAval.ToUpper() == ItemAval.RfcAval.ToString().ToUpper() &&
                    OperacionRegistrada.ToUpper() == ItemAval.TipoOperacion.ToString().ToUpper() )
                {
                    if (idRegistro == ItemAval.Id.ToString() && idRegistro.Trim() != "0")
                    {
                        // Si el id del registro es diferente de 0 se trata de la actualizacion de un registro
                        // Si el id del aval encontrado es igual al del registro que esta siendo editado permitimos
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

        this.RgdAvales.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        btnExportarPDF.Visible = false;
        btnExportarExcel.Visible = false;

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_AVAL")))
        {
            this.RgdAvales.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_AVAL")))
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
        GridFilterMenu menu = RgdAvales.FilterMenu;
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