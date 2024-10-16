using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Estado
    {
        public int Id { get; private set; }
        public int ClaveClic { get; private set; }
        public string ClaveBuro { get; private set; }
        public string Descripcion { get; private set; }
        public Enums.Persona TipoPersona { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Entidad de catálogo Estados
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="claveClic">Número de Clave Clic</param>
        /// <param name="claveBuro">Varchar(4) CLave Buro</param>
        /// <param name="descripcion">Varchar(30) Descripción</param>
        /// <param name="tipoPersona">Tipo de Persona. Moral o Fisica</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public Estado(int id, int claveClic, string claveBuro, string descripcion, Enums.Persona tipoPersona, Enums.Estado estatus)
        {
            this.Id = id;
            this.ClaveClic = claveClic;
            this.ClaveBuro = claveBuro;
            this.Descripcion = descripcion;
            this.TipoPersona = tipoPersona;
            this.Estatus = estatus;
        }
    }
}
