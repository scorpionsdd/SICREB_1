using Banobras.Credito.SICREB.Entities.Util;
using System;

namespace Banobras.Credito.SICREB.Entities
{
    public class Rol
    {

        public Rol() { }

        public Rol(int id, string descripcion, Enums.Estado estatus)
        {
            Id = id;
            Descripcion = descripcion;
            Estatus = estatus;
        }

        public Rol(Rol model)
        {
            this.CreationDate = model.CreationDate;
            this.Descripcion = model.Descripcion;
            this.Estatus = model.Estatus;
            this.Id = model.Id;
            this.TransactionDate = model.TransactionDate;
            this.TransactionLogin = model.TransactionLogin;
        }

        /// <summary>
        /// Identificador del rol
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descripción o denominación del rol
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Estatus de Operación
        /// </summary>
        public Enums.Estado Estatus { get; set; }

        /// <summary>
        /// Fecha de creación del registro del rol
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha en que se realiza un cambio en el registro del rol.
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Username de quien realiza un cambio en el registro del rol.
        /// </summary>
        public string TransactionLogin { get; set; }

    }
}
