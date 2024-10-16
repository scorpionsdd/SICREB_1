using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class CreditoFiduciario
    {

        public int Id { get; private set; }
        public string Credito { get; private set; }
        public Enums.Estado Estatus { get; private set; }
        public string Nombre {get;set; }
        public string RFC { get; set; }
        /// <summary>
        /// Entidad de catálogo Creditos Fiducuario
        /// </summary>
        /// <param name="pid">Número de identificación</param>
        /// <param name="pCredito">Descripcion del Credito</param>
        /// <param name="pestatus">Estatus del registro. Activo o Inactivo</param>
        public CreditoFiduciario(int pId, string pCredito, Enums.Estado pEstatus)
        {
            this.Id = pId;
            this.Credito = pCredito;
            this.Estatus = pEstatus;
        }
        public CreditoFiduciario(int pId, string pCredito,string rfc,string nombre, Enums.Estado pEstatus)
        {
            this.Id = pId;
            this.Credito = pCredito;
            this.Estatus = pEstatus;
            this.RFC = rfc;
            this.Nombre = nombre;
        }
    }
}
