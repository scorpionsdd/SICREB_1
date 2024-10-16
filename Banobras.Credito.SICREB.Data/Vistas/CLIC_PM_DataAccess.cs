using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PM;

namespace Banobras.Credito.SICREB.Data
{

    public class CLIC_PM_DataAccess : OracleBase
    {
        private List<PM_EM> allRecordsEM = null;

        /// <summary>
        /// StoredProcedure: SP_LOAD_PM
        /// </summary>
        /// <param name="cinta">Segmento padre/root del archivo</param>
        /// <param name="tipoReporte">Mensual o Semanal</param>
        /// <param name="procesarArchivo">Indica si el archivo debe procesarse o regresa el último archivo procesado</param>
        /// <returns></returns>
        public List<PM_EM> GetPM_EMs(PM_Cinta cinta, Enums.Reporte tipoReporte, bool procesarArchivo, string grupos)
        {
            if (allRecordsEM == null || allRecordsEM.Count == 0)
            {
                try
                {
                    allRecordsEM = new List<PM_EM>();
                    String query = "DATOSEXTERNOS.SP_LOAD_PM";

                    //MASS. 04octubre2021 para pruebas en ambiente de desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        query = "DATOSEXTERNOS_Z.SP_LOAD_PM";
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
                            PM_EM item = new PM_EM(cinta);

                            item.Id = Parser.ToNumber(reader["ID"]);
                            item.EM_EM = reader["EM_EM"].ToString();
                            item.EM_00 = reader["EM_00"].ToString();
                            item.EM_01 = reader["EM_01"].ToString();
                            item.EM_02 = reader["EM_02"].ToString();
                            item.EM_03 = reader["EM_03"].ToString();
                            item.EM_04 = reader["EM_04"].ToString();
                            item.EM_05 = reader["EM_05"].ToString();
                            item.EM_06 = reader["EM_06"].ToString();
                            item.EM_07 = reader["EM_07"].ToString();
                            item.EM_08 = reader["EM_08"].ToString();
                            item.EM_09 = reader["EM_09"].ToString();
                            item.EM_10 = reader["EM_10"].ToString();
                            item.EM_11 = reader["EM_11"].ToString();
                            item.EM_12 = reader["EM_12"].ToString();
                            item.EM_13 = reader["EM_13"].ToString();
                            item.EM_14 = reader["EM_14"].ToString();
                            item.EM_15 = reader["EM_15"].ToString();
                            item.EM_16 = reader["EM_16"].ToString();
                            item.EM_17 = reader["EM_17"].ToString();
                            item.EM_18 = reader["EM_18"].ToString();
                            item.EM_19 = reader["EM_19"].ToString();
                            item.EM_20 = reader["EM_20"].ToString();
                            item.EM_21 = reader["EM_21"].ToString();
                            item.EM_22 = reader["EM_22"].ToString();
                            item.EM_23 = reader["EM_23"].ToString();
                            item.EM_24 = reader["EM_24"].ToString();
                            item.EM_25 = reader["EM_25"].ToString();
                            item.EM_26 = reader["EM_26"].ToString();
                            item.EM_27 = "";

                            // Ajuste de Informacion
                            item.EM_04 = AjustarEM_04_PrimerNombrePFAE(reader["NOMBRE"].ToString());
                            item.EM_05 = AjustarEM_05_SegundoNombrePFAE(reader["NOMBRE"].ToString());
                            item.EM_13 = AjustarEM_13_PrimerDireccion(reader["DIRECCION"].ToString());
                            item.EM_14 = AjustarEM_14_SegundaDireccion(reader["DIRECCION"].ToString());
                            item.EM_20 = AjustarEM_20_Telefono(reader["TELEFONO"].ToString());
                            item.EM_21 = AjustarEM_21_Extension(reader["TELEFONO"].ToString());
                            item.EM_22 = AjustarEM_22_Fax(reader["TELEFONO"].ToString());
                            item.EM_23 = AjustarEM_23_TipoAcreditado (reader["EM_00"].ToString(), reader["EM_23"].ToString());
                            item.EM_24 = AjustarEM_24_EstadoExtranjero(reader["EM_25"].ToString(), reader["EM_24"].ToString());

                            allRecordsEM.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cargar los segmentos EM", ex);
                }
            }

            return allRecordsEM;
        }

        /// <summary>
        /// StoredProcedure: SP_INVALID_EM
        /// </summary>
        /// <param name="em">Segmento EM que se cancelará</param>
        public void InvalidateEMs(PM_EM em)
        {
            if (em.IsValid)
            {
                throw new Exception("ERROR: No se puede cancelar un registro valido");
            }

            try
            {
                string query = "DATOSEXTERNOS.SP_INVALID_EM";

                //MASS. 04octubre2021 para pruebas en ambiente de desarrollo
                if (WebConfig.MailFrom.StartsWith("desarrollo"))
                {
                    query = "DATOSEXTERNOS_Z.SP_INVALID_EM";
                }

                DbCommand cmd = DB.GetStoredProcCommand(query);
                DB.AddInParameter(cmd, "pId", DbType.Int32, em.Id);

                DB.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar registros EM", ex);
            }
        }

        private string AjustarEM_04_PrimerNombrePFAE(string NombreOriginal)
        {
            if (NombreOriginal == null)
            {
                return string.Empty;
            }

            string[] separator = { " " };
            string[] Nombres = NombreOriginal.ToUpper().Split(separator, StringSplitOptions.None);
            if (Nombres.Count() >= 1)
            {
                return Nombres[0];
            }

            return string.Empty;
        }

        private string AjustarEM_05_SegundoNombrePFAE(string NombreOriginal)
        {
            if (NombreOriginal == null)
            {
                return string.Empty;
            }

            string[] separator = { " " };
            string[] Nombres = NombreOriginal.ToUpper().Split(separator, StringSplitOptions.None);
            if (Nombres.Count() > 1)
            {
                string SegundoNombre = "";
                for (int i = 1; i < Nombres.Count(); i++)
                {
                    SegundoNombre = SegundoNombre + " " + Nombres[i];
                }

                return SegundoNombre.Trim();
            }

            return string.Empty;
        }

        private string AjustarEM_13_PrimerDireccion(string DireccionOriginal)
        {
            if (DireccionOriginal == null)
            {
                return string.Empty;
            }

            // En la Primera Linea de Direccion regresamos los primeros 40 caracteres el cual es el tamaño maximo
            // de la etiqueta; si la longitud de la direccion es menor rellenamos a la derecha con espacios en blanco
            return DireccionOriginal.PadRight(40).Substring(0, 40);
        }

        private string AjustarEM_14_SegundaDireccion(string DireccionOriginal)
        {
            if (DireccionOriginal == null)
            {
                return string.Empty;
            }

            // Si la Direccion es mayor a 40 caracteres, quitamos los primeros 40 caracteres que se reportaron en la etiqueta 
            // Primer Linea de direccion y los caracteres restantes se reportan en esta etiqueta (Segunda Linea de Direccion)
            if (DireccionOriginal.Length > 40)
            {
                return DireccionOriginal.Substring(40);
            }

            return string.Empty;
        }

        private string AjustarEM_20_Telefono(string TelefonoExtensionFax)
        {
            if (TelefonoExtensionFax == null)
                return string.Empty;

            if (!TelefonoExtensionFax.ToUpper().Contains("EXT") && !TelefonoExtensionFax.ToUpper().Contains("FAX"))
            {
                return TelefonoExtensionFax;
            }
            else
            {
                string[] separator = { "EXT", "FAX" };
                string[] Telefonos = TelefonoExtensionFax.ToUpper().Split(separator, StringSplitOptions.None);

                // El primer elemento siempre es el telefono      
                if (Telefonos.Count() >= 1)
                {
                    return Telefonos[0];
                }
            }

            return string.Empty;
        }

        private string AjustarEM_21_Extension(string TelefonoExtensionFax)
        {
            // El Formato de la cadena seria por ejemplo 5512398745 EXT 3045 FAX 0180032165478

            if (TelefonoExtensionFax == null)
                return string.Empty;

            // Quitamos los espacios en blanco que pudiera haber
            TelefonoExtensionFax = TelefonoExtensionFax.Replace(" ", "");

            if (TelefonoExtensionFax.Contains("EXT"))
            {
                string[] separator = { "EXT" };
                string[] Telefonos = TelefonoExtensionFax.ToUpper().Split(separator, StringSplitOptions.None);

                if (Telefonos[1].Contains("FAX"))
                {
                    string[] separator2 = { "FAX" };
                    string[] ExpensionFax = TelefonoExtensionFax.ToUpper().Split(separator, StringSplitOptions.None);

                    // El Primer Elemento es la Extension y el segundo el Fax
                    return ExpensionFax[0];
                }

                // El segundo elemento es la Extension
                return Telefonos[1];
            }

            return string.Empty;
        }

        private string AjustarEM_22_Fax(string TelefonoExtensionFax)
        {
            // El Formato de la cadena seria por ejemplo 5512398745 EXT 3045 FAX 0180032165478

            if (TelefonoExtensionFax == null)
                return string.Empty;

            // Quitamos los espacios en blanco que pudiera haber
            TelefonoExtensionFax = TelefonoExtensionFax.Replace(" ", "");

            if (TelefonoExtensionFax.Contains("FAX"))
            {
                string[] separator = { "FAX" };
                string[] Telefonos = TelefonoExtensionFax.ToUpper().Split(separator, StringSplitOptions.None);

                // El segundo elemento es el Fax
                return Telefonos[1];
            }

            return string.Empty;
        }

        private string AjustarEM_23_TipoAcreditado(string RFCAcreditado, string TipoAcreditadoOriginal)
        {
            string TipoAcreditado = TipoAcreditadoOriginal;

            TipoAcreditadoDataAccess datosAcreditados = new TipoAcreditadoDataAccess(Enums.Persona.Moral);
            List<TipoAcreditado> TipoAcreditadoInfo = null;
            TipoAcreditadoInfo = datosAcreditados.GetTipoAcreditadoPorRFC(RFCAcreditado, true);

            if (TipoAcreditadoInfo.Count > 0)
            {
                if (RFCAcreditado.ToUpper().Trim() == TipoAcreditadoInfo[0].RfcAcreditado.ToUpper().Trim())
                {
                    switch (TipoAcreditadoInfo[0].Tipo_Acreditado)
                    {
                        case Enums.Persona.Moral:
                            TipoAcreditado = "1";
                            break;
                        case Enums.Persona.Fisica:
                            TipoAcreditado = "2";
                            break;                        
                        case Enums.Persona.Fideicomiso:
                            TipoAcreditado = "3";
                            break;
                        case Enums.Persona.Gobierno:
                            TipoAcreditado = "4";
                            break;
                    }
                }
            }

            return TipoAcreditado;
        }

        private string AjustarEM_24_EstadoExtranjero(string PaisDomicilio, string Estado)
        {
            if (PaisDomicilio.ToUpper().Trim() != "MX" & PaisDomicilio.ToUpper().Trim() != "")
            {
                return Estado;
            }

            return string.Empty;
        }

    }

}
