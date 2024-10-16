
namespace Banobras.Credito.SICREB.Entities
{
    public class Request_Edit
    {
        public int id { get; private set; }
        public string fecha_inicio { get; private set; }
        public string fecha_final { get; private set; }
        public string estado { get; private set; }
        public int id_archivo_pm { get; private set; }
        public int id_archivo_pf { get; private set; }
        public string reporte { get; private set; }
        public string notificaciones { get; private set; }
        public string grupos { get; private set; }
        
        
        /// <summary>
        /// Entidad de Request_Edit
        /// </summary>
        /// <param name="pid">Número de identificación</param>
        /// <param name="pfecha_inicio"></param>
        /// <param name="pfecha_final"></param>
        /// <param name="pestado"></param>
        /// <param name="pid_archivo_pm"></param>
        /// <param name="pid_archivo_pf"></param>
        /// <param name="preporte"></param>
        /// <param name="pnotificaciones"></param>
        /// <param name="pgrupos"></param>

        public Request_Edit(int pid, string pfecha_inicio, string pfecha_final, string pestado, int pid_archivo_pm, int pid_archivo_pf, string preporte, string pnotificaciones, string pgrupos)
        {
            this.id = pid;
            this.fecha_inicio = pfecha_inicio;
            this.fecha_final = pfecha_final;
            this.estado = pestado;
            this.id_archivo_pm = pid_archivo_pm;
            this.id_archivo_pf = pid_archivo_pf;
            this.reporte = preporte;
            this.notificaciones = pnotificaciones;
            this.grupos = pgrupos;
        }
    }
}
