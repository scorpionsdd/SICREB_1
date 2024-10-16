using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{

    public class FacultadRol
    {

        public FacultadRol()
        {

        }

        public FacultadRol(FacultadRol model)
        {
            this.Estatus = model.Estatus;
            this.Id = model.Id;
            this.IdFacultad = model.IdFacultad;
            this.IdRol = model.IdRol;
        }

        public FacultadRol(int id, int idrol, int idfacultad, Enums.Estado estatus)
        {
            Id = id;
            IdRol = idrol;
            IdFacultad = idfacultad;
            Estatus = estatus;
        }


        /// <summary>
        /// Identificador automático
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador de Rol en el catálogo
        /// </summary>
        public int IdRol { get; set; }

        /// <summary>
        /// Identificador de la Facultad en el catálogo
        /// </summary>
        public int IdFacultad { get; set; }

        /// <summary>
        /// Estatus de la factultad con el rol
        /// </summary>
        public Enums.Estado Estatus { get; set; }

    }

}
