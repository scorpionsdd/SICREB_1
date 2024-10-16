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

    public class EstadoCivilDataAccess : CatalogoBase<EstadoCivil, int>
    {

        public override string IdField{ get { return "ID"; } }
        public override String ErrorGet { get { return " 1043 - Error al cargar los datos de la base de datos en catálogo Estado Civil."; } }
        public override String ErrorInsert { get { return "1044 - Error al insertar nuevo registro en catálogo Estado Civil."; } }
        public override String ErrorUpdate { get { return "1045 - Error al actualizar registro en catálogo Estado Civil."; } }
        public override String ErrorDelete { get { return "1046 - Error al borrar el registro en catálogo Estado Civil."; } }

        public EstadoCivilDataAccess()
        {

        }

        public override EstadoCivil GetEntity(IDataReader reader, out bool activo)
        {

            int Id = Parser.ToNumber(reader["ID"]);
            int ClaveCLIC = Parser.ToNumber(reader["CLIC_ID"]);
            string ClaveBuro = reader["CLAVE_BURO"].ToString();
            string Descripcion = reader["DESCRIPCION_CLIC"].ToString();
            EstadoCivil estadoCivil = new EstadoCivil(Id, ClaveCLIC, Descripcion, ClaveBuro);

            activo = true;
            return estadoCivil;
        }

        public List<EstadoCivil> GetByClaveClic(string idClic, bool soloActivos)
        {
            List<EstadoCivil> toReturn = new List<EstadoCivil>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);

                DB.AddInParameter(cmd, "idClic", DbType.AnsiString, idClic);
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (pers == Enums.Persona.Moral ? 'M' : 'F'));

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        EstadoCivil entity = GetEntity(reader, out activo);

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
            get { return "CATALOGOS.SP_CATALOGOS_GetEstadoCivil"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKESTADOCIVIL.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKESTADOCIVIL.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKESTADOCIVIL.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, EstadoCivil entityOld, EstadoCivil entityNew )
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCLIC_ID", DbType.Int32, entityNew.IdClic);
            DB.AddInParameter(cmd, "pDESCRIPCION_CLIC", DbType.AnsiString, entityNew.DescripcionClic);
            DB.AddInParameter(cmd, "pCLAVE_BURO", DbType.AnsiString, entityNew.ClaveBuro);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, EstadoCivil entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pCLIC_ID", DbType.Int32, entityToDelete.IdClic);
            DB.AddInParameter(cmd, "pCLAVE_BURO", DbType.AnsiString, entityToDelete.ClaveBuro);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, EstadoCivil entityToInsert)
        {
            //parametros para insertar
            DB.AddInParameter(cmd, "pCLIC_ID", DbType.Int32, entityToInsert.IdClic);
            DB.AddInParameter(cmd, "pDESCRIPCION_CLIC", DbType.AnsiString, entityToInsert.DescripcionClic);
            DB.AddInParameter(cmd, "pCLAVE_BURO", DbType.AnsiString, entityToInsert.ClaveBuro );
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
