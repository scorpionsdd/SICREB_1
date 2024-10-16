using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Pais
    {
        //propiedades:
        public int Id { get; private set; }
        public int ClaveSIC { get; private set; }
        public string ClaveBuro { get; private set; } //varchar(2)
        public string Descripcion { get; private set; } //varchar(30)
        public Enums.Persona TipoPersona { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Crea una entidad de Catalogo Clave_Pais
        /// </summary>
        /// <param name="id">Numero de identificacion</param>
        /// <param name="claveSIC">Numero Clave SIC</param>
        /// <param name="claveBuro">Varchar(2) Clave Buró</param>
        /// <param name="descripcion">Varchar(30) Descripción</param>
        /// <param name="tipoPersona">Tipo persona Moral o Fisica</param>
        /// <param name="estatus">Estado de registro Activo o Inactivo</param>
        public Pais(int id, int claveSIC, string claveBuro, string descripcion, Enums.Persona tipoPersona, Enums.Estado estatus)
        {
            this.Id = id;
            this.ClaveSIC = claveSIC;
            this.ClaveBuro = claveBuro;
            this.Descripcion = descripcion;
            this.TipoPersona = tipoPersona;
            this.Estatus = estatus;
        }
    }
}
