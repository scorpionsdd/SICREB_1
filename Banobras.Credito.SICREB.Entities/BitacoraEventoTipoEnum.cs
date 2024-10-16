using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities
{

    /// <summary>
    /// Tipos de eventos de la bitácora tomados de la tabla "Bitacora_Eventos"
    /// </summary>

    public enum BitacoraEventoTipoEnum
    {

        /// <summary>
        /// Creación de Rol
        /// </summary>
        Rol_Alta = 1,

        /// <summary>
        /// Actualización de información de Rol
        /// </summary>
        Rol_Actualizacion = 2,

        /// <summary>
        /// Eliminación lógica de Rol
        /// </summary>
        Rol_Baja = 3,

        /// <summary>
        /// Reactivación de Rol
        /// </summary>
        Rol_Reactivacion = 4,

        /// <summary>
        /// Alta de Función en Rol
        /// </summary>
        Rol_AltaFuncion = 5,

        /// <summary>
        /// Baja de Función en Rol
        /// </summary>
        Rol_BajaFuncion = 6,

        /// <summary>
        /// Creación de Usuario
        /// </summary>
        Usuario_Alta = 7,

        /// <summary>
        /// Actualización de información de Usuario
        /// </summary>
        Usuario_Actualizacion = 8,

        /// <summary>
        /// Eliminación lógica de Usuario
        /// </summary>
        Usuario_Baja = 9,

        /// <summary>
        /// Reactivación de Usuario
        /// </summary>
        Usuario_Reactivacion = 10,

        /// <summary>
        /// Cerrar sesión de Usuario
        /// </summary>
        Usuario_CerrarSesion = 11,

        /// <summary>
        /// Inicio de sesión
        /// </summary>
        Sesion_Inicio = 12,

        /// <summary>
        /// Cierre de sesión
        /// </summary>
        Sesion_Cierre = 13,

        /// <summary>
        /// Inicio de sesión fallido
        /// </summary>
        Sesion_InicioFallido = 14,

        /// <summary>
        /// Usuario no registrado
        /// </summary>
        UsuarioNoRegistrado = 15,

        /// <summary>
        /// Usuario con sesión iniciada
        /// </summary>
        UsuarioSesionIniciada = 16

    }

}
