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

    public class Mapper_GL_ConvertidorCuentas
    {

        private String ConnectionString { get; set; }

        /// <summary>
        /// Constructor de la clase de acceso a datos.
        /// </summary>
        public Mapper_GL_ConvertidorCuentas()
        {
            //Instanciar SqlMapper
        }

        public List<GLConvertidorCuentas> GL_ObtenerConvertidorCuentas()
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {                
                _connection.Open();
                List<GLConvertidorCuentas> lCreditos = new List<GLConvertidorCuentas>();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("cur_OUT", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                lCreditos = Dapper.SqlMapper.Query<GLConvertidorCuentas>(_connection, "PKG_GL_CATALOGOS.SP_GL_GET_T_CONVERTIDOR", parametros, commandType: CommandType.StoredProcedure).ToList();
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

        public void GL_InsertarConvertidorCuentas(GLConvertidorCuentas ConvertidorNew)
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
                parametros.Add("pCuentaActual", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: ConvertidorNew.CUENTA_ACT);
                parametros.Add("pCuentaAnterior", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: ConvertidorNew.CUENTA_ANT);
                parametros.Add("pEstatus", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: ConvertidorNew.ESTATUS);
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_INS_T_CONVERTIDOR", parametros, commandType: CommandType.StoredProcedure);                
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

        public void GL_ActualizarConvertidorCuentas(GLConvertidorCuentas ConvertidorNew)
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("pIdConvertidor", dbType: OracleDbType.Int64, direction: ParameterDirection.Input, value: ConvertidorNew.ID_CONVERTIDOR);
                parametros.Add("pCuentaActual", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: ConvertidorNew.CUENTA_ACT);
                parametros.Add("pCuentaAnterior", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: ConvertidorNew.CUENTA_ANT);
                parametros.Add("pEstatus", dbType: OracleDbType.Varchar2, direction: ParameterDirection.Input, value: ConvertidorNew.ESTATUS);
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_UPD_T_CONVERTIDOR", parametros, commandType: CommandType.StoredProcedure);
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

        public void GL_BorrarConvertidorCuentas(GLConvertidorCuentas CuentaDel)
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("pIdConvertidor", dbType: OracleDbType.Int64, direction: ParameterDirection.Input, value: CuentaDel.ID_CONVERTIDOR);                
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_DEL_T_CONVERTIDOR", parametros, commandType: CommandType.StoredProcedure);
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

        public void GL_BorrarALLConvertidorCuentas()
        {
            IDbConnection _connection;
            ConnectionString = ConfigurationManager.ConnectionStrings["SICREB"].ConnectionString;
            _connection = DbUtil.GetConnection(ConnectionString);

            try
            {
                _connection.Open();                                
                Dapper.SqlMapper.Execute(_connection, "PKG_GL_CATALOGOS.SP_GL_DEL_ALL_T_CONVERTIDOR", commandType: CommandType.StoredProcedure);
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
