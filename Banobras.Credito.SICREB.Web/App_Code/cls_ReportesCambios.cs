using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Business.Rules.PM;
using Banobras.Credito.SICREB.Business.Rules.PF;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities;
using System.Data;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Data;

/// <summary>
/// Descripción breve de cls_ReportesCambios
/// </summary>
public class cls_ReportesCambios
{

    //private PM_Cinta_Rules pmCintaRules = null;
    //private PF_Cinta_Rules pfCintaRules = null;
    private ArchivosDataAccess archivosAccessPM = null;
    private ArchivosDataAccess archivosAccessPF = null;
    private ArchivosDataAccess archivosData = null;
    private Enums.Persona _pPersona;

	public cls_ReportesCambios()
	{
        //pmCintaRules = new PM_Cinta_Rules();
        //pfCintaRules = new PF_Cinta_Rules();
    }

    public cls_ReportesCambios(Enums.Persona persona)
    {
        archivosData = new ArchivosDataAccess(persona);
        _pPersona = persona;
    }

    public DataTable GeneraArchivoPM(int idArchivo, string grupos, string strMes, int iAnio)
    {
       
        System.Data.DataTable dtArchivoPM = new System.Data.DataTable();
        System.Data.DataTable datos = new System.Data.DataTable();
        CargarColumnasPM(ref dtArchivoPM);
        cls_accesoData AD = new cls_accesoData();
        int iCount = 1;

        System.Data.Common.DbParameter param;

        param = OracleBase.DB.DbProviderFactory.CreateParameter();
        param.Direction = ParameterDirection.Input;
        param.ParameterName = "pARCHIVOID";        
        param.DbType = DbType.String;
        param.Size = 50;
        param.Value = (String)idArchivo.ToString();

        datos = AD.cmdtodt("SP_ObtenerFILE_PM", new System.Data.Common.DbParameter[] {param });
        foreach (DataRow row in datos.Rows)
        {
           
            double dSaldiInit = 0;
            double.TryParse((row["CR_07"] is DBNull)?"":((String)row["CR_07"]).Trim(), out dSaldiInit);
            int iNumPagos = 0;
            Int32.TryParse((row["CR_09"] is DBNull) ? "" : ((String)row["CR_09"]).Trim(), out iNumPagos);
            dtArchivoPM.Rows.Add(strMes, iAnio,
                (row["CR_02"]), (row["EM_00"]), ((row["EM_04"]) + " " + (row["EM_05"])),
                (row["EM_06"]), (row["EM_07"]),
                ((row["EM_13"]) + " " + (row["EM_14"])), (row["EM_15"]),
                (row["EM_16"]), (row["EM_17"]),
                (row["EM_19"]), (row["EM_18"]), (row["CR_04"]),
                (row["CR_05"]), (row["CR_08"]), dSaldiInit,
                (row["CR_06"]), iNumPagos, (row["CR_10"]), (row["EM_08"]),
                (row["EM_10"]),
                (row["EM_23"]), (row["CR_19"]));
               
        }
        return dtArchivoPM;
    }

    public DataTable GeneraArchivoPF(int idArchivo, string grupos, string strMes, int iAnio)
    {
       
        System.Data.DataTable dtArchivoPF = new System.Data.DataTable();
        System.Data.DataTable datos = new System.Data.DataTable();
        CargarColumnasPF(ref dtArchivoPF);
        int iCount = 1;

        cls_accesoData AD = new cls_accesoData();
       
        System.Data.Common.DbParameter param;

        param = OracleBase.DB.DbProviderFactory.CreateParameter();
        param.Direction = ParameterDirection.Input;
        param.ParameterName = "pARCHIVOID";
        param.DbType = DbType.String;
        param.Size = 50;
        param.Value = (String)idArchivo.ToString();

        datos = AD.cmdtodt("SP_ObtenerFILE_PF", new System.Data.Common.DbParameter[] { param });



        foreach (DataRow row in datos.Rows)
        {
            //pn.Id
            string strLinea1 = string.Empty;
            string strColonia_Poblacion = string.Empty;
            string strMunicipio_Delegacion = string.Empty;
            string strCiudad = string.Empty;
            string strCodigoPostal = string.Empty;
            string strEstado = string.Empty;
                         //pa.PN_ID
                    //pa.Id
                    //pa.ParentId
            strLinea1 = row["CALLE"] + " " + row["NUM_EXT"];
            strColonia_Poblacion =(String)((row["PA_01"] is DBNull) ? "" : row["PA_01"]);
            strMunicipio_Delegacion = (String)((row["PA_02"] is DBNull)? "" :row["PA_02"]);
            strCiudad = (String)((row["PA_03"] is DBNull) ? "" : row["PA_03"]); ;
            strCodigoPostal = (String)((row["PA_05"] is DBNull) ? "" : row["PA_05"]);
            strEstado = (String)((row["PA_04"] is DBNull) ? "" : row["PA_04"]); 
               
            string strCredito = string.Empty;
            string strFechaApertura = string.Empty;
            string strTipoResponsabilidad = string.Empty;
            string strTipoCuenta = string.Empty;
            string strTipoContrato = string.Empty;
            string strNumeroPagos = string.Empty;
            string strFrecuenciaPago = string.Empty;
            string strFechaUltimaDisposicion = string.Empty;
            string strClaveObservacion = string.Empty;

            strCredito = (String)((row["CREDITO"] is DBNull) ? "" : row["CREDITO"]).ToString();
            strFechaApertura = (String)((row["FECHA_APERTURA"] is DBNull) ? "" : row["FECHA_APERTURA"]).ToString();
            strTipoResponsabilidad = "" ;
            strTipoCuenta = (String)((row["TIPO_CREDITO"] is DBNull) ? "" : row["TIPO_CREDITO"]).ToString();
            strTipoContrato = (String)((row["TIPO_CREDITO"] is DBNull) ? "" : row["TIPO_CREDITO"]).ToString();
            strNumeroPagos = (String)((row["NUM_PAGOS"] is DBNull) ? "" : row["NUM_PAGOS"]).ToString();
            strFrecuenciaPago = (String)((row["FREC_PAGOS"] is DBNull) ? "" : row["FREC_PAGOS"]).ToString();
            strFechaUltimaDisposicion = (String)((row["FECHA_ULTIMA_COMPRA"] is DBNull) ? "" : row["FECHA_ULTIMA_COMPRA"]).ToString();
            strClaveObservacion = (String)((row["CALIFICACION"] is DBNull) ? "" : row["CALIFICACION"]).ToString();
             
            string strMoneda = string.Empty;
           
            //pe.ParentId
            strMoneda = "";
            dtArchivoPF.Rows.Add(
                strMes,
                iAnio,
                strCredito,
                row["PN_05"],
                row["NOMBRE"],
                row["PN_PN"],
                row["PN_00"],
                strLinea1,
                strColonia_Poblacion,
                strMunicipio_Delegacion,
                strCiudad,
                strCodigoPostal,
                strEstado,
                strFechaApertura,
                strTipoResponsabilidad,
                strMoneda,
                strTipoCuenta,
                strTipoContrato,
                strNumeroPagos,
                strFrecuenciaPago,
                row["PN_08"],
                strFechaUltimaDisposicion,
                row["FECHA_NACIMIENTO"],
                row["PN_12"],
                strClaveObservacion);
            //dtArchivoPM.Rows.Add
        }
        //return cintaPF;
        return dtArchivoPF;
    }

    public DataTable GetMesesArchivo(int iAnio) {
        DataTable dtMeses = archivosData.GetMesesArchivos(iAnio);
        return dtMeses;
    }

    public DataTable GetAniosArchivo() {
        DataTable dtAnios = archivosData.GetAniosArchivos();
        return dtAnios;
    }

    public DataTable GetNombreArchivo(int mes, int anio)
    {
        var archivos = (from arc in archivosData.GetAllArchivosMesAnio(mes, anio)
                        where arc.Fecha.Year.Equals(anio)
                        orderby arc.Fecha descending
                        select arc).ToList();
        DataTable dtTable = new DataTable();
        dtTable.Columns.Add(new DataColumn("Id", typeof(int)));
        dtTable.Columns.Add(new DataColumn("Nombre", typeof(string)));

        foreach (Archivo arc in archivos) {
            dtTable.Rows.Add(arc.Id, arc.Nombre);
        }

        return dtTable;
    }

    private void CargarColumnasPM(ref DataTable dtArchivoPM)
    {
        //dtArchivoPM.Columns.Add(new DataColumn("IdReporte", typeof(int)));
        dtArchivoPM.Columns.Add(new DataColumn("Mes", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Año", typeof(int)));
        dtArchivoPM.Columns.Add(new DataColumn("Crédito", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("RFC", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Nombre", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Apellido Paterno", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Apellido Materno", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Línea1", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Colonia/Población", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Municipio/Delegación", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Ciudad", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Código Postal", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Estado", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Fecha de Apertura", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Plazo", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Moneda", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Saldo Inicial", typeof(double)));
        dtArchivoPM.Columns.Add(new DataColumn("Tipo de Crédito", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Número de Pagos", typeof(int)));
        dtArchivoPM.Columns.Add(new DataColumn("Frecuencia", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Nacionalidad", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Clave Banxico", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Tipo de Cliente", typeof(string)));
        dtArchivoPM.Columns.Add(new DataColumn("Clave de Observación", typeof(string)));
    }

    private void CargarColumnasPF(ref DataTable dtArchivoPF) {
        //dtArchivoPM.Columns.Add(new DataColumn("IdReporte", typeof(int)));
        dtArchivoPF.Columns.Add(new DataColumn("Mes", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Año", typeof(int)));
        dtArchivoPF.Columns.Add(new DataColumn("Crédito", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("RFC", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Nombre", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Apellido Paterno", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Apellido Materno", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Línea1", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Colonia/Población", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Municipio/Delegación", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Ciudad", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Código Postal", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Estado", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Fecha de Apertura", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Tipo de Responsabilidad", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Moneda", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Tipo de Cuenta", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Tipo de Contrato", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Número de Pagos", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Frecuencia de Pago", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Nacionalidad", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Fecha de Última Disposición", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Fecha de Nacimiento", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Sexo", typeof(string)));
        dtArchivoPF.Columns.Add(new DataColumn("Clave de Observación", typeof(string)));
    }

    public static DataTable CrearTabla(Enums.Persona pPersona) {
        cls_ReportesCambios clsMe = new cls_ReportesCambios();
        DataTable dtArchivo = new DataTable();
        clsMe._pPersona = pPersona;
        if (clsMe._pPersona == Enums.Persona.Fisica)
            clsMe.CargarColumnasPF(ref dtArchivo);
        else
            clsMe.CargarColumnasPM(ref dtArchivo);
        return dtArchivo;
    }


    #region Genera Tabla Con cambios

    public static DataTable GetTablaCambios(DataTable dtArchivo1, DataTable dtArchivo2) {
        cls_ReportesCambios clsMe = new cls_ReportesCambios();
        DataTable dtCambios = new DataTable();
        dtCambios = clsMe.BuscaCambiosReport(dtArchivo1, dtArchivo2);
        return dtCambios;
    }

    private DataTable BuscaCambiosReport(DataTable dtArchivo1, DataTable dtArchivo2)
    {
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        dt1 = dtArchivo1.DefaultView.ToTable(true, dtArchivo1.Columns[2].ColumnName.ToString());
        dt2 = dtArchivo2.DefaultView.ToTable(true, dtArchivo2.Columns[2].ColumnName.ToString());

        DataTable dtResult = new DataTable();
        dtResult = CreacionPlantillaTabla(dtArchivo1);

        string strAux = string.Empty;
        foreach (DataRow dr in dt1.Rows)
        {
            DataRow[] drFilasEncontradas1 = getBuscaRegistro(dtArchivo1, 2, dr[0].ToString());
            DataRow[] drFilasEncontradas2 = getBuscaRegistro(dtArchivo2, 2, dr[0].ToString());
            bool bDiferente = false;
            if (drFilasEncontradas1.Length > 1)
            {
                DataRow[] drFilasEncontradasAux = getBuscaRegistro(dtResult, 2, dr[0].ToString());
                if (drFilasEncontradasAux.Length == 0)
                {
                    for (int iRows = 0; iRows < drFilasEncontradas1.Length - 1; iRows++)
                    {
                        if (SonDiferentes(drFilasEncontradas1[iRows], drFilasEncontradas1[iRows + 1]))
                        {
                            //bDiferente = true;
                            for (int iRowsB = 0; iRowsB < drFilasEncontradas2.Length; iRowsB++)
                            {
                                if (SonDiferentes(drFilasEncontradas1[iRows], drFilasEncontradas2[iRowsB]))
                                    bDiferente = true;
                            }
                        }
                    }
                    if (bDiferente == true)
                    {
                        foreach (DataRow drFila1 in drFilasEncontradas1)
                        {
                            dtResult.Rows.Add(drFila1.ItemArray);
                        }
                        foreach (DataRow drFila2 in drFilasEncontradas2)
                        {
                            dtResult.Rows.Add(drFila2.ItemArray);
                        }
                        dtResult.Rows.Add();
                    }
                }
                bDiferente = false;
            }
            else
            {
                DataRow[] drFilasEncontradasAux = getBuscaRegistro(dtResult, 2, dr[0].ToString());
                if (drFilasEncontradasAux.Length == 0)
                {
                    for (int iRows = 0; iRows < drFilasEncontradas1.Length; iRows++)
                    {
                        for (int iRowsB = 0; iRowsB < drFilasEncontradas2.Length; iRowsB++)
                        {
                            if (SonDiferentes(drFilasEncontradas1[iRows], drFilasEncontradas2[iRowsB]))
                                bDiferente = true;
                        }
                    }
                    if (bDiferente == true)
                    {
                        foreach (DataRow drFila1 in drFilasEncontradas1)
                        {
                            dtResult.Rows.Add(drFila1.ItemArray);
                        }
                        foreach (DataRow drFila2 in drFilasEncontradas2)
                        {
                            dtResult.Rows.Add(drFila2.ItemArray);
                        }
                        dtResult.Rows.Add();
                    }
                }
                bDiferente = false;
            }
        }
        if (dtResult.Rows.Count > 0)
            dtResult.Rows.RemoveAt(dtResult.Rows.Count - 1);

        return dtResult;
    }

    private DataTable CreacionPlantillaTabla(DataTable dtDataTable)
    {
        int numColumns = dtDataTable.Columns.Count;
        DataTable dt = new DataTable();
        for (int i = 0; i < numColumns; i++)
        {
            dt.Columns.Add(dtDataTable.Columns[i].ColumnName, dtDataTable.Columns[i].DataType);
        }
        return dt;
    }

    private DataRow[] getBuscaRegistro(DataTable dtDataTable, int iColumnIndex, string strValorBuscar)
    {
        string strValorEncontrado = string.Empty;
        DataRow[] drFilasEncontradas;
        drFilasEncontradas = dtDataTable.Select(dtDataTable.Columns[iColumnIndex].ColumnName + " = '" + strValorBuscar + "'");
        return drFilasEncontradas;
    }

    private bool SonDiferentes(DataRow drFilaArch1, DataRow drFilaArch2)
    {
        bool bSonDiferentes = false;
        object[] objCamposArch1 = drFilaArch1.ItemArray;
        object[] objCamposArch2 = drFilaArch2.ItemArray;
        if (objCamposArch1.Length == objCamposArch2.Length)
        {
            for (int iCount = 2; iCount < objCamposArch1.Length - 1; iCount++)
            {
                if (!objCamposArch1[iCount].ToString().Trim().Equals(objCamposArch2[iCount].ToString().Trim()))
                    return true;
            }
        }
        return bSonDiferentes;
    }

    #endregion

}
public class TipoArch
{
    public enum Archivo { PrimerArch, SegundoArch };
}