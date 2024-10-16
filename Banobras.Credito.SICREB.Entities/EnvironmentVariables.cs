using System.Collections.Generic;

namespace Banobras.Credito.SICREB.Entities
{

    /// <summary>
    /// Propiedades de las variables de ambiente definidas en el SO anfitrión
    /// </summary>
    public class EnvironmentVariables
    {

        /// <summary>
        /// Información de bases de datos
        /// </summary>
        public List<AccountDetail> Databases { get; set; }

        /// <summary>
        /// Información de cuenta de Directorio Activo
        /// </summary>
        public AccountDetail ActiveDirectory { get; set; }

        /// <summary>
        /// Información de cuentas de correo electrónico
        /// </summary>
        public List<AccountDetail> Emails { get; set; }

    }

    /// <summary>
    /// Información de Cuenta
    /// </summary>
    public class AccountDetail
    {

        /// <summary>
        /// Hostname o IP del servidor
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Puerto de acceo al servidor
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Nombre de Esquema de BD
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Contraseña
        /// </summary>
        public string Password { get; set; }

    }

}
