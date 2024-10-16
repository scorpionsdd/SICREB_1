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

    public class ConvertidorActDataAccess : CatalogoBase<ConvertidorAct, int>
    {

        //MASS 19/06/13 se integra Convertidor Act en los mensajes
        public override string IdField { get { return "IdCuenta"; } }
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Convertidor Act."; } }
        public override String ErrorInsert { get { return "1040 - Error al insertar nuevo registro en catálogo Convertidor Act."; } }
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Convertidor Act."; } }
        public override String ErrorDelete { get { return "1042 - Error al borrar el registro en catálogo Convertidor Act."; } }

        public ConvertidorActDataAccess()
            : base()
        {

        }

        public override ConvertidorAct GetEntity(IDataReader reader, out bool activo)
        {

            int id_convertidor = Parser.ToNumber(reader["id_convertidor"]);
            string cuenta_act = reader["cuenta_act"].ToString();
            string cuenta_ant_vigente = reader["cuenta_ant_vigente"].ToString();            
            string cuenta_ant_vencido = reader["cuenta_ant_vencido"].ToString();
            string cuenta_ant_moratorios = reader["cuenta_ant_moratorios"].ToString();            
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["estatus"]));
            string cuenta_capital = reader["cuenta_capital"].ToString();
            
            //MASS 19/06/13
            //PSL se agrego cuenta_nueva 07 12 2021
            ConvertidorAct cuenta = new ConvertidorAct(id_convertidor, cuenta_act, cuenta_ant_vigente, cuenta_ant_vencido, cuenta_ant_moratorios, estado, cuenta_capital);// , cuenta_nueva
            activo = (cuenta.estatus == Enums.Estado.Activo);
            return cuenta;
        }

        public override string StoredProcedure
        {           
            get { return "PACKCONVERTIDOR_ACT.SP_CATALOGOS_GetConvertidorAct"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKCONVERTIDOR_ACT.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCONVERTIDOR_ACT.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCONVERTIDOR_ACT.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, ConvertidorAct entityOld, ConvertidorAct entityNew)
        {            
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityOld.id_convertidor);
            DB.AddInParameter(cmd, "pcuenta_act", DbType.AnsiString, entityNew.cuenta_act.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_vigente", DbType.AnsiString, entityNew.cuenta_ant_vigente.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_vencido", DbType.AnsiString, entityNew.cuenta_ant_vencido.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_moratorios", DbType.AnsiString, entityNew.cuenta_ant_moratorios.ToString());            
            DB.AddInParameter(cmd, "pestatus", DbType.AnsiString, Util.SetEstado(entityNew.estatus.ToString()));
            DB.AddInParameter(cmd, "pcuenta_capital", DbType.AnsiString, entityNew.cuenta_capital.ToString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, ConvertidorAct entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityToDelete.id_convertidor);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, ConvertidorAct entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pcuenta_act", DbType.AnsiString, entityToInsert.cuenta_act.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_vigente", DbType.AnsiString, entityToInsert.cuenta_ant_vigente.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_vencido", DbType.AnsiString, entityToInsert.cuenta_ant_vencido.ToString());
            DB.AddInParameter(cmd, "pcuenta_ant_moratorios", DbType.AnsiString, entityToInsert.cuenta_ant_moratorios.ToString());            
            DB.AddInParameter(cmd, "pestatus", DbType.AnsiString, Util.SetEstado(entityToInsert.estatus.ToString()));
            DB.AddInParameter(cmd, "pcuenta_capital", DbType.AnsiString, entityToInsert.cuenta_capital.ToString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        } 
       
    }

}
