using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class ConvertidorActivos
    {
        public int id_convertidor { get; private set; }
        public string auxiliar { get; private set; }
        public string credito { get; private set; }        
        public string cuenta_ant_vigente { get; private set; }
        public string cuenta_ant_vencido { get; private set; }
        public string cuenta_ant_moratorios { get; private set; }        
        public Enums.Estado estatus { get; private set; }
        
        /// <summary>
        /// Entidad de convertidor activos
        /// </summary>
        /// <param name="pid_convertidor">Número de identificación</param>
        /// <param name="pauxiliar"></param>
        /// <param name="pcredito"></param>
        /// <param name="pcuenta_ant_vigente"></param>
        /// <param name="pcuenta_ant_vencido"></param>
        /// <param name="pcuenta_ant_moratorios"></param>
        /// <param name="pestatus">Estatus del registro. Activo o Inactivo</param>

        public ConvertidorActivos(int pid_convertidor, string pauxiliar, string pcredito, string pcuenta_ant_vigente, string pcuenta_ant_vencido, string pcuenta_ant_moratorios, Enums.Estado pestatus)
        {
            this.id_convertidor = pid_convertidor;
            this.auxiliar = pauxiliar;
            this.credito = pcredito;
            this.cuenta_ant_vigente = pcuenta_ant_vigente;
            this.cuenta_ant_vencido = pcuenta_ant_vencido;
            this.cuenta_ant_moratorios = pcuenta_ant_moratorios;            
            this.estatus = pestatus;            
        }
    }
}
