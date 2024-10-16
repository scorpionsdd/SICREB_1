using Banobras.Credito.SICREB.Data.Seguridad;
using Banobras.Credito.SICREB.Entities;
using System.Collections.Generic;

namespace Banobras.Credito.SICREB.Business.Seguridad
{
    public class RolRules
    {

        /// <summary>
        /// Obtener catálogo de roles
        /// </summary>
        /// <param name="estatus">True = Sólo los activos | False = Todos los registros</param>
        /// <returns></returns>
        public List<Rol> Roles(bool estatus = true)
        {
            RolesDataAccess rda = new RolesDataAccess();
            //return rda.GetRecords(true);
            return rda.GetRecords(estatus);
        }

        /// <summary>
        /// Crear nuevo rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        public int InsertarRol(Rol rol)
        {
            RolesDataAccess rda = new RolesDataAccess();
             return rda.InsertRecord(rol);
        }

        /// <summary>
        /// Eliminado lógico de un rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        public int EliminarRol(Rol rol)
        {
            RolesDataAccess rda = new RolesDataAccess();
            return rda.DeleteRecord(rol);
        }

        /// <summary>
        /// Actualizar información de un rol
        /// </summary>
        /// <param name="rolCurrent"></param>
        /// <param name="rolUpdate"></param>
        /// <returns></returns>
        public int ActualizarRol(Rol rolCurrent, Rol rolUpdate)
        {
            RolesDataAccess rda = new RolesDataAccess();
            return rda.UpdateRecord(rolCurrent, rolUpdate);
        }
    }


}
