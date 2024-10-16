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

    public class AlertasDataAccess : CatalogoBase<Alerta, String>
    {

        public override string IdField { get { return "IdAlertas"; } }
        public override String ErrorGet { get { return " 1080 - Error al cargar los datos de la base de datos en catálogo Alertas."; } }
        public override String ErrorInsert { get { return "1081 - Error al insertar nuevo registro en catálogo Alertas."; } }
        public override String ErrorUpdate { get { return "1082 - Error al actualizar registro en catálogo Alertas."; } }
        public override String ErrorDelete { get { return "1083 - Error al borrar el registro en catálogo Alertas."; } }

        public override Alerta GetEntity(IDataReader reader, out bool activo)
        {

            int idAlerta = Parser.ToNumber(reader["ID"]);
            string identificadorAlerta = reader["IDENTIFICADORALERTA"].ToString();
            string tituloAlrta = reader["TITULOALERTA"].ToString();
            string mensajeAlerta = reader["MENSAJEALERTA"].ToString();
            string periodicidad = reader["PERIODICIDAD"].ToString();
            string aplicacionPeriodo = reader["FECHAAPLICACIONPERIODO"].ToString();
            string estadoAlarma = reader["ACTIVADA"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            Alerta alerta = new Alerta(idAlerta, identificadorAlerta, tituloAlrta, mensajeAlerta, periodicidad, aplicacionPeriodo, estadoAlarma);
            activo = true;

            return alerta;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetAlertas"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "CATALOGOS.F_CATALOGOS_UpdAlerta"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "CATALOGOS.F_CATALOGOS_DelAlerta"; }            
        }

        public override string StoredProcedureInsert
        {
            get { return "CATALOGOS.F_CATALOGOS_InsAlerta"; }
        }

        public override void SetEntity(DbCommand cmd, Alerta entityOld, Alerta entityNew)
        {
            DB.AddInParameter(cmd, "idalerta", DbType.String, entityNew.Id);
            DB.AddInParameter(cmd, "pidentificadoralerta", DbType.String, entityNew.IdententifadorAlerta);
            DB.AddInParameter(cmd, "ptituloalerta", DbType.String, entityNew.TituloAlerta);
            DB.AddInParameter(cmd, "pmensajealerta", DbType.String, entityNew.Mensaje);
            DB.AddInParameter(cmd, "pperiodicidad", DbType.String, entityNew.Periodicidad);
            DB.AddInParameter(cmd, "pfechaaplicacionperiodo", DbType.String, entityNew.AplicacionDePeriodicidad);
            DB.AddInParameter(cmd, "pactivada", DbType.String, entityNew.AlarmaActivada);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Alerta entityToDelete)
        {
            DB.AddInParameter(cmd, "idalerta", DbType.AnsiString, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Alerta entityToInsert)
        {
            DB.AddInParameter(cmd, "idalerta", DbType.String, entityToInsert.Id);
            DB.AddInParameter(cmd, "pidentificadoralerta", DbType.String, entityToInsert.IdententifadorAlerta);
            DB.AddInParameter(cmd, "ptituloalerta", DbType.String,entityToInsert.TituloAlerta );
            DB.AddInParameter(cmd, "pmensajealerta", DbType.String, entityToInsert.Mensaje);
            DB.AddInParameter(cmd, "pperiodicidad", DbType.String,entityToInsert.Periodicidad);
            DB.AddInParameter(cmd, "pfechaaplicacionperiodo", DbType.String, entityToInsert.AplicacionDePeriodicidad);
            DB.AddInParameter(cmd, "pactivada", DbType.String, entityToInsert.AlarmaActivada);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
