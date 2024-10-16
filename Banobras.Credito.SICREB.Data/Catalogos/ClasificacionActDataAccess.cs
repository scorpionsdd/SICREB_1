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

    public class ClasificacionActDataAccess : CatalogoBase<ClasificacionAct, int>
    {

        //MASS 24/06/13 se integra Clasificacion Act en los mensajes
        public override string IdField { get { return "IdCuenta"; } }
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Clasificacion Act."; } }
        public override String ErrorInsert { get { return "1040 - Error al insertar nuevo registro en catálogo Clasificacion Act."; } }
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Clasificacion Act."; } }
        public override String ErrorDelete { get { return "1042 - Error al borrar el registro en catálogo Clasificacion Act."; } }

        public ClasificacionActDataAccess()
            : base()
        {

        }

        public override ClasificacionAct GetEntity(IDataReader reader, out bool activo)
        {

            int id = Parser.ToNumber(reader["id"]);
            string descripcion = reader["descripcion"].ToString();
            string vigente = reader["vigente"].ToString();
            //Enums.Estado vigente = Util.GetEstado(Parser.ToChar(reader["vigente"]));           
            //MASS 24/06/13            
            ClasificacionAct cuenta = new ClasificacionAct(id, descripcion, vigente);
            //activo = (cuenta.vigente == Enums.Estado.Activo);
            activo = true;
            return cuenta;
        }

        public override string StoredProcedure
        {           
            get { return "PACKCLASIFICACION_ACT.SP_CATALOGOS_Get"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKCLASIFICACION_ACT.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCLASIFICACION_ACT.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCLASIFICACION_ACT.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, ClasificacionAct entityOld, ClasificacionAct entityNew)
        {            
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityOld.id);
            DB.AddInParameter(cmd, "pdescripcion", DbType.AnsiString, entityNew.descripcion.ToString());            
            DB.AddInParameter(cmd, "pvigente", DbType.AnsiString, entityNew.vigente.ToString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, ClasificacionAct entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityToDelete.id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, ClasificacionAct entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pdescripcion", DbType.AnsiString, entityToInsert.descripcion.ToString());
            DB.AddInParameter(cmd, "pvigente", DbType.AnsiString, entityToInsert.vigente.ToString());            
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        } 
       
    }

}
