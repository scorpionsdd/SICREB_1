using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Common;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Data;
using System.Data;
using System.IO;
using System.Data.Common;

public partial class ActividadesEconomicasBanxicoPage : System.Web.UI.Page
{ 
  public const String catalog = "Actividad Economica Banxico";
    int idUs; 
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            CambiaAtributosRGR();

			if (Session["Facultades"] != null)
			{
				string persona = "PM";
				getFacultades(persona);
                if (!this.Page.IsPostBack)
                {                    
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(4444, idUs, "El Usuario " + Session["nombreUser"] + "Ingreso a Catalogo " + catalog);
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
    public void CambiaAtributosRGR()
    {

        GridFilterMenu menu = RgdBanxico.FilterMenu;
        for (int i = menu.Items.Count - 1; i >= 0; i--)
        {
            if (menu.Items[i].Text == "NoFilter")
            {
                menu.Items[i].Text = "Sin Filtro";
            }
            else if (menu.Items[i].Text == "EqualTo")
            {
                menu.Items[i].Text = "Igual";
            }
            else if (menu.Items[i].Text == "NotEqualTo")
            {
                menu.Items[i].Text = "Diferente";
            }
            else if (menu.Items[i].Text == "GreaterThan")
            {
                menu.Items[i].Text = "Mayor que";
            }
            else if (menu.Items[i].Text == "LessThan")
            {
                menu.Items[i].Text = "Menor que";
            }
            else if (menu.Items[i].Text == "GreaterThanOrEqualTo")
            {
                menu.Items[i].Text = "Mayor o igual a";
            }
            else if (menu.Items[i].Text == "LessThanOrEqualTo")
            {
                menu.Items[i].Text = "Menor o igual a";
            }
            else if (menu.Items[i].Text == "Between")
            {
                menu.Items[i].Text = "Entre";
            }
            else if (menu.Items[i].Text == "NotBetween")
            {
                menu.Items[i].Text = "No entre";
            }
            else if (menu.Items[i].Text == "IsNull")
            {
                menu.Items[i].Text = "Es nulo";
            }
            else if (menu.Items[i].Text == "NotIsNull")
            {
                menu.Items[i].Text = "No es nulo";
            }
            else if (menu.Items[i].Text == "Contains")
            {
                menu.Items[i].Text = "Contenga";
            }
            else if (menu.Items[i].Text == "DoesNotContain")
            {
                menu.Items[i].Text = "No Contenga";
            }
            else if (menu.Items[i].Text == "StartsWith")
            {
                menu.Items[i].Text = "Inicie con";
            }
            else if (menu.Items[i].Text == "EndsWith")
            {
                menu.Items[i].Text = "Finalice con";
            }
            else if (menu.Items[i].Text == "IsEmpty")
            {
                // item.Text = "Finalice con";
                menu.Items[i].Visible = false;
            }
            else if (menu.Items[i].Text == "NotIsEmpty")
            {
                // item.Text = "Finalice con";
                menu.Items[i].Visible = false;
            }   
        }

    }
	protected void RgdBanxico_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
	{
        BanxicoRules getRecords = null;
        List<string> lstClaveBuro = new List<string>();
        List<string> lstClaveClic = new List<string>();
		List<Banxico> BanxicoInfo;
		try
		{
			getRecords = new BanxicoRules();
			BanxicoInfo = getRecords.GetRecords(false);
			RgdBanxico.DataSource = BanxicoInfo;
            RgdBanxico.VirtualItemCount = BanxicoInfo.Count;

            ///BMS
            for (int i = 0; i < BanxicoInfo.Count; i++)
            {
                lstClaveBuro.Add(BanxicoInfo[i].ClaveBuro.ToString());
                lstClaveClic.Add(BanxicoInfo[i].ClaveCLIC.ToString());
            }

            ViewState["ClaveBuro"] = lstClaveBuro;
            ViewState["ClaveCLIC"] = lstClaveClic;
		}
		catch (Exception exc)
		{
			Mensajes.ShowError(this.Page, this.GetType(), exc);
		}
	}
	protected void RgdBanxico_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
	{
		
	}
	protected void RgdBanxico_UpdateCommand(object source, GridCommandEventArgs e)
    {
        ///BMS
        string claveClicOld = string.Empty;
        string claveClick = string.Empty;
        string claveBuroOld = string.Empty;
        string claveBuro = string.Empty;
        bool bvalidaCB = false;
        bool bvalidaCl = false;
        ///BMS
		int IdValue = 0;
		int IdTipo = 0;
		//ArrayList estatus;
		ArrayList type;
		Enums.Estado estado;
		Banxico banxicoOld;
		Banxico banxicoNew;
		BanxicoTipo  tipo;
		bool Actualizar = false;

		try
		{
			GridEditableItem editedItem = e.Item as GridEditableItem;
			Hashtable newValues = new Hashtable();
			editedItem.ExtractValues(newValues);

			GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;
            ///BMS
            claveClick = newValues["ClaveCLIC"].ToString();
            claveClicOld = item.SavedOldValues["ClaveCLIC"].ToString();
            claveBuro = newValues["ClaveBuro"].ToString();
            claveBuroOld = item.SavedOldValues["ClaveBuro"].ToString();

            if (claveClicOld != claveClick)
            {
                if (bValidaClaveCl(claveClick))
                {
                    bvalidaCl = true;
                }
            }
            else
                bvalidaCl = true;

            if (claveBuroOld != claveBuro)
            {
                if (bValidaClaveCB(claveBuro))
                {
                    bvalidaCB = true;
                }
            }
            else
                bvalidaCB = true;

            if (bvalidaCl && bvalidaCB)
            {

                Actualizar = validaCampo(1, newValues["ClaveCLIC"].ToString(), newValues["ClaveBuro"].ToString());

                if (Actualizar)
                {
                    //Datos Orginales
                    IdValue = Parser.ToNumber(item.SavedOldValues["Id"]);
                    //Datos para actualizar
                    Int32 claveClic = Parser.ToNumber(newValues["ClaveCLIC"]);
                    Int32 claveBuroNew = Parser.ToNumber(newValues["ClaveBuro"]);
                    string actividad = newValues["Actividad"].ToString();
                    estado = Enums.Estado.Activo;
                    type = Util.RadComboToString(item["TipoTemp"].FindControl("ComboTipo"));

                    IdTipo = Parser.ToNumber(type[1]);
                    tipo = new BanxicoTipo(IdTipo, type[0].ToString(), Enums.Estado.Activo);

                    banxicoOld = new Banxico(IdValue, claveClic, claveBuroNew, actividad, tipo, estado);
                    banxicoNew = new Banxico(IdValue, claveClic, claveBuroNew, actividad, tipo, estado);

                    //Aqui debo mandar llamar el SP correspondiente con las entidades old y new
                    BanxicoRules BanxicoUpdate = new BanxicoRules();


                    if (BanxicoUpdate.Update(banxicoOld, banxicoNew) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                        Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                        ActividadRules.GActividadCatalogo(8888, idUs, "Modificacion de Registro", null, null, catalog, 1, oldValues, newValues);

                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
                    }
                }
            }
		}
		catch (Exception ex)
		{
			Mensajes.ShowError(this.Page, this.GetType(), ex);
		}
		type = null;
	}

    private bool bValidaClaveCl(string sClaveClic)
    {
        bool Valido = true;

        List<string> lsSic = new List<string>();

        lsSic = (List<string>)ViewState["ClaveCLIC"];

        if (lsSic.FindIndex(s => s == sClaveClic) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Clave Buró : " + sClaveClic + " ya se encuentra dada de alta.");
        }

        return Valido;
    }

    private bool bValidaClaveCB(string sClaveBuro)
    {
        bool Valido = true;

        List<string> lsSic = new List<string>();

        lsSic = (List<string>)ViewState["ClaveBuro"];

        if (lsSic.FindIndex(s => s == sClaveBuro) >= 0)
        {
            Valido = false;

            Mensajes.ShowMessage(this.Page, this.GetType(), "La Clave Buró : " + sClaveBuro + " ya se encuentra dada de alta.");
        }

        return Valido;
    }

	protected void RgdBanxico_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
	{


	}
	protected void RgdBanxico_DeleteCommand(object source, GridCommandEventArgs e)
	{
		Banxico BanxicoDelete;
		int IdBanxico = 0;
		BanxicoTipo tipo = new BanxicoTipo(0, string.Empty, Enums.Estado.Activo);
		GridDataItem item = (GridDataItem)e.Item;

		try
		{

			IdBanxico = Convert.ToInt32(item["Id"].Text);
			BanxicoDelete = new Banxico(IdBanxico, 0, 0, string.Empty, tipo, Enums.Estado.Activo);
			//Aqui se llama el SP correspondiente para eliminar
			BanxicoRules BanxicoBorrar = new BanxicoRules();

			if (BanxicoBorrar.Delete(BanxicoDelete) > 0)
			{
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                
				Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminacion de Registro", null, null, catalog, 1,null, null);
			}
			else
			{
				Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
			}
		}
		catch (Exception ex)
		{
			Mensajes.ShowError(this.Page, this.GetType(), ex);
		}
		
	}
	protected void RgdBanxico_InsertCommand(object source, GridCommandEventArgs e)
	{
		InsertBanxico(e);
	}
	private void InsertBanxico(GridCommandEventArgs e)
	{
		BanxicoRules banxInsertar;
		
		try
		{
			Banxico  record = this.ValidaNulos(e.Item as GridEditableItem);

			if (e.Item is GridDataInsertItem)
			{
				if (record.ClaveBuro != 0 || record.ClaveCLIC != 0)
				{
                    if (bValidaClaveCl(record.ClaveCLIC.ToString()) && bValidaClaveCB(record.ClaveBuro.ToString()))
                    {
                        banxInsertar = new BanxicoRules();
                        if (banxInsertar.Insert(record) > 0)
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
			}

		}
		catch (Exception ex)
		{
			e.Canceled = true;
			Mensajes.ShowError(this.Page, this.GetType(), ex);
		}
	}
	private Banxico ValidaNulos(GridEditableItem editedItem)
	{
		Hashtable newValues = new Hashtable();
		Banxico record;
		int idTipo = 0;
		int clave = 0;
		int idClic = 0;
		string actividad = string.Empty;
		ArrayList type;
		BanxicoTipo tipo;

		// Extrae todos los elementos
		type = Util.RadComboToString(editedItem["TipoTemp"].FindControl("ComboTipo"));
		idTipo = Parser.ToNumber(type[1]);
		tipo = new BanxicoTipo(idTipo, type[0].ToString(),Enums.Estado.Activo);
	
		editedItem.ExtractValues(newValues);
		if (newValues.Count > 0)
		{
			if (newValues["ClaveBuro"] != null && newValues["ClaveCLIC"] != null && newValues["Actividad"] != null && tipo.Id != 0)
			{
				if (validaCampo(1, newValues["ClaveCLIC"].ToString(), newValues["ClaveBuro"].ToString()))
				{
					Enums.Estado estado = Enums.Estado.Activo;
					idClic = Parser.ToNumber(newValues["ClaveCLIC"]);
					clave = Parser.ToNumber(newValues["ClaveBuro"]);
					actividad = newValues["Actividad"].ToString();
					record = new Banxico(0, idClic, clave, actividad, tipo, estado);
                    idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GActividadCatalogo(7777, idUs, "Se Inserto un Registro", null, null, catalog, 1, null, newValues);
       
				}
				else
				{
					record = new Banxico(0, 0, 0, string.Empty, tipo, Enums.Estado.Activo);
				}
			}
			else
			{
				record = new Banxico(0, 0, 0, string.Empty, tipo, Enums.Estado.Activo);
				Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
			}
		}
		else
		{
			record = new Banxico(0, 0, 0, string.Empty, tipo, Enums.Estado.Activo);
		}

		type = null;
		return record;
	}


	protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
	{
        Response.ContentType = "application/force-download";
        RgdBanxico.Columns[0].Visible = false;
        RgdBanxico.Columns[RgdBanxico.Columns.Count - 1].Visible = false;
        RgdBanxico.MasterTableView.HierarchyDefaultExpanded = true;
        RgdBanxico.ExportSettings.OpenInNewWindow = false;
        RgdBanxico.ExportSettings.ExportOnlyData = true;
        RgdBanxico.MasterTableView.GridLines = GridLines.Both;
        RgdBanxico.ExportSettings.IgnorePaging = true;
        RgdBanxico.ExportSettings.OpenInNewWindow = true;
        RgdBanxico.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
        RgdBanxico.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
        RgdBanxico.MasterTableView.ExportToPdf();
         int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catalogo " + catalog);
      
	}
	protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
	{


        Response.ContentType = "application/force-download";
     
        RgdBanxico.Columns[0].Visible = false;
        RgdBanxico.Columns[RgdBanxico.Columns.Count - 1].Visible = false;
        RgdBanxico.MasterTableView.HierarchyDefaultExpanded = true;
        RgdBanxico.ExportSettings.OpenInNewWindow = false;
        RgdBanxico.ExportSettings.ExportOnlyData = true;

        RgdBanxico.ExportSettings.IgnorePaging = true;
        RgdBanxico.ExportSettings.OpenInNewWindow = true;
        RgdBanxico.MasterTableView.ExportToExcel();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catalogo " + catalog);
     //   RgdBanxico.Columns[6].Visible = false;
  
	}
	protected void RgdBanxico_ItemDataBound(object sender, GridItemEventArgs e)
	{

		try
		{

			if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
			{
				GridDataItem item = (GridDataItem)e.Item;
				item["TipoTemp"].Text = item["Tipo"].Text;
			}

			else if (e.Item.IsInEditMode)
			{

				GridDataItem item;
				BanxicoTiposRules catalogs;
				RadComboBox combo;

				item = (GridDataItem)e.Item;
				GridEditableItem editedItem = e.Item as GridEditableItem;
				Hashtable newValues = new Hashtable();
				editedItem.ExtractValues(newValues);

				catalogs = new BanxicoTiposRules();
				combo = (RadComboBox)item["TipoTemp"].FindControl("ComboTipo");
				combo.DataTextField = "Descripcion";
				combo.DataValueField = "Id";
				combo.DataSource = catalogs.GetRecords(false);
				combo.DataBind();

				if (newValues["Tipo.Descripcion"] != null)
				{
					combo.SelectedItem.Text = newValues["Tipo.Descripcion"].ToString();
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

	public bool validaCampo(int type, string txtClic, string txtClave )
	{
		//1  Numerico
		bool valido = false;
		switch( type )
		{
			case 1:
				for (int n = 0; n < txtClic.Length; n++)
				{
					if (!Char.IsNumber(txtClic, n))
					{
						valido = false;
						Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en los campos ID CLIC y Clave Buró deben ser numericos, favor de verificar");
						break;
					}
					else
						valido = true;
				}
				if (valido)
				{
					for (int n = 0; n < txtClave.Length; n++)
					{
						if (!Char.IsNumber(txtClave, n))
						{
							valido = false;
							Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Los datos en los campos ID CLIC y Clave Buró deben ser númericos, favor de verificar");
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
	private void getFacultades(string persona)
	{
		UsuarioRules facultad = new UsuarioRules();
		bool valido = false;
		this.RgdBanxico.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
		//this.RgdBanxico.MasterTableView.GetColumn("DeleteState").Visible = false;
		btnExportPDF.Visible = false;
		ImageButton1.Visible = false;
		if (persona == "PM")
		{

			if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_AEBANXICO")))
			{
				this.RgdBanxico.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
				valido = true;
			}
			if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_AEBANXICO")))
			{
				//this.RgdBanxico.MasterTableView.GetColumn("DeleteState").Visible = true;
				valido = true;
			}
			if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDPDF") + "|"))
				btnExportPDF.Visible = true;
			if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDEX")+"|"))
				ImageButton1.Visible = true;
		}
		else
		{
			if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_AEBANXICO")))
			{
				this.RgdBanxico.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
				valido = true;
			}
			if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_AEBANXICO")))
			{
				//this.RgdBanxico.MasterTableView.GetColumn("DeleteState").Visible = true;
				valido = true;
			}
			if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDPDF") + "|"))
				btnExportPDF.Visible = true;
			if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDEX")+"|"))
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
		if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_AEBANXICO")))
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
                row1["nombreColumna"] = "Clave CLIC";
                row1["tipoDato"] = "NUMBER";
                row1["longitud"] = "38";


                DataRow row2;

                row2 = dt_metaDataLayout.NewRow();
                row2["nombreColumna"] = "Clave Buró";
                row2["tipoDato"] = "VARCHAR2";
                row2["longitud"] = "38";


                DataRow row3;

                row3 = dt_metaDataLayout.NewRow();
                row3["nombreColumna"] = "Actividad";
                row3["tipoDato"] = "VARCHAR2";
                row3["longitud"] = "30";


                DataRow row4;

                row4 = dt_metaDataLayout.NewRow();
                row4["nombreColumna"] = "Tipo";
                row4["tipoDato"] = "VARCHAR2";
                row4["longitud"] = "30";

                DataRow row5;

                row5 = dt_metaDataLayout.NewRow();
                row5["nombreColumna"] = "Tipo Clave";
                row5["tipoDato"] = "VARCHAR2";
                row5["longitud"] = "30";


                dt_metaDataLayout.Rows.Add(row1);
                dt_metaDataLayout.Rows.Add(row2);
                dt_metaDataLayout.Rows.Add(row3);
                dt_metaDataLayout.Rows.Add(row4);
                dt_metaDataLayout.Rows.Add(row5);

                DbParameter[] parametros = new DbParameter[8];


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
                parametros[3].ParameterName = "idclicp";
                parametros[3].DbType = DbType.String;
                parametros[3].Size = 30;


                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "clavep";
                parametros[4].DbType = DbType.String;
                parametros[4].Size = 30;

                parametros[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].ParameterName = "actividadp";
                parametros[5].DbType = DbType.String;
                parametros[5].Size = 30;


                parametros[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].ParameterName = "tipop";
                parametros[6].DbType = DbType.String;
                parametros[6].Size = 100;
                parametros[7] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].ParameterName = "tipoidp";
                parametros[7].DbType = DbType.String;
                parametros[7].Size = 100;

                string storeBase = "SP_CM_BANXICO";

                dt_layout_procesado = cargaMasiva.cargaMasiva("cat_accionistas", dt_metaDataLayout, ruta_archivo, " * ", parametros, storeBase, "PM");
                int numeros = cargaMasiva.Correctos;
                if (cargaMasiva.Errores > 0)
                {

                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"),cargaMasiva.Log.ToString().Split('\n'),System.Text.Encoding.Unicode);
                }
                Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("{0} Registros Cargados Correctamente<br/> <b style=\"color:red\">{1}</b>", numeros,
                    ((cargaMasiva.Errores > 0) ? "" + (cargaMasiva.Errores) + " Errores Durante la carga.<br/> <a href=\"../wfLog.aspx\" target=\"_blank\" >Ver Detalle de Errores</a>" : "")));
                int total = RgdBanxico.VirtualItemCount;
                int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GuardarActividad(9999, idUs, "Se cargaron " + numeros + " registros al Catálogo " + catalog, total + " Registros", numeros + " Registros", catalog, 1);
               
                RgdBanxico.Rebind();
            }
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"),cargaMasiva.Log.ToString().Split('\n'),System.Text.Encoding.Unicode);
            Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("<b style=\"color:red\">Error en la carga del archivo<br/> {0}</b>",
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
            GridHeaderItem headerItem = RgdBanxico.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            chkHeader = (CheckBox)headerItem.FindControl("ChkTodo");
            if (chkHeader.Checked == true)
            {
                RgdBanxico.AllowPaging = false;
                RgdBanxico.Rebind();
                foreach (GridDataItem row in RgdBanxico.Items)
                {

                    Banxico BanxicoDelete;
                    int IdBanxico = 0;
                    BanxicoTipo tipo = new BanxicoTipo(0, string.Empty, Enums.Estado.Activo);

                    IdBanxico = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
                    BanxicoDelete = new Banxico(IdBanxico, 0, 0, string.Empty, tipo, Enums.Estado.Activo);
                    //Aqui se llama el SP correspondiente para eliminar
                    BanxicoRules BanxicoBorrar = new BanxicoRules();

                    if (BanxicoBorrar.Delete(BanxicoDelete) > 0)
                    {
                        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                        
                        Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                        ActividadRules.GActividadCatalogo(1111, idUs, "Eliminacion de Registro", null, null, catalog, 1,null, null);
                    }
                    else
                    {
                        Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
                    }

                }//foreach

                RgdBanxico.Rebind();
                RgdBanxico.DataSource = null;
                RgdBanxico.AllowPaging = true;
                RgdBanxico.Rebind();
                RgdBanxico.DataBind();
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(5555, idUs, "Eliminacion Masiva del catálogo: " + catalog, Convert.ToString( RgdBanxico.VirtualItemCount) + " Registros", "0 registros", catalog, 1, null, null);
           
            }//header
            else
            {
            foreach (GridDataItem row in RgdBanxico.Items)
            {

                chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                chkResult = (bool)chkCell.Checked;


                if (chkResult)
                {
                        Banxico BanxicoDelete;
		                int IdBanxico = 0;
		                BanxicoTipo tipo = new BanxicoTipo(0, string.Empty, Enums.Estado.Activo);
		
                        IdBanxico = Parser.ToNumber(row.GetDataKeyValue("Id").ToString());
			            BanxicoDelete = new Banxico(IdBanxico, 0, 0, string.Empty, tipo, Enums.Estado.Activo);

			            //Aqui se llama el SP correspondiente para eliminar
			            BanxicoRules BanxicoBorrar = new BanxicoRules();

			                if (BanxicoBorrar.Delete(BanxicoDelete) > 0)
			                {
                                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                                
				                Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminacion de Registro con id " + IdBanxico, null, null, catalog, 1, null, null);
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
            Response.Write(exep.Message.ToString());
        }

        RgdBanxico.Rebind();      
    }

    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
           CheckBox chkCell;
           CheckBox chkHeader;

           chkHeader = (CheckBox)sender;

           foreach (GridDataItem row in RgdBanxico.Items)
            {
              chkCell = (CheckBox)row.Cells[0].FindControl("chk");
              chkCell.Checked = chkHeader.Checked;
              row["TipoTemp"].Text = row["Tipo"].Text;
            }
           
        }
        catch (Exception exep)
        {
            Response.Write(exep.Message.ToString());
        }
    }
}