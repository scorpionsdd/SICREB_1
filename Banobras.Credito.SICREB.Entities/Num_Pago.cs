using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Num_Pago
    {
        public int Id { get; private set; }
        public string ClaveBuro { get; private set; }
        public string ClaveSIC { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Entidad de catálogo Num_Pagos
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="claveBuro">Varchar(10) Clave Buró</param>
        /// <param name="claveSIC">Varchar(10) Clave SIC</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public Num_Pago(int id, string claveBuro, string claveSIC, Enums.Estado estatus)
        {
            this.Id = id;
            this.ClaveBuro = claveBuro;
            this.ClaveSIC = claveSIC;
            this.Estatus = estatus;
        }
    }
}
