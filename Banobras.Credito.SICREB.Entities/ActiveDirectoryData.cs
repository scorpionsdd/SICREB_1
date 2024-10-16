
namespace Banobras.Credito.SICREB.Entities
{
    
    /// <summary>
    /// Atributos de un usuario dentro del Directorio Activo.
    /// </summary>
    public class ActiveDirectoryData
    {

        /// <summary>
        /// Departamento
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Nombre completo
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Objeto DN con información de un usuario
        /// </summary>
        public string DistinguishedName { get; set; }

        /// <summary>
        /// Nombre(s)
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Iniciales - No. Empleado
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// Dirección de correo
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// UserName o Login
        /// </summary>
        public string SamAccountName { get; set; }

        /// <summary>
        /// Apellidos
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Téléfono
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Puesto
        /// </summary>
        public string Title { get; set; }
                
    }

}
