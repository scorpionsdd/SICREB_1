using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Vistas;

using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Banobras.Credito.SICREB.Data.Seguridad
{

    public class ActividadDataAccess : OracleBase
    {

        private const string AGREGA = "ACTIVIDADES.SP_ACTIVIDAD_InsertAct";
        private const string CONSULTA = "ACTIVIDADES.SP_ACTIVIDAD_GetBitacora";
        private const string CONSULTADATOS = "ACTIVIDADES.SP_ACTIVIDAD_GetDatos";

        public ActividadDataAccess() { }

        //DULCEA MARIA MARTINEZ HERRERA 19/OCT/2012 REQUERIMIENTO NO.20
        public static bool insertActividad(int idUsuario, string catalogo, string fechaRegistro, string horaRegistro, string status, int facultad, string datoInicial, string datoFinal)
        {
            bool resp;
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(ActividadDataAccess.AGREGA);
                DB.AddInParameter(cmd, "VID_USUARIO", System.Data.DbType.Int32, idUsuario);
                DB.AddInParameter(cmd, "VNOMBRE_CATALOGO", System.Data.DbType.String, catalogo);
                DB.AddInParameter(cmd, "VFECHA_REGISTRO", System.Data.DbType.String, fechaRegistro);
                DB.AddInParameter(cmd, "VHORA_REGISTRO", System.Data.DbType.String, horaRegistro);
                DB.AddInParameter(cmd, "VESTATUS", System.Data.DbType.String, status);
                DB.AddInParameter(cmd, "VID_FACULTAD", System.Data.DbType.Int32, facultad);//id de faculatades
                DB.AddInParameter(cmd, "VDATO_INICIAL", System.Data.DbType.String, datoInicial);
                DB.AddInParameter(cmd, "VDATO_FINAL", System.Data.DbType.String, datoFinal);

                DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
                DB.ExecuteNonQuery(cmd);
                if ((int)DB.GetParameterValue(cmd, "return") > 0)
                    resp = true;
                else
                    resp = true;
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return resp;
        }

        public static bool insertActividad(int idActividad, int idUsuario, string detalle)
        {
            bool resp;
            try
            {

                DbCommand cmd = DB.GetStoredProcCommand(ActividadDataAccess.AGREGA);
                DB.AddInParameter(cmd, "idFacultad", System.Data.DbType.Int32, idActividad);
                DB.AddInParameter(cmd, "idUsuario", System.Data.DbType.Int32, idUsuario);
                DB.AddInParameter(cmd, "detalle", System.Data.DbType.AnsiString, detalle);
                DB.AddInParameter(cmd, "datosAntes", System.Data.DbType.AnsiString, "");
                DB.AddInParameter(cmd, "datosDespues", System.Data.DbType.AnsiString, "");
                DB.AddInParameter(cmd, "Catalogo", System.Data.DbType.AnsiString, "");
                DB.AddInParameter(cmd, "Tipo", System.Data.DbType.AnsiString, "0");
              
                DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
                DB.ExecuteNonQuery(cmd);
                if ((int)DB.GetParameterValue(cmd, "return") > 0)
                    resp = true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return resp;
        }

        public static bool insertActividad(int idActividad, int idUsuario, string detalle,String antes,String despues,String Catalogo,int Tipo)
        {
            bool resp;
            try
            {

                DbCommand cmd = DB.GetStoredProcCommand(ActividadDataAccess.AGREGA);
                DB.AddInParameter(cmd, "idFacultad", System.Data.DbType.Int32, idActividad);
                DB.AddInParameter(cmd, "idUsuario", System.Data.DbType.Int32, idUsuario);
                DB.AddInParameter(cmd, "detalle", System.Data.DbType.AnsiString, detalle);
                DB.AddInParameter(cmd, "datosAntes", System.Data.DbType.AnsiString, antes);
                DB.AddInParameter(cmd, "datosDespues", System.Data.DbType.AnsiString, despues);
                DB.AddInParameter(cmd, "Catalogo", System.Data.DbType.AnsiString, Catalogo);
                DB.AddInParameter(cmd, "Tipo", System.Data.DbType.Int32, Tipo);
                DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
                DB.ExecuteNonQuery(cmd);
                if ((int)DB.GetParameterValue(cmd, "return") > 0)
                    resp = true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return resp;
        }

        public static List<Actividad> consultaActividad(string fechaInicio, string fechaFin)
        {
            List<Actividad> actividad = new List<Actividad>();
            DbCommand cmd = DB.GetStoredProcCommand(ActividadDataAccess.CONSULTA);

            DB.AddInParameter(cmd, "fechaIni", DbType.String, fechaInicio.Substring(0,10));
            DB.AddInParameter(cmd, "fechFin", DbType.String, fechaFin.Substring(0, 10));
            IDataReader reader = DB.ExecuteReader(cmd);
            while (reader.Read())
            {
                actividad.Add(new Actividad(reader));
            }
            return actividad;
        }

        public static List<ActividadDatos> consultaActividadDatos(string fechaInicio, string fechaFin)
        {
            List<ActividadDatos> actividad = new List<ActividadDatos>();
            DbCommand cmd = DB.GetStoredProcCommand(ActividadDataAccess.CONSULTADATOS);

            DB.AddInParameter(cmd, "fechaIni", DbType.String, fechaInicio.Substring(0, 10));
            DB.AddInParameter(cmd, "fechFin", DbType.String, fechaFin.Substring(0, 10));
            IDataReader reader = DB.ExecuteReader(cmd);
            while (reader.Read())
            {

                actividad.Add(new ActividadDatos(reader));
            }

            return actividad;
        }

    }

}
