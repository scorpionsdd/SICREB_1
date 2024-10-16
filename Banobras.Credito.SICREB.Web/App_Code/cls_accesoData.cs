using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;


namespace Banobras.Credito.SICREB.Data
{
    public class cls_accesoData : OracleBase
    {
        




        public void fn_getResultadoSTORE_Command(string str_stored, DbParameter[] parametros)
        {
            DbCommand cmd = null;
            try
            {

                cmd = DB.GetStoredProcCommand(str_stored);
              
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                cmd.Connection = OracleBase.DB.CreateConnection();
                //cmd.CommandText = str_stored;

                for (int i = 0; i < parametros.Length; i++)
                {
                    cmd.Parameters.Add(parametros[i]);
                }
                cmd.Connection.Open();
                int r= cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }//try
            catch (Exception ex_)
            {
                ex_.ToString();
                cmd.Connection.Close();

            }//catch
            finally{
                DB.CreateConnection().Close();
            }

        }//fn_getResultado_DataTable

        public void fn_getResultadoText_Command(string str_stored, DbParameter[] parametros)
        {
            try
            {

                DbCommand cmd = DB.GetStoredProcCommand(str_stored);

                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Connection = OracleBase.DB.CreateConnection();



                for (int i = 0; i < parametros.Length; i++)
                {
                    cmd.Parameters.Add(parametros[i]);
                }

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();


            }//try
            catch (Exception ex_)
            {
                ex_.ToString();

            }//catch
            finally
            {
                DB.CreateConnection().Close();
            }

        }//fn_getResultado_DataTable

        public void excecutecmd(string str_stored, DbParameter[] parametros)
        {
            try
            {

                DbCommand cmd = DB.GetSqlStringCommand(str_stored);

                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.Connection = OracleBase.DB.CreateConnection();



                for (int i = 0; i < parametros.Length; i++)
                {
                    cmd.Parameters.Add(parametros[i]);
                }

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }//try
            catch (Exception ex_)
            {
                ex_.ToString();

            }//catch
            finally
            {
                DB.CreateConnection().Close();
            }

        }//fn_getResultado_DataTable

        public DataTable cmdtodt(string str_stored, DbParameter[] parametros)
        {
            DataTable ds = new DataTable();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(str_stored);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                cmd.Connection = OracleBase.DB.CreateConnection();
               
                for (int i = 0; i < parametros.Length; i++)
                {

                    cmd.Parameters.Add(parametros[i]);
                    

                }
                cmd.Connection.Open();
                DbDataAdapter da = OracleBase.DB.GetDataAdapter();
              //  da.SelectCommand = cmd;
              // cmd.ExecuteReader();
               using (IDataReader reader = DB.ExecuteReader(cmd))
               {
                   ds.Load(reader);
               }

                //da.Fill(ds);
              //  cmd.ExecuteNonQuery();
              //  cmd.Connection.Close();
            }//try
            catch (Exception ex_)
            {
                ex_.ToString();

            }//catch
            finally
            {
                DB.CreateConnection().Close();
            }
            return ds;
        }//fn_getResultado_DataTable

    }
}
