using System;
using System.Text;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PM
{

    /// <summary>
    /// Segmento Encabezado (HD)
    /// </summary>
    public class PM_HD : SegmentoType<PM_Cinta, PM_Cinta>
    {

        #region Etiquetas del Segmento

            /// <summary>
            /// Identificador del Segmento
            /// </summary>
            public string HD_HD { get; set; }
        
            /// <summary>
            /// Clave del Usuario (Institucion)
            /// </summary>
            public string HD_00 { get; set; }
        
            /// <summary>
            /// Clave del Usuario Anterior (Institucion Anterior)
            /// </summary>
            public string HD_01 { get; set; }
        
            /// <summary>
            /// Tipo de Usuario (Institucion)
            /// </summary>
            public string HD_02 { get; set; }
        
            /// <summary>
            /// Tipo de Formato
            /// </summary>
            public string HD_03 { get; set; }
        
            /// <summary>
            /// Fecha de Reporte de Informacion
            /// </summary>
            public string HD_04 { get; set; }
        
            /// <summary>
            /// Periodo (DDMMAAAA)
            /// </summary>
            public string HD_05 { get; set; }

            /// <summary>
            /// Versión del Archivo
            /// </summary>
            public string HD_06 { get; set; }

            /// <summary>
            /// Nombre del Otorgante  
            /// </summary>
            public string HD_07 { get; set; }

            /// <summary>
            /// Filler
            /// </summary>
            public string HD_08 { get; set; }

        #endregion

        #region Constructores

            /// <summary>
            /// Constructor Default
            /// </summary>
            public PM_HD() 
                : base(Enums.Persona.Moral)
            {
                // Sin Instrucciones
            }

            public PM_HD(ISegmentoType parent)
                : base(Enums.Persona.Moral, parent)
            {
                // Sin Instrucciones
            }

        #endregion

        public override void InicializaEmpty()
        {
            HD_HD = String.Empty;
            HD_00 = String.Empty;
            HD_01 = String.Empty;
            HD_02 = String.Empty;
            HD_03 = String.Empty;
            HD_04 = String.Empty;
            HD_05 = String.Empty;
            HD_06 = String.Empty;
            HD_07 = String.Empty;
            HD_08 = String.Empty;

            this.AuxId = 1;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("HD{0}", Str(HD_HD,  5, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(HD_00,  4, TipoDato.Numero));
            sb.AppendFormat("01{0}", Str(HD_01,  4, TipoDato.Numero));
            sb.AppendFormat("02{0}", Str(HD_02,  3, TipoDato.Texto));
            sb.AppendFormat("03{0}", Str(HD_03,  1, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(HD_04,  8, TipoDato.Numero));
            sb.AppendFormat("05{0}", HD_05.Trim().Substring(2), 8, TipoDato.Numero);     //Periodo es el único especial, originalmente viene DDMMAAAA tiene que devolver MMAAAA
            sb.AppendFormat("06{0}", Str(HD_06,  2, TipoDato.Numero));
            sb.AppendFormat("07{0}", Str(HD_07, 75, TipoDato.Texto));
            sb.AppendFormat("08{0}", Str(HD_08, 52, TipoDato.Texto));

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("HD{0}", Str(HD_HD,  5, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(HD_00,  4, TipoDato.Numero));
            sb.AppendFormat("01{0}", Str(HD_01,  4, TipoDato.Numero));
            sb.AppendFormat("02{0}", Str(HD_02,  3, TipoDato.Texto));
            sb.AppendFormat("03{0}", Str(HD_03,  1, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(HD_04,  8, TipoDato.Numero));
            sb.AppendFormat("05{0}", HD_05.Trim().Substring(2), 8, TipoDato.Numero);     //Periodo es el único especial, originalmente viene DDMMAAAA tiene que devolver MMAAAA
            sb.AppendFormat("06{0}", Str(HD_06,  2, TipoDato.Numero));
            sb.AppendFormat("07{0}", Str(HD_07, 75, TipoDato.Texto));
            sb.AppendFormat("08{0}", Str(HD_08, 52, TipoDato.Texto));

            try
            {
                if (ds != null)
                {
                    ds.Tables["HD"].Rows.Add(Str(HD_HD,  5, TipoDato.Texto),
                                             Str(HD_00,  4, TipoDato.Numero),
                                             Str(HD_01,  4, TipoDato.Numero),
                                             Str(HD_02,  3, TipoDato.Texto),
                                             Str(HD_03,  1, TipoDato.Texto),
                                             Str(HD_04,  8, TipoDato.Numero),
                                             Str(HD_05,  8, TipoDato.Numero).Substring(2), //Periodo es el único especial, originalmente viene DDMMAAAA tiene que devolver MMAAAA
                                             Str(HD_06,  2, TipoDato.Numero),  
                                             Str(HD_07, 75, TipoDato.Texto),
                                             Str(HD_08, 52, TipoDato.Texto)
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
