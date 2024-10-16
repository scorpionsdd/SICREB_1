
namespace Banobras.Credito.SICREB.Entities
{
    public class CruceUsuarioRol
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public int IdUsuario { get; set; }
        public int IdFacultad { get; set; }
        public string Descripcion { get; set; }

        public CruceUsuarioRol(int id, int idrol, int idusuario, int idfacultad, string descripcion)
        {
            Id = id;
            IdRol=idrol;
            IdUsuario=idusuario;
            IdFacultad = idfacultad;
            Descripcion = descripcion;
        }
    }
}
