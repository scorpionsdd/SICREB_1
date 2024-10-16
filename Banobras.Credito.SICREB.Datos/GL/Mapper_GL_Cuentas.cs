using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

using SHCP.SIGAIF.Datos;
using Oracle.DataAccess.Client;
using Banobras.Credito.SICREB.Entities.GL;

namespace Banobras.Credito.SICREB.Datos.GL
{

    public class Mapper_GL_Cuentas
    {

        private String ConnectionString { get; set; }

        /// <summary>
        /// Constructor de la clase de acceso a datos.
        /// </summary>
        public Mapper_GL_Cuentas()
        {
            //Instanciar SqlMapper
        }

        public List<GLCuentas> GL_ObtenerCuentas()
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {                
                _connection.Open();
                List<GLCuentas> lCuentas = new List<GLCuentas>();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("cur_OUT", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lCuentas = Dapper.SqlMapper.Query<GLCuentas>(_connection, "PKG_GL_CATALOGOS.SP_GL_GET_T_CUENTAS", parametros, commandType: CommandType.StoredProcedure).ToList();
                return lCuentas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                _connection.Close();
            }  
          
        }

        public void GL_InsertarCuenta(GLCuentas CuentaNew)
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
                parametros.Add("pCODIGO", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CuentaNew.CODIGO);
                parametros.Add("pDESCRIPCION", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CuentaNew.DESCRIPCION);
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_INS_T_CUENTAS", parametros, commandType: CommandType.StoredProcedure);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }

        }

        public void GL_ActualizarCuenta(GLCuentas CuentaNew)
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("pId", dbType: OracleDbType.Int64, direction: ParameterDirection.Input, value: CuentaNew.Cuenta_Id);
                parametros.Add("pCODIGO", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CuentaNew.CODIGO);
                parametros.Add("pDESCRIPCION", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CuentaNew.DESCRIPCION);
                parametros.Add("pEstatus", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: CuentaNew.ESTATUS);
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_UPD_T_CUENTAS", parametros, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }

        }

        public void GL_BorrarCuenta(GLCuentas CuentaDel)
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("pId", dbType: OracleDbType.Int64, direction: ParameterDirection.Input, value: CuentaDel.Cuenta_Id);                
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_DEL_T_CUENTAS", parametros, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }

        }

        public void GL_BorrarALLCuentas()
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();                                
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_DEL_T_CUENTAS_ALL", commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }  
          
        }
    
    }

}
