using System;
using System.Data;
using System.Data.Common;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class ValidacionesDataAccess : CatalogoBase<Validacion, string>
    {

        public override string IdField { get { return "idvalidacion"; } }
        public override String ErrorGet { get { return " 1056 - Error al cargar los datos de la base de datos en catálogo Avisos de Rechazo."; } }
        public override String ErrorInsert { get { return "0000 -No Aplica."; } }
        public override String ErrorUpdate { get { return "1057 - Error al actualizar registro en catálogo Avisos de Rechazo."; } }
        public override String ErrorDelete { get { return "0000 - No Aplica."; } }

        public ValidacionesDataAccess(Enums.Persona persona)
            : base(persona)
        {

        }

        public override Validacion GetEntity(IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["ID"]);
            Enums.Rechazo rechazo = (reader["TIPO"].ToString().ToUpper().Trim() == "ERROR") ? Enums.Rechazo.Error : Enums.Rechazo.Warning;
            bool aplicable = Parser.ToNumber(reader["Aplicable"]) == 1;
            Enums.Persona persona = Util.GetPersona(Parser.ToChar(reader["PERSONA"]));
            string mensaje = reader["MENSAJE"].ToString();
            Enums.Estado estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            string codigo = reader["CODIGO_ERROR"].ToString();
            int etiquetaId = Parser.ToNumber(reader["ID_ETIQUETA"]);
            string etiqueta = reader["ETIQUETA"].ToString();
            string campo = reader["CAMPO"].ToString();

            Validacion val = new Validacion(id, rechazo, aplicable, persona, mensaje, estatus, codigo, etiquetaId,etiqueta,campo);
            activo = (val.Estatus == Enums.Estado.Activo);

            return val;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetValidaciones"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKAVISORECHAZO.SpUpdate"; }
        }

        public override void SetEntity(DbCommand cmd, Validacion entityOld, Validacion entityNew)
        {
            string OldAplicConvert = string.Empty;
            string NewAplicConvert = string.Empty;
            if (entityOld.Aplicable)
                OldAplicConvert = "1";
            else
                OldAplicConvert = "0";

            if (entityNew.Aplicable)
                NewAplicConvert = "1";
            else
                NewAplicConvert = "0";
            
            //Parametros Actuales (Viejos)
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            
            //Parametros Nuevos(Actualizacion)
            DB.AddInParameter(cmd, "newAPLICABLE", DbType.AnsiString, NewAplicConvert);
            DB.AddInParameter(cmd, "newMENSAJE", DbType.AnsiString, entityNew.Mensaje);
            DB.AddInParameter(cmd, "newTIPO", DbType.AnsiString, Convert.ToString(entityNew.Tipo));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }
    }

}
