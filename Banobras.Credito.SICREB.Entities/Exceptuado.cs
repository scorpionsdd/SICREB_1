using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Exceptuado
    {

        public int Id { get; private set; }
        public String Credito { get; private set; }
        public String Motivo { get; set; }
        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Entidad de catálogo Exceptuados
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="credito">Varchar(10) Crédito</param>
        /// <param name="motivo">Varchar(150) Motivo</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public Exceptuado(int id, string credito, string motivo, Enums.Estado estatus)
        {
            this.Id = id;
            this.Credito = credito;
            this.Motivo = motivo;
            this.Estatus = estatus;
        }
    }
}
