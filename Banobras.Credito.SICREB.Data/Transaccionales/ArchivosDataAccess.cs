using Banobras.Credito.SICREB.Common.ExceptionHelpers;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{

    public class ArchivosDataAccess : OracleBase
    {

        private Enums.Persona persona;
        private List<Archivo> allArchivos = null;

        public ArchivosDataAccess(Enums.Persona persona)
        {
            this.persona = persona;
        }

        public int AgregaArchivoInicial(Archivo archivo)
        {
            int registroId = 0;
            try
            {
                string query = "TRANSACCIONALES.SP_TRANS_SetArchivo";
                DbCommand cmd = DB.GetStoredProcCommand(query);

                DB.AddInParameter(cmd, "nombrearchivo", DbType.AnsiString, archivo.Nombre);
                DB.AddInParameter(cmd, "personaarchivo", DbType.AnsiString, (archivo.TipoPersona == Enums.Persona.Moral ? "M" : "F"));
                DB.AddInParameter(cmd, "fechaarchivo", DbType.Date, archivo.Fecha);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        registroId = Parser.ToNumber(reader[0]);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return registroId;
        }

        public void ActualizaArchivo(Archivo archivo)
        {
            //para agregar un campo blob no es posible hacerlo con un command, es necesario usar un data adapter/data set
            //por lo que no podemos usar Enterprise library
            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "TRANSACCIONALES.SP_TRANS_GuardaArchivo";

                        cmd.Parameters.Add("pArchivoId", OracleDbType.Int32).Value = archivo.Id;
                        cmd.Parameters.Add("pNombre", OracleDbType.Varchar2).Value = archivo.Nombre;
                        cmd.Parameters.Add("pCorrectos", OracleDbType.Int32).Value = archivo.Reg_Correctos;
                        cmd.Parameters.Add("pErrores", OracleDbType.Int32).Value = archivo.Reg_Errores;
                        cmd.Parameters.Add("pWarnings", OracleDbType.Int32).Value = archivo.Reg_Advertencias;
                        cmd.Parameters.Add("pArchivo", OracleDbType.Blob).Value = archivo.GetByteArchivo();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar archivo. ", ex);
            }
        }

        public int AgregaArchivo(Archivo archivo, out string mensaje)
        {
            //para agregar un campo blob no es posible hacerlo con un command, es necesario usar un data adapter/data set
            //por lo que no podemos usar Enterprise library
            int registroId = 0;

            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "TRANSACCIONALES.SP_TRANS_SetArchivo";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("cur_OUT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("bytearchivo", OracleDbType.Blob).Value = archivo.GetByteArchivo();
                        cmd.Parameters.Add("nombrearchivo", OracleDbType.Varchar2).Value = archivo.Nombre;
                        cmd.Parameters.Add("personaarchivo", OracleDbType.Varchar2).Value = (archivo.TipoPersona == Enums.Persona.Moral ? 'M' : 'F');
                        cmd.Parameters.Add("fechaarchivo", OracleDbType.Date).Value = archivo.Fecha;
                        cmd.Parameters.Add("registroscorrectos", OracleDbType.Int32).Value = archivo.Reg_Correctos;
                        cmd.Parameters.Add("registroserrores", OracleDbType.Int32).Value = archivo.Reg_Errores;
                        cmd.Parameters.Add("registrosadvertencias", OracleDbType.Int32).Value = archivo.Reg_Advertencias;

                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                registroId = Parser.ToNumber(reader["CURRVAL"]);
                            }
                        }
                    }
                }

                mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el archivo", ex);
            }

            return registroId;
        }

        public int AgregaArchivo(Archivo archivo)
        {
            string msj;
            return AgregaArchivo(archivo, out msj);
        }

        public Archivo GetUltimoArchivo()
        {
            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetUltimoArchivo");
            DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (this.persona == Enums.Persona.Moral ? "M" : "F"));

            using (IDataReader reader = DB.ExecuteReader(cmd))
            {
                if (reader.Read())
                {

                    int id = Parser.ToNumber(reader["ID"]);
                    string nombre = reader["NOMBRE"].ToString();
                    DateTime fecha = Parser.ToDateTime(reader["FECHA"]);
                    StringBuilder sb = new StringBuilder(reader["archivo"].ToString());

                    byte[] bytes = null;
                    if (reader["ARCHIVO"] != System.DBNull.Value)
                    {
                        bytes = (byte[])reader["ARCHIVO"];
                    }

                    int correctos = Parser.ToNumber(reader["REGISTROS_CORRECTOS"]);
                    int errores = Parser.ToNumber(reader["REGISTROS_ERRORES"]);
                    int advertencias = Parser.ToNumber(reader["REGISTROS_ADVERTENCIAS"]);

                    Archivo arc = new Archivo(id, nombre, this.persona, fecha, sb);
                    arc.SetEstadisticas(correctos, errores, advertencias);
                    arc.BytesArchivo = bytes;

                    return arc;
                }
            }

            return null;
        }

        public List<Archivo> GetAllArchivos()
        {
            if (allArchivos == null)
            {
                allArchivos = new List<Archivo>();

                DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetArchivos");
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (this.persona == Enums.Persona.Moral ? "M" : "F"));

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        int id = Parser.ToNumber(reader["ID"]);
                        string nombre = reader["NOMBRE"].ToString();
                        DateTime fecha = Parser.ToDateTime(reader["FECHA"]);
                        int correctos = Parser.ToNumber(reader["registros_correctos"]);
                        int errores = Parser.ToNumber(reader["registros_errores"]);
                        int advertencias = Parser.ToNumber(reader["registros_advertencias"]);
                        byte[] bytes = (byte[])reader["ARCHIVO"];

                        Archivo arch = new Archivo(id, nombre, this.persona, fecha, new StringBuilder());
                        arch.SetEstadisticas(correctos, errores, advertencias);
                        arch.BytesArchivo = bytes;

                        allArchivos.Add(arch);
                    }
                }
            }

            return allArchivos;
        }

        //SICREB-INICIO-VHCC OCT-2012
        //Se creo esta nueva funcion para correr un Stored Procedure que te devuelve 
        //los archivos que cumplen cierta condicion.
        public List<Archivo> GetAllArchivosMesAnio(int vmes, int vanio)
        {
            if (allArchivos == null)
            {
                allArchivos = new List<Archivo>();

                DbCommand cmd = DB.GetStoredProcCommand("SP_BUSCAARCHIVOSMESANIO");
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (this.persona == Enums.Persona.Moral ? "M" : "F"));
                DB.AddInParameter(cmd, "vmes", DbType.Int32, vmes);
                DB.AddInParameter(cmd, "vanio", DbType.Int32, vanio);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        int id = Parser.ToNumber(reader["ID"]);
                        string nombre = reader["NOMBRE"].ToString();
                        DateTime fecha = Parser.ToDateTime(reader["FECHA"]);
                        int correctos = Parser.ToNumber(reader["registros_correctos"]);
                        int errores = Parser.ToNumber(reader["registros_errores"]);
                        int advertencias = Parser.ToNumber(reader["registros_advertencias"]);
                        byte[] bytes = (byte[])reader["ARCHIVO"];

                        Archivo arch = new Archivo(id, id+"-"+nombre, this.persona, fecha, new StringBuilder());
                        arch.SetEstadisticas(correctos, errores, advertencias);
                        arch.BytesArchivo = bytes;

                        allArchivos.Add(arch);
                    }
                }
            }

            return allArchivos;
        }

        public DataTable GetMesesArchivos(int vanio)
        {
            DataTable dtMeses = new DataTable();
            DbCommand cmd = DB.GetStoredProcCommand("SP_BUSCARMESESARCHIVOS");
            DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (this.persona == Enums.Persona.Moral ? "M" : "F"));
            DB.AddInParameter(cmd, "vanio", DbType.Int32, vanio);
            
            using (IDataReader reader = DB.ExecuteReader(cmd))
            {
                dtMeses.Columns.Add(new DataColumn("Id", typeof(int)));
                dtMeses.Columns.Add(new DataColumn("Nombre", typeof(string)));
                while (reader.Read())
                {
                    dtMeses.Rows.Add(Parser.ToNumber(reader["idMes"]), reader["Mes"].ToString());
                }
            }

            return dtMeses;
        }

        public DataTable GetAniosArchivos()
        {
            DataTable dtAnios = new DataTable();
            DbCommand cmd = DB.GetStoredProcCommand("SP_BUSCARANIOSARCHIVOS");
            DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (this.persona == Enums.Persona.Moral ? "M" : "F"));

            using (IDataReader reader = DB.ExecuteReader(cmd))
            {
                dtAnios.Columns.Add(new DataColumn("Id", typeof(int)));
                dtAnios.Columns.Add(new DataColumn("Nombre", typeof(string)));
                while (reader.Read())
                {
                    dtAnios.Rows.Add(Parser.ToNumber(reader["idAnio"]), reader["Anio"].ToString());
                }
            }

            return dtAnios;
        }
        //SICREB-FIN-VHCC SEP-2012


        public string GetUltimoArchivoGL()
        {
            string CadenaArchivo = "";

            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "PKG_GPOS_LCCYR.SP_GET_ARCHIVO_GL";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("cur_OUT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CadenaArchivo = reader[0].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al Consultar el Ultimo Archivo de GPOs y LCCyR", ex);
            }

            return CadenaArchivo.ToString();

        }

        public string GetFechaUltimoProcesoGL()
        {
            DateTime FehcaProcedoGL = Parser.ToDateTime("01/01/1900", "ddMMyyyy");

            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "PKG_GPOS_LCCYR.SP_GET_FECHA_ULTIMO_PROCESO";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("cur_OUT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FehcaProcedoGL = Parser.ToDateTime(reader[0]);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string message = ExceptionMessageHelper.GetExceptionMessage(ex);
                throw new Exception("Error al Consultar la fecha del Ultimo Proceso de GPOs y LCCyR", ex);
                return FehcaProcedoGL.ToString();
            }
                       
            return FehcaProcedoGL.ToString();
        }

        public string ProcesarArchivoGL()
        {
            string Mensaje = "";

            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PKG_GPOS_LCCYR.SP_SET_GPOS_LCCYR";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al procesar el archivo de GPOs y LCCyR", ex);
                Mensaje = ex.ToString();
            }

            return Mensaje;
        }

    }
}
