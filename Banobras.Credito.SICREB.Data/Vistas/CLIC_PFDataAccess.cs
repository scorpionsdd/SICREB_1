using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PF;

namespace Banobras.Credito.SICREB.Data
{

    public class CLIC_PFDataAccess: OracleBase
    {

        List<CLIC_PF> allRecords;
        private List<PF_PN> allRecordsPN = null;
        private List<PF_PA> allRecordsPA = null;

        #region Segmento del Nombre del Cliente o Acreditado (PN)

            public List<PF_PN> GetPF_PN(PF_Cinta cinta, Enums.Reporte tipoReporte, bool procesarArchivo, string grupos)
            {
                if (allRecordsPN == null)
                {
                    allRecordsPN = new List<PF_PN>();
                    try
                    {
                        String query = "BYPASS.SP_LOAD_PF";

                        //MASS. 04octubre2021 para pruebas en desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            query = "BYPASS_Z.SP_LOAD_PF";
                        }


                        DbCommand cmd = DB.GetStoredProcCommand(query);
                        DB.AddInParameter(cmd, "pReporte", DbType.AnsiString, tipoReporte);
                        DB.AddInParameter(cmd, "pProcesar", DbType.AnsiString, (procesarArchivo ? "1" : "0"));
                        DB.AddInParameter(cmd, "pArchivoId", DbType.Int32, cinta.ArchivoId);
                        DB.AddInParameter(cmd, "pGrupos", DbType.AnsiString, grupos);

                        using (IDataReader reader = DB.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                PF_PN item = new PF_PN(cinta);

                                item.Id = Parser.ToNumber(reader["ID"]);
                                item.ArchivoId = Parser.ToNumber(reader["ARCHIVO_ID"]);

                                item.PN_PN = reader["PN_PN"].ToString();
                                item.PN_00 = AjustarPN_00_ApellidoMaterno(reader["PN_00"].ToString());
                                item.PN_02 = AjustarPN_02_PrimerNombre(reader["NOMBRE"].ToString());
                                item.PN_03 = AjustarPN_03_SegundoNombre(reader["SEGUNDO_NOMBRE"].ToString());
                                item.PN_04 = AjustarPN_04_FechaNacimiento(Parser.ToDateTime(reader["FECHA_NACIMIENTO"]));
                                item.PN_05 = reader["PN_05"].ToString();
                                item.PN_08 = reader["PN_08"].ToString();
                                item.PN_11 = AjustarPN_11_EstadoCivil( reader["PN_11"].ToString() );
                                item.PN_12 = reader["PN_12"].ToString();
                                item.PN_15 = AjustarPN_15_ClaveIdentificacion(reader["PN_15"].ToString());
                                item.PN_16 = AjustarPN_16_ClavePais(reader["PN_16"].ToString(), reader["PN_15"].ToString());
                                item.PN_20 = AjustarPN_20_FechaDefuncion(Parser.ToDateTime(reader["FECHA_DEFUNCION"]));
                                item.PN_21 = AjustarPN_21_IndicadorDefuncion(Parser.ToDateTime(reader["FECHA_DEFUNCION"]));

                                allRecordsPN.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al cargar segmentos PN", ex);
                    }
                }

                return allRecordsPN;
            }

            public void InvalidatePNs(PF_PN pn)
            {
                if (pn.IsValid)
                {
                    throw new Exception("ERROR: No se puede cancelar un registro PN valido");
                }

                try
                {
                    string query = "DATOSEXTERNOS.SP_INVALID_PN";

                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        query = "DATOSEXTERNOS_Z.SP_INVALID_PN";
                    }

                    DbCommand cmd = DB.GetStoredProcCommand(query);

                    DB.AddInParameter(cmd, "pId", DbType.Int32, pn.Id);
                    DB.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cancelar registros PN", ex);
                }
            }

            private string AjustarPN_00_ApellidoMaterno(string ApellidoMaternoOriginal)
            {
                return ApellidoMaternoOriginal.Trim() == string.Empty ? "NO PROPORCIONADO" : ApellidoMaternoOriginal.Trim();
            }

            private string AjustarPN_02_PrimerNombre(string PrimerNombreOriginal)
            {
                return PrimerNombreOriginal.PadRight(26).Substring(0, 26).Trim();
            }

            private string AjustarPN_03_SegundoNombre(string SegundoNombreOriginal)
            {
                return SegundoNombreOriginal.PadRight(26).Substring(0, 26).Trim();
            }

            private string AjustarPN_04_FechaNacimiento(DateTime FechaNacimientoOriginal)
            {
                return (FechaNacimientoOriginal != default(DateTime) ? FechaNacimientoOriginal.ToString(Parser.FORMATO_FECHA) : "");
            }

            private string AjustarPN_11_EstadoCivil(string EstadoCivilOriginal)
            {
                if (EstadoCivilOriginal != "D" && EstadoCivilOriginal != "F" && EstadoCivilOriginal != "M" && EstadoCivilOriginal != "S" && EstadoCivilOriginal != "W")
                {
                    return string.Empty;
                }

                return EstadoCivilOriginal;
            }

            private string AjustarPN_15_ClaveIdentificacion(string ClaveIdentificacionOriginal)
            {
                return ClaveIdentificacionOriginal;
            }

            private string AjustarPN_16_ClavePais(string ClavePaisOriginal, string ClaveIdentificacionOriginal)
            {
                if (ClaveIdentificacionOriginal == string.Empty)
                {
                    return "";
                }

                return ClavePaisOriginal;
            }

            private string AjustarPN_20_FechaDefuncion(DateTime FechaDefuncionOriginal)
            {
                return (FechaDefuncionOriginal != default(DateTime) ? FechaDefuncionOriginal.ToString(Parser.FORMATO_FECHA) : "" );
            }

            private string AjustarPN_21_IndicadorDefuncion(DateTime FechaDefuncionOriginal)
            {
                return (FechaDefuncionOriginal != default(DateTime) ? "Y" : "");
            }

        #endregion

        #region Segmento de Dirección del Cliente (PA)

            public List<PF_PA> GetPF_PA(int archivoId, bool procesarArchivo)
            {
                if (allRecordsPA == null)
                {
                    allRecordsPA = new List<PF_PA>();

                    String query = "DATOSEXTERNOS.SP_LOAD_PA";

                    //MASS. 04octubre2021 para pruebas en desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        query = "DATOSEXTERNOS_Z.SP_LOAD_PA";
                    }

                    DbCommand cmd = DB.GetStoredProcCommand(query);

                    DB.AddInParameter(cmd, "pProcesar", DbType.AnsiString, (procesarArchivo ? "1" : "0"));
                    DB.AddInParameter(cmd, "pArchivoId", DbType.Int32, archivoId);

                    using (IDataReader reader = DB.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            PF_PA item = new PF_PA();

                            item.Id = Parser.ToNumber(reader["ID"]);
                            item.PN_ID = Parser.ToNumber(reader["PN_ID"]);
                            item.ArchivoId = Parser.ToNumber(reader["ARCHIVO_ID"]);

                            item.PA_PA = AjustarPA_PA_PrimerDireccion( reader["CALLE"].ToString(), reader["NUM_EXT"].ToString(), reader["NUM_INT"].ToString() );
                            item.PA_00 = AjustarPA_00_SegundaDireccion( reader["CALLE"].ToString(), reader["NUM_EXT"].ToString(), reader["NUM_INT"].ToString() );
                            item.PA_01 = AjustarPA_01_ColoniaPoblacion( reader["PA_01"].ToString() );
                            item.PA_02 = reader["PA_02"].ToString();
                            item.PA_03 = reader["PA_03"].ToString();
                            item.PA_04 = reader["PA_04"].ToString();
                            item.PA_05 = reader["PA_05"].ToString();
                            item.PA_07 = AjustarPA_07_Telefono( reader["TELEFONO"].ToString() );
                            item.PA_12 = AjustarPA_12_OrigenDomicilioPais(" "); //NUEVO CAMPO Agregar Informacion

                            allRecordsPA.Add(item);
                        }
                    }
                }

                return allRecordsPA;
            }

            public void InvalidatePAs(PF_PA pa)
            {
                if (pa.IsValid)
                {
                    throw new Exception("ERROR: No se puede cancelar un registro PA válido");
                }

                try
                {
                    string query = "DATOSEXTERNOS.SP_INVALID_PA";

                    //MASS. 04octubre2021 para pruebas en desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        query = "DATOSEXTERNOS_Z.SP_INVALID_PA";
                    }

                    DbCommand cmd = DB.GetStoredProcCommand(query);
                    DB.AddInParameter(cmd, "pId", DbType.Int32, pa.Id);

                    DB.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cancelar registros PA", ex);
                }
            }

            private string AjustarPA_PA_PrimerDireccion(string Calle, string NumExterior, string NumInterior)
            {

                string Direccion = string.Format("{0} {1} {2}", Calle.Trim(), NumExterior.Trim(), NumInterior.Trim());
                
                if (Direccion.Trim().Length > 40)
                { 
                    return string.Format("{0}", Direccion.Trim().Substring(0, 40)); 
                }
                else
                { 
                    return Direccion.Trim(); 
                }
            }

            private string AjustarPA_00_SegundaDireccion(string Calle, string NumExterior, string NumInterior)
            {
                string Direccion = string.Format("{0} {1} {2}", Calle.Trim(), NumExterior.Trim(), NumInterior.Trim());

                if (Direccion.Trim().Length > 40)
                {
                    return string.Format("{0}", Direccion.Trim().Substring(40, Direccion.Trim().Length - 40));
                }
                else
                {
                    return "";
                }
            }

            private string AjustarPA_01_ColoniaPoblacion(string ColoniaPoblacionOriginal)
            {

                if (ColoniaPoblacionOriginal.Trim().Length > 40)
                {
                    return ColoniaPoblacionOriginal.Trim().Substring(0, 40);
                }
                else
                {
                    return ColoniaPoblacionOriginal.Trim();
                }
            }

            private string AjustarPA_07_Telefono(string TelefonoOriginal)
            {
                return TelefonoOriginal.Replace("-", string.Empty).Replace(" ", string.Empty);
            }

            private string AjustarPA_12_OrigenDomicilioPais(string DomicilioPaisOrigen)
            {

                if (DomicilioPaisOrigen.Trim() == string.Empty)
                {
                    return "MX";
                }
                else
                {
                    return DomicilioPaisOrigen;
                }
            }

        #endregion

        #region Segmento de Empleo del Cliente (PE)

        #endregion

        #region Segmento de Empleo del Cliente (TL)

            public List<CLIC_PF> GetRecords(Enums.Reporte tipoReporte)
            {
                allRecords = new List<CLIC_PF>();

                String query = "DATOSEXTERNOS.SP_ObtenerCLICPF";

                //MASS. 04octubre2021 para pruebas en desarrollo
                if (WebConfig.MailFrom.StartsWith("desarrollo"))
                {
                    query = "DATOSEXTERNOS_Z.SP_ObtenerCLICPF";
                }

                DbCommand cmd = DB.GetStoredProcCommand(query);
                DB.AddInParameter(cmd, "pReporte", DbType.AnsiString, tipoReporte);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        CLIC_PF item = new CLIC_PF();

                        item.Rfc = reader["RFC"].ToString();
                        item.Curp = reader["CURP"].ToString();
                        item.Nombre = reader["NOMBRE"].ToString();
                        item.ApellidoP = reader["APELLIDO_PATERNO"].ToString();
                        item.ApellidoM = reader["APELLIDO_MATERNO"].ToString();
                        item.NacionalidadClave = reader["NACIONALIDAD_CLAVE"].ToString();
                        item.Nacionalidad = reader["NACIONALIDAD"].ToString();
                        item.NacionalidadBuro = reader["NACIONALIDAD_BURO"].ToString();
                        item.Calle = reader["CALLE"].ToString();
                        item.NumExt = reader["NUM_EXT"].ToString();
                        item.NumInt = reader["NUM_INT"].ToString();
                        item.Colonia = reader["COLONIA"].ToString();
                        item.MunicipioClave = reader["MUNICIPIO_CLAVE"].ToString();
                        item.Municipio = reader["MUNICIPIO"].ToString();
                        item.SmxMunicipio = reader["SMX_MUNICIPIO"].ToString();
                        item.Ciudad = reader["CIUDAD"].ToString();
                        item.SmxCiudad = reader["SMX_CIUDAD"].ToString();
                        item.EstadoClave = reader["ESTADO_CLAVE"].ToString();
                        item.Estado = reader["ESTADO"].ToString();
                        item.EstadoBuro = reader["ESTADO_BURO"].ToString();
                        item.SmxEstado = reader["SMX_ESTADO"].ToString();
                        item.CodigoPostal = reader["CODIGO_POSTAL"].ToString();
                        item.Telefono = reader["TELEFONOS"].ToString();
                        item.PaisClave = reader["PAIS_CLAVE"].ToString();
                        item.Pais = reader["PAIS"].ToString();
                        item.PaisBuro = reader["PAIS_BURO"].ToString();
                        item.TipoClienteClave = reader["TIPO_CLIENTE_CLAVE"].ToString();
                        item.TipoCliente = reader["TIPO_CLIENTE"].ToString();
                        item.FechaNacimiento = reader["Fecha_nac"].ToString();
                        item.EdoCivilClave = reader["estado_civil_clave"].ToString();
                        item.EdoCivil = reader["estado_civil"].ToString();
                        item.EdoCivilBuro = reader["estado_civil_buro"].ToString();
                        item.Sexo = reader["sexo"].ToString();
                        item.FechaDefuncion = reader["Fecha_Defuncion"].ToString();
                        item.UsuarioAlta = reader["USUARIO_ALTA"].ToString();
                        

                        allRecords.Add(item);
                    }
                }

                return allRecords;
            }

            public void InvalidateTLs(PF_TL tl)
            {
                if (tl.IsValid)
                {
                    throw new Exception("ERROR: No se puede cancelar un registro TL válido");
                }

                try
                {
                    string query = "DATOSEXTERNOS.SP_INVALID_TL";
                    //MASS. 04octubre2021 para pruebas en desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        query = "DATOSEXTERNOS_Z.SP_INVALID_TL";
                    }

                    DbCommand cmd = DB.GetStoredProcCommand(query);
                    DB.AddInParameter(cmd, "pId", DbType.Int32, tl.Id);
                    DB.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cancelar registros TL", ex);
                }
            }

        #endregion

    }

}
