namespace Banobras.Credito.SICREB.Business.Rules.PF
{


    public class ArchivoPersonasFisica
    {

        //public List<PF_Cinta> SegmentosCredito = new List<PF_Cinta>();
        //public PF_INTF SegmentoEncabezado { get; set; }
        //private PF_TR SegmentoCierre { get; set; }
        //public List<Validacion> Validaciones { get; private set; }
        public ArchivoPersonasFisica()
        {

        }

        //public void AgregarRegistroCinta(PF_Cinta Registro)
        //{

        //    SegmentosCredito.Add(Registro);


        //}




        //public string GenerarCinta()
        //{
        //    VerificarErroresAdvertencias();
        //    StringBuilder Cinta = new StringBuilder();
        //    int Espacios;
        //    Cinta.Append(SegmentoEncabezado.INTF_01);
        //    Cinta.Append(SegmentoEncabezado.INTF_05);
        //    Cinta.Append(SegmentoEncabezado.INTF_07);
        //    Espacios = 10 - SegmentoEncabezado.INTF_07.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append(" ");
        //    Cinta.Append(SegmentoEncabezado.INTF_17);
        //    Espacios = 16 - SegmentoEncabezado.INTF_17.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append(" ");
        //    Cinta.Append(SegmentoEncabezado.INTF_33);
        //    Espacios = 2 - SegmentoEncabezado.INTF_33.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append(" ");
        //    Cinta.Append(SegmentoEncabezado.INTF_35);
        //    Cinta.Append(SegmentoEncabezado.INTF_43);
        //    Cinta.Append(SegmentoEncabezado.INTF_53);
        //    Espacios = 98 - SegmentoEncabezado.INTF_53.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append(" ");



        //    foreach (PF_Cinta DatosCinta in SegmentosCredito)
        //    {
        //        AgregarEtiqueta(Cinta, DatosCinta.SegmentoNombre.PN_Etiqueta);
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoNombre.PN_PN, string.Empty);
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoNombre.PN_00, "00");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_01, "01");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoNombre.PN_02, "02");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_03, "03");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_04, "04");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoNombre.PN_05, "05");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_06, "06");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_07, "07");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_08, "08");
        //        AgregarEntero(Cinta, DatosCinta.SegmentoNombre.PN_09, "09");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_10, "10");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_11, "11");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_12, "12");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_13, "13");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_14, "14");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_15, "15");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_16, "16");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_17, "17");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_18, "18");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_20, "20");
        //        AgregarDato(Cinta, DatosCinta.SegmentoNombre.PN_21, "21");

        //        AgregarEtiqueta(Cinta, DatosCinta.SegmentoDireccion01.PA_Etiqueta);
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion01.PA_PA, string.Empty);
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_00, "00");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_01, "01");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_02, "02");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_03, "03");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion01.PA_04, "04");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion01.PA_05, "05");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_06, "06");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_07, "07");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_08, "08");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_09, "09");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_10, "10");
        //        AgregarDato(Cinta, DatosCinta.SegmentoDireccion01.PA_11, "11");
        //        if (DatosCinta.SegmentoDireccion02 != null)
        //        {

        //            AgregarEtiqueta(Cinta, DatosCinta.SegmentoDireccion01.PA_Etiqueta);
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion01.PA_PA, string.Empty);
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_00, "00");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_01, "01");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_02, "02");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_03, "03");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion02.PA_04, "04");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion02.PA_05, "05");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_06, "06");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_07, "07");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_08, "08");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_09, "09");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_10, "10");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion02.PA_11, "11");
        //        }
        //        if (DatosCinta.SegmentoDireccion03 != null)
        //        {

        //            AgregarEtiqueta(Cinta, DatosCinta.SegmentoDireccion03.PA_Etiqueta);
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion03.PA_PA, string.Empty);
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_00, "00");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_01, "01");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_02, "02");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_03, "03");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion03.PA_04, "04");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion03.PA_05, "05");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_06, "06");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_07, "07");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_08, "08");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_09, "09");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_10, "10");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion03.PA_11, "11");
        //        }

        //        if (DatosCinta.SegmentoDireccion04 != null)
        //        {

        //            AgregarEtiqueta(Cinta, DatosCinta.SegmentoDireccion04.PA_Etiqueta);
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion04.PA_PA, string.Empty);
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_00, "00");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_01, "01");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_02, "02");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_03, "03");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion04.PA_04, "04");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoDireccion04.PA_05, "05");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_06, "06");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_07, "07");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_08, "08");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_09, "09");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_10, "10");
        //            AgregarDato(Cinta, DatosCinta.SegmentoDireccion04.PA_11, "11");
        //        }

        //        if (DatosCinta.SegmentoEmpleo01 != null)
        //        {
        //            AgregarEtiqueta(Cinta, DatosCinta.SegmentoEmpleo01.PE_Etiqueta);
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoEmpleo01.PE_PE, string.Empty);
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoEmpleo01.PE_00, "00");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_01, "01");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_02, "02");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_03, "03");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_04, "04");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoEmpleo01.PE_05, "05");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoEmpleo01.PE_06, "06");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_07, "07");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_08, "08");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_09, "09");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_10, "10");
        //            AgregarEntero(Cinta, DatosCinta.SegmentoEmpleo01.PE_11, "11");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_12, "12");
        //            AgregarEntero(Cinta, DatosCinta.SegmentoEmpleo01.PE_13, "13");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_14, "14");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_15, "15");
        //            AgregarEntero(Cinta, DatosCinta.SegmentoEmpleo01.PE_16, "16");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo01.PE_17, "17");
        //        }

        //        if (DatosCinta.SegmentoEmpleo02 != null)
        //        {
        //            AgregarEtiqueta(Cinta, DatosCinta.SegmentoEmpleo02.PE_Etiqueta);
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoEmpleo02.PE_PE, string.Empty);
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoEmpleo02.PE_00, "00");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_01, "01");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_02, "02");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_03, "03");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_04, "04");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoEmpleo02.PE_05, "05");
        //            AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoEmpleo02.PE_06, "06");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_07, "07");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_08, "08");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_09, "09");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_10, "10");
        //            AgregarEntero(Cinta, DatosCinta.SegmentoEmpleo02.PE_11, "11");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_12, "12");
        //            AgregarEntero(Cinta, DatosCinta.SegmentoEmpleo02.PE_13, "13");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_14, "14");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_15, "15");
        //            AgregarEntero(Cinta, DatosCinta.SegmentoEmpleo02.PE_16, "16");
        //            AgregarDato(Cinta, DatosCinta.SegmentoEmpleo02.PE_17, "17");
        //        }

        //        AgregarEtiqueta(Cinta, DatosCinta.SegmentoCuenta.TL_Etiqueta);
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_Etiqueta, string.Empty);
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_01, "01");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_02, "02");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_04, "04");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_05, "05");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_06, "06");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_07, "07");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_08, "08");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_09, "09");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_10, "10");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_11, "11");
        //        AgregarEnteroObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_12, "12");
        //        AgregarEnteroObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_13, "13");
        //        AgregarEnteroObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_14, "14");
        //        AgregarEnteroObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_15, "15");
        //        AgregarEntero(Cinta, DatosCinta.SegmentoCuenta.TL_16, "16");
        //        AgregarEntero(Cinta, DatosCinta.SegmentoCuenta.TL_17, "17");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_20, "20");
        //        AgregarEntero(Cinta, DatosCinta.SegmentoCuenta.TL_21, "21");
        //        AgregarEntero(Cinta, DatosCinta.SegmentoCuenta.TL_22, "22");
        //        AgregarEntero(Cinta, DatosCinta.SegmentoCuenta.TL_23, "23");
        //        AgregarEntero(Cinta, DatosCinta.SegmentoCuenta.TL_24, "24");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_25, "25");
        //        AgregarDatoObligatorio(Cinta, DatosCinta.SegmentoCuenta.TL_26, "26");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_27, "27");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_30, "30");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_31, "31");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_32, "32");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_33, "33");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_34, "34");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_35, "35");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_39, "39");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_40, "40");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_41, "41");
        //        AgregarDato(Cinta, DatosCinta.SegmentoCuenta.TL_99, "99");
        //    }
        //    CalcularTR();


        //    Cinta.Append(SegmentoCierre.TR_Etiqueta);
        //    Espacios = 14 - SegmentoCierre.TR_05.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append("0");
        //    Cinta.Append(SegmentoCierre.TR_05);
        //    Espacios = 14 - SegmentoCierre.TR_19.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append("0");
        //    Cinta.Append(SegmentoCierre.TR_19);
        //    Espacios = 3 - SegmentoCierre.TR_33.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append("0");
        //    Cinta.Append(SegmentoCierre.TR_33);
        //    Espacios = 9 - SegmentoCierre.TR_36.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append("0");
        //    Cinta.Append(SegmentoCierre.TR_36);
        //    Espacios = 9 - SegmentoCierre.TR_45.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append("0");
        //    Cinta.Append(SegmentoCierre.TR_45);
        //    Espacios = 9 - SegmentoCierre.TR_54.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append("0");
        //    Cinta.Append(SegmentoCierre.TR_54);
        //    Espacios = 9 - SegmentoCierre.TR_63.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append("0");
        //    Cinta.Append(SegmentoCierre.TR_63);
        //    Espacios = 6 - SegmentoCierre.TR_72.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append("0");
        //    Cinta.Append(SegmentoCierre.TR_72.Length);
        //    Espacios = 16 - SegmentoCierre.TR_78.Length;
        //    Cinta.Append(SegmentoCierre.TR_78);
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append(" ");
        //    Cinta.Append(SegmentoCierre.TR_94);
        //    Espacios = 160 - SegmentoCierre.TR_94.Length;
        //    for (; Espacios > 0; Espacios--)
        //        Cinta.Append(" ");

        //    int idArchivo =GuardarArchivo(Cinta);
        //    GuardarValorEtiqueta(idArchivo);
        //    return Cinta.ToString();
        //}


        //void AgregarEtiqueta(StringBuilder SB, string Informacion)
        //{
        //    SB.Append(Informacion);
        //}

        //void AgregarDato(StringBuilder SB, string Informacion, string Etiqueta)
        //{
        //    if (!string.IsNullOrEmpty(Informacion))
        //    {
        //        SB.Append(Etiqueta);
        //        SB.Append(Informacion.Length.ToString("00"));
        //        SB.Append(Informacion);
        //    }
        //}

        //void AgregarDatoObligatorio(StringBuilder SB, string Informacion, string Etiqueta)
        //{
        //    SB.Append(Etiqueta);
        //    SB.Append(Informacion.Length.ToString("00"));
        //    SB.Append(Informacion);
        //}


        //void AgregarEnteroObligatorio(StringBuilder SB, int Informacion, string Etiqueta)
        //{
        //    SB.Append(Etiqueta);
        //    SB.Append(Informacion.ToString().Length.ToString("00"));
        //    SB.Append(Informacion.ToString());
        //}

        //void AgregarEntero(StringBuilder SB, int Informacion, string Etiqueta)
        //{
        //    if (Informacion.ToString() != "0")
        //    {
        //        SB.Append(Etiqueta);
        //        SB.Append(Informacion.ToString().Length.ToString("00"));
        //        SB.Append(Informacion.ToString());
        //    }
        //}

        //void AgregarLong(StringBuilder SB, Int64 Informacion, string Etiqueta)
        //{
        //    if (Informacion.ToString() != "0")
        //    {
        //        SB.Append(Etiqueta);
        //        SB.Append(Informacion.ToString().Length.ToString("00"));
        //        SB.Append(Informacion.ToString());
        //    }
        //}

        //void AgregarLongObligatorio(StringBuilder SB, Int64 Informacion, string Etiqueta)
        //{
        //    SB.Append(Etiqueta);
        //    SB.Append(Informacion.ToString().Length.ToString("00"));
        //    SB.Append(Informacion.ToString());
        //}

        //void AgregarEnteroObligatorio(StringBuilder SB, int? Informacion, string Etiqueta)
        //{
        //    SB.Append(Etiqueta);
        //    SB.Append(Informacion.ToString().Length.ToString("00"));
        //    SB.Append(Informacion.ToString());
        //}

        //void AgregarEntero(StringBuilder SB, int? Informacion, string Etiqueta)
        //{

        //    if (Informacion != null && Informacion != 0)
        //    {
        //        SB.Append(Etiqueta);
        //        SB.Append(Informacion.ToString().Length.ToString("00"));
        //        SB.Append(Informacion.ToString());
        //    }
        //}

        //void AgregarEnteroObligatorio(StringBuilder SB, double? Informacion, string Etiqueta)
        //{
        //    SB.Append(Etiqueta);
        //    SB.Append(Informacion.ToString().Length.ToString("00"));
        //    SB.Append(Informacion.ToString());
        //}

        //void AgregarEntero(StringBuilder SB, double? Informacion, string Etiqueta)
        //{
        //    if (Informacion != null && Informacion != 0)
        //    {
        //        SB.Append(Etiqueta);
        //        SB.Append(Informacion.ToString().Length.ToString("00"));
        //        SB.Append(Informacion.ToString());
        //    }
        //}

        //void CalcularTR()
        //{
        //    double? SaldosActuales = 0;
        //    double? SaldosVencidos = 0;
        //    int SegmentosPN = 0;
        //    int SegmentosPA = 0;
        //    int SegmentosPE = 0;
        //    int SegmentosTL = 0;
        //    SegmentoCierre = new PF_TR();
        //    foreach (PF_Cinta DatosCinta in SegmentosCredito)
        //    {
        //        if (DatosCinta.SegmentoNombre != null)
        //            SegmentosPN++;
        //        if (DatosCinta.SegmentoEmpleo01 != null)
        //            SegmentosPE++;
        //        if (DatosCinta.SegmentoEmpleo02 != null)
        //            SegmentosPE++;
        //        if (DatosCinta.SegmentoDireccion01 != null)
        //            SegmentosPA++;
        //        if (DatosCinta.SegmentoDireccion02 != null)
        //            SegmentosPA++;
        //        if (DatosCinta.SegmentoDireccion03 != null)
        //            SegmentosPA++;
        //        if (DatosCinta.SegmentoDireccion04 != null)
        //            SegmentosPA++;
        //        if (DatosCinta.SegmentoCuenta != null)
        //        {
        //            SegmentosTL++;
        //            SaldosActuales += Math.Round(Convert.ToDouble(DatosCinta.SegmentoCuenta.TL_22));
        //            SaldosVencidos += Math.Round(Convert.ToDouble(DatosCinta.SegmentoCuenta.TL_24));
        //        }

        //        SegmentoCierre.TR_05 = SaldosActuales.ToString();
        //        SegmentoCierre.TR_19 = SaldosVencidos.ToString();
        //        SegmentoCierre.TR_33 = "1";
        //        SegmentoCierre.TR_36 = SegmentosPN.ToString();
        //        SegmentoCierre.TR_45 = SegmentosPA.ToString();
        //        SegmentoCierre.TR_54 = SegmentosPE.ToString();
        //        SegmentoCierre.TR_63 = SegmentosTL.ToString();
        //    }
        //}
        //int GuardarArchivo(StringBuilder Archivo)
        //{
        //    ArchivoFisicoPersonasFisicas ArchivoFisico= new ArchivoFisicoPersonasFisicas();
        //    return ArchivoFisico.GuardarArchivo(Archivo, 0, 0, 0);
            
        //}

        //void GuardarValorEtiqueta(int IdArchivo)
        //{
        //    foreach (PF_Cinta DatosIdArchivo in SegmentosCredito)
        //    {
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_PN, 125);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_00, 126);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_01, 127);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_02, 128);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_03, 129);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_04, 130);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_05, 131);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_06, 132);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_07, 133);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_08, 134);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_09.ToString(), 135);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_10, 136);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_11, 137);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_12, 138);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_13, 139);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_14, 140);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_15, 141);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_16, 142);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_17, 143);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_18, 144);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_20, 145);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoNombre.PN_21, 146);

        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_PA, 147);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_00, 148);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_01, 149);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_02, 150);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_03, 151);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_04, 152);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_05, 153);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_06, 154);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_07, 155);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_08, 156);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_09, 157);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_10, 158);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion01.PA_11, 159);
        //        if (DatosIdArchivo.SegmentoDireccion02 != null)
        //        {

        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_PA, 147);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_00, 148);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_01, 149);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_02, 150);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_03, 151);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_04, 152);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_05, 153);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_06, 154);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_07, 155);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_08, 156);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_09, 157);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_10, 158);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion02.PA_11, 159);
        //        }
        //        if (DatosIdArchivo.SegmentoDireccion03 != null)
        //        {

        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_PA, 147);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_00, 148);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_01, 149);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_02, 150);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_03, 151);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_04, 152);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_05, 153);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_06, 154);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_07, 155);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_08, 156);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_09, 157);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_10, 158);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion03.PA_11, 159);
        //        }

        //        if (DatosIdArchivo.SegmentoDireccion04 != null)
        //        {
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_PA, 147);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_00, 148);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_01, 149);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_02, 150);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_03, 151);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_04, 152);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_05, 153);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_06, 154);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_07, 155);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_08, 156);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_09, 157);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_10, 158);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoDireccion04.PA_11, 159);
        //        }

        //        if (DatosIdArchivo.SegmentoEmpleo01 != null)
        //        {
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_PE, 160);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_00, 161);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_01, 162);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_02, 163);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_03, 164);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_04, 165);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_05, 166);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_06, 167);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_07, 168);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_08, 169);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_09, 170);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_10, 171);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_11.ToString(), 172);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_12, 173);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_13.ToString(), 174);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_14, 175);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_15 ,176);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_16.ToString(), 177);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo01.PE_17, 178);
        //        }

        //        if (DatosIdArchivo.SegmentoEmpleo02 != null)
        //        {

        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_PE, 160);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_00, 161);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_01, 162);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_02, 163);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_03, 164);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_04, 165);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_05, 166);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_06, 167);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_07, 168);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_08, 169);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_09, 170);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_10, 171);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_11.ToString(), 172);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_12, 173);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_13.ToString(), 174);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_14, 175);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_15, 176);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_16.ToString(), 177);
        //            GuardarValor(IdArchivo, DatosIdArchivo.SegmentoEmpleo02.PE_17, 178);
        //        }

                
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_01, 179);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_02, 180);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_04, 181);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_05, 182);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_06, 183);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_07, 184);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_08, 185);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_09, 186);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_10, 187);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_11, 188);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_12.ToString(), 189);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_13.ToString(), 190);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_14.ToString(), 191);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_15.ToString(), 192);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_16.ToString(), 193);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_17.ToString(), 194);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_20, 195);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_21.ToString(), 196);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_22.ToString(), 197);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_23.ToString(), 198);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_24.ToString(), 199);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_25, 200);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_26, 201);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_27, 202);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_30, 203);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_31, 204);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_32, 205);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_33, 206);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_34, 207);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_35, 208);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_39, 209);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_40, 210);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_41, 211);
        //        GuardarValor(IdArchivo, DatosIdArchivo.SegmentoCuenta.TL_99, 212);
        //    }
        //}

        //void GuardarValor( int IdArchivo, string Valor,int IdEtiqueta)
        //{
        //    if (!string.IsNullOrEmpty(Valor))
        //    {
        //        PF_Cinta.GuardarDatoEtiqueta(IdEtiqueta, IdArchivo, Valor);
        //    }
        //}

        //void VerificarErroresAdvertencias()
        //{
        //    ValidacionesDataAccess validaciones = new ValidacionesDataAccess(Enums.Persona.Fisica);
        //    Validaciones = validaciones.GetRecords(true);
        //    ValidatorFactory factory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
        //    Validator<PF_PN> v_pf_pn = factory.CreateValidator<PF_PN>();
        //    Validator<PF_PA> v_pf_pa01 = factory.CreateValidator<PF_PA>();
        //    Validator<PF_PA> v_pf_pa02 = factory.CreateValidator<PF_PA>();
        //    Validator<PF_PA> v_pf_pa03 = factory.CreateValidator<PF_PA>();
        //    Validator<PF_PA> v_pf_pa04 = factory.CreateValidator<PF_PA>();
        //    Validator<PF_PE> v_pf_pe01 = factory.CreateValidator<PF_PE>();
        //    Validator<PF_PE> v_pf_pe02 = factory.CreateValidator<PF_PE>();
        //    Validator<PF_TL> v_pf_tl = factory.CreateValidator<PF_TL>();
        //    List<int> indices = new  List<int>();
        //    ErrorWarning error;
        //    for (int i = 0; i < SegmentosCredito.Count;i++ )
        //    {
        //        PF_Cinta DatoArchivo = SegmentosCredito[i];
        //        ValidationResults VrPN;
        //        ValidationResults VrPA01;
        //        ValidationResults VrPA02;
        //        ValidationResults VrPA03;
        //        ValidationResults VrPA04;
        //        ValidationResults VrPE01;
        //        ValidationResults VrPE02;
        //        ValidationResults VrTL;
        //        if (DatoArchivo.SegmentoNombre != null)
        //        {
        //            VrPN = v_pf_pn.Validate(DatoArchivo.SegmentoNombre);
        //        }
        //        else
        //        {
        //            VrPN = new ValidationResults();
        //        }
        //        if (DatoArchivo.SegmentoDireccion01 != null)
        //        {
        //            VrPA01 = v_pf_pa01.Validate(DatoArchivo.SegmentoDireccion01);
        //        }
        //        else
        //        {
        //            VrPA01 = new ValidationResults(); 
        //        }
        //        if (DatoArchivo.SegmentoDireccion02 != null)
        //        {
        //            VrPA02 = v_pf_pa02.Validate(DatoArchivo.SegmentoDireccion02);
        //        }
        //        else
        //        {
        //            VrPA02 = new ValidationResults();
        //        }
        //        if (DatoArchivo.SegmentoDireccion03 != null)
        //        {
        //            VrPA03 = v_pf_pa03.Validate(DatoArchivo.SegmentoDireccion03);
        //        }
        //        else
        //        {
        //            VrPA03 = new ValidationResults();
        //        }
        //        if (DatoArchivo.SegmentoDireccion04 != null)
        //        {
        //            VrPA04 = v_pf_pa04.Validate(DatoArchivo.SegmentoDireccion04);
        //        }
        //        else
        //        {
        //            VrPA04 = new ValidationResults();
        //        }
        //        if (DatoArchivo.SegmentoEmpleo01 != null)
        //        {
        //            VrPE01 = v_pf_pe01.Validate(DatoArchivo.SegmentoEmpleo01);
        //        }
        //        else
        //        {
        //            VrPE01 = new ValidationResults();
                    
        //        }
        //        if (DatoArchivo.SegmentoEmpleo02 != null)
        //        {
        //            VrPE02 = v_pf_pe02.Validate(DatoArchivo.SegmentoEmpleo02);
        //        }
        //        else
        //        {
        //            VrPE02 = new ValidationResults();
        //        }
        //        if (DatoArchivo.SegmentoCuenta != null)
        //        {
        //            VrTL = v_pf_tl.Validate(DatoArchivo.SegmentoCuenta);
        //        }
        //        else
        //        {
        //            VrTL = new ValidationResults();
        //        }
        //        foreach (ValidationResult vr in VrPN)
        //        {
        //            var validacion = (from val in Validaciones where val.Codigo == vr.Message select val).FirstOrDefault();
        //            error = new ErrorWarning(0, validacion.Id, DatoArchivo.SegmentoNombre.PN_05, DatoArchivo.SegmentoCuenta.TL_04, vr.Tag);
        //            ErrorAdvertenciaDataAccess errorGuardar = new ErrorAdvertenciaDataAccess();
        //            errorGuardar.AddErrorAdvertencia(error);
        //        }

        //        foreach (ValidationResult vr in VrPA01)
        //        {
        //            var validacion = (from val in Validaciones where val.Codigo == vr.Message select val).FirstOrDefault();
        //            error = new ErrorWarning(0, validacion.Id, DatoArchivo.SegmentoNombre.PN_05, DatoArchivo.SegmentoCuenta.TL_04, vr.Tag);
        //            ErrorAdvertenciaDataAccess errorGuardar = new ErrorAdvertenciaDataAccess();
        //            errorGuardar.AddErrorAdvertencia(error);
        //        }
        //        foreach (ValidationResult vr in VrPA02)
        //        {
        //            var validacion = (from val in Validaciones where val.Codigo == vr.Message select val).FirstOrDefault();
        //            error = new ErrorWarning(0, validacion.Id, DatoArchivo.SegmentoNombre.PN_05, DatoArchivo.SegmentoCuenta.TL_04, vr.Tag);
        //            ErrorAdvertenciaDataAccess errorGuardar = new ErrorAdvertenciaDataAccess();
        //            errorGuardar.AddErrorAdvertencia(error);
        //        }
        //        foreach (ValidationResult vr in VrPA03)
        //        {
        //            var validacion = (from val in Validaciones where val.Codigo == vr.Message select val).FirstOrDefault();
        //            error = new ErrorWarning(0, validacion.Id, DatoArchivo.SegmentoNombre.PN_05, DatoArchivo.SegmentoCuenta.TL_04, vr.Tag);
        //            ErrorAdvertenciaDataAccess errorGuardar = new ErrorAdvertenciaDataAccess();
        //            errorGuardar.AddErrorAdvertencia(error);
        //        }
        //        foreach (ValidationResult vr in VrPA04)
        //        {
        //            var validacion = (from val in Validaciones where val.Codigo == vr.Message select val).FirstOrDefault();
        //            error = new ErrorWarning(0, validacion.Id, DatoArchivo.SegmentoNombre.PN_05, DatoArchivo.SegmentoCuenta.TL_04, vr.Tag);
        //            ErrorAdvertenciaDataAccess errorGuardar = new ErrorAdvertenciaDataAccess();
        //            errorGuardar.AddErrorAdvertencia(error);
        //        }
        //        foreach (ValidationResult vr in VrPE01)
        //        {
        //            var validacion = (from val in Validaciones where val.Codigo == vr.Message select val).FirstOrDefault();
        //            error = new ErrorWarning(0, validacion.Id, DatoArchivo.SegmentoNombre.PN_05, DatoArchivo.SegmentoCuenta.TL_04, vr.Tag);
        //            ErrorAdvertenciaDataAccess errorGuardar = new ErrorAdvertenciaDataAccess();
        //            errorGuardar.AddErrorAdvertencia(error);
        //        }
        //        foreach (ValidationResult vr in VrPE02)
        //        {
        //            var validacion = (from val in Validaciones where val.Codigo == vr.Message select val).FirstOrDefault();
        //            error = new ErrorWarning(0, validacion.Id, DatoArchivo.SegmentoNombre.PN_05, DatoArchivo.SegmentoCuenta.TL_04, vr.Tag);
        //            ErrorAdvertenciaDataAccess errorGuardar = new ErrorAdvertenciaDataAccess();
        //            errorGuardar.AddErrorAdvertencia(error);
        //        }
        //        foreach (ValidationResult vr in VrTL)
        //        {
        //            var validacion = (from val in Validaciones where val.Codigo == vr.Message select val).FirstOrDefault();
        //            error = new ErrorWarning(0, validacion.Id, DatoArchivo.SegmentoNombre.PN_05, DatoArchivo.SegmentoCuenta.TL_04, vr.Tag);
        //            ErrorAdvertenciaDataAccess errorGuardar = new ErrorAdvertenciaDataAccess();
        //            errorGuardar.AddErrorAdvertencia(error);
        //        }
                
        //    }
        //}


        ////TODO::TENEMOS QUE REFACTORIZAR TODA ESTA CLASE. 
        ////POR EL MOMENTO LOS CAMBIOS PARA GENERAR EL ARCHIVO SE BASARÁN EN LA ESTRUCTURA ACTUAL, PERO AL REFACTORIZAR LA ESTRUCTURA DE PF LOS SIGUIENTES METODOS TENDRAN QUE SER CAMBIADOS

        //public void LoadCintaDesdeArchivo()
        //{
        //    ValoresArchivoDataAccess valData = new ValoresArchivoDataAccess();
        //    List<ValorArchivo> valores = valData.GetValoresArchivo(Enums.Persona.Fisica);
        //    List<ValorArchivo> valoresTemp = new List<ValorArchivo>();

        //    List<Etiqueta> etiquetas = (new EtiquetasDataAccess()).GetRecords(true);

        //    PF_Cinta currentCinta = new PF_Cinta();

        //    int currentSegmentoId = 0;
        //    int countAuxiliar = 0;
        //    Etiqueta etiq = new Etiqueta(0, 0, "", "", Enums.Estado.Activo);

        //    foreach (ValorArchivo val in valores)
        //    {
        //        etiq = Util.GetEtiqueta(etiquetas, val.EtiquetaId);
        //        if (currentSegmentoId != etiq.SegmentoId)
        //        {
        //            if (currentSegmentoId != 0)
        //            {
        //                ConstruyeSegmento(valoresTemp, etiquetas, currentSegmentoId, ref currentCinta, ref countAuxiliar);
        //                valoresTemp.Clear();
        //            }
        //            currentSegmentoId = etiq.SegmentoId;
        //        }
        //        valoresTemp.Add(val);
        //    }
        //    ConstruyeSegmento(valoresTemp, etiquetas, currentSegmentoId, ref currentCinta, ref countAuxiliar);
        //    if (currentCinta != default(PF_Cinta))
        //        SegmentosCredito.Add(currentCinta);
        //}

        //private void ConstruyeSegmento(List<ValorArchivo> valores, List<Etiqueta> etiquetas, int segmentoId, ref PF_Cinta cinta, ref int countAuxiliar)
        //{
        //    Segmento seg = (new SegmentosDataAccess()).GetItemById(segmentoId);
            
        //    if (seg != default(Segmento))
        //    {
        //        switch (seg.Codigo)
        //        {
        //            case "PN":
        //                if (cinta.SegmentoNombre != null)
        //                {
        //                    SegmentosCredito.Add(cinta);
        //                    cinta = new PF_Cinta();
        //                }
        //                cinta.SegmentoNombre = GeneraPN(valores, etiquetas);
        //                countAuxiliar++;
        //                cinta.SegmentoNombre.Auxiliar = countAuxiliar;
        //                break;

        //            case "PA":
        //                if (cinta.SegmentoDireccion01 != null && cinta.SegmentoDireccion02 != null && cinta.SegmentoDireccion03 != null && cinta.SegmentoDireccion04 != null)
        //                {
        //                    SegmentosCredito.Add(cinta);
        //                    cinta = new PF_Cinta();
        //                }
        //                PF_PA pa = GeneraPA(valores, etiquetas);
        //                pa.Auxiliar = countAuxiliar;

        //                if (cinta.SegmentoDireccion01 == null)
        //                    cinta.SegmentoDireccion01 = pa;
        //                else if (cinta.SegmentoDireccion02 == null)
        //                    cinta.SegmentoDireccion02 = pa;
        //                else if (cinta.SegmentoDireccion03 == null)
        //                    cinta.SegmentoDireccion03 = pa;
        //                else if (cinta.SegmentoDireccion04 == null)
        //                    cinta.SegmentoDireccion04 = pa;
                        
        //                break;

        //            case "PE":
        //                if (cinta.SegmentoEmpleo01 != null && cinta.SegmentoEmpleo02 != null)
        //                { 
        //                    SegmentosCredito.Add(cinta);
        //                    cinta = new PF_Cinta();
        //                }
        //                PF_PE pe = GeneraPE(valores, etiquetas);
        //                pe.Auxiliar = countAuxiliar;

        //                if (cinta.SegmentoEmpleo01 == null)
        //                    cinta.SegmentoEmpleo01 = pe;
        //                else if (cinta.SegmentoEmpleo02 == null)
        //                    cinta.SegmentoEmpleo02 = pe;

        //                break;
        //            case "TL":
        //                if (cinta.SegmentoCuenta != null)
        //                {
        //                    SegmentosCredito.Add(cinta);
        //                    cinta = new PF_Cinta();
        //                }
        //                PF_TL tl = GeneraTL(valores, etiquetas);
        //                tl.Auxiliar = countAuxiliar;
        //                cinta.SegmentoCuenta = tl;
        //                break;
        //        }
        //    }
        //}
        //private PF_PN GeneraPN(List<ValorArchivo> valores, List<Etiqueta> etiquetas)
        //{
            
        //    PF_PN pn = new PF_PN();
        //    foreach (ValorArchivo val in valores)
        //    {
        //        Etiqueta etiq = Util.GetEtiqueta(etiquetas, val.EtiquetaId);
        //        switch (etiq.Codigo)
        //        { 
        //            case "PN":
        //                 pn.PN_PN = val.Valor;
        //                 break;
        //            case "00":
        //                pn.PN_00 = val.Valor;
        //                break;
        //            case "01":
        //                pn.PN_01 = val.Valor;
        //                break;
        //            case "02":
        //                pn.PN_02 = val.Valor;
        //                break;
        //            case "03":
        //                pn.PN_03 = val.Valor;
        //                break;
        //            case "04":
        //                pn.PN_04 = val.Valor;
        //                break;
        //            case "05":
        //                pn.PN_05 = val.Valor;
        //                break;
        //            case "06":
        //                pn.PN_06 = val.Valor;
        //                break;
        //            case "07":
        //                pn.PN_07 = val.Valor;
        //                break;
        //            case "08":
        //                pn.PN_08 = val.Valor;
        //                break;
        //            case "09":
        //                pn.PN_09 = Parser.ToNumber(val.Valor);
        //                break;

        //            case "10":
        //                pn.PN_10 = val.Valor;
        //                break;
        //            case "11":
        //                pn.PN_11 = val.Valor;
        //                break;
        //            case "12":
        //                pn.PN_12 = val.Valor;
        //                break;
        //            case "13":
        //                pn.PN_13 = val.Valor;
        //                break;
        //            case "14":
        //                pn.PN_14 = val.Valor;
        //                break;
        //            case "15":
        //                pn.PN_15 = val.Valor;
        //                break;
        //            case "16":
        //                pn.PN_16 = val.Valor;
        //                break;
                        
        //        }
        //    }
        //    return pn;
        //}
        //private PF_PA GeneraPA(List<ValorArchivo> valores, List<Etiqueta> etiquetas)
        //{
        //    PF_PA pa = new PF_PA();

        //    foreach (ValorArchivo val in valores)
        //    {
        //        Etiqueta etiq = Util.GetEtiqueta(etiquetas, val.EtiquetaId);
        //        switch (etiq.Codigo)
        //        {
        //            case "PA":
        //                pa.PA_PA = val.Valor;
        //                break;
        //            case "00":
        //                pa.PA_00 = val.Valor;
        //                break;
        //            case "01":
        //                pa.PA_01 = val.Valor;
        //                break;
        //            case "02":
        //                pa.PA_02 = val.Valor;
        //                break;
        //            case "03":
        //                pa.PA_03 = val.Valor;
        //                break;
        //            case "04":
        //                pa.PA_04 = val.Valor;
        //                break;
        //            case "05":
        //                pa.PA_05 = val.Valor;
        //                break;
        //            case "06":
        //                pa.PA_06 = val.Valor;
        //                break;
        //            case "07":
        //                pa.PA_07 = val.Valor;
        //                break;
        //            case "08":
        //                pa.PA_08 = val.Valor;
        //                break;
        //            case "09":
        //                pa.PA_09 = val.Valor;
        //                break;
        //            case "10":
        //                pa.PA_10 = val.Valor;
        //                break;
        //            case "11":
        //                pa.PA_11 = val.Valor;
        //                break;

        //        }
        //    }
        //    return pa;
        //}
        //private PF_PE GeneraPE(List<ValorArchivo> valores, List<Etiqueta> etiquetas)
        //{
        //    PF_PE pe = new PF_PE();
        //    foreach (ValorArchivo val in valores)
        //    {
        //        Etiqueta etiq = Util.GetEtiqueta(etiquetas, val.EtiquetaId);
        //        switch (etiq.Codigo)
        //        {
        //            case "PE":
        //                pe.PE_PE = val.Valor;
        //                break;
        //            case "00":
        //                pe.PE_00 = val.Valor;
        //                break;
        //            case "01":
        //                pe.PE_01 = val.Valor;
        //                break;
        //            case "02":
        //                pe.PE_02 = val.Valor;
        //                break;
        //            case "03":
        //                pe.PE_03 = val.Valor;
        //                break;
        //            case "04":
        //                pe.PE_04 = val.Valor;
        //                break;
        //            case "05":
        //                pe.PE_05 = val.Valor;
        //                break;
        //            case "06":
        //                pe.PE_06 = val.Valor;
        //                break;
        //            case "07":
        //                pe.PE_07 = val.Valor;
        //                break;
        //            case "08":
        //                pe.PE_08 = val.Valor;
        //                break;
        //            case "09":
        //                pe.PE_09 = val.Valor;
        //                break;
        //            case "10":
        //                pe.PE_10 = val.Valor;
        //                break;
        //            case "11":
        //                pe.PE_11 = Parser.ToNumber(val.Valor);
        //                break;
        //            case "12":
        //                pe.PE_12 = val.Valor;
        //                break;
        //            case "13":
        //                pe.PE_13 = Parser.ToNumber(val.Valor);
        //                break;
        //            case "14":
        //                pe.PE_14 = val.Valor;
        //                break;
        //            case "15":
        //                pe.PE_15 = val.Valor;
        //                break;
        //            case "16":
        //                pe.PE_16 = Parser.ToNumber(val.Valor);
        //                break;
        //            case "17":
        //                pe.PE_17 = val.Valor;
        //                break;
                        
        //        }
        //    }
        //    return pe;
        //}
        //private PF_TL GeneraTL(List<ValorArchivo> valores, List<Etiqueta> etiquetas)
        //{
        //    PF_TL tl = new PF_TL();
        //    foreach (ValorArchivo val in valores)
        //    {
        //        Etiqueta etiq = Util.GetEtiqueta(etiquetas, val.EtiquetaId);
        //        switch (etiq.Codigo)
        //        {
        //            case "04":
        //                tl.TL_04 = val.Valor;
        //                break;
        //            //case "05":
        //            //    tl.TL_05 = val.Valor;
        //            //    break;
        //            case "06":
        //                tl.TL_06 = val.Valor;
        //                break;
        //            case "07":
        //                tl.TL_07 = val.Valor;
        //                break;
        //            //case "08":
        //            //    tl.TL_08 = val.Valor;
        //            //    break;
        //            case "09":
        //                tl.TL_09 = val.Valor;
        //                break;
        //            case "10":
        //                tl.TL_10 = val.Valor;
        //                break;
        //            case "11":
        //                tl.TL_11 = val.Valor;
        //                break;
        //            case "12":
        //                tl.TL_12 = Parser.ToDouble(val.Valor);
        //                break;
        //            case "13":
        //                tl.TL_13 = Parser.ToNumber(val.Valor);
        //                break;
        //            case "14":
        //                tl.TL_14 = Parser.ToNumber(val.Valor);
        //                break;
        //            case "15":
        //                tl.TL_15 = Parser.ToNumber(val.Valor);
        //                break;
        //            case "16":
        //                tl.TL_16 = Parser.ToNumber(val.Valor);
        //                break;
        //            //case "17":
        //            //    tl.TL_17 = val.Valor;
        //            //    break;

        //            case "20":
        //                tl.TL_20 = val.Valor;
        //                break;
        //            case "21":
        //                tl.TL_21 = Parser.ToDouble(val.Valor);
        //                break;
        //            case "22":
        //                tl.TL_22 = Parser.ToDouble(val.Valor);
        //                break;
        //            case "23":
        //                tl.TL_23 = Parser.ToDouble(val.Valor);
        //                break;
        //            case "24":
        //                tl.TL_24 = Parser.ToDouble(val.Valor);
        //                break;
        //            case "25":
        //                tl.TL_25 = val.Valor;
        //                break;
        //            case "26":
        //                tl.TL_26 = val.Valor;
        //                break;
        //            case "27":
        //                tl.TL_27 = val.Valor;
        //                break;

        //            case "30":
        //                tl.TL_30 = val.Valor;
        //                break;
        //            case "31":
        //                tl.TL_31 = val.Valor;
        //                break;
        //            case "32":
        //                tl.TL_32 = val.Valor;
        //                break;
        //            case "33":
        //                tl.TL_33 = val.Valor;
        //                break;
        //            case "34":
        //                tl.TL_34 = val.Valor;
        //                break;
        //            case "35":
        //                tl.TL_35 = val.Valor;
        //                break;
        //            case "36":
        //                tl.TL_39 = val.Valor;
        //                break;

        //            case "40":
        //                tl.TL_40 = val.Valor;
        //                break;
        //            case "41":
        //                tl.TL_41 = val.Valor;
        //                break;
                    
        //        }
        //    }
        //    return tl;
        //}
        
    }

}
