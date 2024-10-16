using System;
using System.Text;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PM
{

    /// <summary>
    /// Segmento Fin de Archivo (TS)
    /// </summary>
    public class PM_TS : SegmentoType<PM_Cinta, PM_Cinta>
    {

        #region Etiquetas del Segmento

            /// <summary>
            /// Identificador del Segmento
            /// </summary>
            public string TS_TS { get; set; }

            /// <summary>
            /// Numero de Compañias Reportadas
            /// </summary>
            public string TS_00 { get; set; }

            /// <summary>
            /// Total de Cantidad (Saldo)
            /// </summary>
            public string TS_01 { get; set; }

            /// <summary>
            /// Filler
            /// </summary>
            public string TS_02 { get; set; }

        #endregion

        #region Constructores

            /// <summary>
            /// Constructor Default
            /// </summary>
            public PM_TS()
                : base(Enums.Persona.Moral)
            {
                // Sin Instrucciones
            }

            public PM_TS(PM_Cinta parent)
                : base(Enums.Persona.Moral, parent)
            {
                // Sin Instrucciones
            }

        #endregion

        #region Otros Miembros

            public bool EsValido { get; private set; }

        #endregion

        public override void InicializaEmpty()
        {
            TS_TS = String.Empty;
            TS_00 = String.Empty;
            TS_01 = String.Empty;
            TS_02 = String.Empty;

            this.AuxId = 1;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("TS{0}", Str(TS_TS,  2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(TS_00,  7, TipoDato.Numero));
            sb.AppendFormat("01{0}", Str(TS_01, 30, TipoDato.Numero));
            sb.AppendFormat("02{0}", Str(TS_02, 53, TipoDato.Texto));

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("TS{0}", Str(TS_TS,  2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(TS_00,  7, TipoDato.Numero));
            sb.AppendFormat("01{0}", Str(TS_01, 30, TipoDato.Numero));
            sb.AppendFormat("02{0}", Str(TS_02, 53, TipoDato.Texto));

            try
            {
                if (ds != null)
                {
                    ds.Tables["TS"].Rows.Add(Str(TS_TS,  2, TipoDato.Texto),
                                             Str(TS_00,  7, TipoDato.Numero),
                                             Str(TS_01, 30, TipoDato.Numero),
                                             Str(TS_02, 53, TipoDato.Texto)
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
