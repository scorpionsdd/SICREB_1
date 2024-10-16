using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Business.Repositorios;

namespace Banobras.Credito.SICREB.Common.Validator.PM
{
    public static class CommonValidator
    {

        public const string REGX_DIGITOS = @"^\d*$";
        public const string REGX_TEXTO_ESPACIOS = @"^[a-zA-Z0-9 ]*$";
        public const string REGX_FECHA_DDMMAAAA = @"^(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[012])(19|20)\d\d$";
        public const string REGX_RFC = @"^[a-zA-Z]{3}[0-9]{6}([a-zA-Z0-9]{3})?\s*$";
        public const string REGX_RFC_FISICA = @"^[A-Za-z]{4}[ |\\-]{0,1}[0-9]{6}[ |\\-]{0,1}$?";
        

        public static bool EmptyDate(string toValidate)
        {
            if (String.IsNullOrWhiteSpace(toValidate) || Parser.ToNumber(toValidate) == 0)
                return true;

            string empty = default(DateTime).ToString("ddMMyyyy");

            if (toValidate.Trim().Equals(empty))
                return true;

            return false;
        }

        public static bool Match(string toValidate, string pattern, bool opcional)
        {
            if (String.IsNullOrWhiteSpace(toValidate))
            {
                return opcional;
            }

            return Regex.IsMatch(toValidate.Trim(), pattern);
        }

        public static bool V_Rfc(string input, bool opcional, out int codigo_Error)
        {
            codigo_Error = 0;

            bool matches = Match(input, REGX_RFC, opcional);
            bool matches2 = Match(input, REGX_RFC_FISICA, opcional);
            if (matches || matches2)
            {
                RfcFallecidosDataAccess fallecidos = new RfcFallecidosDataAccess();
                RfcFallecido fallecido = fallecidos.GetItem(input);

                if (fallecido == default(RfcFallecido))
                    return true;
                else
                {
                    codigo_Error = 101;
                }
            }
            else
            {
                codigo_Error = 102;
            }
            return false;
        }

        public static bool V_CaracteresEspeciales(string input)
        {
            if (input.Contains("\"") || 
                input.Contains("&") ||
                input.Contains("#") ||
                input.Contains("$") ||
                input.Contains("@") ||
                input.Contains("!") ||
                input.Contains("¡") ||
                input.Contains("¿") ||
                input.Contains("?") ||
                input.Contains("/") ||
                input.Contains("*") ||
                input.Contains("-") ||
                input.Contains("+"))
            {
                return true;
            }

            return false;
        }

        public static bool V_RFCGenerico(string input)
        {
            if (input.Contains("XXX010101") ||
                input.Contains("XAXX010101") ||
                input.Contains("XEXX010101"))
            {   
                return true;  
            }
                
            return false;
        }

        public static bool ValidTipoCreditoEspecial(string segmento, string etiqueta, string tipoDeCuenta)
        {
            int numCuenta;
            int.TryParse(tipoDeCuenta, out numCuenta);

            //CreditosAccessLayer creditos = new CreditosAccessLayer();
            CreditosDataAccess creditos = new CreditosDataAccess();
            List<int> warnings = creditos.CreditosWarnings(segmento, etiqueta);

            List<Entities.TipoCredito> listCreditos = creditos.GetRecords(true);

            var cat = listCreditos
                        .Where(c => c.Id.Equals(numCuenta))
                        .FirstOrDefault();

            if (cat != null)
            {
                if (warnings.Contains(cat.Id))
                    return true;
            }
            return false;
        }

        public static bool ValidarClaveEstado(string ClaveEstado, Enums.Persona TipoPersona)
        {
            List<Estado> EstadosInfo;

            try
            {
                EstadosInfo = ConsultarEstado(ClaveEstado, TipoPersona);
                if (EstadosInfo.Count >= 1)
                    return true;
                else
                    return false;
            }
            catch
            { 
                return false; 
            }
        }

        public static bool ValidarClavePais(string ClavePais)
        {
            List<Pais> PaisesInfo;

            try
            {
                PaisesInfo = ConsultarPais(ClavePais);
                if (PaisesInfo.Count >= 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static string ValidarCPSEPOMEX_PM(string CP, string ClaveEstado, string Municipio, string Ciudad)
        {
            try
            {
                List<Estado> EstadosInfo;
                List<SEPOMEX> SEPOMEXInfo;
                string Estado = "";
                string MunicipioCP;
                string CiudadCP;
                string EstadoCP;

                EstadosInfo = ConsultarEstado(ClaveEstado, Enums.Persona.Moral);

                if (EstadosInfo.Count() == 0)
                {
                    // 00) Si no existe es la Clave del Estado en el catalogo se rechaza el Registro.           
                    // Pero se deja la validacion del estado al segmento corresponciente.
                    return "00";
                }

                foreach (Estado ItemEstado in EstadosInfo)
                {
                    Estado = ItemEstado.Descripcion;
                }

                Estado = Estado.ToUpper();
                Estado = Estado.Replace("Á", "A");
                Estado = Estado.Replace("É", "E");
                Estado = Estado.Replace("Í", "I");
                Estado = Estado.Replace("Ó", "O");
                Estado = Estado.Replace("Ú", "U");

                Municipio = Municipio.ToUpper();
                Municipio = Municipio.Replace("Á", "A");
                Municipio = Municipio.Replace("É", "E");
                Municipio = Municipio.Replace("Í", "I");
                Municipio = Municipio.Replace("Ó", "O");
                Municipio = Municipio.Replace("Ú", "U");

                Ciudad = Ciudad.ToUpper();
                Ciudad = Ciudad.Replace("Á", "A");
                Ciudad = Ciudad.Replace("É", "E");
                Ciudad = Ciudad.Replace("Í", "I");
                Ciudad = Ciudad.Replace("Ó", "O");
                Ciudad = Ciudad.Replace("Ú", "U");

                SEPOMEXInfo = ConsultaCPSepomex(CP);

                if (SEPOMEXInfo.Count() == 0)
                {
                    // 01) Si el CP no existe en el catalogo de SEPOMEX el registro se rechaza.
                    return "01";
                }

                foreach (SEPOMEX ItemSEPOMEX in SEPOMEXInfo)
                {
                    EstadoCP = ItemSEPOMEX.DEstado;
                    MunicipioCP = ItemSEPOMEX.DMnpio;
                    CiudadCP = ItemSEPOMEX.DCiudad;

                    EstadoCP = EstadoCP.ToUpper();
                    EstadoCP = EstadoCP.Replace("Á", "A");
                    EstadoCP = EstadoCP.Replace("É", "E");
                    EstadoCP = EstadoCP.Replace("Í", "I");
                    EstadoCP = EstadoCP.Replace("Ó", "O");
                    EstadoCP = EstadoCP.Replace("Ú", "U");

                    // Como no todos los estados que regresa SEPOMEX en el codigo postal corresponden exactamente 
                    // con los estados del catalogo del Buro de Credito agregamos las excepciones correspondientes.
                    if (EstadoCP == "VERACRUZ DE IGNACIO DE LA LLAVE") EstadoCP = "VERACRUZ";
                    if (EstadoCP == "MICHOACAN DE OCAMPO") EstadoCP = "MICHOACAN";
                    //if (EstadoCP == "MEXICO") EstadoCP = "ESTADO DE MEXICO";
                    if (EstadoCP == "COAHUILA DE ZARAGOZA") EstadoCP = "COAHUILA";
                    //if (EstadoCP == "BAJA CALIFORNIA" && EstadoCP != "BAJA CALIFORNIA SUR") EstadoCP = "BAJA CALIFORNIA NORTE";

                    MunicipioCP = MunicipioCP.ToUpper();
                    MunicipioCP = MunicipioCP.Replace("Á", "A");
                    MunicipioCP = MunicipioCP.Replace("É", "E");
                    MunicipioCP = MunicipioCP.Replace("Í", "I");
                    MunicipioCP = MunicipioCP.Replace("Ó", "O");
                    MunicipioCP = MunicipioCP.Replace("Ú", "U");

                    CiudadCP = CiudadCP.ToUpper();
                    CiudadCP = CiudadCP.Replace("Á", "A");
                    CiudadCP = CiudadCP.Replace("É", "E");
                    CiudadCP = CiudadCP.Replace("Í", "I");
                    CiudadCP = CiudadCP.Replace("Ó", "O");
                    CiudadCP = CiudadCP.Replace("Ú", "U");

                    // Actualmente solo se valida con el ultimo CP localizado, si un CP regresa mas de un registro no
                    // se toman en cuenta los registros iniciales.

                    // 02) Si el Estado del catalogo y el Estado del CP no coinciden se rechaza el Registro.    Rechazado
                    if (EstadoCP != Estado)
                    { return "02"; }

                    // 03) EstadoCP = EstadoR      MunicipioCP != MunicipioR       CiudadCP != CiudadR          Warning
                    // Si el Municipio y la Ciudad son reportadas y ambas son diferentes de las que indica el CP se marca como Warning
                    if (EstadoCP == Estado && MunicipioCP != Municipio && CiudadCP != Ciudad && Municipio != string.Empty && Ciudad != string.Empty)
                    { return "03"; }

                    // 04) EstadoCP = EstadoR       MunicipioCP != MunicipioR                                   Warning
                    // Si el municipio es reporteado y es diferente del CP y la ciudad no es reportada, se marca como Warning
                    if (EstadoCP == Estado && MunicipioCP != Municipio && Ciudad == string.Empty && Municipio != string.Empty)
                    { return "04"; }

                    // 05) EstadoCP = EstadoR                                       CiudadCP != CiudadR         Warning
                    // Si la ciudad es diferente del CP y el municipio no es reportado, se marca como Warning
                    if (EstadoCP == Estado && Municipio == string.Empty && CiudadCP != Ciudad && Ciudad != string.Empty)
                    { return "05"; }

                    // 06) EstadoCP = EstadoR       MunicipioCP = MunicipioR        CiudadCP = CiudadR          Aceptado
                    // Si los tres datos coinciden el registro se acepta
                    if (EstadoCP == Estado && MunicipioCP == Municipio && CiudadCP == Ciudad && Municipio != string.Empty && Ciudad != string.Empty)
                    { return "06"; }
                    
                    // 07) EstadoCP = EstadoR       MunicipioCP = MunicipioR                                    Aceptado
                    // Si el municipio es igual al CP y la ciudad no se reporta, el registro se acepta
                    if (EstadoCP == Estado && MunicipioCP == Municipio && Ciudad == string.Empty && Municipio != string.Empty)
                    { return "07"; }

                    // 08) EstadoCP = EstadoR                                       CiudadCP = CiudadR          Aceptado
                    // Si la ciudad es igual al CP y el municipio no se reporta, el registro se acepta.
                    if (EstadoCP == Estado && Municipio == string.Empty && CiudadCP == Ciudad && Ciudad != string.Empty)
                    { return "08"; }
                    
                    // 09) EstadoCP = EstadoR       MunicipioCP != MunicipioR       CiudadCP = CiudadR          Aceptado
                    // Si el municipio es diferente pero las ciudades coinciden el registro se acepta
                    if (EstadoCP == Estado && MunicipioCP != Municipio && CiudadCP == Ciudad && Municipio != string.Empty && Ciudad != string.Empty)
                    { return "09"; }

                    // 10) EstadoCP = EstadoR      MunicipioCP = MunicipioR        CiudadCP != CiudadR          Rechazado
                    // Si el estado y el municipio coinciden con CP, pero la ciudad no, el registro se rechaza
                    // (Es importante revizar esta validacion ya que es muy confusa y se contradice un poco)
                    if (EstadoCP == Estado && MunicipioCP == Municipio && CiudadCP != Ciudad && Municipio != string.Empty && Ciudad != string.Empty)
                    { return "10"; }
                }

                return "00";
            }
            catch
            {
                return "00";
            } 
        }

        public static string ValidarCPSEPOMEX_PF(string CP, string ClaveEstado, string Municipio, string Ciudad)
        {
            try
            {
                List<Estado> EstadosInfo;
                List<SEPOMEX> SEPOMEXInfo;
                string Estado = "";
                string MunicipioCP;
                string CiudadCP;
                string EstadoCP;

                EstadosInfo = ConsultarEstado(ClaveEstado, Enums.Persona.Fisica);

                if (EstadosInfo.Count() == 0)
                {
                    // 00) Si no existe es la Clave del Estado en el catalogo se rechaza el Registro.           
                    // Pero se deja la validacion del estado al segmento corresponciente.
                    return "00";
                }

                foreach (Estado ItemEstado in EstadosInfo)
                {
                    Estado = ItemEstado.Descripcion;
                }

                Estado = Estado.ToUpper();
                Estado = Estado.Replace("Á", "A");
                Estado = Estado.Replace("É", "E");
                Estado = Estado.Replace("Í", "I");
                Estado = Estado.Replace("Ó", "O");
                Estado = Estado.Replace("Ú", "U");

                Municipio = Municipio.ToUpper();
                Municipio = Municipio.Replace("Á", "A");
                Municipio = Municipio.Replace("É", "E");
                Municipio = Municipio.Replace("Í", "I");
                Municipio = Municipio.Replace("Ó", "O");
                Municipio = Municipio.Replace("Ú", "U");

                Ciudad = Ciudad.ToUpper();
                Ciudad = Ciudad.Replace("Á", "A");
                Ciudad = Ciudad.Replace("É", "E");
                Ciudad = Ciudad.Replace("Í", "I");
                Ciudad = Ciudad.Replace("Ó", "O");
                Ciudad = Ciudad.Replace("Ú", "U");

                SEPOMEXInfo = ConsultaCPSepomex(CP);

                if (SEPOMEXInfo.Count() == 0)
                {
                    // 01) Si el CP no existe en el catalogo de SEPOMEX el registro se rechaza.
                    return "01";
                }

                foreach (SEPOMEX ItemSEPOMEX in SEPOMEXInfo)
                {
                    EstadoCP = ItemSEPOMEX.DEstado;
                    MunicipioCP = ItemSEPOMEX.DMnpio;
                    CiudadCP = ItemSEPOMEX.DCiudad;

                    EstadoCP = EstadoCP.ToUpper();
                    EstadoCP = EstadoCP.Replace("Á", "A");
                    EstadoCP = EstadoCP.Replace("É", "E");
                    EstadoCP = EstadoCP.Replace("Í", "I");
                    EstadoCP = EstadoCP.Replace("Ó", "O");
                    EstadoCP = EstadoCP.Replace("Ú", "U");

                    // Como no todos los estados que regresa SEPOMEX en el codigo postal corresponden exactamente 
                    // con los estados del catalogo del Buro de Credito agregamos las excepciones correspondientes.
                    if (EstadoCP == "VERACRUZ DE IGNACIO DE LA LLAVE") EstadoCP = "VERACRUZ";
                    if (EstadoCP == "MICHOACAN DE OCAMPO") EstadoCP = "MICHOACAN";
                    //if (EstadoCP == "MEXICO") EstadoCP = "ESTADO DE MEXICO";
                    if (EstadoCP == "COAHUILA DE ZARAGOZA") EstadoCP = "COAHUILA";
                    //if (EstadoCP == "BAJA CALIFORNIA" && EstadoCP != "BAJA CALIFORNIA SUR") EstadoCP = "BAJA CALIFORNIA NORTE";

                    MunicipioCP = MunicipioCP.ToUpper();
                    MunicipioCP = MunicipioCP.Replace("Á", "A");
                    MunicipioCP = MunicipioCP.Replace("É", "E");
                    MunicipioCP = MunicipioCP.Replace("Í", "I");
                    MunicipioCP = MunicipioCP.Replace("Ó", "O");
                    MunicipioCP = MunicipioCP.Replace("Ú", "U");

                    CiudadCP = CiudadCP.ToUpper();
                    CiudadCP = CiudadCP.Replace("Á", "A");
                    CiudadCP = CiudadCP.Replace("É", "E");
                    CiudadCP = CiudadCP.Replace("Í", "I");
                    CiudadCP = CiudadCP.Replace("Ó", "O");
                    CiudadCP = CiudadCP.Replace("Ú", "U");

                    // Actualmente solo se valida con el ultimo CP localizado, si un CP regresa mas de un registro no
                    // se toman en cuenta los registros iniciales.

                    // 02) Si el Estado del catalogo y el Estado del CP no coinciden se rechaza el Registro.    Rechazado
                    if (EstadoCP != Estado)
                    { return "02"; }

                    // 03) EstadoCP = EstadoR      MunicipioCP != MunicipioR       CiudadCP != CiudadR          Rechazado
                    // Si el Municipio y la Ciudad son reportadas y ambas son diferentes de las que indica el CP se marca como Warning
                    if (EstadoCP == Estado && MunicipioCP != Municipio && CiudadCP != Ciudad && Municipio != string.Empty && Ciudad != string.Empty)
                    { return "03"; }

                    // 04) EstadoCP = EstadoR       MunicipioCP != MunicipioR                                   Rechazado
                    // Si el municipio es reporteado y es diferente del CP y la ciudad no es reportada, se marca como Warning
                    if (EstadoCP == Estado && MunicipioCP != Municipio && Ciudad == string.Empty && Municipio != string.Empty)
                    { return "04"; }

                    // 05) EstadoCP = EstadoR                                       CiudadCP != CiudadR         Rechazado
                    // Si la ciudad es diferente del CP y el municipio no es reportado, se marca como Warning
                    if (EstadoCP == Estado && Municipio == string.Empty && CiudadCP != Ciudad && Ciudad != string.Empty)
                    { return "05"; }

                    // 06) EstadoCP = EstadoR       MunicipioCP = MunicipioR        CiudadCP = CiudadR          Aceptado
                    // Si los tres datos coinciden el registro se acepta
                    if (EstadoCP == Estado && MunicipioCP == Municipio && CiudadCP == Ciudad && Municipio != string.Empty && Ciudad != string.Empty)
                    { return "06"; }

                    // 07) EstadoCP = EstadoR       MunicipioCP = MunicipioR                                    Aceptado
                    // Si el municipio es igual al CP y la ciudad no se reporta, el registro se acepta
                    if (EstadoCP == Estado && MunicipioCP == Municipio && Ciudad == string.Empty && Municipio != string.Empty)
                    { return "07"; }

                    // 08) EstadoCP = EstadoR                                       CiudadCP = CiudadR          Aceptado
                    // Si la ciudad es igual al CP y el municipio no se reporta, el registro se acepta.
                    if (EstadoCP == Estado && Municipio == string.Empty && CiudadCP == Ciudad && Ciudad != string.Empty)
                    { return "08"; }

                    // 09) EstadoCP = EstadoR       MunicipioCP != MunicipioR       CiudadCP = CiudadR          Aceptado
                    // Si el municipio es diferente pero las ciudades coinciden el registro se acepta
                    if (EstadoCP == Estado && MunicipioCP != Municipio && CiudadCP == Ciudad && Municipio != string.Empty && Ciudad != string.Empty)
                    { return "09"; }

                    // 10) EstadoCP = EstadoR      MunicipioCP = MunicipioR        CiudadCP != CiudadR          Aceptado
                    // Si el estado y el municipio coinciden con CP, pero la ciudad no, el registro se acepta
                    if (EstadoCP == Estado && MunicipioCP == Municipio && CiudadCP != Ciudad && Municipio != string.Empty && Ciudad != string.Empty)
                    { return "10"; }
                }

                return "00";
            }
            catch
            {
                return "00";
            }
        }

        public static String StrCP(string value, int size)
        {
            if (value.Trim().Length > size)
                value = value.Trim().Substring(0, size);

            string v = "";
            if (!String.IsNullOrWhiteSpace(value))
                v = value.Trim();

            return v.PadLeft(size, '0');
        }

        public static bool? ComparaFecha(string inputDDMMAAAA, string compareDDMMAAAA, int addDays)
        {
            DateTime inputDT = ToDateTime(inputDDMMAAAA);
            DateTime compareDT = ToDateTime(compareDDMMAAAA);

            if (inputDT != default(DateTime) && compareDT != default(DateTime))
            {
                return (inputDT >= compareDT.AddDays(addDays));
            }
            return null;
        }

        /// <summary>
        /// Verifica si el codigo de Banxico es un codigo válido en el catálogo.
        /// </summary>
        /// <param name="codigoBanxico">Indica el código Banxico</param>
        /// <param name="opcional">Indica si el campo puede ser vacio.</param>
        /// <returns></returns>
        public static bool V_Banxico(string codigoBanxico, bool opcional)
        {
            int result;
            int.TryParse(codigoBanxico, out result);

            if (String.IsNullOrWhiteSpace(codigoBanxico) || result == 0)
                return opcional;
            else if (result == 9999999)
                return true;

            BanxicoDataAccess banxicos = new BanxicoDataAccess();
            Banxico banxico = banxicos.GetItemByClave(result);

            return banxico != null && banxico.Estatus == Enums.Estado.Activo;
        }

        public static ValidacionEntry GetValidacionEntry(string codigoError, List<Validacion> validaciones, string dato, ISegmentoType aRechazar, string rfc, string credito)
        {
            Validacion validacion = Util.GetValidacion(validaciones, codigoError);

            int validacionId = (validacion != null) ? validacion.Id : 0;
            ErrorWarning error = new ErrorWarning(0, validacionId, rfc, credito, dato, "");

            ValidacionEntry entry = new ValidacionEntry(error, aRechazar);

            return entry;
        }

        public static ValidacionEntry GetValidacionEntry(string codigoError, List<Validacion> validaciones, string dato, ISegmentoType aRechazar)
        {
            return GetValidacionEntry(codigoError, validaciones, dato, aRechazar, string.Empty, string.Empty);
        }

        public static ValidacionEntry GetValidacionEntry(string codigoError, List<Validacion> validaciones, string dato, ISegmentoType aRechazar, string rfc)
        {
            return GetValidacionEntry(codigoError, validaciones, dato, aRechazar, rfc, string.Empty);
        }

        #region Converter

            public static DateTime ToDateTime(string toValidate)
            {
                //Recibe siempre DDMMAAAA
                if (String.IsNullOrWhiteSpace(toValidate) || Parser.ToNumber(toValidate) == 0)
                    return new DateTime();

                string format = @"(\d{2})(\d{2})(\d{4})";
                if (Regex.IsMatch(toValidate, format))
                {
                    Match m = Regex.Match(toValidate, format);
                    int dd, mm, aaaa;

                    int.TryParse(m.Groups[1].Value, out dd);
                    int.TryParse(m.Groups[2].Value, out mm);
                    int.TryParse(m.Groups[3].Value, out aaaa);

                    return new DateTime(aaaa, mm, dd);
                }
                return new DateTime();
            }

        #endregion

        #region Catalogos Generico

            public static String GetFrecuencia(string codigo_Ext, string defaultValue)
            {
                FrecuenciasPagoDataAccess frecuencias = new FrecuenciasPagoDataAccess();
                Frecuencia_Pago freq = frecuencias.GetItemByClave(codigo_Ext);

                if (freq != default(Frecuencia_Pago))
                    return freq.ClaveSIC;
                else
                    return defaultValue;
            }

            public static String GetNumPagos(string codigo_Ext, string defaultValue)
            {
                NumPagosDataAccess pagos = new NumPagosDataAccess();
                Num_Pago pago = pagos.GetItemByClave(codigo_Ext);

                if (pago != default(Num_Pago))
                    return pago.ClaveSIC;
                else
                    return defaultValue;
            }

            public static String GetPais(int codigo_Ext, string defaultValue)
            {
                PaisesDataAccess paises = new PaisesDataAccess(Enums.Persona.Moral);
                Pais pais = paises.GetItemByClave(codigo_Ext);

                if (pais != default(Pais))
                    return pais.ClaveBuro;
                else
                    return defaultValue;
            }

            public static String GetMoneda(string codigo_Ext, string defaultValue)
            {
                MonedasDataAccess monedas = new MonedasDataAccess();
                Moneda moneda = monedas.GetItemByClave(codigo_Ext);

                if (moneda != default(Moneda))
                    return moneda.ClaveBuro.ToString();
                else
                    return defaultValue;
            }

            public static String GetEstado(int codigo_Ext, string defaultValue)
            {
                EstadosDataAccess estados = new EstadosDataAccess(Enums.Persona.Moral);
                Estado estado = estados.GetItemByClave(codigo_Ext);

                if (estado != default(Estado))
                    return estado.ClaveBuro;
                else
                    return defaultValue;
            }

            public static String GetCredito(string clave, string defaultValue)
            {
                CreditosDataAccess creditos = new CreditosDataAccess();
                Entities.TipoCredito credito = creditos.GetItemByClave(clave);

                if (credito == default(Entities.TipoCredito))
                    return defaultValue;
                else
                    return credito.tipoCredito;
            }

            public static String GetInstitucion(int clave, string defaultValue)
            {
                InstitucionesDataAccess instituciones = new InstitucionesDataAccess();
                Institucion inst = instituciones.GetItemByClave(clave);

                if (inst == default(Institucion))
                    return defaultValue;
                else
                    return inst.ClaveExterna.ToString();
            }

            public static String GetCalifCartera(string clave, string defaultValue)
            {
                CalificacionesDataAccess calificaciones = new CalificacionesDataAccess();
                Calificacion cal = calificaciones.GetItemByClave(clave);

                if (cal == default(Calificacion))
                    return defaultValue;
                else
                    return cal.ClaveBuro;
            }

            public static String GetExceptuados(string clave, string defaultValue)
            {
                ExceptuadosDataAccess exceptuados = new ExceptuadosDataAccess();
                Exceptuado excep = exceptuados.GetItemByClave(clave);

                if (excep == default(Exceptuado))
                    return defaultValue;
                else
                    return excep.Credito;
            }

            public static String GetFrecuencias(string clave, string defaultValue)
            {
                return defaultValue;
            }

            public static String GetCalificacionAltoRiesgo(string RFC_RIESGO) 
            {
                V_SICALCDataAccess accesoCalificacion = new V_SICALCDataAccess();
                return accesoCalificacion.GetCalificacionAltoRiesgoAccess(RFC_RIESGO);
            }

            public static List<SEPOMEX> ConsultaCPSepomex(string CodigoPostal)
            {
                SEPOMEXRules getRecords = null;
                List<SEPOMEX> SEPOMEXInfo = null;

                try
                {
                    getRecords = new SEPOMEXRules();
                    var s = getRecords.GetSepomexPorCP(CodigoPostal, true);
                    SEPOMEXInfo = s;
                }
                catch
                { }

                return SEPOMEXInfo;
            }

            public static List<Estado> ConsultarEstado(string ClaveEstado, Enums.Persona TipoPersona)
            {
                Estados_Rules getRecords = null;
                List<Estado> EstadosInfo = null;

                string CadenaTipoPersona = "";
                try
                {
                    if (TipoPersona == Enums.Persona.Moral)
                    { CadenaTipoPersona = "M"; }
                    else
                    { CadenaTipoPersona = "F"; }

                    getRecords = new Estados_Rules(TipoPersona);
                    var s = getRecords.GetEstadoPorClaveBuro(CadenaTipoPersona, "0", ClaveEstado, true);
                    EstadosInfo = s;
                }
                catch
                { }

                return EstadosInfo;
            }

            public static List<Pais> ConsultarPais(string ClavePais)
            {
                PaisRules getRecords = null;
                List<Pais> PaisesInfo = null;

                try
                {
                    getRecords = new PaisRules();
                    var s = getRecords.GetPaisPorClaveBuro(Enums.Persona.Moral, "0", ClavePais, true);
                    PaisesInfo = s;
                }
                catch
                { }

                return PaisesInfo;
            }

        #endregion

    }
}
