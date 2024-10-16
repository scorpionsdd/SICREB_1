using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{
    public class ClientesPMDataAccess : OracleBase
    {
        public virtual String ErrorGet { get { return "1001 - Error al cargar los datos de la base de datos en catálogo X."; } }
        public virtual String ErrorInsert { get { return "1050 - Error al insertar nuevo registro en catálogo X."; } }
        public virtual String ErrorUpdate { get { return "1100 - Error al actualizar registro en catálogo X."; } }
        public virtual String ErrorDelete { get { return "1150 - Error al borrar el registro en catálogo X."; } }


        public ClientesPMDataAccess()
        {

        }

        public List<ClientesPM> GetRecords()
        {
            List<ClientesPM> toReturn = new List<ClientesPM>();
            DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);
            DB.AddInParameter(cmd, "pPersona", DbType.String, "PM");

            using (IDataReader reader = DB.ExecuteReader(cmd))
            {

                //recorres los resultados
                while (reader.Read())
                {
                    toReturn.Add(GetEntity(reader));
                }

            }
            return toReturn;

        }

        public ClientesPM GetEntity(IDataReader reader)
        {

            string Rfc = reader["RFC"].ToString();
            string Curp = reader["CURP"].ToString();
            string Nombre = reader["NOMBRE"].ToString();
            string Apellido_paterno = reader["APELLIDO_PATERNO"].ToString();
            string Apellido_materno = reader["APELLIDO_MATERNO"].ToString();
            string Nacionalidad_clave = reader["NACIONALIDAD_CLAVE"].ToString();
            string Nacionalidad = reader["NACIONALIDAD"].ToString();
            string Calle = reader["CALLE"].ToString();
            string Num_ext = reader["NUM_EXT"].ToString();
            string Num_int = reader["NUM_INT"].ToString();
            string Colonia = reader["COLONIA"].ToString();
            string Municipio_clave = reader["MUNICIPIO_CLAVE"].ToString();
            string Municipio = reader["MUNICIPIO"].ToString();
            string Ciudad = reader["CIUDAD"].ToString();
            string Estado_clave = reader["ESTADO_CLAVE"].ToString();
            string Estado = reader["ESTADO"].ToString();
            string Codigo_postal = reader["CODIGO_POSTAL"].ToString();
            string Telefonos = reader["TELEFONOS"].ToString();
            string Pais_clave = reader["PAIS_CLAVE"].ToString();
            string Pais = reader["PAIS"].ToString();
            string Tipo_cliente_clave = reader["TIPO_CLIENTE_CLAVE"].ToString();
            string Tipo_cliente = reader["TIPO_CLIENTE"].ToString();
            string Compania = reader["COMPANIA"].ToString();
            string Act_eco_clave = reader["ACT_ECO_CLAVE"].ToString();
            string Act_eco = reader["ACT_ECO"].ToString();
            string Usuario_alta = reader["USUARIO_ALTA"].ToString();
            string Consecutivo = reader["CONSECUTIVO"].ToString();
            string Estatus = reader["ESTATUS"].ToString();
            string Id_tipo_cliente = reader["ID_TIPO_CLIENTE"].ToString();
            ClientesPM cliente = new ClientesPM(Rfc, Curp, Nombre, Apellido_paterno, Apellido_materno, Nacionalidad_clave, Nacionalidad, Calle, Num_ext, Num_int, Colonia, Municipio_clave, Municipio, Ciudad, Estado_clave, Estado, Codigo_postal, Telefonos, Pais_clave, Pais, Tipo_cliente_clave, Tipo_cliente,  Compania, Act_eco_clave, Act_eco, Usuario_alta, Consecutivo, Estatus, Id_tipo_cliente);

            return cliente;
        }

        public int UpdateRecord(ClientesPM entityOld, ClientesPM entityNew)
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
        public int InsertRecord(ClientesPM entity)
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
        public int DeleteRecord(string rfc)
        {
            int filasAfectadas = 0;
            try
            {
                //Define el stored procedure
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedureDelete);
                cmd.CommandType = CommandType.StoredProcedure;
                SetEntityDelete(cmd, rfc);
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
        public int DeleteAllRecords()
        {
            int filasAfectadas = 0;
            try
            {
                //Define el stored procedure
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedureDeleteAll);
                cmd.CommandType = CommandType.StoredProcedure;
                SetEntityDeleteAll(cmd); 
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


        
        public string StoredProcedure
        {
            get { return "PACKCLIENTES.SP_SELECTCTESTMP"; }
        }
        public string StoredProcedureUpdate
        {
            get { return "PACKCLIENTES.SP_UPDATE_PM"; }
        }
        public string StoredProcedureInsert
        {
            get { return "PACKCLIENTES.SP_INSERT_PM"; }
        }
        public string StoredProcedureDelete
        {
            get { return "PACKCLIENTES.SP_DELETE_PM"; }
        }

        public string StoredProcedureDeleteAll
        {
            get { return "PACKCLIENTES.SP_DELETEALL_PM"; }
        }

        public void SetEntity(DbCommand cmd, ClientesPM entityOld, ClientesPM entityNew)
        {
            
            if (entityOld.Rfc != null && entityOld.Rfc.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pRfcOld", DbType.AnsiString, entityOld.Rfc.ToString()); }
            
            if (entityNew.Rfc != null && entityNew.Rfc.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pRfc", DbType.AnsiString, entityNew.Rfc.ToString()); }

            if (entityNew.Curp != null && entityNew.Curp.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pCurp", DbType.AnsiString, entityNew.Curp.ToString()); }

            if (entityNew.Nombre != null && entityNew.Nombre.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pNombre", DbType.AnsiString, entityNew.Nombre.ToString()); }

            if (entityNew.Apellido_paterno != null && entityNew.Apellido_paterno.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pApellido_paterno", DbType.AnsiString, entityNew.Apellido_paterno.ToString());}

            if (entityNew.Apellido_materno != null && entityNew.Apellido_materno.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pApellido_materno", DbType.AnsiString, entityNew.Apellido_materno.ToString());}

            if (entityNew.Nacionalidad_clave != null && entityNew.Nacionalidad_clave.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pNacionalidad_clave", DbType.AnsiString, entityNew.Nacionalidad_clave.ToString());}

            if (entityNew.Nacionalidad != null && entityNew.Nacionalidad.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pNacionalidad", DbType.AnsiString, entityNew.Nacionalidad.ToString()); }

            if (entityNew.Calle != null && entityNew.Calle.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pCalle", DbType.AnsiString, entityNew.Calle.ToString());}

            if (entityNew.Num_ext != null && entityNew.Num_ext.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pNum_ext", DbType.AnsiString, entityNew.Num_ext.ToString());}

            if (entityNew.Num_int != null && entityNew.Num_int.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pNum_int", DbType.AnsiString, entityNew.Num_int.ToString());}

            if (entityNew.Colonia != null && entityNew.Colonia.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pColonia", DbType.AnsiString, entityNew.Colonia.ToString());}

            if (entityNew.Municipio_clave != null && entityNew.Municipio_clave.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pMunicipio_clave", DbType.AnsiString, entityNew.Municipio_clave.ToString());}

            if (entityNew.Municipio != null && entityNew.Municipio.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pMunicipio", DbType.AnsiString, entityNew.Municipio.ToString());}

            if (entityNew.Ciudad != null && entityNew.Ciudad.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pCiudad", DbType.AnsiString, entityNew.Ciudad.ToString());}

            if (entityNew.Estado_clave != null && entityNew.Estado_clave.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pEstado_clave", DbType.AnsiString, entityNew.Estado_clave.ToString());}

            if (entityNew.Estado != null && entityNew.Estado.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pEstado", DbType.AnsiString, entityNew.Estado.ToString());}

            if (entityNew.Codigo_postal != null && entityNew.Codigo_postal.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pCodigo_postal", DbType.AnsiString, entityNew.Codigo_postal.ToString());}

            if (entityNew.Telefonos != null && entityNew.Telefonos.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pTelefonos", DbType.AnsiString, entityNew.Telefonos.ToString());}

            if (entityNew.Pais_clave != null && entityNew.Pais_clave.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pPais_clave", DbType.AnsiString, entityNew.Pais_clave.ToString());}

            if (entityNew.Pais != null && entityNew.Pais.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pPais", DbType.AnsiString, entityNew.Pais.ToString());}

            if (entityNew.Tipo_cliente_clave != null && entityNew.Tipo_cliente_clave.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pTipo_cliente_clave", DbType.AnsiString, entityNew.Tipo_cliente_clave.ToString());}

            if (entityNew.Tipo_cliente != null && entityNew.Tipo_cliente.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pTipo_cliente", DbType.AnsiString, entityNew.Tipo_cliente.ToString());}

            if (entityNew.Compania != null && entityNew.Compania.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pCompania", DbType.AnsiString, entityNew.Compania.ToString());}

            if (entityNew.Act_eco_clave != null && entityNew.Act_eco_clave.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pAct_eco_clave", DbType.AnsiString, entityNew.Act_eco_clave.ToString());}

            if (entityNew.Act_eco != null && entityNew.Act_eco.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pAct_eco", DbType.AnsiString, entityNew.Act_eco.ToString());}

            if (entityNew.Usuario_alta != null && entityNew.Usuario_alta.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pUsuario_alta", DbType.AnsiString, entityNew.Usuario_alta.ToString());}

            if (entityNew.Consecutivo != null && entityNew.Consecutivo.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pConsecutivo", DbType.AnsiString, entityNew.Consecutivo); }

            if (entityNew.Estatus != null && entityNew.Estatus.ToString().Trim() != "")
            {DB.AddInParameter(cmd, "pEstatus", DbType.AnsiString, entityNew.Estatus.ToString());}

            if (entityNew.Id_tipo_cliente != null && entityNew.Id_tipo_cliente.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pId_tipo_cliente", DbType.AnsiString, entityNew.Id_tipo_cliente); }

            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);

        }
        public void SetEntityInsert(DbCommand cmd, ClientesPM entityToInsert)
        {
            if (entityToInsert.Rfc != null && entityToInsert.Rfc.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pRfc", DbType.AnsiString, entityToInsert.Rfc.ToString()); }

            if (entityToInsert.Curp != null && entityToInsert.Curp.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pCurp", DbType.AnsiString, entityToInsert.Curp.ToString()); }

            if (entityToInsert.Nombre != null && entityToInsert.Nombre.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pNombre", DbType.AnsiString, entityToInsert.Nombre.ToString()); }

            if (entityToInsert.Apellido_paterno != null && entityToInsert.Apellido_paterno.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pApellido_paterno", DbType.AnsiString, entityToInsert.Apellido_paterno.ToString()); }

            if (entityToInsert.Apellido_materno != null && entityToInsert.Apellido_materno.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pApellido_materno", DbType.AnsiString, entityToInsert.Apellido_materno.ToString()); }

            if (entityToInsert.Nacionalidad_clave != null && entityToInsert.Nacionalidad_clave.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pNacionalidad_clave", DbType.AnsiString, entityToInsert.Nacionalidad_clave.ToString()); }

            if (entityToInsert.Nacionalidad != null && entityToInsert.Nacionalidad.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pNacionalidad", DbType.AnsiString, entityToInsert.Nacionalidad.ToString()); }

            if (entityToInsert.Calle != null && entityToInsert.Calle.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pCalle", DbType.AnsiString, entityToInsert.Calle.ToString()); }

            if (entityToInsert.Num_ext != null && entityToInsert.Num_ext.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pNum_ext", DbType.AnsiString, entityToInsert.Num_ext.ToString()); }

            if (entityToInsert.Num_int != null && entityToInsert.Num_int.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pNum_int", DbType.AnsiString, entityToInsert.Num_int.ToString()); }

            if (entityToInsert.Colonia != null && entityToInsert.Colonia.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pColonia", DbType.AnsiString, entityToInsert.Colonia.ToString()); }

            if (entityToInsert.Municipio_clave != null && entityToInsert.Municipio_clave.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pMunicipio_clave", DbType.AnsiString, entityToInsert.Municipio_clave.ToString()); }

            if (entityToInsert.Municipio != null && entityToInsert.Municipio.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pMunicipio", DbType.AnsiString, entityToInsert.Municipio.ToString()); }

            if (entityToInsert.Ciudad != null && entityToInsert.Ciudad.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pCiudad", DbType.AnsiString, entityToInsert.Ciudad.ToString()); }

            if (entityToInsert.Estado_clave != null && entityToInsert.Estado_clave.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pEstado_clave", DbType.AnsiString, entityToInsert.Estado_clave.ToString()); }

            if (entityToInsert.Estado != null && entityToInsert.Estado.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pEstado", DbType.AnsiString, entityToInsert.Estado.ToString()); }

            if (entityToInsert.Codigo_postal != null && entityToInsert.Codigo_postal.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pCodigo_postal", DbType.AnsiString, entityToInsert.Codigo_postal.ToString()); }

            if (entityToInsert.Telefonos != null && entityToInsert.Telefonos.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pTelefonos", DbType.AnsiString, entityToInsert.Telefonos.ToString()); }

            if (entityToInsert.Pais_clave != null && entityToInsert.Pais_clave.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pPais_clave", DbType.AnsiString, entityToInsert.Pais_clave.ToString()); }

            if (entityToInsert.Pais != null && entityToInsert.Pais.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pPais", DbType.AnsiString, entityToInsert.Pais.ToString()); }

            if (entityToInsert.Tipo_cliente_clave != null && entityToInsert.Tipo_cliente_clave.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pTipo_cliente_clave", DbType.AnsiString, entityToInsert.Tipo_cliente_clave.ToString()); }

            if (entityToInsert.Tipo_cliente != null && entityToInsert.Tipo_cliente.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pTipo_cliente", DbType.AnsiString, entityToInsert.Tipo_cliente.ToString()); }

            if (entityToInsert.Compania != null && entityToInsert.Compania.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pCompania", DbType.AnsiString, entityToInsert.Compania.ToString()); }

            if (entityToInsert.Act_eco_clave != null && entityToInsert.Act_eco_clave.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pAct_eco_clave", DbType.AnsiString, entityToInsert.Act_eco_clave.ToString()); }

            if (entityToInsert.Act_eco != null && entityToInsert.Act_eco.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pAct_eco", DbType.AnsiString, entityToInsert.Act_eco.ToString()); }

            if (entityToInsert.Usuario_alta != null && entityToInsert.Usuario_alta.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pUsuario_alta", DbType.AnsiString, entityToInsert.Usuario_alta.ToString()); }

            if (entityToInsert.Consecutivo != null && entityToInsert.Consecutivo.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pConsecutivo", DbType.AnsiString, entityToInsert.Consecutivo); }

            if (entityToInsert.Estatus != null && entityToInsert.Estatus.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pEstatus", DbType.AnsiString, entityToInsert.Estatus.ToString()); }

            if (entityToInsert.Id_tipo_cliente != null && entityToInsert.Id_tipo_cliente.ToString().Trim() != "")
            { DB.AddInParameter(cmd, "pId_tipo_cliente", DbType.AnsiString, entityToInsert.Id_tipo_cliente); }

            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);

        }

        public void SetEntityDelete(DbCommand cmd, string entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pRfc", DbType.AnsiString, entityToDelete);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);

        }
        public void SetEntityDeleteAll(DbCommand cmd)
        {
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }


    }
}
