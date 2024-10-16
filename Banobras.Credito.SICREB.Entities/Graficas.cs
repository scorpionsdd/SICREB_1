//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
//using System.Data.Common;
//using Banobras.Credito.SICREB.Common.Data;
//using System.Data;
//namespace Banobras.Credito.SICREB.Entities
//{
//    public class Graficas:OracleBase
//    {
        
        
//        public enum StoreGrafica {EstadisticasArchivo  ,ErrorHistorico}
//        public List<double> ConsultarEstadisticasArchivo(string Persona,StoreGrafica Store)
//        {

//            string store = "";
//            if (Store == StoreGrafica.EstadisticasArchivo)
//                store = "TRANSACCIONALES.SP_TRANS_GetEstadisArchiv";
//            DbCommand cmd = DB.GetStoredProcCommand(store);
//            DB.AddInParameter(cmd, "TipoPersona", DbType.String, Persona);
//            IDataReader reader= DB.ExecuteReader(cmd);
//            List<double> Porcentaje=new List<double>();
//            if (reader.Read())
//            {
//                Porcentaje.Add(int.Parse(reader["registros_correctos"].ToString()));
//                Porcentaje.Add(int.Parse(reader["registros_errores"].ToString()));
//                Porcentaje.Add(int.Parse(reader["registros_advertencias"].ToString()));
//            }
//            else
//            {
//                Porcentaje.Add(0);
//                Porcentaje.Add(0);
//                Porcentaje.Add(0);
//            }
//            return Porcentaje;
//        }

//        public Dictionary<string, double> ConsultarHistorico(string Persona,StoreGrafica Store,out Dictionary<string, double> DatosHistoricosErrores)
//        {
//            string store = "";
//            if (Store == StoreGrafica.ErrorHistorico)
//                store = "TRANSACCIONALES.SP_TRANS_GetEstadisHist";
//            Dictionary<string, double> DatosHistoricosAdvertencias = new Dictionary<string, double>();
//            DatosHistoricosErrores= new Dictionary<string,double>();
//            DbCommand cmd = DB.GetStoredProcCommand(store);
//            DB.AddInParameter(cmd, "TipoPersona", DbType.String,Persona);
//            DB.AddInParameter(cmd, "TipoEstadistica", DbType.Int32, 1);
//            IDataReader reader = DB.ExecuteReader(cmd);
//            while (reader.Read())
//            {
//                DatosHistoricosErrores.Add(reader["fechaarchivo"].ToString(),double.Parse(reader["registros_errores"].ToString()));
//            }
//            reader.Close();
//            cmd = DB.GetStoredProcCommand(store);
//            DB.AddInParameter(cmd, "TipoPersona", DbType.String, Persona);
//            DB.AddInParameter(cmd, "TipoEstadistica", DbType.Int32, 2);
//            reader = DB.ExecuteReader(cmd);
//            while (reader.Read())
//            {
//                DatosHistoricosAdvertencias.Add(reader["fechaarchivo"].ToString(), double.Parse(reader["registros_advertencias"].ToString()));
//            }
//            reader.Close();
//            return DatosHistoricosAdvertencias;
//        }
//    }
//}
