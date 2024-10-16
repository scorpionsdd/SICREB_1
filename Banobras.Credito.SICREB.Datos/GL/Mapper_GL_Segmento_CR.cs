using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using Oracle.DataAccess.Client;
using Banobras.Credito.SICREB.Entities.GL;

namespace Banobras.Credito.SICREB.Datos.GL
{

    public class Mapper_GL_Segmento_CR
    {

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor de la clase de acceso a datos.
        /// </summary>
        public Mapper_GL_Segmento_CR()
        {
            //Instanciar SqlMapper
        }

        #endregion

        #region PUBLIC METHODS

        public List<Segmento_CR_Gpos_Lcryc> Get_Segmento_CR_Gpos_Lcryc(IDbConnection connection, IDbTransaction transaction = null)
        {
            try
            {
                List<Segmento_CR_Gpos_Lcryc> Segmento_CR = new List<Segmento_CR_Gpos_Lcryc>();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("cur_OUT", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                Segmento_CR = Dapper.SqlMapper.Query<Segmento_CR_Gpos_Lcryc>(connection,
                                                                             "PKG_GPOS_LCCYR.SP_GET_GPOS_LCCYR_SEG_PM_HD",
                                                                             parametros,
                                                                             commandType: CommandType.StoredProcedure).ToList();
                return Segmento_CR;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: "+ ex.Message);
                return null;
            }
        }

        #endregion

    }

}
