using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Data.Vistas;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Entities.Types.PF;
using System.Data;

namespace Banobras.Credito.SICREB.Business.Rules
{
    public class ReestructuradoRules
    {
        private Enums.Persona persona;
        //private Enums.Reporte reporte;
        //private int archivoId;

        private ReestructuradoDataAccess rData = null;
        private ArchivosDataAccess archivosData = null;

        public ReestructuradoRules(Enums.Persona persona)
        {
            this.persona = persona;

            //this.archivoId = archivoId;
            this.archivosData = new ArchivosDataAccess(persona);

        }
        

        public DataTable GetReestructuradosPresentacion()
        {
            rData = new ReestructuradoDataAccess();
            return rData.GetReestructuradoInfo();
        }

        public List<int> GetAniosDeArchivos()
        {
            var anios = (from arc in archivosData.GetAllArchivos()
                         orderby arc.Fecha descending
                         select arc.Fecha.Year).Distinct().ToList();
            return anios;
        }
        public List<Archivo> GetArchivosPorAnio(int anio)
        {
            var archivos = (from arc in archivosData.GetAllArchivos()
                            where arc.Fecha.Year.Equals(anio)
                            orderby arc.Fecha descending
                            select arc).ToList();

            return archivos;
        }
    }
}
