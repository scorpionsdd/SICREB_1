using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Auxiliares
    {
        public int Id { get; private set; }
        public string Credito { get; private set; }
        public string Auxiliar { get; private set; }
        public Enums.Estado Estatus { get; private set; }
        public string RFC { get; set; }
        public string Nombre { get; set; }
        /// <summary>
        /// Entidad de catálogo Creditos Auxiliares
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="Credito">Varchar (50)</param>
        /// <param name="Auxiliar">Varchar(100) Descripción</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public Auxiliares(int id, string credito, string auxiliar, Enums.Estado estatus)
        {
            this.Id = id;
            this.Credito = credito;
            this.Auxiliar = auxiliar;
            this.Estatus = estatus;
        }
        public Auxiliares(int id, string credito, string auxiliar,string rfc,string nombre, Enums.Estado estatus)
        {
            this.Id = id;
            this.Credito = credito;
            this.Auxiliar = auxiliar;
            this.Estatus = estatus;
            this.RFC = rfc;
            this.Nombre = nombre;
        }
    }
}
