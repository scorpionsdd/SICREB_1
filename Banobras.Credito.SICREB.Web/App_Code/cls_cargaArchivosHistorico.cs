using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using Banobras.Credito.SICREB.Data;
using System.Data.Common;
using System.Text;

public class cls_cargaArchivosHistorico
{
    public cls_cargaArchivosHistorico()
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
    
    //DataTable cabezal_grid,
    public Boolean cargaMasiva(string catalogo, string archivoLayout, string select, DbParameter[] param, string sp_bd, string SQLWhere = "" )
    {
        log.Clear();
        DataTable data_layout = new DataTable();
        Boolean todoOk = false;
        cls_accesoData accesoData = new cls_accesoData();

        try
        {
            if (validacionExtencionArchivo(archivoLayout) == 1)
            {
                accesoData = new cls_accesoData();
                int numeroParametros = param.Length;

                DbParameter[] parametros = new DbParameter[numeroParametros];
                for (int ipar = 0; ipar < numeroParametros; ipar++)
                {
                    parametros[ipar] = OracleBase.DB.DbProviderFactory.CreateParameter();
                    parametros[ipar].ParameterName = param[ipar].ParameterName;
                    parametros[ipar].DbType = param[ipar].DbType;
                    parametros[ipar].Direction = param[ipar].Direction;
                    parametros[ipar].Size = param[ipar].Size;
                    parametros[ipar].Value = param[ipar].Value;
                }
                accesoData.fn_getResultadoSTORE_Command(sp_bd, parametros);

                todoOk= true;

            }//if validacionExtencionArchivo
            //ObjConn.Close();
            return todoOk;
        }
        catch (Exception e)
        {
            this.log.Append(String.Format("Error al Intentar guardar el archivo \n\r"));
            this.Errores++;
            //ObjConn.Close();
            return false;
        }//try-catch
    }//cargaMasiva
    
    public int validacionExtencionArchivo(string rutaArchivoP)
    {
        int resTipoArchivo = 0;
        FileInfo archivoInfo = new FileInfo(rutaArchivoP);

        try
        {
            if (archivoInfo.Extension == ".txt")
            {
                resTipoArchivo = 1;
            }
            else
            {
                resTipoArchivo = 0;
                this.log.Append(String.Format("Extensión de Archivo no es TXT. "));
                this.Errores++;
            }
            return resTipoArchivo;
        }
        catch (Exception exp)
        {
            return -1;
        }//try-catch
    }//validacionExtencionArchivo
    

    public int validacionHead(DataTable tbl_head_BD, DataTable tbl_head_file)
    {

        int diferenciaHeadArchivoCargado = 0;
        try
        {
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
    
}
