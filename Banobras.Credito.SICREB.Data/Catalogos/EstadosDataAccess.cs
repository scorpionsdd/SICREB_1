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

    public class EstadosDataAccess : CatalogoBase<Estado, int>
    {

        public override string IdField{ get { return "idestado"; } }
        public override String ErrorGet { get { return " 1047 - Error al cargar los datos de la base de datos en catálogo Estados."; } }
        public override String ErrorInsert { get { return "1048 - Error al insertar nuevo registro en catálogo Estados."; } }
        public override String ErrorUpdate { get { return "1049 - Error al actualizar registro en catálogo Estados."; } }
        public override String ErrorDelete { get { return "1050 - Error al borrar el registro en catálogo Estados."; } }

        public EstadosDataAccess(Enums.Persona pers)
            : base(pers)
        {
        }

        public override Estado GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            int claveCLIC = Parser.ToNumber(reader["CLAVE_CLIC"]);
            string claveBuro = reader["CLAVE_BURO"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();
            Enums.Persona persona = Util.GetPersona(Parser.ToChar(reader["Persona"]));
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            Estado est = new Estado(idValue, claveCLIC, claveBuro, descripcion, persona, estado);
            activo = (est.Estatus == Enums.Estado.Activo);
            
            return est;
        }

        public List<Estado> GetEstadoPorClaveBuro(string Persona, string IdEstado, string ClaveBuro, bool soloActivos)
        {
            List<Estado> toReturn = new List<Estado>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, Persona);
                DB.AddInParameter(cmd, "idestado", DbType.AnsiString, IdEstado);
                DB.AddInParameter(cmd, "clave", DbType.AnsiString, ClaveBuro);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        Estado entity = GetEntity(reader, out activo);

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
                throw new Exception("Error al obtener los datos del Estado por Clave del Buro", ex);
            }
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetEstados"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKESTADOS.SpUpdate"; }
        }
        
        public override string StoredProcedureInsert
        {
            get { return "PACKESTADOS.SpInsert"; }
        }
        
        public override string StoredProcedureDelete
        {
            get { return "PACKESTADOS.SpDelete"; }
        }
        
        public override void SetEntity(DbCommand cmd, Estado entityOld, Estado entityNew )
        {
            DB.AddInParameter(cmd, "oldCLAVE_CLIC", DbType.Int32, entityOld.ClaveClic);
            DB.AddInParameter(cmd, "oldCLAVE_BURO", DbType.AnsiString, entityOld.ClaveBuro);
            DB.AddInParameter(cmd, "oldPERSONA", DbType.AnsiString, Util.SetPersona(entityOld.TipoPersona.ToString()));
            DB.AddInParameter(cmd, "newCLAVE_CLIC", DbType.Int32, entityNew.ClaveClic);
            DB.AddInParameter(cmd, "newCLAVE_BURO", DbType.AnsiString, entityNew.ClaveBuro);
            DB.AddInParameter(cmd, "newDESCRIPCION", DbType.AnsiString, entityNew.Descripcion);
            DB.AddInParameter(cmd, "newPERSONA", DbType.AnsiString, Util.SetPersona(entityNew.TipoPersona.ToString()));
            DB.AddInParameter(cmd, "newESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Estado entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pCLAVE_CLIC", DbType.Int32, entityToDelete.ClaveClic);
            DB.AddInParameter(cmd, "pCLAVE_BURO", DbType.AnsiString, entityToDelete.ClaveBuro);
            DB.AddInParameter(cmd, "pPERSONA", DbType.AnsiString, Util.SetPersona(entityToDelete.TipoPersona.ToString()));
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToDelete.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }
       
        public override void SetEntityInsert(DbCommand cmd, Estado entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pCLAVE_CLIC", DbType.Int32, entityToInsert.ClaveClic);
            DB.AddInParameter(cmd, "pCLAVE_BURO", DbType.AnsiString, entityToInsert.ClaveBuro);
            DB.AddInParameter(cmd, "pPERSONA", DbType.AnsiString, Util.SetPersona(entityToInsert.TipoPersona.ToString()));
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityToInsert.Descripcion);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
