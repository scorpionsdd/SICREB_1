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

    public class ConvertidorActivosDataAccess : CatalogoBase<ConvertidorActivos, int>
    {

        //MASS 21/06/13 se integra Convertidor Activos en los mensajes
        public override string IdField { get { return "IdCuenta"; } }
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Convertidor Activos."; } }
        public override String ErrorInsert { get { return "1040 - Error al insertar nuevo registro en catálogo Convertidor Activos."; } }
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Convertidor Activos."; } }
        public override String ErrorDelete { get { return "1042 - Error al borrar el registro en catálogo Convertidor Activos."; } }

        public ConvertidorActivosDataAccess()
            : base()
        {

        }

        public override ConvertidorActivos GetEntity(IDataReader reader, out bool activo)
        {

            int id_convertidor = Parser.ToNumber(reader["id_convertidor"]);
            string auxiliar = reader["auxiliar"].ToString();
            string credito = reader["credito"].ToString();
            string cuenta_ant_vigente = reader["cuenta_ant_vigente"].ToString();            
            string cuenta_ant_vencido = reader["cuenta_ant_vencido"].ToString();
            string cuenta_ant_moratorios = reader["cuenta_ant_moratorios"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["estatus"]));
            
            //MASS 21/06/13            
            ConvertidorActivos cuenta = new ConvertidorActivos(id_convertidor, auxiliar, credito, cuenta_ant_vigente, cuenta_ant_vencido, cuenta_ant_moratorios, estado);
            activo = (cuenta.estatus == Enums.Estado.Activo);

            return cuenta;
        }

        public override string StoredProcedure
        {          
            get { return "PACKCONVERTIDOR_ACTIVOS.SP_CATALOGOS_GetConvertActivos"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKCONVERTIDOR_ACTIVOS.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCONVERTIDOR_ACTIVOS.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCONVERTIDOR_ACTIVOS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, ConvertidorActivos entityOld, ConvertidorActivos entityNew)
        {            
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityOld.id_convertidor);
            DB.AddInParameter(cmd, "pauxiliar", DbType.AnsiString, entityNew.auxiliar.ToString());
            DB.AddInParameter(cmd, "pcredito", DbType.AnsiString, entityNew.credito.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_vigente", DbType.AnsiString, entityNew.cuenta_ant_vigente.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_vencido", DbType.AnsiString, entityNew.cuenta_ant_vencido.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_moratorios", DbType.AnsiString, entityNew.cuenta_ant_moratorios.ToString());            
            DB.AddInParameter(cmd, "pestatus", DbType.AnsiString, Util.SetEstado(entityNew.estatus.ToString()));            
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, ConvertidorActivos entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityToDelete.id_convertidor);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, ConvertidorActivos entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pauxiliar", DbType.AnsiString, entityToInsert.auxiliar.ToString());
            DB.AddInParameter(cmd, "pcredito", DbType.AnsiString, entityToInsert.credito.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_vigente", DbType.AnsiString, entityToInsert.cuenta_ant_vigente.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_vencido", DbType.AnsiString, entityToInsert.cuenta_ant_vencido.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_moratorios", DbType.AnsiString, entityToInsert.cuenta_ant_moratorios.ToString());            
            DB.AddInParameter(cmd, "pestatus", DbType.AnsiString, Util.SetEstado(entityToInsert.estatus.ToString()));            
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }
        
    }

}
