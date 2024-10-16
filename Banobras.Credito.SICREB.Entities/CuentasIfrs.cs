using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class CuentasIfrs
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; }
        public string Descripcion { get; private set; }
        public string Sector { get; private set; }
        public string Rol { get; private set; }
        public int IdClasificacion { get; private set; }
        public string descripcionClasificacion { get; private set; }
        public string FormatSic { get; private set; }
        public string FormatSicofin { get; private set; }
        public Enums.Persona Persona { get; private set; }
        public int Grupo { get; private set; }
        public Enums.Estado Estatus { get; private set; }
        public string TipoCuenta { get;private set; }

        /// <summary>
        /// Entidad de catálogo Estados
        /// </summary>
        /// <param name="pid">Número de identificación</param>
        /// <param name="pCodigo">Codigo</param>
        /// <param name="pDescripcion">Descripcion de la Cuenta</param>
        /// <param name="pSector">VARCHAR2(200)</param>
        /// <param name="pRol"></param>
        /// <param name="pIdCalificacion"></param>
        /// <param name="pFormatSic"></param>
        /// <param name="pFormatSicofin"></param>
        /// <param name="pFormatSicofin"></param>
        /// <param name="pestatus">Estatus del registro. Activo o Inactivo</param>
        public CuentasIfrs(int pId, string pCodigo, string pDescripcion, string pSector, string pRol, int pIdClasificacion, string pDescripcionClasif, string pFormatSic, string pFormatSicofin, Enums.Persona pPersona, int pGrupo, Enums.Estado pEstatus, string pTipo)
        {
            this.Id = pId;
            this.Codigo = pCodigo;
            this.Descripcion = pDescripcion;
            this.Sector = pSector;
            this.Rol = pRol;
            this.IdClasificacion = pIdClasificacion;
            this.descripcionClasificacion = pDescripcionClasif;
            this.FormatSic = pFormatSic;
            this.FormatSicofin = pFormatSicofin;
            this.Persona = pPersona;
            this.Grupo = pGrupo;
            this.Estatus = pEstatus;
            this.TipoCuenta = pTipo;
        }
    }
}
