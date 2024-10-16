using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Common.ExceptionMng
{

    public static class ExceptionMessageHelper
    {

        /// <summary>
        /// Obtener mensaje de excepción
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception exception)
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
