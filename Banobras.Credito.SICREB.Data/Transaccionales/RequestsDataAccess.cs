using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

//using testConectionOracle;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{

    public class RequestsDataAccess : OracleBase
    {

        public void ActualizaRequest(Request request)
        {
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_UpdateRequest");

                DB.AddInParameter(cmd, "pStatus", DbType.AnsiString, request.Status);
                if (request.Fecha_Final != null && request.Fecha_Final != default(DateTime))
                {
                    DB.AddInParameter(cmd, "pFechaFinal", DbType.AnsiString, request.Fecha_Final.ToString());
                }
                DB.AddInParameter(cmd, "pArchivoPM", DbType.Int32, request.ArchivoPM);
                DB.AddInParameter(cmd, "pArchivoPF", DbType.Int32, request.ArchivoPF);
                DB.AddInParameter(cmd, "pReporte", DbType.AnsiString, (request.TipoReporte == Enums.Reporte.Mensual ? "M" : "S"));
                DB.AddInParameter(cmd, "pNotif", DbType.AnsiString, (request.MandaNotificaciones ? "1" : "0"));

                DB.AddInParameter(cmd, "pId", DbType.Int32, request.Id);

                int rows = DB.ExecuteNonQuery(cmd);

                if (rows == 0)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado del request.", ex);
            }
        }

        public Request AddRequest(Enums.Reporte reporte, bool notificaciones, string grupos)
        {
            Request toReturn = new Request();
            try
            {
                toReturn.Fecha_Inicio = DateTime.Now;

                DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_AddRequest");
                DB.AddInParameter(cmd, "pFechaInicio", DbType.AnsiString, toReturn.Fecha_Inicio.ToString());
                DB.AddInParameter(cmd, "pReporte", DbType.AnsiString, (reporte == Enums.Reporte.Mensual ? "M" : "S"));
                DB.AddInParameter(cmd, "pNotif", DbType.AnsiString, (notificaciones ? "1" : "0"));
                DB.AddInParameter(cmd, "pGrupos", DbType.AnsiString, grupos);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        toReturn.Id = Parser.ToNumber(reader[0]);
                    }
                }

                if (toReturn.Id == 0)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                toReturn.Mensaje = "No se pudo iniciar el proceso" + ex.Message;
            }
            return toReturn;
        }

        public Request GetRequestPendiente()
        {
            Request toReturn = new Request();
            toReturn.Status = Request_Estado.Estado.COMPLETO;

            try
            {
                DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetRequestPendiente");

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        toReturn.Id = Parser.ToNumber(reader["id"]);
                        toReturn.Fecha_Inicio = Parser.ToDateTime(reader["Fecha_Inicio"]);
                        toReturn.Fecha_Final = Parser.ToDateTime(reader["Fecha_Final"]);
                        toReturn.Status = Request_Estado.EstadoFromString(reader["ESTADO"].ToString());
                        toReturn.ArchivoPM = Parser.ToNumber(reader["ID_ARCHIVO_PM"]);
                        toReturn.ArchivoPF = Parser.ToNumber(reader["ID_ARCHIVO_PF"]);
                        toReturn.TipoReporte = (reader["REPORTE"].ToString() == "M" ? Enums.Reporte.Mensual : Enums.Reporte.Semanal);
                        toReturn.MandaNotificaciones = Parser.ToChar(reader["NOTIFICACIONES"]) == '1';
                        toReturn.Mensaje = reader["MENSAJE"].ToString();
                        toReturn.Grupos = reader["GRUPOS"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar requests", ex);
            }

            return toReturn;
        }

        public List<Request_Estado> GetRequestsEstado()
        {
            List<Request_Estado> estados = new List<Request_Estado>();

            try
            {
                DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetRequestEstados");

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        estados.Add(new Request_Estado(reader["Estado"].ToString(), reader["Mensaje"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar los estados de requests", ex);
            }

            return estados;
        }

    }

}
