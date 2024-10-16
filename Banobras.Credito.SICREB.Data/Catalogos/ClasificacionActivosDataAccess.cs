using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class ClasificacionActivosDataAccess : CatalogoBase<ClasificacionActivos, int>
    {

        //MASS 27/06/13 se integra Clasificacion Activos en los mensajes
        public override string IdField { get { return "IdCuenta"; } }
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Clasificacion Activos."; } }
        public override String ErrorInsert { get { return "1040 - Error al insertar nuevo registro en catálogo Clasificacion Activos."; } }
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Clasificacion Activos."; } }
        public override String ErrorDelete { get { return "1042 - Error al borrar el registro en catálogo Clasificacion Activos."; } }

        public ClasificacionActivosDataAccess()
            : base()
        {

        }

        public override ClasificacionActivos GetEntity(IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["id"]);
            string descripcion = reader["descripcion"].ToString();
            string vigente = reader["vigente"].ToString();
            
            //MASS 27/06/13            
            ClasificacionActivos cuenta = new ClasificacionActivos(id, descripcion, vigente);            
            activo = true;
            return cuenta;
        }

        public override string StoredProcedure
        {           
            get { return "PACKCLASIFICACION_ACTIVOS.SP_CATALOGOS_Get"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKCLASIFICACION_ACTIVOS.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCLASIFICACION_ACTIVOS.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCLASIFICACION_ACTIVOS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, ClasificacionActivos entityOld, ClasificacionActivos entityNew)
        {            
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityOld.id);
            DB.AddInParameter(cmd, "pdescripcion", DbType.AnsiString, entityNew.descripcion.ToString());            
            DB.AddInParameter(cmd, "pvigente", DbType.AnsiString, entityNew.vigente.ToString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, ClasificacionActivos entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityToDelete.id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, ClasificacionActivos entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pdescripcion", DbType.AnsiString, entityToInsert.descripcion.ToString());            
            DB.AddInParameter(cmd, "pvigente", DbType.AnsiString, entityToInsert.vigente.ToString());            
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        } 
       
    }

}
