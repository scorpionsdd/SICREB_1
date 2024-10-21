using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Common.Exception
{
    public class ExceptionManagerCustom:System.Exception
    {
        private Guid _id;
        public ExceptionManagerCustom() : base("Se ha producido un error.") {
            string result = getMessage(base.GetBaseException());
            _id = Guid.NewGuid();
            setLog(string.Format("Identificador {0}. {1}", _id, result));
        }
        public ExceptionManagerCustom(string message) : base(message) {
            string result = getMessage(base.GetBaseException());
            _id = Guid.NewGuid();
            setLog(string.Format("Identificador {0}. {1}", _id, result));
        }
        public ExceptionManagerCustom(string message, System.Exception innerException) : base(message, innerException) {
            string result = getMessage(base.GetBaseException());
            _id = Guid.NewGuid();
            setLog(string.Format("Identificador {0}. {1}", _id, result));
        }
        public override string ToString()
        {
            return string.Format("Se detecto un error. Se ha generado un Log. Identificador {0}.", _id);
        }
        private string getMessage(System.Exception exception) {
            string result = string.Empty;
            result = (exception.InnerException != null ? string.Format("Error:{0}{1}Detalle:{2}", exception.Message, Environment.NewLine, getMessage(exception.InnerException)) : exception.Message);
            return result;
        }
        private void setLog(string message){
            string path = string.Format("LogSRT_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format("{0}: {1}",DateTime.Now.ToString("yyyy/MM/ddTHH:mm:ss"),message));
            }
        }
    }
}
