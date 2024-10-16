using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PF
{

    /// <summary>
    /// Contiene la información de la cuenta o crédito del cliente.
    /// </summary>
    public class PF_TL : SegmentoType<PF_PN, PF_PN>
    {

        #region  Etiquetas del Segmento

            /// <summary>
            /// Etiqueta del Segmento
            /// </summary>
            public string TL_Etiqueta { get; set; }

            /// <summary>
            /// Clave del Usuario
            /// </summary>
            public string TL_01 { get; set; }

            /// <summary>
            /// Nombre del Usuario
            /// </summary>
            public string TL_02 { get; set; }

            /// <summary>
            /// Numero de Cuenta o Credito Actual
            /// </summary>
            public string TL_04 { get; set; }

            /// <summary>
            /// Tipo de Responsabilidad de la Cuenta
            /// </summary>
            public string TL_05 { get; set; }

            /// <summary>
            /// Tipo de Cuenta
            /// </summary>
            public string TL_06 { get; set; }

            /// <summary>
            /// Tipo de Contrato o Producto
            /// </summary>
            public string TL_07 { get; set; }

            /// <summary>
            /// Moneda del Credito
            /// </summary>
            public string TL_08 { get; set; }

            /// <summary>
            /// Importe del Avaluo
            /// </summary>
            public string TL_09 { get; set; }

            /// <summary>
            /// Numero de Pagos
            /// </summary>
            public string TL_10 { get; set; }

            /// <summary>
            /// Frecuencia de Pagos
            /// </summary>
            public string TL_11 { get; set; }

            /// <summary>
            /// Monto a Pagar
            /// </summary>
            public string TL_12 { get; set; }

            /// <summary>
            /// Fecha de Apertura de Cuenta o Credito
            /// </summary>
            public string TL_13 { get; set; }

            /// <summary>
            /// Fecha del Ultimo Pago
            /// </summary>
            public string TL_14 { get; set; }

            /// <summary>
            /// Fecha Ultima Compra o Disposicion
            /// </summary>
            public string TL_15 { get; set; }

            /// <summary>
            /// Fecha Cierre 
            /// </summary>
            public string TL_16 { get; set; }

            /// <summary>
            /// Fecha Reporte de Informacion
            /// </summary>
            public string TL_17 { get; set; }

            /// <summary>
            /// Garantia
            /// </summary>
            public string TL_20 { get; set; }

            /// <summary>
            /// Credito Maximo Autorizado
            /// </summary>
            public string TL_21 { get; set; }

            /// <summary>
            /// Saldo Actual
            /// </summary>
            public string TL_22 { get; set; }

            /// <summary>
            /// Limite de Credito
            /// </summary>
            public string TL_23 { get; set; }

            /// <summary>
            /// Saldo Vencido
            /// </summary>
            public string TL_24 { get; set; }

            /// <summary>
            /// Numero de Pagos Vencidos
            /// </summary>
            public string TL_25 { get; set; }

            /// <summary>
            /// Forma de Pago Actual (MOP)
            /// </summary>
            public string TL_26 { get; set; }

            /// <summary>
            /// Clave de Observacion
            /// </summary>
            public string TL_30 { get; set; }
            
            /// <summary>
            /// Clave del Usuario Anterior
            /// </summary>
            public string TL_39 { get; set; }

            /// <summary>
            /// Nombre del Usuario Anterior
            /// </summary>
            public string TL_40 { get; set; }

            /// <summary>
            /// Numero de Cuenta Anterior
            /// </summary>
            public string TL_41 { get; set; }

            /// <summary>
            /// Fecha de Primer Incumplimiento
            /// </summary>
            public string TL_43 { get; set; }

            /// <summary>
            /// Saldo Insoluto del Principal
            /// </summary>
            public string TL_44 { get; set; }
       
            /// <summary>
            /// Monto del Ultimo Pago (Esta etiqueta no existia cuando debia estar desde la version 12 de Archivo PF)
            /// </summary>
            public string TL_45 { get; set; }

            /// <summary>
            /// Fecha de Ingreso a Cartera Vencida  (Versión 14)
            /// </summary>
            public string TL_46 { get; set; }

            /// <summary>
            /// Monto correspondiente a intereses (Versión 14)
            /// </summary>
            public string TL_47 { get; set; }

            /// <summary>
            /// Froma de Pago (MOP) Actual de los Intereses (Versión 14)
            /// </summary>
            public string TL_48 { get; set; }

            /// <summary>
            /// Dias de Vencido  (Versión 14)
            /// </summary>
            public string TL_49 { get; set; }

            /// <summary>
            /// Plazo en Meses
            /// </summary>
            public string TL_50 { get; set; } 
        
            /// <summary>
            /// Monto de Credito a la Originacion
            /// </summary>
            public string TL_51 { get; set; }

            /// <summary>
            /// TODO: SOL53051 => Campos telefono y mail obligatorios
            /// Correo electrónico del consumidor obligatorio
            ///  </summary>
            public string TL_52 { get; set; } 

            /// <summary>
            ///  Indicador de Fin del Segmento TL
            /// </summary>
            public string TL_99 { get; set; }

        #endregion

        # region Constructores

            public PF_TL()
                : base(Enums.Persona.Fisica)
            {
                // Sin Instrucciones.
            }

            public PF_TL(PF_PN parent)
                : base(Enums.Persona.Fisica, parent)
            {
                this.AuxId = 1;
            }

        #endregion

        #region Otros Miembros

            public int ParentId
            {
                get
                {
                    return Parent.AuxId;
                }
            }

            public int Id { get; set; }
            public int PN_ID { get; set; }
            public int ArchivoId { get; set; }
            public int Grupo { get; set; }
            public String Auxiliar { get; set; }
            public String Rfc { get; set; }
            public Double MontoPagar { get; set; }      

        #endregion

        public override void InicializaEmpty()
        {
            TL_01 = string.Empty;         
            TL_02 = string.Empty;
            TL_04 = string.Empty;
            TL_05 = string.Empty;
            TL_06 = string.Empty;
            TL_07 = string.Empty;
            TL_08 = string.Empty;     
            TL_09 = string.Empty;
            TL_10 = string.Empty;
            TL_11 = string.Empty;
            TL_12 = string.Empty;
            TL_13 = string.Empty;
            TL_14 = string.Empty;
            TL_15 = string.Empty;
            TL_16 = string.Empty;
            TL_17 = string.Empty;
            TL_20 = string.Empty;
            TL_21 = string.Empty;
            TL_22 = string.Empty;
            TL_23 = string.Empty;
            TL_24 = string.Empty;
            TL_25 = string.Empty;
            TL_26 = string.Empty;
            TL_30 = string.Empty;
            TL_39 = string.Empty;
            TL_40 = string.Empty;
            TL_41 = string.Empty;
            TL_43 = string.Empty;
            TL_44 = string.Empty;
            TL_45 = string.Empty;
            TL_46 = string.Empty;
            TL_47 = string.Empty;
            TL_48 = string.Empty;
            TL_49 = string.Empty;
            TL_50 = string.Empty;
            TL_51 = string.Empty;
            //TODO: SOL53051 => Campos telefono y mail obligatorios
            TL_52 = string.Empty;
            TL_99 = string.Empty;   
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{1}{0}{1}", TL_Etiqueta.Length.ToString("00"), TL_Etiqueta);
            sb.AppendFormat("01{0}{1}", TL_01.Length.ToString("00"), TL_01);
            sb.AppendFormat("02{0}{1}", TL_02.Length.ToString("00"), TL_02);
            sb.AppendFormat("04{0}{1}", TL_04.Length.ToString("00"), TL_04);
            sb.AppendFormat((TL_05.Trim() != string.Empty ? "05{0}{1}" : string.Empty), TL_05.Length.ToString("00"), TL_05);
            sb.AppendFormat("06{0}{1}", TL_06.Length.ToString("00"), TL_06);
            sb.AppendFormat("07{0}{1}", TL_07.Length.ToString("00"), TL_07);
            sb.AppendFormat("08{0}{1}", TL_08.Length.ToString("00"), TL_08);
            sb.AppendFormat((TL_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), TL_09.Length.ToString("00"), TL_09);
            sb.AppendFormat("10{0}{1}", TL_10.Length.ToString("00"), TL_10);
            sb.AppendFormat((TL_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), TL_11.Length.ToString("00"), TL_11);
            sb.AppendFormat("12{0}{1}", TL_12.ToString().Length.ToString("00"), TL_12);
            sb.AppendFormat((TL_13.Trim() != "00000000" ? "13{0}{1}" : string.Empty), TL_13.Length.ToString("00"), TL_13);
            sb.AppendFormat((TL_14.Trim() != "00000000" ? "14{0}{1}" : string.Empty), TL_14.Length.ToString("00"), TL_14);
            sb.AppendFormat((TL_15.Trim() != "00000000" ? "15{0}{1}" : string.Empty), TL_15.Length.ToString("00"), TL_15);
            sb.AppendFormat((TL_16.Trim() != "00000000" ? "16{0}{1}" : string.Empty), TL_16.Length.ToString("00"), TL_16);            
            sb.AppendFormat((Parser.ToNumber(TL_17) > 0 ? "17{0}{1}" : string.Empty), TL_17.ToString().Length.ToString("00"), TL_17);
            sb.AppendFormat((TL_20.Trim() != string.Empty ? "20{0}{1}" : string.Empty), TL_20.Length.ToString("00"), TL_20);
            sb.AppendFormat((Parser.ToDouble(TL_21) > 0 ? "21{0}{1}" : string.Empty), TL_21.ToString().Length.ToString("00"), TL_21);
            sb.AppendFormat("22{0}{1}", TL_22.Length.ToString("00"), TL_22);
            sb.AppendFormat("23{0}{1}", TL_23.Length.ToString("00"), TL_23);
            sb.AppendFormat("24{0}{1}", TL_24.Length.ToString("00"), TL_24);
            sb.AppendFormat((TL_25.Trim() != string.Empty ? "25{0}{1}" : string.Empty), TL_25.Length.ToString("00"), TL_25);
            sb.AppendFormat("26{0}{1}", TL_26.Length.ToString("00"), TL_26);
            sb.AppendFormat((TL_30.Trim() != string.Empty ? "30{0}{1}" : string.Empty), TL_30.Length.ToString("00"), TL_30);
            sb.AppendFormat((TL_39.Trim() != string.Empty ? "39{0}{1}" : string.Empty), TL_39.Length.ToString("00"), TL_39);
            sb.AppendFormat((TL_40.Trim() != string.Empty ? "40{0}{1}" : string.Empty), TL_40.Length.ToString("00"), TL_40);
            sb.AppendFormat((TL_41.Trim() != string.Empty ? "41{0}{1}" : string.Empty), TL_41.Length.ToString("00"), TL_41);
            sb.AppendFormat("43{0}{1}", TL_43.Length.ToString("00"), TL_43);            
            sb.AppendFormat("44{0}{1}", (TL_44 == "" ? "01" : TL_44.ToString().Length.ToString("00")), (TL_44 == "" ? "0" : TL_44));
            sb.AppendFormat("45{0}{1}", TL_45.Length.ToString("00"), TL_45);
            sb.AppendFormat("46{0}{1}", TL_46.Length.ToString("00"), TL_46);
            sb.AppendFormat("47{0}{1}", TL_47.Length.ToString("00"), TL_47);
            sb.AppendFormat("48{0}{1}", TL_48.Length.ToString("00"), TL_48);
            sb.AppendFormat("49{0}{1}", TL_49.Length.ToString("00"), TL_49);
            sb.AppendFormat("50{0}{1}", TL_50.Length.ToString("00"), TL_50);
            sb.AppendFormat("51{0}{1}", TL_51.Length.ToString("00"), TL_51);
            //TODO: SOL53051 => Campos telefono y mail obligatorios
            //Correo electrónico del consumidor
            TL_52 = TL_52.Trim() != string.Empty ? TL_52 : "NO PROPORCIONADO";
            sb.AppendFormat("52{0}{1}", TL_52.Length.ToString("00"), TL_52.Trim());
            sb.AppendFormat((TL_99.Trim() != string.Empty ? "99{0}{1}" : string.Empty), TL_99.Length.ToString("00"), TL_99);
            
            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{1}{0}{1}", TL_Etiqueta.Length.ToString("00"), TL_Etiqueta);
            sb.AppendFormat("01{0}{1}", TL_01.Length.ToString("00"), TL_01);
            sb.AppendFormat("02{0}{1}", TL_02.Length.ToString("00"), TL_02);
            sb.AppendFormat("04{0}{1}", TL_04.Length.ToString("00"), TL_04);
            sb.AppendFormat((TL_05.Trim() != string.Empty ? "05{0}{1}" : string.Empty), TL_05.Length.ToString("00"), TL_05);
            sb.AppendFormat("06{0}{1}", TL_06.Length.ToString("00"), TL_06);
            sb.AppendFormat("07{0}{1}", TL_07.Length.ToString("00"), TL_07);
            sb.AppendFormat("08{0}{1}", TL_08.Length.ToString("00"), TL_08);
            sb.AppendFormat((TL_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), TL_09.Length.ToString("00"), TL_09);
            sb.AppendFormat("10{0}{1}", TL_10.Length.ToString("00"), TL_10);
            sb.AppendFormat((TL_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), TL_11.Length.ToString("00"), TL_11);
            sb.AppendFormat("12{0}{1}", TL_12.ToString().Length.ToString("00"), TL_12);
            sb.AppendFormat((TL_13.Trim() != "00000000" ? "13{0}{1}" : string.Empty), TL_13.Length.ToString("00"), TL_13);
            sb.AppendFormat((TL_14.Trim() != "00000000" ? "14{0}{1}" : string.Empty), TL_14.Length.ToString("00"), TL_14);
            sb.AppendFormat((TL_15.Trim() != "00000000" ? "15{0}{1}" : string.Empty), TL_15.Length.ToString("00"), TL_15);
            sb.AppendFormat((TL_16.Trim() != "00000000" ? "16{0}{1}" : string.Empty), TL_16.Length.ToString("00"), TL_16);
            sb.AppendFormat((Parser.ToNumber(TL_17) > 0 ? "17{0}{1}" : string.Empty), TL_17.ToString().Length.ToString("00"), TL_17);
            sb.AppendFormat((TL_20.Trim() != string.Empty ? "20{0}{1}" : string.Empty), TL_20.Length.ToString("00"), TL_20);
            sb.AppendFormat((Parser.ToDouble(TL_21) > 0 ? "21{0}{1}" : string.Empty), TL_21.ToString().Length.ToString("00"), TL_21);
            sb.AppendFormat("22{0}{1}", TL_22.Length.ToString("00"), TL_22);
            sb.AppendFormat("23{0}{1}", TL_23.Length.ToString("00"), TL_23);
            sb.AppendFormat("24{0}{1}", TL_24.Length.ToString("00"), TL_24);
            sb.AppendFormat((TL_25.Trim() != string.Empty ? "25{0}{1}" : string.Empty), TL_25.Length.ToString("00"), TL_25);
            sb.AppendFormat("26{0}{1}", TL_26.Length.ToString("00"), TL_26);
            sb.AppendFormat((TL_30.Trim() != string.Empty ? "30{0}{1}" : string.Empty), TL_30.Length.ToString("00"), TL_30);
            sb.AppendFormat((TL_39.Trim() != string.Empty ? "39{0}{1}" : string.Empty), TL_39.Length.ToString("00"), TL_39);
            sb.AppendFormat((TL_40.Trim() != string.Empty ? "40{0}{1}" : string.Empty), TL_40.Length.ToString("00"), TL_40);
            sb.AppendFormat((TL_41.Trim() != string.Empty ? "41{0}{1}" : string.Empty), TL_41.Length.ToString("00"), TL_41);
            sb.AppendFormat("43{0}{1}", TL_43.Length.ToString("00"), TL_43);
            sb.AppendFormat("44{0}{1}", (TL_44 == "" ? "01" : TL_44.ToString().Length.ToString("00")), (TL_44 == "" ? "0" : TL_44));
            sb.AppendFormat("45{0}{1}", TL_45.Length.ToString("00"), TL_45);

            sb.AppendFormat("46{0}{1}", TL_46.Length.ToString("00"), TL_46);
            sb.AppendFormat("47{0}{1}", TL_47.Length.ToString("00"), TL_47);
            sb.AppendFormat("48{0}{1}", TL_48.Length.ToString("00"), TL_48);
            sb.AppendFormat("49{0}{1}", TL_49.Length.ToString("00"), TL_49);

            sb.AppendFormat("50{0}{1}", TL_50.Length.ToString("00"), TL_50);
            sb.AppendFormat("51{0}{1}", TL_51.Length.ToString("00"), TL_51);
            //TODO: SOL53051 => Campos telefono y mail obligatorios
            //Correo electrónico del consumidor
            TL_52 = TL_52.Trim() != string.Empty ? TL_52 : "NO PROPORCIONADO";
            sb.AppendFormat("52{0}{1}", TL_52.Length.ToString("00"), TL_52.Trim());
            sb.AppendFormat((TL_99.Trim() != string.Empty ? "99{0}{1}" : string.Empty), TL_99.Length.ToString("00"), TL_99);

            try
            {
                if (ds != null)
                {
                    ds.Tables["TL"].Rows.Add(string.Format("{1}{0}{1}", TL_Etiqueta.Length.ToString("00"), TL_Etiqueta),
                                             string.Format("01{0}{1}", TL_01.Length.ToString("00"), TL_01),
                                             string.Format("02{0}{1}", TL_02.Length.ToString("00"), TL_02),
                                             string.Format("04{0}{1}", TL_04.Length.ToString("00"), TL_04),
                                             string.Format((TL_05.Trim() != string.Empty ? "05{0}{1}" : string.Empty), TL_05.Length.ToString("00"), TL_05),
                                             string.Format("06{0}{1}", TL_06.Length.ToString("00"), TL_06),
                                             string.Format("07{0}{1}", TL_07.Length.ToString("00"), TL_07),
                                             string.Format("08{0}{1}", TL_08.Length.ToString("00"), TL_08),
                                             string.Format((TL_09.Trim() != string.Empty ? "09{0}{1}" : string.Empty), TL_09.Length.ToString("00"), TL_09),
                                             string.Format("10{0}{1}", TL_10.Length.ToString("00"), TL_10),
                                             string.Format((TL_11.Trim() != string.Empty ? "11{0}{1}" : string.Empty), TL_11.Length.ToString("00"), TL_11),
                                             string.Format("12{0}{1}", TL_12.ToString().Length.ToString("00"), TL_12),
                                             string.Format((TL_13.Trim() != "00000000" ? "13{0}{1}" : string.Empty), TL_13.Length.ToString("00"), TL_13),
                                             string.Format((TL_14.Trim() != "00000000" ? "14{0}{1}" : string.Empty), TL_14.Length.ToString("00"), TL_14),
                                             string.Format((TL_15.Trim() != "00000000" ? "15{0}{1}" : string.Empty), TL_15.Length.ToString("00"), TL_15),
                                             string.Format((TL_16.Trim() != "00000000" ? "16{0}{1}" : string.Empty), TL_16.Length.ToString("00"), TL_16),
                                             string.Format((Parser.ToNumber(TL_17) > 0 ? "17{0}{1}" : string.Empty), TL_17.ToString().Length.ToString("00"), TL_17),
                                             string.Format((TL_20.Trim() != string.Empty ? "20{0}{1}" : string.Empty), TL_20.Length.ToString("00"), TL_20),
                                             string.Format((Parser.ToDouble(TL_21) > 0 ? "21{0}{1}" : string.Empty), TL_21.ToString().Length.ToString("00"), TL_21),
                                             string.Format("22{0}{1}", TL_22.Length.ToString("00"), TL_22),
                                             string.Format("23{0}{1}", TL_23.Length.ToString("00"), TL_23),
                                             string.Format("24{0}{1}", TL_24.Length.ToString("00"), TL_24),
                                             string.Format((TL_25.Trim() != string.Empty ? "25{0}{1}" : string.Empty), TL_25.Length.ToString("00"), TL_25),
                                             string.Format("26{0}{1}", TL_26.Length.ToString("00"), TL_26),
                                             string.Format((TL_30.Trim() != string.Empty ? "30{0}{1}" : string.Empty), TL_30.Length.ToString("00"), TL_30),
                                             string.Format((TL_39.Trim() != string.Empty ? "39{0}{1}" : string.Empty), TL_39.Length.ToString("00"), TL_39),
                                             string.Format((TL_40.Trim() != string.Empty ? "40{0}{1}" : string.Empty), TL_40.Length.ToString("00"), TL_40),
                                             string.Format((TL_41.Trim() != string.Empty ? "41{0}{1}" : string.Empty), TL_41.Length.ToString("00"), TL_41),
                                             string.Format("43{0}{1}", TL_43.Length.ToString("00"), TL_43),
                                             string.Format("44{0}{1}", (TL_44 == "" ? "01" : TL_44.ToString().Length.ToString("00")), (TL_44 == "" ? "0" : TL_44)),
                                             string.Format("45{0}{1}", TL_45.Length.ToString("00"), TL_45),
                                             string.Format("46{0}{1}", TL_46.Length.ToString("00"), TL_46),
                                             string.Format("47{0}{1}", TL_47.Length.ToString("00"), TL_47),
                                             string.Format("48{0}{1}", TL_48.Length.ToString("00"), TL_48),
                                             string.Format("49{0}{1}", TL_49.Length.ToString("00"), TL_49),
                                             string.Format("50{0}{1}", TL_50.Length.ToString("00"), TL_50),
                                             string.Format("51{0}{1}", TL_51.Length.ToString("00"), TL_51),
                                            //TODO: SOL53051 => Campos telefono y mail obligatorios
                                             //Correo electrónico del consumidor
                                             string.Format("52{0}{1}", TL_52.Length.ToString("00"), TL_52),
                                             string.Format((TL_99.Trim() != string.Empty ? "99{0}{1}" : string.Empty), TL_99.Length.ToString("00"), TL_99)
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

    public class PF_TL_Monto
    {
        public int TL_ID { get; set; }
        public int Grupo { get; set; }
        public string Tipo { get; set; }
        public double Cantidad { get; set; }  
    }

}
