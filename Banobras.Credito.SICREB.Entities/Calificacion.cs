using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Calificacion
    {
        public int Id { get; private set; }
        public string ClaveBuro { get; private set; }
        public string Descripcion { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Entidad de catálogo Calificaciones
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="claveBuro">Varchar(20) Clave Buró</param>
        /// <param name="descripcion">Varchar(300) Descripción</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public Calificacion(int id, string claveBuro, string descripcion, Enums.Estado estatus)
        {
            this.Id = id;
            this.ClaveBuro = claveBuro;
            this.Descripcion = descripcion;
            this.Estatus = estatus;
        }
    }
}
