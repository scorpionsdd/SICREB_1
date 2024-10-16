using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class TipoRelacionDataAccess : CatalogoBase<TipoRelacion, string>
    {

        public override string IdField { get { return "ClaveRelacion"; } }
        public override String ErrorGet { get { return "1003 - Error al cargar los datos de la base de datos en catálogo Tipos de Relación."; } }
        public override String ErrorInsert { get { return "1004 - Error al insertar nuevo registro en catálogo Tipos de Relación."; } }
        public override String ErrorUpdate { get { return "1005 - Error al actualizar registro en catálogo Tipos de Relación."; } }
        public override String ErrorDelete { get { return "1006 - Error al borrar el registro en catálogo Tipos de Relación."; } }

        public TipoRelacionDataAccess()
        { 
        
        }

        public override TipoRelacion GetEntity(System.Data.IDataReader reader, out bool activo)
        {

            int id = Parser.ToNumber(reader["ID"]);
            string ClaveRelacion = reader["CLAVE_RELACION"].ToString();
            string TipoRelaciones = reader["TIPO_RELACION"].ToString();
            string Descripcion = reader["DESCRIPCION"].ToString();
            string Ocupada = reader["OCUPADA"].ToString();
            Enums.Estado Estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            TipoRelacion tpRelacion = new TipoRelacion(id, ClaveRelacion, TipoRelaciones, Descripcion, Ocupada, Estatus);
            activo = tpRelacion.Estatus == Enums.Estado.Activo;
            return tpRelacion;
        }

        public List<TipoRelacion> GetTipoRelacionPorClave(string claveRelacion, bool soloActivos)
        {
            List<TipoRelacion> toReturn = new List<TipoRelacion>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);

                DB.AddInParameter(cmd, "ClaveRelacion", DbType.AnsiString, claveRelacion);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        TipoRelacion entity = GetEntity(reader, out activo);

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
                throw new Exception("Error al Obtener los Datos del Tipo de Relacion por Clave Relacion", ex);
            }
        }

        public List<TipoRelacion> GetTipoRelacionPorUtilidad(string ocupadas, bool soloActivos)
        {
            List<TipoRelacion> toReturn = new List<TipoRelacion>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);

                DB.AddInParameter(cmd, "Ocupados", DbType.AnsiString, ocupadas);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        TipoRelacion entity = GetEntity(reader, out activo);

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
                throw new Exception("Error al Obtener los Datos del Tipo de Relacion por Utilidad", ex);
            }
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetRelaciones"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKTIPOSRELACIONES.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKTIPOSRELACIONES.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKTIPOSRELACIONES.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, TipoRelacion entityOld, TipoRelacion entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCLAVE_RELACION", DbType.AnsiString, entityNew.ClaveRelacion);
            DB.AddInParameter(cmd, "pTIPO_RELACION", DbType.AnsiString, entityNew.TipoRelaciones);
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityNew.Descripcion);
            DB.AddInParameter(cmd, "pOCUPADA", DbType.AnsiString, entityNew.Ocupada);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, TipoRelacion entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pCLAVE_RELACION", DbType.AnsiString, entityToDelete.ClaveRelacion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, TipoRelacion entityToInsert)
        {
            //parametros para insertar
            DB.AddInParameter(cmd, "pCLAVE_RELACION", DbType.AnsiString, entityToInsert.ClaveRelacion);
            DB.AddInParameter(cmd, "pTIPO_RELACION", DbType.AnsiString, entityToInsert.TipoRelaciones);
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityToInsert.Descripcion);
            DB.AddInParameter(cmd, "pOCUPADA", DbType.AnsiString, entityToInsert.Ocupada);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
