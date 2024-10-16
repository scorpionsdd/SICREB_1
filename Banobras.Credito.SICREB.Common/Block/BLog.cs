using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using Banobras.Credito.SICREB.Common.HttpUnity;

namespace Banobras.Credito.SICREB.Common.Block
{
    public class BLog
    {

        static readonly BLog _bLog = new BLog();

        static BLog()
        {

        }

        public static BLog Instance
        {
            get
            {
                return _bLog;
            }
        }


        public static BLog Current
        {
            get
            {
                
                BLog log = null;

                HttpApplicationState app = HttpContext.Current != null ? HttpContext.Current.Application : null;
                
                if (app != null)
                {
                    try
                    {
                        //IUnityContainer container = app.GetContainer();
                        //log = (BLog)container.Resolve(typeof(BLog));
                        //30 de octubre

                        //log = new BLog();

                    }
                    catch (Exception ex) {
                        string mgs = ex.Message;
                    }
                }
                else
                {
                    log = new BLog();
                    log.LogWrtMrg = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
                }

                return log;

            }
        }

        
        private LogWriter logWrtMgr;

        [Dependency]
        public LogWriter LogWrtMrg
        {
            get
            {
                return _bLog.logWrtMgr;
            }
            set
            {
                _bLog.logWrtMgr = value;
            }
        }

    }
}
