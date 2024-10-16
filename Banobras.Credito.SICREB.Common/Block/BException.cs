using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Unity;

namespace Banobras.Credito.SICREB.Common.Block
{
    public class BException
    {

        static readonly BException _exception = new BException();

        static BException()
        {

        }

        public static BException Instance
        {
            get
            {
                return _exception;
            }
        }


        private ExceptionManager exceptionManager;

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
