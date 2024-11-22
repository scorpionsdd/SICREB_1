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

            //SOL53051 => Campos telefono y mail obligatorios
            PA_07 = string.IsNullOrEmpty(PA_07.Trim()) ? PA_07 : this.ProcessPhoneNumber(PA_07.Trim());

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

            //SOL53051 => Campos telefono y mail obligatorios
            PA_07 = string.IsNullOrEmpty(PA_07.Trim()) ? PA_07 : this.ProcessPhoneNumber(PA_07.Trim());

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
        /// Obtener número de teléfono y extensión de una cadena.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private string ProcessPhoneNumber(string original)
        {
            string phoneNumberTemp = string.Empty;
            string phoneExtension = string.Empty;
            string textHouse = "CASA";
            string textOffice = "OF";
            try
            {
                // Eliminar de la cadena los caracteres => "-", "(", ")"
                string input = original.Replace("-", string.Empty).Replace("(", string.Empty).Replace(") ", string.Empty).Replace(")", string.Empty);

                // Separar la cadena por ';', 'Y' o 'Ó' y tomar el primer elemento 
                var parts = input.Split(new char[] { ';', 'Y', 'Ó' }, StringSplitOptions.RemoveEmptyEntries);

                //Tomando el primer elemento de la lista
                phoneNumberTemp = parts[0].Trim();

                var isHouse = phoneNumberTemp.IndexOf(textHouse);
                var isOfficeLetter = phoneNumberTemp.IndexOf(textOffice);

                //Si OFICINA está despues de casa, eliminar apartir de OFICINA
                if (isOfficeLetter > isHouse)
                {
                    phoneNumberTemp = phoneNumberTemp.Substring(0, isOfficeLetter).Trim();
                    isHouse = phoneNumberTemp.IndexOf(textHouse);
                    isOfficeLetter = phoneNumberTemp.IndexOf(textOffice);
                }

                //Si trae OFICINA + CASA, pero OFICINA aparece primero que CASA
                if ((isOfficeLetter != -1 && isHouse != -1) && (isOfficeLetter < isHouse))
                {
                    phoneNumberTemp = phoneNumberTemp.Substring(isHouse).Trim();
                    isHouse = phoneNumberTemp.IndexOf(textHouse);
                    isOfficeLetter = phoneNumberTemp.IndexOf(textOffice);
                }

                //Verificando si es número de oficina
                var isOfficeNumber = phoneNumberTemp.IndexOf("5270");
                var isExtension = phoneNumberTemp.IndexOf("EXT");


                //Separando toda la cadena resultante
                var words = phoneNumberTemp.Split(' ');

                //Verificando si es número de oficina
                if (isOfficeNumber != -1)
                {
                    // Eliminar números que comienzan con 5270
                    words = words.Where(data => !(data.StartsWith("5270"))).ToArray();
                    if (isExtension != -1)
                    {
                        int index = Array.IndexOf(words, "EXT") != -1 ? Array.IndexOf(words, "EXT") : Array.IndexOf(words, "EXT.");
                        //Eliminando el elemento con la palabra EXT
                        words = words.Where(data => !(data.StartsWith("EXT"))).ToArray();
                        if (index != -1 && words.Length > index)
                        {
                            var value = words[index];
                            words = words.Where(data => !(data.StartsWith(value))).ToArray();
                        }
                    }
                }

                //Verificando si es de CASA
                if (isHouse != -1)
                {
                    //Eliminando el elemento con la palabra CASA
                    words = words.Where(data => !(data.StartsWith(textHouse))).ToArray();
                    phoneNumberTemp = String.Join("", words);
                    //Si no hay letras en la cadena resultante, unir de nuevo para luego separar
                    if (!phoneNumberTemp.Any(char.IsLetter))
                    {
                        phoneNumberTemp = phoneNumberTemp.Trim();
                        words = phoneNumberTemp.Split(' ');
                    }
                }

                //Verificando si no contiene OFICINA + CASA, pero contiene EXT
                if (isOfficeLetter == -1 && isHouse == -1 && isExtension != -1)
                {
                    int index = Array.IndexOf(words, "EXT") != -1 ? Array.IndexOf(words, "EXT") : Array.IndexOf(words, "EXT.");
                    phoneExtension = words[index + 1];
                }

                //Si el primer elemento es texto, igualar con vacío
                phoneNumberTemp = words[0].Trim().Any(char.IsLetter) ? string.Empty : words[0].Trim();

                //Si el número resultante contiene más de 10 dígitos, obtener los últimos 10
                if (phoneNumberTemp.Length >= 12)
                {
                    var reverse = phoneNumberTemp.AsEnumerable().Reverse();
                    var strReverse = String.Join("", reverse);
                    string strReverseTemp = strReverse.Substring(0, 10);
                    var phoneNumberTemp2 = strReverseTemp.AsEnumerable().Reverse();
                    phoneNumberTemp = String.Join("", phoneNumberTemp2);
                }

                this.PA_07 = phoneNumberTemp;
                this.PA_08 = phoneExtension;
            }
            catch (Exception ex)
            {
                this.PA_07 = string.Empty;
                this.PA_08 = string.Empty;
                Console.WriteLine(ex.Message);
            }


            return this.PA_07;
        }

    }

}
