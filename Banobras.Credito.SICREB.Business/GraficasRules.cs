using System;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
namespace Banobras.Credito.SICREB.Business
{
    public class GraficasRules
    {

        public Archivo EstadisticasArchivo(string persona, Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica Store)
        {
            GraficasDataAccess gda = new GraficasDataAccess();
            return gda.ConsultarEstadisticasArchivo(persona, Store);
        }
        public List<Archivo> HistoricoArchivo(string persona, Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica Store, out List<Archivo> HistoricosAdvertencias)
        {
            GraficasDataAccess gda = new GraficasDataAccess();
            return gda.ConsultarHistorico( persona,  Store,out  HistoricosAdvertencias);
        }
        public List<int> ConciliacionArchivo(int idArchivoPersonasMorales,int idArchivoPersonasFisica)
        {
            GraficasDataAccess gda = new GraficasDataAccess();
            List<int> DatosConciliacionArchivo = new List<int>();
            try
            {
                
                
                DatosConciliacionArchivo.Add(gda.ConsultarConciliacionCorrectos(idArchivoPersonasFisica) + gda.ConsultarConciliacionCorrectos(idArchivoPersonasMorales));
                DatosConciliacionArchivo.Add(gda.ConsultarConciliacionDiferencias(idArchivoPersonasFisica) + gda.ConsultarConciliacionDiferencias(idArchivoPersonasMorales));
                DatosConciliacionArchivo.Add(gda.ConsultarConciliacionError(idArchivoPersonasFisica) + gda.ConsultarConciliacionError(idArchivoPersonasMorales));
                DatosConciliacionArchivo.Add(gda.ConsultarConciliacionExceptuados(idArchivoPersonasFisica) + gda.ConsultarConciliacionExceptuados(idArchivoPersonasMorales));
                DatosConciliacionArchivo.Add(gda.ConsultarConciliacionInvestigar(idArchivoPersonasFisica) + gda.ConsultarConciliacionInvestigar(idArchivoPersonasMorales));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }
            return DatosConciliacionArchivo;
        }

        //SICREB-INICIO-VHCC SEP-2012
        public List<Archivo> HistoricoArchivo(string persona, Banobras.Credito.SICREB.Entities.Util.Enums.StoreGrafica Store, DateTime dFechaInicio, DateTime dFechaFin, out List<Archivo> HistoricosAdvertencias)
        {
            GraficasDataAccess gda = new GraficasDataAccess();
            return gda.ConsultarHistorico(persona, Store, dFechaInicio, dFechaFin, out HistoricosAdvertencias);
        }
        //SICREB-FIN-VHCC SEP-2012
    }
}

