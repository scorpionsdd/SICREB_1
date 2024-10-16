using System;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using SHCP.SIGAIF.Datos;
using Oracle.DataAccess.Client;
using Banobras.Credito.SICREB.Entities.GL;

namespace Banobras.Credito.SICREB.Datos.GL
{

    public class Mapper_GL_CreditosExceptuados
    {

        private String ConnectionString { get; set; }

        /// <summary>
        /// Constructor de la clase de acceso a datos.
        /// </summary>
        public Mapper_GL_CreditosExceptuados()
        {
            //Instanciar SqlMapper
        }

        public List<GLCreditosExceptuados> GL_ObtenerCreditosExeptuados()
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {                
                _connection.Open();
                List<GLCreditosExceptuados> lCreditos = new List<GLCreditosExceptuados>();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("cur_OUT", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lCreditos = Dapper.SqlMapper.Query<GLCreditosExceptuados>(_connection, "PKG_GL_CATALOGOS.SP_GL_GET_T_EXCEPTUADOS", parametros, commandType: CommandType.StoredProcedure).ToList();
                return lCreditos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
                finally 
            {
                _connection.Close();
            }
            
        }

        public void GL_InsertarCreditoExceptuado(GLCreditosExceptuados CreditoNew)
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();                
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("pID", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("pTIPO", dbType: OracleDbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("pCredito", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CreditoNew.CREDITO);
                parametros.Add("pMotivo", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CreditoNew.MOTIVO);
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_INS_T_EXCEPTUADOS", parametros, commandType: CommandType.StoredProcedure);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

        }

        public void GL_ActualizarCreditoExceptuado(GLCreditosExceptuados CreditoNew)
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("pId", dbType: OracleDbType.Int64, direction: ParameterDirection.Input, value: CreditoNew.Id);
                parametros.Add("pCredito", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CreditoNew.CREDITO);
                parametros.Add("pMotivo", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CreditoNew.MOTIVO);
                parametros.Add("pEstatus", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CreditoNew.ESTATUS);
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_UPD_T_EXCEPTUADOS", parametros, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

        }

        public void GL_BorrarCreditoExceptuado(GLCreditosExceptuados CreditoDel)
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("pId", dbType: OracleDbType.Int64, direction: ParameterDirection.Input, value: CreditoDel.Id);                
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_DEL_T_EXCEPTUADOS", parametros, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

        }

        public void GL_BorrarALLCreditosExceptuados()
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();                                
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_DEL_T_EXCEPTUADOS_ALL", commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }            
        }
    
    }

}
