using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Bitacora;
using System;

namespace Banobras.Credito.SICREB.Business.Repositorios
{

    /// <summary>
    /// Clase que implementa los métodos para la Bitácora
    /// </summary>
    public class BitacoraRules
    {

        /// <summary>
        /// Guardar bitacora
        /// </summary>
        /// <param name="bitacora">Contenedor con información de bitácora</param>
        /// <returns></returns>
        public static bool AgregarBitacora(Bitacora bitacora)
        {
            return BitacoraDataAccess.AgregarBitacora(bitacora);
        }

        /// <summary>
        /// Obtener data de Bitácora
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial de consulta</param>
        /// <param name="fechaFinal">Fecha hasta donde se desea consultar</param>
        /// <param name="eventoId">Identificador de Evento</param>
        /// <param name="userName">Nombre de usuario por el que se desea consultar</param>
        /// <returns></returns>
        public static List<Bitacora> ObtenerBitacora(DateTime? fechaInicial, DateTime? fechaFinal, int? eventoId, string userName)
        {
            return BitacoraDataAccess.ObtenerBitacora(fechaInicial, fechaFinal, eventoId, userName);
            //return BitacoraDataAccess.ObtenerBitacoraDUMMY(fechaInicial, fechaFinal, eventoId, userName);
        }

        /// <summary>
        /// Obtener data de Eventos
        /// </summary>
        /// <returns></returns>
        public static List<BitacoraEvento> ObtenerEventos()
        {
            return BitacoraDataAccess.ObtenerEventos();
        }

    }

}
