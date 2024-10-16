using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Vistas;
using Banobras.Credito.SICREB.Data.Vistas;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class CarteraRules
    {
        private List<Cartera> allRecords = null;
        private Enums.Persona persona;
        private Enums.Reporte reporte;

        public CarteraRules(Enums.Persona persona, Enums.Reporte reporte)
        {
            this.persona = persona;
            this.reporte = reporte;
        }

        private List<Cartera> GetCarteras(string periodo)
        {
            if (allRecords == null || allRecords.Count == 0)
            {
                CarteraDataAccess cars = new CarteraDataAccess(persona);
                
                allRecords = cars.GetRegistros(periodo, this.reporte);
            }
            return allRecords;
        }

        public List<Cartera> GetCarterasPorRfc(string rfc, string periodo)
        {
            var cars = (from car in GetCarteras(periodo)
                        where car.Rfc.Trim().Equals(rfc.Trim(), StringComparison.InvariantCultureIgnoreCase)
                        select car).ToList();
            if (cars == null)
                cars = new List<Cartera>();
            
            return cars;
        }
    }
}
