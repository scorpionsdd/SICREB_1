using System;

namespace Banobras.Credito.SICREB.Entities.Types
{
    public class PM_RegistroException : Exception
    {
        private string message = null;

        public override string Message
        {
            get
            {
               
                if (message == null)
                    message = String.Empty;
                return message;
            }
        }

        public PM_RegistroException(string message)
        {
            this.message = message;
        }
    }
}
