using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class TipoAcreditadoDataAccess : CatalogoBase<TipoAcreditado, string>
    {

        private readonly Enums.Persona pers = Enums.Persona.Moral;
        public override string IdField { get { return "rfcAcreditado"; } }
        public override String ErrorGet { get { return "1003 - Error al cargar los datos de la base de datos en catálogo Tipo de Acreditado."; } }
        public override String ErrorInsert { get { return "1004 - Error al insertar nuevo registro en catálogo Tipo de Acreditado."; } }
        public override String ErrorUpdate { get { return "1005 - Error al actualizar registro en catálogo Tipo de Acreditado."; } }
        public override String ErrorDelete { get { return "1006 - Error al borrar el registro en catálogo Tipo de Acreditado."; } }

        public TipoAcreditadoDataAccess(Enums.Persona persona)
            : base(persona)
        {
            this.pers = persona;
        }

        public override TipoAcreditado GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int id = Parser.ToNumber(reader["ID"]);
            string rfcAcreditado = reader["RFC_ACREDITADO"].ToString().Trim();
            string NombreAcreditado = reader["NOMBRE_ACREDITADO"].ToString().Trim();
            Enums.Persona TipoAcreditado = Util.GetPersona(Parser.ToChar(reader["TIPO_ACREDITADO"]));
            Enums.Estado Estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            TipoAcreditado acr = new TipoAcreditado(id, rfcAcreditado, NombreAcreditado, TipoAcreditado, Estatus);
            activo = acr.Estatus == Enums.Estado.Activo;
            return acr;
        }

        public List<TipoAcreditado> GetTipoAcreditadoPorRFC(string rfcAcreditado, bool soloActivos)
        {
            List<TipoAcreditado> toReturn = new List<TipoAcreditado>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);
                DB.AddInParameter(cmd, "RFCAcreditado", DbType.AnsiString, rfcAcreditado);
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (pers == Enums.Persona.Moral ? 'M' : 'F'));

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        TipoAcreditado entity = GetEntity(reader, out activo);

                        //si se requieren todos O (si solo los activos y el registro actual es activo).. lo agregamos
                        if (!soloActivos || activo)
                        {
                            toReturn.Add(entity);
                        }
                    }
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener los Datos del Tipo de Acreditado por RFC", ex);
            }
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetTAcreditados"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKTIPOACREDITADO.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKTIPOACREDITADO.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKTIPOACREDITADO.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, TipoAcreditado entityOld, TipoAcreditado entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityNew.RfcAcreditado);
            DB.AddInParameter(cmd, "pNOMBRE_ACREDITADO", DbType.AnsiString, entityNew.NombreAcreditado);            
            DB.AddInParameter(cmd, "pTIPO_ACREDITADO", DbType.AnsiString, Util.SetPersona(entityNew.Tipo_Acreditado.ToString()));            
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, TipoAcreditado entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityToDelete.RfcAcreditado);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, TipoAcreditado entityToInsert)
        {
            //parametros para insertar
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityToInsert.RfcAcreditado);
            DB.AddInParameter(cmd, "pNOMBRE_ACREDITADO", DbType.AnsiString, entityToInsert.NombreAcreditado);            
            DB.AddInParameter(cmd, "pTIPO_ACREDITADO", DbType.AnsiString, Util.SetPersona(entityToInsert.Tipo_Acreditado.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
