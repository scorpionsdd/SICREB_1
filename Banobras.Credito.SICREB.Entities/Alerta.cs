using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Alerta
    {
        public int Id { get; private set; }
        public string IdententifadorAlerta { get; private set; }
        public string TituloAlerta { get; private set; }
        public string Mensaje { get; private set; }
        public string Periodicidad { get; private set; }
        public string AplicacionDePeriodicidad { get; private set; }
        public string AlarmaActivada { get; private set; }

        public Enums.Estado Estatus { get; private set; }

        /// <summary>
        /// Entidad de catalogo Monedas
        /// </summary>
        /// <param name="id">Numero de identificación</param>
        /// <param name="claveBuro">Número Clave Buro</param>
        /// <param name="claveClic">Número Clave Clic</param>
        /// <param name="descripcion">Varchar(35) Descripción</param>
        /// <param name="estatus">Estatus del registro. Activo o Inactivo</param>
        public Alerta(int id, string IdententifadorAlertaP, string tituloAlertaP, string mensajeP, string periodicidadP, string aplicacionDePeriodicidadP, string alarmaActivadaP)
        {
            this.Id = id;
            this.IdententifadorAlerta = IdententifadorAlertaP;
            this.TituloAlerta = tituloAlertaP;
            this.Mensaje = mensajeP;
            this.Periodicidad = periodicidadP;
            this.AplicacionDePeriodicidad = aplicacionDePeriodicidadP;
            this.AlarmaActivada = alarmaActivadaP;
            
            
        }

    }
}
