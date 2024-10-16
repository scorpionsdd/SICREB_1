using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Transaccionales;
using System.Data;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class Inconsistencias_Rules
    {
        private Enums.Persona persona;
        private int iArchivoId;
        private DateTime fdFecha;

        private InconsistenciasDataAccess data = null;

        public Inconsistencias_Rules(Enums.Persona persona, int archivoId, DateTime fecha)
        {
            this.persona = persona;
            this.iArchivoId = archivoId;
            this.fdFecha = fecha;
            data = new InconsistenciasDataAccess(this.persona, this.iArchivoId, this.fdFecha);
        }

        public Inconsistencias_Rules()
        {
        }

        public void LlenarInconsistencias()
        {
            InconsistenciasDataAccess datos = new InconsistenciasDataAccess();
            datos.LlenarInconsistencias();
        }

        public DataTable GetInconsistencias()
        {
            return data.GetInconsistencias();
        }
    }
}
