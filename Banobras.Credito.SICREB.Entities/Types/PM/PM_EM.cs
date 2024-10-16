using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types.PM
{

    /// <summary>
    /// Segmento Compañia (EM)
    /// </summary>
    public class PM_EM : SegmentoType<PM_EM, PM_Cinta>
    {

        #region Etiquetas del Segmento

            /// <summary>
            /// Identificador del Segmento
            /// </summary>
            public string EM_EM { get; set; }

            /// <summary>
            /// RFC del Acreditado
            /// </summary>
            public string EM_00 { get; set; }

            /// <summary>
            /// Codigo de Ciudadano (CURP en México)
            /// </summary>
            public string EM_01 { get; set; }

            /// <summary>
            /// Reservado
            /// </summary>
            public string EM_02 { get; set; }

            /// <summary>
            /// Nombre de Compañia (PM)
            /// </summary>
            public string EM_03 { get; set; }

            /// <summary>
            /// Primer Nombre (PFAE)
            /// </summary>
            public string EM_04 { get; set; }

            /// <summary>
            /// Segundo Nombre (PFAE)
            /// </summary>
            public string EM_05 { get; set; }

            /// <summary>
            /// Apellido Paterno (PFAE)
            /// </summary>
            public string EM_06 { get; set; }

            /// <summary>
            /// Apellido Materno (PFAE)
            /// </summary>
            public string EM_07 { get; set; }
        
            /// <summary>
            /// Nacionalidad
            /// </summary>
            public string EM_08 { get; set; }

            /// <summary>
            /// Calificacion de Cartera
            /// </summary>
            public string EM_09
            {
                get { return this.CalificacionCartera; }
                set { }
            }

            /// <summary>
            /// Actividad Economica 1 BANXICO/SCIAN
            /// </summary>
            public string EM_10 { get; set; }

            /// <summary>
            /// Actividad Economica 2 BANXICO/SCIAN
            /// </summary>
            public string EM_11 { get; set; }

            /// <summary>
            /// Actividad Economica 3 BANXICO/SCIAN
            /// </summary>
            public string EM_12 { get; set; }

            /// <summary>
            /// Primera Linea de Direccion
            /// </summary>
            public string EM_13 { get; set; }

            /// <summary>
            /// Segunda Linea de Direccion
            /// </summary>
            public string EM_14 { get; set; }

            /// <summary>
            /// Colonia o Poblacion
            /// </summary>
            public string EM_15 { get; set; }

            /// <summary>
            /// Delegacion o Municipio
            /// </summary>
            public string EM_16 { get; set; }

            /// <summary>
            /// Ciudad
            /// </summary>
            public string EM_17 { get; set; }

            /// <summary>
            /// Nombre de Estado para domicilios en México
            /// </summary>
            public string EM_18 { get; set; }

            /// <summary>
            /// Codigo Postal
            /// </summary>
            public string EM_19 { get; set; }

            /// <summary>
            /// Numero de Telefono
            /// </summary>
            public string EM_20 { get; set; }

            /// <summary>
            /// Extension Telefonica
            /// </summary>
            public string EM_21 { get; set; }

            /// <summary>
            /// Numero de Fax
            /// </summary>
            public string EM_22 { get; set; }

            /// <summary>
            /// Tipo de Cliente
            /// </summary>
            public string EM_23 { get; set; }

            /// <summary>
            /// Nombre de Estado en el Pais Extranjero
            /// </summary>
            public string EM_24 { get; set; }

            /// <summary>
            /// Pais de Origen del Domicilio
            /// </summary>
            public string EM_25 { get; set; }

            /// <summary>
            /// Clave de Consolidacion
            /// </summary>
            public string EM_26 { get; set; }
        
            /// <summary>
            /// Filler
            /// </summary>
            public string EM_27 { get; set; }

        #endregion

        #region Constructores

            /// <summary>
            /// Constructor Default
            /// </summary>
            public PM_EM()
                : base(Enums.Persona.Moral)
            {
                // Sin Instrucciones
            }

            public PM_EM(PM_Cinta parent)
                : base(Enums.Persona.Moral, parent)
            {
                this.AuxId = TypedParent.EMs.Count + 1;
            }

            public PM_EM(PM_Cinta parent, bool ValoresOriginales)
                : this(parent)
            {
                respetaOriginales = ValoresOriginales;
            }

        #endregion

        #region Otros Miembros

            private bool respetaOriginales = false;
            
            public int Id { get; set; }

            public bool EsExtranjero
            {
                get
                {
                    return (EM_25 != "MX" & EM_25.Trim() != "");
                }
            }

            public string CalificacionCartera
            {
                get
                {
                    if (CRs != null && CRs.Count > 0)
                    {

                        string calif = (from cal in CRs
                                        where cal.IsValid
                                        orderby cal.Calificacion
                                        select cal.Calificacion).FirstOrDefault();
                        if (!String.IsNullOrWhiteSpace(calif))
                            return calif;
                    }

                    return "NC";
                }
            }

        #endregion

        #region Segmentos de Contenidos

            private List<PM_AC> _acs = null;
            public List<PM_AC> ACs
            {
                get
                {
                    if (_acs == null)
                        _acs = new List<PM_AC>();
                    return _acs;
                }
                private set
                {
                    _acs = value;
                }
            }

            private List<PM_CR> _crs = null;
            public List<PM_CR> CRs
            {
                get
                {
                    if (_crs == null)
                        _crs = new List<PM_CR>();
                    return _crs;
                }
                private set
                {
                    _crs = value;
                }
            }

        #endregion

        public override void InicializaEmpty()
        {
            EM_EM = String.Empty;
            EM_00 = String.Empty;
            EM_01 = String.Empty;
            EM_02 = String.Empty;
            EM_03 = String.Empty;
            EM_04 = String.Empty;
            EM_05 = String.Empty;
            EM_06 = String.Empty;
            EM_07 = String.Empty;
            EM_08 = String.Empty;
            EM_09 = String.Empty;
            EM_10 = String.Empty;
            EM_11 = String.Empty;
            EM_12 = String.Empty;
            EM_13 = String.Empty;
            EM_14 = String.Empty;
            EM_15 = String.Empty;
            EM_16 = String.Empty;
            EM_17 = String.Empty;
            EM_18 = String.Empty;
            EM_19 = String.Empty;
            EM_20 = String.Empty;
            EM_21 = String.Empty;
            EM_22 = String.Empty;
            EM_23 = String.Empty;
            EM_24 = String.Empty;
            EM_25 = String.Empty;
            EM_26 = String.Empty;
            EM_27 = String.Empty;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("EM{0}", Str(EM_EM,   2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(EM_00,  13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(EM_01,  18, TipoDato.Texto));
            sb.AppendFormat("02{0}", Str(EM_02,  10, TipoDato.Numero));
            sb.AppendFormat("03{0}", Str(EM_03, 150, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(EM_04,  30, TipoDato.Texto));
            sb.AppendFormat("05{0}", Str(EM_05,  30, TipoDato.Texto));
            sb.AppendFormat("06{0}", Str(EM_06,  25, TipoDato.Texto));
            sb.AppendFormat("07{0}", Str(EM_07,  25, TipoDato.Texto));
            sb.AppendFormat("08{0}", Str(EM_08,   2, TipoDato.Texto));
            sb.AppendFormat("09{0}", Str(EM_09,   2, TipoDato.Texto));
            sb.AppendFormat("10{0}", Str(EM_10,  11, TipoDato.Numero));
            sb.AppendFormat("11{0}", Str(EM_11,  11, TipoDato.Numero));
            sb.AppendFormat("12{0}", Str(EM_12,  11, TipoDato.Numero));
            sb.AppendFormat("13{0}", Str(EM_13,  40, TipoDato.Texto));
            sb.AppendFormat("14{0}", Str(EM_14,  40, TipoDato.Texto));
            sb.AppendFormat("15{0}", Str(EM_15,  60, TipoDato.Texto));
            sb.AppendFormat("16{0}", Str(EM_16,  40, TipoDato.Texto));
            sb.AppendFormat("17{0}", Str(EM_17,  40, TipoDato.Texto));
            sb.AppendFormat("18{0}", Str(EM_18,   4, TipoDato.Texto));
            sb.AppendFormat("19{0}", Str(EM_19,  10, TipoDato.Texto));
            sb.AppendFormat("20{0}", Str(EM_20,  11, TipoDato.Texto));
            sb.AppendFormat("21{0}", Str(EM_21,   8, TipoDato.Texto));
            sb.AppendFormat("22{0}", Str(EM_22,  11, TipoDato.Texto));
            sb.AppendFormat("23{0}", Str(EM_23,   1, TipoDato.Numero));
            sb.AppendFormat("24{0}", Str(EM_24,  40, TipoDato.Texto));
            sb.AppendFormat("25{0}", Str(EM_25,   2, TipoDato.Texto));
            sb.AppendFormat("26{0}", Str(EM_26,   8, TipoDato.Texto));
            sb.AppendFormat("27{0}", Str(EM_27,  87, TipoDato.Numero));

            return sb.ToString();
        }

        public string ToString(ref System.Data.DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("EM{0}", Str(EM_EM,   2, TipoDato.Texto));
            sb.AppendFormat("00{0}", Str(EM_00,  13, TipoDato.Texto));
            sb.AppendFormat("01{0}", Str(EM_01,  18, TipoDato.Texto));
            sb.AppendFormat("02{0}", Str(EM_02,  10, TipoDato.Numero));
            sb.AppendFormat("03{0}", Str(EM_03, 150, TipoDato.Texto));
            sb.AppendFormat("04{0}", Str(EM_04,  30, TipoDato.Texto));
            sb.AppendFormat("05{0}", Str(EM_05,  30, TipoDato.Texto));
            sb.AppendFormat("06{0}", Str(EM_06,  25, TipoDato.Texto));
            sb.AppendFormat("07{0}", Str(EM_07,  25, TipoDato.Texto));
            sb.AppendFormat("08{0}", Str(EM_08,   2, TipoDato.Texto));
            sb.AppendFormat("09{0}", Str(EM_09,   2, TipoDato.Texto));
            sb.AppendFormat("10{0}", Str(EM_10,  11, TipoDato.Numero));
            sb.AppendFormat("11{0}", Str(EM_11,  11, TipoDato.Numero));
            sb.AppendFormat("12{0}", Str(EM_12,  11, TipoDato.Numero));
            sb.AppendFormat("13{0}", Str(EM_13,  40, TipoDato.Texto));
            sb.AppendFormat("14{0}", Str(EM_14,  40, TipoDato.Texto));
            sb.AppendFormat("15{0}", Str(EM_15,  60, TipoDato.Texto));
            sb.AppendFormat("16{0}", Str(EM_16,  40, TipoDato.Texto));
            sb.AppendFormat("17{0}", Str(EM_17,  40, TipoDato.Texto));
            sb.AppendFormat("18{0}", Str(EM_18,   4, TipoDato.Texto));
            sb.AppendFormat("19{0}", Str(EM_19,  10, TipoDato.Texto));
            sb.AppendFormat("20{0}", Str(EM_20,  11, TipoDato.Texto));
            sb.AppendFormat("21{0}", Str(EM_21,   8, TipoDato.Texto));
            sb.AppendFormat("22{0}", Str(EM_22,  11, TipoDato.Texto));
            sb.AppendFormat("23{0}", Str(EM_23,   1, TipoDato.Numero));
            sb.AppendFormat("24{0}", Str(EM_24,  40, TipoDato.Texto));
            sb.AppendFormat("25{0}", Str(EM_25,   2, TipoDato.Texto));
            sb.AppendFormat("26{0}", Str(EM_26,   8, TipoDato.Texto));
            sb.AppendFormat("27{0}", Str(EM_27,  87, TipoDato.Numero));

            try
            {
                if (ds != null)
                {
                    ds.Tables["EM"].Rows.Add(Str(EM_EM,   2, TipoDato.Texto),
                                             Str(EM_00,  13, TipoDato.Texto),
                                             Str(EM_01,  18, TipoDato.Texto),
                                             Str(EM_02,  10, TipoDato.Numero),
                                             Str(EM_03, 150, TipoDato.Texto),
                                             Str(EM_04,  30, TipoDato.Texto),
                                             Str(EM_05,  30, TipoDato.Texto),
                                             Str(EM_06,  25, TipoDato.Texto),
                                             Str(EM_07,  25, TipoDato.Texto),
                                             Str(EM_08,   2, TipoDato.Texto),
                                             Str(EM_09,   2, TipoDato.Texto),
                                             Str(EM_10,  11, TipoDato.Numero),
                                             Str(EM_11,  11, TipoDato.Numero),
                                             Str(EM_12,  11, TipoDato.Numero),
                                             Str(EM_13,  40, TipoDato.Texto),
                                             Str(EM_14,  40, TipoDato.Texto),
                                             Str(EM_15,  60, TipoDato.Texto),
                                             Str(EM_16,  40, TipoDato.Texto),
                                             Str(EM_17,  40, TipoDato.Texto),
                                             Str(EM_18,   4, TipoDato.Texto),
                                             Str(EM_19,  10, TipoDato.Texto),
                                             Str(EM_20,  11, TipoDato.Texto),
                                             Str(EM_21,   8, TipoDato.Texto),
                                             Str(EM_22,  11, TipoDato.Texto),
                                             Str(EM_23,   1, TipoDato.Numero),
                                             Str(EM_24,  40, TipoDato.Texto),
                                             Str(EM_25,   2, TipoDato.Texto),
                                             Str(EM_26,   8, TipoDato.Texto),
                                             Str(EM_27,  87, TipoDato.Numero)
                                            );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sb.ToString();
        }

        public void AgregaAC(PM_AC ac)
        {
            if (this.ACs == null)
            { this.ACs = new List<PM_AC>(); } 

            this.ACs.Add(ac);
        }

        public void AgregaCR(PM_CR cr)
        {
            if (this.CRs == null)
            { this.CRs = new List<PM_CR>(); } 

            this.CRs.Add(cr);
        }

    }

}
