using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Oracle.DataAccess.Client;
using Banobras.Credito.SICREB.Entities.GL;

namespace Banobras.Credito.SICREB.Datos.GL
{

    public class Mapper_GL_Segmento_EM
    {

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor de la clase de acceso a datos.
        /// </summary>
        public Mapper_GL_Segmento_EM()
        {
            //Instanciar SqlMapper
        }

        #endregion

        #region PUBLIC METHODS

        public List<Segmento_EM_Gpos_Lcryc> Get_Segmento_EM_Gpos_Lcryc(IDbConnection connection, IDbTransaction transaction = null)
        {
            try
            {
                List<Segmento_EM_Gpos_Lcryc> Segmento_EM = new List<Segmento_EM_Gpos_Lcryc>();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("cur_OUT", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                Segmento_EM = Dapper.SqlMapper.Query<Segmento_EM_Gpos_Lcryc>(connection,
                                                                             "PKG_GPOS_LCCYR.SP_GET_GPOS_LCCYR_SEG_EM_HD",
                                                                             parametros,
                                                                             commandType: CommandType.StoredProcedure).ToList();
                return Segmento_EM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    
    }

}
