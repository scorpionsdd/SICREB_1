using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;

namespace Banobras.Credito.SICREB.Common.ExceptionHelpers
{
    public class ExceptionHandling
    {
        static readonly ExceptionHandling _exception = null;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ExceptionHandling()
        {
            _exception = new ExceptionHandling();
        }
        ExceptionHandling()
        {
        }

        public static ExceptionHandling Instance
        {
            get
            {
                return _exception;
            }
        }

        private ExceptionManager exceptionManager;
        private LogWriter logWrt;

        [Dependency]
        public LogWriter LogWrt
        {
            get
            {
                return _exception.logWrt;
            }
            set
            {
                _exception.logWrt = value;
            }
        }

        [Dependency]
        public ExceptionManager ExceptionMgr
        {
            get
            {
                return _exception.exceptionManager;
            }
            set
            {
                _exception.exceptionManager = value;
            }
        }

    }
}
