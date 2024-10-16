using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PF
{

    public class PF_PN : SegmentoType<PF_PN, PF_Cinta>
    {
        
        #region  Etiquetas del Segmento
        
            /// <summary>
            /// Etiqueta del Segmento
            /// </summary>
            public string PN_Etiqueta { get; set; }

            /// <summary>
            /// Apellido Paterno
            /// </summary>
            public string PN_PN { get; set; }
        
            /// <summary>
            /// Apellido Materno
            /// </summary>
            public string PN_00 { get; set; }

            /// <summary>
            /// Apellido Adicional
            /// </summary>
            public string PN_01 { get; set; }

            /// <summary>
            /// Primer Nombre
            /// </summary>
            public string PN_02 { get; set; }

            /// <summary>
            /// Segundo Nombre
            /// </summary>
            public string PN_03 { get; set; }

            /// <summary>
            /// Fecha de Nacimiento
            /// </summary>
            public string PN_04 { get; set; }

            /// <summary>
            /// RFC
            /// </summary>
            public string PN_05 { get; set; }

            /// <summary>
            /// Prefijo Personal o Profesional
            /// </summary>
            public string PN_06 { get; set; }

            /// <summary>
            /// Sufijo Personal del Cliente
            /// </summary>
            public string PN_07 { get; set; }

            /// <summary>
            /// Nacionalidad del Acreditado
            /// </summary>
            public string PN_08 { get; set; }

            /// <summary>
            /// Tipo de Residencia
            /// </summary>
            public string PN_09 { get; set; }

            /// <summary>
            /// Numero de Licencia de Conducir
            /// </summary>
            public string PN_10 { get; set; }

            /// <summary>
            /// Estado Civil
            /// </summary>
            public string PN_11 { get; set; }

            /// <summary>
            /// Sexo
            /// </summary>
            public string PN_12 { get; set; }

            /// <summary>
            /// Numero de Cedula Profesional
            /// </summary>
            public string PN_13 { get; set; }

            /// <summary>
            /// Numero de Registro Electoral (IFE, INE)
            /// </summary>
            public string PN_14 { get; set; }

            /// <summary>
            /// Clave de Identificacion Unica (CURP en México)
            /// </summary>
            public string PN_15 { get; set; }

            /// <summary>
            /// Clave de Pais
            /// </summary>
            public string PN_16 { get; set; }

            /// <summary>
            /// Numero de Dependientes
            /// </summary>
            public string PN_17 { get; set; }

            /// <summary>
            /// Edades de los Dependientes
            /// </summary>
            public string PN_18 { get; set; }

            /// <summary>
            /// Fecha de Defuncion
            /// </summary>
            public string PN_20 { get; set; }

            /// <summary>
            /// Indicador de Defuncion
            /// </summary>
            public string PN_21 { get; set; }

        #endregion

        #region Constructores

            public PF_PN()
                : base(Enums.Persona.Fisica)
            { 
                // Sin Instrucciones.
            }

            public PF_PN(PF_Cinta parent)
                : base(Enums.Persona.Fisica, parent)
            {
                this.AuxId = parent.PNs.Count + 1;
            }

        #endregion

        #region Otros Miembros

            public int Id { get; set; }
            public int ArchivoId { get; set; }

            public bool EsExtranjero
            {
                get
                {
                    return PN_08 != "MX" & PN_08.Trim() != "";
                }
            }

            public string NombreCompleto
            {
                get
                {
                    return string.Format("{0} {1} {2} {3} {4}", this.PN_06, this.PN_02, this.PN_03, this.PN_PN, this.PN_00);
                }
            }

        #endregion

        #region Segmentos Contenidos

            private List<PF_PA> pas = null;
            public List<PF_PA> PAs
            {
                get
                {
                    if (pas == null)
                        pas = new List<PF_PA>();
                    return pas;
                }
                set { pas = value; }
            }

            private List<PF_PE> pes = null;
            public List<PF_PE> PEs {
                get
                {
                    if (pes == null)
                        pes = new List<PF_PE>();
                    return pes;
                }
                set { pes = value; }
            }

            private List<PF_TL> tls = null;
            public List<PF_TL> TLs
            {
                get
                {
                    if (tls == null)
                        tls = new List<PF_TL>();
                    return tls;
                }
                set { tls = value; }
            }

        #endregion

        public override void InicializaEmpty()
        {  
            PN_PN = string.Empty;
            PN_00 = string.Empty;
            PN_01 = string.Empty;
            PN_02 = string.Empty;
            PN_03 = string.Empty;
            PN_04 = string.Empty;
            PN_05 = string.Empty;
            PN_06 = string.Empty;
            PN_07 = string.Empty;
            PN_08 = string.Empty;
            PN_09 = string.Empty;
            PN_10 = string.Empty;
            PN_11 = string.Empty;
            PN_12 = string.Empty;
            PN_13 = string.Empty;
            PN_14 = string.Empty;
            PN_15 = string.Empty;
            PN_16 = string.Empty;
            PN_17 = string.Empty;
            PN_18 = string.Empty;
            PN_20 = string.Empty;
            PN_21 = string.Empty;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("PN{0}{1}", PN_PN.Length.ToString("00"), PN_PN);
            sb.AppendFormat("00{0}{1}", PN_00.Length.ToString("00"), PN_00);
            sb.AppendFormat((PN_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PN_01.Length.ToString("00"), PN_01);
            sb.AppendFormat("02{0}{1}", PN_02.Length.ToString("00"), PN_02);
            sb.AppendFormat("03{0}{1}", PN_03.Length.ToString("00"), PN_03);
            sb.AppendFormat("04{0}{1}", PN_04.Length.ToString("00"), PN_04);
            sb.AppendFormat("05{0}{1}", PN_05.Length.ToString("00"), PN_05);
            sb.AppendFormat((PN_06.Trim() != string.Empty ? "06{0}{1}" : string.Empty), PN_06.Length.ToString("00"), PN_06);
            sb.AppendFormat((PN_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PN_07.Length.ToString("00"), PN_07);
            sb.AppendFormat((PN_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PN_08.Length.ToString("00"), PN_08);
            sb.AppendFormat((PN_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PN_09.Length.ToString("00"), PN_09);
            sb.AppendFormat((PN_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PN_10.Length.ToString("00"), PN_10);
            sb.AppendFormat((PN_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PN_11.Length.ToString("00"), PN_11);
            sb.AppendFormat((PN_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PN_12.Length.ToString("00"), PN_12);
            sb.AppendFormat((PN_13.Trim() != string.Empty ? "13{0}{1}" : string.Empty), PN_13.Length.ToString("00"), PN_13);
            sb.AppendFormat((PN_14.Trim() != string.Empty ? "14{0}{1}" : string.Empty), PN_14.Length.ToString("00"), PN_14);
            sb.AppendFormat((PN_15.Trim() != string.Empty ? "15{0}{1}" : string.Empty), PN_15.Length.ToString("00"), PN_15);
            sb.AppendFormat((PN_16.Trim() != string.Empty ? "16{0}{1}" : string.Empty), PN_16.Length.ToString("00"), PN_16);
            sb.AppendFormat((PN_17.Trim() != string.Empty ? "17{0}{1}" : string.Empty), PN_17.Length.ToString("00"), PN_17);
            sb.AppendFormat((PN_18.Trim() != string.Empty ? "18{0}{1}" : string.Empty), PN_18.Length.ToString("00"), PN_18);
            sb.AppendFormat((PN_20.Trim() != string.Empty ? "20{0}{1}" : string.Empty), PN_20.Length.ToString("00"), PN_20);
            sb.AppendFormat((PN_21.Trim() != string.Empty ? "21{0}{1}" : string.Empty), PN_21.Length.ToString("00"), PN_21);

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("PN{0}{1}", PN_PN.Length.ToString("00"), PN_PN);
            sb.AppendFormat("00{0}{1}", PN_00.Length.ToString("00"), PN_00);
            sb.AppendFormat((PN_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PN_01.Length.ToString("00"), PN_01);
            sb.AppendFormat("02{0}{1}", PN_02.Length.ToString("00"), PN_02);
            sb.AppendFormat("03{0}{1}", PN_03.Length.ToString("00"), PN_03);
            sb.AppendFormat("04{0}{1}", PN_04.Length.ToString("00"), PN_04);
            sb.AppendFormat("05{0}{1}", PN_05.Length.ToString("00"), PN_05);
            sb.AppendFormat((PN_06.Trim() != string.Empty ? "06{0}{1}" : string.Empty), PN_06.Length.ToString("00"), PN_06);
            sb.AppendFormat((PN_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PN_07.Length.ToString("00"), PN_07);
            sb.AppendFormat((PN_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PN_08.Length.ToString("00"), PN_08);
            sb.AppendFormat((PN_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PN_09.Length.ToString("00"), PN_09);
            sb.AppendFormat((PN_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PN_10.Length.ToString("00"), PN_10);
            sb.AppendFormat((PN_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PN_11.Length.ToString("00"), PN_11);
            sb.AppendFormat((PN_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PN_12.Length.ToString("00"), PN_12);
            sb.AppendFormat((PN_13.Trim() != string.Empty ? "13{0}{1}" : string.Empty), PN_13.Length.ToString("00"), PN_13);
            sb.AppendFormat((PN_14.Trim() != string.Empty ? "14{0}{1}" : string.Empty), PN_14.Length.ToString("00"), PN_14);
            sb.AppendFormat((PN_15.Trim() != string.Empty ? "15{0}{1}" : string.Empty), PN_15.Length.ToString("00"), PN_15);
            sb.AppendFormat((PN_16.Trim() != string.Empty ? "16{0}{1}" : string.Empty), PN_16.Length.ToString("00"), PN_16);
            sb.AppendFormat((PN_17.Trim() != string.Empty ? "17{0}{1}" : string.Empty), PN_17.Length.ToString("00"), PN_17);
            sb.AppendFormat((PN_18.Trim() != string.Empty ? "18{0}{1}" : string.Empty), PN_18.Length.ToString("00"), PN_18);
            sb.AppendFormat((PN_20.Trim() != string.Empty ? "20{0}{1}" : string.Empty), PN_20.Length.ToString("00"), PN_20);
            sb.AppendFormat((PN_21.Trim() != string.Empty ? "21{0}{1}" : string.Empty), PN_21.Length.ToString("00"), PN_21);

            try
            {
                if (ds != null)
                {
                    ds.Tables["PN"].Rows.Add(string.Format("PN{0}{1}", PN_PN.Length.ToString("00"), PN_PN),
                                             string.Format("00{0}{1}", PN_00.Length.ToString("00"), PN_00),
                                             string.Format((PN_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PN_01.Length.ToString("00"), PN_01),
                                             string.Format("02{0}{1}", PN_02.Length.ToString("00"), PN_02),
                                             string.Format("03{0}{1}", PN_03.Length.ToString("00"), PN_03),
                                             string.Format("04{0}{1}", PN_04.Length.ToString("00"), PN_04),
                                             string.Format("05{0}{1}", PN_05.Length.ToString("00"), PN_05),
                                             string.Format((PN_06.Trim() != string.Empty ? "06{0}{1}" : string.Empty), PN_06.Length.ToString("00"), PN_06),
                                             string.Format((PN_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PN_07.Length.ToString("00"), PN_07),
                                             string.Format((PN_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PN_08.Length.ToString("00"), PN_08),
                                             string.Format((PN_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PN_09.Length.ToString("00"), PN_09),
                                             string.Format((PN_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PN_10.Length.ToString("00"), PN_10),
                                             string.Format((PN_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PN_11.Length.ToString("00"), PN_11),
                                             string.Format((PN_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PN_12.Length.ToString("00"), PN_12),
                                             string.Format((PN_13.Trim() != string.Empty ? "13{0}{1}" : string.Empty), PN_13.Length.ToString("00"), PN_13),
                                             string.Format((PN_14.Trim() != string.Empty ? "14{0}{1}" : string.Empty), PN_14.Length.ToString("00"), PN_14),
                                             string.Format((PN_15.Trim() != string.Empty ? "15{0}{1}" : string.Empty), PN_15.Length.ToString("00"), PN_15),
                                             string.Format((PN_16.Trim() != string.Empty ? "16{0}{1}" : string.Empty), PN_16.Length.ToString("00"), PN_16),
                                             string.Format((PN_17.Trim() != string.Empty ? "17{0}{1}" : string.Empty), PN_17.Length.ToString("00"), PN_17),
                                             string.Format((PN_18.Trim() != string.Empty ? "18{0}{1}" : string.Empty), PN_18.Length.ToString("00"), PN_18),
                                             string.Format((PN_20.Trim() != string.Empty ? "20{0}{1}" : string.Empty), PN_20.Length.ToString("00"), PN_20),
                                             string.Format((PN_21.Trim() != string.Empty ? "21{0}{1}" : string.Empty), PN_21.Length.ToString("00"), PN_21) 
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
