using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities.Types
{
    public class PFCinta
    {

        /*
         * Encabezado (HD) 150 Bytes
         */
        #region Encabezado

        private string intf_es00;
        private string intf_v01;

        /// <summary>
        /// Fija o recupera la etiqueta del segmento
        /// </summary>
        public string INTF_ES
        {
            get { return this.intf_es00; }
            set { this.intf_es00 = value; }
        }

        /// <summary>
        /// Fija o recupera la versión
        /// </summary>
        public string INTF_V
        {
            get { return this.intf_v01; }
            set { this.intf_v01 = value; }
        }

        /// <summary>
        /// Devuelve el segmento de encabezado (HD).
        /// </summary>.
        public string HD
        {
            get
            {
                StringBuilder str = new StringBuilder(150, 150);
                str.Append(this.intf_es00);
                str.Append(this.intf_v01);
                str.Append(_HD_01);
                str.Append(_HD_02);
                str.Append(_HD_03);
                str.Append(_HD_04);
                str.Append(_HD_05);
                str.Append(_HD_06);
                ///BMS HD_07
                str.Append(_HD_07);
                return str.ToString();
            }
        }


        private string _HD_01;

        public string HD_01
        {
            get { return _HD_01; }
            set { _HD_01 = value; }
        }
        private string _HD_02;

        public string HD_02
        {
            get { return _HD_02; }
            set { _HD_02 = value; }
        }
        private string _HD_03;

        public string HD_03
        {
            get { return _HD_03; }
            set { _HD_03 = value; }
        }
        private string _HD_04;

        public string HD_04
        {
            get { return _HD_04; }
            set { _HD_04 = value; }
        }
        private string _HD_05;

        public string HD_05
        {
            get { return _HD_05; }
            set { _HD_05 = value; }
        }
        private string _HD_06;

        public string HD_06
        {
            get { return _HD_06; }
            set { _HD_06 = value; }
        }
        ///BMS HD_07
        private string _HD_07;

        public string HD_07
        {
            get { return _HD_07; }
            set { _HD_07 = value; }
        }
        #endregion

    }
}
