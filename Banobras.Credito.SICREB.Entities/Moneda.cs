using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Moneda
    {
        public int Id { get; private set; }
        public int ClaveBuro { get; private set; }
        public int ClaveClic { get; private set; }
        public string Descripcion { get; private set; }
        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Entidad de catalogo Monedas
        /// </summary>
        /// <param name="id">Numero de identificación</param>
        /// <param name="claveBuro">Número Clave Buro</param>
        /// <param name="claveClic">Número Clave Clic</param>
        /// <param name="descripcion">Varchar(35) Descripción</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public Moneda(int id, int claveBuro, int claveClic, string descripcion, Enums.Estado estatus)
        {
            this.Id = id;
            this.ClaveBuro = claveBuro;
            this.ClaveClic = claveClic;
            this.Descripcion = descripcion;
            this.Estatus = estatus;
        }

    }
}
