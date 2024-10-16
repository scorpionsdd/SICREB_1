using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class ConvertidorAct
    {
        public int id_convertidor { get; private set; }
        public string cuenta_act { get; private set; }
        public string cuenta_ant_vigente { get; private set; }
        public string cuenta_ant_vencido { get; private set; }
        public string cuenta_ant_moratorios { get; private set; }
        public string cuenta_capital { get; private set; }
        public Enums.Estado estatus { get; private set; }

        /// <summary>
        /// Entidad de convertidor act
        /// </summary>
        /// <param name="pid_convertidor">Número de identificación</param>
        /// <param name="pcuenta_act"></param>
        /// <param name="pcuenta_ant_vigente"></param>
        /// <param name="pcuenta_ant_vencido"></param>
        /// <param name="pcuenta_ant_moratorios"></param>
        /// <param name="pestatus">Estatus del registro. Activo o Inactivo</param>

        public ConvertidorAct(int pid_convertidor, string pcuenta_act, string pcuenta_ant_vigente, string pcuenta_ant_vencido, string pcuenta_ant_moratorios, Enums.Estado pestatus, string pcuenta_capital)
        {
            this.id_convertidor = pid_convertidor;
            this.cuenta_act = pcuenta_act;
            this.cuenta_ant_vigente = pcuenta_ant_vigente;
            this.cuenta_ant_vencido = pcuenta_ant_vencido;
            this.cuenta_ant_moratorios = pcuenta_ant_moratorios;
            this.estatus = pestatus;
            this.cuenta_capital = pcuenta_capital;
        }
    }
}
