using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PF
{

    public class PF_TR : SegmentoType<PF_Cinta, PF_Cinta>
    {

        #region  Etiquetas del Segmento

            /// <summary>
            /// Etiqueta del Segmento
            /// </summary>
            public string TR_Etiqueta { get; set; }

            /// <summary>
            /// Total de Saldos Actuales
            /// </summary>
            public string TR_05 { get; set; }

            /// <summary>
            /// Total Saldos Vencidos 
            /// </summary>
            public string TR_19 { get; set; }

            /// <summary>
            /// Total de Segmentos INTF
            /// </summary>
            public string TR_33 { get; set; }

            /// <summary>
            /// Total de Segmentos de Nombre del Cliente (PN) 
            /// </summary>
            public string TR_36 { get; set; }

            /// <summary>
            /// Total de Segmentos de Direccion del Cliente (PA)
            /// </summary>
            public string TR_45 { get; set; }

            /// <summary>
            /// Total de Segmentos de Empleado del Cliente (PE)
            /// </summary>
            public string TR_54 { get; set; }

            /// <summary>
            /// Total de Segmentos de Cuenta o Credito del Cliente (TL)
            /// </summary>
            public string TR_63 { get; set; }

            /// <summary>
            /// Contador de Bloques
            /// </summary>
            public string TR_72 { get; set; }

            /// <summary>
            /// Nombre del Usuario para Devolucion
            /// </summary>
            public string TR_78 { get; set; }

            /// <summary>
            /// Direccion del Usuario para Devolucion
            /// </summary>
            public string TR_94 { get; set; }

        #endregion 

        #region Constructores

            public PF_TR()
                : base(Enums.Persona.Fisica)
            {
                // Sin Instrucciones.
            }

            public PF_TR(PF_Cinta parent)
                : base(Enums.Persona.Fisica, parent)
            {
                this.AuxId = 1;
            }

        #endregion

        #region Otros Miembros

            public bool EsValido { get; private set; }

        #endregion

        public void CalcularTR()
        {
            int SegmentosPN = 0;
            int SegmentosPA = 0;
            int SegmentosPE = 0;
            int SegmentosTL = 0;
            double? SaldosActuales = 0;            
            double? SaldosVencidos = 0;
            
            // JAGH en esta parte se comentan todas las validaciones para generar la cinta
            foreach (PF_PN pn in MainRoot.PNs)
            {
                if (pn.IsValid && pn.TLs.Exists(delegate(PF.PF_TL tl) { return tl.IsValid == true; }) && pn.PAs.Exists(delegate(PF.PF_PA pa) { return pa.IsValid == true; }))
                {
                    SegmentosPN++;
                    SegmentosPA++;

                    foreach (PF_TL tl in pn.TLs)
                    {
                        if (tl.IsValid)
                        {
                            // Segmentos PN++;
                            foreach (PF_PE pe in pn.PEs)
                            {
                                if (pe.IsValid)
                                    SegmentosPE++;
                            }
                            
                            //foreach (PF_PA pa in pn.PAs)
                            //{
                            //    if (pa.IsValid)
                            //        SegmentosPA++;
                            //}

                            SegmentosTL++;
                            SaldosActuales += Math.Round(Parser.ToDouble(tl.TL_22));
                            SaldosVencidos += Math.Round(Parser.ToDouble(tl.TL_24));
                        }
                    }
                }
            }

            this.TR_05 = SaldosActuales.ToString();
            this.TR_19 = SaldosVencidos.ToString();
            this.TR_33 = "1";
            this.TR_36 = SegmentosPN.ToString();
            this.TR_45 = SegmentosPA.ToString();
            this.TR_54 = SegmentosPE.ToString();
            this.TR_63 = SegmentosTL.ToString();
        }

        public override void InicializaEmpty()
        {
            TR_05 = string.Empty;
            TR_19 = string.Empty;
            TR_33 = string.Empty;
            TR_36 = string.Empty;
            TR_45 = string.Empty;
            TR_54 = string.Empty;
            TR_63 = string.Empty;
            TR_72 = string.Empty; 
            TR_78 = string.Empty; 
            TR_94 = string.Empty; 
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(TR_Etiqueta);
            sb.Append(TR_05.PadLeft(14, '0'));
            sb.Append(TR_19.PadLeft(14, '0'));
            sb.Append(TR_33.PadLeft( 3, '0'));
            sb.Append(TR_36.PadLeft( 9, '0'));
            sb.Append(TR_45.PadLeft( 9, '0'));
            sb.Append(TR_54.PadLeft( 9, '0'));
            sb.Append(TR_63.PadLeft( 9, '0'));
            sb.Append(TR_72.PadLeft( 6, '0'));
            sb.Append(TR_78.PadRight( 16, ' '));
            sb.Append(TR_94.PadRight(160, ' ')); 

            return sb.ToString();
        }

    }

}
