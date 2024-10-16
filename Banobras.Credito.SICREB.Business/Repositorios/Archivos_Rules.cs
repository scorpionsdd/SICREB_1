using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class Archivos_Rules
    {
        private ArchivosDataAccess data = null;
        private Enums.Persona persona;

        public Archivos_Rules(Enums.Persona persona)
        {
            this.persona = persona;
            data = new ArchivosDataAccess(persona);
        }

        public Archivo GetUltimoArchivo()
        {
            return data.GetUltimoArchivo();
        }

        public List<Archivo> GetAllArchivos()
        {
            return data.GetAllArchivos();
        }

        public string GetUltimoArchivoGL()
        {
            // Metodo creado en Enero de 2016 para Lineas y Garantias
            return data.GetUltimoArchivoGL();
        }

        public string GetFechaUltimoProcesoGL()
        {
            // Metodo creado en Enero de 2016 para Lineas y Garantias
            return data.GetFechaUltimoProcesoGL();
        }

        public string ProcesarArchivoGL()
        {
            // Metodo creado en Enero de 2016 para Lineas y Garantias
            return data.ProcesarArchivoGL();
        }

    }
}
