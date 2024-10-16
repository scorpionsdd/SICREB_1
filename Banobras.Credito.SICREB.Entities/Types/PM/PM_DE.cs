using System;
using System.Text;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PM
{

    /// <summary>
    /// Segmento Detalle Credito (DE)
    /// </summary>
    public class PM_DE : SegmentoType<PM_EM, PM_CR>
    {

        #region Etiquetas del Segmento

            /// <summary>
            /// Identificador del Segmento
            /// </summary>
            public string DE_DE { get; set; }

            /// <summary>
            /// RFC del Acreditado
            /// </summary>
            public string DE_00 { get; set; }

            /// <summary>
            /// Numero de Cuenta, Crédito o Contrato
            /// </summary>
            public string DE_01 { get; set; }

            /// <summary>
            /// Número de Dias de Vencido (Forma de Pago)
            /// </summary>
            public string DE_02 { get; set; }

            /// <summary>
            /// Cantidad (Saldo)
            /// </summary>
            public string DE_03 { get; set; }

        //<MASS 19-oct-2017 ajuste PM-V05

            /// <summary>
            /// Intereses antes Filler
            /// </summary>
            public string DE_04 { get; set; }

            /// <summary>
            /// Filler
            /// </summary>
            public string DE_05 { get; set; }

        //</MASS>



        #endregion

        #region Constructores

            /// <summary>
            /// Constructor Default
            /// </summary>
            public PM_DE()
                : base(Enums.Persona.Moral)
            {
                // Sin Instrucciones
            }

            public PM_DE(PM_CR parent)
                : base(Enums.Persona.Moral, parent)
            {
                this.AuxId = this.TypedParent.DEs.Count + 1;
            }

        #endregion

        #region Otros Miembros

            public int Id { get; set; }
            public int CR_ID { get; set; }
            public string Vigente { get; set; }
            public string Auxiliar { get; set; }
        
        #endregion

        public override void InicializaEmpty()
        {
            DE_DE = String.Empty;
            DE_00 = String.Empty;
            DE_01 = String.Empty;
            DE_02 = String.Empty;
            DE_03 = String.Empty;
            DE_04 = String.Empty;
            //<MASS 19-oct-2017 ajuste PM-V05
            DE_05 = string.Empty;
            //</MASS>
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("DE{0}", Str(DE_DE,  2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(DE_00, 13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(DE_01, 25, TipoDato.Texto));
            sb.AppendFormat("02{0}", Str(DE_02,  3, TipoDato.Numero));
            sb.AppendFormat("03{0}", Str(DE_03, 20, TipoDato.Numero));
            //<MASS 19-oct-2017 ajuste PM-V05
            //sb.AppendFormat("04{0}", Str(DE_04, 75, TipoDato.Texto)); versión anterior
            sb.AppendFormat("04{0}", Str(DE_04, 20, TipoDato.Numero));
            sb.AppendFormat("05{0}", Str(DE_05, 53, TipoDato.Texto));
            //</MASS>

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("DE{0}", Str(DE_DE,  2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(DE_00, 13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(DE_01, 25, TipoDato.Texto));
            sb.AppendFormat("02{0}", Str(DE_02,  3, TipoDato.Numero));
            sb.AppendFormat("03{0}", Str(DE_03, 20, TipoDato.Numero));
            //<MASS 19-oct-2017 ajuste PM-V05
            //sb.AppendFormat("04{0}", Str(DE_04, 75, TipoDato.Texto)); versión anterior
            sb.AppendFormat("04{0}", Str(DE_04, 20, TipoDato.Numero));
            sb.AppendFormat("05{0}", Str(DE_05, 53, TipoDato.Texto));
            //</MASS>


            try
            {
                if (ds != null)
                {
                    ds.Tables["DE"].Rows.Add(Str(DE_DE,  2, TipoDato.Texto),
                                             Str(DE_00, 13, TipoDato.Texto),
                                             Str(DE_01, 25, TipoDato.Texto),
                                             Str(DE_02,  3, TipoDato.Numero),
                                             Str(DE_03, 20, TipoDato.Numero),
                                             //<MASS 19-oct-2017 ajuste PM-V05
                                             //Str(DE_04, 75, TipoDato.Texto) versión anterior
                                             Str(DE_04, 20, TipoDato.Numero),
                                             Str(DE_05, 53, TipoDato.Texto)
                                             //</MASS>
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
