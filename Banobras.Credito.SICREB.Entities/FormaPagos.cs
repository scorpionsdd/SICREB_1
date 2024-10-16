using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class FormaPagos
    {
        public int Id { get; private set; }
        public string Descripcion { get; private set; }
        public string Comentarios { get; private set; }
        public string Clave { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Entidad de catálogo FormaPagos
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="claveBuro">Varchar(10) Descripcion</param>
        /// <param name="claveSIC">Varchar(10) Comentarios</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public FormaPagos(int pid, string pDescripcion, string pComentarios, Enums.Estado pEstatus, string pClave)
        {
            this.Id = pid;
            this.Descripcion = pDescripcion;
            this.Comentarios = pComentarios;
            this.Clave = pClave;
            this.Estatus = pEstatus;
        }
    }
}
