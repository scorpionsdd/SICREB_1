using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Data.OracleClient;
using Banobras.Credito.SICREB.Entities.SICOFIN;
using System.ComponentModel;
using Banobras.Credito.SICREB.Common;
using Oracle.DataAccess.Client;

namespace Banobras.Credito.SICREB.Data
{
    public class clsSICOFIN
    {
        public string strConexion
        {
            get { return strConexion_; }
        }
        private string strConexion_;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="Conexion">Cadena de conexión requerida</param>
        public clsSICOFIN(string Conexion)
        {
            strConexion_ = Conexion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RegistroSemanal"></param>
        public void LimpiaSemanal(int empresa)
        {
            int resultado = -1;
            OracleCommand command = new OracleCommand();
            OracleConnection conn = new OracleConnection
            {
                ConnectionString = strConexion_//"data source=172.27.208.4:1523/zsicrebpr;user id=SICREB;password=sicreb"
            };

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    StringBuilder sQuery = new StringBuilder();
                    sQuery.AppendLine("delete from SCON_SALDOS_PCC where SCON_EMPRESA = @parametro1");
                    //FBS. 19032022 para pruebas en desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        sQuery.Clear();
                        sQuery.AppendLine("delete from SCON_SALDOS_PCC_Z where SCON_EMPRESA = @parametro1");
                    }
                    command.Parameters.Clear();
                    command.Parameters.Add("@parametro1", OracleDbType.Varchar2).Value =empresa;
                    command.Connection = conn;
                    command.CommandText = sQuery.ToString();
                    command.CommandType = CommandType.Text;
                    command.Transaction = conn.BeginTransaction();
                    resultado = command.ExecuteNonQuery();

                    command.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    command.Transaction.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RegistroMensual"></param>
        public void LimpiaMensual(int empresa)
        {
            int resultado = -1;
            OracleCommand command = new OracleCommand();
            OracleConnection conn = new OracleConnection
            {
                ConnectionString = strConexion_//"data source=172.27.208.4:1523/zsicrebpr;user id=SICREB;password=sicreb"
            };

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    StringBuilder sQuery = new StringBuilder();
                    string strQuery = string.Format("DELETE from SCON_SALDOS_ANT where SCON_EMPRESA = {0}", empresa);
                    sQuery.AppendLine("delete from SCON_SALDOS_ANT where SCON_EMPRESA = @parametro1");
                    //FBS. 19032022 para pruebas en desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        strQuery = string.Format("DELETE from SCON_SALDOS_ANT_Z where SCON_EMPRESA = {0}", empresa);
                        sQuery.Clear();
                        sQuery.AppendLine("delete from SCON_SALDOS_ANT_Z where SCON_EMPRESA = @parametro1");
                    }

                    //command.Parameters.Clear();
                    //command.Parameters.Add("@parametro1", OracleDbType.Int32).Value = empresa;
                    command.Connection = conn;
                    command.CommandText = strQuery; // sQuery.ToString();
                    command.CommandType = CommandType.Text;
                    command.Transaction = conn.BeginTransaction();

                    resultado = command.ExecuteNonQuery();
                    command.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    command.Transaction.Rollback();
                    throw new Exception(ex.Message);
                    //ExceptionUtility.LogException(ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="RegistroMensual"></param>
        public void GuardaMensual(clsSICOFIN_Mensual.Root RegistroMensual)
        {
            try
            {
                using (Oracle.DataAccess.Client.OracleConnection connection = new Oracle.DataAccess.Client.OracleConnection(strConexion_))
                {
                    connection.Open();
                    Oracle.DataAccess.Client.OracleTransaction trans = connection.BeginTransaction();
                    using (Oracle.DataAccess.Client.OracleBulkCopy bulkCopy = new Oracle.DataAccess.Client.OracleBulkCopy(connection))
                    {
                        DataTable table = new DataTable();
                        table = ConvertToDataTable(RegistroMensual.data);
                        bulkCopy.DestinationTableName = "SCON_SALDOS_ANT";

                        //FBS. 19032022 para pruebas en desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            bulkCopy.DestinationTableName = "SCON_SALDOS_ANT_Z";
                        }

                        bulkCopy.BulkCopyTimeout = 60;

                        bulkCopy.ColumnMappings.Add(table.Columns["anio"].ToString(), "SCON_ANIO");
                        bulkCopy.ColumnMappings.Add(table.Columns["mes"].ToString(), "SCON_MES");
                        bulkCopy.ColumnMappings.Add(table.Columns["area"].ToString(), "SCON_AREA");
                        bulkCopy.ColumnMappings.Add(table.Columns["moneda"].ToString(), "SCON_MONEDA");
                        bulkCopy.ColumnMappings.Add(table.Columns["numeroMayor"].ToString(), "SCON_NUMERO_MAYOR");
                        bulkCopy.ColumnMappings.Add(table.Columns["cuenta"].ToString(), "SCON_CUENTA");
                        bulkCopy.ColumnMappings.Add(table.Columns["sector"].ToString(), "SCON_SECTOR");
                        bulkCopy.ColumnMappings.Add(table.Columns["auxiliar"].ToString(), "SCON_AUXILIAR");
                        bulkCopy.ColumnMappings.Add(table.Columns["fechaUltimoMovimiento"].ToString(), "SCON_FECHA_ULTIMO_MOVIMIENTO");
                        bulkCopy.ColumnMappings.Add(table.Columns["saldo"].ToString(), "SCON_SALDO");
                        bulkCopy.ColumnMappings.Add(table.Columns["monedaOrigen"].ToString(), "SCON_MONEDA_ORIGEN");
                        bulkCopy.ColumnMappings.Add(table.Columns["naturalezaCuenta"].ToString(), "SCON_NATURALEZA_CUENTA");
                        bulkCopy.ColumnMappings.Add(table.Columns["saldoPromedio"].ToString(), "SCON_SALDO_PROMEDIO");
                        bulkCopy.ColumnMappings.Add(table.Columns["montoDebito"].ToString(), "SCON_MONTO_DEBITO");
                        bulkCopy.ColumnMappings.Add(table.Columns["montoCredito"].ToString(), "SCON_MONTO_CREDITO");
                        bulkCopy.ColumnMappings.Add(table.Columns["saldoAnterior"].ToString(), "SCON_SALDO_ANTERIOR");
                        bulkCopy.ColumnMappings.Add(table.Columns["empresa"].ToString(), "SCON_EMPRESA");
                        bulkCopy.ColumnMappings.Add(table.Columns["calificaMoneda"].ToString(), "SCON_CALIFICA_MONEDA");
                        bulkCopy.ColumnMappings.Add(table.Columns["dia"].ToString(), "SCON_DIA");

                        for (int i = 0; i < table.Rows.Count; i++)
                            if ((table.Rows[i]["calificaMoneda"] == null || table.Rows[i]["calificaMoneda"] as string == "null"))
                                table.Rows[i]["calificaMoneda"] = null;
                        //else
                        //    table.Rows[i]["calificaMoneda"] = "1";

                        try
                        {
                            DataTable ddt = table.Select(" auxiliar <> '0' and saldo <> '0'").CopyToDataTable();
                            bulkCopy.WriteToServer(ddt);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            bulkCopy.Close();
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                            bulkCopy.Close();
                            bulkCopy.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RegistroMensual"></param>
        public void GuardaSemanal(clsSICOFIN_Semanal.Root RegistroSemanal)
        {
            try
            {
                using (Oracle.DataAccess.Client.OracleConnection connection = new Oracle.DataAccess.Client.OracleConnection(strConexion_))
                {
                    connection.Open();
                    Oracle.DataAccess.Client.OracleTransaction trans = connection.BeginTransaction();
                    using (Oracle.DataAccess.Client.OracleBulkCopy bulkCopy = new Oracle.DataAccess.Client.OracleBulkCopy(connection))
                    {
                        DataTable table = new DataTable();
                        table = ConvertToDataTable(RegistroSemanal.data);
                        bulkCopy.DestinationTableName = "SCON_SALDOS_PCC";
                        //FBS. 19032022 para pruebas en desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            bulkCopy.DestinationTableName = "SCON_SALDOS_PCC_Z";
                        }
                        bulkCopy.BulkCopyTimeout = 60;

                        bulkCopy.ColumnMappings.Add(table.Columns["anio"].ToString(), "SCON_ANIO");
                        bulkCopy.ColumnMappings.Add(table.Columns["mes"].ToString(), "SCON_MES");
                        bulkCopy.ColumnMappings.Add(table.Columns["area"].ToString(), "SCON_AREA");
                        bulkCopy.ColumnMappings.Add(table.Columns["moneda"].ToString(), "SCON_MONEDA");
                        bulkCopy.ColumnMappings.Add(table.Columns["numeroMayor"].ToString(), "SCON_NUMERO_MAYOR");
                        bulkCopy.ColumnMappings.Add(table.Columns["cuenta"].ToString(), "SCON_CUENTA");
                        bulkCopy.ColumnMappings.Add(table.Columns["sector"].ToString(), "SCON_SECTOR");
                        bulkCopy.ColumnMappings.Add(table.Columns["auxiliar"].ToString(), "SCON_AUXILIAR");
                        bulkCopy.ColumnMappings.Add(table.Columns["fechaUltimoMovimiento"].ToString(), "SCON_FECHA_ULTIMO_MOVIMIENTO");
                        bulkCopy.ColumnMappings.Add(table.Columns["saldo"].ToString(), "SCON_SALDO");
                        bulkCopy.ColumnMappings.Add(table.Columns["monedaOrigen"].ToString(), "SCON_MONEDA_ORIGEN");
                        bulkCopy.ColumnMappings.Add(table.Columns["naturalezaCuenta"].ToString(), "SCON_NATURALEZA_CUENTA");
                        bulkCopy.ColumnMappings.Add(table.Columns["saldoPromedio"].ToString(), "SCON_SALDO_PROMEDIO");
                        bulkCopy.ColumnMappings.Add(table.Columns["montoDebito"].ToString(), "SCON_MONTO_DEBITO");
                        bulkCopy.ColumnMappings.Add(table.Columns["montoCredito"].ToString(), "SCON_MONTO_CREDITO");
                        bulkCopy.ColumnMappings.Add(table.Columns["saldoAnterior"].ToString(), "SCON_SALDO_ANTERIOR");
                        bulkCopy.ColumnMappings.Add(table.Columns["empresa"].ToString(), "SCON_EMPRESA");
                        bulkCopy.ColumnMappings.Add(table.Columns["calificaMoneda"].ToString(), "SCON_CALIFICA_MONEDA");
                        bulkCopy.ColumnMappings.Add(table.Columns["dia"].ToString(), "SCON_DIA");

                        for (int i = 0; i < table.Rows.Count; i++)

                            if ((table.Rows[i]["calificaMoneda"] == null || table.Rows[i]["calificaMoneda"] as string == "null"))
                                table.Rows[i]["calificaMoneda"] = null;
                        //else
                        //    table.Rows[i]["calificaMoneda"] = table.Rows[i]["calificaMoneda"];

                        try
                        {
                            DataTable ddt = table.Select(" auxiliar <> '0' and saldo <> '0'").CopyToDataTable();
                            bulkCopy.WriteToServer(ddt);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            bulkCopy.Close();
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                            bulkCopy.Close();
                            bulkCopy.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
    }
}
