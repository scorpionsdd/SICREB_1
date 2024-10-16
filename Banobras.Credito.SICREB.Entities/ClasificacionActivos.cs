
namespace Banobras.Credito.SICREB.Entities
{
    public class ClasificacionActivos
    {
        public int id { get; private set; }
        public string descripcion { get; private set; }                
        public string vigente { get; private set; }
        
        /// <summary>
        /// Entidad de clasificacion activos
        /// </summary>
        /// <param name="pid">Número de identificación</param>
        /// <param name="pdescripcion"></param>        
        /// <param name="pestado_clas">Estatus de la cuenta. Vigente o Vencido</param>

        public ClasificacionActivos(int pid, string pdescripcion, string pvigente)
        {
            this.id = pid;
            this.descripcion = pdescripcion;            
            this.vigente = pvigente;
        }
    }
}
