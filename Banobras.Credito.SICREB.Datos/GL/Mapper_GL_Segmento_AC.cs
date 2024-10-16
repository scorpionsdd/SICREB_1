using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Oracle.DataAccess.Client;
using Banobras.Credito.SICREB.Entities.GL;

namespace Banobras.Credito.SICREB.Datos.GL
{

    public class Mapper_GL_Segmento_AC
    {

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor de la clase de acceso a datos.
        /// </summary>
        public Mapper_GL_Segmento_AC()
        {
            //Instanciar SqlMapper
        }

        #endregion

        #region PUBLIC METHODS

        public Segmento_HD_Gpos_Lcryc Get_Segmento_HD_Gpos_Lcryc(IDbConnection connection, IDbTransaction transaction = null)
        {
            try
            {
                Segmento_HD_Gpos_Lcryc Segmento_HD = new Segmento_HD_Gpos_Lcryc();
                var parametros = new Dapper.OracleDynamicParameters();
                parametros.Add("p_CURSOR", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
                Segmento_HD = Dapper.SqlMapper.Query<Segmento_HD_Gpos_Lcryc>(connection,
                                                                             "PKG_GPOS_LCCYR.SP_GET_GPOS_LCCYR_SEG_PM_HD",
                                                                             commandType: CommandType.StoredProcedure).ElementAt(0);
                return Segmento_HD;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    
    }

}
