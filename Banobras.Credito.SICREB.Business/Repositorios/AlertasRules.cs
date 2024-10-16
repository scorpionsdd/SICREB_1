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
    public class AlertasRules
    {
        public List<Alerta> Alertas()
        {
            AlertasDataAccess ada = new AlertasDataAccess();
            return ada.GetRecords(true);
        }

        public int InsertarAlerta(Alerta alerta)
        {
            AlertasDataAccess ada = new AlertasDataAccess();
            return ada.InsertRecord(alerta);
        }
        public int ActulizarAlerta(Alerta alerta)
        {
            AlertasDataAccess ada = new AlertasDataAccess();
            return ada.UpdateRecord(null, alerta);
        }
        public int BorrarAlerta(Alerta Alerta)
        {
            AlertasDataAccess ada = new AlertasDataAccess();
            return ada.DeleteRecord(Alerta);
        }
    }
}
