using System;

namespace Banobras.Credito.SICREB.Entities.Types
{
    public class PM_SegmentoException : Exception
    {
        private string message = null;

        public override string Message
        {
            get
            {
                if (message == null)
                    message = string.Empty;

                return message;
            }
        }

        public PM_SegmentoException(string message)
        {
            this.message = message;
        }
    }
}
