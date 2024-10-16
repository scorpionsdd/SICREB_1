using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PM
{

    /// <summary>
    /// Segmento Accionista (AC)
    /// </summary>
    public class PM_AC : SegmentoType<PM_EM, PM_EM>
    {

        #region Etiquetas del Segmento

            /// <summary>
            /// Identificador del Segmento
            /// </summary>
            public string AC_AC { get; set; }

            /// <summary>
            /// RFC del Accionista
            /// </summary>
            public string AC_00 { get; set; }

            /// <summary>
            /// Codigo de Ciudadano (CURP en México)
            /// </summary>
            public string AC_01 { get; set; }

            /// <summary>
            /// Campo Reservado
            /// </summary>
            public string AC_02 { get; set; }

            /// <summary>
            /// Nombre de la Compañia Accionista
            /// </summary>
            public string AC_03 { get; set; }

            /// <summary>
            /// Primer Nombre del Accionista
            /// </summary>
            public string AC_04 { get; set; }

            /// <summary>
            /// Segundo Nombre del Accionista
            /// </summary>
            public string AC_05 { get; set; }

            /// <summary>
            /// Apellid Paterno del Accionista
            /// </summary>
            public string AC_06 { get; set; }

            /// <summary>
            /// Apellido Materno del Accionista
            /// </summary>
            public string AC_07 { get; set; }

            /// <summary>
            /// Porcentaje del Accionista
            /// </summary>
            public string AC_08 { get; set; }

            /// <summary>
            /// Primer Linea de Dirección del Accionista
            /// </summary>
            public string AC_09 { get; set; }

            /// <summary>
            /// Segunda Linea de Dirección del Accionista 
            /// </summary>
            public string AC_10 { get; set; }

            /// <summary>
            /// Colonia o Población
            /// </summary>
            public string AC_11 { get; set; }

            /// <summary>
            /// Delegacion o Municipio
            /// </summary>
            public string AC_12 { get; set; }

            /// <summary>
            /// Ciudad
            /// </summary>
            public string AC_13 { get; set; }

            /// <summary>
            /// Estado (en Mexico)
            /// </summary>
            public string AC_14 { get; set; }

            /// <summary>
            /// Codigo Postal
            /// </summary>
            public string AC_15 { get; set; }

            /// <summary>
            /// Número de Telefono
            /// </summary>
            public string AC_16 { get; set; }

            /// <summary>
            /// Extension Telefonica
            /// </summary>
            public string AC_17 { get; set; }

            /// <summary>
            /// Número de Fax
            /// </summary>
            public string AC_18 { get; set; }

            /// <summary>
            /// Tipo de Accionista
            /// </summary>
            public string AC_19 { get; set; }

            /// <summary>
            /// Nombre del Estado en el Pais Extranjero
            /// </summary>
            public string AC_20 { get; set; }

            /// <summary>
            /// Pais de Origen del Domicilio
            /// </summary>
            public string AC_21 { get; set; }

            /// <summary>
            /// Filler
            /// </summary>
            public string AC_22 { get; set; }

        #endregion

        #region Constructores

            /// <summary>
            /// Constructor Default
            /// </summary>
            public PM_AC()
                : base(Enums.Persona.Moral)
            {
                // Sin Instrucciones
            }

            public PM_AC(PM_EM parent)
                : base(Enums.Persona.Moral, parent)
            {
                this.AuxId = MainRoot.ACs.Count + 1;
            }

        #endregion

        #region Otros Miembros

            public bool EsExtranjero
            {
                get
                {
                    return AC_21 != "MX" & AC_21.Trim() != "";
                }
            }

            public string RfcAcreditado
            {
                get
                {
                    return this.MainRoot.EM_00;
                }
            }

            public int ParentId
            {
                get
                {
                    return TypedParent.AuxId;
                }
            }

        #endregion

        public override void InicializaEmpty()
        {
            AC_AC = String.Empty;
            AC_00 = String.Empty;
            AC_01 = String.Empty;
            AC_02 = String.Empty;
            AC_03 = String.Empty;
            AC_04 = String.Empty;
            AC_05 = String.Empty;
            AC_06 = String.Empty;
            AC_07 = String.Empty;
            AC_08 = String.Empty;
            AC_09 = String.Empty;
            AC_10 = String.Empty;
            AC_11 = String.Empty;
            AC_12 = String.Empty;
            AC_13 = String.Empty;
            AC_14 = String.Empty;
            AC_15 = String.Empty;
            AC_16 = String.Empty;
            AC_17 = String.Empty;
            AC_18 = String.Empty;
            AC_19 = String.Empty;
            AC_20 = String.Empty;
            AC_21 = String.Empty;
            AC_22 = String.Empty;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("AC{0}", Str(AC_AC,   2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(AC_00,  13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(AC_01,  18, TipoDato.Texto));
            sb.AppendFormat("02{0}", Str(AC_02,  10, TipoDato.Numero));
            sb.AppendFormat("03{0}", Str(AC_03, 150, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(AC_04,  30, TipoDato.Texto));
            sb.AppendFormat("05{0}", Str(AC_05,  30, TipoDato.Texto));
            sb.AppendFormat("06{0}", Str(AC_06,  25, TipoDato.Texto));
            sb.AppendFormat("07{0}", Str(AC_07,  25, TipoDato.Texto));
            sb.AppendFormat("08{0}", Str(AC_08,   2, TipoDato.Numero));
            sb.AppendFormat("09{0}", Str(AC_09,  40, TipoDato.Texto));
            sb.AppendFormat("10{0}", Str(AC_10,  40, TipoDato.Texto));
            sb.AppendFormat("11{0}", Str(AC_11,  60, TipoDato.Texto));
            sb.AppendFormat("12{0}", Str(AC_12,  40, TipoDato.Texto));
            sb.AppendFormat("13{0}", Str(AC_13,  40, TipoDato.Texto));
            sb.AppendFormat("14{0}", Str(AC_14,   4, TipoDato.Texto));
            sb.AppendFormat("15{0}", Str(AC_15,  10, TipoDato.Texto));
            sb.AppendFormat("16{0}", Str(AC_16,  11, TipoDato.Texto));
            sb.AppendFormat("17{0}", Str(AC_17,   8, TipoDato.Texto));
            sb.AppendFormat("18{0}", Str(AC_18,  11, TipoDato.Texto));
            sb.AppendFormat("19{0}", Str(AC_19,   1, TipoDato.Numero));
            sb.AppendFormat("20{0}", Str(AC_20,  40, TipoDato.Texto));
            sb.AppendFormat("21{0}", Str(AC_21,   2, TipoDato.Texto));
            sb.AppendFormat("22{0}", Str(AC_22,  40, TipoDato.Texto));

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("AC{0}", Str(AC_AC,   2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(AC_00,  13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(AC_01,  18, TipoDato.Texto));
            sb.AppendFormat("02{0}", Str(AC_02,  10, TipoDato.Numero));
            sb.AppendFormat("03{0}", Str(AC_03, 150, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(AC_04,  30, TipoDato.Texto));
            sb.AppendFormat("05{0}", Str(AC_05,  30, TipoDato.Texto));
            sb.AppendFormat("06{0}", Str(AC_06,  25, TipoDato.Texto));
            sb.AppendFormat("07{0}", Str(AC_07,  25, TipoDato.Texto));
            sb.AppendFormat("08{0}", Str(AC_08,   2, TipoDato.Numero));
            sb.AppendFormat("09{0}", Str(AC_09,  40, TipoDato.Texto));
            sb.AppendFormat("10{0}", Str(AC_10,  40, TipoDato.Texto));
            sb.AppendFormat("11{0}", Str(AC_11,  60, TipoDato.Texto));
            sb.AppendFormat("12{0}", Str(AC_12,  40, TipoDato.Texto));
            sb.AppendFormat("13{0}", Str(AC_13,  40, TipoDato.Texto));
            sb.AppendFormat("14{0}", Str(AC_14,   4, TipoDato.Texto));
            sb.AppendFormat("15{0}", Str(AC_15,  10, TipoDato.Texto));
            sb.AppendFormat("16{0}", Str(AC_16,  11, TipoDato.Texto));
            sb.AppendFormat("17{0}", Str(AC_17,   8, TipoDato.Texto));
            sb.AppendFormat("18{0}", Str(AC_18,  11, TipoDato.Texto));
            sb.AppendFormat("19{0}", Str(AC_19,   1, TipoDato.Numero));
            sb.AppendFormat("20{0}", Str(AC_20,  40, TipoDato.Texto));
            sb.AppendFormat("21{0}", Str(AC_21,   2, TipoDato.Texto));
            sb.AppendFormat("22{0}", Str(AC_22,  40, TipoDato.Texto));

            try
            {
                if (ds != null)
                {
                    ds.Tables["AC"].Rows.Add(Str(AC_AC,   2, TipoDato.Texto),
                                             Str(AC_00,  13, TipoDato.Texto),
                                             Str(AC_01,  18, TipoDato.Texto),
                                             Str(AC_02,  10, TipoDato.Numero),
                                             Str(AC_03, 150, TipoDato.Texto),
                                             Str(AC_04,  30, TipoDato.Texto),
                                             Str(AC_05,  30, TipoDato.Texto),
                                             Str(AC_06,  25, TipoDato.Texto),
                                             Str(AC_07,  25, TipoDato.Texto),
                                             Str(AC_08,   2, TipoDato.Numero),
                                             Str(AC_09,  40, TipoDato.Texto),
                                             Str(AC_10,  40, TipoDato.Texto),
                                             Str(AC_11,  60, TipoDato.Texto),
                                             Str(AC_12,  40, TipoDato.Texto),
                                             Str(AC_13,  40, TipoDato.Texto),
                                             Str(AC_14,   4, TipoDato.Texto),
                                             Str(AC_15,  10, TipoDato.Texto),
                                             Str(AC_16,  11, TipoDato.Texto),
                                             Str(AC_17,   8, TipoDato.Texto),
                                             Str(AC_18,  11, TipoDato.Texto),
                                             Str(AC_19,   1, TipoDato.Numero),
                                             Str(AC_20,  40, TipoDato.Texto),
                                             Str(AC_21,   2, TipoDato.Texto),
                                             Str(AC_22,  40, TipoDato.Texto)
                                            );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sb.ToString();
        }

    }

}
