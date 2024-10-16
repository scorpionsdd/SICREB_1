using System;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Data;

namespace Banobras.Credito.SICREB.Business
{
    public class FechasVistasRules
    {
        public List<DateTime> FechaVista()
        {
            FechaDatosDataAccess fdda = new FechaDatosDataAccess();
            return fdda.FechasVistas();
        }
    }
}
