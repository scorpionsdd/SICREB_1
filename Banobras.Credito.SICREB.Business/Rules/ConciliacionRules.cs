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

namespace Banobras.Credito.SICREB.Business.Rules
{
    public class ConciliacionRules
    {
        private Enums.Persona persona;
        //private Enums.Reporte reporte;
        //private int archivoId;
        private ConciliacionDataAccess conciliacionData = null;
        private ArchivosDataAccess archivosData = null;
        
        public ConciliacionRules(Enums.Persona persona)
        {
            this.persona = persona;
            
            //this.archivoId = archivoId;
            this.archivosData = new ArchivosDataAccess(persona);

        }
        

        public List<ConciliacionInfo> GetConciliacionPresentacion(int archivoId)
        {
            conciliacionData = new ConciliacionDataAccess();

            return conciliacionData.GetConcliacionInfo(this.persona, archivoId);
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
