using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PF
{

    /// <summary>
    /// Contiene los datos de la dirección del cliente responsable del crédito.
    /// </summary>
    public class PF_PA : SegmentoType<PF_PN, PF_PN>
    {
        
        #region  Etiquetas del Segmento

            /// <summary>
            /// Etiqueta del Segmento
            /// </summary>
            public string PA_Etiqueta { get; set; }

            /// <summary>
            /// Primera Linea de Direccion
            /// </summary>
            public string PA_PA { get; set; }

            /// <summary>
            /// Segunda Linea de Direccion
            /// </summary>
            public string PA_00 { get; set; }

            /// <summary>
            /// Colonia o Poblacion
            /// </summary>
            public string PA_01 { get; set; }

            /// <summary>
            /// Delegacion o Municipio
            /// </summary>
            public string PA_02 { get; set; }

            /// <summary>
            /// Ciudad
            /// </summary>
            public string PA_03 { get; set; }

            /// <summary>
            /// Estado
            /// </summary>
            public string PA_04 { get; set; }

            /// <summary>
            /// Codigo Postal
            /// </summary>
            public string PA_05 { get; set; }
        
            /// <summary>
            /// Fecha de Residencia
            /// </summary>
            public string PA_06 { get; set; }
        
            /// <summary>
            /// Numero de Telefono (En esta Direccion)
            /// </summary>
            public string PA_07 { get; set; }

            /// <summary>
            /// Extension Telefonica
            /// </summary>
            public string PA_08 { get; set; }

            /// <summary>
            /// Numero de Fax (En esta Direccion)
            /// </summary>
            public string PA_09 { get; set; }

            /// <summary>
            /// Tipo de Domicilio
            /// </summary>
            public string PA_10 { get; set; }

            /// <summary>
            /// Indicador Especial de Domicilio
            /// </summary>
            public string PA_11 { get; set; }

            /// <summary>
            /// Origen del Domicilio (Pais)
            /// </summary>
            public string PA_12 { get; set; }

        #endregion

        #region Constructures

            public PF_PA()
                : base(Enums.Persona.Fisica)
            {
                // Sin Instrucciones.
            }

            public PF_PA(PF_PN parent)
                : base(Enums.Persona.Fisica, parent)
            {
                this.AuxId = TypedParent.PAs.Count + 1;
            }

        #endregion

        #region Otros Miembros

            public int Id { get; set; }
            public int PN_ID { get; set; }
            public int ArchivoId { get; set; }

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
                    return PA_12 != "MX" & PA_12.Trim() != "";
                }
            }

            public string DireccionCompleta
            {
                get
                {
                    return string.Format("{0} {1}, {2}", PA_PA, PA_00, PA_01);
                }
            }

        #endregion

        public override void InicializaEmpty()
        {
            PA_PA = string.Empty;
            PA_00 = string.Empty;
            PA_01 = string.Empty;
            PA_02 = string.Empty;
            PA_03 = string.Empty;
            PA_04 = string.Empty;
            PA_05 = string.Empty;
            PA_06 = string.Empty;
            PA_07 = string.Empty;
            PA_08 = string.Empty;
            PA_09 = string.Empty;
            PA_10 = string.Empty;
            PA_11 = string.Empty;
            PA_12 = string.Empty;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            //TODO: SOL53051 => Campos telefono y mail obligatorios
            PA_07 = PA_07.Trim() != string.Empty ? this.PhoneNumberValid(PA_07.Trim()) : "0000000000";

            sb.AppendFormat("PA{0}{1}", PA_PA.Length.ToString("00"), PA_PA);
            sb.AppendFormat((PA_00.Trim() != string.Empty ? "00{0}{1}" : string.Empty), PA_00.Length.ToString("00"), PA_00);
            sb.AppendFormat((PA_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PA_01.Length.ToString("00"), PA_01);
            sb.AppendFormat((PA_02.Trim() != string.Empty ? "02{0}{1}" : string.Empty), PA_02.Length.ToString("00"), PA_02);
            sb.AppendFormat((PA_03.Trim() != string.Empty ? "03{0}{1}" : string.Empty), PA_03.Length.ToString("00"), PA_03);
            sb.AppendFormat("04{0}{1}", PA_04.Length.ToString("00"), PA_04);
            sb.AppendFormat("05{0}{1}", PA_05.Length.ToString("00"), PA_05);
            sb.AppendFormat((PA_06.Trim() != string.Empty ? "06{0}{1}" : string.Empty), PA_06.Length.ToString("00"), PA_06);
            sb.AppendFormat((PA_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PA_07.Length.ToString("00"), PA_07);
            sb.AppendFormat((PA_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PA_08.Length.ToString("00"), PA_08);
            sb.AppendFormat((PA_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PA_09.Length.ToString("00"), PA_09);
            sb.AppendFormat((PA_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PA_10.Length.ToString("00"), PA_10);
            sb.AppendFormat((PA_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PA_11.Length.ToString("00"), PA_11);
            sb.AppendFormat((PA_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PA_12.Length.ToString("00"), PA_12);
            
            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            //TODO: SOL53051 => Campos telefono y mail obligatorios
            PA_07 = PA_07.Trim() != string.Empty ? this.PhoneNumberValid(PA_07.Trim()) : "TELEFONO NO PROPORCIONADO";

            sb.AppendFormat("PA{0}{1}", PA_PA.Length.ToString("00"), PA_PA);
            sb.AppendFormat((PA_00.Trim() != string.Empty ? "00{0}{1}" : string.Empty), PA_00.Length.ToString("00"), PA_00);
            sb.AppendFormat((PA_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PA_01.Length.ToString("00"), PA_01);
            sb.AppendFormat((PA_02.Trim() != string.Empty ? "02{0}{1}" : string.Empty), PA_02.Length.ToString("00"), PA_02);
            sb.AppendFormat((PA_03.Trim() != string.Empty ? "03{0}{1}" : string.Empty), PA_03.Length.ToString("00"), PA_03);
            sb.AppendFormat("04{0}{1}", PA_04.Length.ToString("00"), PA_04);
            sb.AppendFormat("05{0}{1}", PA_05.Length.ToString("00"), PA_05);
            sb.AppendFormat((PA_06.Trim() != string.Empty ? "06{0}{1}" : string.Empty), PA_06.Length.ToString("00"), PA_06);
            sb.AppendFormat((PA_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PA_07.Length.ToString("00"), PA_07);
            sb.AppendFormat((PA_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PA_08.Length.ToString("00"), PA_08);
            sb.AppendFormat((PA_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PA_09.Length.ToString("00"), PA_09);
            sb.AppendFormat((PA_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PA_10.Length.ToString("00"), PA_10);
            sb.AppendFormat((PA_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PA_11.Length.ToString("00"), PA_11);
            sb.AppendFormat((PA_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PA_12.Length.ToString("00"), PA_12);

            try
            {
                if (ds != null)
                {
                    ds.Tables["PA"].Rows.Add(string.Format("PA{0}{1}", PA_PA.Length.ToString("00"), PA_PA),
                                             string.Format((PA_00.Trim() != string.Empty ? "00{0}{1}" : string.Empty), PA_00.Length.ToString("00"), PA_00),
                                             string.Format((PA_01.Trim() != string.Empty ? "01{0}{1}" : string.Empty), PA_01.Length.ToString("00"), PA_01),
                                             string.Format((PA_02.Trim() != string.Empty ? "02{0}{1}" : string.Empty), PA_02.Length.ToString("00"), PA_02),
                                             string.Format((PA_03.Trim() != string.Empty ? "03{0}{1}" : string.Empty), PA_03.Length.ToString("00"), PA_03),
                                             string.Format("04{0}{1}", PA_04.Length.ToString("00"), PA_04),
                                             string.Format("05{0}{1}", PA_05.Length.ToString("00"), PA_05),
                                             string.Format((PA_06.Trim() != string.Empty ? "06{0}{1}" : string.Empty), PA_06.Length.ToString("00"), PA_06),
                                             string.Format((PA_07.Trim() != string.Empty ? "07{0}{1}" : string.Empty), PA_07.Length.ToString("00"), PA_07),
                                             string.Format((PA_08.Trim() != string.Empty ? "08{0}{1}" : string.Empty), PA_08.Length.ToString("00"), PA_08),
                                             string.Format((PA_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), PA_09.Length.ToString("00"), PA_09),
                                             string.Format((PA_10.Trim() != string.Empty ? "10{0}{1}" : string.Empty), PA_10.Length.ToString("00"), PA_10),
                                             string.Format((PA_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), PA_11.Length.ToString("00"), PA_11),
                                             string.Format((PA_12.Trim() != string.Empty ? "12{0}{1}" : string.Empty), PA_12.Length.ToString("00"), PA_12)
                                            );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Obteniendo sólo el primer teléfono, sin espacios y guiones
        /// </summary>
        /// <param name="telefono"></param>
        /// <returns></returns>
        public string PhoneNumberValid(string telefono)
        {
            string phoneNumber = string.Empty;

            try
            {
                var lista = telefono.Split(';');
                phoneNumber = lista[0].Replace("-", "").Replace(" ", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return phoneNumber;
        }

    }

}
