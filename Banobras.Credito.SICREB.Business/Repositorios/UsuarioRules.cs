using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Common.ExceptionHelpers;
using System;
using System.Configuration;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class UsuarioRules
    {
        public UsuarioRules()
        {         
        }

        /// <summary>
        /// Verificando si las credenciales del usuario deben buscarse en el DA
        /// </summary>
        /// <param name="nombreUsuario">Login de usuario en DA</param>
        /// <param name="password">Contraseña d usuario en DA</param>
        /// <returns></returns>
        public bool UsuarioValido(string nombreUsuario, string password)
        {
            bool validado = false;
            string nombre = null;
            string mail = null;
            bool verificaAD = false;

            try
            {
                verificaAD = (bool)ConfigurationManager.AppSettings["AuthAD"].ToLower().Equals("true");

                if (verificaAD)
                {
                    validado = ActiveDir.AutenticaUsuario(nombreUsuario, password, ref nombre, ref mail);
                }
                else
                {
                    validado = true;
                }
            }
            catch(Exception ex)
            {
                //throw new Exception(ex.Message);
                string message = ExceptionMessageHelper.GetExceptionMessage(ex);
            }

            return validado;
        }

        public string GetVariable(string var)
        {
            string toReturn = string.Empty;
            if (ConfigurationManager.AppSettings[var] != null)
            {
                toReturn = ConfigurationManager.AppSettings[var].ToString();
            }

            return toReturn;
        }
        
    }
}
