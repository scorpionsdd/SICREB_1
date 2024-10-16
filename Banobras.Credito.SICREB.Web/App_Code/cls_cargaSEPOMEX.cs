using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.IO;
using System.Data;
using Banobras.Credito.SICREB.Data;
using System.Data.Common;
using System.Text;
/// <summary>
/// Descripción breve de cls_cargaSEPOMEX
/// </summary>
public class cls_cargaSEPOMEX
{

	public cls_cargaSEPOMEX()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
        log.Append("No se pudo abrir el archivo, compruebe extensión y contenido.");
        this.Errores++;
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

    public OleDbConnection getRutaArchivoSEPOMEX(string stringPathP)
    {

        OleDbConnection conexionArchivo;
        try
        {
            string ruta_archivo = Path.GetFullPath(stringPathP); 
            string sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + ruta_archivo + "; Extended Properties=Excel 8.0;";

            conexionArchivo = new OleDbConnection(sConnectionString);
            conexionArchivo.Open();
            
            return conexionArchivo;
        }
        catch (Exception e)
        {
            return conexionArchivo = null;
        }//try-catch

    }//getRutaArchivoLayout

    public int validacionExtencionCATSEPOMEX(string rutaArchivoP)
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
                this.log.Append(String.Format("Extención de archivo no es XLS \n\r"));
                this.Errores++;
                resTipoArchivo = 0;
            }

            return resTipoArchivo;

        }
        catch (Exception exp)
        {
            return -1;
        }//try-catch

    }//validacionExtencionArchivo

    public DataTable cargaSEPOMEX(string archivoLayout,string sp_sepomex)
    {
     
        cls_accesoData accesoData = new cls_accesoData();
        this.Log.Clear();
        this.Errores = 0;
          DataTable data_excel=null ;
          DataTable dt_rowsDB=null;



          DbParameter[] param = new DbParameter[17];

          param[0] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[0].Direction = ParameterDirection.Output;
          param[0].ParameterName = "id_OUT";
          param[0].DbType = DbType.Decimal;
          param[0].Size = 38;

          param[1] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[1].Direction = ParameterDirection.Output;
          param[1].ParameterName = "tipo_OUT";
          param[1].DbType = DbType.Decimal;
          param[1].Size = 38;

          param[2] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[2].Direction = ParameterDirection.Input;
          param[2].ParameterName = "d_codigop";
          param[2].DbType = DbType.String;
          param[2].Size = 10;


          param[3] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[3].Direction = ParameterDirection.Input;
          param[3].ParameterName = "d_asentap";
          param[3].DbType = DbType.String;
          param[3].Size = 100;

          param[4] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[4].Direction = ParameterDirection.Input;
          param[4].ParameterName = "d_tipo_asentap";
          param[4].DbType = DbType.String;
          param[4].Size = 50;


          param[5] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[5].Direction = ParameterDirection.Input;
          param[5].ParameterName = "D_mnpiop";
          param[5].DbType = DbType.String;
          param[5].Size = 100;


          param[6] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[6].Direction = ParameterDirection.Input;
          param[6].ParameterName = "d_estadop";
          param[6].DbType = DbType.String;
          param[6].Size = 50;


          param[7] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[7].Direction = ParameterDirection.Input;
          param[7].ParameterName = "d_ciudadp";
          param[7].DbType = DbType.String;
          param[7].Size = 100;

          param[8] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[8].Direction = ParameterDirection.Input;
          param[8].ParameterName = "d_CPp";
          param[8].DbType = DbType.String;
          param[8].Size = 30;

          param[9] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[9].Direction = ParameterDirection.Input;
          param[9].ParameterName = "c_estadop";
          param[9].DbType = DbType.String;
          param[9].Size = 50;


          param[10] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[10].Direction = ParameterDirection.Input;
          param[10].ParameterName = "c_oficinap";
          param[10].DbType = DbType.String;
          param[10].Size = 20;


          param[11] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[11].Direction = ParameterDirection.Input;
          param[11].ParameterName = "c_CPp";
          param[11].DbType = DbType.String;
          param[11].Size = 30;


          param[12] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[12].Direction = ParameterDirection.Input;
          param[12].ParameterName = "c_tipo_asentap";
          param[12].DbType = DbType.String;
          param[12].Size = 20;

          param[13] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[13].Direction = ParameterDirection.Input;
          param[13].ParameterName = "c_mnpiop";
          param[13].DbType = DbType.String;
          param[13].Size = 15;

          param[14] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[14].Direction = ParameterDirection.Input;
          param[14].ParameterName = "id_asenta_cpconsp";
          param[14].DbType = DbType.String;
          param[14].Size = 30;


          param[15] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[15].Direction = ParameterDirection.Input;
          param[15].ParameterName = "d_zonap";
          param[15].DbType = DbType.String;
          param[15].Size = 30;

          param[16] = OracleBase.DB.DbProviderFactory.CreateParameter();
          param[16].Direction = ParameterDirection.Input;
          param[16].ParameterName = "c_cve_ciudadp";
          param[16].DbType = DbType.String;
          param[16].Size = 30;

          OleDbConnection ObjConn = getRutaArchivoSEPOMEX(archivoLayout);

        try
        {
           
            if (validacionExtencionCATSEPOMEX(archivoLayout) == 1)
            {
                String name = String.Empty;
                using (DataTable tableschema = ObjConn.GetSchema(OleDbMetaDataCollectionNames.Tables))
                {
                    // first column name
                  
                    accesoData.excecutecmd("truncate table T_SEPOMEX", new DbParameter[0] { });
                    foreach (DataRow row in tableschema.Rows)
                    {
                        name = row["TABLE_NAME"].ToString();
                       data_excel = new DataTable();
                       dt_rowsDB = new DataTable();

                       if (!name.Contains("$"))//checks whether row contains '_xlnm#_FilterDatabase' or sheet name(i.e. sheet name always ends with $ sign)
                       {
                           continue;
                       }


                        OleDbCommand objCmdSelect = new OleDbCommand("SELECT *  FROM [" + name + "]", ObjConn);
                        

                        OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                        objAdapter.SelectCommand = objCmdSelect;

                        DataSet objDataset = new DataSet();

                        objAdapter.Fill(objDataset, "tbl_excel");
                        data_excel = objDataset.Tables["tbl_excel"];



                        dt_rowsDB.Columns.Add("ID_ROW");
                        dt_rowsDB.Columns.Add("TIPO_OPERACION");

                       
                        string storeBase = "SP_CM_sepomex";

                        for (int irow = 0; irow < data_excel.Rows.Count; irow++)
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
                                    if (icol >= 2)
                                    {
                                        
                                        parametros[icol].Value = data_excel.Rows[irow][numColumna].ToString().Replace(',','*');
                                        values += " ," + data_excel.Rows[irow][numColumna];
                                        numColumna++;
                                    }
                                }


                                accesoData.fn_getResultadoSTORE_Command(storeBase, parametros);

                                DataRow row_resultado;

                                row_resultado = dt_rowsDB.NewRow();
                                row_resultado["ID_ROW"] = parametros[0].Value.ToString();
                                //parametros[1].Value.ToString();

                                if ("1" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "Insercción";
                                    this.Correctos++;
                                }
                                else if ("-100" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "ERROR en la insercción";
                                    if (parametros[3].Value == DBNull.Value || parametros[3].Value == "" || parametros[3].Value == null) continue;
                                    this.log.Append(String.Format("ERROR en la insercción de {1} posible duplicidad o valor nulo,valores({0})\n\r", values,name));
                                    this.Errores++;
                                }
                                else if ("2" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "Actulización";
                                }
                                else if ("-200" == parametros[1].Value.ToString())
                                {
                                    row_resultado["TIPO_OPERACION"] = "ERROR en la actulización";
                                }

                                dt_rowsDB.Rows.Add(row_resultado);
                            }
                            catch (Exception e)
                            {
                                this.Log.Append(String.Format("Error al insertar fila con datos ({0}) \n\r", values));
                                this.Errores++;
                                continue;
                            }//

                        }//for



                    }
                }


            }//if validacionExtencionArchivo
            ObjConn.Close();
            return dt_rowsDB;

        }
        catch (Exception e)
        {
           
            this.Log.Append(String.Format("Error al consultar datos del archivo, el layout no corresponde a SEPOMEX \n\r"));
            this.Errores++;
            ObjConn.Close();
            return data_excel = null;
        }//try-catch


    }//cargaMasiva




}