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

    public class PersonasFallecidasDataAccess : CatalogoBase<PersonasFallecidas, int>
    {

        public override string IdField{ get { return "idrfc"; } }
        public override String ErrorGet { get { return " 1064 - Error al cargar los datos de la base de datos en catálogo PersonasFallecidas."; } }
        public override String ErrorInsert { get { return "1065 - Error al insertar nuevo registro en catálogo PersonasFallecidas."; } }
        public override String ErrorUpdate { get { return "1066 - Error al actualizar registro en catálogo PersonasFallecidas."; } }
        public override String ErrorDelete { get { return "1067 - Error al borrar el registro en catálogo PersonasFallecidas."; } }

        public PersonasFallecidasDataAccess()
        {

        }

        public override PersonasFallecidas GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string rfc = reader["RFC"].ToString();
            //SICREB-INICIO ACA Sep-2012
            string nombre = reader["NOMBRE"].ToString();
            string credito = reader["CREDITO"].ToString();
            string auxiliar = reader["AUXILIAR"].ToString();
            //SICREB-FIN ACA Sep-2012
            DateTime fecha = Parser.ToDateTime(reader["FECHA"]);
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));
            activo = true;
            //SICREB-INICIO ACA Sep-2012  Se anexo nombre, credito, auxiliar
            PersonasFallecidas rfcFallecidos = new PersonasFallecidas(idValue, rfc, nombre, credito, auxiliar, fecha,estado);
            //SICREB-INICIO ACA Sep-2012

            return rfcFallecidos;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetRfcFallecidos"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKRFCFALLECIDOS.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKRFCFALLECIDOS.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKRFCFALLECIDOS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, PersonasFallecidas entityOld, PersonasFallecidas entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityNew.Rfc);
            //SICREB-INICIO ACA Sep-2012
            DB.AddInParameter(cmd, "pNOMBRE", DbType.AnsiString, entityNew.Nombre);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "pAUXILIAR", DbType.AnsiString, entityNew.Auxiliar);
            //SICREB-FIN ACA Sep-2012
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddInParameter(cmd, "pFECHA", DbType.DateTime, entityNew.Fecha);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, PersonasFallecidas entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, PersonasFallecidas entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityToInsert.Rfc);
            DB.AddInParameter(cmd, "pFECHA", DbType.DateTime, entityToInsert.Fecha);
            //SICREB-INICIO ACA Sep-2012
            DB.AddInParameter(cmd, "pNOMBRE", DbType.AnsiString, entityToInsert.Nombre);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pAUXILIAR", DbType.AnsiString, entityToInsert.Auxiliar);
            //SICREB-INICIO ACA Sep-2012
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}