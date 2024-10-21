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

public partial class ReestructuradosRV : System.Web.UI.Page
{ 
  public const String catalog = "Reestructurados Con Clave De Observacion";
  private int idUs; //JAGH

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            		try
			{
	
				this.idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH
			}
			catch {;}

            if (Session["Facultades"] != null)
			{
				string persona = Request.QueryString["Persona"].ToString();
				getFacultades(persona);
                
                if (!this.Page.IsPostBack)
                {
                     CambiaAtributosRGR();                      
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

        GridFilterMenu menu = RgdCredCveObs.FilterMenu;
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
            else
                menu.Items.RemoveAt(i);
        }

    }
	protected void RgdCredCveObs_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
	{
        cls_accesoData ad = new cls_accesoData();
		try
		{

            var dt = ad.cmdtodt("TRANSACCIONALES.SP_TRANS_GetReestructuradosRV", new System.Data.Common.DbParameter[] { });
            RgdCredCveObs.DataSource = dt;


            RgdCredCveObs.VirtualItemCount = dt.Rows.Count;
		}
		catch (Exception exc)
		{
			Mensajes.ShowError(this.Page, this.GetType(), exc);
		}
	}
	protected void RgdCredCveObs_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
	{
		
	}
	protected void RgdCredCveObs_UpdateCommand(object source, GridCommandEventArgs e)
	{
		int IdValue = 0;
		string Credito = string.Empty;
		ArrayList IdCveObs;
		//ArrayList estatus;

		Enums.Estado estado;
		CreditoObservacion CredObsOld;
		CreditoObservacion CredObsNew;

		try
		{
			this.RgdCredCveObs.MasterTableView.GetColumn("IdCvesObservacion").Visible = false;
			this.RgdCredCveObs.MasterTableView.GetColumn("CveObservacion").Visible = true;
			GridEditableItem editedItem = e.Item as GridEditableItem;
			Hashtable newValues = new Hashtable();
			editedItem.ExtractValues(newValues);

			GridDataItem item = (GridDataItem)e.Item;
            Hashtable oldValues = (Hashtable)item.SavedOldValues;
   
			//Datos Orginales
			IdValue = Convert.ToInt32(item.SavedOldValues["Id"]);
			//Datos para actualizar
			Credito = newValues["Credito"].ToString();
			//estatus = Util.RadComboToString(item["EstatusTemp"].FindControl("ComboEstatus"));
			IdCveObs = Util.RadComboToString(item["CveObservacion"].FindControl("ComboObservacion"));


			//if (estatus[0].ToString() == "Activo") { estado = Enums.Estado.Activo; } else if (estatus[0].ToString() == "Inactivo") { estado = Enums.Estado.Inactivo; } else { estado = Enums.Estado.Test; }
			estado = Enums.Estado.Activo;

			CredObsOld = new CreditoObservacion(IdValue, Credito, Parser.ToNumber(IdCveObs[1]),"0", estado);
			CredObsNew = new CreditoObservacion(IdValue, Credito, Parser.ToNumber(IdCveObs[1]), "0", estado);

			//Aqui debo mandar llamar el SP correspondiente con las entidades old y new de Validacion
			CredCveObs_Rules CredCveObsUpdate = new CredCveObs_Rules();

			if (CredCveObsUpdate.Update(CredObsOld, CredObsNew) > 0)
			{
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());

                Mensajes.ShowMessage(this.Page, this.GetType(), "El registro se modificó correctamente");
                ActividadRules.GActividadCatalogo(8888, this.idUs, "Modificacion de Registro", null, null, catalog, 1, oldValues, newValues); //JAGH				
			}
			else
			{
				Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue modificado");
				//Response.Write("<script> alert('El Registro no fue Modificado'); </script>");
			}
		}
		catch (Exception ex)
		{
			Mensajes.ShowError(this.Page, this.GetType(), ex);
		}
	}
	protected void RgdCredCveObs_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
	{


	}
	protected void RgdCredCveObs_DeleteCommand(object source, GridCommandEventArgs e)
	{
		CreditoObservacion CredCveObsDelete;
		int Id = 0;
		string Credito = string.Empty;
		int IdCredCveObs = 0;

		GridDataItem item = (GridDataItem)e.Item;

		try
		{

			Id = Convert.ToInt32(item["Id"].Text);
			Credito = item["Credito"].Text;
			CredCveObsDelete = new CreditoObservacion(Id, Credito, IdCredCveObs,"0", Enums.Estado.Activo);

			//Aqui se llama el SP correspondiente para eliminar
			CredCveObs_Rules CreditoObsBorrar = new CredCveObs_Rules();

			if (CreditoObsBorrar.Delete(CredCveObsDelete) > 0)
			{
				idUs = Parser.ToNumber(Session["idUsuario"].ToString());
				
				Mensajes.ShowMessage(this.Page, this.GetType(), "El Registro fue removido Correctamente");
                ActividadRules.GActividadCatalogo(1111, idUs, "Eliminacion de Registro", null, null, catalog, 1,null, null);
				//Response.Write("<script> alert('El Registro fue removido Correctamente'); </script>");
			}
			else
			{
				Mensajes.ShowAdvertencia(this.Page, this.GetType(), "El registro no fue removido");
				//Response.Write("<script> alert('El Registro no fue Removido'); </script>");
			}
		}
		catch (Exception ex)
		{
			Mensajes.ShowError(this.Page, this.GetType(), ex);
		}
		
	}
	protected void RgdCredCveObs_InsertCommand(object source, GridCommandEventArgs e)
	{

		InsertCredCvObs(e);

	}
	private void InsertCredCvObs(GridCommandEventArgs e)
	{

		CredCveObs_Rules CredCveObsInsertar;
		try
		{
			CreditoObservacion record = this.ValidaNulos(e.Item as GridEditableItem);

			if (e.Item is GridDataInsertItem)
			{
				if (record.Id != 0 || record.IdCvesObservacion != 0)
				{
					CredCveObsInsertar = new CredCveObs_Rules();
					if (CredCveObsInsertar.Insert(record) > 0)
					{
						idUs = Parser.ToNumber(Session["idUsuario"].ToString());
						
						Mensajes.ShowMessage(this.Page,this.GetType(),"El registro se guardó exitosamente");
					}
					else
					{
						Mensajes.ShowAdvertencia(this.Page,this.GetType(),"El registro no fue guardado");
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
	private CreditoObservacion ValidaNulos(GridEditableItem editedItem)
	{
		Hashtable newValues = new Hashtable();
		CreditoObservacion record;
		ArrayList clave;
		// Extrae todos los elementos

		clave = Util.RadComboToString(editedItem["CveObservacion"].FindControl("ComboObservacion"));
		editedItem.ExtractValues(newValues);
		if (newValues.Count > 0)
		{

			if (newValues["Credito"] != null && Parser.ToNumber(clave[1]) != 0)
			{
				Enums.Estado estado = Enums.Estado.Activo;


                record = new CreditoObservacion(0, newValues["Credito"].ToString(), Parser.ToNumber(clave[1]), "0", estado);
                idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                ActividadRules.GActividadCatalogo(7777, idUs, "Se Inserto un Registro", null, null, catalog, 1, null, newValues);

			}
			else
			{
				record = new CreditoObservacion(0, "0", 0, "0", Enums.Estado.Activo);
				Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Favor de verificar los datos ingresados");
				//Response.Write("<script> alert('Favor de Verificar los Datos Ingresados')</script>");
			}
		}
		else
		{
			record = new CreditoObservacion(0, "0", 0, "0", Enums.Estado.Activo);
		}
		return record;
	}


	protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
	{
        Response.ContentType = "application/force-download";
        RgdCredCveObs.Columns[0].Visible = false;
        RgdCredCveObs.Columns[5].Visible = true;
        RgdCredCveObs.Columns[RgdCredCveObs.Columns.Count - 1].Visible = false;
        RgdCredCveObs.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCredCveObs.ExportSettings.OpenInNewWindow = false;
        RgdCredCveObs.ExportSettings.ExportOnlyData = true;

        RgdCredCveObs.ExportSettings.IgnorePaging = true;
        RgdCredCveObs.ExportSettings.OpenInNewWindow = true;
        RgdCredCveObs.MasterTableView.ExportToPdf();
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
         ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catalogo " + catalog);
      
	}
	protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
	{


        Response.ContentType = "application/force-download";
        RgdCredCveObs.Columns[0].Visible = false;
        RgdCredCveObs.Columns[5].Visible = true;
        RgdCredCveObs.Columns[RgdCredCveObs.Columns.Count - 1].Visible = false;
        RgdCredCveObs.MasterTableView.HierarchyDefaultExpanded = true;
        RgdCredCveObs.ExportSettings.OpenInNewWindow = false;
        RgdCredCveObs.ExportSettings.ExportOnlyData = true;

        RgdCredCveObs.ExportSettings.IgnorePaging = true;
        RgdCredCveObs.ExportSettings.OpenInNewWindow = true;
        RgdCredCveObs.MasterTableView.ExportToExcel();
  int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(3333, idUs, "El Usuario " + Session["nombreUser"] + "Exporto el Catalogo " + catalog);
  
	}
	protected void RgdCredCveObs_ItemDataBound(object sender, GridItemEventArgs e)
	{

		

	}
	private void getFacultades(string persona)
	{
		UsuarioRules facultad = new UsuarioRules(); 
		bool valido = false;
		//this.RgdCredCveObs.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
		//this.RgdCredCveObs.MasterTableView.GetColumn("DeleteState").Visible = false;
		btnExportPDF.Visible = false;
		ImageButton1.Visible = false;

		if (persona == "PM")
		{

			if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CREDOBS_M")))
			{
				//this.RgdCredCveObs.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
				valido = true;
			}
			if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CREDOBS_M")))
			{ 
				//this.RgdCredCveObs.MasterTableView.GetColumn("DeleteState").Visible = true;
				valido = true;
			}

			if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDPDF") + "|"))
				btnExportPDF.Visible = true;
			if (Session["Facultades"].ToString().Contains("|"+facultad.GetVariable("EDEX")+"|"))
				ImageButton1.Visible = true;
		}
		else
		{
			if (Session["Facultades"].ToString().Contains(facultad.GetVariable("MOD_CREDOBS_F")))
			{
				//this.RgdCredCveObs.MasterTableView.GetColumn("EditCommandColumn").Visible = true;
				valido = true;
			}

			if (Session["Facultades"].ToString().Contains(facultad.GetVariable("ELIM_CREDOBS_F")))
			{ 
		//		this.RgdCredCveObs.MasterTableView.GetColumn("DeleteState").Visible = true;
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
		if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CREDOBS_F")))
		{
			valido = true;
		}

		if (Session["Facultades"].ToString().Contains(facultad.GetVariable("AGREGA_CREDOBS_M")))
		{
			valido = true;
		}
		return valido;
	}

	protected void RgdCredCveObs_ItemCreated(object sender, GridItemEventArgs e)
	{


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
                foreach (GridDataItem row in RgdCredCveObs.Items)
                {
                    chkCell = (CheckBox)row.Cells[0].FindControl("chk");
                    chkCell.Checked = true;
                }
            }
            else
            {

                foreach (GridDataItem row in RgdCredCveObs.Items)
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
}
