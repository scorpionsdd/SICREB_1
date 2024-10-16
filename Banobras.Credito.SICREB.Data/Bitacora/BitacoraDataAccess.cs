using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Newtonsoft.Json;

namespace Banobras.Credito.SICREB.Data.Bitacora
{

    /// <summary>
    /// Clase que ejecuta las operaciones hacia la BD de la Bitácora.
    /// </summary>
    public class BitacoraDataAccess : OracleBase
    {

        private const string sp_GuardarBitacora = "PKG_BITACORA.SP_AGREGAR_BITACORA";
        private const string sp_ObtenerBitacora = "PKG_BITACORA.SP_OBTENER_BITACORA";
        private const string sp_ObtenerEventos = "PKG_BITACORA.SP_OBTENER_EVENTOS";

        public BitacoraDataAccess() { }

        /// <summary>
        /// Guardar bitacora
        /// </summary>
        /// <param name="bitacora">Contenedor con información de bitácora</param>
        /// <returns></returns>
        public static bool AgregarBitacora(Entities.Bitacora bitacora)
        {
            bool isSaved = false;
            string jsonEntity = JsonConvert.SerializeObject(bitacora.Request);
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(BitacoraDataAccess.sp_GuardarBitacora);

                bitacora.Request = string.Empty;
                //DB.AddInParameter(cmd, "Bitacora_Id", System.Data.DbType.Int32, bitacora.LogId);
                DB.AddInParameter(cmd, "p_Comentarios", System.Data.DbType.AnsiString, bitacora.Comments);
                DB.AddInParameter(cmd, "p_EventoId", System.Data.DbType.Int32, bitacora.EventId);
                DB.AddInParameter(cmd, "p_Fecha", System.Data.DbType.DateTime, bitacora.CreationDate);
                DB.AddInParameter(cmd, "p_IPSesion", System.Data.DbType.AnsiString, bitacora.SessionIP);
                DB.AddInParameter(cmd, "p_UsuarioId", System.Data.DbType.Int32, bitacora.EmployeeNumber);
                DB.AddInParameter(cmd, "p_UsuarioLogin", System.Data.DbType.AnsiString, bitacora.UserLogin);
                DB.AddInParameter(cmd, "p_UsuarioNombre", System.Data.DbType.AnsiString, bitacora.UserFullName);
                DB.AddInParameter(cmd, "p_Request", System.Data.DbType.AnsiString, jsonEntity);
                DB.AddInParameter(cmd, "p_Tipo", System.Data.DbType.AnsiString, bitacora.LogType);

                DB.ExecuteNonQuery(cmd);
                isSaved = true;
            }
            catch (Exception ex)
            {
                string message = BitacoraDataAccess.GetExceptionMessage(ex);
                //throw;
            }

            return isSaved;
        }

        /// <summary>
        /// Obtener data de Bitácora
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial de consulta</param>
        /// <param name="fechaFinal">Fecha hasta donde se desea consultar</param>
        /// <param name="eventoId">Identificador de Evento</param>
        /// <param name="userName">Nombre de usuario por el que se desea consultar</param>
        /// <returns></returns>
        public static List<Entities.Bitacora> ObtenerBitacora(DateTime? fechaInicial, DateTime? fechaFinal, int? eventoId, string userName)
        {
            List<Entities.Bitacora> result = new List<Entities.Bitacora>();
            DbCommand cmd = DB.GetStoredProcCommand(BitacoraDataAccess.sp_ObtenerBitacora);

            if ((fechaInicial != null && fechaInicial != DateTime.MinValue) && (fechaFinal != null && fechaFinal != DateTime.MinValue))
            {
                DB.AddInParameter(cmd, "p_FechaInicial", DbType.Date, fechaInicial);
                DB.AddInParameter(cmd, "p_FechaFinal", DbType.Date, fechaFinal);
            }

            if (eventoId != null && eventoId != 0)
            {
                DB.AddInParameter(cmd, "p_EventoId", DbType.Int32, eventoId);
            }

            if (!string.IsNullOrEmpty(userName))
            {
                DB.AddInParameter(cmd, "p_UsuarioId", DbType.String, userName);
            }

            IDataReader reader = DB.ExecuteReader(cmd);
            while (reader.Read())
            {
                //Bitacora bitacora = new Bitacora();
                //if (reader["descripcion"] != null)
                //{
                //    this.descripcion = Convert.ToString(reader["descripcion"]);
                //}

                Entities.Bitacora bitacora = new Entities.Bitacora
                {
                    LogId = Convert.ToInt32(reader["Bitacora_Id"]),
                    Comments = Convert.ToString(reader["Comentarios"]),
                    CreationDate = Convert.ToDateTime(reader["Fecha"]),
                    EmployeeNumber = Convert.ToInt32(reader["Usuario_Id"]),
                    EventId = Convert.ToInt32(reader["Evento_Id"]),
                    Request = Convert.ToString(reader["Request"]),
                    SessionIP = Convert.ToString(reader["IP_Sesion"]),
                    UserLogin = Convert.ToString(reader["Usuario_Login"]),
                    UserFullName = Convert.ToString(reader["Usuario_Nombre"]),
                    LogType = Convert.ToString(reader["Bitacora_Tipo"])
                };

                result.Add(bitacora);
            }

            return result;
        }

        public static List<Entities.Bitacora> ObtenerBitacoraDUMMY(DateTime? fechaInicial, DateTime? fechaFinal, int? eventoId, string userName)
        {
            List<Entities.Bitacora> result = new List<Entities.Bitacora>();

            var time = DateTime.Now;
            for (int i = 1; i <= 10; i++)
            {
                result.Add(new Entities.Bitacora
                {
                    LogId = i,
                    Comments = "Aquí un comentario de la transacción realizada por el usuario.",
                    CreationDate = time,
                    EmployeeNumber = 1234 + i,
                    EventId = i,
                    Request = string.Empty,
                    SessionIP = "127.0.0.1",
                    UserLogin = "rlopeztr",
                    UserFullName = "Nelson Garcia Arevalo",
                    LogType = i % 2 == 0 ? BitacoraTipoEstatusEnum.Successful : BitacoraTipoEstatusEnum.NotSuccessful
                });
                time = time.AddHours(8);
            }


            if ((fechaInicial != null && fechaInicial != DateTime.MinValue) && (fechaFinal != null && fechaFinal != DateTime.MinValue))
            {
                result = result.FindAll(x => x.CreationDate.Date >= fechaInicial.Value.Date && x.CreationDate.Date <= fechaFinal.Value.Date);
            }

            if (eventoId != null && eventoId != 0)
            {
                result = result.FindAll(x => x.EventId == eventoId);
            }

            if (!string.IsNullOrEmpty(userName))
            {
                result = result.FindAll(x => x.UserLogin == userName);
            }

            return result;
        }

        /// <summary>
        /// Obtener data de Eventos
        /// </summary>
        /// <returns></returns>
        public static List<BitacoraEvento> ObtenerEventos()
        {
            List<BitacoraEvento> result = new List<BitacoraEvento>();
            DbCommand cmd = DB.GetStoredProcCommand(BitacoraDataAccess.sp_ObtenerEventos);
            IDataReader reader = DB.ExecuteReader(cmd);
            while (reader.Read())
            {
                BitacoraEvento evento = new BitacoraEvento {
                    Evento_Id = Convert.ToInt32(reader["Evento_Id"]),
                    Descripcion = Convert.ToString(reader["Descripcion"])
                };

                result.Add(evento);
            }

            return result;
        }
        
        public static List<BitacoraEvento> ObtenerEventosDUMMY()
        {
            List<BitacoraEvento> result = new List<BitacoraEvento>();

            result.Add(new BitacoraEvento { Evento_Id = 1, Descripcion = "Alta de Rol" });
            result.Add(new BitacoraEvento { Evento_Id = 2, Descripcion = "Actualización de Rol" });
            result.Add(new BitacoraEvento { Evento_Id = 3, Descripcion = "Baja de Rol" });
            result.Add(new BitacoraEvento { Evento_Id = 4, Descripcion = "Reactivación de Rol" });
            result.Add(new BitacoraEvento { Evento_Id = 5, Descripcion = "Alta de función en Rol" });
            result.Add(new BitacoraEvento { Evento_Id = 6, Descripcion = "Baja de función en Rol" });
            result.Add(new BitacoraEvento { Evento_Id = 7, Descripcion = "Alta de Usuario" });
            result.Add(new BitacoraEvento { Evento_Id = 8, Descripcion = "Actualización de Usuario" });
            result.Add(new BitacoraEvento { Evento_Id = 9, Descripcion = "Baja de Usuario" });
            result.Add(new BitacoraEvento { Evento_Id = 10, Descripcion = "Reactivación de Usuario" });
            result.Add(new BitacoraEvento { Evento_Id = 11, Descripcion = "Cerrar sesión de Usuario" });
            result.Add(new BitacoraEvento { Evento_Id = 12, Descripcion = "Inico de sesión" });
            result.Add(new BitacoraEvento { Evento_Id = 13, Descripcion = "Cierre de sesión" });
            result.Add(new BitacoraEvento { Evento_Id = 14, Descripcion = "Inicio de sesión fallido" });
            result.Add(new BitacoraEvento { Evento_Id = 15, Descripcion = "Usuario no registrado" });
            result.Add(new BitacoraEvento { Evento_Id = 16, Descripcion = "Usuario con sesión fallida" });

            return result;
        }

        /// <summary>
        /// Obtener mensaje de excepción
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static string GetExceptionMessage(Exception exception)
        {
            StringBuilder sb = new StringBuilder(exception.Message);
            while (exception != null && exception.InnerException != null)
            {
                sb.Append(" " + exception.InnerException.Message);
                exception = exception.InnerException;
            }

            return sb.ToString();
        }
        
    }

}
