using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using System.Data;
using testConectionOracle;
using Banobras.Credito.SICREB.Entities;

namespace Banobras.Credito.SICREB.testConectionOracle
{
    public class cls_conecta : OracleBase2012
    {

        public void conetaME(){
            try
            {
                DataTable dt= new DataTable();
                Oracle.DataAccess.Client.OracleDataAdapter oada = new Oracle.DataAccess.Client.OracleDataAdapter();
                {

                    using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                    {
                        conn.Open();
                        using (OracleCommand cmd = conn.CreateCommand())
                        {

                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM T_ARCHIVOS";

                            cmd.ExecuteReader();
                            oada.SelectCommand = cmd;

                            oada.Fill(dt);


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        
        
        }



        public void ActualizaArchivo2012(Archivo archivo)
        {


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
                throw new Exception("Error al actualizar archivo", ex);
            }

        }//ActualizaArhivo2012

        public void ActualizaArchivo_2011(Archivo archivo)
        {

            try
            {
                using (OracleConnection conn = new OracleConnection(DB.ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = conn.CreateCommand())
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "TRANSACCIONALES.SP_TRANS_GuardaArchivo_2011";

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
                throw new Exception("Error al actualizar archivo", ex);
            }
        }


    }
}