using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data.Guias_Contables;

namespace Banobras.Credito.SICREB.Business.Guias_Contables
{
    public class Sicofin2Business
    {
        Sicofin2DataAccess Sicofin2Data = null;

        public void GeneraSicofin()
        {
            Sicofin2Data = new Sicofin2DataAccess();
            Sicofin2Data.GeneraSICOFIN2();
        }

        public void GeneraSicofinSemanal()
        {
            Sicofin2Data = new Sicofin2DataAccess();
            Sicofin2Data.GeneraSICOFIN2Semanal();
        }

        public void GeneraSicofinMensual()
        {
            Sicofin2Data = new Sicofin2DataAccess();
            Sicofin2Data.GeneraSICOFIN2Mensual();
        }
    }
}
