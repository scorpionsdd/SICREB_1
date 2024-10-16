using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PF
{

    public class PF_INTF : SegmentoType<PF_Cinta, PF_Cinta>
    {

        #region  Etiquetas del Segmento
        
            /// <summary>
            /// Etiqueta del Segmento
            /// </summary>
            public string INTF_01 { get; set; }

            /// <summary>
            /// Version 
            /// </summary>
            public string INTF_05 { get; set; }

            /// <summary>
            /// Clave del Usuario
            /// </summary>
            public string INTF_07 { get; set; }

            /// <summary>
            /// Nombre del Usuario
            /// </summary>
            public string INTF_17 { get; set; }

            /// <summary>
            /// Reservado (Numero de Ciclo)
            /// </summary>
            public string INTF_33 { get; set; }
        
            /// <summary>
            /// Fecha de Reporte
            /// </summary>
            public string INTF_35 { get; set; }

            /// <summary>
            /// Reservado (Usu Futuro)
            /// </summary>
            public string INTF_43 { get; set; }
        
            /// <summary>
            /// Informacion Adicional del Usuario
            /// </summary>
            public string INTF_53 { get; set; }

        #endregion

        #region Constructores

            public PF_INTF()
                : base(Enums.Persona.Fisica)
            { 
                // Sin Instrucciones.
            }

            public PF_INTF(PF_Cinta parent)
                : base(Enums.Persona.Fisica, parent)
            {
                this.AuxId = 1;
            }

        #endregion

        public override void InicializaEmpty()
        {
            INTF_01 = string.Empty;
            INTF_05 = string.Empty;
            INTF_07 = string.Empty;
            INTF_17 = string.Empty;
            INTF_33 = string.Empty;
            INTF_35 = string.Empty;
            INTF_43 = string.Empty;
            INTF_53 = string.Empty; 
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(INTF_01.Length > INTF_01.Length ? INTF_01.Substring(0, INTF_01.Length) : INTF_01);
            sb.Append(INTF_05.Length > INTF_05.Length ? INTF_05.Substring(0, INTF_05.Length) : INTF_05);
            sb.Append(INTF_07.Length > 10 ? INTF_07.Substring(0, 10) : INTF_07);
            sb.Append(INTF_17.Length > 16 ? INTF_17.Substring(0, 16) : INTF_17);
            sb.Append(Str(INTF_33, 2, TipoDato.Texto));
            sb.Append(INTF_35.Length > INTF_35.Length ? INTF_35.Substring(0, INTF_35.Length) : INTF_35);
            sb.Append(Str(INTF_43, INTF_43.Length, TipoDato.Texto));
            sb.Append(INTF_53.Length > INTF_53.Length ? INTF_53.Substring(0, INTF_53.Length) : INTF_53);

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(INTF_01.Length > INTF_01.Length ? INTF_01.Substring(0, INTF_01.Length) : INTF_01);
            sb.Append(INTF_05.Length > INTF_05.Length ? INTF_05.Substring(0, INTF_05.Length) : INTF_05);
            sb.Append(INTF_07.Length > 10 ? INTF_07.Substring(0, 10) : INTF_07);
            sb.Append(INTF_17.Length > 16 ? INTF_17.Substring(0, 16) : INTF_17);
            sb.Append(Str(INTF_33, 2, TipoDato.Texto));
            sb.Append(INTF_35.Length > INTF_35.Length ? INTF_35.Substring(0, INTF_35.Length) : INTF_35);
            sb.Append(Str(INTF_43, INTF_43.Length, TipoDato.Texto));
            sb.Append(INTF_53.Length > INTF_53.Length ? INTF_53.Substring(0, INTF_53.Length) : INTF_53);
    
            try
            {
                if (ds != null)
                {
                    ds.Tables["INTF"].Rows.Add((INTF_01.Length > INTF_01.Length ? INTF_01.Substring(0, INTF_01.Length) : INTF_01)
                                             , (INTF_05.Length > INTF_05.Length ? INTF_05.Substring(0, INTF_05.Length) : INTF_05)
                                             , (INTF_07.Length > 10 ? INTF_07.Substring(0, 10) : INTF_07)
                                             , (INTF_17.Length > 16 ? INTF_17.Substring(0, 16) : INTF_17)
                                             , Str(INTF_33, 2, TipoDato.Texto)
                                             , (INTF_35.Length > INTF_35.Length ? INTF_35.Substring(0, INTF_35.Length) : INTF_35)
                                             , Str(INTF_43, INTF_43.Length, TipoDato.Texto)
                                             , (INTF_53.Length > INTF_53.Length ? INTF_53.Substring(0, INTF_53.Length) : INTF_53)
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
