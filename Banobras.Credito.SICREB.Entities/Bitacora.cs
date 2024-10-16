using System;


namespace Banobras.Credito.SICREB.Entities
{

    /// <summary>
    /// Contenedor de datos de bitácora
    /// </summary>
    public class Bitacora
    {

        /// <summary>
        /// Identificador de bitácora (autogenerado)
        /// </summary>
        public int LogId { get; set; }

        /// <summary>
        /// Fecha de transacción
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Comentario de la transacción
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Identificador del usuario en LDAP o Nómina que ejecuta la transacción
        /// </summary>
        public int EmployeeNumber { get; set; }

        /// <summary>
        /// Identificador de evento
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Json de la petición
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// Dirección IP desde donde se ejecutó la transacción
        /// </summary>
        public string SessionIP { get; set; }

        /// <summary>
        /// UserName-Login del usuario que ejecuta la transacción
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Nombre del usuario que ejecuta la transacción
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Tipo de Bitácora
        /// </summary>
        public string LogType { get; set; }

        public string FullName_Login
        {
            get
            {
                return string.Format("{0} ({1})", UserFullName, UserLogin);
            }
        }
    }

}
