using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Banobras.Credito.SICREB.Common.Types
{
    /// <summary>
    /// Cinta de Personas Morales
    /// Lenght: 3000 Bytes
    /// Segmentos: HD, EM, AC, CR, DE, AV, TS.
    /// </summary>
    public class PMCinta
    {
        /*
         * Encabezado (HD) 100 Bytes
         */
        #region Encabezado
        /// <summary>
        /// Devuelve el segmento de encabezado (HD).
        /// </summary>.
        public string HD
        {
            get {
                StringBuilder str = new StringBuilder(100,100);
                str.Append(_HD_HD);
                str.Append(_HD_00);
                str.Append(_HD_01);
                str.Append(_HD_02);
                str.Append(_HD_03);
                str.Append(_HD_04);
                str.Append(_HD_05);
                str.Append(_HD_06);
                return str.ToString();
            }
        }

        private string _HD_HD;

        public string HD_HD
        {
            get { return _HD_HD; }
            set { _HD_HD = value; }
        }
        private string _HD_00;

        public string HD_00
        {
            get { return _HD_00; }
            set { _HD_00 = value; }
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
        #endregion


        /*
         * Compañia (EM) 900 Bytes
         */
        #region Compañia
        private string _EM_EM;

        public string EM_EM
        {
            get { return _EM_EM; }
            set { _EM_EM = value; }
        }
        private string _EM_00;

        public string EM_00
        {
            get { return _EM_00; }
            set { _EM_00 = value; }
        }
        private string _EM_01;

        public string EM_01
        {
            get { return _EM_01; }
            set { _EM_01 = value; }
        }
        private string _EM_02;

        public string EM_02
        {
            get { return _EM_02; }
            set { _EM_02 = value; }
        }
        private string _EM_03;

        public string EM_03
        {
            get { return _EM_03; }
            set { _EM_03 = value; }
        }
        private string _EM_04;

        public string EM_04
        {
            get { return _EM_04; }
            set { _EM_04 = value; }
        }
        private string _EM_05;

        public string EM_05
        {
            get { return _EM_05; }
            set { _EM_05 = value; }
        }
        private string _EM_06;

        public string EM_06
        {
            get { return _EM_06; }
            set { _EM_06 = value; }
        }
        private string _EM_07;

        public string EM_07
        {
            get { return _EM_07; }
            set { _EM_07 = value; }
        }
        private string _EM_08;

        public string EM_08
        {
            get { return _EM_08; }
            set { _EM_08 = value; }
        }
        private string _EM_09;

        public string EM_09
        {
            get { return _EM_09; }
            set { _EM_09 = value; }
        }
        private string _EM_10;

        public string EM_10
        {
            get { return _EM_10.PadLeft(11,'0'); }
            set { _EM_10 = value; }
        }
        private string _EM_11;

        public string EM_11
        {
            get { return _EM_11; }
            set { _EM_11 = value; }
        }
        private string _EM_12;

        public string EM_12
        {
            get { return _EM_12; }
            set { _EM_12 = value; }
        }
        private string _EM_13;

        public string EM_13
        {
            get { return _EM_13; }
            set { _EM_13 = value; }
        }
        private string _EM_14;

        public string EM_14
        {
            get { return _EM_14; }
            set { _EM_14 = value; }
        }
        private string _EM_15;

        public string EM_15
        {
            get { return _EM_15; }
            set { _EM_15 = value; }
        }
        private string _EM_16;

        public string EM_16
        {
            get { return _EM_16; }
            set { _EM_16 = value; }
        }
        private string _EM_17;

        public string EM_17
        {
            get { return _EM_17; }
            set { _EM_17 = value; }
        }
        private string _EM_18;

        public string EM_18
        {
            get { return _EM_18; }
            set { _EM_18 = value; }
        }
        private string _EM_19;

        public string EM_19
        {
            get { return _EM_19; }
            set { _EM_19 = value; }
        }
        private string _EM_20;

        public string EM_20
        {
            get { return _EM_20; }
            set { _EM_20 = value; }
        }
        private string _EM_21;

        public string EM_21
        {
            get { return _EM_21; }
            set { _EM_21 = value; }
        }
        private string _EM_22;

        public string EM_22
        {
            get { return _EM_22; }
            set { _EM_22 = value; }
        }
        private string _EM_23;

        public string EM_23
        {
            get { return _EM_23; }
            set { _EM_23 = value; }
        }
        private string _EM_24;

        public string EM_24
        {
            get { return _EM_24; }
            set { _EM_24 = value; }
        }
        private string _EM_25;

        public string EM_25
        {
            get { return _EM_25; }
            set { _EM_25 = value; }
        }
        private string _EM_26;

        public string EM_26
        {
            get { return _EM_26; }
            set { _EM_26 = value; }
        }
        #endregion


        /*
         * Accionistas (AC) 700 Bytes 
         */
        #region Accionistas
        private string _AC_AC;

        public string AC_AC
        {
            get { return _AC_AC; }
            set { _AC_AC = value; }
        }
        private string _AC_00;

        public string AC_00
        {
            get { return _AC_00; }
            set { _AC_00 = value; }
        }
        private string _AC_01;

        public string AC_01
        {
            get { return _AC_01; }
            set { _AC_01 = value; }
        }
        private string _AC_02;

        public string AC_02
        {
            get { return _AC_02; }
            set { _AC_02 = value; }
        }
        private string _AC_03;

        public string AC_03
        {
            get { return _AC_03; }
            set { _AC_03 = value; }
        }
        private string _AC_04;

        public string AC_04
        {
            get { return _AC_04; }
            set { _AC_04 = value; }
        }
        private string _AC_05;

        public string AC_05
        {
            get { return _AC_05; }
            set { _AC_05 = value; }
        }
        private string _AC_06;

        public string AC_06
        {
            get { return _AC_06; }
            set { _AC_06 = value; }
        }
        private string _AC_07;

        public string AC_07
        {
            get { return _AC_07; }
            set { _AC_07 = value; }
        }
        private string _AC_08;

        public string AC_08
        {
            get { return _AC_08; }
            set { _AC_08 = value; }
        }
        private string _AC_09;

        public string AC_09
        {
            get { return _AC_09; }
            set { _AC_09 = value; }
        }
        private string _AC_10;

        public string AC_10
        {
            get { return _AC_10; }
            set { _AC_10 = value; }
        }
        private string _AC_11;

        public string AC_11
        {
            get { return _AC_11; }
            set { _AC_11 = value; }
        }
        private string _AC_12;

        public string AC_12
        {
            get { return _AC_12; }
            set { _AC_12 = value; }
        }
        private string _AC_13;

        public string AC_13
        {
            get { return _AC_13; }
            set { _AC_13 = value; }
        }
        private string _AC_14;

        public string AC_14
        {
            get { return _AC_14; }
            set { _AC_14 = value; }
        }
        private string _AC_15;

        public string AC_15
        {
            get { return _AC_15; }
            set { _AC_15 = value; }
        }
        private string _AC_16;

        public string AC_16
        {
            get { return _AC_16; }
            set { _AC_16 = value; }
        }
        private string _AC_17;

        public string AC_17
        {
            get { return _AC_17; }
            set { _AC_17 = value; }
        }
        private string _AC_18;

        public string AC_18
        {
            get { return _AC_18; }
            set { _AC_18 = value; }
        }
        private string _AC_19;

        public string AC_19
        {
            get { return _AC_19; }
            set { _AC_19 = value; }
        }
        private string _AC_20;

        public string AC_20
        {
            get { return _AC_20; }
            set { _AC_20 = value; }
        }
        private string _AC_21;

        public string AC_21
        {
            get { return _AC_21; }
            set { _AC_21 = value; }
        }
        private string _AC_22;

        public string AC_22
        {
            get { return _AC_22; }
            set { _AC_22 = value; }
        }
        #endregion


        /*
         * Credito (CR) 400 Bytes
         */
        #region Crédito
        private string _CR_CR;

        public string CR_CR
        {
            get { return _CR_CR; }
            set { _CR_CR = value; }
        }
        private string _CR_00;

        public string CR_00
        {
            get { return _CR_00; }
            set { _CR_00 = value; }
        }
        private string _CR_01;

        public string CR_01
        {
            get { return _CR_01; }
            set { _CR_01 = value; }
        }
        private string _CR_02;

        public string CR_02
        {
            get { return _CR_02; }
            set { _CR_02 = value; }
        }
        private string _CR_03;

        public string CR_03
        {
            get { return _CR_03; }
            set { _CR_03 = value; }
        }
        private string _CR_04;

        public string CR_04
        {
            get { return _CR_04; }
            set { _CR_04 = value; }
        }
        private string _CR_05;

        public string CR_05
        {
            get { return _CR_05; }
            set { _CR_05 = value; }
        }
        private string _CR_06;

        public string CR_06
        {
            get { return _CR_06; }
            set { _CR_06 = value; }
        }
        private string _CR_07;

        public string CR_07
        {
            get { return _CR_07; }
            set { _CR_07 = value; }
        }
        private string _CR_08;

        public string CR_08
        {
            get { return _CR_08; }
            set { _CR_08 = value; }
        }
        private string _CR_09;

        public string CR_09
        {
            get { return _CR_09; }
            set { _CR_09 = value; }
        }
        private string _CR_10;

        public string CR_10
        {
            get { return _CR_10; }
            set { _CR_10 = value; }
        }
        private string _CR_11;

        public string CR_11
        {
            get { return _CR_11; }
            set { _CR_11 = value; }
        }
        private string _CR_12;

        public string CR_12
        {
            get { return _CR_12; }
            set { _CR_12 = value; }
        }
        private string _CR_13;

        public string CR_13
        {
            get { return _CR_13; }
            set { _CR_13 = value; }
        }
        private string _CR_14;

        public string CR_14
        {
            get { return _CR_14; }
            set { _CR_14 = value; }
        }
        private string _CR_15;

        public string CR_15
        {
            get { return _CR_15; }
            set { _CR_15 = value; }
        }
        private string _CR_16;

        public string CR_16
        {
            get { return _CR_16; }
            set { _CR_16 = value; }
        }
        private string _CR_17;

        public string CR_17
        {
            get { return _CR_17; }
            set { _CR_17 = value; }
        }
        private string _CR_18;

        public string CR_18
        {
            get { return _CR_18; }
            set { _CR_18 = value; }
        }
        private string _CR_19;

        public string CR_19
        {
            get { return _CR_19; }
            set { _CR_19 = value; }
        }
        private string _CR_20;

        public string CR_20
        {
            get { return _CR_20; }
            set { _CR_20 = value; }
        }
        private string _CR_21;

        public string CR_21
        {
            get { return _CR_21; }
            set { _CR_21 = value; }
        }
        #endregion

        /*
         * Detalle de Crédito (DE) 150 Bytes
         */
        #region Detalle_Credito
        private string _DE_DE;

        public string DE_DE
        {
            get { return _DE_DE; }
            set { _DE_DE = value; }
        }
        private string _DE_00;

        public string DE_00
        {
            get { return _DE_00; }
            set { _DE_00 = value; }
        }
        private string _DE_01;

        public string DE_01
        {
            get { return _DE_01; }
            set { _DE_01 = value; }
        }
        private string _DE_02;

        public string DE_02
        {
            get { return _DE_02; }
            set { _DE_02 = value; }
        }
        private string _DE_03;

        public string DE_03
        {
            get { return _DE_03; }
            set { _DE_03 = value; }
        }
        private string _DE_04;

        public string DE_04
        {
            get { return _DE_04; }
            set { _DE_04 = value; }
        }
        #endregion


        /*
         * Aval (AV) 750 Bytes  
         */
        #region Aval
        private string _AV_AV;

        public string AV_AV
        {
            get { return _AV_AV; }
            set { _AV_AV = value; }
        }
        private string _AV_00;

        public string AV_00
        {
            get { return _AV_00; }
            set { _AV_00 = value; }
        }
        private string _AV_01;

        public string AV_01
        {
            get { return _AV_01; }
            set { _AV_01 = value; }
        }
        private string _AV_02;

        public string AV_02
        {
            get { return _AV_02; }
            set { _AV_02 = value; }
        }
        private string _AV_03;

        public string AV_03
        {
            get { return _AV_03; }
            set { _AV_03 = value; }
        }
        private string _AV_04;

        public string AV_04
        {
            get { return _AV_04; }
            set { _AV_04 = value; }
        }
        private string _AV_05;

        public string AV_05
        {
            get { return _AV_05; }
            set { _AV_05 = value; }
        }
        private string _AV_06;

        public string AV_06
        {
            get { return _AV_06; }
            set { _AV_06 = value; }
        }
        private string _AV_07;

        public string AV_07
        {
            get { return _AV_07; }
            set { _AV_07 = value; }
        }
        private string _AV_08;

        public string AV_08
        {
            get { return _AV_08; }
            set { _AV_08 = value; }
        }
        private string _AV_09;

        public string AV_09
        {
            get { return _AV_09; }
            set { _AV_09 = value; }
        }
        private string _AV_10;

        public string AV_10
        {
            get { return _AV_10; }
            set { _AV_10 = value; }
        }
        private string _AV_11;

        public string AV_11
        {
            get { return _AV_11; }
            set { _AV_11 = value; }
        }
        private string _AV_12;

        public string AV_12
        {
            get { return _AV_12; }
            set { _AV_12 = value; }
        }
        private string _AV_13;

        public string AV_13
        {
            get { return _AV_13; }
            set { _AV_13 = value; }
        }
        private string _AV_14;

        public string AV_14
        {
            get { return _AV_14; }
            set { _AV_14 = value; }
        }
        private string _AV_15;

        public string AV_15
        {
            get { return _AV_15; }
            set { _AV_15 = value; }
        }
        private string _AV_16;

        public string AV_16
        {
            get { return _AV_16; }
            set { _AV_16 = value; }
        }
        private string _AV_17;

        public string AV_17
        {
            get { return _AV_17; }
            set { _AV_17 = value; }
        }
        private string _AV_18;

        public string AV_18
        {
            get { return _AV_18; }
            set { _AV_18 = value; }
        }
        private string _AV_19;

        public string AV_19
        {
            get { return _AV_19; }
            set { _AV_19 = value; }
        }
        private string _AV_20;

        public string AV_20
        {
            get { return _AV_20; }
            set { _AV_20 = value; }
        }
        private string _AV_21;

        public string AV_21
        {
            get { return _AV_21; }
            set { _AV_21 = value; }
        }
        #endregion


        /*
         * Cierre de Archivo (TS) 100 Bytes
         */
        #region Cierre_Archivo
        private string _TS_TS;

        public string TS_TS
        {
            get { return _TS_TS; }
            set { _TS_TS = value; }
        }
        private string _TS_00;

        public string TS_00
        {
            get { return _TS_00; }
            set { _TS_00 = value; }
        }
        private string _TS_01;

        public string TS_01
        {
            get { return _TS_01; }
            set { _TS_01 = value; }
        }
        private string _TS_02;

        public string TS_02
        {
            get { return _TS_02; }
            set { _TS_02 = value; }
        }
        #endregion
    }
}
