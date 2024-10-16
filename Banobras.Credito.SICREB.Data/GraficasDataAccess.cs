using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Banobras.Credito.SICREB.Data
{

    public class GraficasDataAccess : OracleBase
    {

        public Archivo ConsultarEstadisticasArchivo(string Persona, Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica Store)
        {

            string store = "";
            if (Store == Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica.EstadisticasArchivo)
                store = "TRANSACCIONALES.SP_TRANS_GetEstadisArchiv";

            DbCommand cmd = DB.GetStoredProcCommand(store);
            DB.AddInParameter(cmd, "TipoPersona", DbType.String, Persona);
            Archivo ar;
            using (IDataReader reader = DB.ExecuteReader(cmd))
            {
                ar = new Archivo(0, "", Entities.Util.Enums.Persona.Fisica, DateTime.Now, null);
                if (reader.Read())
                {
                    ar.SetEstadisticas(int.Parse(reader["registros_correctos"].ToString()), int.Parse(reader["registros_errores"].ToString()), int.Parse(reader["registros_advertencias"].ToString()));
                }
                else
                {
                    ar.SetEstadisticas(0, 0, 0);
                }
            }

            return ar;
        }

        public List<Archivo> ConsultarHistorico(string Persona, Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica Store, out List<Archivo> HistoricosAdvertencias)
        {
            string store = "";
            if (Store == Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica.ErrorHistorico)
                store = "TRANSACCIONALES.SP_TRANS_GetEstadisHist";

            DbCommand cmd = DB.GetStoredProcCommand(store);
            DB.AddInParameter(cmd, "TipoPersona", DbType.String, Persona);
            DB.AddInParameter(cmd, "TipoEstadistica", DbType.Int32, 1);
            IDataReader reader = DB.ExecuteReader(cmd);
            List<Archivo> HistoricoErrores= new List<Archivo>();
            
            Archivo ar;
            while (reader.Read())
            {
                ar = new Archivo(0, "", Entities.Util.Enums.Persona.Fisica,DateTime.Parse(reader["fechaarchivo"].ToString()), null);
                ar.SetEstadisticas(0,Parser.ToNumber(reader["registros_errores"].ToString()),0);
                HistoricoErrores.Add(ar);
            }
            reader.Close();

            cmd = DB.GetStoredProcCommand(store);
            DB.AddInParameter(cmd, "TipoPersona", DbType.String, Persona);
            DB.AddInParameter(cmd, "TipoEstadistica", DbType.Int32, 2);
            reader = DB.ExecuteReader(cmd);
            HistoricosAdvertencias = new List<Archivo>();
           
            while (reader.Read())
            {
                ar = new Archivo(0, "", Entities.Util.Enums.Persona.Fisica, DateTime.Parse(reader["fechaarchivo"].ToString()), null);
                ar.SetEstadisticas(0,0,Parser.ToNumber(reader["registros_advertencias"].ToString()));
                HistoricosAdvertencias.Add(ar);
            }
            reader.Close();

            return HistoricoErrores;
        }

        public int ConsultarConciliacionCorrectos(int idarchivo)
        {
            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetEstatisCorre");
            DB.AddInParameter(cmd, "pidarchivo", DbType.Int32, idarchivo);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
            DB.ExecuteNonQuery(cmd);
             
            return (int)DB.GetParameterValue(cmd, "return");     
        }

        public int ConsultarConciliacionInvestigar(int idarchivo)
        {
            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetEstatisInve");
            DB.AddInParameter(cmd, "pidarchivo", DbType.Int32, idarchivo);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
            DB.ExecuteNonQuery(cmd);
            
            return (int)DB.GetParameterValue(cmd, "return");
        }

        public int ConsultarConciliacionDiferencias(int idarchivo)
        {
            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetEstatisDif");
            DB.AddInParameter(cmd, "pidarchivo", DbType.Int32, idarchivo);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
            DB.ExecuteNonQuery(cmd);
            
            return (int)DB.GetParameterValue(cmd, "return");
        }

        public int ConsultarConciliacionExceptuados(int idarchivo)
        {
            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetEstatisExep");
            DB.AddInParameter(cmd, "pidarchivo", DbType.Int32, idarchivo);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
            DB.ExecuteNonQuery(cmd);
            
            return (int)DB.GetParameterValue(cmd, "return");
        }

        public int ConsultarConciliacionError(int idarchivo)
        {
            DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_GetEstatisErro");
            DB.AddInParameter(cmd, "pidarchivo", DbType.Int32, idarchivo);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
            DB.ExecuteNonQuery(cmd);
            
            return (int)DB.GetParameterValue(cmd, "return");
        }

        //SICREB-INICIO-VHCC SEP-2012
        // Se sobrecargo la funcion ConsultaHistorico con 2 nuevos parametros mas.
        //      DateTime dFechaInicio
        //      DateTime dFechaFin
        public List<Archivo> ConsultarHistorico(string Persona, Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica Store, DateTime dFechaInicio, DateTime dFechaFin, out List<Archivo> HistoricosAdvertencias)
        {
            string store = "";
            if (Store == Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica.ErrorHistorico)
                store = "SP_TRANS_GETESTADISHISTRANGFE";

            DbCommand cmd = DB.GetStoredProcCommand(store);
            DB.AddInParameter(cmd, "TipoPersona", DbType.String, Persona);
            DB.AddInParameter(cmd, "TipoEstadistica", DbType.Int32, 1);
            DB.AddInParameter(cmd, "FechaInicio", DbType.String, dFechaInicio.ToString("dd/MM/yyyy"));
            DB.AddInParameter(cmd, "FechaFin", DbType.String, dFechaFin.ToString("dd/MM/yyyy"));
            IDataReader reader = DB.ExecuteReader(cmd);
            List<Archivo> HistoricoErrores = new List<Archivo>();
            
            Archivo ar;
            while (reader.Read())
            {
                ar = new Archivo(0, "", Entities.Util.Enums.Persona.Fisica, DateTime.Parse(reader["fechaarchivo"].ToString()), null);
                ar.SetEstadisticas(0, Parser.ToNumber(reader["registros_errores"].ToString()), 0);
                HistoricoErrores.Add(ar);
            }
            reader.Close();

            cmd = DB.GetStoredProcCommand(store);
            DB.AddInParameter(cmd, "TipoPersona", DbType.String, Persona);
            DB.AddInParameter(cmd, "TipoEstadistica", DbType.Int32, 2);
            DB.AddInParameter(cmd, "FechaInicio", DbType.String, dFechaInicio.ToString("dd/MM/yyyy"));
            DB.AddInParameter(cmd, "FechaFin", DbType.String, dFechaFin.ToString("dd/MM/yyyy"));
            reader = DB.ExecuteReader(cmd);
            HistoricosAdvertencias = new List<Archivo>();
           
            while (reader.Read())
            {
                ar = new Archivo(0, "", Entities.Util.Enums.Persona.Fisica, DateTime.Parse(reader["fechaarchivo"].ToString()), null);
                ar.SetEstadisticas(0, 0, Parser.ToNumber(reader["registros_advertencias"].ToString()));
                HistoricosAdvertencias.Add(ar);
            }
            reader.Close();

            return HistoricoErrores;
        }
        //SICREB-FIN-VHCC SEP-2012

    }
    
}
