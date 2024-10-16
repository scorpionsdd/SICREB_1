using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Business.Repositorios;
using Telerik.Web.UI;
using Banobras.Credito.SICREB.Data.Catalogos;
using System.Data;
using System.Data.Common;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;


public partial class Reportes_AdminHistorico : System.Web.UI.Page
{
    private int idUs;
    public const String catalog = "ArchivosHistoricos";
        
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CambiaAtributosRGR();
            this.idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            if (Session["Facultades"] != null)
            {
                getFacultades();                
            }
            else
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }

            if (!this.Page.IsPostBack)
            {
                ActividadRules.GuardarActividad(4444, this.idUs, "El Usuario " + Session["nombreUser"] + "Ingresó a Catálogo " + catalog);
            }
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();
        bool valido = false;
        btnExportPDF.Visible = false;
        ImageButton1.Visible = false;

        //ver donde va
        btnExportPDF.Visible = true;
        ImageButton1.Visible = true;

        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("GASEM") + "|"))
        {
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("GAMEN") + "|"))
        {
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("DAPF") + "|"))
        {
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("DAPM") + "|"))
        {
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("IA") + "|"))
        {
            valido = true;
        }

        if (!valido)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario Válido", "~/Login.aspx");
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
            else if (menu.Items[i].Text.Equals("StartsWith"))
                menu.Items[i].Text = "Inicie con";
            else if (menu.Items[i].Text.Equals("EndsWith"))
                menu.Items[i].Text = "Finalice con";
        }
    }
    
    protected void RgdCuentas_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ArchivosHistoricos_Rules getRecords = new ArchivosHistoricos_Rules();
        List<ArchivosHistoricos> cuentasInfo;
        List<string> lsCodigo = new List<string>();
        RgdCuentas.EnableLinqExpressions = false;

        try
        {
            cuentasInfo = getRecords.GetRecords(true);
            RgdCuentas.DataSource = cuentasInfo;
            RgdCuentas.VirtualItemCount = cuentasInfo.Count;

            for (int i = 0; i < cuentasInfo.Count; i++)
            {
                //lsCodigo.Add(cuentasInfo[i].Codigo);
                lsCodigo.Add(cuentasInfo[i].Id + "-" + cuentasInfo[i].Nombre + "-" + cuentasInfo[i].Url + "-" + cuentasInfo[i].Estatus); 
            }
            ViewState["Codigo"] = lsCodigo;
        }
        catch (Exception exc)
        {
            Mensajes.ShowError(this.Page, this.GetType(), exc);
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
    protected void btnExportPDF_Click1(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/force-download";
        //RgdCuentas.Columns[0].Visible = false;
        //RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
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
        //RgdCuentas.Columns[0].Visible = false;
        //RgdCuentas.Columns[RgdCuentas.Columns.Count - 1].Visible = false;
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
            }
            else if (e.Item.IsInEditMode)
            {
                GridDataItem items = (GridDataItem)e.Item;
            }

            if (e.Item is GridFilteringItem)
            {
                //GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                //RadComboBox combo = (RadComboBox)filterItem.FindControl("FiltroRadCombo");
                //combo.SelectedValue = SeleccionTipo.ToString();
            }
            //if (e.Item is GridCommandItem)
            //{
            //}
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }
    protected void ChkTodo_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void grids_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "download_file")
        {
            GridDataItem ditem = (GridDataItem)e.Item;
            string filename = ditem["Nombre"].Text;

            string path = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + filename));

            byte[] bts = System.IO.File.ReadAllBytes(path);
            Response.Clear();
            Response.ClearHeaders();
            Response.AddHeader("Content-Type", "Application/octet-stream");
            Response.AddHeader("Content-Length", bts.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.BinaryWrite(bts);
            Response.Flush();
            Response.End();
        }

        //sobre filtro y paginacion
        if (e.CommandName == RadGrid.FilterCommandName || e.CommandName == RadGrid.PageCommandName || e.CommandName.Equals("ChangePageSize") ||
             e.CommandName == RadGrid.PrevPageCommandArgument || e.CommandName == RadGrid.NextPageCommandArgument ||
             e.CommandName == RadGrid.FirstPageCommandArgument || e.CommandName == RadGrid.LastPageCommandArgument)
        {
            CambiaAtributosRGR();
        }
    }

    /*protected void lnkDownload_Click(object sender, EventArgs e)
    {
        string filePath = (sender as LinkButton).CommandArgument;

        //GridDataItem ditem = (GridDataItem)e.Item;
        string filename = filePath; // ditem["Nombre"].Text;

        string path = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + filename));

        byte[] bts = System.IO.File.ReadAllBytes(path);
        Response.Clear();
        Response.ClearHeaders();
        Response.AddHeader("Content-Type", "Application/octet-stream");
        Response.AddHeader("Content-Length", bts.Length.ToString());
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        Response.BinaryWrite(bts);
        Response.Flush();
        Response.End();
    }*/

    protected void btn_cargar_Click(object sender, EventArgs e)
    {
        cls_cargaArchivosHistorico cargaMasiva = new cls_cargaArchivosHistorico();
        try
        {
            if (file_txt_layout.HasFile)
            {
                string ruta_archivo = Path.GetFullPath(Server.MapPath("../tpmCatalogos/" + file_txt_layout.FileName));
                file_txt_layout.SaveAs(ruta_archivo);                
                Boolean seproceso = false;

                //string strCodigo = file_txt_layout.FileName;
                //if (bValidaCodigo(strCodigo)) { 

                //}
                //List<DbParameter> parametros = new List<DbParameter>();

                DbParameter[] parametros;
                parametros = new DbParameter[5];

                parametros[0] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[0].Direction = ParameterDirection.Output;
                parametros[0].ParameterName = "id_OUT";
                parametros[0].DbType = DbType.Decimal;
                parametros[0].Size = 38;                

                parametros[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].ParameterName = "Nombrep";
                parametros[1].DbType = DbType.String;
                parametros[1].Size = 100;
                parametros[1].Value = file_txt_layout.FileName;

                parametros[2] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].ParameterName = "urlp";
                parametros[2].DbType = DbType.String;
                parametros[2].Size = 300;
                parametros[2].Value = ruta_archivo;

                DateTime aDate = DateTime.Now;
                parametros[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].ParameterName = "fechap";
                parametros[3].DbType = DbType.DateTime;
                parametros[3].Size = 60;
                parametros[3].Value = aDate; //Convert.ToDateTime(aDate.ToString("MM/dd/yyyy HH:mm:ss"));

                parametros[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].ParameterName = "idusuariop";
                parametros[4].DbType = DbType.Decimal;
                parametros[4].Size = 38;
                parametros[4].Value = Parser.ToNumber(Session["idUsuario"].ToString());

                //id_OUT OUT number, Nombrep IN VARCHAR2,urlp IN VARCHAR2, fechap IN date,idusuariop NUMBER
                string storeBase = "PACKCUENTAS_IFRS.SP_CARGAMASIVA_ARCHIVOSH";
                seproceso = cargaMasiva.cargaMasiva("cat_cuentas", ruta_archivo, " * ", parametros, storeBase);
                //string msgFinal = string.Empty;
                if (seproceso == false)
                {
                    System.IO.File.WriteAllLines(Server.MapPath("../Logs/log.txt"), cargaMasiva.Log.ToString().Split('\n'), System.Text.Encoding.Unicode);
                    Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("El Archivo <br/> <b style=\"color:red\">{0}</b> no se ha cargado correctamente, favor de validarlo.", file_txt_layout.FileName));
                }
                else {
                    Mensajes.ShowMessage(this.Page, this.GetType(), String.Format("El Archivo <br/> <b style=\"color:green\">{0}</b> se ha cargado correctamente", file_txt_layout.FileName));
                    int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
                    ActividadRules.GuardarActividad(9999, idUs, "Se carga un archivo" + file_txt_layout.FileName, " reporte txt", " ", catalog, 1);
                    RgdCuentas.Rebind();
                }                
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
    }
    protected void RgdCuentas_UpdateCommand(object source, GridCommandEventArgs e)
    {        
    }
    public void RegresarPropiedades()
    {
    }
    public bool validaCampoNumerico(int type, string txtIdCalificacion, string txtGrupo)
    {
        //1  Numerico
        bool valido = false;        
        return valido;
    }
    protected void RgdCuentas_DeleteCommand(object source, GridCommandEventArgs e)
    {        
    }
    protected void RgdCuentas_InsertCommand(object source, GridCommandEventArgs e)
    {
    }
    private void InsertEstado(GridCommandEventArgs e)
    {        
    }
    private ArchivosHistoricos ValidaNulos(GridEditableItem editedItem)
    {        
        Hashtable newValues = new Hashtable();
        ArchivosHistoricos record = null;        
        return record;        
    }



    /*
    protected void btnSicofin2Mensual_Click(object sender, EventArgs e)
    {
        try
        {
            Sicofin2Business Sicofin2 = new Sicofin2Business();
            //Sicofin2.GeneraSicofinMensual();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
            //ActividadRules.GuardarActividad(iAct, this.idUs, "Seleccionado Cambios Reportes " + li.Text);  //JAGH
        }
    }      
    */

}