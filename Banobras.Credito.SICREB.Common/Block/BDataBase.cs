using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.Unity;

namespace Banobras.Credito.SICREB.Common.Block
{
    public class BDataBase
    {

        static readonly BDataBase _bDB = new BDataBase();

        static BDataBase()
        {

        }

        public static BDataBase Instance
        {
            get
            {
                return _bDB;
            }
        }

        private Database dataBaseMgr;

        [Dependency]
        public Database DataBaseMgr
        {
            get
            {
                return _bDB.dataBaseMgr;
            }
            set
            {
                _bDB.dataBaseMgr = value;
            }
        }

    }
}
