using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Telerik.Web.UI;
using Microsoft.Practices.EnterpriseLibrary.Validation;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Business.Rules;
using Banobras.Credito.SICREB.Business.Rules.PM;
using Banobras.Credito.SICREB.Business.Validator.PM;
using Banobras.Credito.SICREB.Business.Repositorios;


using Banobras.Credito.SICREB.Data.Catalogos;


public partial class PersonasMoralesV2 : System.Web.UI.Page
{
    int idUs;
    private Archivo ultimoArchivo;
    public System.Data.DataTable dtWarnings;
    public System.Data.DataTable dtErrores;

    #region Constantes para proceso

    // Constantes para el segmento HD Version de Base PM 4.0
    const string HD_NOMBRE = "SEGMENTO HD";
    const int HD_LNG = 5;
    const int HD_POS = 1;

    const string HD00_NOMBRE = "CLAVE DEL USUARIO";
    const int HD00_LNG = 4;
    const int HD00_POS = 8;

    const string HD01_NOMBRE = "CLAVE DEL USUARIO ANTERIOR";
    const int HD01_LNG = 4;
    const int HD01_POS = 14;

    const string HD02_NOMBRE = "TIPO DE USUARIO";
    const int HD02_LNG = 3;
    const int HD02_POS = 20;

    const string HD03_NOMBRE = "TIPO DE FORMATO";
    const int HD03_LNG = 1;
    const int HD03_POS = 25;

    const string HD04_NOMBRE = "FECHA DE REPORTE DE INFORMACION";
    const int HD04_LNG = 8;
    const int HD04_POS = 28;

    const string HD05_NOMBRE = "PERIODO";
    const int HD05_LNG = 6;
    const int HD05_POS = 38;

    const string HD06_NOMBRE = "VERSION (4)";
    const int HD06_LNG = 2;
    const int HD06_POS = 46;

    const string HD07_NOMBRE = "NOMBRE DEL OTORGANTE";
    const int HD07_LNG = 75;
    const int HD07_POS = 50;

    const string HD08_NOMBRE = "FILLER HD";
    const int HD08_LNG = 52;
    const int HD08_POS = 127;

    // Constantes para el segmento HD Version de Base PM 4.0
    const string EM_NOMBRE = "SEGMENTO EM";
    const int EM_LNG = 2;
    const int EM_POS = 1;

    const string EM00_NOMBRE = "RFC DEL ACREDITADO";
    const int EM00_LNG = 13;
    const int EM00_POS = 5;

    const string EM01_NOMBRE = "CODIGO DE CIUDADANO (CURP EN MEXICO)";
    const int EM01_LNG = 18;
    const int EM01_POS = 20;

    const string EM02_NOMBRE = "RESERVADO";
    const int EM02_LNG = 10;
    const int EM02_POS = 40;

    const string EM03_NOMBRE = "NOMBRE DE LA COMPAÑIA";
    const int EM03_LNG = 150;
    const int EM03_POS = 52;

    const string EM04_NOMBRE = "PRIMER NOMBRE";
    const int EM04_LNG = 30;
    const int EM04_POS = 204;

    const string EM05_NOMBRE = "SEGUNDO NOMBRE";
    const int EM05_LNG = 30;
    const int EM05_POS = 236;

    const string EM06_NOMBRE = "APELLIDO PATERNO";
    const int EM06_LNG = 25;
    const int EM06_POS = 268;

    const string EM07_NOMBRE = "APELLIDO MATERNO";
    const int EM07_LNG = 25;
    const int EM07_POS = 295;

    const string EM08_NOMBRE = "NACIONALIDAD";
    const int EM08_LNG = 2;
    const int EM08_POS = 322;

    const string EM09_NOMBRE = "CALIFICACION DE CARTERA";
    const int EM09_LNG = 2;
    const int EM09_POS = 326;

    const string EM10_NOMBRE = "ACTIVIDAD ECONOMICA 1 BANXICO / SCIAN";
    const int EM10_LNG = 11;
    const int EM10_POS = 330;

    const string EM11_NOMBRE = "ACTIVIDAD ECONOMICA 2 BANXICO / SCIAN";
    const int EM11_LNG = 11;
    const int EM11_POS = 343;

    const string EM12_NOMBRE = "ACTIVIDAD ECONOMICA 3 BANXICO / SCIAN";
    const int EM12_LNG = 11;
    const int EM12_POS = 356;

    const string EM13_NOMBRE = "PRIMERA LINEA DE DIRECCION";
    const int EM13_LNG = 40;
    const int EM13_POS = 369;

    const string EM14_NOMBRE = "SEGUNDA LINEA DE DIRECCION";
    const int EM14_LNG = 40;
    const int EM14_POS = 411;

    const string EM15_NOMBRE = "COLONIA O POBLACION";
    const int EM15_LNG = 60;
    const int EM15_POS = 453;

    const string EM16_NOMBRE = "DELEGACION O MUNICIPIO";
    const int EM16_LNG = 40;
    const int EM16_POS = 515;

    const string EM17_NOMBRE = "CIUDAD";
    const int EM17_LNG = 40;
    const int EM17_POS = 557;

    const string EM18_NOMBRE = "NOMBRE DEL ESTADO PARA DOMICILIOS EN MEXICO";
    const int EM18_LNG = 4;
    const int EM18_POS = 599;

    const string EM19_NOMBRE = "CODIGO POSTAL";
    const int EM19_LNG = 10;
    const int EM19_POS = 605;

    const string EM20_NOMBRE = "NUMERO DE TELEFONO";
    const int EM20_LNG = 11;
    const int EM20_POS = 617;

    const string EM21_NOMBRE = "EXTENSION TELEFONICA";
    const int EM21_LNG = 8;
    const int EM21_POS = 630;

    const string EM22_NOMBRE = "NUMERO DE FAX";
    const int EM22_LNG = 11;
    const int EM22_POS = 640;

    const string EM23_NOMBRE = "TIPO DE CLIENTE";
    const int EM23_LNG = 1;
    const int EM23_POS = 653;

    const string EM24_NOMBRE = "NOMBRE DEL ESTADO EN EL PAIS EXTRANJERO";
    const int EM24_LNG = 40;
    const int EM24_POS = 656;

    const string EM25_NOMBRE = "PAIS DE ORIGEN DEL DOMICILIO";
    const int EM25_LNG = 2;
    const int EM25_POS = 698;

    const string EM26_NOMBRE = "CLAVE DE CONSOLIDACION";
    const int EM26_LNG = 8;
    const int EM26_POS = 702;

    const string EM27_NOMBRE = "FILLER EM";
    const int EM27_LNG = 72;
    const int EM27_POS = 712;

    // Constantes para el segmento AC Version de Base PM 4.0
    const string AC_NOMBRE = "SEGMENTO AC";
    const int AC_LNG = 2;
    const int AC_POS = 1;

    const string AC00_NOMBRE = "RFC DEL ACCIONISTA";
    const int AC00_LNG = 13;
    const int AC00_POS = 5;

    const string AC01_NOMBRE = "CODIGO DEL CIUDADANO (CURP EN MEXICO) DEL ACCIONISTA";
    const int AC01_LNG = 18;
    const int AC01_POS = 20;

    const string AC02_NOMBRE = "CAMPO RESERVADO";
    const int AC02_LNG = 10;
    const int AC02_POS = 40;

    const string AC03_NOMBRE = "NOMBRE DE LA COMPAÑIA ACCIONISTA";
    const int AC03_LNG = 150;
    const int AC03_POS = 52;

    const string AC04_NOMBRE = "PRIMER NOMBRE DEL ACCIONISTA";
    const int AC04_LNG = 30;
    const int AC04_POS = 204;

    const string AC05_NOMBRE = "SEGUNDO NOMBRE DEL ACCIONISTA";
    const int AC05_LNG = 30;
    const int AC05_POS = 236;

    const string AC06_NOMBRE = "APELLIDO PATERNO DEL ACCIONISTA";
    const int AC06_LNG = 25;
    const int AC06_POS = 268;

    const string AC07_NOMBRE = "APELLIDO MATERNO DEL ACCIONISTA";
    const int AC07_LNG = 25;
    const int AC07_POS = 295;

    const string AC08_NOMBRE = "PORCENTAJE DEL ACCIONISTA";
    const int AC08_LNG = 2;
    const int AC08_POS = 322;

    const string AC09_NOMBRE = "PRIMERA LINEA DE DIRECCION DEL ACCIONISTA";
    const int AC09_LNG = 40;
    const int AC09_POS = 326;

    const string AC10_NOMBRE = "SEGUNDA LINEA DE DIRECCION DEL ACCIONISTA";
    const int AC10_LNG = 40;
    const int AC10_POS = 368;

    const string AC11_NOMBRE = "COLONIA O POBLACION";
    const int AC11_LNG = 60;
    const int AC11_POS = 410;

    const string AC12_NOMBRE = "DELEGACION O MUNICIPIO";
    const int AC12_LNG = 40;
    const int AC12_POS = 472;

    const string AC13_NOMBRE = "CUIDAD";
    const int AC13_LNG = 40;
    const int AC13_POS = 514;

    const string AC14_NOMBRE = "NOMBRE DEL ESTADO PARA DOMICILIOS EN MEXICO";
    const int AC14_LNG = 4;
    const int AC14_POS = 556;

    const string AC15_NOMBRE = "CODIGO POSTAL";
    const int AC15_LNG = 10;
    const int AC15_POS = 562;

    const string AC16_NOMBRE = "NUMERO DE TELEFONO";
    const int AC16_LNG = 11;
    const int AC16_POS = 574;

    const string AC17_NOMBRE = "EXTENSION TELEFONICA";
    const int AC17_LNG = 8;
    const int AC17_POS = 587;

    const string AC18_NOMBRE = "NUMERO DE FAX";
    const int AC18_LNG = 11;
    const int AC18_POS = 597;

    const string AC19_NOMBRE = "TIPO DE ACCIONISTA";
    const int AC19_LNG = 1;
    const int AC19_POS = 610;

    const string AC20_NOMBRE = "NOMBRE DEL ESTADO EN EL PAIS EXTRANJERO";
    const int AC20_LNG = 40;
    const int AC20_POS = 613;

    const string AC21_NOMBRE = "PAIS DE ORIGEN DEL DOMICILIO";
    const int AC21_LNG = 2;
    const int AC21_POS = 655;

    const string AC22_NOMBRE = "FILLER AC";
    const int AC22_LNG = 40;
    const int AC22_POS = 659;

    // Constantes para el segmento CR Version de Base PM 4.0
    const string CR_NOMBRE = "SEGMENTO CR";
    const int CR_LNG = 2;
    const int CR_POS = 1;

    const string CR00_NOMBRE = "RFC DEL ACREDITADO";
    const int CR00_LNG = 13;
    const int CR00_POS = 5;

    const string CR01_NOMBRE = "NUMERO DE EXPERIENCIAS CREDITICIAS";
    const int CR01_LNG = 6;
    const int CR01_POS = 20;

    const string CR02_NOMBRE = "NUMERO DE CUENTA, CREDITO O CONTRATO";
    const int CR02_LNG = 25;
    const int CR02_POS = 28;

    const string CR03_NOMBRE = "NUMERO DE CUENTA, CREDITO O CONTRATO ANTERIOR";
    const int CR03_LNG = 25;
    const int CR03_POS = 55;

    const string CR04_NOMBRE = "FECHA DE APERTURA DE CUENTA O CREDITO";
    const int CR04_LNG = 8;
    const int CR04_POS = 82;

    const string CR05_NOMBRE = "PLAZO";
    const int CR05_LNG = 6;
    const int CR05_POS = 92;

    const string CR06_NOMBRE = "TIPO DE CREDITO";
    const int CR06_LNG = 4;
    const int CR06_POS = 100;

    const string CR07_NOMBRE = "MONTO AUTORIZADO DEL CREDITO (SALDO INICIAL)";
    const int CR07_LNG = 20;
    const int CR07_POS = 106;

    const string CR08_NOMBRE = "MONEDA";
    const int CR08_LNG = 3;
    const int CR08_POS = 128;

    const string CR09_NOMBRE = "NUMERO DE PAGOS";
    const int CR09_LNG = 4;
    const int CR09_POS = 133;

    const string CR10_NOMBRE = "FRECUENCIA DE PAGOS";
    const int CR10_LNG = 5;
    const int CR10_POS = 139;

    const string CR11_NOMBRE = "IMPORTE DE PAGO";
    const int CR11_LNG = 20;
    const int CR11_POS = 146;

    const string CR12_NOMBRE = "FECHA DE ULTIMO PAGO";
    const int CR12_LNG = 8;
    const int CR12_POS = 168;

    const string CR13_NOMBRE = "FECHA DE RESTRUCTURA";
    const int CR13_LNG = 8;
    const int CR13_POS = 178;

    const string CR14_NOMBRE = "PAGO FINAL PARA CIERRE DE CUENTA MOROSA (PAGO EN EFECTIVO)";
    const int CR14_LNG = 20;
    const int CR14_POS = 188;

    const string CR15_NOMBRE = "FECHA DE LIQUIDACION";
    const int CR15_LNG = 8;
    const int CR15_POS = 210;

    const string CR16_NOMBRE = "QUITA";
    const int CR16_LNG = 20;
    const int CR16_POS = 220;

    const string CR17_NOMBRE = "DACION EN PAGO";
    const int CR17_LNG = 20;
    const int CR17_POS = 242;

    const string CR18_NOMBRE = "QUEBRANTO O CASTIGO";
    const int CR18_LNG = 20;
    const int CR18_POS = 264;

    const string CR19_NOMBRE = "CLAVE OBSERVACION";
    const int CR19_LNG = 4;
    const int CR19_POS = 286;

    const string CR20_NOMBRE = "MARCA PARA CREDITO ESPECIAL";
    const int CR20_LNG = 1;
    const int CR20_POS = 292;

    const string CR21_NOMBRE = "FECHA DE PRIMER INCUMPLIMIENTO";
    const int CR21_LNG = 8;
    const int CR21_POS = 295;

    const string CR22_NOMBRE = "SALDO INSOLUTO DEL PRINCIPAL";
    const int CR22_LNG = 20;
    const int CR22_POS = 305;

    const string CR23_NOMBRE = "CREDITO MAXIMO UTILIZADO";
    const int CR23_LNG = 20;
    const int CR23_POS = 327;
    // *JGSP1 Se modifica Constante por FECHA DE INGRESO A CARTERA VENCIDA (SOFTTEK)
    const string CR24_NOMBRE = "FECHA DE INGRESO A CARTERA VENCIDA";
    const int CR24_LNG = 8;
    const int CR24_POS = 349;
    // *JGSP1 Se agrega Constante para FILLER CR porque se agrega campo de Fecha de ingreso de Cartera vencida (SOFTTEK)
    const string CR25_NOMBRE = "FILLER CR";
    const int CR25_LNG = 40; 
    const int CR25_POS = 359;

    // Constantes para el segmento DE Version de Base PM 4.0
    const string DE_NOMBRE = "SEGMENTO DE";
    const int DE_LNG = 2;
    const int DE_POS = 1;

    const string DE00_NOMBRE = "RFC DEL ACREDITADO";
    const int DE00_LNG = 13;
    const int DE00_POS = 5;

    const string DE01_NOMBRE = "NUMERO DE CUENTA, CREDITO O CONTRATO";
    const int DE01_LNG = 25;
    const int DE01_POS = 20;

    const string DE02_NOMBRE = "NUMERO DE DIAS VENCIDO";
    const int DE02_LNG = 3;
    const int DE02_POS = 47;

    const string DE03_NOMBRE = "CANTIDAD (SALDO)";
    const int DE03_LNG = 20;
    const int DE03_POS = 52;
    //JGSP1 Se agrega posicion de INTERESES (SOFTTEK)
    const string DE04_NOMBRE = "INTERES";
    const int DE04_LNG = 20;
    const int DE04_POS = 74;

    const string DE05_NOMBRE = "FILLER DE";
    const int DE05_LNG = 53;
    const int DE05_POS = 96;

    // Constantes para el segmento AV Version de Base PM 4.0
    const string AV_NOMBRE = "SEGMENTO AV";
    const int AV_LNG = 2;
    const int AV_POS = 1;

    const string AV00_NOMBRE = "RFC DEL AVAL";
    const int AV00_LNG = 13;
    const int AV00_POS = 5;

    const string AV01_NOMBRE = "CODIGO DE CIUDADANO (CURP EN MEXICO) DEL AVAL";
    const int AV01_LNG = 18;
    const int AV01_POS = 20;

    const string AV02_NOMBRE = "CAMPO RESERVADO";
    const int AV02_LNG = 10;
    const int AV02_POS = 40;

    const string AV03_NOMBRE = "NOMBRE DE COMPAÑIA AVAL";
    const int AV03_LNG = 150;
    const int AV03_POS = 52;

    const string AV04_NOMBRE = "PRIMER NOMBRE DEL AVAL";
    const int AV04_LNG = 30;
    const int AV04_POS = 204;

    const string AV05_NOMBRE = "SEGUNDO NOMBRE DEL AVAL";
    const int AV05_LNG = 30;
    const int AV05_POS = 236;

    const string AV06_NOMBRE = "APELLIDO PATERNO DEL AVAL";
    const int AV06_LNG = 25;
    const int AV06_POS = 268;

    const string AV07_NOMBRE = "APELLIDO MATERNO DEL AVAL";
    const int AV07_LNG = 25;
    const int AV07_POS = 295;

    const string AV08_NOMBRE = "PRIMERA LINEA DE DIRECCION DEL AVAL";
    const int AV08_LNG = 40;
    const int AV08_POS = 322;

    const string AV09_NOMBRE = "SEGUNDA LINEA DE DIRECCION DEL AVAL";
    const int AV09_LNG = 40;
    const int AV09_POS = 364;

    const string AV10_NOMBRE = "COLONIA O POBLACION";
    const int AV10_LNG = 60;
    const int AV10_POS = 406;

    const string AV11_NOMBRE = "DELEGACION O MUNICIPIO";
    const int AV11_LNG = 40;
    const int AV11_POS = 468;

    const string AV12_NOMBRE = "CIUDAD";
    const int AV12_LNG = 40;
    const int AV12_POS = 510;

    const string AV13_NOMBRE = "NOMBRE DE ESTADO PARA DIRECCIONES EN MEXICO";
    const int AV13_LNG = 4;
    const int AV13_POS = 552;

    const string AV14_NOMBRE = "CODIGO POSTAL";
    const int AV14_LNG = 10;
    const int AV14_POS = 558;

    const string AV15_NOMBRE = "NUMERO DE TELEFONO";
    const int AV15_LNG = 11;
    const int AV15_POS = 570;

    const string AV16_NOMBRE = "EXTENSION TELEFONICA";
    const int AV16_LNG = 8;
    const int AV16_POS = 583;

    const string AV17_NOMBRE = "NUMERO DE FAX";
    const int AV17_LNG = 11;
    const int AV17_POS = 593;

    const string AV18_NOMBRE = "TIPO DE AVAL";
    const int AV18_LNG = 1;
    const int AV18_POS = 606;

    const string AV19_NOMBRE = "NOMBRE DE ESTADO EN EL PAIS EXTRANJERO";
    const int AV19_LNG = 40;
    const int AV19_POS = 609;

    const string AV20_NOMBRE = "PAIS DE ORIGEN DEL DOMICILIO";
    const int AV20_LNG = 2;
    const int AV20_POS = 651;

    const string AV21_NOMBRE = "FILLER AV";
    const int AV21_LNG = 94;
    const int AV21_POS = 655;

    // Constantes para el segmento TS Version de Base PM 3.0
    const string TS_NOMBRE = "SEGMENTO TS";
    const int TS_LNG = 2;
    const int TS_POS = 1;

    const string TS00_NOMBRE = "";
    const int TS00_LNG = 7;
    const int TS00_POS = 5;

    const string TS01_NOMBRE = "";
    const int TS01_LNG = 30;
    const int TS01_POS = 14;

    const string TS02_NOMBRE = "";
    const int TS02_LNG = 53;
    const int TS02_POS = 46;

    // Longitudes de segmentos

    const int LNG_SGT_HD = 180;
    const int LNG_SGT_EM = 800;
    const int LNG_SGT_AC = 700;
    const int LNG_SGT_CR = 400;
    const int LNG_SGT_DE = 150;
    const int LNG_SGT_AV = 750;
    const int LNG_SGT_TS = 100;

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {

        // Revisamos si el usuario esta logueado en la aplicacion si tiene privilegios para consultar el menu actual.
        if (Session["Facultades"] != null)
        {
            getFacultades();
            ActividadRules.GuardarActividad(5, this.idUs, "Acceso al Menú detalle de Personas Morales");
        }
        else
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }


        if (!IsPostBack)
        {
            //// Revisamos si el usuario esta logueado en la aplicacion si tiene privilegios para consultar el menu actual.
            //if (Session["Facultades"] != null)
            //{
            //    getFacultades();
            //    ActividadRules.GuardarActividad(5, this.idUs, "Acceso al Menú detalle de Personas Morales");
            //}
            //else
            //{
            //    Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
            //}

            // Si el usuario es valido registramos su acceso
            idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(5, idUs, "Acceso al Menú detalle de Personas Morales");

            // Obtenemos el ultimo archivo PM generado
            Archivos_Rules archRules = new Archivos_Rules(Enums.Persona.Moral);
            ultimoArchivo = archRules.GetUltimoArchivo();

            // Si el ultimo proceso se ejecut bien y recuperamos el archivo, mostramos la informacion.
            if (ultimoArchivo != null)
            {
                lblErrores.Text = ultimoArchivo.Reg_Errores.ToString();
                lblWarnings.Text = ultimoArchivo.Reg_Advertencias.ToString();
                lblCorrectos.Text = ultimoArchivo.Reg_Correctos.ToString();
                lblProcesados.Text = (ultimoArchivo.Reg_Correctos + ultimoArchivo.Reg_Errores + ultimoArchivo.Reg_Advertencias).ToString();
                lblFechaArchivo.Text = ultimoArchivo.Fecha.ToShortDateString();

                // Guardamos el archivo en session para utilizrlo en el RADGrid
                Session.Add("UltimoArchivoPM", ultimoArchivo);

                // Consultamos loe Warnings y Errores generardos durante la construccion del archivo PM
                string strGrupos = "13,6378";
                ErroresWarnings_Rules errores = new ErroresWarnings_Rules();
                Session.Add("erroresInfoPM", errores.GetErroresPresentacion(Enums.Persona.Moral, 1, strGrupos));
                Session.Add("warningsInfoPM", errores.GetErroresPresentacion(Enums.Persona.Moral, 0, strGrupos));
            }

            CambiaAtributosRGR(rgArchivoMorales.FilterMenu);
            CambiaAtributosRGR(rgErrores.FilterMenu);
            CambiaAtributosRGR(rgWarnings.FilterMenu);
        }
    }

    protected void rgArchivoMorales_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        if (Session["UltimoArchivoPM"] != null)
        {



            List<PM_EM> ems = new List<PM_EM>();
            List<PM_AC> acs = new List<PM_AC>();
            List<PM_CR> crs = new List<PM_CR>();
            List<PM_AV> avs = new List<PM_AV>();

            //Archivo ultimoArchivo = new Archivo();
            //ultimoArchivo = Session["UltimoArchivoPM"] as Archivo;

            Archivo ultimoArchivo = Session["UltimoArchivoPM"] as Archivo;
            ASCIIEncoding encoding = new ASCIIEncoding();
            string ArchivoPM = encoding.GetString(ultimoArchivo.BytesArchivo);



            PM_Cinta cinta = LoadPMCinta();
            cinta.ArchivoId = ultimoArchivo.Id;

            //PM_EM_Rules emRules = new PM_EM_Rules();
            //emRules.LoadEMs(cinta, Enums.Reporte.Mensual, false, "");

            

            int TotalSegmentos = 0;
            string[] Segmentos;

            ArchivoPM = ArchivoPM.Replace("HDBNCPM", ">>>>>>HDBNCPM");
            ArchivoPM = ArchivoPM.Replace("EMEM00", ">>>>>>EMEM00");
            ArchivoPM = ArchivoPM.Replace("ACAC00", ">>>>>>ACAC00");
            ArchivoPM = ArchivoPM.Replace("CRCR00", ">>>>>>CRCR00");
            ArchivoPM = ArchivoPM.Replace("DEDE00", ">>>>>>DEDE00");
            ArchivoPM = ArchivoPM.Replace("AVAV00", ">>>>>>AVAV00");
            ArchivoPM = ArchivoPM.Replace("TSTS00", ">>>>>>TSTS00");

            string[] stringSeparadores = new string[] { ">>>>>>" };
            Segmentos = ArchivoPM.Split(stringSeparadores, StringSplitOptions.RemoveEmptyEntries);
            TotalSegmentos = Segmentos.Count();

            int ContadorEmpresas = 0;
            //PM_Cinta CintaPM = new PM_Cinta();
            PM_EM em = new PM_EM();
            PM_CR cr = new PM_CR();

            for (int i = 0; i < Segmentos.Count(); i++)
            {
                if (Segmentos[i].Contains("EMEM00"))
                {
                    ContadorEmpresas++;
                    em = ObtenerAcreditado(Segmentos[i], cinta, ContadorEmpresas);
                    cinta.EMs.Add(em);
                }

                if (Segmentos[i].Contains("ACAC00"))
                {
                    PM_AC ac = new PM_AC();
                    ac = ObtenerAccionista(Segmentos[i], em, ContadorEmpresas);
                    ac.Parent = em;
                    acs.Add(ac);
                }

                if (Segmentos[i].Contains("CRCR00"))
                {
                    cr = ObtenerCredito(Segmentos[i], em, ContadorEmpresas, Segmentos, i);
                    crs.Add(cr);
                }

                if (Segmentos[i].Contains("AVAV00"))
                {
                    PM_AV av = new PM_AV();
                    av = ObtenerAval(Segmentos[i], cr, ContadorEmpresas);                    
                    //av.AuxId = em.Id;
                    //av.AV_21 = cr.CR_02;  //Asignamos temporalmente el numero de Credito al Filler
                    //av.Parent = em;
                    av.AV_21 = cr.CR_02;
                    avs.Add(av);
                }
            }

            this.rgArchivoMorales.MasterTableView.DataSource = cinta.EMs;
            this.rgArchivoMorales.MasterTableView.DetailTables[0].DataSource = crs;
            this.rgArchivoMorales.MasterTableView.DetailTables[1].DataSource = acs;
            this.rgArchivoMorales.MasterTableView.DetailTables[2].DataSource = avs;
        }
    }

    // Temp 1
    private PM_Cinta LoadPMCinta()
    {

        SegmentosDataAccess segs = new SegmentosDataAccess();
        List<Segmento> segmentos = segs.GetRecords(true);

        EtiquetasDataAccess etis = new EtiquetasDataAccess();
        List<Etiqueta> etiquetas = etis.GetRecords(true);

        ValidacionesDataAccess vals = new ValidacionesDataAccess(Enums.Persona.Moral);
        List<Validacion> validaciones = vals.GetRecords(true);

        return new PM_Cinta(segmentos, etiquetas, validaciones);
    }

         
    
    private PM_EM ObtenerAcreditado(string CadenaAcreditado, PM_Cinta CintaPM, int idContador) 
    {
        PM_EM em = new PM_EM(CintaPM);

        if (CadenaAcreditado.Length == LNG_SGT_EM)
        {
            em.Id = Parser.ToNumber(idContador);
            em.EM_00 = CadenaAcreditado.Substring(EM00_POS + 1, EM00_LNG);
            em.EM_01 = CadenaAcreditado.Substring(EM01_POS + 1, EM01_LNG);
            em.EM_02 = CadenaAcreditado.Substring(EM02_POS + 1, EM02_LNG);
            em.EM_03 = CadenaAcreditado.Substring(EM03_POS + 1, EM03_LNG);
            em.EM_04 = CadenaAcreditado.Substring(EM04_POS + 1, EM04_LNG);
            em.EM_05 = CadenaAcreditado.Substring(EM05_POS + 1, EM05_LNG);
            em.EM_06 = CadenaAcreditado.Substring(EM06_POS + 1, EM06_LNG);
            em.EM_07 = CadenaAcreditado.Substring(EM07_POS + 1, EM07_LNG);
            em.EM_08 = CadenaAcreditado.Substring(EM08_POS + 1, EM08_LNG);
            em.EM_09 = CadenaAcreditado.Substring(EM09_POS + 1, EM09_LNG);
            em.EM_10 = CadenaAcreditado.Substring(EM10_POS + 1, EM10_LNG);
            em.EM_11 = CadenaAcreditado.Substring(EM11_POS + 1, EM11_LNG);
            em.EM_12 = CadenaAcreditado.Substring(EM12_POS + 1, EM12_LNG);
            em.EM_13 = CadenaAcreditado.Substring(EM13_POS + 1, EM13_LNG);
            em.EM_14 = CadenaAcreditado.Substring(EM14_POS + 1, EM14_LNG);
            em.EM_15 = CadenaAcreditado.Substring(EM15_POS + 1, EM15_LNG);
            em.EM_16 = CadenaAcreditado.Substring(EM16_POS + 1, EM16_LNG);
            em.EM_17 = CadenaAcreditado.Substring(EM17_POS + 1, EM17_LNG);
            em.EM_18 = CadenaAcreditado.Substring(EM18_POS + 1, EM18_LNG);
            em.EM_19 = CadenaAcreditado.Substring(EM19_POS + 1, EM19_LNG);
            em.EM_20 = CadenaAcreditado.Substring(EM20_POS + 1, EM20_LNG);
            em.EM_21 = CadenaAcreditado.Substring(EM21_POS + 1, EM21_LNG);
            em.EM_22 = CadenaAcreditado.Substring(EM22_POS + 1, EM22_LNG);
            em.EM_23 = CadenaAcreditado.Substring(EM23_POS + 1, EM23_LNG);
            em.EM_24 = CadenaAcreditado.Substring(EM24_POS + 1, EM24_LNG);
            em.EM_25 = CadenaAcreditado.Substring(EM25_POS + 1, EM25_LNG);
            em.EM_26 = "'" + CadenaAcreditado.Substring(EM26_POS + 1, EM26_LNG);
            em.EM_27 = CadenaAcreditado.Substring(EM27_POS + 1, EM27_LNG);

            // Utilizamos temporalmente el campo 27 Filler para almacenar el dato que esta guardado en el archivo PM
            // No lo podemos guardar en el campo 09 que es el que le corresponde debido a que esta propiedad cada vez que se
            // asigna ejecuta un metodo para tratar de calcular ese valor con los creditos relacionados a la empresa pero al
            // no existir esa informacion en este caso siempre asigna por default NC.
            em.EM_27 = CadenaAcreditado.Substring(EM09_POS + 1, EM09_LNG);
        }

        return em;
    }

    private PM_AC ObtenerAccionista(string cadenaAccionista, PM_EM emItem, int IdContador)
    {
        PM_AC ac = new PM_AC(emItem);

        if (cadenaAccionista.Length == LNG_SGT_AC)
        {
            ac.AuxId = IdContador;
            ac.AC_00 = cadenaAccionista.Substring(AC00_POS + 1, AC00_LNG);
            ac.AC_01 = cadenaAccionista.Substring(AC01_POS + 1, AC01_LNG);
            ac.AC_02 = cadenaAccionista.Substring(AC02_POS + 1, AC02_LNG);
            ac.AC_03 = cadenaAccionista.Substring(AC03_POS + 1, AC03_LNG);
            ac.AC_04 = cadenaAccionista.Substring(AC04_POS + 1, AC04_LNG);
            ac.AC_05 = cadenaAccionista.Substring(AC05_POS + 1, AC05_LNG);
            ac.AC_06 = cadenaAccionista.Substring(AC06_POS + 1, AC06_LNG);
            ac.AC_07 = cadenaAccionista.Substring(AC07_POS + 1, AC07_LNG);
            ac.AC_08 = cadenaAccionista.Substring(AC08_POS + 1, AC08_LNG);
            ac.AC_09 = cadenaAccionista.Substring(AC09_POS + 1, AC09_LNG);
            ac.AC_10 = cadenaAccionista.Substring(AC10_POS + 1, AC10_LNG);
            ac.AC_11 = cadenaAccionista.Substring(AC11_POS + 1, AC11_LNG);
            ac.AC_12 = cadenaAccionista.Substring(AC12_POS + 1, AC12_LNG);
            ac.AC_13 = cadenaAccionista.Substring(AC13_POS + 1, AC13_LNG);
            ac.AC_14 = cadenaAccionista.Substring(AC14_POS + 1, AC14_LNG);
            ac.AC_15 = cadenaAccionista.Substring(AC15_POS + 1, AC15_LNG);
            ac.AC_16 = cadenaAccionista.Substring(AC16_POS + 1, AC16_LNG);
            ac.AC_17 = cadenaAccionista.Substring(AC17_POS + 1, AC17_LNG);
            ac.AC_18 = cadenaAccionista.Substring(AC18_POS + 1, AC18_LNG);
            ac.AC_19 = cadenaAccionista.Substring(AC19_POS + 1, AC19_LNG);
            ac.AC_20 = cadenaAccionista.Substring(AC20_POS + 1, AC20_LNG);
            ac.AC_21 = cadenaAccionista.Substring(AC21_POS + 1, AC21_LNG);
            ac.AC_22 = cadenaAccionista.Substring(AC22_POS + 1, AC22_LNG);
            emItem.ACs.Add(ac);
        }

        return ac;
    }

    private PM_CR ObtenerCredito(string cadenaCredito, PM_EM emItem, int IdContador, string[] Segmentos, int Contador)
    {
        PM_CR cr = new PM_CR(emItem);

        if (cadenaCredito.Length == LNG_SGT_CR)
        {
            cr.EM_ID = IdContador;
            cr.CR_00 = cadenaCredito.Substring(CR00_POS + 1, CR00_LNG);
            cr.CR_01 = cadenaCredito.Substring(CR01_POS + 1, CR01_LNG);
            cr.CR_02 = cadenaCredito.Substring(CR02_POS + 1, CR02_LNG);
            cr.CR_03 = cadenaCredito.Substring(CR03_POS + 1, CR03_LNG);
            cr.CR_04 = TransformarFecha(cadenaCredito.Substring(CR04_POS + 1, CR04_LNG));
            cr.CR_05 = cadenaCredito.Substring(CR05_POS + 1, CR05_LNG);
            cr.CR_06 = cadenaCredito.Substring(CR06_POS + 1, CR06_LNG);
            cr.CR_07 = String.Format("{0:$###,###,##0.00}", Convert.ToDouble(cadenaCredito.Substring(CR07_POS + 1, CR07_LNG)));  
            cr.CR_08 = cadenaCredito.Substring(CR08_POS + 1, CR08_LNG);
            cr.CR_09 = cadenaCredito.Substring(CR09_POS + 1, CR09_LNG);
            cr.CR_10 = cadenaCredito.Substring(CR10_POS + 1, CR10_LNG);
            cr.CR_11 = String.Format("{0:$###,###,##0.00}", Convert.ToDouble(cadenaCredito.Substring(CR11_POS + 1, CR11_LNG)));  
            cr.CR_12 = TransformarFecha(cadenaCredito.Substring(CR12_POS + 1, CR12_LNG));
            cr.CR_13 = TransformarFecha(cadenaCredito.Substring(CR13_POS + 1, CR13_LNG));
            cr.CR_14 = String.Format("{0:$###,###,##0.00}", Convert.ToDouble(cadenaCredito.Substring(CR14_POS + 1, CR14_LNG)));
            cr.CR_15 = TransformarFecha(cadenaCredito.Substring(CR15_POS + 1, CR15_LNG));
            cr.CR_16 = String.Format("{0:$###,###,##0.00}", Convert.ToDouble(cadenaCredito.Substring(CR16_POS + 1, CR16_LNG)));  
            cr.CR_17 = String.Format("{0:$###,###,##0.00}", Convert.ToDouble(cadenaCredito.Substring(CR17_POS + 1, CR17_LNG))); 
            cr.CR_18 = String.Format("{0:$###,###,##0.00}", Convert.ToDouble(cadenaCredito.Substring(CR18_POS + 1, CR18_LNG))); 
            cr.CR_19 = cadenaCredito.Substring(CR19_POS + 1, CR19_LNG);
            cr.CR_20 = cadenaCredito.Substring(CR20_POS + 1, CR20_LNG);
            cr.CR_21 = TransformarFecha(cadenaCredito.Substring(CR21_POS + 1, CR21_LNG));
            cr.CR_22 = String.Format("{0:$###,###,##0.00}", Convert.ToDouble(cadenaCredito.Substring(CR22_POS + 1, CR22_LNG)));
            // *JGSP1 Se Agrega Columna de Fecha de Ingreso a Cartera Vencida (SOFTTEK)
            cr.CR_24 = TransformarFecha(cadenaCredito.Substring(CR24_POS + 1, CR24_LNG));

            int DiasVencidos = 0;
            Double MontoPagar = 0;
            Double Intereses = 0;
            Double MontoVencido = 0;
            Boolean ContinuarRecorrido = true;

            Contador++;

            while (Contador < Segmentos.Count() && ContinuarRecorrido)
            {
                int TempDias = 0;
                string NoCredito = "";
                string CadenaDetalle = Segmentos[Contador];
                
                if (CadenaDetalle.Length == LNG_SGT_DE)
                {
                    NoCredito = CadenaDetalle.Substring(DE01_POS + 1, DE01_LNG);
                    TempDias = Convert.ToInt32(CadenaDetalle.Substring(DE02_POS + 1, DE02_LNG));
                    // JGSP1 Se Agrega Linea de Intereses (SOFTTEK)
                    Intereses = Intereses + Convert.ToDouble(CadenaDetalle.Substring(DE04_POS + 1, DE04_LNG));
                    if (TempDias == 0 && cr.CR_02.Trim() == NoCredito.Trim())
                    {
                        MontoPagar = MontoPagar + Convert.ToDouble(CadenaDetalle.Substring(DE03_POS + 1, DE03_LNG));
                    }
                    if (TempDias != 0 && cr.CR_02.Trim() == NoCredito.Trim())
                    {
                        // Verificar si son validos los numero negativos
                        // Se agrego replace(-) para evitar error de numeros negativos con ceros al inicio ejemplo 000-12345
                        if (CadenaDetalle.Substring(DE03_POS + 1, DE03_LNG).Contains("-"))
                            { MontoVencido = MontoVencido - Convert.ToDouble(CadenaDetalle.Substring(DE03_POS + 1, DE03_LNG).Replace("-", "")); }
                        else
                            { MontoVencido = MontoVencido + Convert.ToDouble(CadenaDetalle.Substring(DE03_POS + 1, DE03_LNG)); }

                        if (TempDias > DiasVencidos) { DiasVencidos = TempDias; }
                    }
                }

                // Validamos si ahi mas detalles de este credito
                if ( (Contador + 1) < Segmentos.Count() )
                {
                    Contador++;
                    CadenaDetalle = Segmentos[Contador];
                    if (CadenaDetalle.Length == LNG_SGT_DE)
                        { ContinuarRecorrido = true; }
                    else
                        { ContinuarRecorrido = false; }
                }

            }

            cr.MontoPagar = MontoPagar;
            cr.MontoPagarVencido = MontoVencido;
            // JGSP1 Se Agrega intereses (SOFTTEK)
            cr.Intereses = Intereses;
            cr.DiasVencido = DiasVencidos;

            emItem.CRs.Add(cr);
        }

        return cr;
    }

    private PM_AV ObtenerAval(string cadenaAval, PM_CR crItem, int IdContador)
    {
        PM_AV av = new PM_AV(crItem);

        if (cadenaAval.Length == LNG_SGT_AV)
        {
            av.AuxId = IdContador;
            av.AV_00 = cadenaAval.Substring(AV00_POS + 1, AV00_LNG);
            av.AV_01 = cadenaAval.Substring(AV01_POS + 1, AV01_LNG);
            av.AV_02 = cadenaAval.Substring(AV02_POS + 1, AV02_LNG);
            av.AV_03 = cadenaAval.Substring(AV03_POS + 1, AV03_LNG);
            av.AV_04 = cadenaAval.Substring(AV04_POS + 1, AV04_LNG);
            av.AV_05 = cadenaAval.Substring(AV05_POS + 1, AV05_LNG);
            av.AV_06 = cadenaAval.Substring(AV06_POS + 1, AV06_LNG);
            av.AV_07 = cadenaAval.Substring(AV07_POS + 1, AV07_LNG);
            av.AV_08 = cadenaAval.Substring(AV08_POS + 1, AV08_LNG);
            av.AV_09 = cadenaAval.Substring(AV09_POS + 1, AV09_LNG);
            av.AV_10 = cadenaAval.Substring(AV10_POS + 1, AV10_LNG);
            av.AV_11 = cadenaAval.Substring(AV11_POS + 1, AV11_LNG);
            av.AV_12 = cadenaAval.Substring(AV12_POS + 1, AV12_LNG);
            av.AV_13 = cadenaAval.Substring(AV13_POS + 1, AV13_LNG);
            av.AV_14 = cadenaAval.Substring(AV14_POS + 1, AV14_LNG);
            av.AV_15 = cadenaAval.Substring(AV15_POS + 1, AV15_LNG);
            av.AV_16 = cadenaAval.Substring(AV16_POS + 1, AV16_LNG);
            av.AV_17 = cadenaAval.Substring(AV17_POS + 1, AV17_LNG);
            av.AV_18 = cadenaAval.Substring(AV18_POS + 1, AV18_LNG);
            av.AV_19 = cadenaAval.Substring(AV19_POS + 1, AV19_LNG);
            av.AV_20 = cadenaAval.Substring(AV20_POS + 1, AV20_LNG);
            av.AV_21 = cadenaAval.Substring(AV21_POS + 1, AV21_LNG);
            crItem.AVs.Add(av);
        }

        return av;
    }



    private string TransformarFecha(string cadena)
    {
        string CadenaFecha = cadena.Substring(0, 2) + "/" + cadena.Substring(2, 2) + "/" + cadena.Substring(4, 4);
        return CadenaFecha;
    }





    protected void imgExportArchivoXLS_Click(object sender, ImageClickEventArgs e)
    {
        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(8, idUs, "Exportación a Excel");
        rgArchivoMorales.MasterTableView.AllowPaging = false;
        rgArchivoMorales.Rebind();

        foreach (GridDataItem dgi in rgArchivoMorales.MasterTableView.Items)
            dgi.Expanded = true;

        List<RadGrid> rg = new List<RadGrid>();
        rg.Add(rgArchivoMorales);
        string[] titulares = new string[1];
        titulares[0] = "titulo";
        string fileName = string.Format("{0}{2}{1}", "PersonasMoralesDetalles", ".xls", DateTime.Now.ToString());

        DataSet dsReportePM = ExportarExcel.GenerarExcelDetallesPM(rg, titulares, "Detalle Personas Morales", "Banobras");
        ExportarReportePM(dsReportePM, fileName);

        rgArchivoMorales.MasterTableView.AllowPaging = true;
        rgArchivoMorales.Rebind();
    }

    protected void imgExportArchivoPDF_Click(object sender, ImageClickEventArgs e)
    {
        // Logica para exportar PDF
        string[] nombres = new string[] { "PROCESO DE PERSONAS MORALES" };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {
            Response.ContentType = "application/force-download";
            rgArchivoMorales.MasterTableView.HierarchyDefaultExpanded = true;
            rgArchivoMorales.ExportSettings.OpenInNewWindow = false;
            rgArchivoMorales.ExportSettings.ExportOnlyData = true;
            rgArchivoMorales.ExportSettings.IgnorePaging = true;
            rgArchivoMorales.ExportSettings.OpenInNewWindow = true;
            rgArchivoMorales.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
            rgArchivoMorales.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
            rgArchivoMorales.MasterTableView.ExportToPdf();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó conciliación PM");
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    protected void rgErrores_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        dtErrores = Session["erroresInfoPM"] as System.Data.DataTable;
        try
        {
            this.rgErrores.DataSource = dtErrores;
        }
        catch (Exception ex)
        {
            string strMessageError = ex.Message.ToString();
        }
    }

    protected void imgExportErrorXLS_Click(object sender, ImageClickEventArgs e)
    {
        rgErrores.MasterTableView.HierarchyDefaultExpanded = true;
        rgErrores.ExportSettings.OpenInNewWindow = false;
        rgErrores.MasterTableView.ExportToExcel();
        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(10, idUs, "Exportación a Excel");
    }

    protected void imgExportErrorPDF_Click(object sender, ImageClickEventArgs e)
    {
        // Logica para exportar PDF
        string[] nombres = new string[] { "ERRORES" };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {
            Response.ContentType = "application/force-download";
            rgErrores.MasterTableView.HierarchyDefaultExpanded = true;
            rgErrores.ExportSettings.OpenInNewWindow = false;
            rgErrores.ExportSettings.ExportOnlyData = true;
            rgErrores.ExportSettings.IgnorePaging = true;
            rgErrores.ExportSettings.OpenInNewWindow = true;
            rgErrores.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
            rgErrores.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
            rgErrores.MasterTableView.ExportToPdf();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó Error PM");
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }

    protected void rgWarnings_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        dtWarnings = Session["warningsInfoPM"] as System.Data.DataTable;
        try
        {
            this.rgWarnings.DataSource = dtWarnings;
        }
        catch (Exception ex)
        {
            string strMessageError = ex.Message.ToString();
        }
    }

    protected void imgExportWarningXLS_Click(object sender, ImageClickEventArgs e)
    {
        rgWarnings.MasterTableView.HierarchyDefaultExpanded = true;
        rgWarnings.ExportSettings.OpenInNewWindow = false;
        rgWarnings.MasterTableView.ExportToExcel();
        idUs = Parser.ToNumber(Session["idUsuario"].ToString());
        ActividadRules.GuardarActividad(10, idUs, "Exportación a Excel");
    }

    protected void imgExportWarningPDF_Click(object sender, ImageClickEventArgs e)
    {
        // Logica para exportar PDF
        string[] nombres = new string[] { "WARNINGS" };
        List<RadGrid> listaGrids = new List<RadGrid>();

        try
        {
            Response.ContentType = "application/force-download";
            rgWarnings.MasterTableView.HierarchyDefaultExpanded = true;
            rgWarnings.ExportSettings.OpenInNewWindow = false;
            rgWarnings.ExportSettings.ExportOnlyData = true;
            rgWarnings.ExportSettings.IgnorePaging = true;
            rgWarnings.ExportSettings.OpenInNewWindow = true;
            rgWarnings.ExportSettings.Pdf.PageHeight = Unit.Parse("162mm");
            rgWarnings.ExportSettings.Pdf.PageWidth = Unit.Parse("600mm");
            rgWarnings.MasterTableView.ExportToPdf();
            int idUs = Parser.ToNumber(Session["idUsuario"].ToString());
            ActividadRules.GuardarActividad(2222, idUs, "El Usuario " + Session["nombreUser"] + "Exportó Warning PM");
        }
        catch (Exception ex)
        {
            Mensajes.ShowError(this.Page, this.GetType(), ex);
        }
    }


    private void getFacultades()
    {
        UsuarioRules facultad = new UsuarioRules();

        if (!Session["Facultades"].ToString().Contains("|" + facultad.GetVariable("CDPM") + "|"))
        {
            Mensajes.ShowAdvertencia(this.Page, this.GetType(), "Debe ingresar un usuario válido", "~/Login.aspx");
        }
    }

    private void CambiaAtributosRGR(GridFilterMenu menu)
    {
        for (int i = 0; i < menu.Items.Count; i++)
        {
            if (menu.Items[i].Text.Equals("NoFilter")) menu.Items[i].Text = "Sin Filtro";
            if (menu.Items[i].Text.Equals("EqualTo")) menu.Items[i].Text = "Igual";
            if (menu.Items[i].Text.Equals("NotEqualTo")) menu.Items[i].Text = "Diferente";
            if (menu.Items[i].Text.Equals("GreaterThan")) menu.Items[i].Text = "Mayor que";
            if (menu.Items[i].Text.Equals("LessThan")) menu.Items[i].Text = "Menor que";
            if (menu.Items[i].Text.Equals("GreaterThanOrEqualTo")) menu.Items[i].Text = "Mayor o igual a";
            if (menu.Items[i].Text.Equals("LessThanOrEqualTo")) menu.Items[i].Text = "Menor o igual a";
            if (menu.Items[i].Text.Equals("Between")) menu.Items[i].Text = "Entre";
            if (menu.Items[i].Text.Equals("NotBetween")) menu.Items[i].Text = "No entre";
            if (menu.Items[i].Text.Equals("IsNull")) menu.Items[i].Text = "Es nulo";
            if (menu.Items[i].Text.Equals("NotIsNull")) menu.Items[i].Text = "No es nulo";
            if (menu.Items[i].Text.Equals("Contains")) menu.Items[i].Text = "Contenga";
            if (menu.Items[i].Text.Equals("DoesNotContain")) menu.Items[i].Text = "No Contenga";
            if (menu.Items[i].Text.Equals("StartsWith")) menu.Items[i].Text = "Inicie con";
            if (menu.Items[i].Text.Equals("EndsWith")) menu.Items[i].Text = "Finalice con";
            if (menu.Items[i].Text.Equals("NotIsEmpty")) menu.Items[i].Text = "No es vacio";
            if (menu.Items[i].Text.Equals("IsEmpty")) menu.Items[i].Text = "Vacio";
        }
    }

    private void ExportarReportePM(DataSet dsReporte, string NombreArchivo)
    {
        GridView grvTemporal = new GridView();
        grvTemporal.DataSource = dsReporte;
        grvTemporal.DataBind();

        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + NombreArchivo);
        Response.Charset = "";

        this.EnableViewState = false;
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

        grvTemporal.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }

    protected void grids_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        // Conservar nombres de filtros en español al filtrar dato (sobre filtro y paginacion)
        if (e.CommandName == RadGrid.FilterCommandName ||
             e.CommandName == RadGrid.PageCommandName ||
             e.CommandName.Equals("ChangePageSize") ||
             e.CommandName == RadGrid.PrevPageCommandArgument ||
             e.CommandName == RadGrid.NextPageCommandArgument ||
             e.CommandName == RadGrid.FirstPageCommandArgument ||
             e.CommandName == RadGrid.LastPageCommandArgument)
        {
            RadGrid grid = (RadGrid)sender;
            CambiaAtributosRGR(grid.FilterMenu);
        }
    }


}