using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.OleDb;
using System.IO;
using System.Data;
using Banobras.Credito.SICREB.Data;
using System.Data.Common;
using System.Text;

public class cls_cargaArchivos:ExportToExcel
{
    public cls_cargaArchivos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
        log.Append("Imposible abrir el archivo que esta usando, compruebe la extensión y contenido");
    }

    private StringBuilder log = new StringBuilder();
    public StringBuilder Log
    {
        get { return log; }
        set { log.Append(value); }
    }
    private int errores = 0;
    public int Errores
    {
        get { return errores; }
        set { errores = value; }
    }
    private int correctos = 0;
    public int Correctos
    {
        get { return correctos; }
        set { correctos = value; }
    }

    //public OleDbConnection getRutaArchivoLayout(string stringPathP)
    //{

    //    OleDbConnection conexionArchivo;
    //    try
    //    {
    //        string ruta_archivo = Path.GetFullPath(stringPathP); //file_txt_layout.PostedFile.FileName
    //        string sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ruta_archivo + ";Extended Properties=Excel 8.0;";
    //        conexionArchivo = new OleDbConnection(sConnectionString);
    //        conexionArchivo.Open();
    //        return conexionArchivo;
    //    }
    //    catch (Exception e)
    //    {
    //        this.log.Append(String.Format("El formato del archivo debe ser excel 2003 / xls"));
    //        this.Errores++;
    //        return conexionArchivo = null;
    //    }//try-catch

    //}//getRutaArchivoLayout

  //  private string getSheetName(OleDbConnection ObjConn)
  //  {
  //      string strSheetName = String.Empty;
  //      try
  //      {
  //          System.Data.DataTable dtSheetNames = ObjConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
  //new object[] { null, null, null, "TABLE" });

  //          if (dtSheetNames.Rows.Count > 0)
  //          {
  //              strSheetName = dtSheetNames.Rows[0]["TABLE_NAME"].ToString();
  //          }
  //          return strSheetName;
  //      }
  //      catch (Exception ex)
  //      {
  //          throw new Exception("Falló. Al obtener el nombre de la hoja en el archivo que cargó", ex);
  //      }
  //  }

    public DataTable cargaMasiva(string catalogo, DataTable cabezal_grid, string archivoLayout, string select, DbParameter[] param, string sp_bd, string tipoPresona, string SQLWhere = "" )
    {
        log.Clear();
        DataTable data_layout = new DataTable();
        DataTable dt_rowsDB = new DataTable();
        cls_accesoData accesoData = new cls_accesoData();

        try
        {
            if (validacionExtencionArchivo(archivoLayout) == 1)
            {
                List<string> lstBoxLogs = new List<string>();


                DataSet objDataset = GetDatasetFromExcelFile(archivoLayout,0,"tbl_col");
                data_layout = objDataset.Tables["tbl_col"];

                dt_rowsDB.Columns.Add("ID_ROW");
                dt_rowsDB.Columns.Add("TIPO_OPERACION");

                if (validacionHead(cabezal_grid, data_layout) == 0)
                {
                    //if (vadilidaTipoDato(cabezal_grid, data_layout) == 0) {

                    for (int irow = 0; irow < data_layout.Rows.Count; irow++)
                    {
                        String values = "";
                        try
                        {
                            int numeroParametros = param.Length;
                            int resp_trueFalse = 0;

                            DbParameter[] parametros = new DbParameter[numeroParametros];

                            for (int ipar = 0; ipar < numeroParametros; ipar++)
                            {
                                parametros[ipar] = OracleBase.DB.DbProviderFactory.CreateParameter();
                                parametros[ipar].ParameterName = param[ipar].ParameterName;
                                parametros[ipar].DbType = param[ipar].DbType;
                                parametros[ipar].Direction = param[ipar].Direction;
                                parametros[ipar].Size = param[ipar].Size;
                            }
                            int numColumna = 0;

                            for (int icol = 0; icol < numeroParametros; icol++)
                            {
                                int num = 3;
                                if (tipoPresona == "") num = 2;
                                if (icol >= num)
                                {
                                    try
                                    {
                                        try
                                        {
                                            parametros[icol].Value = ((String)data_layout.Rows[irow][numColumna]).Replace("'", "");
                                        }
                                        catch (Exception e)
                                        {

                                            parametros[icol].Value = data_layout.Rows[irow][numColumna];

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        parametros[icol].Value = "";
                                    }
                                    values += " ," + parametros[icol].Value;
                                    if (tipoPresona != "")
                                        parametros[2].Value = tipoPresona;
                                    numColumna++;

                                }
                                else
                                {
                                }
                            }
                            accesoData = new cls_accesoData();
                            accesoData.fn_getResultadoSTORE_Command(sp_bd, parametros);

                            DataRow row_resultado;

                            row_resultado = dt_rowsDB.NewRow();
                            if (parametros[0].Value != null && parametros[0].Value != DBNull.Value)
                                row_resultado["ID_ROW"] = parametros[0].Value.ToString();
                            else
                                row_resultado["ID_ROW"] = 0;
                            //parametros[1].Value.ToString();

                            if (parametros[1].Value != null && parametros[1].Value != DBNull.Value)
                            {
                                if ("1" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "Inserción Correcta";
                                    this.Correctos++;

                                }
                                else if ("-100" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "ERROR en la inserción";
                                    this.log.Append(String.Format("Error en la inserción ID duplicado Nulo o Inexistente, valores({0})\n\r", values));
                                    this.Errores++;
                                }
                                ///FBS Validación de cuenta no existente 11/12/2021
                                else if ("-999" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "ERROR en la inserción";
                                    this.log.Append(String.Format("Error en la inserción la cuenta actual o cuenta nueva no xistente en el catálogo de cuentas, valores({0})<br/>\n\r", values));
                                    this.Errores++;
                                }
                                else if (("0" == parametros[1].Value.ToString()) && ("000" == parametros[0].Value.ToString()))
                                {
                                    row_resultado["TIPO_OPERACION"] = "Valores Vacios";
                                    this.log.Append(String.Format("Error: Valores vacios, valores ({0}) \n\r", values));
                                    this.Errores++;
                                }
                                else
                                {
                                    row_resultado["TIPO_OPERACION"] = "Inserción Correcta";
                                    // this.Correctos++;
                                    this.log.Append(String.Format("ADVERTENCIA: el Registro ya existe: Se ha cambiado a estatus activo({0})\n\r", values));
                                    this.Errores++;
                                } 
                            }
                            else if ("-100" == parametros[1].Value.ToString())
                            {
                                row_resultado["TIPO_OPERACION"] = "ERROR en la inserción";
                                this.log.Append(String.Format("Error en la inserción ID duplicado Nulo o Inexistente, valores({0})\n\r", values));
                                this.Errores++;
                            }
                            else if (("0" == parametros[1].Value.ToString()) && ("000" == parametros[0].Value.ToString()))
                            {
                                row_resultado["TIPO_OPERACION"] = "Valores Vacios";
                                this.log.Append(String.Format("Error: Valores vacíos, valores ({0}) \n\r", values));
                                this.Errores++;
                            }
                            else
                            {
                                row_resultado["TIPO_OPERACION"] = "Inserción Correcta";
                                // this.Correctos++;
                                this.log.Append(String.Format("ADVERTENCIA: el Registro ya existe: Se ha cambiado a estatus activo({0})\n\r", values));
                                this.Errores++;
                            }
                            dt_rowsDB.Rows.Add(row_resultado);

                        }
                        catch (Exception e)
                        {
                            this.log.Append(String.Format("Error al insertar fila con datos ({0}) \n\r", values));
                            this.Errores++;
                            continue;
                            //   return data_layout = null;
                        }//

                    }//for

                    //} //if valida Head

                }
                else
                {
                    return dt_rowsDB = null;
                    //ObjConn.Close();
                }//else-if

            }//if validacionExtencionArchivo
            //ObjConn.Close();
            return dt_rowsDB;

        }

        catch (Exception e)
        {
            this.log.Append(String.Format("Error al Consultar datos del Archivo, el layout no coincide con la tabla \n\r"));
            this.Errores++;
            //ObjConn.Close();
            return data_layout = null;
        }//try-catch


    }//cargaMasiva

    public DataTable cargaMasivaDS(string catalogo, DataTable cabezal_grid, DataSet RegistrosRC, string select, DbParameter[] param, string sp_bd, string tipoPresona, string SQLWhere = "")
    {
        log.Clear();
        DataTable data_layout = new DataTable();
        DataTable dt_rowsDB = new DataTable();
        cls_accesoData accesoData = new cls_accesoData();

        try
        {
            if (RegistrosRC.Tables[0].Rows.Count > 0)
            {
                List<string> lstBoxLogs = new List<string>();
                //DataSet objDataset = GetDatasetFromExcelFile(archivoLayout, 0, "tbl_col");
                DataSet objDataset = RegistrosRC;
                data_layout = objDataset.Tables["tbl_col"];

                dt_rowsDB.Columns.Add("ID_ROW");
                dt_rowsDB.Columns.Add("TIPO_OPERACION");

                if (validacionHead(cabezal_grid, data_layout) == 0)
                {
                    //if (vadilidaTipoDato(cabezal_grid, data_layout) == 0) {

                    for (int irow = 0; irow < data_layout.Rows.Count; irow++)
                    {
                        String values = "";
                        try
                        {
                            int numeroParametros = param.Length;
                            int resp_trueFalse = 0;

                            DbParameter[] parametros = new DbParameter[numeroParametros];

                            for (int ipar = 0; ipar < numeroParametros; ipar++)
                            {
                                parametros[ipar] = OracleBase.DB.DbProviderFactory.CreateParameter();
                                parametros[ipar].ParameterName = param[ipar].ParameterName;
                                parametros[ipar].DbType = param[ipar].DbType;
                                parametros[ipar].Direction = param[ipar].Direction;
                                parametros[ipar].Size = param[ipar].Size;
                            }
                            int numColumna = 0;

                            for (int icol = 0; icol < numeroParametros; icol++)
                            {
                                int num = 3;
                                if (tipoPresona == "") num = 2;
                                if (icol >= num)
                                {
                                    try
                                    {
                                        try
                                        {
                                            parametros[icol].Value = ((String)data_layout.Rows[irow][numColumna]).Replace("'", "");
                                        }
                                        catch (Exception e)
                                        {

                                            parametros[icol].Value = data_layout.Rows[irow][numColumna];

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        parametros[icol].Value = "";
                                    }
                                    values += " ," + parametros[icol].Value;
                                    if (tipoPresona != "")
                                        parametros[2].Value = tipoPresona;
                                    numColumna++;

                                }
                                else
                                {
                                }
                               
                            }
                            accesoData = new cls_accesoData();
                            accesoData.fn_getResultadoSTORE_Command(sp_bd, parametros);

                            DataRow row_resultado;

                            row_resultado = dt_rowsDB.NewRow();
                            if (parametros[0].Value != null && parametros[0].Value != DBNull.Value)
                                row_resultado["ID_ROW"] = parametros[0].Value.ToString();
                            else
                                row_resultado["ID_ROW"] = 0;
                            //parametros[1].Value.ToString();

                            if (parametros[1].Value != null && parametros[1].Value != DBNull.Value)
                            {
                                if ("1" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "Inserción Correcta";
                                    this.Correctos++;

                                }
                                else if ("-100" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "ERROR en la inserción";
                                    this.log.Append(String.Format("Error en la inserción ID duplicado Nulo o Inexistente, valores({0})<br/>\n\r", values));
                                    this.Errores++;
                                }
                                else if (("0" == parametros[1].Value.ToString()) && ("000" == parametros[0].Value.ToString()))
                                {
                                    row_resultado["TIPO_OPERACION"] = "Valores Vacios";
                                    this.log.Append(String.Format("Error: Valores vacios, valores ({0}) \n\r", values));
                                    this.Errores++;
                                }
                                else
                                {
                                    row_resultado["TIPO_OPERACION"] = "Inseción Correcta";
                                    // this.Correctos++;
                                    this.log.Append(String.Format("ADVERTENCIA: el Registro ya existe: Se ha cambiado a estatus activo({0})<br/>\n\r", values));
                                    this.Errores++;
                                }
                            }
                            else if ("-100" == parametros[1].Value.ToString())
                            {
                                row_resultado["TIPO_OPERACION"] = "ERROR en la inserción";
                                this.log.Append(String.Format("Error en la inserción ID duplicado Nulo o Inexistente, valores({0})<br/>\n\r", values));
                                this.Errores++;
                            }
                            else if (("0" == parametros[1].Value.ToString()) && ("000" == parametros[0].Value.ToString()))
                            {
                                row_resultado["TIPO_OPERACION"] = "Valores Vacios";
                                this.log.Append(String.Format("Error: Valores vacíos, valores ({0}) \n\r", values));
                                this.Errores++;
                            }
                            else
                            {
                                row_resultado["TIPO_OPERACION"] = "Inserción Correcta";
                                // this.Correctos++;
                                this.log.Append(String.Format("ADVERTENCIA: el Registro ya existe: Se ha cambiado a estatus activo({0})<br/>\n\r", values));
                                this.Errores++;
                            }
                            dt_rowsDB.Rows.Add(row_resultado);

                        }
                        catch (Exception e)
                        {
                            this.log.Append(String.Format("Error al insertar fila con datos ({0}) <br/>\n\r", values));
                            this.Errores++;
                            continue;
                            //   return data_layout = null;
                        }//

                    }//for

                    //} //if valida Head

                }
                else
                {
                    return dt_rowsDB = null;
                }//else-if

            }//IF Numero de resgistros > 0

            return dt_rowsDB;
        }
        catch (Exception e)
        {
            this.log.Append(String.Format("Error al Consultar datos del Archivo, el layout no coincide con la tabla <br/>\n\r"));
            this.Errores++;
            return data_layout = null;
        }
    }






    public int validacionExtencionArchivo(string rutaArchivoP)
    {
        int resTipoArchivo = 0;
        FileInfo archivoInfo = new FileInfo(rutaArchivoP);

        try
        {
            if (archivoInfo.Extension == ".xls")
            {
                resTipoArchivo = 1;
            }
            else
            {
                resTipoArchivo = 0;
                this.log.Append(String.Format("Extensión de Archivo no es XLS. "));
                this.Errores++;
            }
            return resTipoArchivo;

        }
        catch (Exception exp)
        {
            return -1;
        }
    }//validacionExtencionArchivo



    public int validacionHead(DataTable tbl_head_BD, DataTable tbl_head_file)
    {        

        int diferenciaHeadArchivoCargado = 0;
        try
        {
            for (int i = 0; i < tbl_head_BD.Rows.Count; i++)
            {

                if (StringIgnoreCaseAndAccents(tbl_head_BD.Rows[i]["nombreColumna"].ToString()) != StringIgnoreCaseAndAccents(tbl_head_file.Columns[i].ColumnName.ToString()))
                {
                    diferenciaHeadArchivoCargado = 1;
                    this.log.Append(String.Format("Cabecera del Archivo no coincide con la Tabla"));
                    this.Errores++;
                    break;
                }
                else
                {
                    diferenciaHeadArchivoCargado = 0;
                }//else

            }//for

            return diferenciaHeadArchivoCargado;
        }
        catch (Exception e)
        {

            throw new Exception(e.Message.ToString(), e);
        }//try-catch


    }//cargaMasiva

    public int vadilidaTipoDato(DataTable tbl_head_BD, DataTable tbl_head_file)
    {

        int respuestaTipoDato = 0;
        try
        {

            for (int i = 0; i < tbl_head_BD.Columns.Count; i++)
            {
                if (tbl_head_BD.Rows[i]["tipoDato"].ToString() == "NUMBER")
                {
                    for (int irow = 0; irow < tbl_head_file.Rows.Count; irow++)
                    {

                        if (tbl_head_file.Rows[irow][i].ToString() != "")
                        {
                            try
                            {
                                int numer = int.Parse(tbl_head_file.Rows[irow][i].ToString());
                                numer = numer * 1;
                            }
                            catch (Exception NaN)
                            {

                                respuestaTipoDato = 1;
                                break;

                            }//trycatch
                        }//if                                  
                    }//for

                }
                else
                {

                }//else

            }//for


            return respuestaTipoDato;

        }
        catch (Exception e)
        {
            return respuestaTipoDato = 0;
        }//try-catch


    }//vadilidaTipoDato


    public DataTable dt_Registros(DataTable dt_layout)
    {
        DataTable dt_respuestaIDRegistro = new DataTable();

        dt_respuestaIDRegistro.Columns.Add("ID");
        DataRow row;
        cls_accesoData datosOracle = new cls_accesoData();

        try
        {



            return dt_respuestaIDRegistro;
        }
        catch (Exception e)
        {

            return dt_respuestaIDRegistro = null;
        }//try-catch

    }//comparaRegistros


    public static string StringIgnoreCaseAndAccents(string text)
    {
        byte[] tempBytes;
        tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(text);
        return System.Text.Encoding.UTF8.GetString(tempBytes).ToLowerInvariant();
    }

    /// <summary>
    /// Carga masiva excel, ultrasist 20211231 carga masiva accionistas
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void EliminaAccionistasTodos()
    {
        try
        {
            cls_accesoData accesoData = new cls_accesoData();
            accesoData.excecutecmd("UPDATE T_ACCIONISTAS SET ESTATUS = '0' WHERE ESTATUS = '1'", new DbParameter[0] { });
        }

        catch (Exception ex)
        {
        }

    }





}
