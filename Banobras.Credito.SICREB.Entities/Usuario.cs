using Banobras.Credito.SICREB.Entities.Util;
using System;

namespace Banobras.Credito.SICREB.Entities
{
    public class Usuario
    {

        public Usuario(int id, string login, Enums.Estado estatus)
        {
            Id = id;
            Login = login;
            Estatus = estatus;
        }

        //TODO: SOL54125 Bitacora
        public Usuario()
        {

        }

        //TODO: SOL54125 Bitacora
        public Usuario(Usuario model)
        {
            CreationDate = model.CreationDate;
            Email = model.Email;
            EmployeeNumber = model.EmployeeNumber;
            Estatus = model.Estatus;
            FullName = model.FullName;
            Id = model.Id;
            Login = model.Login;
            SessionDate = model.SessionDate;
            SessionIP = model.SessionIP;
            TransactionDate = model.TransactionDate;
            TransactionLogin = model.TransactionLogin;
        }

        /// <summary>
        /// Consecutivo autogenerado
        /// </summary>
        public int Id{ get; set; }

        /// <summary>
        /// Username para el usuario
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Estatus de operación
        /// </summary>
        public Enums.Estado Estatus { get; set; }

        /// <summary>
        /// Dirección de correo
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Fecha de creación del registro del usuario
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Número de empleado tomado de LDAP o RH
        /// </summary>
        public int EmployeeNumber { get; set; }

        /// <summary>
        /// Nombre para el usuario
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Fecha de la última sesión del usuario
        /// </summary>
        public DateTime SessionDate { get; set; }

        /// <summary>
        /// Dirección IP del equipo donde se inicia la sesión.
        /// Si tiene valor, no se puede volver a iniciar sesión por el mismo usuario.
        /// </summary>
        public string SessionIP { get; set; }

        /// <summary>
        /// Fecha en que se realiza un cambio en el registro del usuario.
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Username de quien realiza un cambio en el registro del usuario.
        /// </summary>
        public string TransactionLogin { get; set; }

    }
}
