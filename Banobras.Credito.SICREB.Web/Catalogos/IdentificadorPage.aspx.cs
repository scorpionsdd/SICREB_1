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
using System.Globalization;

public partial class IdentificadorPage : System.Web.UI.Page
{
    public const String catalog = "Identificador";
    int idUs;
    string persona;

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            if (Session["Facultades"] != null)
            {
                persona = "PM";
                getFacultades(persona);

                if (!this.Page.IsPostBack)
                {
                    CambiaAtributosRGR();
                    
                    //JAGH se agregan actividades
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "Acceso al Cátalogo Dígito Identificador");
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
    
    //JAGH se modifica para mostrar titulos filtros
    public void CambiaAtributosRGR()
    {
        GridFilterMenu menu = RgdIdentificador.FilterMenu;
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
	
    protected void RgdIdentificador_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
	{
		IdentificadorRules getRecords = null;
		string Persona = string.Empty;
		List<Identificador> identificadorInfo;
		try
		{
            getRecords = new IdentificadorRules(Enums.Persona.Moral);
			identificadorInfo = getRecords.GetRecords(true);
			RgdIdentificador.DataSource = identificadorInfo;
		}
		catch (Exception exc)
		{
			Mensajes.ShowError(this.Page, this.GetType(), exc);
		}
	}

	protected void RgdIdentificador_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
	{

		GridDataItem item = (GridDataItem)e.Item;

	}

	protected void RgdIdentificador_UpdateCommand(object source, GridCommandEventArgs e)
	{
		Int32 idIdentificador = 0;
		string rfcIdentificador = string.Empty;
        string creditoIdentificador = string.Empty;
        string digitoIdentificador = string.Empty;
        string nombreIdentificador = string.Empty;

		Enums.Estado estado;

        ArrayList TipoPersona;
        
        Identificador identificadorOld;
		Identificador identificadorNew;

		
		GridEditableItem editedItem = e.Item as GridEditableItem;
		Hashtable newValues = new Hashtable();
		editedItem.ExtractValues(newValues);
		
		GridDataItem item = (GridDataItem)e.Item;
		try
		{
			//Datos Orginales
			idIdentificador = Parser.ToNumber(item.SavedOldValues["Id"]);

			//Datos Nuevos
			rfcIdentificador = newValues["Rfc"].ToString();
            creditoIdentificador = newValues["Credito"].ToString();
            digitoIdentificador = newValues["DigitoIdentificador"].ToString();
            nombreIdentificador = newValues["Nombre"].ToString();

            TipoPersona = Util.RadComboToString(item["TipoPersonaTemp"].FindControl("ComboPersona"));
            Enums.Persona persona = new Enums.Persona();

			estado = Enums.Estado.Activo;

			identificadorOld = new Identificador(idIdentificador, rfcIdentificador, creditoIdentificador, digitoIdentificador, nombreIdentificador, estado, persona);
            identificadorNew = new Identificador(idIdentificador, rfcIdentificador, creditoIdentificador, digitoIdentificador, nombreIdentificador, estado, persona);


			//Llamar el SP correspondiente con las entidades old y new de Validacion

			IdentificadorRules IdentificadorValores = new IdentificadorRules(Enums.Persona.Moral);
			//if (validaCampoNumerico(1,newValues["Porcentaje"].ToString()))
			//{
				if (IdentificadorValores.Update(identificadorOld, identificadorNew) > 0)
				{
                    idUs=Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(8888, idUs, rfcIdentificador + " " + creditoIdentificador + " " + digitoIdentificador + " " + nombreIdentificador);
					Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro se Modifico Correctamente");
				}
				else
				{
					Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El Registro no fue Modificado");
				}
			//}

		}
		catch (Exception exc)
		{
			Mensajes.ShowError(this.Page, this.GetType(), exc);
		}
	}

	protected void RgdIdentificador_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
	{


	}

	protected void RgdIdentificador_DeleteCommand(object source, GridCommandEventArgs e)
	{
		Identificador IdentificadorDelete;

		Int32 idIdentificador = 0;
		string rfcIdentificador = string.Empty;

//Datos para identificar valor

		GridDataItem item = (GridDataItem)e.Item;

		try
		{
			idIdentificador = Convert.ToInt32(item["Id"].Text);
			rfcIdentificador = item["Rfc"].Text.ToString();

            IdentificadorDelete = new Identificador(idIdentificador, rfcIdentificador, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo, Enums.Persona.Moral);

			//Llamar el SP correspondiente con las entidades old y new de Validacion
			IdentificadorRules IdentificadorBorrar = new IdentificadorRules(Enums.Persona.Moral);

			if (IdentificadorBorrar.Delete(IdentificadorDelete) > 0)
			{
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(1111, idUs, "Acreditado: " + rfcIdentificador);
				Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
			}
			else
			{
				Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue Removido");
			}

		}
		catch (Exception exc)
		{
			Mensajes.ShowError(this.Page, this.GetType(), exc);
		}
	}

	protected void RgdIdentificador_InsertCommand(object source, GridCommandEventArgs e)
	{
		InsertIdentificador(e);
	}

	private void InsertIdentificador(GridCommandEventArgs e)
	{
		IdentificadorRules IdentificadorInsertar;
		
        bool insertar = true;
		try
		{
			Identificador record = this.ValidaNulos(e.Item as GridEditableItem);
			
			if (e.Item is GridDataInsertItem)
			{   //si trae informacion valida hara la insercion
				if (record.Rfc != string.Empty)
				{
					if (insertar)
					{
						IdentificadorInsertar = new IdentificadorRules(Enums.Persona.Moral);
						if (IdentificadorInsertar.Insert(record) > 0)
						{
                            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                            ActividadRules.GuardarActividad(7777, idUs, "Identificador: " + record.Rfc);
							Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se guardó exitosamente");
						}
						else
						{
							Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue guardado");
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			e.Canceled = true;
			Mensajes.ShowError(this.Page, this.GetType(), ex);
		}
	}

	private Identificador ValidaNulos(GridEditableItem editedItem)
	{
		Hashtable newValues = new Hashtable();
		Identificador record;
		ArrayList TipoPersona;
		Enums.Persona persona;
		Enums.Estado estado;

		TipoPersona = Util.RadComboToString(editedItem["TipoPersonaTemp"].FindControl("ComboPersona"));

		if (TipoPersona[0].ToString() == "Moral" || TipoPersona[0].ToString() == "moral") { persona = Enums.Persona.Moral; } else { persona = Enums.Persona.Fisica; }
		estado = Enums.Estado.Activo;
		
		// Extrae todos los elementos
		editedItem.ExtractValues(newValues);
		if (newValues.Count > 0)
		{

			if (newValues["Rfc"] != null && newValues["Nombre"] != null && newValues["Credito"] != null && newValues["DigitoIdentificador"] != null)
			{

                record = new Identificador(0, newValues["Rfc"].ToString(), newValues["Credito"].ToString(), newValues["DigitoIdentificador"].ToString(), newValues["Nombre"].ToString(), estado, persona);


			}
			else
			{
				record = new Identificador(0, string.Empty, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo, Enums.Persona.Moral);
				Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de Verificar los Datos Ingresados");
			}
		}
		else
		{
            record = new Identificador(0, string.Empty, string.Empty, string.Empty, string.Empty, Enums.Estado.Activo, Enums.Persona.Moral);
		}
		return record;
	}

	protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
	{
        Response.ContentType = "application/force-download";
        RgdIdentificador.Columns[0].Visible = false;
          RgdIdentificador.Columns[RgdIdentificador.Columns.Count - 1].Visible = false;
        RgdIdentificador.MasterTableView.HierarchyDefaultExpanded = true;
        RgdIdentificador.ExportSettings.OpenInNewWindow = false;
        RgdIdentificador.ExportSettings.ExportOnlyData = true;
        RgdIdentificador.MasterTableView.GridLines = GridLines.Both;
        RgdIdentificador.ExportSettings.IgnorePaging = true;
        RgdIdentificador.ExportSettings.OpenInNewWindow = true;
        RgdIdentificador.ExportSettings.Pdf.PageHeight = Unit.Parse("280mm");
        RgdIdentificador.ExportSettings.Pdf.PageWidth = Unit.Parse("215mm");
        RgdIdentificador.MasterTableView.ExportToPdf();

        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(2222, idUs, "Digito Identificador");
      
	}

	protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
	{
        Response.ContentType = "application/force-download";
        RgdIdentificador.Columns[0].Visible = false;
        RgdIdentificador.Columns[RgdIdentificador.Columns.Count - 1].Visible = false;
        RgdIdentificador.MasterTableView.HierarchyDefaultExpanded = true;
        RgdIdentificador.ExportSettings.OpenInNewWindow = false;
        RgdIdentificador.ExportSettings.ExportOnlyData = true;

        RgdIdentificador.ExportSettings.IgnorePaging = true;
        RgdIdentificador.ExportSettings.OpenInNewWindow = true;
        RgdIdentificador.MasterTableView.ExportToExcel();

        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(3333, idUs, "Digito Identificador");
  
	}

	protected void RgdIdentificador_ItemDataBound(object sender, GridItemEventArgs e)
	{
		try
		{
			
			if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
			{
				GridDataItem item = (GridDataItem)e.Item;
				item["TipoPersonaTemp"].Text = item["Persona"].Text;
			}
			else if (e.Item.IsInEditMode) 
			{

				GridDataItem item;
				RadComboBox comboPersona;

				item = (GridDataItem)e.Item;
				comboPersona = (RadComboBox)item["TipoPersonaTemp"].FindControl("ComboPersona");
				GridEditableItem editedItem = e.Item as GridEditableItem;
				Hashtable newValues = new Hashtable();
				editedItem.ExtractValues(newValues);
				if (newValues["Persona"] != null)
				{
					if (newValues["Persona"].ToString() == "Fisica")
						comboPersona.SelectedIndex = 0;
					else
						comboPersona.SelectedIndex = 1;
				}
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

	public bool validaCampoNumerico(int type, string txtPorcentaje)
	{
		//1  Numerico
		bool valido = false;
		switch (type)
		{
			case 1:
				for (int n = 0; n < txtPorcentaje.Length; n++)
				{
					if (!Char.IsNumber(txtPorcentaje, n))
					{
						valido = false;
						Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Porcentaje debe ser un Numero Favor de Verificar");
						break;
					}
					else
						valido = true;
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
		this.RgdIdentificador.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
		//this.RgdIdentificador.MasterTableView.GetColumn("DeleteState").Visible = false;
		btnExportPDF.Visible = false;
		ImageButton1.Visible = false;

		if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_IDENTIFICADOR")))
		{
			this.RgdIdentificador.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
			valido = true;
		}
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_IDENTIFICADOR")))
		{
			//this.RgdIdentificador.MasterTableView.GetColumn("DeleteState").Visible = true;
			valido = true;
		}
		if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDPDF") + "|"))
			btnExportPDF.Visible = true;
		if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDEX")+"|"))
			ImageButton1.Visible = true;
		if (!valido)
		{
			Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario Válido", "~/Login.aspx");
		}
	
	}

	private bool facultadInsertar()
	{
		bool valido = false;
		UsuarioRules facultad = new UsuarioRules();
        if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_IDENTIFICADOR")))
		{
			valido = true;
		}
		return valido;
	}

    //JAGH 18/02/2013 se redimensionan cajas de filtros y das formato a datos numericos
	protected void RgdIdentificador_ItemCreated(object sender, GridItemEventArgs e)
	{
        if (e.Item is GridFilteringItem)
        {            
            GridFilteringItem filteringItem = e.Item as GridFilteringItem;
            
            //set dimensions for the filter textbox  
            TextBox tbRfc = filteringItem["Rfc"].Controls[0] as TextBox;
            RadNumericTextBox tbcredito = (RadNumericTextBox)filteringItem["credito"].Controls[0];
            TextBox tbDigitoIdentificador = filteringItem["DigitoIdentificador"].Controls[0] as TextBox;            
            TextBox tbNombre = filteringItem["Nombre"].Controls[0] as TextBox;
            tbRfc.Width = Unit.Pixel(100);
            tbcredito.Width = Unit.Pixel(60);
            tbcredito.NumberFormat.DecimalDigits = 0;
            tbcredito.NumberFormat.GroupSeparator = "";
            tbDigitoIdentificador.Width = Unit.Pixel(60);
            //tbDigitoIdentificador.ClientEvents.OnLoad = "AddZeros";
            tbDigitoIdentificador.Attributes.Add("onKeyPress", "javascript:return OnlyNumbers(event);");
            tbNombre.Width = Unit.Pixel(350);
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
                row1["nombreColumna"] = "RFC";
                row1["tipoDato"] = "VARCHAR2";
                row1["longitud"] = "25";

                DataRow row2;
                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "No Credito";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "25";

                DataRow row3;
                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Digito Id";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "25";

                DataRow row4;
                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "Nombre";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "255";                

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
                parametros[2].ParameterName = "pPersona";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 5;

                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "pRFC";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "pCredito";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 30;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "pDigitoIdentificador";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 25;

                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "pNombre";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 255;
                
                string storeBase = "SP_cargaMasiva_Identificador";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_identificador", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, persona);
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"),cargaMasiva.Log.ToString().Split('\n'));
                }
                    Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", (numeros), //((numeros>0)?(numeros- 1):numeros),
                        ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Erores Durante la carga.<br/> <a href=\"../Logs/log.txt\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                RgdIdentificador.Rebind();

                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Carga Masiva Digito Identificador Correctos: " + numeros + ((cargaMasiva.Errores > 0)? " Errores: " + (cargaMasiva.Errores) : ""));
            }
        }
        catch (Exception ex)
        {           
                System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"),cargaMasiva.Log.ToString().Split('\n'));

                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error En la carga del archivo<br/> {0}</b>", 
                ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Erores Detectados.<br/> <a href=\"../Logs/log.txt\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
            //RgdIdentificador.Rebind();
          //   Mensajes.ShowMessage(this.Page,this.GetType(),cargaMasiva.Log.ToString());
        }//try-Catch
    }

    protected void btn_eliminar_Click(object sender, EventArgs e)
    {
        try
        {

            CheckBox chkCell;

            bool chkResult;
               CheckBox chkHeader;
            GridHeaderItem headerItem = RgdIdentificador.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdIdentificador.AllowPaging = false;
                RgdIdentificador.Rebind();
                foreach (GridDataItem row in RgdIdentificador.Items)
                {
                    Identificador IdentificadorDelete;
                    Int32 idIdentificador = 0;
                    string rfcIdentificador = string.Empty;

                    //Datos para identificar valor
                    
                    idIdentificador = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    rfcIdentificador = row["Rfc"].Text.ToString();

                    IdentificadorDelete = new Identificador(idIdentificador, rfcIdentificador, string.Empty, string.Empty,
                                                        string.Empty, Enums.Estado.Activo, Enums.Persona.Moral);

                    //Llamar el SP correspondiente con las entidades old y new de Validacion
                    IdentificadorRules IdentificadorBorrar = new IdentificadorRules(Enums.Persona.Moral);

                    if (IdentificadorBorrar.Delete(IdentificadorDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        ActividadRules.GuardarActividad(1111, idUs, "Identificador: " + rfcIdentificador);
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue Removido");
                    }

                }//foreach
                RgdIdentificador.Rebind();
                RgdIdentificador.DataSource = null;
                RgdIdentificador.AllowPaging = true;
                RgdIdentificador.Rebind();
                RgdIdentificador.DataBind();

                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminación Masiva del catálogo: " + catalog, Convert.ToString(RgdIdentificador.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
            }//header
            else
            {
            foreach (GridDataItem row in RgdIdentificador.Items)
            {
                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkResult = (bool)chkCell.Checked;

                if (chkResult)
                {
                       Identificador IdentificadorDelete;
		                Int32 idIdentificador = 0;
		                string rfcAcreditado = string.Empty;
		                string rfcIdentificador = string.Empty;

                //Datos para identificar valor        

                        idIdentificador = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                        rfcIdentificador = row["Rfc"].Text.ToString();

			            IdentificadorDelete = new Identificador(idIdentificador, rfcIdentificador, 
                                                                string.Empty, string.Empty,
                                                                string.Empty, Enums.Estado.Activo, Enums.Persona.Moral);

			                //Llamar el SP correspondiente con las entidades old y new de Validacion
			                IdentificadorRules IdentificadorBorrar = new IdentificadorRules(Enums.Persona.Moral);

			                if (IdentificadorBorrar.Delete(IdentificadorDelete) > 0)
			                {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                                ActividadRules.GuardarActividad(1111, idUs, "Identificador: " + rfcAcreditado);
				                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
			                }
			                else
			                {
				                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue Removido");
			                }
                }//checket

            }//foreach Items


            }
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);
        }//try-catch
        //btn_eliminar_Click
        RgdIdentificador.Rebind();


        //btn_eliminar_Click
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
                foreach (GridDataItem row in RgdIdentificador.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdIdentificador.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = false;
                }
            }
        }
        catch (Exception exep)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exep);

        }//try-catch


        //ChkTodo_CheckedChanged
    }

    // jagh conservar nombres de filtros en español al filtrar dato
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

}
