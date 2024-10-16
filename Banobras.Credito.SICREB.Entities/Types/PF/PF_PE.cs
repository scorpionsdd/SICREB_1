using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PF
{

    /// <summary>
    /// Contiene los datos del Empleo del Cliente
    /// </summary>
    public class  PF_PE : SegmentoType<PF_PN, PF_PN>
    {

        #region  Etiquetas del Segmento
        
            /// <summary>
            /// Etiqueta del Segmento
            /// </summary>
            public string PE_Etiqueta { get; set; }

            /// <summary>
            /// Nombre o Razon Social del Empleador
            /// </summary>
            public string PE_PE { get; set; }

            /// <summary>
            /// Primera Linea de Direccion
            /// </summary>
            public string PE_00 { get; set; }

            /// <summary>
            /// Segunda Linea de Direccion
            /// </summary>
            public string PE_01 { get; set; }

            /// <summary>
            /// Colonia o Poblacion
            /// </summary>
            public string PE_02 { get; set; }

            /// <summary>
            /// Delegacion o Municipio
            /// </summary>
            public string PE_03 { get; set; }

            /// <summary>
            /// Ciudad 
            /// </summary>
            public string PE_04 { get; set; }

            /// <summary>
            /// Estado 
            /// </summary>
            public string PE_05 { get; set; }

            /// <summary>
            /// Codigo Postal
            /// </summary>
            public string PE_06 { get; set; }

            /// <summary>
            /// Numero de Telefono (En esta Direccion)
            /// </summary>
            public string PE_07 { get; set; }

            /// <summary>
            /// Extension Telefonica
            /// </summary>
            public string PE_08 { get; set; }

            /// <summary>
            /// Numero de Fax (En esta Direccion)
            /// </summary>
            public string PE_09 { get; set; }

            /// <summary>
            /// Cargo u Ocupacion
            /// </summary>
            public string PE_10 { get; set; }

            /// <summary>
            /// Fecha de Contratacion
            /// </summary>
            public string PE_11 { get; set; }    

            /// <summary>
            /// Clave de la Moneda del Pago del Sueldo 
            /// </summary>
            public string PE_12 { get; set; }
            
            /// <summary>
            /// Monto del Sueldo o Salario
            /// </summary>
            public string PE_13 { get; set; }

            /// <summary>
            /// Periodo de Pago o Base Salarial
            /// </summary>
            public string PE_14 { get; set; }

            /// <summary>
            /// Numero de Empleado
            /// </summary>
            public string PE_15 { get; set; }

            /// <summary>
            /// Fecha del Ultimo Dia de Empleo
            /// </summary>
            public string PE_16 { get; set; }

            /// <summary>
            /// Fecha de Verificacion de Empleo
            /// </summary>
            public string PE_17 { get; set; }

            /// <summary>
            /// Origen de la Razon Social y Domicilio (País)
            /// </summary>
            public string PE_18 { get; set; }

        #endregion

        # region Constructores

            public PF_PE()
                : base(Enums.Persona.Fisica)
            {
                // Sin Instrucciones.
            }

            public PF_PE(PF_PN parent)
                : base(Enums.Persona.Fisica, parent)
            {
                this.AuxId = TypedParent.PEs.Count + 1;
            }

        # endregion

        #region Otros Miembros

            public int Auxiliar { get; set; }
            public bool EsValido { get; private set; }

            public int ParentId
            {
                get
                {
                    return Parent.AuxId;
                }
            }

            public bool EsExtranjero
            {
                get
                {
                    return PE_18 != "MX" & PE_18.Trim() != "";
                }
            }

            public string DireccionCompleta
            {
                get 
                {
                    return string.Format("{0} {1}, {2}", this.PE_00, this.PE_01, this.PE_02);
                }
            }

        #endregion

        public override void InicializaEmpty()
        {
            PE_PE = string.Empty;
            PE_00 = string.Empty;
            PE_01 = string.Empty;
            PE_02 = string.Empty;
            PE_03 = string.Empty;
            PE_04 = string.Empty;
            PE_05 = string.Empty;
            PE_06 = string.Empty;
            PE_07 = string.Empty;
            PE_08 = string.Empty;
            PE_09 = string.Empty;
            PE_10 = string.Empty;
            PE_11 = string.Empty;
            PE_12 = string.Empty;
            PE_13 = string.Empty;
            PE_14 = string.Empty;
            PE_15 = string.Empty;
            PE_16 = string.Empty;
            PE_17 = string.Empty;
            PE_18 = string.Empty;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("PE{0}{1}", PE_PE.Length.ToString("00"), PE_PE);
            sb.AppendFormat((PE_00.Trim() != string.Empty ? "00{0}{1}" : string.Empty), PE_00.Length.ToString("00"), PE_00);
            sb.AppendFormat((PE_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PE_01.Length.ToString("00"), PE_01);
            sb.AppendFormat("02{0}{1}", PE_02.Length.ToString("00"), PE_02);
            sb.AppendFormat("03{0}{1}", PE_03.Length.ToString("00"), PE_03);
            sb.AppendFormat("04{0}{1}", PE_04.Length.ToString("00"), PE_04);
            sb.AppendFormat("05{0}{1}", PE_05.Length.ToString("00"), PE_05);
            sb.AppendFormat("06{0}{1}", PE_06.Length.ToString("00"), PE_06);
            sb.AppendFormat((PE_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PE_07.Length.ToString("00"), PE_07);
            sb.AppendFormat((PE_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PE_08.Length.ToString("00"), PE_08);
            sb.AppendFormat((PE_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PE_09.Length.ToString("00"), PE_09);
            sb.AppendFormat((PE_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PE_10.Length.ToString("00"), PE_10);
            sb.AppendFormat((PE_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PE_11.Length.ToString("00"), PE_11);
            sb.AppendFormat((PE_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PE_12.Length.ToString("00"), PE_12);
            sb.AppendFormat((PE_13.Trim() != string.Empty ? "13{0}{1}" : string.Empty), PE_13.Length.ToString("00"), PE_13);
            sb.AppendFormat((PE_14.Trim() != string.Empty ? "14{0}{1}" : string.Empty), PE_14.Length.ToString("00"), PE_14);
            sb.AppendFormat((PE_15.Trim() != string.Empty ? "15{0}{1}" : string.Empty), PE_15.Length.ToString("00"), PE_15);
            sb.AppendFormat((PE_16.Trim() != string.Empty ? "16{0}{1}" : string.Empty), PE_16.Length.ToString("00"), PE_16);
            sb.AppendFormat((PE_17.Trim() != string.Empty ? "17{0}{1}" : string.Empty), PE_17.Length.ToString("00"), PE_17);
            sb.AppendFormat("18{0}{1}", PE_18.Length.ToString("00"), PE_18);

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("PE{0}{1}", PE_PE.Length.ToString("00"), PE_PE);
            sb.AppendFormat((PE_00.Trim() != string.Empty ? "00{0}{1}" : string.Empty), PE_00.Length.ToString("00"), PE_00);
            sb.AppendFormat((PE_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PE_01.Length.ToString("00"), PE_01);
            sb.AppendFormat("02{0}{1}", PE_02.Length.ToString("00"), PE_02);
            sb.AppendFormat("03{0}{1}", PE_03.Length.ToString("00"), PE_03);
            sb.AppendFormat("04{0}{1}", PE_04.Length.ToString("00"), PE_04);
            sb.AppendFormat("05{0}{1}", PE_05.Length.ToString("00"), PE_05);
            sb.AppendFormat("06{0}{1}", PE_06.Length.ToString("00"), PE_06);
            sb.AppendFormat((PE_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PE_07.Length.ToString("00"), PE_07);
            sb.AppendFormat((PE_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PE_08.Length.ToString("00"), PE_08);
            sb.AppendFormat((PE_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PE_09.Length.ToString("00"), PE_09);
            sb.AppendFormat((PE_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PE_10.Length.ToString("00"), PE_10);
            sb.AppendFormat((PE_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PE_11.Length.ToString("00"), PE_11);
            sb.AppendFormat((PE_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PE_12.Length.ToString("00"), PE_12);
            sb.AppendFormat((PE_13.Trim() != string.Empty ? "13{0}{1}" : string.Empty), PE_13.Length.ToString("00"), PE_13);
            sb.AppendFormat((PE_14.Trim() != string.Empty ? "14{0}{1}" : string.Empty), PE_14.Length.ToString("00"), PE_14);
            sb.AppendFormat((PE_15.Trim() != string.Empty ? "15{0}{1}" : string.Empty), PE_15.Length.ToString("00"), PE_15);
            sb.AppendFormat((PE_16.Trim() != string.Empty ? "16{0}{1}" : string.Empty), PE_16.Length.ToString("00"), PE_16);
            sb.AppendFormat((PE_17.Trim() != string.Empty ? "17{0}{1}" : string.Empty), PE_17.Length.ToString("00"), PE_17);
            sb.AppendFormat("18{0}{1}", PE_18.Length.ToString("00"), PE_18);

            try
            {
                if (ds != null)
                {

                    ds.Tables["PE"].Rows.Add(sb.AppendFormat("PE{0}{1}", PE_PE.Length.ToString("00"), PE_PE),
                                             sb.AppendFormat((PE_00.Trim() != string.Empty ? "00{0}{1}" : string.Empty), PE_00.Length.ToString("00"), PE_00),
                                             sb.AppendFormat((PE_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PE_01.Length.ToString("00"), PE_01),
                                             sb.AppendFormat("02{0}{1}", PE_02.Length.ToString("00"), PE_02),
                                             sb.AppendFormat("03{0}{1}", PE_03.Length.ToString("00"), PE_03),
                                             sb.AppendFormat("04{0}{1}", PE_04.Length.ToString("00"), PE_04),
                                             sb.AppendFormat("05{0}{1}", PE_05.Length.ToString("00"), PE_05),
                                             sb.AppendFormat("06{0}{1}", PE_06.Length.ToString("00"), PE_06),
                                             sb.AppendFormat((PE_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PE_07.Length.ToString("00"), PE_07),
                                             sb.AppendFormat((PE_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PE_08.Length.ToString("00"), PE_08),
                                             sb.AppendFormat((PE_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PE_09.Length.ToString("00"), PE_09),
                                             sb.AppendFormat((PE_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PE_10.Length.ToString("00"), PE_10),
                                             sb.AppendFormat((PE_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PE_11.Length.ToString("00"), PE_11),
                                             sb.AppendFormat((PE_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PE_12.Length.ToString("00"), PE_12),
                                             sb.AppendFormat((PE_13.Trim() != string.Empty ? "13{0}{1}" : string.Empty), PE_13.Length.ToString("00"), PE_13),
                                             sb.AppendFormat((PE_14.Trim() != string.Empty ? "14{0}{1}" : string.Empty), PE_14.Length.ToString("00"), PE_14),
                                             sb.AppendFormat((PE_15.Trim() != string.Empty ? "15{0}{1}" : string.Empty), PE_15.Length.ToString("00"), PE_15),
                                             sb.AppendFormat((PE_16.Trim() != string.Empty ? "16{0}{1}" : string.Empty), PE_16.Length.ToString("00"), PE_16),
                                             sb.AppendFormat((PE_17.Trim() != string.Empty ? "17{0}{1}" : string.Empty), PE_17.Length.ToString("00"), PE_17),
                                             sb.AppendFormat("18{0}{1}", PE_18.Length.ToString("00"), PE_18)
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
