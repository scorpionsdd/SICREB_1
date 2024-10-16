using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class UsuarioRol
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public int IdUsuario { get; set; }
        public Enums.Estado Estatus { get; set; }

        public UsuarioRol()
        {

        }

        public UsuarioRol(int id,int idrol,int idusuario,Enums.Estado estatus)
        {
            Id = id;
            IdRol=idrol;
            IdUsuario=idusuario;
            Estatus=estatus;
        }

        public UsuarioRol(UsuarioRol model)
        {
            this.Estatus = model.Estatus;
            this.Id = model.Id;
            this.IdRol = model.IdRol;
            this.IdUsuario = model.IdUsuario;
        }


    }
}
