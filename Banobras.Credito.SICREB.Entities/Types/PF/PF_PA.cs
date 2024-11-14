using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

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
            this.PhoneNumberAndExtension(this.PA_07);

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
            this.PhoneNumberAndExtension(this.PA_07);

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
        /// Obtener un número de teléfono válido
        /// </summary>
        /// <param name="telefono">Texto con el número de teléfono</param>
        /// <returns></returns>
        private string PhoneNumberAndExtension(string telefono)
        {
            string phoneNumber = string.Empty;
            string phoneExtension = string.Empty;
            string separator = ";";

            try
            {
                phoneNumber = PA_07.Trim();
                phoneNumber = telefono;

                //Verificar si existe separador y en caso de que lo haya, tomar el primer conjunto de números
                if (phoneNumber.Contains(separator))
                {
                    //Formando lista de números
                    var lista = phoneNumber.Split(separator[0]);

                    //Obteniendo el primer conjunto de números de la lista, sin espacios al inicio y final
                    phoneNumber = lista[0].Trim();
                }

                //Verificar si el número de teléfono resultante, tiene letras. 
                bool containsLetters = phoneNumber.Any(char.IsLetter);

                //Si contiene letras, separa el número y la extensión
                if (containsLetters)
                {
                    string extension = "EXT";
                    int position = 0;

                    //Verificar si la cadena contiene número de extensión
                    if (phoneNumber.Contains(extension) || phoneNumber.Contains(string.Format("{0}.", extension)))
                    {
                        //Si contiene extensión, extraer resultado desde la aparición de la extensión
                        phoneExtension = this.OnlyPhoneExtension(phoneNumber, ref position);
                        this.PA_08 = phoneExtension;
                        //Recalculando el número de teléfono sin extensión, a partir de la aparición de la extensión
                        if (position > 0)
                        {
                            phoneNumber = phoneNumber.Substring(0, position - 1);
                            //Relanzando el proceso para el número resultante
                            phoneNumber = this.PhoneNumberAndExtension(phoneNumber);
                            return string.Empty;
                        }
                        else
                        {
                            //Cuando la posición es 0, indica que la cadena del teléfono sólo contiene número de extensión
                            phoneNumber = string.Empty;
                        }                        
                    }
                    else
                    { 
                    //Tiene otro tipo de letras
                        phoneNumber = this.OnlyPhoneNumber(phoneNumber);
                    }
                }
                else
                { 
                    //Si no contiene, dejarlo como viene
                }

                //Removiendo guiones, espacios en blanco, paréntesis
                phoneNumber = phoneNumber.Replace("-", string.Empty).Replace(" ", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace(".", string.Empty);

                //Cuando es una cadena PAR
                if (phoneNumber.Length % 2 == 0)
                {
                    if (phoneNumber.Length <= 10)
                    {
                        this.PA_07 = phoneNumber;
                    }
                    else if (phoneNumber.Length <= 12)
                    {
                        var reverse = phoneNumber.AsEnumerable().Reverse();
                        var strReverse = String.Join("", reverse);
                        string strReverseTemp = strReverse.Substring(0, 10);
                        var phoneNumberTemp = strReverseTemp.AsEnumerable().Reverse();
                        phoneNumber = String.Join("", phoneNumberTemp);

                        this.PA_07 = phoneNumber;
                    }
                    else
                    {
                        int middle = 8; // phoneNumber.Length / 2;
                        var reverse = phoneNumber.AsEnumerable().Reverse();
                        var strReverse = String.Join("", reverse);
                        string strReverseTemp = strReverse.Substring(0, middle);
                        var phoneNumberTemp = strReverseTemp.AsEnumerable().Reverse();
                        phoneNumber = String.Join("", phoneNumberTemp);

                        this.PA_07 = phoneNumber;
                    }
                }
                else
                { 
                //Cuando es una cadena IMPAR
                    var reverse = phoneNumber.AsEnumerable().Reverse();
                    var strReverse = String.Join("", reverse);
                    string strReverseTemp = strReverse.Substring(0, 10);
                    var phoneNumberTemp = strReverseTemp.AsEnumerable().Reverse();
                    phoneNumber = String.Join("", phoneNumberTemp);

                    this.PA_07 = phoneNumber;
                }
            }
            catch (Exception ex)
            {
                this.PA_07 = phoneNumber;
                this.PA_08 = phoneExtension;
                Console.WriteLine(ex.Message);
            }

            return string.Empty;
        }

        /// <summary>
        /// Obtener números antes de la aparición de una letra-caracter
        /// </summary>
        /// <param name="phoneNumber">Cadena con el número</param>
        /// <returns></returns>
        private string OnlyPhoneNumber(string phoneNumber)
        {
            string tmpPhoneNumber = phoneNumber;

            for (int i = phoneNumber.Length-1; i > 0; i--)
            {
                if (char.IsLetter(phoneNumber[i]))
                {
                    tmpPhoneNumber = phoneNumber.Substring(i+1);
                    break;
                }
            }

            return tmpPhoneNumber;
        }

        /// <summary>
        /// Obtener números antes de la aparición de una letra-caracter
        /// </summary>
        /// <param name="phoneNumber">Cadena con el número</param>
        /// <returns></returns>
        private string OnlyPhoneExtension(string phoneNumber, ref int position)
        {
            string extension = "EXT";
            string tmpPhoneExtension = phoneNumber;
            string founded = string.Empty;

            for (int i = 0; i < phoneNumber.Length; i++)
            {
                if (char.IsLetter(phoneNumber[i]))
                {
                    founded += phoneNumber[i];
                    if (founded.Contains(extension))
                    {
                        tmpPhoneExtension = phoneNumber.Substring(i-2);
                        position = i-2;
                        break;
                    }
                    //tmpPhoneExtension = phoneNumber.Substring(i);
                    //position = i;
                    //break;
                }
            }

            //Verificando si la cadena resultante contiene el texto: EXT ó EXT.
            if (tmpPhoneExtension.Contains(extension) || tmpPhoneExtension.Contains(string.Format("{0}.", extension)))
            {
                //Eliminando el texto: EXT ó EXT.
                tmpPhoneExtension = tmpPhoneExtension.Replace("EXT.", string.Empty).Replace("EXT", string.Empty);

                //Verificar si la cadena resultante tiene letras. 
                bool containsLetters = tmpPhoneExtension.Any(char.IsLetter);
                if (containsLetters)
                {
                    for (int i = 0; i < tmpPhoneExtension.Length; i++)
                    {
                        if (char.IsLetter(tmpPhoneExtension[i]))
                        {
                            //Extrayendo sólo la parte numérica hasta antes de la aparición de un caracter
                            tmpPhoneExtension = tmpPhoneExtension.Substring(0, i);
                            //Removiendo guiones y espacios en blanco
                            tmpPhoneExtension = tmpPhoneExtension.Replace("-", "").Replace(" ", string.Empty);
                            break;
                        }
                    }
                }
            }

            return tmpPhoneExtension;
        }


        

        public void PhoneNumberConvert()
        {
            List<string> numbers = new List<string> { 
                "11034000 EXT 2354;52546441", 
                "CASA 722-217-9700 OFICINA 5270-1607",
                "5270-1200 EXT. 3246 CASA 5604-1305",
                "56715869;56736401;56736368",
                "5669-0756    5543-2356",
                "01-833-2151550",
                "016241205355",
                "(442) 2131448",
                "EXT. 2001 EN LA DELEG. ESTATAL PUEBLA",
                "CASA 01-728-2810-667 OFICINA 5270-1200 EXT. 3177",
                "2601-0100  SUEGRA 5756-6060",
                "5518-7630 OFNA./ CASA 57159910",
                "54260852    OFICINA 5270-1467",
                "5941209 1992-01-25",
                "5781-1540 Y 1590",
                "01(222)2376138",
                "5255-5810 Y 5255-5336",
                "OF. 5520-5542 CASA 5857-2309 CEL. 3233-3117",
                "198-3278 EN SAN LUIS POTOSI",
                "56740934 Ó 39",
                "57107731  5270 1200 EST. 3439",
                "01-444   814-2819",
                "5536-4565 TEL. EN CUERNAVACA 01-777-318-7762",
                "(311) 210-02-63 Y 64 EXT. 108"
            };

            StringBuilder result = new StringBuilder();
            foreach (var telephone in numbers)
            {
                this.PA_07 = string.Empty;
                this.PA_08 = string.Empty;
                var value = this.PhoneNumberAndExtension(telephone);
                result.Append("Original: " + telephone);
                result.Append("   Número: " + this.PA_07);
                result.AppendLine("   Extensión: " + this.PA_08);
            }

            Console.WriteLine(result);
        }

        /// <summary>
        /// Obteniendo sólo el primer teléfono, sin espacios y guiones
        /// </summary>
        /// <param name="telefono">Cadena con el número</param>
        /// <returns></returns>
        private string PhoneNumberValid(string telefono)
        {
            string phoneNumber = string.Empty;
            string separator = ";";
            // Ejemplos exitosos    => 11034000 EXT 2354;52546441  |  5270-1200 EXT. 3246 CASA 5604-1305  |  56715869;56736401;56736368
            // Ejemplos no exitosos => 5669-0756    5543-2356  |  01-833-2151550  |  (442) 2131448  |  EXT. 2001 EN LA DELEG. ESTATAL PUEBLA

            try
            {
                phoneNumber = telefono;

                //Verificar si existe separador y en caso de que lo haya, tomar el primer conjunto de números
                if (telefono.Contains(separator))
                {
                    //Formando lista de números
                    var lista = telefono.Split(separator[0]);

                    //Obteniendo el primer conjunto de números de la lista
                    phoneNumber = lista[0];
                }

                //Verificar si el nuevo conjunto tiene letras. 
                bool containsLetters = phoneNumber.Any(char.IsLetter);

                //Si las contiene, obtener sólo el número hasta la posición de la letra: 11034000 EXT 2354;52546441 => 11034000
                var withOutLetters = this.TextWithOutLetters(phoneNumber);

                //Removiendo guiones y espacios en blanco
                phoneNumber = withOutLetters.Replace("-", "").Replace(" ", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return phoneNumber;
        }

        /// <summary>
        /// Obtener números antes de la aparición de una letra-caracter
        /// </summary>
        /// <param name="phoneNumber">Cadena con el número</param>
        /// <returns></returns>
        private string TextWithOutLetters(string phoneNumber)
        {
            string tmpPhonenumber = phoneNumber;
            //Verificar si el nuevo conjunto tiene letras
            bool containsLetters = phoneNumber.Any(char.IsLetter);

            //Si contiene letras, obtener el conjunto de números hasta antes de la aparición de la primer letra
            if (containsLetters)
            {
                for (int i = 0; i < phoneNumber.Length; i++)
                {
                    if (char.IsLetter(phoneNumber[i]))
                    {
                        tmpPhonenumber = phoneNumber.Substring(0, i);
                        break;
                    }
                }
            }

            return tmpPhonenumber;
        }

    }

}
