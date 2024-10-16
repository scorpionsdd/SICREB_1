using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Util;
using Telerik.Web.UI;
using System.Configuration;
using Newtonsoft.Json;

namespace Banobras.Credito.SICREB.Common
{
    public static class Util
    {
        public const string FORMATO_FECHA = "ddMMyyyy";        
        private static List<Etiqueta> allEtiquetas;

        #region Enums 

        public static Enums.Persona GetPersona(char persona)
        {

            switch (persona.ToString().ToUpper())
            {
                case "M": return Enums.Persona.Moral;
                case "F": return Enums.Persona.Fisica;
                case "O": return Enums.Persona.Fideicomiso;
                case "G": return Enums.Persona.Gobierno;
                default : return Enums.Persona.Moral;
            }
        }

        public static string SetPersona(string pPersona)
        {
            switch (pPersona)
            {
                case "Moral": return "M";
                case "Fisica": return "F";
                case "Fideicomiso": return "O";
                case "Gobierno": return "G";
                default: return "M";
            }
        }

        public static Enums.Estado GetEstado(char estado)
        {
            switch (estado)
            { 
                case '1' :
                    return Enums.Estado.Activo;
                case '0' :
                    return Enums.Estado.Inactivo;
                default :
                    return Enums.Estado.Test;
            }
        }
        public static string SetEstado(string pEstado)
        {
            if (pEstado == "Activo")
            { 
                return "1";
            }
            else if (pEstado =="Inactivo")
            {
                return "0";
            }
            else
                return "T";
        }

        public static Enums.TipoOperacion GetTipoOperacion(char operacion)
        {
            switch (operacion)
            {
                case 'C':
                    return Enums.TipoOperacion.Credito;
                case 'L':
                    return Enums.TipoOperacion.Linea;
                default:
                    return Enums.TipoOperacion.Credito;
            }
        }

        public static string SetTipoOperacion(string pOperacion)
        {
            if (pOperacion == "Credito")
            {
                return "C";
            }
            else if (pOperacion == "Linea")
            {
                return "L";
            }
            else
                return "C";
        }


        #endregion

        #region Entidades por listas

        public static ISegmentoType GetCinta(ISegmentoType entidad, Enums.Persona persona)
        {
            ISegmentoType cinta = entidad;
            while (cinta != null && (cinta as PM_Cinta) == null && (cinta as PF_Cinta) == null)
            {
                cinta = cinta.Parent;
            }            
            return cinta;
        }
        public static List<Etiqueta> EtiquetasPorSegmento(ISegmentoType parent, string codigoSegmento)
        {
            Type type = parent.GetType();
            List<Segmento> segmentos = null;
            if (type == typeof(PM_Cinta))
                segmentos = ((PM_Cinta)parent).Segmentos;
            else if (type == typeof(PF_Cinta))
                segmentos = ((PF_Cinta)parent).Segmentos;


            if (segmentos != null && segmentos.Count > 0)
            {
                
                int idSegmento = (from seg in segmentos
                                  where seg.Codigo.Equals(codigoSegmento, StringComparison.InvariantCultureIgnoreCase)
                                  select seg.Id).SingleOrDefault();

                return (from etiq in parent.Etiquetas
                        where etiq.SegmentoId == idSegmento
                        select etiq).ToList();
            }
            return new List<Etiqueta>();
        }
        public static List<Validacion> ValidacionesPorSegmento(Enums.Persona persona, ISegmentoType parent, string codigoSegmento)
        {
            ISegmentoType cinta = parent as PM_Cinta;
            

            List<Validacion> toReturn = new List<Validacion>();
            if (cinta != null)
            {
                List<Etiqueta> etiquetasSegmento = EtiquetasPorSegmento(cinta, codigoSegmento);

                foreach (Etiqueta etiq in etiquetasSegmento)
                {
                    var val = (from va in cinta.Validaciones
                               where va.Etiqueta_Id == etiq.Id
                               select va).ToList();
                    if (val != null)
                    {
                        foreach (Validacion v in val)
                        {
                            toReturn.Add(v);
                        }
                    }
                }
            }
            return toReturn;
        }
        public static Etiqueta GetEtiqueta(List<Etiqueta> etiquetas, string etiqueta)
        {
            Etiqueta et = (from e in etiquetas
                           where e.Codigo.Equals(etiqueta, StringComparison.InvariantCultureIgnoreCase)
                           select e).FirstOrDefault();
            return et;
        }
        public static Etiqueta GetEtiqueta(List<Etiqueta> etiquetas, int id)
        {
            Etiqueta et = etiquetas.Where(e => e.Id == id).FirstOrDefault();

            return et;
        }
        //JAGH 31/01/13
        public static Validacion GetValidacion(List<Validacion> validaciones, string codigoError)
        {
            Validacion val = null;

            if (validaciones != null)
            {
                val = (from va in validaciones
                       where va.Codigo.Equals(codigoError, StringComparison.InvariantCultureIgnoreCase)
                       select va).FirstOrDefault();
            }
            
            return val;
        }
        public static Validacion GetValidacion(List<Validacion> validaciones, int validacionId)
        {
            Validacion val = (from va in validaciones
                              where va.Id.Equals(validacionId)
                              select va).FirstOrDefault();
            return val;
        }
        public static String GetValorArchivo(List<ValorArchivo> valores, List<Etiqueta> etiquetas, string etiqueta)
        {
            Etiqueta et = GetEtiqueta(etiquetas, etiqueta);

            if (et != default(Etiqueta))
            {
                var toReturn = valores.Where(v => v.EtiquetaId == et.Id).FirstOrDefault();
                if (toReturn != default(ValorArchivo))
                {
                    return toReturn.Valor;
                }
            }
            return string.Empty;
        }

        #endregion

        public static ArrayList RadComboToString(object ComboBox)
        {
            ArrayList datos = new ArrayList();
            RadComboBox dato = (RadComboBox)ComboBox;
            datos.Add(dato.Text);
            datos.Add(dato.SelectedValue);
            return datos; 
        }

        /// <summary>
        /// Obteniendo los valores de la variable de ambiente declarada en el SO anfitrión
        /// </summary>
        /// <returns></returns>
        public static EnvironmentVariables GetEnvironmentVariableValues()
        {
            EnvironmentVariables values = new EnvironmentVariables();

            //Nombre de la variable de ambiente definida en el SO anfitrión
            string environmentVariable = ConfigurationManager.AppSettings["EnvironmentVariable"].ToString();
            //Obteniendo el contenido de la variable de ambiente: par | valor
            string environmentVariableValue = Environment.GetEnvironmentVariable(environmentVariable);
            if (!string.IsNullOrEmpty(environmentVariableValue))
            {
                //Convirtiendo el contenido en una entidad
                values = JsonConvert.DeserializeObject<EnvironmentVariables>(environmentVariableValue);
            }

            return values;
        }

    }
}
