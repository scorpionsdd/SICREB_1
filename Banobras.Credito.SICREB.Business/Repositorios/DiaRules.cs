using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class DiaRules
    {
        public List<Dia> Dias()
        {
            DiaDataAccess dda = new DiaDataAccess();
            return dda.GetRecords(true);
        }

        public int InsertarDia(Dia dia)
        {
            DiaDataAccess dda = new DiaDataAccess();
            return dda.InsertRecord(dia);
        }
        public int ActulizarDia(Dia dia)
        {
            DiaDataAccess dda = new DiaDataAccess();
            return dda.UpdateRecord(null, dia);
        }
        public int BorrarDia(Dia dia)
        {
            DiaDataAccess dda = new DiaDataAccess();
            return dda.DeleteRecord(dia);
        }

    }//class DiaRules
}
