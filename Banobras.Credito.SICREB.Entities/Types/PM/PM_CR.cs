using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PM
{

    /// <summary>
    /// Segmento Credito (CR)
    /// </summary>
    public class PM_CR : SegmentoType<PM_EM, PM_EM>
    {

        #region Etiquetas de Segmento

            /// <summary>
            /// Identificador del Segmento
            /// </summary>
            public string CR_CR { get; set; }

            /// <summary>
            /// RFC del Acreditado
            /// </summary>
            public string CR_00 { get; set; }

            /// <summary>
            /// Numero de Experiencias Crediticias
            /// </summary>
            public string CR_01 { get; set; }

            /// <summary>
            /// Numero de Credito o Contrato
            /// </summary>
            public string CR_02 { get; set; }

            /// <summary>
            /// Numero de Cuenta, Credito o Contrato Anterior
            /// </summary>
            public string CR_03 { get; set; }

            /// <summary>
            /// Fecha de Apertura del Credito
            /// </summary>
            public string CR_04 { get; set; }

            /// <summary>
            /// Plazo en Meses
            /// </summary>
            public string CR_05 { get; set; }

            /// <summary>
            /// Tipo de Crédito
            /// </summary>
            public string CR_06 { get; set; }

            /// <summary>
            /// Monto Autorizado del Crédito (Saldo Inicial)
            /// </summary>
            public string CR_07 { get; set; }

            /// <summary>
            /// Moneda
            /// </summary>
            public string CR_08 { get; set; }

            /// <summary>
            /// Numero de Pagos  (Código buró de la base de datos)
            /// </summary>
            public string CR_09 { get; set; }

            /// <summary>
            /// Frecuencia de Pagos
            /// </summary>
            public string CR_10 { get; set; }

            /// <summary>
            /// Importe de Pago
            /// </summary>
            public string CR_11 { get; set; }

            /// <summary>
            /// Fecha de Ultimo Pago
            /// </summary>
            public string CR_12 { get; set; }

            /// <summary>
            /// Fecha de Reestructura
            /// </summary>
            public string CR_13 { get; set; }

            /// <summary>
            /// Pago en Efectivo
            /// </summary>
            public string CR_14 { get; set; }

            /// <summary>
            /// Fecha de Liquidacion
            /// </summary>
            public string CR_15 { get; set; }

            /// <summary>
            /// Quita
            /// </summary>
            public string CR_16 { get; set; }

            /// <summary>
            /// Dacion en Pago
            /// </summary>
            public string CR_17 { get; set; }   

            /// <summary>
            /// Quebranto o Castigo
            /// </summary>
            public string CR_18 { get; set; }    

            /// <summary>
            /// Clave de Observacion
            /// </summary>
            public string CR_19 { get; set; }

            /// <summary>
            /// Marca para Crédito Especial
            /// </summary>
            public string CR_20 { get; set; }

            /// <summary>
            /// Fecha de Primer Incumplimiento
            /// </summary>
            public string CR_21 { get; set; }

            /// <summary>
            /// Saldo Insoluto del Principal
            /// </summary>
            public string CR_22 { get; set; }

            /// <summary>
            /// Crédito Máximo Utilizado
            /// </summary>
            public string CR_23 { get; set; }

            /// <summary>
            /// Fecha Ingreso a Cartera Vencida - PM-V05 > MASS 08-nov-2017 antes Filler 
            /// </summary>
            public string CR_24 { get; set; }

            /// <summary>
            /// Filler V05 - MASS 08-nov-2017 PM-V05
            /// </summary>
            public string CR_25 { get; set; }

        #endregion

        #region Constructores

            /// <summary>
            /// Constructor Default
            /// </summary>
            public PM_CR()
                : base(Enums.Persona.Moral)
            {
                // Sin Instrucciones
            }

            public PM_CR(PM_EM parent)
                : base(Enums.Persona.Moral, parent)
            {
                this.AuxId = MainRoot.CRs.Count + 1;
            }

        #endregion

        #region Otros Miembros

            private string _periodoHD = null;

            public int Id { get; set; }
            public int EM_ID { get; set; }
            public int Status { get; set; }
            public int DiasVencido { get; set; }

            public string Auxiliar { get; set; }
            public string Calificacion { get; set; }

            public DateTime Fecha_Cierre { get; set; }   
            public DateTime Fecha_Reestructura { get; set; }
            public DateTime Fecha_Final_Gracia_Capital { get; set; }
            public DateTime Fecha_Final_Gracia_Intereses { get; set; }
            public DateTime Fecha_Maxima_Refinanciamiento { get; set; }

            public double Pago_Cta_13 { get; set; }
            public double Pago_Cta_Orden { get; set; }
            public double Pago_Vigente { get; set; }
            public double MontoPagar { get; set; }
            // JGSP1 Se agrega columna de intereses
            public double Intereses { get; set; }
            public double MontoPagarVencido { get; set; }

            public bool WarningsRequeridos
            {
                get
                {
                    PM_DE detail = GetDetail(true);
                    if (detail == null)
                        return true;

                    int cantidad;
                    int.TryParse(detail.DE_03.ToString(), out cantidad);

                    return (this.Fecha_Cierre == default(DateTime) && cantidad > 0);
                }
            }

            public bool EsExtranjero
            {
                get
                {
                    return this.MainRoot.EsExtranjero;
                }
            }

            public string PeriodoHD05
            {
                // Formato del Periodo DDMMAAAA
                get
                {
                    if (_periodoHD == null)
                    {
                        PM_Cinta cinta = this.MainRoot.Parent as PM_Cinta;
                        if (cinta != null)
                        {
                            _periodoHD = cinta.HD.HD_05;
                        }
                    }
                    return _periodoHD ?? String.Empty;
                }
            }

        #endregion

        #region Segmentos Contenidos

            private List<PM_DE> _des = null;
            public List<PM_DE> DEs
            {
                get
                {
                    if (_des == null)
                        _des = new List<PM_DE>();
                    return _des;
                }
                private set
                {
                    _des = value;
                }
            }

            private List<PM_AV> _avs = null;
            public List<PM_AV> AVs
            {
                get
                {
                    if (_avs == null)
                        _avs = new List<PM_AV>();
                    return _avs;
                }
                private set
                {
                    _avs = value;
                }
            }

        #endregion

        public override void InicializaEmpty()
        {
            CR_CR = String.Empty;
            CR_00 = String.Empty;
            CR_01 = String.Empty;
            CR_02 = String.Empty;
            CR_03 = String.Empty;
            CR_04 = String.Empty;
            CR_05 = String.Empty;
            CR_06 = String.Empty;
            CR_07 = String.Empty;
            CR_08 = String.Empty;
            CR_09 = String.Empty;
            CR_10 = String.Empty;
            CR_11 = String.Empty;
            CR_12 = String.Empty;
            CR_13 = String.Empty;
            CR_14 = String.Empty;
            CR_15 = String.Empty;
            CR_16 = String.Empty;
            CR_17 = String.Empty;
            CR_18 = String.Empty;
            CR_19 = String.Empty;
            CR_20 = String.Empty;
            CR_21 = String.Empty;
            CR_22 = String.Empty;
            CR_23 = String.Empty;
            CR_24 = String.Empty;
            CR_25 = String.Empty; //MASS 08-nov-2017 PM-V05
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("CR{0}", Str(CR_CR,  2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(CR_00, 13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(CR_01,  6, TipoDato.Numero));
            sb.AppendFormat("02{0}", Str(CR_02, 25, TipoDato.Texto));
            sb.AppendFormat("03{0}", Str(CR_03, 25, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(CR_04,  8, TipoDato.Texto));
            sb.AppendFormat("05{0}", Str(CR_05,  6, TipoDato.Numero));
            sb.AppendFormat("06{0}", Str(CR_06,  4, TipoDato.Numero));
            sb.AppendFormat("07{0}", Str(CR_07, 20, TipoDato.Numero));
            sb.AppendFormat("08{0}", Str(CR_08,  3, TipoDato.Numero));
            sb.AppendFormat("09{0}", Str(CR_09,  4, TipoDato.Numero));
            sb.AppendFormat("10{0}", Str(CR_10,  5, TipoDato.Numero));
            sb.AppendFormat("11{0}", Str(CR_11, 20, TipoDato.Numero));
            sb.AppendFormat("12{0}", Str(CR_12,  8, TipoDato.Texto));
            sb.AppendFormat("13{0}", Str(CR_13,  8, TipoDato.Texto));
            sb.AppendFormat("14{0}", Str(CR_14, 20, TipoDato.Numero));
            sb.AppendFormat("15{0}", Str(CR_15,  8, TipoDato.Texto));
            sb.AppendFormat("16{0}", Str(CR_16, 20, TipoDato.Numero));
            sb.AppendFormat("17{0}", Str(CR_17, 20, TipoDato.Numero));
            sb.AppendFormat("18{0}", Str(CR_18, 20, TipoDato.Numero));
            sb.AppendFormat("19{0}", Str(CR_19,  4, TipoDato.Texto));
            sb.AppendFormat("20{0}", Str(CR_20,  1, TipoDato.Texto));
            sb.AppendFormat("21{0}", Str(CR_21,  8, TipoDato.Texto));
            sb.AppendFormat("22{0}", Str(CR_22, 20, TipoDato.Numero));
            sb.AppendFormat("23{0}", Str(CR_23, 20, TipoDato.Numero));            

            //<MASS 08-nov-2017 PM-V05
            //sb.AppendFormat("24{0}", Str(CR_24, 50, TipoDato.Texto));
            sb.AppendFormat("24{0}", Str(CR_24, 8, TipoDato.Texto));
            sb.AppendFormat("25{0}", Str(CR_25, 40, TipoDato.Texto));
            //</MASS>

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("CR{0}", Str(CR_CR,  2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(CR_00, 13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(CR_01,  6, TipoDato.Numero));
            sb.AppendFormat("02{0}", Str(CR_02, 25, TipoDato.Texto));
            sb.AppendFormat("03{0}", Str(CR_03, 25, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(CR_04,  8, TipoDato.Texto));
            sb.AppendFormat("05{0}", Str(CR_05,  6, TipoDato.Numero));
            sb.AppendFormat("06{0}", Str(CR_06,  4, TipoDato.Numero));
            sb.AppendFormat("07{0}", Str(CR_07, 20, TipoDato.Numero));
            sb.AppendFormat("08{0}", Str(CR_08,  3, TipoDato.Numero));
            sb.AppendFormat("09{0}", Str(CR_09,  4, TipoDato.Numero));
            sb.AppendFormat("10{0}", Str(CR_10,  5, TipoDato.Numero));
            sb.AppendFormat("11{0}", Str(CR_11, 20, TipoDato.Numero));
            sb.AppendFormat("12{0}", Str(CR_12,  8, TipoDato.Texto));
            sb.AppendFormat("13{0}", Str(CR_13,  8, TipoDato.Texto));
            sb.AppendFormat("14{0}", Str(CR_14, 20, TipoDato.Numero));
            sb.AppendFormat("15{0}", Str(CR_15,  8, TipoDato.Texto));
            sb.AppendFormat("16{0}", Str(CR_16, 20, TipoDato.Numero));
            sb.AppendFormat("17{0}", Str(CR_17, 20, TipoDato.Numero));
            sb.AppendFormat("18{0}", Str(CR_18, 20, TipoDato.Numero));
            sb.AppendFormat("19{0}", Str(CR_19,  4, TipoDato.Texto));
            sb.AppendFormat("20{0}", Str(CR_20,  1, TipoDato.Texto));
            sb.AppendFormat("21{0}", Str(CR_21,  8, TipoDato.Texto));
            sb.AppendFormat("22{0}", Str(CR_22, 20, TipoDato.Numero));
            sb.AppendFormat("23{0}", Str(CR_23, 20, TipoDato.Numero));

            //<MASS 08-nov-2017 PM-V05
            //sb.AppendFormat("24{0}", Str(CR_24, 50, TipoDato.Texto));
            sb.AppendFormat("24{0}", Str(CR_24, 8, TipoDato.Texto));
            sb.AppendFormat("25{0}", Str(CR_25, 40, TipoDato.Texto));
            //</MASS>

            try
            {
                if (ds != null)
                {
                    ds.Tables["CR"].Rows.Add(Str(CR_CR,  2, TipoDato.Texto),
                                             Str(CR_00, 13, TipoDato.Texto),
                                             Str(CR_01,  6, TipoDato.Numero),
                                             Str(CR_02, 25, TipoDato.Texto),
                                             Str(CR_03, 25, TipoDato.Texto),
                                             Str(CR_04,  8, TipoDato.Texto),
                                             Str(CR_05,  6, TipoDato.Numero),
                                             Str(CR_06,  4, TipoDato.Numero),
                                             Str(CR_07, 20, TipoDato.Numero),
                                             Str(CR_08,  3, TipoDato.Numero),
                                             Str(CR_09,  4, TipoDato.Numero),
                                             Str(CR_10,  5, TipoDato.Numero),
                                             Str(CR_11, 20, TipoDato.Numero),
                                             Str(CR_12,  8, TipoDato.Texto),
                                             Str(CR_13,  8, TipoDato.Texto),
                                             Str(CR_14, 20, TipoDato.Numero),
                                             Str(CR_15,  8, TipoDato.Texto),
                                             Str(CR_16, 20, TipoDato.Numero),
                                             Str(CR_17, 20, TipoDato.Numero),
                                             Str(CR_18, 20, TipoDato.Numero),
                                             Str(CR_19,  4, TipoDato.Texto),
                                             Str(CR_20,  1, TipoDato.Texto),
                                             Str(CR_21,  8, TipoDato.Texto),
                                             Str(CR_22, 20, TipoDato.Numero),
                                             Str(CR_23, 20, TipoDato.Numero),
                                    //<MASS 08-nov-2017 PM-V05
                                             //Str(CR_24, 50, TipoDato.Texto)
                                             Str(CR_24, 8, TipoDato.Texto),
                                             Str(CR_25, 40, TipoDato.Texto)
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
        
        private PM_DE GetDetail(bool activo)
        {
            var de = (from d in this.DEs
                      where (activo && Parser.ToNumber(d.DE_02) == 0) || (!activo && Parser.ToNumber(d.DE_02) > 0)
                      select d).FirstOrDefault();

            return de;
        }

        public void AgregaDE(PM_DE de)
        {
            if (this.DEs == null)
            { this.DEs = new List<PM_DE>(); } 

            this.DEs.Add(de);
        }

        public void AgregaAV(PM_AV av)
        {
            if (this.AVs == null)
            { this.AVs = new List<PM_AV>(); } 

            this.AVs.Add(av);
        }

    }

}
