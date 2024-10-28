using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Business.Seguridad;
using Banobras.Credito.SICREB.Common.ExceptionHelpers;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public class BitacoraViewModel : Bitacora
{

    /// <summary>
    /// Nombre de Evento
    /// </summary>
    public string EventName { get; set; }

}

public partial class Bitacora_Bitacora : System.Web.UI.Page
{

    int usuarioId;
    private List<BitacoraEvento> eventosLista = new List<BitacoraEvento>();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                usuarioId = Parser.ToNumber(Session["idUsuario"].ToString());
                this.LoadInitalData();

                //Guardando bitácora
                Bitacora bitacora = this.GetBitacoraTemplate();
                bitacora.Comments = "Ingreso al catálogo de bitácora.";
                bitacora.EventId = (int)BitacoraEventoTipoEnum.UsuarioSesionIniciada;
                bitacora.LogType = BitacoraTipoEstatusEnum.Successful;
                BitacoraRules.AgregarBitacora(bitacora);
            }            
        }
        catch (Exception ex)
        {
            //Mensajes.ShowError(this.Page, this.GetType(), ex);
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }
    }



    #region Botones ...


    /// <summary>
    /// Obteniendo información filtrada
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime? fechaInicial = this.rdpFechaInicial.SelectedDate != null ? this.rdpFechaInicial.SelectedDate : DateTime.MinValue;
            DateTime? fechaFinal = this.rdpFechaFinal.SelectedDate != null ? this.rdpFechaFinal.SelectedDate : DateTime.MinValue;
            int? evento = this.ddlEvento.SelectedItem.Value != "-1" ? int.Parse(this.ddlEvento.SelectedValue) : 0;
            var usuario = this.ddlUsuario.SelectedItem.Value != "-1" ? this.ddlUsuario.SelectedValue : null;

            if (fechaFinal < fechaInicial)
            {
                Mensajes.ShowMessage(this.Page, this.GetType(), "La fecha final no debe ser inferior a la fecha inicial.");
                return;
            }

            var result = this.GetBitacoraData(fechaInicial, fechaFinal.Value.AddDays(1), evento, usuario);

            this.rgBitacora.CurrentPageIndex = 0;
            this.rgBitacora.DataSource = result;
            this.rgBitacora.DataBind();
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            //Mensajes.ShowError(this.Page, this.GetType(), ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), message);
        }
    }

    /// <summary>
    /// Exportar a Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            this.rgBitacora.ExportSettings.ExportOnlyData = true;
            this.rgBitacora.MasterTableView.ExportToExcel();
            //this.rgBitacora.MasterTableView.ExportToCSV();


            //this.rgBitacora.MasterTableView.AllowPaging = false;
            //this.rgBitacora.Rebind();

            ////foreach (GridDataItem dgi in this.rgBitacora.MasterTableView.Items)
            ////    dgi.Expanded = true;

            //string fileName = string.Format("{0}{2}{1}", "Bitacora-2024", ".xlsx", DateTime.Now.ToString());

            //System.Data.DataSet dsReportePM = ExportarExcel.GenerarExcelBitacora(this.rgBitacora);
            //ExportarReportePM(dsReportePM, fileName);

            //this.rgBitacora.MasterTableView.AllowPaging = true;
            //this.rgBitacora.Rebind();

            //Guardando bitácora
            Bitacora bitacora = this.GetBitacoraTemplate();
            bitacora.Comments = "Catálogo de bitácora exportado en Excel.";
            bitacora.EventId = (int)BitacoraEventoTipoEnum.UsuarioSesionIniciada;
            bitacora.LogType = BitacoraTipoEstatusEnum.Successful;
            BitacoraRules.AgregarBitacora(bitacora);
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }
    }

    private void ExportarReportePM(System.Data.DataSet dsReporte, string NombreArchivo)
    {
        GridView grvTemporal = new GridView();
        grvTemporal.DataSource = dsReporte;
        grvTemporal.DataBind();

        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + NombreArchivo);
        Response.Charset = "";

        this.EnableViewState = false;
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

        grvTemporal.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }

    /// <summary>
    /// Actualizar información del catálogo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.rdpFechaInicial.SelectedDate = null;
        this.rdpFechaFinal.SelectedDate = null;
        this.ddlEvento.SelectedIndex = -1;
        this.ddlUsuario.SelectedIndex = -1;
        this.rgBitacora.CurrentPageIndex = 0;
        try
        {
            List<BitacoraViewModel> result = this.GetBitacoraData(null, null, null, null);

            this.rgBitacora.DataSource = result;
            this.rgBitacora.DataBind();
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }
    }


    #endregion


    #region Eventos del grid...


    /// <summary>
    /// Formateando datos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rgBitacora_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if ((e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item) && (GridDataItem)e.Item != null)
        {
            GridDataItem item = (GridDataItem)e.Item;

            //Fecha y Hora
            if (!string.IsNullOrEmpty(item["CreationDate"].Text))
            {
                string originalDate = item["CreationDate"].Text;
                string date = Convert.ToDateTime(originalDate).ToString("dd-MM-yyyy");
                item["CreationDate"].Text = date;

                var time = Convert.ToDateTime(originalDate);
                var stringTime = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hour, time.Minute, time.Second);
                item["Time"].Text = stringTime;
            }

            //Evento
            if (!string.IsNullOrEmpty(item["EventId"].Text) && this.eventosLista.Any())
            {
                int eventId = Convert.ToInt32(item["EventId"].Text);
                item["EventName"].Text = this.eventosLista.FirstOrDefault(x => x.Evento_Id == eventId).Descripcion;
            }
        }
    }

    /// <summary>
    /// Obteniendo catálogo de bitácora
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rgBitacora_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            DateTime? fechaInicial = this.rdpFechaInicial.SelectedDate != null ? this.rdpFechaInicial.SelectedDate : DateTime.MinValue;
            DateTime? fechaFinal = this.rdpFechaFinal.SelectedDate != null ? this.rdpFechaFinal.SelectedDate : DateTime.MinValue;
            int? evento = this.ddlEvento.SelectedItem.Value != "-1" ? int.Parse(this.ddlEvento.SelectedValue) : 0;
            var usuario = this.ddlUsuario.SelectedItem.Value != "-1" ? this.ddlUsuario.SelectedValue : null;

            var result = this.GetBitacoraData(fechaInicial, fechaFinal.Value.AddDays(1), evento, usuario);

            this.rgBitacora.DataSource = result;
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }
    }


    #endregion


    #region Métodos ...


    /// <summary>
    /// Obtener el catálogo de Bitácora
    /// </summary>
    /// <param name="fechaInicial">Fecha inicial de consulta</param>
    /// <param name="fechaFinal">Fecha hasta donde se desea consultar</param>
    /// <param name="eventoId">Identificador de Evento</param>
    /// <param name="userName">Nombre de usuario por el que se desea consultar</param>
    /// <returns></returns>
    private List<BitacoraViewModel> GetBitacoraData(DateTime? fechaInicial, DateTime? fechaFinal, int? eventoId, string userName)
    {
        List<BitacoraViewModel> result = new List<BitacoraViewModel>();
        try
        {
            var response = BitacoraRules.ObtenerBitacora(fechaInicial, fechaFinal, eventoId, userName);
            if (response.Any())
            {
                this.eventosLista = this.GetEventsData();
                foreach (var item in response)
                {
                    BitacoraViewModel newItem = new BitacoraViewModel();
                    newItem.Comments = item.Comments;
                    newItem.CreationDate = item.CreationDate;
                    newItem.EmployeeNumber = item.EmployeeNumber;
                    newItem.EventId = item.EventId;
                    newItem.LogId = item.LogId;
                    newItem.LogType = item.LogType;
                    newItem.Request = item.Request;
                    newItem.SessionIP = item.SessionIP;
                    newItem.UserFullName = item.UserFullName;
                    newItem.UserLogin = item.UserLogin;
                    newItem.EventName = this.eventosLista.FirstOrDefault(x => x.Evento_Id == item.EventId).Descripcion;

                    result.Add(newItem);
                }
            }
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }

        return result;    
    }

    /// <summary>
    /// Obtener la información inicial para guardar la bitácora.
    /// Los campos faltantes a definir son: Comments, EventId, Request
    /// </summary>
    /// <returns></returns>
    private Bitacora GetBitacoraTemplate()
    {
        int transactionEmployeeId = !string.IsNullOrEmpty(Session["EmpleadoId"].ToString().Trim()) ? Parser.ToNumber(Session["EmpleadoId"].ToString()) : 0;
        Bitacora bitacora = new Bitacora
        {
            CreationDate = DateTime.Now,
            //Comments = String.Format("Se ha eliminado información del rol. Identificador: {0}, Descripción: {1}.", currentRolData.Id, currentRolData.Descripcion),
            EmployeeNumber = transactionEmployeeId,
            //EventId = (int)BitacoraEventoTipoEnum.Rol_Baja,
            LogType = BitacoraTipoEstatusEnum.AccountManagement,
            //Request = JsonConvert.SerializeObject(rolToRemoveData) + "   " + JsonConvert.SerializeObject(facultadRolesRemoveData),
            SessionIP = Session["SessionIP"].ToString(),
            UserLogin = Session["UserLogin"].ToString(),
            UserFullName = Session["nombreUser"].ToString()
        };

        return bitacora;
    }

    /// <summary>
    /// Obteniendo el catálogo de Eventos.
    /// </summary>
    /// <returns></returns>
    private List<BitacoraEvento> GetEventsData()
    {
        List<BitacoraEvento> eventList = new List<BitacoraEvento>();

        try
        {
            //BitacoraRules br = new BitacoraRules();
            eventList = BitacoraRules.ObtenerEventos();
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }

        return eventList;
    }

    /// <summary>
    /// Obtener información de Usuarios
    /// </summary>
    /// <returns></returns>
    private List<Usuario> GetUsersData()
    {
        List<Usuario> userList = new List<Usuario>();
        try
        {
            UsuarioEntidadRules uer = new UsuarioEntidadRules();
            userList = uer.Usuarios();
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }

        return userList;
    }

    /// <summary>
    /// Cargando información inicial
    /// </summary>
    private void LoadInitalData()
    {
        try
        {
            this.eventosLista = this.GetEventsData();
            if (this.eventosLista.Any())
            {
                this.ddlEvento.DataSource = this.eventosLista;
                this.ddlEvento.DataTextField = "Descripcion";
                this.ddlEvento.DataValueField = "Evento_Id";
                this.ddlEvento.DataBind();
            }
            this.ddlEvento.Items.Insert(0, new ListItem { Text = "[ Seleccionar ]", Value = "-1" });

            //var userList = this.GetUsersData();
            //if (userList.Any())
            //{
            //    this.ddlUsuario.DataSource = userList;
            //    this.ddlUsuario.DataTextField = "Login";
            //    this.ddlUsuario.DataValueField = "Id";
            //    this.ddlUsuario.DataBind();
            //}
            var userList = this.GetBitacoraData(null, null, null, null).GroupBy(x => x.UserLogin).Select(y => y.First()).ToList();
            if (userList.Any())
            {
                this.ddlUsuario.DataSource = userList;
                this.ddlUsuario.DataTextField = "FullName_Login";
                this.ddlUsuario.DataValueField = "EmployeeNumber";
                this.ddlUsuario.DataBind();
            }
            this.ddlUsuario.Items.Insert(0, new ListItem { Text = "[ Seleccionar ]", Value = "-1" });
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowMessage(this.Page, this.GetType(), "Verificar su usuario");
        }
    }


    #endregion

    
}