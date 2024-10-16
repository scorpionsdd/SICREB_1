
namespace Banobras.Credito.SICREB.Entities
{
    public class ClasificacionAct
    {
        public int id { get; private set; }
        public string descripcion { get; private set; }                
        public string vigente { get; private set; }
        
        /// <summary>
        /// Entidad de clasificacion act
        /// </summary>
        /// <param name="pid">Número de identificación</param>
        /// <param name="pdescripcion"></param>        
        /// <param name="pestado_clas">Estatus de la cuenta. Vigente o Vencido</param>

        public ClasificacionAct(int pid, string pdescripcion, string pvigente)
        {
            this.id = pid;
            this.descripcion = pdescripcion;            
            this.vigente = pvigente;
        }
    }
}
