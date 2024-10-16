

using System.Data;
namespace Banobras.Credito.SICREB.Data.Transaccionales
{
    public class cls_conexionOracle
    {
        public cls_conexionOracle()
        {

        }//cls_conexionMysql

        public OracleConnection getConnectionOracle()
        {
            DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_AddConciliacion");

            try
            {

                Cnn.ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ToString();
                return Cnn;
            }
            catch
            {
                return Cnn = null;
            }

        }//


        protected DataTable ExecuteData(OracleConnection Conexion, string str_sql)
        {
            DataTable dt = new DataTable();
            OracleCommand Cmd = new OracleCommand();

            try
            {
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandTimeout = 120;
                Cmd.Connection = Conexion;
                Cmd.CommandText = str_sql;

                Cmd.Connection.Open();
                Cmd.ExecuteNonQuery();

                OracleDataAdapter da = new OracleDataAdapter(Cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex_)
            {


                return dt = new DataTable();

            }//try-catch
            finally
            {
                Conexion.Close();

            }//finally
        }//ExecuteDataTable


        protected DataTable ExecuteStore(OracleConnection Conexion, string str_sp, OracleParameter[] parametros)
        {
            DataTable dt = new DataTable();
            OracleCommand Cmd = new OracleCommand();

            try
            {
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandTimeout = 120;
                Cmd.Connection = Conexion;
                Cmd.CommandText = str_sp;

                for (int i = 0; i < parametros.Length; i++)
                {
                    Cmd.Parameters.Add(parametros[i]);
                }

                Cmd.Connection.Open();
                Cmd.ExecuteNonQuery();

                OracleDataAdapter da = new OracleDataAdapter(Cmd);
                da.Fill(dt);

                Conexion.Close();
                return dt;

            }
            catch (Exception ex_)
            {
                Conexion.Close();

                return dt = new DataTable();

            }//try-catch
            finally
            {
                Conexion.Close();

            }//finally
        }//ExecuteDataTable retorna DataTable


        protected void ExecuteStoreCommand(OracleConnection Conexion, string str_sp, OracleParameter[] parametros)
        {

            OracleCommand Cmd = new OracleCommand();

            try
            {
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandTimeout = 120;
                Cmd.Connection = Conexion;
                Cmd.CommandText = str_sp;

                for (int i = 0; i < parametros.Length; i++)
                {
                    Cmd.Parameters.Add(parametros[i]);
                }

                Cmd.Connection.Open();
                Cmd.ExecuteNonQuery();



            }
            catch (Exception ex_)
            {
                Conexion.Close();



            }//try-catch
            finally
            {
                Conexion.Close();

            }//finally
        }//Execute retorna numero




    }

    
}
