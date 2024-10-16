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
    public class UsuarioEntidadRules
    {

        /// <summary>
        /// Obtener catálogo de usuarios
        /// </summary>
        /// <returns></returns>
        public List<Usuario> Usuarios(bool estatus = true)
        {
            UsuariosDataAccess uda = new UsuariosDataAccess();
            //return uda.GetRecords(true);
            return uda.GetRecords(estatus);
        }

        /// <summary>
        /// Crear nuevo usuario
        /// </summary>
        /// <param name="usuario">Contenedor con información de usuario</param>
        /// <returns>Identificador de usuario creado</returns>
        public int InsertarUsuario(Usuario usuario)
        {
            UsuariosDataAccess uda = new UsuariosDataAccess();
            return uda.InsertRecord(usuario);
        }

        /// <summary>
        /// Eliminado lógico de un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int EliminarUsuario(Usuario usuario)
        {
            UsuariosDataAccess uda = new UsuariosDataAccess();
            return uda.DeleteRecord(usuario);
        }

        /// <summary>
        /// Actualizar información de un usuario
        /// </summary>
        /// <param name="usuarioCurrent"></param>
        /// <param name="usuarioUpdate"></param>
        /// <returns></returns>
        public int ActualizarUsuario(Usuario usuarioCurrent, Usuario usuarioUpdate)
        {
            UsuariosDataAccess uda = new UsuariosDataAccess();
            return uda.UpdateRecord(usuarioCurrent, usuarioUpdate);
        }

    }
}
