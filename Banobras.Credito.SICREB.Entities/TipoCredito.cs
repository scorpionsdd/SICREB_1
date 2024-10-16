using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class TipoCredito
    {
        public int Id { get; private set; }
        public string tipoCredito { get; private set; }
        public string Descripcion { get; private set; }
        public string NombreGenerico { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Entidad de catálogo Creditos
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="tipoCredito">Varchar(50) Tipo de crédito.</param>
        /// <param name="descripcion">Varchar(200) Descripción</param>
        /// <param name="nombreGenerico">Varchar(50) Nombre genérico</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public TipoCredito(int pid, string ptipoCredito, string pdescripcion, string pnombreGenerico, Enums.Estado pestatus)
        {
            this.Id = pid;
            this.tipoCredito = ptipoCredito;
            this.Descripcion = pdescripcion;
            this.NombreGenerico = pnombreGenerico;
            this.Estatus = pestatus;
        }
    }
}
