using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;

namespace Banobras.Credito.SICREB.Business.DTOs
{
    
    /// <summary>
    /// Información de usuarios, más roles asociados
    /// </summary>
    public class UsuarioDTO : Usuario
    {

        public UsuarioDTO() { }


        public UsuarioDTO(Usuario model)
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
        /// Roles asociados al usuario
        /// </summary>
        public List<UsuarioRol> Roles { get; set; }

        /// <summary>
        /// Lista de nombres de roles asociados al usuario
        /// </summary>
        public string RolesNameList { get; set; }

    }

}
