
namespace Banobras.Credito.SICREB.Entities
{
    public class Clasificacion
    {
        public int Id { get; private set; }
        public string Descripcion { get; private set; }

        /// <summary>
        /// Entidad de catálogo Clasificaciones
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="descripcion">Varchar(300) Descripción</param>
        public Clasificacion(int id, string descripcion)
        {
            this.Id = id;
            this.Descripcion = descripcion;
         }
    }
}
