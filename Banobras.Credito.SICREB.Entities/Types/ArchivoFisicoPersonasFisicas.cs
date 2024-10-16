//using System;
//using System.Text;
//using System.Data;
//using System.Data.Common;
//namespace Banobras.Credito.SICREB.Entities.Types
//{
//    public class ArchivoFisicoPersonasFisicas : OracleBase
//    {

//        public int IdArchivo { private set; get; }
//        public string ContenidoArchivo { private set; get; }
//        public string NombreArchivo { private set; get; }
//        public char PersonaArchivo { private set; get; }
//        public DateTime FechaArchivo { private set; get; }
//        public int Correctos { private set; get; }
//        public int Errores { private set; get; }
//        public int Advertencia { private set; get; }
//        public int GuardarArchivo(StringBuilder Archivo, int RegistrosCorrectos, int RegistrosErrores, int RegistrosAdvertencia)
//        {
//            int registro = 0;
//            try
//            {
//                //  MemoryStream ArchivoMemoria = new MemoryStream();
//                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
//                byte[] byteArchivo = encoding.GetBytes(Archivo.ToString());
//                // ArchivoMemoria.Write(byteArchivo, 0, byteArchivo.Length);

//                DbCommand cmd = DB.GetStoredProcCommand("TRANSACCIONALES.SP_TRANS_SetArchivo");
//                DB.AddInParameter(cmd, "bytearchivo", DbType.Binary, byteArchivo);
//                DB.AddInParameter(cmd, "nombrearchivo", DbType.String, "Persona_Fisica_" + DateTime.Now.ToString("ddMMyyyy") + ".txt");
//                DB.AddInParameter(cmd, "personaarchivo", DbType.String, "F");
//                DB.AddInParameter(cmd, "fechaarchivo", DbType.Date, DateTime.Now);
//                DB.AddInParameter(cmd, "registroscorrectos", DbType.Int32, RegistrosCorrectos);
//                DB.AddInParameter(cmd, "registroserrores", DbType.Int32, RegistrosErrores);
//                DB.AddInParameter(cmd, "registrosadvertencias", DbType.Int32, RegistrosAdvertencia);
//                using (IDataReader reader = DB.ExecuteReader(cmd))
//                {
//                    if (reader.Read())
//                    {
//                        registro = int.Parse(reader["CURRVAL"].ToString());
//                    }
//                }
//            }
//            catch (Exception ex)
//            {

//            }
//            return registro;
//        }

//        public ArchivoFisicoPersonasFisicas ObtenerArchivo(int idArchivo)
//        {

//            try
//            {
//                ArchivoFisicoPersonasFisicas Regreso = new ArchivoFisicoPersonasFisicas();

//                byte[] archivo = new byte[0];
//                Microsoft.Practices.EnterpriseLibrary.Data.Database DB = DatabaseFactory.CreateDatabase("SICREB");
//                DbCommand cmd = DB.GetStoredProcCommand("CATALOGOS.SP_CATALOGOS_GetArchivo");
//                DB.AddInParameter(cmd, "idarchivo", DbType.Int32, IdArchivo);
//                IDataReader reader = DB.ExecuteReader(cmd);
//                if (reader.Read())
//                {
//                    Regreso.IdArchivo = int.Parse(reader["id"].ToString());
//                    Regreso.NombreArchivo = reader["nombre"].ToString();
//                    Regreso.PersonaArchivo = char.Parse(reader["persona"].ToString());
//                    Regreso.FechaArchivo = DateTime.Parse(reader["fecha"].ToString());
//                    Regreso.Advertencia = int.Parse(reader["registros_advertencias"].ToString());
//                    Regreso.Correctos = int.Parse(reader["registros_correctos"].ToString());
//                    Regreso.Errores = int.Parse(reader["registros_errores"].ToString());
//                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
//                    archivo = (byte[])reader["archivo"];
//                    Regreso.ContenidoArchivo = encoding.GetString(archivo);
//                    return Regreso;
//                }

//            }
//            catch (Exception ex)
//            {

//            }
//            return new ArchivoFisicoPersonasFisicas();
//        }
//    }
//}
