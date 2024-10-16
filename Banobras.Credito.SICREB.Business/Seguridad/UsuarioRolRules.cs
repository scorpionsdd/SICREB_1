using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Seguridad;

namespace Banobras.Credito.SICREB.Business.Seguridad
{
    public class UsuarioRolCadena
    {
        public int id { get; set; }
        public int idRol { get; set; }
        public int idUsuario { get; set; }
        public string Rol { get; set; }
        public string Usuario { get; set; }
        public Enums.Estado estatus { get; set; }
        //TODO: SOL54125 Bitacora
        public DateTime CreationDate { get; set; }
        public string Email { get; set; }
        public int EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public DateTime SessionDate { get; set; }
        public string SessionIP { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionLogin { get; set; }
    }

    public class UsuarioRolRules
    {

        /// <summary>
        /// Insertar y/o asociar roles a un usuario
        /// </summary>
        /// <param name="usuariorol"></param>
        /// <returns></returns>
        public int InsertarUsuarioRol(UsuarioRol usuariorol)
        {
            UsuarioRolDataAccess urda = new UsuarioRolDataAccess();
            return urda.InsertRecord(usuariorol);
        }

        /// <summary>
        /// Obtener roles asociados a un usuario
        /// </summary>
        /// <returns></returns>
        public List<UsuarioRol> UsuariosRol(bool estatus = true)
        {
            UsuarioRolDataAccess urda = new UsuarioRolDataAccess();
            //return urda.GetRecords(true);
            return urda.GetRecords(estatus);
        }

        public List<UsuarioRolCadena> UsuarioRolesCadena()
        {
            RolRules rr = new RolRules();
            List<Rol> Roles = rr.Roles();
            UsuarioEntidadRules uer = new UsuarioEntidadRules();
            List<Usuario> Usuarios = uer.Usuarios();

            List<UsuarioRol> usuariosRol = UsuariosRol();
            List<UsuarioRolCadena> usuarioRolCadena = new List<UsuarioRolCadena>();
            var query = from usurol in usuariosRol join r in Roles on usurol.IdRol equals r.Id 
                        select new { 
                            usurol.IdRol, 
                            Rol = r.Descripcion, 
                            usurol.IdUsuario, 
                            usurol.Id, 
                            usurol.Estatus
                        };
            List<UsuarioRolCadena> urc = (from q in query join f in Usuarios on q.IdUsuario equals f.Id 
                                          select new UsuarioRolCadena { 
                                              idRol = q.IdRol, 
                                              Rol = q.Rol, 
                                              idUsuario = q.IdUsuario, 
                                              id = q.Id, 
                                              estatus = q.Estatus,
                                              Usuario = f.Login,
                                              //TODO: SOL54125 Bitacora
                                              CreationDate = f.CreationDate,
                                              Email = f.Email,
                                              EmployeeNumber = f.EmployeeNumber,
                                              FullName = f.FullName,
                                              SessionDate = f.SessionDate,
                                              SessionIP = f.SessionIP,
                                              TransactionDate = f.TransactionDate,
                                              TransactionLogin = f.TransactionLogin
                                          }).ToList<UsuarioRolCadena>();
            foreach (UsuarioRolCadena tmpurc in urc)
            {
                int indice = usuarioRolCadena.FindIndex(delegate(UsuarioRolCadena p) { return p.idUsuario == tmpurc.idUsuario; });
                if (indice >= 0)
                {
                    UsuarioRolCadena temporal = usuarioRolCadena[indice];
                    temporal.Rol = string.Concat(usuarioRolCadena[indice].Rol, ",", tmpurc.Rol);
                    usuarioRolCadena[indice] = temporal;
                }
                else
                {
                    usuarioRolCadena.Add(tmpurc);
                }
            }
            return usuarioRolCadena;
        }

        /// <summary>
        /// Eliminado lógico de roles asociados a un usuario
        /// </summary>
        /// <param name="usuarioRol"></param>
        /// <returns></returns>
        public int EliminarUsuarioRol(UsuarioRol usuarioRol)
        {
            UsuarioRolDataAccess urda = new UsuarioRolDataAccess();
            return urda.DeleteRecord(usuarioRol);
        }

        /// <summary>
        /// Actualizar información de un usuario
        /// </summary>
        /// <param name="usuarioCurrent"></param>
        /// <param name="usuarioUpdate"></param>
        /// <returns></returns>
        public int ActualizarUsuarioRol(UsuarioRol usuarioCurrent, UsuarioRol usuarioUpdate)
        {
            UsuarioRolDataAccess urda = new UsuarioRolDataAccess();
            return urda.UpdateRecord(usuarioCurrent, usuarioUpdate);
        }

    }

}
