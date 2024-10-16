using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Institucion
    {
        public int Id { get; private set; }
        public int  ClaveExterna { get; private set; }
        public string Descripcion { get; private set; }
        public Enums.Estado Estatus { get; private set; }
        public String Generico { get; private set; }

        /// <summary>
        /// Entidad de catálogo Instituciones
        /// </summary>
        /// <param name="id">Número de identificación</param>
        /// <param name="claveExterna">Varchar(3) Clave externa</param>
        /// <param name="descripcion">Varchar(50) Descripción</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        /// <param name="generico">Varchar(25) Genérico</param>
        public Institucion(int id, int claveExterna, string descripcion, Enums.Estado estatus, string generico)
        {
            this.Id = id;
            this.ClaveExterna = claveExterna;
            this.Descripcion = descripcion;
            this.Estatus = estatus;
            this.Generico = generico;
        }
    }
}
