
namespace Banobras.Credito.SICREB.Entities
{
    public class CreditosFaltantes
    {
        public int id { get; private set; }
        public string credito { get; private set; }                
        public string comentarios { get; private set; }        
        public string fecha { get; private set; }
        
        /// <summary>
        /// Entidad de Trace Proceso
        /// </summary>
        /// <param name="pid">Número de identificación</param>
        /// <param name="pcredito"></param>        
        /// <param name="pcomentario"></param>
        /// <param name="pfecha"></param>        

        public CreditosFaltantes(int pid, string pcredito, string pcomentarios, string pfecha)
        {
            this.id = pid;
            this.credito = pcredito;            
            this.comentarios = pcomentarios;
            this.fecha = pfecha;            
        }
    }
}
