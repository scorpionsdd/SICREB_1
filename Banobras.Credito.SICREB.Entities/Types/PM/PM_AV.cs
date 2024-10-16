using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PM
{

    /// <summary>
    /// Segmento Aval (AV)
    /// </summary>
    public class PM_AV : SegmentoType<PM_EM, PM_CR>
    {

        #region Etiquetas del Segmento

            /// <summary>
            /// Identificador del Segmento
            /// </summary>
            public string AV_AV { get; set; }

            /// <summary>
            /// RFC del Aval
            /// </summary>
            public string AV_00 { get; set; }

            /// <summary>
            /// Codigo de Ciudadano (CURP en México)
            /// </summary>
            public string AV_01 { get; set; }

            /// <summary>
            /// Campo Reservado
            /// </summary>
            public string AV_02 { get; set; }

            /// <summary>
            /// Nombre de Compañia Aval
            /// </summary>
            public string AV_03 { get; set; }

            /// <summary>
            /// Primer Nombre del Aval
            /// </summary>
            public string AV_04 { get; set; }

            /// <summary>
            /// Segundo Nombre del Aval
            /// </summary>
            public string AV_05 { get; set; }

            /// <summary>
            /// Apellido Paterno del Aval
            /// </summary>
            public string AV_06 { get; set; }

            /// <summary>
            /// Apellido Materno del Aval
            /// </summary>
            public string AV_07 { get; set; }

            /// <summary>
            /// Primera Linea de Dirección del Aval
            /// </summary>
            public string AV_08 { get; set; }

            /// <summary>
            /// Segunda Linea de Direccion del Aval
            /// </summary>
            public string AV_09 { get; set; }

            /// <summary>
            /// Colonia o Población
            /// </summary>
            public string AV_10 { get; set; }

            /// <summary>
            /// Delegación o Municipio
            /// </summary>
            public string AV_11 { get; set; }

            /// <summary>
            /// Ciudad
            /// </summary>
            public string AV_12 { get; set; }

            /// <summary>
            /// Estados (en Mexico)
            /// </summary>        
            public string AV_13 { get; set; }

            /// <summary>
            /// Codigo Postal
            /// </summary>
            public string AV_14 { get; set; }

            /// <summary>
            /// Número de Telefono
            /// </summary>
            public string AV_15 { get; set; }

            /// <summary>
            /// Extensión Telefonica
            /// </summary>
            public string AV_16 { get; set; }

            /// <summary>
            /// Número de Fax
            /// </summary>
            public string AV_17 { get; set; }

            /// <summary>
            /// Tipo de Aval
            /// </summary>
            public string AV_18 { get; set; }

            /// <summary>
            /// Estado (en extranjero)
            /// </summary>
            public string AV_19 { get; set; }

            /// <summary>
            /// Pais Origen del Domicilio
            /// </summary>
            public string AV_20 { get; set; }

            /// <summary>
            /// Filler
            /// </summary>
            public string AV_21 { get; set; }

        #endregion

        #region Constructores

            /// <summary>
            /// Constructor Default
            /// </summary>
            public PM_AV()
                : base(Enums.Persona.Moral)
            {
                // Sin Instrucciones
            }

            public PM_AV(PM_CR parent)
                : base(Enums.Persona.Moral, parent)
            {
                this.AuxId = ParentCR.AVs.Count + 1;
            }

        #endregion

        #region Otros Miembros

            public bool EsExtranjero
            {
                get 
                {
                    return (AV_20 != "MX" & AV_20.Trim() != "");
                }
            }

            public PM_CR ParentCR
            {
                get
                {
                    return Parent as PM_CR;
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
            AV_AV = String.Empty;
            AV_00 = String.Empty;
            AV_01 = String.Empty;
            AV_02 = String.Empty;
            AV_03 = String.Empty;
            AV_04 = String.Empty;
            AV_05 = String.Empty;
            AV_06 = String.Empty;
            AV_07 = String.Empty;
            AV_08 = String.Empty;
            AV_09 = String.Empty;
            AV_10 = String.Empty;
            AV_11 = String.Empty;
            AV_12 = String.Empty;
            AV_13 = String.Empty;
            AV_14 = String.Empty;
            AV_15 = String.Empty;
            AV_16 = String.Empty;
            AV_17 = String.Empty;
            AV_18 = String.Empty;
            AV_19 = String.Empty;
            AV_20 = String.Empty;
            AV_21 = String.Empty;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("AV{0}", Str(AV_AV,   2, TipoDato.Texto)); 
            sb.AppendFormat("00{0}", Str(AV_00,  13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(AV_01,  18, TipoDato.Texto));
            sb.AppendFormat("02{0}", Str(AV_02,  10, TipoDato.Numero));
            sb.AppendFormat("03{0}", Str(AV_03, 150, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(AV_04,  30, TipoDato.Texto));
            sb.AppendFormat("05{0}", Str(AV_05,  30, TipoDato.Texto));
            sb.AppendFormat("06{0}", Str(AV_06,  25, TipoDato.Texto));
            sb.AppendFormat("07{0}", Str(AV_07,  25, TipoDato.Texto));
            sb.AppendFormat("08{0}", Str(AV_08,  40, TipoDato.Texto));
            sb.AppendFormat("09{0}", Str(AV_09,  40, TipoDato.Texto));
            sb.AppendFormat("10{0}", Str(AV_10,  60, TipoDato.Texto));
            sb.AppendFormat("11{0}", Str(AV_11,  40, TipoDato.Texto));
            sb.AppendFormat("12{0}", Str(AV_12,  40, TipoDato.Texto));
            sb.AppendFormat("13{0}", Str(AV_13,   4, TipoDato.Texto));
            sb.AppendFormat("14{0}", Str(AV_14,  10, TipoDato.Texto));
            sb.AppendFormat("15{0}", Str(AV_15,  11, TipoDato.Texto));
            sb.AppendFormat("16{0}", Str(AV_16,   8, TipoDato.Texto));
            sb.AppendFormat("17{0}", Str(AV_17,  11, TipoDato.Texto));
            sb.AppendFormat("18{0}", Str(AV_18,   1, TipoDato.Numero));
            sb.AppendFormat("19{0}", Str(AV_19,  40, TipoDato.Texto));
            sb.AppendFormat("20{0}", Str(AV_20,   2, TipoDato.Texto));
            sb.AppendFormat("21{0}", Str(AV_21,  94, TipoDato.Texto));
          
            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("AV{0}", Str(AV_AV,   2, TipoDato.Texto)); 
            sb.AppendFormat("00{0}", Str(AV_00,  13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(AV_01,  18, TipoDato.Texto));
            sb.AppendFormat("02{0}", Str(AV_02,  10, TipoDato.Numero));
            sb.AppendFormat("03{0}", Str(AV_03, 150, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(AV_04,  30, TipoDato.Texto));
            sb.AppendFormat("05{0}", Str(AV_05,  30, TipoDato.Texto));
            sb.AppendFormat("06{0}", Str(AV_06,  25, TipoDato.Texto));
            sb.AppendFormat("07{0}", Str(AV_07,  25, TipoDato.Texto));
            sb.AppendFormat("08{0}", Str(AV_08,  40, TipoDato.Texto));
            sb.AppendFormat("09{0}", Str(AV_09,  40, TipoDato.Texto));
            sb.AppendFormat("10{0}", Str(AV_10,  60, TipoDato.Texto));
            sb.AppendFormat("11{0}", Str(AV_11,  40, TipoDato.Texto));
            sb.AppendFormat("12{0}", Str(AV_12,  40, TipoDato.Texto));
            sb.AppendFormat("13{0}", Str(AV_13,   4, TipoDato.Texto));
            sb.AppendFormat("14{0}", Str(AV_14,  10, TipoDato.Texto));
            sb.AppendFormat("15{0}", Str(AV_15,  11, TipoDato.Texto));
            sb.AppendFormat("16{0}", Str(AV_16,   8, TipoDato.Texto));
            sb.AppendFormat("17{0}", Str(AV_17,  11, TipoDato.Texto));
            sb.AppendFormat("18{0}", Str(AV_18,   1, TipoDato.Numero));
            sb.AppendFormat("19{0}", Str(AV_19,  40, TipoDato.Texto));
            sb.AppendFormat("20{0}", Str(AV_20,   2, TipoDato.Texto));
            sb.AppendFormat("21{0}", Str(AV_21,  94, TipoDato.Texto));

            try
            {
                if (ds != null)
                {
                    ds.Tables["AV"].Rows.Add(Str(AV_AV,   2, TipoDato.Texto),
                                             Str(AV_00,  13, TipoDato.Texto),
                                             Str(AV_01,  18, TipoDato.Texto),
                                             Str(AV_02,  10, TipoDato.Numero),
                                             Str(AV_03, 150, TipoDato.Texto),
                                             Str(AV_04,  30, TipoDato.Texto),
                                             Str(AV_05,  30, TipoDato.Texto),
                                             Str(AV_06,  25, TipoDato.Texto),
                                             Str(AV_07,  25, TipoDato.Texto),
                                             Str(AV_08,  40, TipoDato.Texto),
                                             Str(AV_09,  40, TipoDato.Texto),
                                             Str(AV_10,  60, TipoDato.Texto),
                                             Str(AV_11,  40, TipoDato.Texto),
                                             Str(AV_12,  40, TipoDato.Texto),
                                             Str(AV_13,   4, TipoDato.Texto),
                                             Str(AV_14,  10, TipoDato.Texto),
                                             Str(AV_15,  11, TipoDato.Texto),
                                             Str(AV_16,   8, TipoDato.Texto),
                                             Str(AV_17,  11, TipoDato.Texto),
                                             Str(AV_18,   1, TipoDato.Numero),
                                             Str(AV_19,  40, TipoDato.Texto),
                                             Str(AV_20,   2, TipoDato.Texto),
                                             Str(AV_21,  94, TipoDato.Texto)
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
