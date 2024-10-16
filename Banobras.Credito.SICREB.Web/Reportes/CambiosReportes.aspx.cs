using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Banobras.Credito.SICREB.Business.Rules.PF;
using Banobras.Credito.SICREB.Business.Rules.PM;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Business.Repositorios;
using System;
using System.Collections;
using System.Reflection;
using System.Web.UI.WebControls;



public partial class Reportes_CambiosReportes : System.Web.UI.Page
{
    private PM_Cinta_Rules pmCintaRules = null;
    private PF_Cinta_Rules pfCintaRules = null;
    cls_ReportesCambios clsReporteCambios = null;
    Enums.Persona TipoPersona;
    const string GRUPOS = "6378,";

    private System.Data.DataTable dtArch01;
    private System.Data.DataTable dtArch02;
    private System.Data.DataTable dtResult;

    private int idUs; //JAGH

    #region Eventos de la pagina

    protected void Page_Load(object sender, EventArgs e)
    {
        try
	{
	
		this.idUs = Parser.ToNumber(Session["idUsuario"].ToString()); //JAGH
	}
	catch {;}

        CambioTipoPersona();

        if (lblAux.Text.Trim().Equals(String.Empty))
        {
            //CargaComboMes(TipoArch.Archivo.PrimerArch);
            //CargaComboMes(TipoArch.Archivo.SegundoArch);
            //CargaComboAnio
            CargaComboAnio(TipoArch.Archivo.PrimerArch);
            CargaComboAnio(TipoArch.Archivo.SegundoArch);
            lblAux.Text = "1";
        }
        //JAGH
        if (!this.Page.IsPostBack)
        {           
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso a cambios reportes");
        }
    }

    protected void rbPersona_SelectedIndexChanged(object sender, EventArgs e) 
    {
        //recorre elementos de la lista JAGH 17/01/13
        foreach (ListItem li in rbPersona.Items)
        {
            if (li.Selected)
            {
                int iAct = 6;
                if (li.Value.Equals("PM"))
                    iAct = 7;

                ActividadRules.GuardarActividad(iAct, this.idUs, "Seleccionado Cambios Reportes " + li.Text);  //JAGH
            }
            else
            {
                int iAct = 6;
                if (li.Value.Equals("PM"))
                    iAct = 7;
                ActividadRules.GuardarActividad(iAct, this.idUs, "Deseleccionado Cambios Reportes " + li.Text);  //JAGH
            }
        }
    }


    protected void cbMesReporte1_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e) {
        //CargaComboAnio(TipoArch.Archivo.PrimerArch);
        ////cbMesReporte1.Items.Remove(cbMesReporte1.Items.Count - 1);
        //if ((-1) != cbMesReporte1.Items.FindItemIndexByValue("-1"))
        //    cbMesReporte1.Items.Remove(cbMesReporte1.Items.FindItemIndexByValue("-1"));
        CargaComboArchivo(TipoArch.Archivo.PrimerArch);
        if ((-1) != cbMesReporte1.Items.FindItemIndexByValue("-1"))
            cbMesReporte1.Items.Remove(cbMesReporte1.Items.FindItemIndexByValue("-1"));

    }

    protected void cbAnioReporte1_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //CargaComboArchivo(TipoArch.Archivo.PrimerArch);
        //if ((-1) != cbAnioReporte1.Items.FindItemIndexByValue("-1"))
        //cbAnioReporte1.Items.Remove(cbAnioReporte1.Items.FindItemIndexByValue("-1"));
        CargaComboMes(TipoArch.Archivo.PrimerArch);
        if ((-1) != cbAnioReporte1.Items.FindItemIndexByValue("-1"))
            cbAnioReporte1.Items.Remove(cbAnioReporte1.Items.FindItemIndexByValue("-1"));
    }

    protected void cbArchivoReporte1_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if ((-1) != cbArchivoReporte1.Items.FindItemIndexByValue("-1"))
            cbArchivoReporte1.Items.Remove(cbArchivoReporte1.Items.FindItemIndexByValue("-1"));

        int iIdArchivo = 0, iAnio = 0;
        Int32.TryParse(cbArchivoReporte1.SelectedValue, out iIdArchivo);
        Int32.TryParse(cbAnioReporte1.SelectedValue, out iAnio);
        CargaArchivos(TipoArch.Archivo.PrimerArch, iIdArchivo, cbMesReporte1.Text, iAnio);
        LlenarRadGrid();
    }


    protected void cbMesReporte2_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e) {
        //CargaComboAnio(TipoArch.Archivo.SegundoArch);
        //cbMesReporte1.Items.Remove(cbMesReporte1.Items.Count - 1);
        CargaComboArchivo(TipoArch.Archivo.SegundoArch);
        if ((-1) != cbMesReporte2.Items.FindItemIndexByValue("-1"))
            cbMesReporte2.Items.Remove(cbMesReporte2.Items.FindItemIndexByValue("-1"));
    }

    protected void cbAnioReporte2_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //CargaComboArchivo(TipoArch.Archivo.SegundoArch);
        CargaComboMes(TipoArch.Archivo.SegundoArch);
        if ((-1) != cbAnioReporte2.Items.FindItemIndexByValue("-1"))
            cbAnioReporte2.Items.Remove(cbAnioReporte2.Items.FindItemIndexByValue("-1"));
    }

    protected void cbArchivoReporte2_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if ((-1) != cbArchivoReporte2.Items.FindItemIndexByValue("-1"))
            cbArchivoReporte2.Items.Remove(cbArchivoReporte2.Items.FindItemIndexByValue("-1"));

        int iIdArchivo = 0, iAnio = 0;
        Int32.TryParse(cbArchivoReporte2.SelectedValue, out iIdArchivo);
        Int32.TryParse(cbAnioReporte2.SelectedValue, out iAnio);
        CargaArchivos(TipoArch.Archivo.SegundoArch, iIdArchivo, cbMesReporte2.Text, iAnio);
        LlenarRadGrid();
    }

    protected void rgReportPrueba_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e) {
        if (Session["Result"] != null)
            dtResult = Session["Result"] as System.Data.DataTable;
        else {
            if (Session["TipoPersona"] != null)
            {
                TipoPersona = (Enums.Persona)Session["TipoPersona"];
                dtResult = cls_ReportesCambios.CrearTabla(TipoPersona);
            }
        }
        try
        {
            if (TipoPersona == Enums.Persona.Moral)
            {
              this.rgCambiosPM.Visible = true;
              this.rgCambiosPM.DataSource = dtResult;
              this.rgCambiosPF.Visible = false;

            }
            else
            {
                this.rgCambiosPF.Visible = true;
                this.rgCambiosPF.DataSource = dtResult;
                this.rgCambiosPM.Visible = false;
            }
        }
        catch (Exception ex)
        {
            string strMessageError = ex.Message.ToString();
        }
    }

    protected void rgCambiosPF_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (Session["Coordenadas"] != null)
        {
            List<clsCoordenadas> listCord = Session["Coordenadas"] as List<clsCoordenadas>;
            //string strA = string.Empty;
            Telerik.Web.UI.GridDataItem item = e.Item as Telerik.Web.UI.GridDataItem;
            if (item != null)
            {
                foreach (clsCoordenadas coor in listCord)
                {
                    if (item.ItemIndex == coor.Row)
                        item.Cells[coor.Column + 2].BackColor = System.Drawing.Color.Yellow;
                }
            }
        }

        if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem )
        {

            Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;

            if (item != null)
            {
              item["Fecha de Última Disposición"].Text = item["Fecha de Última Disposición"].Text!= "&nbsp;"? Convert.ToDateTime(item["Fecha de Última Disposición"].Text).ToShortDateString(): string.Empty;
              item["Fecha de Nacimiento"].Text = item["Fecha de Nacimiento"].Text != "&nbsp;" ? Convert.ToDateTime(item["Fecha de Nacimiento"].Text).ToShortDateString() : string.Empty; 
            }
        }
            
    }

    protected void rgCambiosPM_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (Session["Coordenadas"] != null)
        {
            List<clsCoordenadas> listCord = Session["Coordenadas"] as List<clsCoordenadas>;
            //string strA = string.Empty;
            Telerik.Web.UI.GridDataItem item = e.Item as Telerik.Web.UI.GridDataItem;
            if (item != null)
            {
                foreach (clsCoordenadas coor in listCord)
                {
                    if (item.ItemIndex == coor.Row)
                        item.Cells[coor.Column + 2].BackColor = System.Drawing.Color.Yellow;
                }
            }
        }
    }



    #endregion

    private void LlenarRadGrid()
    {
        //Session["PrimerArch"] as System.Data.DataTable;
        if (Session["PrimerArch"] != null && Session["SegundoArch"] != null)
        {
            dtArch01 = Session["PrimerArch"] as System.Data.DataTable;
            dtArch02 = Session["SegundoArch"] as System.Data.DataTable;
            dtResult = cls_ReportesCambios.GetTablaCambios(dtArch01, dtArch02);
            Session.Add("Result", dtResult);
            try
            {

                if (TipoPersona == Enums.Persona.Moral)
                {
                    this.rgCambiosPM.Visible = true;
                    this.rgCambiosPM.DataSource = dtResult;
                    this.rgCambiosPM.DataBind();
                    this.rgCambiosPF.Visible = false;
                }
                else
                {
                    this.rgCambiosPF.Visible = true;
                    this.rgCambiosPF.DataSource = dtResult;
                    this.rgCambiosPF.DataBind();
                    this.rgCambiosPM.Visible = false;
                }
                
                List<clsCoordenadas> listCord = new List<clsCoordenadas>();
                listCord = PintarDataGrig(dtResult);
                Session.Add("Coordenadas", listCord);
            }
            catch (Exception ex)
            {
                string strMessageError = ex.Message.ToString();
            }
        }
    }

    private List<clsCoordenadas> PintarDataGrig(System.Data.DataTable dtTable)
    {
        //DataTable dtResultAux = (DataTable)dgvArchivo.DataSource;
        List<clsCoordenadas> listCord = new List<clsCoordenadas>();
        //System.Data.DataTable dtResultAux = (System.Data.DataTable)rgReportPrueba.DataSource;
        System.Data.DataTable dtResultAux = dtTable;
        string strIndexRows = string.Empty;
        int iCount = 0;
        foreach (System.Data.DataRow dr in dtResultAux.Rows)
        {
            object[] objCampos = dr.ItemArray;
            if (!objCampos[0].ToString().Trim().Equals(string.Empty))
                strIndexRows = strIndexRows + iCount.ToString() + ",";
            else
                strIndexRows = strIndexRows + " ,";
            iCount++;
        }
        string stra = string.Empty;
        string[] arrRows = strIndexRows.Split(' ');

        foreach (string strRows in arrRows)
        {
            string strAux = string.Empty;
            if (!strRows.Substring(0, 1).Trim().Equals(","))
                //if(strRows.Trim().Split(',').Length == 3)
                strAux = strRows.Trim().Substring(0, strRows.Trim().Length - 1);
            else
                if (strRows.Trim().Split(',').Length > 3)
                    strAux = strRows.Trim().Substring(1, strRows.Trim().Length - 2);
            //int coun = strAux.Trim().Split(',').Length;
            string[] arrIndex = strAux.Trim().Split(',');
            //if ((arrIndex.Length % 2) == 0)
            //{
            int idxA = 0;
            int idxB = 0;
            string strIndexColums = string.Empty;
            for (int idxRows = 0; idxRows < arrIndex.Length - 1; idxRows++)
            {
                Int32.TryParse(arrIndex[idxRows].Trim(), out idxA);
                Int32.TryParse(arrIndex[idxRows + 1].Trim(), out idxB);
                //for (int c = 2; c < rgReportPrueba.Columns.Count; c++)
                //{
                //    //rgReportPrueba.Items[idxA].Cells[c].Text.ToString().Trim()
                //    //if (!rgReportPrueba.Rows[idxA].Cells[c].Value.ToString().Trim().Equals(rgReportPrueba.Rows[idxB].Cells[c].Value.ToString().Trim()))
                //    if (rgReportPrueba.Items[idxA].Cells[c].Text.ToString().Trim().Equals(rgReportPrueba.Items[idxB].Cells[c].Text.ToString().Trim()))
                //    {
                //        //dgvArchivo.Rows[idxA].Cells[c].Style.BackColor = Color.Yellow;
                //        //dgvArchivo.Rows[idxB].Cells[c].Style.BackColor = Color.Yellow;
                //        strIndexColums = strIndexColums + c + ",";
                //    }
                //}
                for (int c = 2; c < dtResultAux.Columns.Count; c++)
                {
                    if (!dtResultAux.Rows[idxA][c].ToString().Trim().Equals(dtResultAux.Rows[idxB][c].ToString().Trim()))
                        strIndexColums = strIndexColums + c + ",";
                }
                //dgvArchivo.Rows[]
            }
            strAux = strIndexColums.Trim().Substring(0, strIndexColums.Trim().Length - 1);
            string[] arrIndexC = strAux.Trim().Split(',');
            for (int idxRows = 0; idxRows < arrIndex.Length - 1; idxRows++)
            {
                Int32.TryParse(arrIndex[idxRows].Trim(), out idxA);
                Int32.TryParse(arrIndex[idxRows + 1].Trim(), out idxB);
                for (int c = 0; c < arrIndexC.Length; c++)
                {
                    //if (!dgvArchivo.Rows[idxA].Cells[c].Value.ToString().Trim().Equals(dgvArchivo.Rows[idxB].Cells[c].Value.ToString().Trim()))
                    //{
                    int iAux = 0;
                    Int32.TryParse(arrIndexC[c].Trim(), out iAux);
                    clsCoordenadas coor = new clsCoordenadas();
                    coor.Row = idxA;
                    coor.Column = iAux;
                    listCord.Add(coor);
                    coor = new clsCoordenadas();
                    coor.Row = idxB;
                    coor.Column = iAux;
                    listCord.Add(coor);
                    //rgReportPrueba.Rows[idxA].Cells[iAux].Style.BackColor = Color.Yellow;
                    //rgReportPrueba.Rows[idxB].Cells[iAux].Style.BackColor = Color.Yellow;
                    //rgReportPrueba.Items[idxA].Cells[iAux].Style["BackColor"] = "#FF0000";
                    //strIndexColums = strIndexColums + c + ",";
                    //}
                }
                //dgvArchivo.Rows[]
            }

            //}
        }
        return listCord;
    }

    //private void PintarDataGrig()
    //{
    //    //DataTable dtResult = (DataTable)dgvArchivo.DataSource;
    //    System.Data.DataTable dtResult = (System.Data.DataTable)rgReportPrueba.DataSource;
    //    string strIndexRows = string.Empty;
    //    int iCount = 0;
    //    foreach (System.Data.DataRow dr in dtResult.Rows)
    //    {
    //        object[] objCampos = dr.ItemArray;
    //        if (!objCampos[0].ToString().Trim().Equals(string.Empty))
    //            strIndexRows = strIndexRows + iCount.ToString() + ",";
    //        else
    //            strIndexRows = strIndexRows + " ,";
    //        iCount++;
    //    }
    //    string stra = string.Empty;
    //    string[] arrRows = strIndexRows.Split(' ');

    //    foreach (string strRows in arrRows)
    //    {
    //        string strAux = string.Empty;
    //        if (!strRows.Substring(0, 1).Trim().Equals(","))
    //            //if(strRows.Trim().Split(',').Length == 3)
    //            strAux = strRows.Trim().Substring(0, strRows.Trim().Length - 1);
    //        else
    //            if (strRows.Trim().Split(',').Length > 3)
    //                strAux = strRows.Trim().Substring(1, strRows.Trim().Length - 2);
    //        //int coun = strAux.Trim().Split(',').Length;
    //        string[] arrIndex = strAux.Trim().Split(',');
    //        //if ((arrIndex.Length % 2) == 0)
    //        //{
    //        int idxA = 0;
    //        int idxB = 0;
    //        string strIndexColums = string.Empty;
    //        for (int idxRows = 0; idxRows < arrIndex.Length - 1; idxRows++)
    //        {
    //            Int32.TryParse(arrIndex[idxRows].Trim(), out idxA);
    //            Int32.TryParse(arrIndex[idxRows + 1].Trim(), out idxB);
    //            for (int c = 2; c < rgReportPrueba.Columns.Count; c++)
    //            {
    //                //rgReportPrueba.Items[idxA].Cells[c].Text.ToString().Trim()
    //                //if (!rgReportPrueba.Rows[idxA].Cells[c].Value.ToString().Trim().Equals(rgReportPrueba.Rows[idxB].Cells[c].Value.ToString().Trim()))
    //                if (rgReportPrueba.Items[idxA].Cells[c].Text.ToString().Trim().Equals(rgReportPrueba.Items[idxB].Cells[c].Text.ToString().Trim()))
    //                {
    //                    //dgvArchivo.Rows[idxA].Cells[c].Style.BackColor = Color.Yellow;
    //                    //dgvArchivo.Rows[idxB].Cells[c].Style.BackColor = Color.Yellow;
    //                    strIndexColums = strIndexColums + c + ",";
    //                }
    //            }
    //            //dgvArchivo.Rows[]
    //        }
    //        strAux = strIndexColums.Trim().Substring(0, strIndexColums.Trim().Length - 1);
    //        string[] arrIndexC = strAux.Trim().Split(',');
    //        for (int idxRows = 0; idxRows < arrIndex.Length - 1; idxRows++)
    //        {
    //            Int32.TryParse(arrIndex[idxRows].Trim(), out idxA);
    //            Int32.TryParse(arrIndex[idxRows + 1].Trim(), out idxB);
    //            for (int c = 0; c < arrIndexC.Length; c++)
    //            {
    //                //if (!dgvArchivo.Rows[idxA].Cells[c].Value.ToString().Trim().Equals(dgvArchivo.Rows[idxB].Cells[c].Value.ToString().Trim()))
    //                //{
    //                int iAux = 0;
    //                Int32.TryParse(arrIndexC[c].Trim(), out iAux);
    //                //rgReportPrueba.Rows[idxA].Cells[iAux].Style.BackColor = Color.Yellow;
    //                //rgReportPrueba.Rows[idxB].Cells[iAux].Style.BackColor = Color.Yellow;
    //                rgReportPrueba.Items[idxA].Cells[iAux].Style["BackColor"] = "#FF0000";
    //                //strIndexColums = strIndexColums + c + ",";
    //                //}
    //            }
    //            //dgvArchivo.Rows[]
    //        }

    //        //}
    //    }
    //    string strA = string.Empty;
    //}

    private void CargaComboMes(TipoArch.Archivo tipoArch) {
        TipoPersona = (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica);
        clsReporteCambios = new cls_ReportesCambios(TipoPersona);
        if (tipoArch == TipoArch.Archivo.PrimerArch)
        {
            int iAnioAux1 = 0;
            Int32.TryParse(cbAnioReporte1.SelectedValue, out iAnioAux1);
            FillCombos(cbMesReporte1, clsReporteCambios.GetMesesArchivo(iAnioAux1), "Nombre", "Id");
            cbMesReporte1.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Selecciona el Mes", "-1"));
            cbMesReporte1.SelectedValue = "-1";
        }
        else
        {
            int iAnioAux2 = 0;
            Int32.TryParse(cbAnioReporte2.SelectedValue, out iAnioAux2);
            FillCombos(cbMesReporte2, clsReporteCambios.GetMesesArchivo(iAnioAux2), "Nombre", "Id");
            cbMesReporte2.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Selecciona el Mes", "-1"));
            cbMesReporte2.SelectedValue = "-1";
        }
    }

    private void CargaComboAnio(TipoArch.Archivo tipoArch) {
        TipoPersona = (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica);
        clsReporteCambios = new cls_ReportesCambios(TipoPersona);
        //List<int> num = clsReporteCambios.GetAllYear();
        if (tipoArch == TipoArch.Archivo.PrimerArch)
        {
            //int iMesAux1 = 0;
            //Int32.TryParse(cbMesReporte1.SelectedValue, out iMesAux1);
            FillCombos(cbAnioReporte1, clsReporteCambios.GetAniosArchivo(), "Nombre", "Id");
            cbAnioReporte1.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Selecciona el Año", "-1"));
            cbAnioReporte1.SelectedValue = "-1";
            //cbAnioReporte1_SelectedIndexChanged(sender, null);
        }
        else
        {
            //int iMesAux2 = 0;
            //Int32.TryParse(cbMesReporte2.SelectedValue, out iMesAux2);
            FillCombos(cbAnioReporte2, clsReporteCambios.GetAniosArchivo(), "Nombre", "Id");
            cbAnioReporte2.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Selecciona el Año", "-1"));
            cbAnioReporte2.SelectedValue = "-1";
            //cbAnioReporte2_SelectedIndexChanged(sender, null);
        }
    }

    private void CambioTipoPersona() {
        if (Session["TipoPersona"] != null)
            TipoPersona = (Enums.Persona)Session["TipoPersona"];
        if (TipoPersona != (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica))
            lblAux.Text = string.Empty;
        TipoPersona = (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica);

        if (TipoPersona == Enums.Persona.Moral)
            {
              this.rgCambiosPM.Visible = true;           
              this.rgCambiosPF.Visible = false;

            }
            else
            {
                this.rgCambiosPF.Visible = true;              
                this.rgCambiosPM.Visible = false;
            }

        //Session.Add("erroresInfoPF", errores.GetErroresPresentacion(Enums.Persona.Fisica, 1));
        Session.Add("TipoPersona", TipoPersona);
    }

    private void CargaComboArchivo(TipoArch.Archivo tipoArch)
    {
        TipoPersona = (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica);
        if (tipoArch == TipoArch.Archivo.PrimerArch)
        {
            int iMesAux1 = 0;
            int iAnioAux1 = 0;
            Int32.TryParse(cbMesReporte1.SelectedValue, out iMesAux1);
            Int32.TryParse(cbAnioReporte1.SelectedValue, out iAnioAux1);
            clsReporteCambios = new cls_ReportesCambios(TipoPersona);
            FillCombos(cbArchivoReporte1, clsReporteCambios.GetNombreArchivo(iMesAux1, iAnioAux1), "Nombre", "Id");
            cbArchivoReporte1.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Selecciona el Archivo", "-1"));
            cbArchivoReporte1.SelectedValue = "-1";
        }
        else
        {
            int iMesAux2 = 0;
            int iAnioAux2 = 0;
            Int32.TryParse(cbMesReporte2.SelectedValue, out iMesAux2);
            Int32.TryParse(cbAnioReporte2.SelectedValue, out iAnioAux2);
            clsReporteCambios = new cls_ReportesCambios(TipoPersona);
            FillCombos(cbArchivoReporte2, clsReporteCambios.GetNombreArchivo(iMesAux2, iAnioAux2), "Nombre", "Id");
            cbArchivoReporte2.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Selecciona el Archivo", "-1"));
            cbArchivoReporte2.SelectedValue = "-1";
        }
    }

    private void CargaArchivos(TipoArch.Archivo tipoArch, int iIdArchivo, string strMes, int iAnio)
    {
        if (Session["TipoPersona"] != null)
            TipoPersona = (Enums.Persona)Session["TipoPersona"];
        System.Data.DataTable dtArchivoPM = new System.Data.DataTable();
        System.Data.DataTable dtArchivoPF = new System.Data.DataTable();
        //string strGrupos = string.Empty;
        if (tipoArch == TipoArch.Archivo.PrimerArch)
        {
            if (TipoPersona == Enums.Persona.Moral)
            {
                dtArchivoPM = CargaArchivoPM(iIdArchivo, GRUPOS, strMes, iAnio);
                Session.Add("PrimerArch", dtArchivoPM);
            }
            else {
                dtArchivoPF = CargaArchivoPF(iIdArchivo, GRUPOS, strMes, iAnio);
                Session.Add("PrimerArch", dtArchivoPF);
            }
        }
        else {
            if (TipoPersona == Enums.Persona.Moral)
            {
                dtArchivoPM = CargaArchivoPM(iIdArchivo, GRUPOS, strMes, iAnio);
                Session.Add("SegundoArch", dtArchivoPM);
            } else {
                dtArchivoPF = CargaArchivoPF(iIdArchivo, GRUPOS, strMes, iAnio);
                Session.Add("SegundoArch", dtArchivoPF);
            }
        }
    }

    private System.Data.DataTable CargaArchivoPM(int iIdArchivo, string grupos, string strMes, int iAnio)
    {
        System.Data.DataTable dtArchivoPM = new System.Data.DataTable();

        TipoPersona = (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica);
        clsReporteCambios = new cls_ReportesCambios(TipoPersona);
        dtArchivoPM = clsReporteCambios.GeneraArchivoPM(iIdArchivo, grupos, strMes, iAnio);
        return dtArchivoPM;
    }

    private System.Data.DataTable CargaArchivoPF(int iIdArchivo, string grupos, string strMes, int iAnio)
    {
        System.Data.DataTable dtArchivoPF = new System.Data.DataTable();
        
        TipoPersona = (rbPersona.SelectedValue.Equals("PM") ? Enums.Persona.Moral : Enums.Persona.Fisica);
        clsReporteCambios = new cls_ReportesCambios(TipoPersona);
        dtArchivoPF = clsReporteCambios.GeneraArchivoPF(iIdArchivo, grupos, strMes, iAnio);
        return dtArchivoPF;
    }

    private void FilterArchivos() {
        try { }
        catch (Exception ex) {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    private void FillCombos(Telerik.Web.UI.RadComboBox Combo, System.Data.DataTable dt, string TexField, string ValueField) {
        Combo.DataSource = dt;
        Combo.DataTextField = TexField;
        Combo.DataValueField = ValueField;
        Combo.DataBind();
        Combo.SelectedIndex = -1;
    }

    private void ArchivoPM() {
        //clsReporteCambios = new cls_ReportesCambios(TipoPersona);
        //Banobras.Credito.SICREB.Entities.Types.PM_Cinta cintaPM = new Banobras.Credito.SICREB.Entities.Types.PM_Cinta();
        //cintaPM = clsReporteCambios.GeneraArchivoPM(1533, grupos);

    }
    
}
public class clsCoordenadas
{
    private int _Row;
    private int _Column;

    public int Row
    {
        get { return _Row; }
        set { this._Row = value; }
    }

    public int Column
    {
        get { return _Column; }
        set { this._Column = value; }
    }

}