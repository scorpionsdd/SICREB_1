using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Telerik.Web.UI;
using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Common.ExceptionMng;
using Banobras.Credito.SICREB.Entities;

public partial class Bitacora_BitacoraReportes : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //Dependiendo del valor del parámetro asociado en la URL, será el tipo de reporte a ejecutar
        string reportType = Request.QueryString["Tipo"];
        
        string NombreArchivo = string.Empty;
        List<Bitacora> data = BitacoraRules.ObtenerBitacora(null, null, null, null);
        List<Bitacora> dataFiltered = new List<Bitacora>();

        switch (reportType)
        {
            case BitacoraTipoEstatusEnum.Successful:
                NombreArchivo = "Bitacora-Exitosos";
                break;
            case BitacoraTipoEstatusEnum.NotSuccessful:
                NombreArchivo = "Bitacora-NoExitosos";
                break;
            case BitacoraTipoEstatusEnum.Permissions:
                NombreArchivo = "Bitacora-Permisos";
                break;
            case BitacoraTipoEstatusEnum.AccountManagement:
                NombreArchivo = "Bitacora-Gestion-Cuentas";
                break;
            default:
                reportType = BitacoraTipoEstatusEnum.Successful;
                NombreArchivo = "Bitacora-Exitosos";
                break;
        }

        dataFiltered = data.Where(x => x.LogType == reportType).ToList();
        if (!dataFiltered.Any())
        {
            Mensajes.ShowMessage(Page, this.GetType(), "No se encontraron registros con el filtro especificado.");
            return;
        }
        
        this.GenerarTipoReporte(dataFiltered, NombreArchivo);
    }

    /// <summary>
    /// Creando el reporte por tipo
    /// </summary>
    /// <param name="bitacora">Data de la bitácora</param>
    /// <param name="NombreArchivo">Nombre de archivo resultante</param>
    private void GenerarTipoReporte(List<Bitacora> bitacora, string NombreArchivo)
    {
        string fileName = string.Format("{0}_{2}{1}", NombreArchivo, ".xlsx", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
        try
        {
            RadGrid grvTemporal = new RadGrid();
            Page.Controls.Add(grvTemporal);

            List<GridBoundColumn> columnas = this.GenerateColumns();
            foreach (var item in columnas)
            {
                grvTemporal.MasterTableView.Columns.Add(item);
            }
            grvTemporal.AutoGenerateColumns = false;

            //drFila["Fecha"] = item.CreationDate.ToString("dd-MM-yyyy");
            //var time = Convert.ToDateTime(item.CreationDate);
            //var stringTime = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hour, time.Minute, time.Second);

            grvTemporal.DataSource = bitacora;
            grvTemporal.DataBind();

            grvTemporal.ExportSettings.FileName = fileName;
            grvTemporal.ExportSettings.ExportOnlyData = true;
            grvTemporal.ExportSettings.IgnorePaging = true;
            grvTemporal.ExportSettings.OpenInNewWindow = true;
            grvTemporal.MasterTableView.ExportToExcel();

            Page.Response.ClearHeaders();
            Page.Response.ClearContent();
        }
        catch (Exception ex)
        {
            string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            Mensajes.ShowError(this.Page, this.GetType(), message);
        }        
    }

    /// <summary>
    /// Generar los encabezados de las columnas
    /// </summary>
    /// <returns></returns>
    private List<GridBoundColumn> GenerateColumns()
    {
        List<GridBoundColumn> columnas = new List<GridBoundColumn>();

        columnas.Add(new GridBoundColumn { UniqueName = "LogId", DataField = "LogId", HeaderText = "" });
        columnas.Add(new GridBoundColumn { UniqueName = "CreationDate", DataField = "CreationDate", HeaderText = "Fecha" });
        columnas.Add(new GridBoundColumn { UniqueName = "CreationDate", DataField = "CreationDate", HeaderText = "Hora" });
        columnas.Add(new GridBoundColumn { UniqueName = "EmployeeNumber", DataField = "EmployeeNumber", HeaderText = "Empleado Id" });
        columnas.Add(new GridBoundColumn { UniqueName = "UserLogin", DataField = "UserLogin", HeaderText = "Login" });
        columnas.Add(new GridBoundColumn { UniqueName = "UserName", DataField = "UserName", HeaderText = "Nombre" });
        columnas.Add(new GridBoundColumn { UniqueName = "EventId", DataField = "EventId", HeaderText = "Evento" });
        columnas.Add(new GridBoundColumn { UniqueName = "Comments", DataField = "Comments", HeaderText = "Comentarios" });
        columnas.Add(new GridBoundColumn { UniqueName = "SessionIP", DataField = "SessionIP", HeaderText = "IP" });
        columnas.Add(new GridBoundColumn { UniqueName = "LogType", DataField = "LogType", HeaderText = "Tipo Bitácora" });

        return columnas;
    }

}