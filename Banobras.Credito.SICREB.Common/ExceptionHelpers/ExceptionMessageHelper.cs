using System.Text;

namespace Banobras.Credito.SICREB.Common.ExceptionHelpers
{

    public static class ExceptionMessageHelper
    {

        /// <summary>
        /// Obtener mensaje de excepción
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetExceptionMessage(System.Exception exception)
        {
            StringBuilder sb = new StringBuilder(exception.Message);
            while (exception != null && exception.InnerException != null)
            {
                sb.Append(" " + exception.InnerException.Message);
                exception = exception.InnerException;
            }

            return sb.ToString();
        }

    }

}
