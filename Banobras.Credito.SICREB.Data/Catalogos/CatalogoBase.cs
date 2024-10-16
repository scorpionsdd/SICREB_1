using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    /// <summary>
    /// Base de Catalogos
    /// </summary>
    /// <typeparam name="T">Entidad que representa</typeparam>
    /// <typeparam name="U">Tipo del parametro de Clave</typeparam>
    public abstract class CatalogoBase<T,U> : OracleBase
    {
        
        public abstract String IdField { get; }
        public virtual String ClaveField { get { return "clave"; } }
        public virtual String ErrorGet { get { return "1001 - Error al cargar los datos de la base de datos en catálogo X."; } }
        public virtual String ErrorInsert { get { return "1050 - Error al insertar nuevo registro en catálogo X."; } }
        public virtual String ErrorUpdate { get { return "1100 - Error al actualizar registro en catálogo X."; } }
        public virtual String ErrorDelete { get { return "1150 - Error al borrar el registro en catálogo X."; } }

        public Enums.Persona pers;
        private bool definePersona = false;
        private string login = string.Empty;

        public CatalogoBase(Enums.Persona pers)
        {
            definePersona = true;
            this.pers = pers;
        }

        public CatalogoBase()
        {
            definePersona = false;
        }

        public CatalogoBase(string pLogin)
        {
            definePersona = false;
            login = pLogin;
        }

        public abstract T GetEntity(IDataReader reader, out bool activo);
        public abstract void SetEntity(DbCommand cmd, T entityOld, T entityNew);
        public virtual void SetEntityDelete(DbCommand cmd, T entityToDelete) { }
        public virtual void SetEntityInsert(DbCommand cmd, T entityToInsert) { }

        public List<T> GetRecords(bool soloActivos)
        {
            return GetRecords(0, default(U), soloActivos);
        }

        private List<T> GetRecords(int id, U clave, bool soloActivos)
        {
            //Instancia lista de entidades para poder agregar elementos a ella
            List<T> toReturn = new List<T>();

            try
            {

                //Define el stored procedure
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);

                //Agrega los parametros que fueron proporcionados
                if (id > 0)
                {
                    DB.AddInParameter(cmd, IdField, DbType.Int32, id);
                }
                else if (clave != null && !clave.Equals(default(U)))
                {
                    //U puede ser int o string, depende de eso es el tipo de parametro agregado al command
                    if (typeof(U) == typeof(int))
                        DB.AddInParameter(cmd, ClaveField, DbType.Int32, clave);
                    else
                        DB.AddInParameter(cmd, ClaveField, DbType.AnsiString, clave);
                }

                if (definePersona)
                {
                    DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (pers == Enums.Persona.Moral ? 'M' : 'F'));
                }

                if (login != string.Empty)
                {
                    DB.AddInParameter(cmd, "pLogin", DbType.AnsiString, login);
                }

                //ejecutas el stored procedure
                using (IDataReader reader = DB.ExecuteReader(cmd))
                {

                    //recorres los resultados
                    while (reader.Read())
                    {
                        bool activo;
                        //regresa una entidad tipo T, y un boleano indicando si es activo o no. 
                        //Basicamente Entity.Estatus == Activo, pero como T esta strongly typed, no se puede castear.
                        //Asi que necesitamos que el metodo regrese aparte si el registro esta activo o no.
                        T entity = GetEntity(reader, out activo);

                        //si se requieren todos O (si solo los activos y el registro actual es activo).. lo agregamos
                        if (!soloActivos || activo)
                        {
                            toReturn.Add(entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorGet, ex);
            }

            return toReturn;
        }

        public T GetItemById(int id)
        {
            if (id == 0)
            {
                return default(T);
            }

            return GetRecords(id, default(U), false).FirstOrDefault();
        }

        public T GetItemByClave(U clave)
        {
            if (clave == null || clave.Equals(default(U)))
            {
                return default(T);
            }

            //prioridad a los activos
            T toReturn = GetRecords(0, clave, true).FirstOrDefault();

            if (toReturn == null || toReturn.Equals(default(T)))
            {
                toReturn = GetRecords(0, clave, false).FirstOrDefault();
            }

            return toReturn;
        }

        public abstract String StoredProcedure { get; }
        public abstract string StoredProcedureUpdate { get; }
        public virtual string StoredProcedureInsert { get; set; }
        public virtual string StoredProcedureDelete { get; set; }

        public int UpdateRecord(T entityOld, T entityNew)
        {
            int filasAfectadas = 0;

            try
            {
                //Define el stored procedure
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedureUpdate);
                cmd.CommandType = CommandType.StoredProcedure;

                SetEntity(cmd, entityOld, entityNew);

                //ejecutas el stored procedure
                DB.ExecuteNonQuery(cmd);
                filasAfectadas = (int)DB.GetParameterValue(cmd, "return");
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorUpdate, ex);
            }

            //Devolver filasAfectadas si es un 1 si se realizo la actualizacion en 0 no se realizo.
            return filasAfectadas;
        }

        public int InsertRecord(T entity)
        {
            int filasAfectadas = 0;
            try
            {
                //Define el stored procedure
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedureInsert);
                cmd.CommandType = CommandType.StoredProcedure;
                SetEntityInsert(cmd, entity);

                //ejecutas el stored procedure
                DB.ExecuteNonQuery(cmd);
                filasAfectadas = (int)DB.GetParameterValue(cmd, "return");
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorInsert, ex);
            }

            //Devolver filasAfectadas si es un 1 si se realizo la actualizacion en 0 no se realizo.
            return filasAfectadas;
        }

        public int DeleteRecord(T entity)
        {
            int filasAfectadas = 0;
            try
            {
                //Define el stored procedure
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedureDelete);
                cmd.CommandType = CommandType.StoredProcedure;
                SetEntityDelete(cmd, entity);

                //ejecutas el stored procedure
                DB.ExecuteNonQuery(cmd);
                filasAfectadas = (int)DB.GetParameterValue(cmd, "return");
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorDelete, ex);
            }

            //Devolver filasAfectadas si es un 1 si se realizo la actualizacion en 0 no se realizo.
            return filasAfectadas;
        }

    }
   
}
