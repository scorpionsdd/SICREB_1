using System;

namespace Banobras.Credito.SICREB.Entities.Util
{

    public static class Parser
    {

        public const string FORMATO_FECHA = "ddMMyyyy";

        public static int ToNumber(object toConvert)
        {
            if (toConvert != null)
            {
                int result;
                int.TryParse(toConvert.ToString(), out result);
                return result;
            }

            return 0;
        }

        public static DateTime ToDateTime(object toConvert)
        {
            if (toConvert == null)
                return default(DateTime);
            
            if (toConvert.ToString().Length == 8)
            {
                DateTime resultado;
                DateTime.TryParse(string.Format("{0}/{1}/{2}", toConvert.ToString().Substring(0, 2), toConvert.ToString().Substring(2, 2), toConvert.ToString().Substring(4, 4)), out resultado);
                return resultado;
            }

            DateTime result;
            DateTime.TryParse(toConvert.ToString(), out result);
            
            return result;
        }

        /// <summary>
        /// Usa formato ddMMyyyy
        /// </summary>
        /// <param name="toConvert"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object toConvert, string format)
        {
            if (toConvert == null)
                return new DateTime();

            DateTime result;
            DateTime.TryParseExact(toConvert.ToString(), format, null, System.Globalization.DateTimeStyles.None, out result);
            
            return result;
        }

        public static Char ToChar(object toConvert)
        {
            if (toConvert == null)
                return default(Char);

            Char result;
            char.TryParse(toConvert.ToString(), out result);
            
            return result;
        }

        public static Double ToDouble(object toConvert)
        {
            if (toConvert == null)
                return default(double);

            double result;
            double.TryParse(toConvert.ToString(), out result);
            
            return result;
        }

    }

}
