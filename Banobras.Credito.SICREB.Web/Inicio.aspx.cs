using Banobras.Credito.SICREB.Business;
using Banobras.Credito.SICREB.Business.Guias_Contables;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Business.Rules;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.SICOFIN;
using Banobras.Credito.SICREB.Entities.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Charting;
using Telerik.Web.UI.Calendar;

public partial class Inicio : System.Web.UI.Page
{
    private int idUs;
    ConfiguracionRules configuracionReglas = new ConfiguracionRules();

    protected void Page_Load(object sender, EventArgs e)
    {
        RadChartPersonasFisicas.ChartTitle.Remove();
        RadChartPersonasMorales.ChartTitle.Remove();
        RadChartHistorico.ChartTitle.Remove();
        RadChartConciliacion.ChartTitle.Remove();

        try
        {
            this.idUs = Parser.ToNumber(Session["idUsuario"].ToString()); // JAGH se obtiene idUsuario 15/01/13
        }
        catch { ; }

        if (!Page.IsPostBack)
        {
            //****************** psl
            Configuracion sicofin = configuracionReglas.GetRecords("SICOFIN");
            Session["sicofin"] = sicofin;
            Configuracion sic = configuracionReglas.GetRecords("SIC");
            Configuracion calificacion = configuracionReglas.GetRecords("CALIFICACIONES");

            LblSicofin.Text = (sicofin.Catsicofin == 1) ? "Catálogo Anterior" : "Catálogo IFRS9";
            LblSic.Text = (sic.Catsic == 1) ? "Catálogo Anterior" : "Catálogo IFRS9";
            LblCalif.Text = (calificacion.Calificacion == 1) ? "SICALC" : "PCC";
            //******************
            GraficaPersonasFisicas();
            GraficaPersonasMorales();
            GraficaHistorico();
            GraficaConciliacion();
            Archivos_Rules archivos = new Archivos_Rules(Enums.Persona.Fisica);
            Archivo ultimoArchivoFisica = archivos.GetUltimoArchivo();
            archivos = new Archivos_Rules(Enums.Persona.Moral);
            Archivo ultimoArchivoMoral = archivos.GetUltimoArchivo();

            lblFechaProceso.Text = ultimoArchivoFisica != null ? ultimoArchivoFisica.Fecha.ToString() : "";
            lnkArchivoPersonaFisica.Text = ultimoArchivoFisica != null ? ultimoArchivoFisica.Nombre : "";
            lnkArchivoPersonaMorales.Text = ultimoArchivoMoral != null ? ultimoArchivoMoral.Nombre : "";
            ActividadRules.GuardarActividad(4444, this.idUs, "Acceso a la página de inicio"); // se inserta actividad 15/01/13

            try
            {
                IndicaFechas();
                //SICREB-FIN-VHCC SEP-2012
                if (Session["Facultades"] != null)
                    getFacultades();
                else
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            }
            catch (Exception ex)
            {
                Mensajes.ShowError(this.Page, this.GetType(), ex);
            }

            string NombreArchivoGL = "GL_" + lblGruposMensual.Text + "_Mensual.txt";
            NombreArchivoGL = NombreArchivoGL.Replace("/", "");

            lblMensajesGL.Text = "Último proceso: " + archivos.GetFechaUltimoProcesoGL();
            lnkArchivoGL.Text = NombreArchivoGL;
        }

        if (cbxGrupos.Items.Count == 0)
        {
            LoadGrupos();
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        RequestsRules rules = new RequestsRules();
        Banobras.Credito.SICREB.Entities.Request req = rules.GetRequestPendiente();
        lblMensajePendiente.Text = "";

        if (req != null)
        {
            if (req.Status != Request_Estado.Estado.COMPLETO && req.Status != Request_Estado.Estado.ERROR)
            {
                btnProcesarArchivos.Visible = false;
                lblMensajePendiente.Text = string.Format("{0}<br />{1}", req.Mensaje, req.Fecha_Inicio);
            }
            else
            {
                btnProcesarArchivos.Visible = true;
                if (req.Status == Banobras.Credito.SICREB.Entities.Request_Estado.Estado.ERROR)
                {
                    lblMensajePendiente.Text = req.Mensaje;
                }
            }
        }
        else
        {
            btnProcesarArchivos.Visible = true;
        }

        lnkReload.Visible = !btnProcesarArchivos.Visible;
    }


    protected void lnkArchivoPF_Click(object sender, EventArgs e)
    {
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(4, idUs, "Descarga archivo PF: " + DateTime.Today.ToString());

        Archivos_Rules archivos = new Archivos_Rules(Enums.Persona.Fisica);
        Archivo ultimoArchivo = archivos.GetUltimoArchivo();
        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms);
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        sw.Write(encoding.GetString(ultimoArchivo.BytesArchivo));
        sw.Flush();

        Response.Clear();
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", ultimoArchivo.Nombre));
        Response.AppendHeader("Content-Length", ms.Length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.BinaryWrite(ms.ToArray());
        Response.End();
    }

    protected void lnkArchivoPM_Click(object sender, EventArgs e)
    {
        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(5, idUs, "Descarga archivo PM: " + DateTime.Today.ToString());

        Archivos_Rules archivos = new Archivos_Rules(Enums.Persona.Moral);
        Archivo ultimoArchivo = archivos.GetUltimoArchivo();
        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms);
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        sw.Write(encoding.GetString(ultimoArchivo.BytesArchivo));
        sw.Flush();

        Response.Clear();
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", ultimoArchivo.Nombre));
        Response.AppendHeader("Content-Length", ms.Length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.BinaryWrite(ms.ToArray());
        Response.End();
    }

    protected void lnkArchivoGL_Click(object sender, EventArgs e)
    {

        string NombreArchivo = "GL_" + lblGruposMensual.Text + "_Mensual.txt";
        NombreArchivo = NombreArchivo.Replace("/", "");

        int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(5, idUs, "Descarga archivo GL: " + DateTime.Today.ToString());

        Archivos_Rules archivos = new Archivos_Rules(Enums.Persona.Moral);
        string ultimoArchivoGL = archivos.GetUltimoArchivoGL();

        byte[] arrayArchivo = Encoding.ASCII.GetBytes(ultimoArchivoGL);

        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms);
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        sw.WriteLine(encoding.GetString(arrayArchivo));
        sw.Flush();

        Response.Clear();
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", NombreArchivo));
        Response.AppendHeader("Content-Length", ms.Length.ToString());
        Response.ContentType = "application/octet-stream"; // "text/plain";       
        Response.BinaryWrite(ms.ToArray());
        Response.End();
    }

    protected void btnSicofin2Mensual_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdfValidaMensual.Value == "1")
            {
                //Configuracion sicofin = (Configuracion)Session["sicofin"];
                //ConsumirMensual(((sicofin.Catsicofin == 1) ? 1 : 152), lblSICOFINMensual.Text.Split('/')[1] + "-" + lblSICOFINMensual.Text.Split('/')[0]);
            }

            //Sicofin2Business Sicofin2 = new Sicofin2Business();
            //Sicofin2.GeneraSicofinMensual();
        }
        catch (Exception ex)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), " " + ex.Message);
            //throw new Exception(ex.Message);
        }
    }

    protected void btnSicofin2Semanal_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdfValidaSemanal.Value == "1")
            {
                Configuracion sicofin = (Configuracion)Session["sicofin"];
                ConsumirSemanal(((sicofin.Catsicofin == 1) ? 1 : 152), lblSICOFIN2Diaria.Text.Split('/')[2] + "-" + lblSICOFIN2Diaria.Text.Split('/')[1] + "-" + lblSICOFIN2Diaria.Text.Split('/')[0]);
            }

            Sicofin2Business Sicofin2 = new Sicofin2Business();
            Sicofin2.GeneraSicofinSemanal();
        }
        catch (Exception ex)
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), " " + ex.Message);
            //throw new Exception(ex.Message);
        }
    }

    protected void btnProcesarArchivos_Click(object sender, EventArgs e)
    {
        RequestsRules requests = new RequestsRules();
        Banobras.Credito.SICREB.Entities.Request req = requests.GetRequestPendiente();

        if (req.Status != Request_Estado.Estado.COMPLETO && req.Status != Request_Estado.Estado.ERROR)
            return;
        string grupos = "";
        foreach (ListItem item in cbxGrupos.Items)
        {
            if (item.Selected)
            {
                grupos += item.Text + ",";
            }
        }

        if (rdbMensual.Checked)
        {
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(3, idUs, "Archivo: " + DateTime.Today.ToString());
            requests.IniciaProceso(Enums.Reporte.Mensual, chbNotificaciones.Checked, grupos);
        }
        else
        {
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2, idUs, "Archivo: " + DateTime.Today.ToString());
            requests.IniciaProceso(Enums.Reporte.Semanal, chbNotificaciones.Checked, grupos);
        }

        btnProcesarArchivos.Visible = false;
        lnkReload.Visible = true;

        //<MASS 12-ago-2013>
        btnSicofin2Mensual.Enabled = !lnkReload.Visible;
        btnSicofin2Semanal.Enabled = !lnkReload.Visible;
        //</MASS>

        //Response.Redirect("Inicio.aspx");
        //TODO: deshabilitar boton
    }

    protected void btnProcesarGL_Click(object sender, EventArgs e)
    {
        try
        {
            string MensajeError = "";

            lblMensajesGL.Text = "Generando archivo de GPOs y LCCyR";
            btnProcesarGL.Visible = false;

            Archivos_Rules archivosGL = new Archivos_Rules(Enums.Persona.Moral);

            MensajeError = archivosGL.ProcesarArchivoGL();

            if (MensajeError.Trim() == string.Empty)
            {
                lblMensajesGL.Text = "Último proceso: " + archivosGL.GetFechaUltimoProcesoGL();
            }
            else
            {
                lblMensajesGL.Text = "Error: " + MensajeError;
            }

            btnProcesarGL.Visible = true;
        }
        catch (Exception ex)
        {
            lblMensajesGL.Text = "Error: " + ex.ToString();
        }

    }

    protected void txtFechaInicial_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        ValidarFechas();
    }

    protected void txtFechaFinal_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        ValidarFechas();
    }

    //protected void Notificaciones_Checked(Object sender, EventArgs e)
    //{
    //    if (chbNotificaciones.Checked)
    //    {
    //        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
    //        ActividadRules.GuardarActividad(200, idUs, "Activación de Notificaciones");
    //    }
    //    else
    //    {
    //        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
    //        ActividadRules.GuardarActividad(200, idUs, "Desactivación de Notificaciones");
    //    }
    //}

    protected void chbNotificaciones_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox check = sender as CheckBox;
        if (check.Checked)
        {
            ActividadRules.GuardarActividad(200, this.idUs, "Seleccionado check Notificaciones");
        }
        else
        {
            ActividadRules.GuardarActividad(200, this.idUs, "Deseleccionado check Notificaciones");
        }
    }

    protected void cbxGrupos_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Recorre elementos de la lista
        foreach (ListItem li in cbxGrupos.Items)
        {
            if (li.Selected)
            {
                ActividadRules.GuardarActividad(300, this.idUs, "Seleccionado check Grupo " + li.Text);
            }
            else
            {
                ActividadRules.GuardarActividad(300, this.idUs, "Deseleccionado check Grupo " + li.Text);
            }
        }
    }

    protected void rdbSemanal_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton rbtn = (RadioButton)sender;
        if (rbtn.Checked)
        {
            ActividadRules.GuardarActividad(400, this.idUs, "Seleccionado periodicidad semanal");
        }
        else
        {
            ActividadRules.GuardarActividad(400, this.idUs, "Deseleccionado periodicidad semanal");
        }
    }

    protected void rdbMensual_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton rbtn = (RadioButton)sender;
        if (rbtn.Checked)
        {
            ActividadRules.GuardarActividad(400, this.idUs, "Seleccionado periodicidad mensual");
        }
        else
        {
            ActividadRules.GuardarActividad(400, this.idUs, "Deseleccionado periodicidad mensual");
        }
    }


    private void IndicaFechas()
    {
        FechasVistasRules fvr = new FechasVistasRules();
        List<DateTime> Fechas = fvr.FechaVista();

        if (Fechas[0].ToString("dd/MM/yyyy").Equals("01/01/0001"))
            lblSICOFINDiaria.Text = "Sin procesar";
        else
            lblSICOFINDiaria.Text = Fechas[0].ToString("dd/MM/yyyy");
        lblSICOFINMensual.Text = Fechas[1].ToString("MM/yyyy");

        lblCLICDiaria.Text = Fechas[2].ToString("dd/MM/yyyy");
        lblSICDiario.Text = Fechas[2].ToString("dd/MM/yyyy");
        lblClicMensual.Text = Fechas[3].ToString("dd/MM/yyyy");
        lblSICMensual.Text = Fechas[3].ToString("dd/MM/yyyy");
        lblSICOFIN2Diaria.Text = Fechas[4].ToString("dd/MM/yyyy");
        lblSICOFIN2Mensual.Text = Fechas[5].ToString("MM/yyyy");
        lblGruposMensual.Text = Fechas[6].ToString("dd/MM/yyyy");
        lblLineasMensual.Text = Fechas[7].ToString("dd/MM/yyyy");
        Session["lblSICOFIN2Diaria"] = lblCLICDiaria.Text;//lblSICOFIN2Diaria.Text;
        Session["lblSICOFINMensual"] = lblSICOFIN2Mensual.Text;
    }

    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();
        rdbSemanal.Visible = false;
        rdbMensual.Visible = false;
        btnProcesarArchivos.Visible = false;
        chbNotificaciones.Visible = false;
        lnkArchivoPersonaMorales.Visible = false;
        lnkArchivoPersonaFisica.Visible = false;
        bool valido = false;

        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("GASEM") + "|"))
        {
            rdbSemanal.Visible = true;
            btnProcesarArchivos.Visible = true;
            chbNotificaciones.Visible = true;
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("GAMEN") + "|"))
        {
            rdbMensual.Visible = true;
            btnProcesarArchivos.Visible = true;
            chbNotificaciones.Visible = true;
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("DAPF") + "|"))
        {
            lnkArchivoPersonaFisica.Visible = true;
            valido = true;
        }
        if (Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("DAPM") + "|"))
        {
            lnkArchivoPersonaMorales.Visible = true;
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

    private void LoadGrupos()
    {
        // El tipo de persona no importa.
        Cuentas_Rules datos = new Cuentas_Rules(Enums.Persona.Moral);

        List<int> grupos = datos.GetGrupos();
        grupos.Sort();

        cbxGrupos.DataSource = grupos;
        cbxGrupos.DataBind();

        foreach (ListItem item in cbxGrupos.Items)
        {
            item.Selected = true;
        }
    }

    private void ValidarFechas()
    {

        if (this.txtFechaInicial.SelectedDate != null)
        {
            if (this.txtFechaFinal.SelectedDate != null)
            {

                if (this.txtFechaInicial.SelectedDate.Value.CompareTo(this.txtFechaFinal.SelectedDate.Value) <= 0)
                {
                    RadChartHistorico.RemoveAllSeries();
                    RadChartHistorico.Clear();

                    GraficaHistorico(this.txtFechaInicial.SelectedDate.Value, this.txtFechaFinal.SelectedDate.Value);
                }
                else
                {
                    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "La Fecha Inicial no puede ser mayor a la Fecha Final");
                }
            }
        }
    }

    private void OcultarCharHistorico(bool Visible)
    {
        // SICREB-INICIO-VHCC DIC-2012
        // Se Creo esta nueva funcion debido que al pintar la grafica de los Historicos esta nos daba un error,
        // y pues se le agrego un Try{} Catch(){} para poder controlarlo y se nesesito ocultar la grafica.
        RadChartHistorico.Visible = Visible;
        lblTituloRango.Visible = Visible;
        lblFechaInicial.Visible = Visible;
        lblFechaFinal.Visible = Visible;
        txtFechaFinal.Visible = Visible;
        txtFechaInicial.Visible = Visible;
    }


    void GraficaPersonasFisicas()
    {
        ChartSeries cs = new ChartSeries();
        cs.Name = "Personas Fisicas";
        cs.Type = ChartSeriesType.Pie;
        GraficasRules DatosGrafica = new GraficasRules();
        Archivo Porcentajes = DatosGrafica.EstadisticasArchivo("F", Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica.EstadisticasArchivo);

        cs.AddItem(Porcentajes.Reg_Correctos, Porcentajes.Reg_Correctos.ToString());
        cs.AddItem(Porcentajes.Reg_Errores, Porcentajes.Reg_Errores.ToString());
        cs.AddItem(Porcentajes.Reg_Advertencias, Porcentajes.Reg_Advertencias.ToString());
        cs[0].Name = "Correctos";
        cs[0].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        cs[0].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#39B54A");
        cs[0].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#1D5B25");
        cs[1].Name = "Errores";
        cs[1].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        cs[1].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#BE1E2D");
        cs[1].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#5F0F17");
        cs[2].Name = "Advertencias";
        cs[2].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        cs[2].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#FFDC42");
        cs[2].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#8A611C");
        cs.Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.ItemLabels;

        RadChartPersonasFisicas.Series.Add(cs);
        RadChartPersonasFisicas.Series[0].Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.ItemLabels;
        RadChartPersonasFisicas.PlotArea.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        RadChartPersonasFisicas.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
        RadChartPersonasFisicas.PlotArea.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        RadChartPersonasFisicas.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        RadChartPersonasFisicas.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
        RadChartPersonasFisicas.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        RadChartPersonasFisicas.ChartTitle.Remove();
        RadChartPersonasFisicas.PlotArea.Appearance.Dimensions.Margins.Left = new Telerik.Charting.Styles.Unit(1);
        RadChartPersonasFisicas.PlotArea.Appearance.Dimensions.Margins.Top = new Telerik.Charting.Styles.Unit(1);
        RadChartPersonasFisicas.PlotArea.Appearance.Dimensions.Margins.Bottom = new Telerik.Charting.Styles.Unit(1);
        RadChartPersonasFisicas.PlotArea.Appearance.Border.Visible = false;
        RadChartPersonasFisicas.Legend.Appearance.Border.Visible = false;
        RadChartPersonasFisicas.Legend.Appearance.Position.Auto = false;
        RadChartPersonasFisicas.Legend.Appearance.Position.X = 245;
        RadChartPersonasFisicas.Legend.Appearance.Position.Y = 225;
    }

    void GraficaPersonasMorales()
    {
        ChartSeries cs = new ChartSeries();
        cs.Name = "Personas Morales";
        cs.Type = ChartSeriesType.Pie;
        GraficasRules DatosGrafica = new GraficasRules();
        Archivo Porcentajes = DatosGrafica.EstadisticasArchivo("M", Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica.EstadisticasArchivo);

        cs.AddItem(Porcentajes.Reg_Correctos, Porcentajes.Reg_Correctos.ToString());
        cs.AddItem(Porcentajes.Reg_Errores, Porcentajes.Reg_Errores.ToString());
        cs.AddItem(Porcentajes.Reg_Advertencias, Porcentajes.Reg_Advertencias.ToString());
        cs[0].Name = "Correctos";
        cs[0].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        cs[0].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#39B54A");
        cs[0].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#1D5B25");
        cs[1].Name = "Errores";
        cs[1].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        cs[1].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#BE1E2D");
        cs[1].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#5F0F17");
        cs.Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.ItemLabels;
        cs[2].Name = "Advertencias";
        cs[2].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        cs[2].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#FFDC42");
        cs[2].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#8A611C");

        RadChartPersonasMorales.Series.Add(cs);
        RadChartPersonasMorales.Series[0].Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.ItemLabels;
        RadChartPersonasMorales.PlotArea.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        RadChartPersonasMorales.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
        RadChartPersonasMorales.PlotArea.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        RadChartPersonasMorales.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        RadChartPersonasMorales.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
        RadChartPersonasMorales.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        RadChartPersonasMorales.ChartTitle.Remove();
        RadChartPersonasMorales.PlotArea.Appearance.Dimensions.Margins.Left = new Telerik.Charting.Styles.Unit(1);
        RadChartPersonasMorales.PlotArea.Appearance.Dimensions.Margins.Top = new Telerik.Charting.Styles.Unit(1);
        RadChartPersonasMorales.PlotArea.Appearance.Dimensions.Margins.Bottom = new Telerik.Charting.Styles.Unit(1);
        RadChartPersonasMorales.PlotArea.Appearance.Border.Visible = false;
        RadChartPersonasMorales.Legend.Appearance.Border.Visible = false;
        RadChartPersonasMorales.Legend.Appearance.Position.Auto = false;
        RadChartPersonasMorales.Legend.Appearance.Position.X = 245;
        RadChartPersonasMorales.Legend.Appearance.Position.Y = 225;
    }

    void GraficaHistorico()
    {

        GraficasRules DatosGrafica = new GraficasRules();
        List<Archivo> ArchivoFisicasErrores;
        List<Archivo> ArchivoFisicasAdvertencias;
        List<Archivo> ArchivoMoralesErrores;
        List<Archivo> ArchivoMoralesAdvertencias;

        //SICREB-INICIO-VHCC DIC-2012
        // Apartir de aqui, todo lo que se encuentra despues de esta linea se integro dentro de un Try-Catch, 
        // esto se realizo para poder cachar los errores, que se presenten en un futuro.
        try
        {
            ArchivoFisicasErrores = DatosGrafica.HistoricoArchivo("F", Enums.StoreGrafica.ErrorHistorico, out ArchivoFisicasAdvertencias);
            ArchivoMoralesErrores = DatosGrafica.HistoricoArchivo("M", Enums.StoreGrafica.ErrorHistorico, out ArchivoMoralesAdvertencias);
            ChartSeries cs2 = new ChartSeries("Advertencias PF");
            cs2.Type = ChartSeriesType.Line;
            foreach (Archivo ar in ArchivoFisicasAdvertencias)
            {
                ChartSeriesItem cs = new ChartSeriesItem(Convert.ToDouble(ar.Reg_Advertencias));
                cs.XValue = ar.Fecha.Date.ToOADate();
                cs.Label.Appearance.Visible = false;
                cs.ActiveRegion.Tooltip = string.Format("{0}", ar.Reg_Advertencias.ToString());
                cs2.AddItem(cs);
            }
            cs2.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs2.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#FFDC42");
            cs2.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#8A611C");
            ChartSeries cs3 = new ChartSeries("Errores PF");
            cs3.Type = ChartSeriesType.Line;
            foreach (Archivo ar in ArchivoFisicasErrores)
            {
                ChartSeriesItem cs = new ChartSeriesItem(Convert.ToDouble(ar.Reg_Errores));
                cs.XValue = ar.Fecha.Date.ToOADate();
                cs.Label.Appearance.Visible = false;
                cs.ActiveRegion.Tooltip = string.Format("{0}", ar.Reg_Errores.ToString());
                cs3.AddItem(cs);
            }
            cs3.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs3.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#BE1E2D");
            cs3.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#5F0F17");

            ChartSeries cs4 = new ChartSeries("Advertencias PM");
            cs4.Type = ChartSeriesType.Line;
            foreach (Archivo ar in ArchivoMoralesAdvertencias)
            {
                ChartSeriesItem cs = new ChartSeriesItem(Convert.ToDouble(ar.Reg_Advertencias));
                cs.XValue = ar.Fecha.Date.ToOADate();
                cs.Label.Appearance.Visible = false;
                cs.ActiveRegion.Tooltip = string.Format("{0}", ar.Reg_Advertencias.ToString());
                cs4.AddItem(cs);
            }
            cs4.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs4.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#67AAD6");
            cs4.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#416F8D");

            ChartSeries cs5 = new ChartSeries("Errores PM");
            cs5.Type = ChartSeriesType.Line;
            foreach (Archivo ar in ArchivoMoralesErrores)
            {
                ChartSeriesItem cs = new ChartSeriesItem(Convert.ToDouble(ar.Reg_Errores));
                cs.XValue = ar.Fecha.Date.ToOADate();
                cs.Label.Appearance.Visible = false;
                cs.ActiveRegion.Tooltip = string.Format("{0}", ar.Reg_Errores.ToString());
                cs5.AddItem(cs);
            }
            cs5.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs5.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#39B54A");
            cs5.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#1D5B25");

            //SICREB-INICIO-VHCC SEP-2012
            // Visualizar los puntos referentes a los datos.
            cs2.Appearance.PointMark.Dimensions.Width = 1;
            cs2.Appearance.PointMark.Dimensions.Height = 1;
            cs2.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Black;
            cs2.Appearance.PointMark.Visible = true;

            cs3.Appearance.PointMark.Dimensions.Width = 1;
            cs3.Appearance.PointMark.Dimensions.Height = 1;
            cs3.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Black;
            cs3.Appearance.PointMark.Visible = true;

            cs4.Appearance.PointMark.Dimensions.Width = 1;
            cs4.Appearance.PointMark.Dimensions.Height = 1;
            cs4.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Black;
            cs4.Appearance.PointMark.Visible = true;

            cs5.Appearance.PointMark.Dimensions.Width = 1;
            cs5.Appearance.PointMark.Dimensions.Height = 1;
            cs5.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Black;
            cs5.Appearance.PointMark.Visible = true;
            //SICREB-INICIO-VHCC SEP-2012

            if (cs2.Items.Count > 0)
            {
                RadChartHistorico.Series.Add(cs2);
                RadChartHistorico.Series.Add(cs3);
                RadChartHistorico.Series.Add(cs4);
                RadChartHistorico.Series.Add(cs5);
                RadChartHistorico.ChartTitle.Remove();
                RadChartHistorico.PlotArea.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
                RadChartHistorico.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
                RadChartHistorico.PlotArea.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                RadChartHistorico.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
                RadChartHistorico.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
                RadChartHistorico.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                RadChartHistorico.PlotArea.YAxis.IsZeroBased = false;
                RadChartHistorico.PlotArea.XAxis.LayoutMode = Telerik.Charting.Styles.ChartAxisLayoutMode.Inside;
                RadChartHistorico.PlotArea.XAxis.AutoScale = false;
                RadChartHistorico.PlotArea.XAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.ShortDate;
                RadChartHistorico.PlotArea.XAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.ShortDate;
                RadChartHistorico.PlotArea.XAxis.Appearance.MajorGridLines.Visible = false;
                RadChartHistorico.PlotArea.XAxis.AddRange(cs2[cs2.Items.Count - 1].XValue, cs2[0].XValue + 1, 1);
                RadChartHistorico.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 90;
                RadChartHistorico.PlotArea.XAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Top;
                RadChartHistorico.PlotArea.Appearance.Dimensions.Margins.Top = new Telerik.Charting.Styles.Unit(7);
                RadChartHistorico.PlotArea.Appearance.Dimensions.Margins.Bottom = new Telerik.Charting.Styles.Unit(100);
                RadChartHistorico.PlotArea.Appearance.Dimensions.Margins.Left = new Telerik.Charting.Styles.Unit(50);
                RadChartHistorico.PlotArea.Appearance.Dimensions.Margins.Right = new Telerik.Charting.Styles.Unit(200);//100
                RadChartHistorico.Legend.Appearance.Border.Visible = false;
                RadChartHistorico.Legend.Appearance.Position.Auto = false;
                RadChartHistorico.Legend.Appearance.Position.X = 270;
                RadChartHistorico.Legend.Appearance.Position.Y = 202;
                RadChartHistorico.Legend.Appearance.Dimensions.AutoSize = false;
                RadChartHistorico.Legend.Appearance.Dimensions.Width = 120;
                RadChartHistorico.Legend.Appearance.Dimensions.Height = 95;

            }
        }

        // Al cachar los Errores se oculta todo lo que tenga que ver con la grafica de los Historicos
        // y se arroja un mensaje describiendo el motivo por el cual no se puede pintar la grafica.
        catch (Exception ex)
        {
            OcultarCharHistorico(false);
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Error al tratar de pintar la Grafica de Historicos.\n." + ex.Message, "~/Inicio.aspx");
        }
        //SICREB-FIN-VHCC DIC-2012

    }

    void GraficaConciliacion()
    {
        GraficasRules gr = new GraficasRules();
        ChartSeries cs = new ChartSeries();
        Archivos_Rules archivos = new Archivos_Rules(Enums.Persona.Fisica);
        Archivo ultimoArchivoFisica = archivos.GetUltimoArchivo();
        archivos = new Archivos_Rules(Enums.Persona.Moral);
        Archivo ultimoArchivoMoral = archivos.GetUltimoArchivo();
        List<int> DatosArchivoConciliacion = gr.ConciliacionArchivo(ultimoArchivoMoral != null ? ultimoArchivoMoral.Id : 0, ultimoArchivoFisica != null ? ultimoArchivoFisica.Id : 0);
        cs.Name = "Conciliación";
        cs.Type = ChartSeriesType.Pie;

        if (DatosArchivoConciliacion.Count >= 5)
        {
            cs.AddItem(DatosArchivoConciliacion[0]);
            cs.AddItem(DatosArchivoConciliacion[1]);
            cs.AddItem(DatosArchivoConciliacion[2]);
            cs.AddItem(DatosArchivoConciliacion[3]);
            cs.AddItem(DatosArchivoConciliacion[4]);

            cs[0].Name = "Bien";
            cs[0].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs[0].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#39B54A");
            cs[0].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#1D5B25");
            cs[1].Name = "Por Redondeo";
            cs[1].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs[1].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#FFDC42");
            cs[1].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#8A611C");
            cs[2].Name = "Errores";
            cs[2].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs[2].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#BE1E2D");
            cs[2].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#5F0F17");
            cs[3].Name = "Exceptuados";
            cs[3].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs[3].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#67AAD6");
            cs[3].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#416F8D");
            cs[4].Name = "Por Investigar";
            cs[4].Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs[4].Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#A53C17");
            cs[4].Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#F15A29");

            cs.Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.ItemLabels;
            RadChartConciliacion.Series.Add(cs);
            RadChartConciliacion.Series[0].Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.ItemLabels;
        }

        RadChartConciliacion.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        RadChartConciliacion.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
        RadChartConciliacion.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        RadChartConciliacion.PlotArea.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
        RadChartConciliacion.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
        RadChartConciliacion.PlotArea.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        RadChartConciliacion.ChartTitle.Remove();
        RadChartConciliacion.PlotArea.Appearance.Dimensions.Margins.Left = new Telerik.Charting.Styles.Unit(5);
        RadChartConciliacion.PlotArea.Appearance.Dimensions.Margins.Top = new Telerik.Charting.Styles.Unit(5);
        RadChartConciliacion.PlotArea.Appearance.Dimensions.Margins.Bottom = new Telerik.Charting.Styles.Unit(5);
        RadChartConciliacion.PlotArea.Appearance.Border.Visible = false;
        RadChartConciliacion.Legend.Appearance.Border.Visible = false;
        RadChartConciliacion.Legend.Appearance.Position.Auto = false;
        RadChartConciliacion.Legend.Appearance.Position.X = 300;
        RadChartConciliacion.Legend.Appearance.Position.Y = 180;
    }

    void GraficaHistorico(DateTime dFechaInicio, DateTime dFechaFin)
    {

        GraficasRules DatosGrafica = new GraficasRules();
        List<Archivo> ArchivoFisicasErrores = new List<Archivo>();
        List<Archivo> ArchivoFisicasAdvertencias = new List<Archivo>();
        List<Archivo> ArchivoMoralesErrores = new List<Archivo>();
        List<Archivo> ArchivoMoralesAdvertencias = new List<Archivo>();

        //SICREB-INICIO-VHCC DIC-2012
        // Apartir de aqui, todo lo que se encuentra denspues de esta linea se integro dentro de un Try-Catch, 
        // esto se realizo para poder cachar los errores, que se presenten en un futuro.


        try
        {
            RadChartHistorico.Clear();
            //ArchivoFisicasErrores = DatosGrafica.HistoricoArchivo("F", Enums.StoreGrafica.ErrorHistorico, out ArchivoFisicasAdvertencias);
            //ArchivoMoralesErrores = DatosGrafica.HistoricoArchivo("M", Enums.StoreGrafica.ErrorHistorico, out ArchivoMoralesAdvertencias);
            ArchivoFisicasErrores = DatosGrafica.HistoricoArchivo("F", Enums.StoreGrafica.ErrorHistorico, dFechaInicio, dFechaFin, out ArchivoFisicasAdvertencias);
            ArchivoMoralesErrores = DatosGrafica.HistoricoArchivo("M", Enums.StoreGrafica.ErrorHistorico, dFechaInicio, dFechaFin, out ArchivoMoralesAdvertencias);


            ChartSeries cs2 = new ChartSeries("Advertencias PF");
            cs2.Type = ChartSeriesType.Line;
            foreach (Archivo ar in ArchivoFisicasAdvertencias)
            {
                ChartSeriesItem cs = new ChartSeriesItem(Convert.ToDouble(ar.Reg_Advertencias));
                cs.XValue = ar.Fecha.Date.ToOADate();
                cs.Label.Appearance.Visible = false;
                cs.ActiveRegion.Tooltip = string.Format("{0}", ar.Reg_Advertencias.ToString());
                cs2.AddItem(cs);
            }
            cs2.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs2.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#FFDC42");
            cs2.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#8A611C");
            ChartSeries cs3 = new ChartSeries("Errores PF");
            cs3.Type = ChartSeriesType.Line;
            foreach (Archivo ar in ArchivoFisicasErrores)
            {
                ChartSeriesItem cs = new ChartSeriesItem(Convert.ToDouble(ar.Reg_Errores));
                cs.XValue = ar.Fecha.Date.ToOADate();
                cs.Label.Appearance.Visible = false;
                cs.ActiveRegion.Tooltip = string.Format("{0}", ar.Reg_Errores.ToString());
                cs3.AddItem(cs);
            }
            cs3.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs3.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#BE1E2D");
            cs3.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#5F0F17");

            ChartSeries cs4 = new ChartSeries("Advertencias PM");
            cs4.Type = ChartSeriesType.Line;
            foreach (Archivo ar in ArchivoMoralesAdvertencias)
            {
                ChartSeriesItem cs = new ChartSeriesItem(Convert.ToDouble(ar.Reg_Advertencias));
                cs.XValue = ar.Fecha.Date.ToOADate();
                cs.Label.Appearance.Visible = false;
                cs.ActiveRegion.Tooltip = string.Format("{0}", ar.Reg_Advertencias.ToString());
                cs4.AddItem(cs);
            }
            cs4.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs4.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#67AAD6");
            cs4.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#416F8D");

            ChartSeries cs5 = new ChartSeries("Errores PM");
            cs5.Type = ChartSeriesType.Line;
            foreach (Archivo ar in ArchivoMoralesErrores)
            {
                ChartSeriesItem cs = new ChartSeriesItem(Convert.ToDouble(ar.Reg_Errores));
                cs.XValue = ar.Fecha.Date.ToOADate();
                cs.Label.Appearance.Visible = false;
                cs.ActiveRegion.Tooltip = string.Format("{0}", ar.Reg_Errores.ToString());
                cs5.AddItem(cs);
            }
            cs5.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
            cs5.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#39B54A");
            cs5.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#1D5B25");

            //SICREB-INICIO-VHCC SEP-2012
            // Visualizar los puntos referentes a los datos.
            cs2.Appearance.PointMark.Dimensions.Width = 1;
            cs2.Appearance.PointMark.Dimensions.Height = 1;
            cs2.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Black;
            cs2.Appearance.PointMark.Visible = true;

            cs3.Appearance.PointMark.Dimensions.Width = 1;
            cs3.Appearance.PointMark.Dimensions.Height = 1;
            cs3.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Black;
            cs3.Appearance.PointMark.Visible = true;

            cs4.Appearance.PointMark.Dimensions.Width = 1;
            cs4.Appearance.PointMark.Dimensions.Height = 1;
            cs4.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Black;
            cs4.Appearance.PointMark.Visible = true;

            cs5.Appearance.PointMark.Dimensions.Width = 1;
            cs5.Appearance.PointMark.Dimensions.Height = 1;
            cs5.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Black;
            cs5.Appearance.PointMark.Visible = true;
            //SICREB-INICIO-VHCC SEP-2012

            if (cs2.Items.Count > 0)
            {
                RadChartHistorico.Series.Add(cs2);
                RadChartHistorico.Series.Add(cs3);
                RadChartHistorico.Series.Add(cs4);
                RadChartHistorico.Series.Add(cs5);
                RadChartHistorico.ChartTitle.Remove();
                RadChartHistorico.PlotArea.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
                RadChartHistorico.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
                RadChartHistorico.PlotArea.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                RadChartHistorico.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient;
                RadChartHistorico.Appearance.FillStyle.MainColor = System.Drawing.ColorTranslator.FromHtml("#E0E0EB");
                RadChartHistorico.Appearance.FillStyle.SecondColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                RadChartHistorico.PlotArea.YAxis.IsZeroBased = false;
                RadChartHistorico.PlotArea.XAxis.LayoutMode = Telerik.Charting.Styles.ChartAxisLayoutMode.Inside;
                RadChartHistorico.PlotArea.XAxis.AutoScale = false;
                RadChartHistorico.PlotArea.XAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.ShortDate;
                RadChartHistorico.PlotArea.XAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.ShortDate;
                RadChartHistorico.PlotArea.XAxis.Appearance.MajorGridLines.Visible = false;
                RadChartHistorico.PlotArea.XAxis.AddRange(cs2[cs2.Items.Count - 1].XValue, cs2[0].XValue + 1, 1);
                //RadChartHistorico.PlotArea.XAxis.AddRange(dFechaInicio.ToOADate(), dFechaFin.ToOADate() + 1, 10);
                RadChartHistorico.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 90;
                RadChartHistorico.PlotArea.XAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Top;
                RadChartHistorico.PlotArea.Appearance.Dimensions.Margins.Top = new Telerik.Charting.Styles.Unit(7);
                RadChartHistorico.PlotArea.Appearance.Dimensions.Margins.Bottom = new Telerik.Charting.Styles.Unit(100);
                RadChartHistorico.PlotArea.Appearance.Dimensions.Margins.Left = new Telerik.Charting.Styles.Unit(50);
                RadChartHistorico.PlotArea.Appearance.Dimensions.Margins.Right = new Telerik.Charting.Styles.Unit(200);//100
                RadChartHistorico.Legend.Appearance.Border.Visible = false;
                RadChartHistorico.Legend.Appearance.Position.Auto = false;
                RadChartHistorico.Legend.Appearance.Position.X = 270;//260;//230
                RadChartHistorico.Legend.Appearance.Position.Y = 202;
                RadChartHistorico.Legend.Appearance.Dimensions.AutoSize = false;
                RadChartHistorico.Legend.Appearance.Dimensions.Width = 120;//115
                RadChartHistorico.Legend.Appearance.Dimensions.Height = 95;

            }
        }
        // Al cachar los Errores se oculta todo lo que tenga que ver con la grafica de los Historicos
        // y se arroja un mensaje describiendo el motivo por el cual no se puede pintar la grafica.
        catch (Exception ex)
        {
            OcultarCharHistorico(false);
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Error al tratar de pintar la Grafica de Historicos.\n." + ex.Message, "~/Inicio.aspx");
        }
        //SICREB-FIN-VHCC DIC-2012

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="empresa"></param>
    /// <param name="fecha"></param>
    private void ConsumirMensual(int empresa, string fecha)
    {
        var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["UriMensual"].ToString()/*UriMensual*/);
        StringBuilder json = new StringBuilder();
        Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "empresa", empresa },
                { "fecha", fecha }
            };

        string body = JsonConvert.SerializeObject(dic);
        request.Method = "POST";
        request.ContentType = "application/json";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(body);
            streamWriter.Flush();
            streamWriter.Close();
        }
        try
        {
            System.Diagnostics.Debug.WriteLine("Hora inicio WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            request.Timeout = 300000;
            using (WebResponse response = request.GetResponse())
            {
                System.Diagnostics.Debug.WriteLine("Hora fin WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                System.Diagnostics.Debug.WriteLine("Hora inicio lectura WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null)
                    {
                        /*implementar procedimiento cuando no haya datos*/
                        return;
                    }
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine("Hora fin lectura WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        clsSICOFIN clsGuardaSICOFIN = new clsSICOFIN(ConfigurationManager.ConnectionStrings["SICREB"].ToString());
                        System.Diagnostics.Debug.WriteLine("Hora inicio conversion WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        clsSICOFIN_Mensual.Root SICOFINMensual = Newtonsoft.Json.JsonConvert.DeserializeObject<clsSICOFIN_Mensual.Root>(responseBody);
                        System.Diagnostics.Debug.WriteLine("Hora fin conversion WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        if (SICOFINMensual.data.Count > 0)
                        {
                            clsGuardaSICOFIN.LimpiaMensual(empresa);
                            System.Diagnostics.Debug.WriteLine("hora inicio copia tablas: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            clsGuardaSICOFIN.GuardaMensual(SICOFINMensual);
                            System.Diagnostics.Debug.WriteLine("hora fin copia tablas: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            System.Diagnostics.Debug.WriteLine("Registros procesados:  " + SICOFINMensual.data.Count + ".");
                        }
                        else
                        {
                            throw new Exception("El servicio web SICOFIN no tiene elementos para importar.");
                        }
                    }
                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Response == null)
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Error en la comunicación " + ex.Message);
            }
            else
            {
                using (StreamReader streamReader2 = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string bodyerror = streamReader2.ReadToEnd();
                    clsSICOFIN_Error.Root ErrorDatos = Newtonsoft.Json.JsonConvert.DeserializeObject<clsSICOFIN_Error.Root>(bodyerror);
                    //Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Error en la comunicación " + ErrorDatos.data.errorSource);
                    throw new Exception(ex.Message + " - " + ErrorDatos.data.errorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="empresa"></param>
    /// <param name="fecha"></param>
    private void ConsumirSemanal(int empresa, string fecha)
    {

        var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["UriSemanal"].ToString());
        StringBuilder json = new StringBuilder();
        Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "empresa", empresa },
                { "fecha", fecha }
            };

        string body = JsonConvert.SerializeObject(dic);
        request.Method = "POST";
        request.ContentType = "application/json";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(body);
            streamWriter.Flush();
            streamWriter.Close();
        }
        try
        {
            request.Timeout = 300000;
            System.Diagnostics.Debug.WriteLine("Hora inicio WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            using (WebResponse response = request.GetResponse())
            {
                System.Diagnostics.Debug.WriteLine("Hora fin WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                System.Diagnostics.Debug.WriteLine("Hora inicio lectura WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null)
                    {
                        /*implementar procedimiento cuando no haya datos*/
                        return;
                    }
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine("Hora fin lectura WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        clsSICOFIN clsGuardaSICOFIN = new clsSICOFIN(ConfigurationManager.ConnectionStrings["SICREB"].ToString());
                        System.Diagnostics.Debug.WriteLine("Hora inicio conversion WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        clsSICOFIN_Semanal.Root SICOFINSemanal = Newtonsoft.Json.JsonConvert.DeserializeObject<clsSICOFIN_Semanal.Root>(responseBody);
                        System.Diagnostics.Debug.WriteLine("Hora fin conversion WS: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        if (SICOFINSemanal.data.Count > 0)
                        {
                            clsGuardaSICOFIN.LimpiaSemanal(empresa);
                            System.Diagnostics.Debug.WriteLine("Hora inicio copia tablas: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            clsGuardaSICOFIN.GuardaSemanal(SICOFINSemanal);
                            System.Diagnostics.Debug.WriteLine("Hora fin copia tablas: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            System.Diagnostics.Debug.WriteLine("Registros procesados: " + SICOFINSemanal.data.Count + ".");
                        }
                    }
                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Response == null)
            {
                Mensajes.ShowAdvertencia(this.Page, this.GetType(), ex.Message);
            }
            else
            {
                using (StreamReader streamReader2 = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string bodyerror = streamReader2.ReadToEnd();
                    clsSICOFIN_Error.Root ErrorDatos = Newtonsoft.Json.JsonConvert.DeserializeObject<clsSICOFIN_Error.Root>(bodyerror);
                    //Mensajes.ShowAdvertencia(this.Page, this.GetType(), ErrorDatos.data.errorSource);
                    throw new Exception(ex.Message + " - " + ErrorDatos.data.errorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            //Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Error en la comunicación  con el webservice" + ex.Message);
            throw new Exception(ex.Message);
        }
    }



    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ConsumirWSM(int opcion)
    {
        try
        {
            Inicio ini = new Inicio();
            return ini.ProcesarMensual(opcion);
        }
        catch (Exception exep)
        {
            return exep.Message;
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ConsumirWSS(int opcion)
    {
        try
        {
            Inicio ini = new Inicio();
            return ini.ProcesarSemanal(opcion);
        }
        catch (Exception exep)
        {
            return exep.Message;
        }
    }

    string ProcesarMensual(Int32 Validar)
    {
        string mensaje = "Proceso terminado.";
        try
        {
            if (Validar == 1)
            {
                Configuracion sicofin = (Configuracion)Session["sicofin"];
                ConsumirMensual(((sicofin.Catsicofin == 1) ? 1 : 152), Session["lblSICOFINMensual"].ToString().Split('/')[1] + "-" + Session["lblSICOFINMensual"].ToString().Split('/')[0]);
                mensaje = "Actualización de saldos terminada.";
            }
            Sicofin2Business Sicofin2 = new Sicofin2Business();
            Sicofin2.GeneraSicofinMensual();
            return mensaje;
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
            //Mensajes.ShowAdvertencia(this.Page, this.GetType(), " " + ex.Message);
            //throw new Exception(ex.Message);
        }
    }

    string ProcesarSemanal(Int32 Validar)
    {
        string mensaje = "Proceso terminado.";
        try
        {
            if (Validar == 1)
            {
                Configuracion sicofin = (Configuracion)Session["sicofin"];
                ConsumirSemanal(((sicofin.Catsicofin == 1) ? 1 : 152), Session["lblSICOFIN2Diaria"].ToString().Split('/')[2] + "-" + Session["lblSICOFIN2Diaria"].ToString().Split('/')[1] + "-" + Session["lblSICOFIN2Diaria"].ToString().Split('/')[0]);
                mensaje = "Actualización de saldos terminada.";
            }
            Sicofin2Business Sicofin2 = new Sicofin2Business();
            Sicofin2.GeneraSicofinSemanal();
            return mensaje;
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
            //Mensajes.ShowAdvertencia(this.Page, this.GetType(), " " + ex.Message);
            //throw new Exception(ex.Message);
        }
    }
}
