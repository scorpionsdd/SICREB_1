using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using ExcelLibrary.SpreadSheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class AccionistasPage : System.Web.UI.Page
{

    int idUsuario;
    string persona;
    public const String catalog = "Accionistas";

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


    protected void RgdAccionistas_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        AccionistaRules ObtenerAcreditados = null;
        List<Accionista> AccionistasInfo;

        try
        {
            ObtenerAcreditados = new AccionistaRules(Enums.Persona.Moral);
            var listaGenerica = ObtenerAcreditados.GetRecords(false);
            AccionistasInfo = listaGenerica;
            RgdAccionistas.DataSource = AccionistasInfo;
            RgdAccionistas.VirtualItemCount = listaGenerica.Count;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
        }
    }

    protected void RgdAccionistas_ItemDataBound(object sender, GridItemEventArgs e)
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
                Accionista ObjAccionista = (Accionista)item.DataItem;

                RadComboBox comboPersona;
                comboPersona = (RadComboBox)item["TipoPersonaTemp"].FindControl("ComboPersona");

                if (ObjAccionista.Persona == Enums.Persona.Moral)
                { comboPersona.SelectedIndex = 0; }
                if (ObjAccionista.Persona == Enums.Persona.Fisica)
                { comboPersona.SelectedIndex = 1; }
                if (ObjAccionista.Persona == Enums.Persona.Fideicomiso)
                { comboPersona.SelectedIndex = 2; }
                if (ObjAccionista.Persona == Enums.Persona.Gobierno)
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

    protected void RgdAccionistas_InsertCommand(object source, GridCommandEventArgs e)
    {
        try
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            Hashtable newValues = new Hashtable();
            editedItem.ExtractValues(newValues);

            // Quitamos los nulos para evitar error en el Hashtable
            if (newValues["Id"] == null) { newValues["Id"] = "0"; }
            if (newValues["RfcAcreditado"] == null) { newValues["RfcAcreditado"] = string.Empty; }
            if (newValues["RfcAccionista"] == null) { newValues["RfcAccionista"] = string.Empty; }
            if (newValues["NombreCompania"] == null) { newValues["NombreCompania"] = string.Empty; }
            if (newValues["Nombre"] == null) { newValues["Nombre"] = string.Empty; }
            if (newValues["SNombre"] == null) { newValues["SNombre"] = string.Empty; }
            if (newValues["ApellidoP"] == null) { newValues["ApellidoP"] = string.Empty; }
            if (newValues["ApellidoM"] == null) { newValues["ApellidoM"] = string.Empty; }
            if (newValues["Porcentaje"] == null) { newValues["Porcentaje"] = 0; }
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
            if (newValues["Persona"] == null) { newValues["Persona"] = string.Empty; }

            Accionista accionistaNew;
            AccionistaRules AccionistaAgregar = new AccionistaRules(Enums.Persona.Moral);
            Enums.Estado estado = Enums.Estado.Activo;
            Enums.Persona persona = Enums.Persona.Moral;
            ArrayList TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));

            //if (TipoPersona[0].ToString() == "Moral" || TipoPersona[0].ToString() == "moral")
            //{
            //    persona = Enums.Persona.Moral;
            //    newValues["Persona"] = "MORAL";
            //}
            //else
            //{
            //    persona = Enums.Persona.Fisica;
            //    newValues["Persona"] = "FISICA";
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

            if (ValidarDatosAccionista(newValues) == true)
            {

                int porcentajeNew = Parser.ToNumber(newValues["Porcentaje"]);

                accionistaNew = new Accionista(0,
                                    newValues["RfcAcreditado"].ToString().ToUpper(),
                                    newValues["RfcAccionista"].ToString().ToUpper(),
                                    newValues["NombreCompania"].ToString().ToUpper(),
                                    newValues["Nombre"].ToString().ToUpper(),
                                    newValues["SNombre"].ToString().ToUpper(),
                                    newValues["ApellidoP"].ToString().ToUpper(),
                                    newValues["ApellidoM"].ToString().ToUpper(),
                                    porcentajeNew,
                                    newValues["Direccion"].ToString().ToUpper(),
                                    newValues["ColoniaPoblacion"].ToString().ToUpper(),
                                    newValues["DelegacionMunicipio"].ToString().ToUpper(),
                                    newValues["Ciudad"].ToString().ToUpper(),
                                    newValues["EstadoMexico"].ToString().ToUpper(),
                                    newValues["CodigoPostal"].ToString().ToUpper(),
                                    persona,
                                    newValues["EstadoExtranjero"].ToString().ToUpper(),
                                    newValues["PaisOrigenDomicilio"].ToString().ToUpper(),
                                    estado);

                if (AccionistaAgregar.Insert(accionistaNew) > 0)
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

    protected void RgdAccionistas_UpdateCommand(object source, GridCommandEventArgs e)
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
            if (oldValues["RfcAccionista"] == null) { oldValues["RfcAccionista"] = string.Empty; }
            if (oldValues["NombreCompania"] == null) { oldValues["NombreCompania"] = string.Empty; }
            if (oldValues["Nombre"] == null) { oldValues["Nombre"] = string.Empty; }
            if (oldValues["SNombre"] == null) { oldValues["SNombre"] = string.Empty; }
            if (oldValues["ApellidoP"] == null) { oldValues["ApellidoP"] = string.Empty; }
            if (oldValues["ApellidoM"] == null) { oldValues["ApellidoM"] = string.Empty; }
            if (oldValues["Porcentaje"] == null) { oldValues["Porcentaje"] = string.Empty; }
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
            if (oldValues["Persona"] == null) { oldValues["Persona"] = string.Empty; }

            Int32 idAccionista = Parser.ToNumber(item.SavedOldValues["Id"]);
            newValues["Id"] = idAccionista;

            if (newValues["RfcAcreditado"] == null) { newValues["RfcAcreditado"] = string.Empty; }
            if (newValues["RfcAccionista"] == null) { newValues["RfcAccionista"] = string.Empty; }
            if (newValues["NombreCompania"] == null) { newValues["NombreCompania"] = string.Empty; }
            if (newValues["Nombre"] == null) { newValues["Nombre"] = string.Empty; }
            if (newValues["SNombre"] == null) { newValues["SNombre"] = string.Empty; }
            if (newValues["ApellidoP"] == null) { newValues["ApellidoP"] = string.Empty; }
            if (newValues["ApellidoM"] == null) { newValues["ApellidoM"] = string.Empty; }
            if (newValues["Porcentaje"] == null) { newValues["Porcenjate"] = string.Empty; }
            if (newValues["Direccion"] == null) { newValues["Direccion"] = string.Empty; }
            if (newValues["ColoniaPoblacion"] == null) { newValues["ColoniaPoblacion"] = string.Empty; }
            if (newValues["DelegacionMunicipio"] == null) { newValues["DelegacionMunicipio"] = string.Empty; }
            if (newValues["Ciudad"] == null) { newValues["Ciudad"] = string.Empty; }
            if (newValues["EstadoMexico"] == null) { newValues["EstadoMexico"] = string.Empty; }
            if (newValues["CodigoPostal"] == null) { newValues["CodigoPostal"] = string.Empty; }




            // Ya no forzamos los 5 Ceros en el CP.
            //newValues["CodigoPostal"] = Str(newValues["CodigoPostal"].ToString(), 5);




            if (newValues["EstadoExtranjero"] == null) { newValues["EstadoExtranjero"] = string.Empty; }
            if (newValues["PaisOrigenDomicilio"] == null) { newValues["PaisOrigenDomicilio"] = string.Empty; }
            string cadenaTempN = newValues["PaisOrigenDomicilio"].ToString();
            if (cadenaTempN == string.Empty) { newValues["PaisOrigenDomicilio"] = "MX"; }
            if (newValues["Persona"] == null) { newValues["Persona"] = string.Empty; }

            Accionista accionistaOld;
            Accionista accionistaNew;
            AccionistaRules AccionistaActualizar = new AccionistaRules(Enums.Persona.Moral);
            Enums.Estado estado = Enums.Estado.Activo;
            Enums.Persona persona = Enums.Persona.Moral;
            ArrayList TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));

            //if (TipoPersona[0].ToString() == "Moral" || TipoPersona[0].ToString() == "moral")
            //{
            //    persona = Enums.Persona.Moral;
            //    newValues["Persona"] = "MORAL";
            //}
            //else
            //{
            //    persona = Enums.Persona.Fisica;
            //    newValues["Persona"] = "FISICA";
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

            if (ValidarDatosAccionista(newValues) == true)
            {
                int porcentajeOld = Parser.ToNumber(oldValues["Porcentaje"]);
                int porcentajeNew = Parser.ToNumber(newValues["Porcentaje"]);

                accionistaOld = new Accionista(idAccionista,
                                    oldValues["RfcAcreditado"].ToString().ToUpper(),
                                    oldValues["RfcAccionista"].ToString().ToUpper(),
                                    oldValues["NombreCompania"].ToString().ToUpper(),
                                    oldValues["Nombre"].ToString().ToUpper(),
                                    oldValues["SNombre"].ToString().ToUpper(),
                                    oldValues["ApellidoP"].ToString().ToUpper(),
                                    oldValues["ApellidoM"].ToString().ToUpper(),
                                    porcentajeOld,
                                    oldValues["Direccion"].ToString().ToUpper(),
                                    oldValues["ColoniaPoblacion"].ToString().ToUpper(),
                                    oldValues["DelegacionMunicipio"].ToString().ToUpper(),
                                    oldValues["Ciudad"].ToString().ToUpper(),
                                    oldValues["EstadoMexico"].ToString().ToUpper(),
                                    oldValues["CodigoPostal"].ToString().ToUpper(),
                                    persona,
                                    oldValues["EstadoExtranjero"].ToString().ToUpper(),
                                    oldValues["PaisOrigenDomicilio"].ToString().ToUpper(),
                                    estado);

                accionistaNew = new Accionista(idAccionista,
                                    newValues["RfcAcreditado"].ToString().ToUpper(),
                                    newValues["RfcAccionista"].ToString().ToUpper(),
                                    newValues["NombreCompania"].ToString().ToUpper(),
                                    newValues["Nombre"].ToString().ToUpper(),
                                    newValues["SNombre"].ToString().ToUpper(),
                                    newValues["ApellidoP"].ToString().ToUpper(),
                                    newValues["ApellidoM"].ToString().ToUpper(),
                                    porcentajeNew,
                                    newValues["Direccion"].ToString().ToUpper(),
                                    newValues["ColoniaPoblacion"].ToString().ToUpper(),
                                    newValues["DelegacionMunicipio"].ToString().ToUpper(),
                                    newValues["Ciudad"].ToString().ToUpper(),
                                    newValues["EstadoMexico"].ToString().ToUpper(),
                                    newValues["CodigoPostal"].ToString().ToUpper(),
                                    persona,
                                    newValues["EstadoExtranjero"].ToString().ToUpper(),
                                    newValues["PaisOrigenDomicilio"].ToString().ToUpper(),
                                    estado);

                if (AccionistaActualizar.Update(accionistaOld, accionistaNew) > 0)
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

    protected void RgdAccionistas_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        // Se requiere la definicion del metodo, pero no lleva codigo
    }

    protected void RgdAccionistas_DeleteCommand(object source, GridCommandEventArgs e)
    {
        // Se requiere la definicion del metodo, pero no lleva codigo
    }

    protected void RgdAccionistas_ItemCreated(object sender, GridItemEventArgs e)
    {
        // Se requiere la definicion del metodo, pero no lleva codigo
    }

    protected void RgdAccionistas_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        // Se requiere la definicion del metodo, pero no lleva codigo
    }

    protected void btnCargarDirecciones_Click(object sender, EventArgs e)
    {
        try
        {
            if (fluDirecciones.HasFile == false)
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "Favor de seleccionar el archivo de Direcciones a cargar");
                return;
            }

            string RutaArchivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + fluDirecciones.FileName));
            fluDirecciones.SaveAs(RutaArchivo);
            FileInfo ArchivoInfo = new FileInfo(RutaArchivo);

            if (ArchivoInfo.Extension == ".xls")
            {
                DataSet dsRegistrosExcel = GetDatasetFromExcelFile(RutaArchivo, 0, "TablaAccionistas");

                bool ErrorEncabezados = false;
                string CampoError = "";

                if (dsRegistrosExcel.Tables[0].Columns.Contains("RFC Acreditado") == false)
                { ErrorEncabezados = true; CampoError = "RFC Acreditado"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("RFC Accionista") == false)
                { ErrorEncabezados = true; CampoError = "RFC Accionista"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Direccion") == false)
                { ErrorEncabezados = true; CampoError = "Direccion"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Colonia o Poblacion") == false)
                { ErrorEncabezados = true; CampoError = "Colonia o Poblacion"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Delegacion o Municipio") == false)
                { ErrorEncabezados = true; CampoError = "Delegacion o Municipio"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Ciudad") == false)
                { ErrorEncabezados = true; CampoError = "RFC Acreditado"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Estado") == false)
                { ErrorEncabezados = true; CampoError = "Estado"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Codigo Postal") == false)
                { ErrorEncabezados = true; CampoError = "Codigo Postal"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Estado en el Extranjero") == false)
                { ErrorEncabezados = true; CampoError = "Estado en el Extranjero"; }

                if (ErrorEncabezados == true)
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Falta la columna " + CampoError + ", favor de verificar");
                    return;
                }

                int RegistrosLocalizadosActualizados = 0;
                int RegistrosLocalizadosError = 0;
                int RegistrosSinLocalizar = 0;

                AccionistaRules getRecords = new AccionistaRules(Enums.Persona.Moral);
                List<Accionista> RegistrosAcionistas = getRecords.GetRecords(false);

               foreach (DataRow fila in dsRegistrosExcel.Tables[0].Rows)
               {
                   string RegistroAcreditado = fila["RFC Acreditado"].ToString().ToUpper().Trim();
                   string RegistroAccionista = fila["RFC Accionista"].ToString().ToUpper().Trim();
                   string Direccion = fila["Direccion"].ToString().ToUpper().Trim();
                   string ColoniaPoblacion = fila["Colonia o Poblacion"].ToString().ToUpper().Trim();
                   string DelegacionMunicipio = fila["Delegacion o Municipio"].ToString().ToUpper().Trim();
                   string Ciudad = fila["Ciudad"].ToString().ToUpper().Trim();
                   string EstadoMexico = fila["Estado"].ToString().ToUpper().Trim();
                   string CodigoPostal = fila["Codigo Postal"].ToString().ToUpper().Trim();
                   string EstadoExtranjero = fila["Estado en el Extranjero"].ToString().ToUpper().Trim();

                   // No se realiza ninguna validacion de los datos de la direccion en la carga masiva de direcciones.
                   // Estas valicaciones se realizan en la creacion del Segmento AC del archivo de PM.

                   foreach (Accionista ItemAccionista in RegistrosAcionistas)
                   {

                       if (RegistroAcreditado.ToUpper().Trim() == ItemAccionista.RfcAcreditado.ToString().ToUpper().Trim() &&
                           RegistroAccionista.ToUpper().Trim() == ItemAccionista.RfcAccionista.ToString().ToUpper().Trim())
                       {
                           // Actualizamos registro y salimos del ciclo
                           Accionista accionistaOld;
                           Accionista accionistaNew;
                           AccionistaRules AccionistaActualizar = new AccionistaRules(Enums.Persona.Moral);

                           accionistaOld = new Accionista( ItemAccionista.Id,
                                                           ItemAccionista.RfcAcreditado,
                                                           ItemAccionista.RfcAccionista,
                                                           ItemAccionista.NombreCompania,
                                                           ItemAccionista.Nombre,
                                                           ItemAccionista.SNombre,
                                                           ItemAccionista.ApellidoP,
                                                           ItemAccionista.ApellidoM,
                                                           ItemAccionista.Porcentaje,
                                                           ItemAccionista.Direccion,
                                                           ItemAccionista.ColoniaPoblacion,
                                                           ItemAccionista.DelegacionMunicipio,
                                                           ItemAccionista.Ciudad,
                                                           ItemAccionista.EstadoMexico,
                                                           ItemAccionista.CodigoPostal,
                                                           ItemAccionista.Persona,
                                                           ItemAccionista.EstadoExtranjero,
                                                           ItemAccionista.PaisOrigenDomicilio,
                                                           ItemAccionista.Estatus );

                           accionistaNew = new Accionista( ItemAccionista.Id,
                                                           ItemAccionista.RfcAcreditado,
                                                           ItemAccionista.RfcAccionista,
                                                           ItemAccionista.NombreCompania,
                                                           ItemAccionista.Nombre,
                                                           ItemAccionista.SNombre,
                                                           ItemAccionista.ApellidoP,
                                                           ItemAccionista.ApellidoM,
                                                           ItemAccionista.Porcentaje,
                                                           Direccion,
                                                           ColoniaPoblacion,
                                                           DelegacionMunicipio,
                                                           Ciudad,
                                                           EstadoMexico,
                                                           CodigoPostal,
                                                           ItemAccionista.Persona,
                                                           EstadoExtranjero,
                                                           ItemAccionista.PaisOrigenDomicilio,
                                                           ItemAccionista.Estatus );

                           try
                           {
                               if (AccionistaActualizar.Update(accionistaOld, accionistaNew) > 0)
                               {
                                   RegistrosLocalizadosActualizados++;
                                   idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());

                                   Hashtable oldValues = new Hashtable();
                                   oldValues.Add("Id,", ItemAccionista.Id);
                                   oldValues.Add("RfcAcreditado,", ItemAccionista.RfcAcreditado);
                                   oldValues.Add("RfcAccionista,", ItemAccionista.RfcAccionista);
                                   oldValues.Add("NombreCompania,", ItemAccionista.NombreCompania);
                                   oldValues.Add("Nombre,", ItemAccionista.Nombre);
                                   oldValues.Add("SNombre,", ItemAccionista.SNombre);
                                   oldValues.Add("ApellidoP,", ItemAccionista.ApellidoP);
                                   oldValues.Add("ApellidoM,", ItemAccionista.ApellidoM);
                                   oldValues.Add("Porcentaje,", ItemAccionista.Porcentaje);
                                   oldValues.Add("Direccion,", ItemAccionista.Direccion);
                                   oldValues.Add("ColoniaPoblacion,", ItemAccionista.ColoniaPoblacion);
                                   oldValues.Add("DelegacionMunicipio,", ItemAccionista.DelegacionMunicipio);
                                   oldValues.Add("Ciudad,", ItemAccionista.Ciudad);
                                   oldValues.Add("EstadoMexico,", ItemAccionista.EstadoMexico);
                                   oldValues.Add("CodigoPostal,", ItemAccionista.CodigoPostal);
                                   oldValues.Add("Persona,", ItemAccionista.Persona);
                                   oldValues.Add("EstadoExtranjero,", ItemAccionista.EstadoExtranjero);
                                   oldValues.Add("PaisOrigenDomicilio,", ItemAccionista.PaisOrigenDomicilio);
                                   oldValues.Add("Estatus );", ItemAccionista.Estatus);

                                   Hashtable newValues = new Hashtable();
                                   newValues.Add("Id,", ItemAccionista.Id);
                                   newValues.Add("RfcAcreditado,", ItemAccionista.RfcAcreditado);
                                   newValues.Add("RfcAccionista,", ItemAccionista.RfcAccionista);
                                   newValues.Add("NombreCompania,", ItemAccionista.NombreCompania);
                                   newValues.Add("Nombre,", ItemAccionista.Nombre);
                                   newValues.Add("SNombre,", ItemAccionista.SNombre);
                                   newValues.Add("ApellidoP,", ItemAccionista.ApellidoP);
                                   newValues.Add("ApellidoM,", ItemAccionista.ApellidoM);
                                   newValues.Add("Porcentaje,", ItemAccionista.Porcentaje);
                                   newValues.Add("Direccion,", Direccion);
                                   newValues.Add("ColoniaPoblacion,", ColoniaPoblacion);
                                   newValues.Add("DelegacionMunicipio,", DelegacionMunicipio);
                                   newValues.Add("Ciudad,", Ciudad);
                                   newValues.Add("EstadoMexico,", EstadoMexico);
                                   newValues.Add("CodigoPostal,", CodigoPostal);
                                   newValues.Add("Persona,", ItemAccionista.Persona);
                                   newValues.Add("EstadoExtranjero,", EstadoExtranjero);
                                   newValues.Add("PaisOrigenDomicilio,", ItemAccionista.PaisOrigenDomicilio);
                                   newValues.Add("Estatus );", ItemAccionista.Estatus);

                                   ActividadRules.GActividadCatalogo(8888, idUsuario, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);
                               }
                            }
                            catch
                            { RegistrosLocalizadosError++; }

                           break;
                       }
                   }
               }

               RgdAccionistas.Rebind();
               RegistrosSinLocalizar = dsRegistrosExcel.Tables[0].Rows.Count - RegistrosLocalizadosActualizados - RegistrosLocalizadosError;
               Mensajes.ShowAdvertencia(this.Page, this.GetType(), RegistrosLocalizadosActualizados + " Registros actualizados, " + RegistrosSinLocalizar + " registros sin coincidencias para actualizar, " + RegistrosLocalizadosError  + " registros marcaron error al actualizar.");
            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "La extensión de archivo debe ser XLS");
            }

        }
        catch
        {
            Mensajes.ShowMessage(this.Page, this.GetType(), "Error abriendo el archivo, puede que el documento este dañado o no tenga el formato correcto");
        }
    }

    protected void btnCargarTipos_Click(object sender, EventArgs e)
    {
        try
        {
            if (fluTipos.HasFile == false)
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "Favor de seleccionar el archivo de Tipos de Accionistas a cargar");
                return;
            }

            string RutaArchivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + fluTipos.FileName));
            fluTipos.SaveAs(RutaArchivo);
            FileInfo ArchivoInfo = new FileInfo(RutaArchivo);

            if (ArchivoInfo.Extension == ".xls")
            {
                DataSet dsRegistrosExcel = GetDatasetFromExcelFile(RutaArchivo, 0, "TablaAccionistas");

                bool ErrorEncabezados = false;
                string CampoError = "";

                if (dsRegistrosExcel.Tables[0].Columns.Contains("RFC") == false)
                { ErrorEncabezados = true; CampoError = "RFC"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Nombre") == false)
                { ErrorEncabezados = true; CampoError = "Nombre"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Persona") == false)
                { ErrorEncabezados = true; CampoError = "Persona"; }

                if (ErrorEncabezados == true)
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Falta la columna " + CampoError + ", favor de verificar");
                    return;
                }

                int RegistrosLocalizadosActualizados = 0;
                int RegistrosLocalizadosError = 0;

                AccionistaRules getRecords = new AccionistaRules(Enums.Persona.Moral);
                List<Accionista> RegistrosAcionistas = getRecords.GetRecords(false);

                foreach (DataRow fila in dsRegistrosExcel.Tables[0].Rows)
                {
                    string RegistroAccionista = fila["RFC"].ToString().ToUpper().Trim();
                    string TipoAcconista = fila["Persona"].ToString().ToUpper().Trim();
                    Enums.Persona TipoPersona = Enums.Persona.Moral;

                    switch (TipoAcconista.ToString().ToUpper().Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U'))
                    {
                        case "MORAL":
                            TipoPersona = Enums.Persona.Moral;
                            break;
                        case "FISICA":
                            TipoPersona = Enums.Persona.Fisica;
                            break;
                        case "FIDEICOMISO":
                            TipoPersona = Enums.Persona.Fideicomiso;
                            break;
                        case "GOBIERNO":
                            TipoPersona = Enums.Persona.Gobierno;
                            break;
                    }

                    foreach (Accionista ItemAccionista in RegistrosAcionistas)
                    {

                        if (RegistroAccionista.ToUpper().Trim() == ItemAccionista.RfcAccionista.ToString().ToUpper().Trim())
                        {
                            // Actualizamos registro y continuamos hasta terminar todos los registros.
                            Accionista accionistaOld;
                            Accionista accionistaNew;
                            AccionistaRules AccionistaActualizar = new AccionistaRules(Enums.Persona.Moral);

                            accionistaOld = new Accionista( ItemAccionista.Id,
                                                            ItemAccionista.RfcAcreditado,
                                                            ItemAccionista.RfcAccionista,
                                                            ItemAccionista.NombreCompania,
                                                            ItemAccionista.Nombre,
                                                            ItemAccionista.SNombre,
                                                            ItemAccionista.ApellidoP,
                                                            ItemAccionista.ApellidoM,
                                                            ItemAccionista.Porcentaje,
                                                            ItemAccionista.Direccion,
                                                            ItemAccionista.ColoniaPoblacion,
                                                            ItemAccionista.DelegacionMunicipio,
                                                            ItemAccionista.Ciudad,
                                                            ItemAccionista.EstadoMexico,
                                                            ItemAccionista.CodigoPostal,
                                                            ItemAccionista.Persona,
                                                            ItemAccionista.EstadoExtranjero,
                                                            ItemAccionista.PaisOrigenDomicilio,
                                                            ItemAccionista.Estatus);

                            accionistaNew = new Accionista( ItemAccionista.Id,
                                                            ItemAccionista.RfcAcreditado,
                                                            ItemAccionista.RfcAccionista,
                                                            ItemAccionista.NombreCompania,
                                                            ItemAccionista.Nombre,
                                                            ItemAccionista.SNombre,
                                                            ItemAccionista.ApellidoP,
                                                            ItemAccionista.ApellidoM,
                                                            ItemAccionista.Porcentaje,
                                                            ItemAccionista.Direccion,
                                                            ItemAccionista.ColoniaPoblacion,
                                                            ItemAccionista.DelegacionMunicipio,
                                                            ItemAccionista.Ciudad,
                                                            ItemAccionista.EstadoMexico,
                                                            ItemAccionista.CodigoPostal,
                                                            TipoPersona,
                                                            ItemAccionista.EstadoExtranjero,
                                                            ItemAccionista.PaisOrigenDomicilio,
                                                            ItemAccionista.Estatus);

                            try
                            {
                                if (AccionistaActualizar.Update(accionistaOld, accionistaNew) > 0)
                                {
                                    RegistrosLocalizadosActualizados++;
                                    idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());

                                    Hashtable oldValues = new Hashtable();
                                    oldValues.Add("Id,", ItemAccionista.Id);
                                    oldValues.Add("RfcAcreditado,", ItemAccionista.RfcAcreditado);
                                    oldValues.Add("RfcAccionista,", ItemAccionista.RfcAccionista);
                                    oldValues.Add("NombreCompania,", ItemAccionista.NombreCompania);
                                    oldValues.Add("Nombre,", ItemAccionista.Nombre);
                                    oldValues.Add("SNombre,", ItemAccionista.SNombre);
                                    oldValues.Add("ApellidoP,", ItemAccionista.ApellidoP);
                                    oldValues.Add("ApellidoM,", ItemAccionista.ApellidoM);
                                    oldValues.Add("Porcentaje,", ItemAccionista.Porcentaje);
                                    oldValues.Add("Direccion,", ItemAccionista.Direccion);
                                    oldValues.Add("ColoniaPoblacion,", ItemAccionista.ColoniaPoblacion);
                                    oldValues.Add("DelegacionMunicipio,", ItemAccionista.DelegacionMunicipio);
                                    oldValues.Add("Ciudad,", ItemAccionista.Ciudad);
                                    oldValues.Add("EstadoMexico,", ItemAccionista.EstadoMexico);
                                    oldValues.Add("CodigoPostal,", ItemAccionista.CodigoPostal);
                                    oldValues.Add("Persona,", ItemAccionista.Persona);
                                    oldValues.Add("EstadoExtranjero,", ItemAccionista.EstadoExtranjero);
                                    oldValues.Add("PaisOrigenDomicilio,", ItemAccionista.PaisOrigenDomicilio);
                                    oldValues.Add("Estatus );", ItemAccionista.Estatus);

                                    Hashtable newValues = new Hashtable();
                                    newValues.Add("Id,", ItemAccionista.Id);
                                    newValues.Add("RfcAcreditado,", ItemAccionista.RfcAcreditado);
                                    newValues.Add("RfcAccionista,", ItemAccionista.RfcAccionista);
                                    newValues.Add("NombreCompania,", ItemAccionista.NombreCompania);
                                    newValues.Add("Nombre,", ItemAccionista.Nombre);
                                    newValues.Add("SNombre,", ItemAccionista.SNombre);
                                    newValues.Add("ApellidoP,", ItemAccionista.ApellidoP);
                                    newValues.Add("ApellidoM,", ItemAccionista.ApellidoM);
                                    newValues.Add("Porcentaje,", ItemAccionista.Porcentaje);
                                    newValues.Add("Direccion,", ItemAccionista.Direccion);
                                    newValues.Add("ColoniaPoblacion,", ItemAccionista.ColoniaPoblacion);
                                    newValues.Add("DelegacionMunicipio,", ItemAccionista.DelegacionMunicipio);
                                    newValues.Add("Ciudad,", ItemAccionista.Ciudad);
                                    newValues.Add("EstadoMexico,", ItemAccionista.EstadoMexico);
                                    newValues.Add("CodigoPostal,", ItemAccionista.CodigoPostal);
                                    newValues.Add("Persona,", TipoPersona);
                                    newValues.Add("EstadoExtranjero,", ItemAccionista.EstadoExtranjero);
                                    newValues.Add("PaisOrigenDomicilio,", ItemAccionista.PaisOrigenDomicilio);
                                    newValues.Add("Estatus );", ItemAccionista.Estatus);

                                    ActividadRules.GActividadCatalogo(8888, idUsuario, "Modificación de Registro", null, null, catalog, 1, oldValues, newValues);
                                }
                            }
                            catch
                            { RegistrosLocalizadosError++; }

                        }
                    }
                }

                RgdAccionistas.Rebind();
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), RegistrosLocalizadosActualizados + " Registros actualizados, " + RegistrosLocalizadosError + " registros marcaron error al actualizar.");
            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "La extensión de archivo debe ser XLS");
            }

        }
        catch
        {
            Mensajes.ShowMessage(this.Page, this.GetType(), "Error abriendo el archivo, puede que el documento este dañado o no tenga el formato correcto");
        }

    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkHeader;
            GridHeaderItem headerItem = RgdAccionistas.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");

            if (chkHeader.Checked == true)
            {
                int RegistrosEliminados = 0;
                RgdAccionistas.AllowPaging = false;
                RgdAccionistas.Rebind();

                foreach (GridDataItem row in RgdAccionistas.Items)
                {
                    Accionista AccionistaDelete;
                    Int32 idAccionista = 0;
                    string rfcAcreditado = string.Empty;
                    string rfcAccionista = string.Empty;

                    //Datos para identificar valor
                    idAccionista = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    rfcAcreditado = row["RfcAcreditado"].Text.ToString();
                    rfcAccionista = row["RfcAccionista"].Text.ToString();

                    AccionistaDelete = new Accionista(idAccionista, rfcAcreditado, rfcAccionista, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty,
                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Enums.Persona.Moral, string.Empty, string.Empty, Enums.Estado.Activo);

                    //Llamar el SP correspondiente con las entidades old y new de Validacion
                    AccionistaRules AccionistaBorrar = new AccionistaRules(Enums.Persona.Moral);

                    if (AccionistaBorrar.Delete(AccionistaDelete) > 0)
                    {
                        RegistrosEliminados++;
                    }
                }

                if (RgdAccionistas.Items.Count == RegistrosEliminados)
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Registros removidos correctamente");
                }
                else
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), RegistrosEliminados + " registros de " + RgdAccionistas.Items.Count + " removidos correctamente");
                }

                RgdAccionistas.Rebind();
                RgdAccionistas.DataSource = null;
                RgdAccionistas.AllowPaging = true;
                RgdAccionistas.Rebind();
                RgdAccionistas.DataBind();
                idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUsuario, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdAccionistas.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
            }
            else
            {
                int RegistrosEliminados = 0;

                foreach (GridDataItem row in RgdAccionistas.Items)
                {
                    CheckBox chkCell;
                    bool chkResult;
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkResult = (bool)chkCell.Checked;

                    if (chkResult)
                    {

                        Accionista AccionistaDelete;
                        Int32 idAccionista = 0;
                        string rfcAcreditado = string.Empty;
                        string rfcAccionista = string.Empty;

                        //Datos para identificar valor

                        idAccionista = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                        rfcAcreditado = row["RfcAcreditado"].Text.ToString();
                        rfcAccionista = row["RfcAccionista"].Text.ToString();

                        AccionistaDelete = new Accionista(idAccionista, rfcAcreditado, rfcAccionista, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty,
                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Enums.Persona.Moral, string.Empty, string.Empty, Enums.Estado.Activo);

                        //Llamar el SP correspondiente con las entidades old y new de Validacion
                        AccionistaRules AccionistaBorrar = new AccionistaRules(Enums.Persona.Moral);

                        if (AccionistaBorrar.Delete(AccionistaDelete) > 0)
                        {
                            RegistrosEliminados++;
                            idUsuario = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GActividadCatalogo(1111, idUsuario, "Eliminación de Registro con ID " + idAccionista, null, null, catalog, 1, null, null);
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

        RgdAccionistas.Rebind();
    }

    protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdAccionistas.Columns[0].Visible = false;
        RgdAccionistas.Columns[RgdAccionistas.Columns.Count - 1].Visible = false;
        RgdAccionistas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAccionistas.ExportSettings.OpenInNewWindow = false;
        RgdAccionistas.ExportSettings.ExportOnlyData = true;
        RgdAccionistas.MasterTableView.GridLines = GridLines.Both;
        RgdAccionistas.ExportSettings.IgnorePaging = true;
        RgdAccionistas.ExportSettings.OpenInNewWindow = true;
        RgdAccionistas.ExportSettings.Pdf.PageHeight = Unit.Parse("350mm");
        RgdAccionistas.ExportSettings.Pdf.PageWidth = Unit.Parse("930mm");
        RgdAccionistas.MasterTableView.ExportToPdf();

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catálogo " + catalog);
    }

    protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";

        RgdAccionistas.Columns[0].Visible = false;
        RgdAccionistas.Columns[RgdAccionistas.Columns.Count - 1].Visible = false;
        RgdAccionistas.MasterTableView.HierarchyDefaultExpanded = true;
        RgdAccionistas.ExportSettings.OpenInNewWindow = false;
        RgdAccionistas.ExportSettings.ExportOnlyData = true;
        RgdAccionistas.ExportSettings.IgnorePaging = true;
        RgdAccionistas.ExportSettings.OpenInNewWindow = true;
        RgdAccionistas.MasterTableView.ExportToExcel();

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

            foreach (GridDataItem row in RgdAccionistas.Items)
            {
                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkCell.Checked = chkHeader.Checked;

                row["TipoPersonaTemp"].Text = row["Persona"].Text;
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
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_ACCIONISTA")))
        {
            valido = true;
        }
        return valido;
    }

    private bool ValidarDatosAccionista(Hashtable DatosAccionista)
    {

        // Solo se contemplan las validaciones mas basicas de los datos del catalogo.
        // Las validaciones generales se realizan a la hora de crear el Segmento AC al procesar la informacion de Personas Morales.

        if (DatosAccionista["RfcAcreditado"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El RFC del acreditado no puede ser nulo");
            return false;
        }

        if (DatosAccionista["RfcAccionista"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El RFC del Accionista no puede ser nulo");
            return false;
        }

        if (ValidarRegistroDuplicado(DatosAccionista["Id"].ToString(), DatosAccionista["RfcAcreditado"].ToString(), DatosAccionista["RfcAccionista"].ToString()) == true)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Ya existe un registro con los mismos datos RFC Acreditado: " + DatosAccionista["RfcAcreditado"].ToString() + ", RFC Accionista: " + DatosAccionista["RfcAccionista"].ToString());
            return false;
        }

        if (DatosAccionista["Porcentaje"] == null)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El dato de Porcentaje no puede ser nulo");
            return false;
        }

        if (ValidarCampoNumerico(DatosAccionista["Porcentaje"].ToString().Trim(), "Credito") == false)
        {
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

    private bool ValidarRegistroDuplicado(string idRegistro, string RegistroAcreditado, string RegistroAccionista)
    {
        bool Respuesta = false;

        try
        {
            AccionistaRules getRecords = new AccionistaRules(Enums.Persona.Moral);
            List<Accionista> RegistrosAcionistas = getRecords.GetRecords(false);

            foreach (Accionista ItemAccionista in RegistrosAcionistas)
            {

                if (RegistroAcreditado.ToUpper() == ItemAccionista.RfcAcreditado.ToString().ToUpper() &&
                    RegistroAccionista.ToUpper() == ItemAccionista.RfcAccionista.ToString().ToUpper())
                {
                    if (idRegistro == ItemAccionista.Id.ToString() && idRegistro.Trim() != "0")
                    {
                        // Si el id del registro es diferente de 0 se trata de la actualizacion de un registro
                        // Si el id del accionista encontrado es igual al del registro que esta siendo editado permitimos
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

    private DataSet GetDatasetFromExcelFile(string excelFileFullPath, int ExcelSheet, string TableName)
    {
        Workbook book = null;
        try
        {
            book = Workbook.Load(excelFileFullPath);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            throw new Exception("No se puede cargar el archivo, formato desconocido (excel 2003 / xls)");
        }

        DataSet ds = new DataSet();
        List<string> renglon = null;
        Worksheet sheet = book.Worksheets[ExcelSheet];

        ds.Tables.Add(TableName);

        for (int i = 0; i <= sheet.Cells.LastRowIndex; i++)
        {
            renglon = new List<string>();
            for (int j = 0; j <= sheet.Cells.LastColIndex; j++)
            {
                if (i < 1)
                { ds.Tables[0].Columns.Add(sheet.Cells[i, j].StringValue, typeof(System.String)); }
                else
                { renglon.Add(sheet.Cells[i, j].StringValue); }
            }

            if (i > 0)
            {
                if (renglon[0].Length > 0)
                { ds.Tables[0].Rows.Add(renglon.ToArray()); }
            }
        }

        return ds;
    }

    private string ObtenerRelacionesConsultar()
    {
        string TipoRelaciones = "-1";
        TipoRelacionRules getRecords = null;
        List<TipoRelacion> RelacionesInfo;

        try
        {
            getRecords = new TipoRelacionRules();
            var s = getRecords.GetRecordsPorUtilidad("1", true);
            RelacionesInfo = s;

            foreach (TipoRelacion ItemRelacion in RelacionesInfo)
            {
                TipoRelaciones = TipoRelaciones + ", " + ItemRelacion.ClaveRelacion.Trim();
            }
        }
        catch
        {
            Mensajes.ShowError(this.Page, this.GetType(), "Error al consultar los tipos de relaciones validos para los accionistas");
        }

        return TipoRelaciones;
    }

    private void getFacultades(string persona)
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;

        this.RgdAccionistas.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
        btnExportarPDF.Visible = false;
        btnExportarExcel.Visible = false;

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_ACCIONISTA")))
        {
            this.RgdAccionistas.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
            valido = true;
        }

        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_ACCIONISTA")))
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
        GridFilterMenu menu = RgdAccionistas.FilterMenu;
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

    private void ConsultaCPSepomex(string CodigoPostal)
    {
        SEPOMEXRules getRecords = null;
        List<SEPOMEX> SEPOMEXInfo;

        try
        {
            getRecords = new SEPOMEXRules();
            var s = getRecords.GetSepomexPorCP(CodigoPostal, true);
            SEPOMEXInfo = s;
        }
        catch
        {
            Mensajes.ShowError(this.Page, this.GetType(), "Error al consultar los datos de SEPOMEX por Codigo Postal");
        }
    }

    private void ConsultarEstado(string ClaveEstado)
    {
        Estados_Rules getRecords = null;
        List<Estado> EstadosInfo;

        try
        {
            getRecords = new Estados_Rules(Enums.Persona.Moral);
            var s = getRecords.GetEstadoPorClaveBuro("M", "0", ClaveEstado, true);
            EstadosInfo = s;
        }
        catch
        {
            Mensajes.ShowError(this.Page, this.GetType(), "Error al consultar el Estado por Clave del Buro");
        }
    }

    private void ConsultarPais(string ClavePais)
    {
        PaisRules getRecords = null;
        List<Pais> PaisesInfo;

        try
        {
            getRecords = new PaisRules();
            var s = getRecords.GetPaisPorClaveBuro(Enums.Persona.Moral, "0", ClavePais, true);
            PaisesInfo = s;
        }
        catch
        {
            Mensajes.ShowError(this.Page, this.GetType(), "Error al consultar el Pais por Clave del Buro");
        }
    }




    public String Str(string value, int size)
    {
        if (value.Trim().Length > size)
            value = value.Trim().Substring(0, size);

        string v = "";
        if (!String.IsNullOrWhiteSpace(value))
            v = value.Trim();

        return v.PadLeft(size, '0');
    }


    /// <summary>
    /// Carga masiva excel, ultrasist 20211231 carga masiva accionistas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCargaAccionistasExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (fileUpCargaAccionistasExcel.HasFile == false)
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "Favor de seleccionar el archivo de Accionistas a cargar");
                return;
            }

            string RutaArchivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + fileUpCargaAccionistasExcel.FileName));
            fileUpCargaAccionistasExcel.SaveAs(RutaArchivo);
            FileInfo ArchivoInfo = new FileInfo(RutaArchivo);

            if (ArchivoInfo.Extension == ".xls")
            {
                DataSet dsRegistrosExcel = GetDatasetFromExcelFile(RutaArchivo, 0, "TablaAccionistas");

                bool ErrorEncabezados = false;
                string CampoError = "";

                if (dsRegistrosExcel.Tables[0].Columns.Contains("RFC Acreditado") == false)
                { ErrorEncabezados = true; CampoError = "RFC Acreditado"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("RFC Accionista") == false)
                { ErrorEncabezados = true; CampoError = "RFC Accionista"; }

                //ultrasist accionistas
                if (dsRegistrosExcel.Tables[0].Columns.Contains("Nombre Compañía") == false)
                { ErrorEncabezados = true; CampoError = "Nombre Compañía"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Nombre") == false)
                { ErrorEncabezados = true; CampoError = "Nombre"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Segundo Nombre") == false)
                { ErrorEncabezados = true; CampoError = "Segundo Nombre"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Apellido Paterno") == false)
                { ErrorEncabezados = true; CampoError = "Apellido Paterno"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Apellido Materno") == false)
                { ErrorEncabezados = true; CampoError = "Apellido Materno"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Porcentaje") == false)
                { ErrorEncabezados = true; CampoError = "Porcentaje"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Direccion") == false)
                { ErrorEncabezados = true; CampoError = "Direccion"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Colonia o Poblacion") == false)
                { ErrorEncabezados = true; CampoError = "Colonia o Poblacion"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Delegacion o Municipio") == false)
                { ErrorEncabezados = true; CampoError = "Delegacion o Municipio"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Ciudad") == false)
                { ErrorEncabezados = true; CampoError = "RFC Acreditado"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Estado") == false)
                { ErrorEncabezados = true; CampoError = "Estado"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Codigo Postal") == false)
                { ErrorEncabezados = true; CampoError = "Codigo Postal"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Estado en el Extranjero") == false)
                { ErrorEncabezados = true; CampoError = "Estado en el Extranjero"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Pais Origen Domicilio") == false)
                { ErrorEncabezados = true; CampoError = "Pais Origen Domicilio"; }

                if (dsRegistrosExcel.Tables[0].Columns.Contains("Persona") == false)
                { ErrorEncabezados = true; CampoError = "Persona"; }

                if (ErrorEncabezados == true)
                {
                    Mensajes.ShowMessage(this.Page, this.GetType(), "Falta la columna " + CampoError + ", favor de verificar");
                    return;
                }

                DataTable dtAccionistas = new DataTable("tbl_col");
                dtAccionistas.Columns.Add("RFC_ACREDITADO");
                dtAccionistas.Columns.Add("RFC_ACCIONISTA");
                dtAccionistas.Columns.Add("NOMBRE_COMPANIA");
                dtAccionistas.Columns.Add("NOMBRE");
                dtAccionistas.Columns.Add("SEGUNDO_NOMBRE");
                dtAccionistas.Columns.Add("APELLIDO_PATERNO");
                dtAccionistas.Columns.Add("APELLIDO_MATERNO");
                dtAccionistas.Columns.Add("PORCENTAJE_PARTICIPACION");
                dtAccionistas.Columns.Add("DIRECCION");
                dtAccionistas.Columns.Add("COLONIA_POBLACION");
                dtAccionistas.Columns.Add("DELEGACION_MUNICIPIO");
                dtAccionistas.Columns.Add("CIUDAD");
                dtAccionistas.Columns.Add("ESTADO_MEXICO");
                dtAccionistas.Columns.Add("CODIGO_POSTAL");
                dtAccionistas.Columns.Add("PERSONA");
                dtAccionistas.Columns.Add("ESTADO_EXTRANJERO");
                dtAccionistas.Columns.Add("PAIS_ORIGEN_DOMICILIO");

                try
                {
                    foreach (DataRow fila in dsRegistrosExcel.Tables[0].Rows)
                    {
                        string RFCAcreditado = fila["RFC Acreditado"].ToString().ToUpper().Trim();
                        string RFCAccionista = fila["RFC Accionista"].ToString().ToUpper().Trim();
                        string Razon = fila["Nombre Compañía"].ToString().ToUpper().Trim();
                        string Nombre1 = fila["Nombre"].ToString().ToUpper().Trim();
                        string Nombre2 = fila["Segundo Nombre"].ToString().ToUpper().Trim();
                        string APaterno = fila["Apellido Paterno"].ToString().ToUpper().Trim();
                        string AMaterno = fila["Apellido Materno"].ToString().ToUpper().Trim();

                        // Quitamos decimales para evitar de una vez problemas futuros al crear la cinta de la BD
                        decimal NumDecimal = 0;
                        try
                        { NumDecimal = Convert.ToDecimal(fila["Porcentaje"].ToString().ToUpper().Trim()); }
                        catch
                        { NumDecimal = 0; }

                        int Porcentaje = Decimal.ToInt32(Math.Round(NumDecimal, 0));

                        // Por regla solo se pueden dos digitos por lo que el 100 pasa a ser 99
                        if (Porcentaje > 99)
                        { Porcentaje = 99; }

                        string Direccion = fila["Direccion"].ToString().ToUpper().Trim();
                        string ColoniaPoblacion = fila["Colonia o Poblacion"].ToString().ToUpper().Trim();
                        string DelegacionMunicipio = fila["Delegacion o Municipio"].ToString().ToUpper().Trim();
                        string Ciudad = fila["Ciudad"].ToString().ToUpper().Trim();
                        string EstadoMexico = fila["Estado"].ToString().ToUpper().Trim();
                        string CodigoPostal = fila["Codigo Postal"].ToString().ToUpper().Trim();
                        string EstadoExtranjero = fila["Estado en el Extranjero"].ToString().ToUpper().Trim();
                        string PaisOrigenDom = fila["Pais Origen Domicilio"].ToString().ToUpper().Trim();
                        string Persona = fila["Persona"].ToString().ToUpper().Trim();

                        // Separacion del Primer y Segundo Nombre
                        string[] NombresPersona;
                        string[] stringSeparadores = new string[] { " " };
                        NombresPersona = Nombre1.Split(stringSeparadores, StringSplitOptions.RemoveEmptyEntries);

                        string Nombre2Obtenido = "";
                        for (int j = 0; j < NombresPersona.Count(); j++)
                        {
                            if (j == 0)
                            {
                                Nombre1 = NombresPersona[j];
                            }
                            else
                            {
                                Nombre2Obtenido = NombresPersona[j] + " ";
                            }
                        }

                        Nombre2 = Nombre2Obtenido + Nombre2 ;

                        DataRow drDatos;
                        drDatos = dtAccionistas.NewRow();
                        drDatos["RFC_ACREDITADO"] = RFCAcreditado.ToUpper().Trim();
                        drDatos["RFC_ACCIONISTA"] = RFCAccionista.ToUpper().Trim();
                        drDatos["NOMBRE_COMPANIA"] = Razon.ToUpper().Trim();
                        drDatos["NOMBRE"] = Nombre1.ToUpper().Trim();
                        drDatos["SEGUNDO_NOMBRE"] = Nombre2.ToUpper().Trim();
                        drDatos["APELLIDO_PATERNO"] = APaterno.ToUpper().Trim();
                        drDatos["APELLIDO_MATERNO"] = AMaterno.ToUpper().Trim();
                        drDatos["PORCENTAJE_PARTICIPACION"] = Porcentaje;
                        drDatos["DIRECCION"] = Direccion.ToUpper().Trim();
                        drDatos["COLONIA_POBLACION"] = ColoniaPoblacion.ToUpper().Trim();
                        drDatos["DELEGACION_MUNICIPIO"] = DelegacionMunicipio.ToUpper().Trim();
                        drDatos["CIUDAD"] = Ciudad.ToUpper().Trim();
                        drDatos["ESTADO_MEXICO"] = EstadoMexico.ToUpper();
                        drDatos["CODIGO_POSTAL"] = CodigoPostal.ToUpper().Trim();
                        drDatos["PERSONA"] = Persona.ToUpper().Trim();
                        drDatos["ESTADO_EXTRANJERO"] = EstadoExtranjero.ToUpper().Trim();
                        drDatos["PAIS_ORIGEN_DOMICILIO"] = PaisOrigenDom.ToUpper().Trim();

                        // No se realiza ninguna validacion en la carga masiva
                        dtAccionistas.Rows.Add(drDatos);
                    }

                    int TotalRegistros = dtAccionistas.Rows.Count;
                }

                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }

                cls_cargaArchivos cargaMasiva = new cls_cargaArchivos();
                try
                {
                    // Creamos el Layout de los metadatos tabla a procesar
                    DataTable dt_metaDataLayout = new DataTable();
                    dt_metaDataLayout.Columns.Add("nombreColumna");
                    dt_metaDataLayout.Columns.Add("tipoDato");
                    dt_metaDataLayout.Columns.Add("longitud");

                    DataRow row1;
                    row1 = dt_metaDataLayout.NewRow();
                    row1["nombreColumna"] = "RFC_Acreditado";
                    row1["tipoDato"] = "VARCHAR2";
                    row1["longitud"] = "15";

                    DataRow row2;
                    row2 = dt_metaDataLayout.NewRow();
                    row2["nombreColumna"] = "RFC_Accionista";
                    row2["tipoDato"] = "VARCHAR2";
                    row2["longitud"] = "15";

                    DataRow row3;
                    row3 = dt_metaDataLayout.NewRow();
                    row3["nombreColumna"] = "Nombre_Compania";
                    row3["tipoDato"] = "VARCHAR2";
                    row3["longitud"] = "150";

                    DataRow row4;
                    row4 = dt_metaDataLayout.NewRow();
                    row4["nombreColumna"] = "Nombre";
                    row4["tipoDato"] = "VARCHAR2";
                    row4["longitud"] = "30";

                    DataRow row5;
                    row5 = dt_metaDataLayout.NewRow();
                    row5["nombreColumna"] = "Segundo_Nombre";
                    row5["tipoDato"] = "VARCHAR2";
                    row5["longitud"] = "30";

                    DataRow row6;
                    row6 = dt_metaDataLayout.NewRow();
                    row6["nombreColumna"] = "Apellido_Paterno";
                    row6["tipoDato"] = "VARCHAR2";
                    row6["longitud"] = "25";

                    DataRow row7;
                    row7 = dt_metaDataLayout.NewRow();
                    row7["nombreColumna"] = "Apellido_Materno";
                    row7["tipoDato"] = "VARCHAR2";
                    row7["longitud"] = "25";

                    DataRow row8;
                    row8 = dt_metaDataLayout.NewRow();
                    row8["nombreColumna"] = "Porcentaje_Participacion";
                    row8["tipoDato"] = "NUMBER";
                    row8["longitud"] = "2";

                    DataRow row9;
                    row9 = dt_metaDataLayout.NewRow();
                    row9["nombreColumna"] = "Direccion";
                    row9["tipoDato"] = "VARCHAR2";
                    row9["longitud"] = "40";

                    DataRow row10;
                    row10 = dt_metaDataLayout.NewRow();
                    row10["nombreColumna"] = "Colonia_Poblacion";
                    row10["tipoDato"] = "VARCHAR2";
                    row10["longitud"] = "60";

                    DataRow row11;
                    row11 = dt_metaDataLayout.NewRow();
                    row11["nombreColumna"] = "Delegacion_Municipio";
                    row11["tipoDato"] = "VARCHAR2";
                    row11["longitud"] = "40";

                    DataRow row12;
                    row12 = dt_metaDataLayout.NewRow();
                    row12["nombreColumna"] = "Ciudad";
                    row12["tipoDato"] = "VARCHAR2";
                    row12["longitud"] = "40";

                    DataRow row13;
                    row13 = dt_metaDataLayout.NewRow();
                    row13["nombreColumna"] = "Estado_Mexico";
                    row13["tipoDato"] = "VARCHAR2";
                    row13["longitud"] = "4";

                    DataRow row14;
                    row14 = dt_metaDataLayout.NewRow();
                    row14["nombreColumna"] = "Codigo_Postal";
                    row14["tipoDato"] = "VARCHAR2";
                    row14["longitud"] = "10";

                    DataRow row15;
                    row15 = dt_metaDataLayout.NewRow();
                    row15["nombreColumna"] = "Persona";
                    row15["tipoDato"] = "VARCHAR2";
                    row15["longitud"] = "15";

                    DataRow row16;
                    row16 = dt_metaDataLayout.NewRow();
                    row16["nombreColumna"] = "Estado_Extranjero";
                    row16["tipoDato"] = "VARCHAR2";
                    row16["longitud"] = "40";

                    DataRow row17;
                    row17 = dt_metaDataLayout.NewRow();
                    row17["nombreColumna"] = "Pais_Origen_Domicilio";
                    row17["tipoDato"] = "VARCHAR2";
                    row17["longitud"] = "2";

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

                    // Creamos el conjunto de parametros del SP
                    DbParameter[] parametros = new DbParameter[20];
                    parametros[0] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[0].Direction = ParameterDirection.Output;
                    parametros[0].ParameterName = "id_OUT";
                    parametros[0].DbType = DbType.Decimal;
                    parametros[0].Size = 15;

                    parametros[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[1].Direction = ParameterDirection.Output;
                    parametros[1].ParameterName = "tipo_OUT";
                    parametros[1].DbType = DbType.Decimal;
                    parametros[1].Size = 15;

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
                    parametros[4].ParameterName = "rfcAccionistaP";
                    parametros[4].DbType = DbType.String;
                    parametros[4].Size = 15;

                    parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[5].Direction = ParameterDirection.Input;
                    parametros[5].ParameterName = "NombreCompaniaP";
                    parametros[5].DbType = DbType.String;
                    parametros[5].Size = 150;

                    parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[6].Direction = ParameterDirection.Input;
                    parametros[6].ParameterName = "NombreP";
                    parametros[6].DbType = DbType.String;
                    parametros[6].Size = 30;

                    parametros[7] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[7].Direction = ParameterDirection.Input;
                    parametros[7].ParameterName = "SNombreP";
                    parametros[7].DbType = DbType.String;
                    parametros[7].Size = 30;

                    parametros[8] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[8].Direction = ParameterDirection.Input;
                    parametros[8].ParameterName = "APaternoP";
                    parametros[8].DbType = DbType.String;
                    parametros[8].Size = 25;

                    parametros[9] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[9].Direction = ParameterDirection.Input;
                    parametros[9].ParameterName = "AMaternoP";
                    parametros[9].DbType = DbType.String;
                    parametros[9].Size = 25;

                    parametros[10] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[10].Direction = ParameterDirection.Input;
                    parametros[10].ParameterName = "PorcentajeP";
                    parametros[10].DbType = DbType.Decimal;
                    parametros[10].Size = 2;

                    parametros[11] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[11].Direction = ParameterDirection.Input;
                    parametros[11].ParameterName = "DireccionP";
                    parametros[11].DbType = DbType.String;
                    parametros[11].Size = 40;

                    parametros[12] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[12].Direction = ParameterDirection.Input;
                    parametros[12].ParameterName = "ColoniaPoblacionP";
                    parametros[12].DbType = DbType.String;
                    parametros[12].Size = 60;

                    parametros[13] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[13].Direction = ParameterDirection.Input;
                    parametros[13].ParameterName = "DelegacionMunicipioP";
                    parametros[13].DbType = DbType.String;
                    parametros[13].Size = 40;

                    parametros[14] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[14].Direction = ParameterDirection.Input;
                    parametros[14].ParameterName = "CiudadP";
                    parametros[14].DbType = DbType.String;
                    parametros[14].Size = 40;

                    parametros[15] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[15].Direction = ParameterDirection.Input;
                    parametros[15].ParameterName = "EstadoMexicoP";
                    parametros[15].DbType = DbType.String;
                    parametros[15].Size = 4;

                    parametros[16] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[16].Direction = ParameterDirection.Input;
                    parametros[16].ParameterName = "CodigoPostalP";
                    parametros[16].DbType = DbType.String;
                    parametros[16].Size = 10;

                    parametros[17] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[17].Direction = ParameterDirection.Input;
                    parametros[17].ParameterName = "PersonaAccionistaP";
                    parametros[17].DbType = DbType.String;
                    parametros[17].Size = 15;

                    parametros[18] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[18].Direction = ParameterDirection.Input;
                    parametros[18].ParameterName = "EstadoExtranjeroP";
                    parametros[18].DbType = DbType.String;
                    parametros[18].Size = 40;

                    parametros[19] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[19].Direction = ParameterDirection.Input;
                    parametros[19].ParameterName = "PaisOrigenDomicilioP";
                    parametros[19].DbType = DbType.String;
                    parametros[19].Size = 2;

                    string storeBase = "SP_cargaMasiva_accionista";

                    // Procesamos la informacion
                    DataSet RegistrosRC = new DataSet();
                    RegistrosRC.Tables.Add(dtAccionistas);

                    DataTable dt_layout_procesado = new DataTable();

                    cargaMasiva.EliminaAccionistasTodos();
                    
                    dt_layout_procesado = cargaMasiva.cargaMasivaDS("cat_accionistas", dt_metaDataLayout, RegistrosRC, " * ", parametros, storeBase, persona);

                    int numeros = cargaMasiva.Correctos;
                    if (cargaMasiva.Errores > 0)
                    {
                        System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                    }

                    
                    Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <br/> <b style=\"color:red\">{1}</b>", numeros, 
                                        ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                    int total = RgdAccionistas.VirtualItemCount;
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);

                    RgdAccionistas.Rebind();

                }

                catch (Exception ex)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                    Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
                                        ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Detectados.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                }

                

                //esta aun no se sabe si se va a usar 
                //Mensajes.ShowAdvertencia(this.Page, this.GetType(), RegistrosLocalizadosActualizados + " Registros actualizados, " + RegistrosSinLocalizar + " registros sin coincidencias para actualizar, " + RegistrosLocalizadosError + " registros marcaron error al actualizar.");
            }
            else
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "La extensión de archivo debe ser XLS");
            }

        }
        catch
        {
            Mensajes.ShowMessage(this.Page, this.GetType(), "Error abriendo el archivo, puede que el documento este dañado o no tenga el formato correcto");
        }
    }

    private void ConsultarPais_Prueba(string ClavePais)
    {
        PaisRules getRecords = null;
        List<Pais> PaisesInfo;

        try
        {
            getRecords = new PaisRules();
            var s = getRecords.GetPaisPorClaveBuro(Enums.Persona.Moral, "0", ClavePais, true);
            PaisesInfo = s;
        }
        catch
        {
            Mensajes.ShowError(this.Page, this.GetType(), "Error al consultar el Pais por Clave del Buro");
        }
    }


}

