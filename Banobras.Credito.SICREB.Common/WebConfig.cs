using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common.Block;

namespace Banobras.Credito.SICREB.Common
{

    public class WebConfig
    {

        public static string site;
        private static string urlImages;
        private static string patronDeFecha;
        private static string patronDeRFC;
        private static string patronDeRFCFisicas;
        private static string mailFrom;
        private static string smtpClient;
        private static string rutaActiveDirectory;
        private static string dcActiveDirectory;
        private static string wcTipoCreditoRE;
        private static string wcTipoCreditoPL;

        private static string wcINTF_ES;
        private static string wcINTF_01_ES;
        private static string wcINTF_05_ES;
        private static string wcINTF_07_ES;
        private static string wcINTF_17_ES;
        private static string wcINTF_33_ES;
        private static string wcPN_Etiqueta_ES;
        private static string wcPA_Etiqueta_ES;
        private static string wcPE_Etiqueta_ES;
        private static string wcTL_Etiqueta_ES;
        private static string wcTL_01_ES;
        private static string wcTL_02_ES;
        private static string wcTL_05_ES;
        private static string wcTL_08_ES;
        //TODO: SOL53051 => Campos telefono y mail obligatorios
        private static string wcTL_52_ES;
        private static string wcTL_99_ES;
        private static string wcTR_Etiqueta_ES;
        private static string wcTR_72_ES;
        private static string wcTR_78_ES;
        private static string wcTR_94_ES;

        private static string wcHD_HD;
        private static string wcHD_00;
        private static string wcHD_01;
        private static string wcHD_02;
        private static string wcHD_06;
        private static string wcHD_07;
        private static string wcHD_03;
        private static string wcEM_11;
        private static string wcEM_12;

        #region Parametros Auxiliares

            public static string Site
            {
                get
                {
                    if (site == null || site.Length == 0)
                    {
                        try
                        {
                            site = ConfigurationManager.AppSettings["Site"].ToString();
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            site = string.Empty;
                        }
                    }

                    return site;
                }
            }

            public static string UrlImages
            {
                get
                {
                    if (urlImages == null || urlImages.Length == 0)
                    {
                        try
                        {
                            urlImages = ConfigurationManager.AppSettings["UrlImages"].ToString();
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            urlImages = string.Empty;
                        }
                    }

                    return urlImages;
                }
            }

            public static string PatronDeFecha
            {
                get
                {
                    if (string.IsNullOrEmpty(patronDeFecha))
                    {

                        try
                        {
                            patronDeFecha = ConfigurationManager.AppSettings["PatronDeFecha"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            patronDeFecha = string.Empty;
                        }
                    }

                    return patronDeFecha;
                }
            }

            public static string PatronDeRFC
            {
                get
                {
                    if (string.IsNullOrEmpty(patronDeRFC))
                    {

                        try
                        {
                            patronDeRFC = ConfigurationManager.AppSettings["PatronDeRFC"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            patronDeRFC = string.Empty;
                        }
                    }

                    return patronDeRFC;
                }
            }

            public static string PatronDeRFCFisicas
            {
                get
                {
                    if (string.IsNullOrEmpty(patronDeRFCFisicas))
                    {

                        try
                        {
                            patronDeRFCFisicas = ConfigurationManager.AppSettings["PatronDeRFCFisicas"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            patronDeRFCFisicas = string.Empty;
                        }
                    }

                    return patronDeRFCFisicas;
                }
            }

            public static string MailFrom
            {
                get
                {
                    if (mailFrom == null || mailFrom.Length == 0)
                    {
                        try
                        {
                            mailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            mailFrom = string.Empty;
                        }
                    }

                    return mailFrom;
                }
            }

            public static string SmtpClient
            {
                get
                {
                    if (smtpClient == null || smtpClient.Length == 0)
                    {
                        try
                        {
                            smtpClient = ConfigurationManager.AppSettings["SmtpClient"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            smtpClient = string.Empty;
                        }
                    }

                    return smtpClient;
                }
            }

            public static string RutaActiveDirectory
            {
                get
                {
                    if (rutaActiveDirectory == null || rutaActiveDirectory.Length == 0)
                    {
                        try
                        {
                            rutaActiveDirectory = ConfigurationManager.AppSettings["RutaActiveDirectory"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            rutaActiveDirectory = string.Empty;
                        }
                    }

                    return rutaActiveDirectory;
                }
            }

            public static string DCActiveDirectory
            {
                get
                {
                    if (dcActiveDirectory == null || dcActiveDirectory.Length == 0)
                    {
                        try
                        {
                            dcActiveDirectory = ConfigurationManager.AppSettings["SmtpClient"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            dcActiveDirectory = string.Empty;
                        }
                    }

                    return dcActiveDirectory;
                }
            }

            public static string TipoCreditoRE
            {
                get
                {
                    if (wcTipoCreditoRE == null || wcTipoCreditoRE.Length == 0)
                    {
                        try
                        {
                            wcTipoCreditoRE = ConfigurationManager.AppSettings["TipoCreditoRE"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTipoCreditoRE = string.Empty;
                        }
                    }

                    return wcTipoCreditoRE;
                }
            }

            public static string TipoCreditoPL
            {
                get
                {
                    if (wcTipoCreditoPL == null || wcTipoCreditoPL.Length == 0)
                    {
                        try
                        {
                            wcTipoCreditoPL = ConfigurationManager.AppSettings["TipoCreditoPL"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTipoCreditoPL = string.Empty;
                        }
                    }

                    return wcTipoCreditoPL;
                }
            }

        #endregion

        #region Parametros del archivo PF

            /// <summary>
            /// Recupera el texto del encabezado para personas físicas.
            /// </summary>
            public static string INTF_ES
            {
                get
                {
                    if (wcINTF_ES == null || wcINTF_ES.Length == 0)
                    {

                        try
                        {
                            wcINTF_ES = ConfigurationManager.AppSettings["INTF_ES"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcINTF_ES = string.Empty;
                        }
                    }

                    return wcINTF_ES;
                }
            }
        
            /// <summary>
            /// Recupera el texto del encabezado para personas físicas.
            /// </summary>
            public static string INTF_01_ES
            {
                get
                {
                    if (wcINTF_01_ES == null || wcINTF_01_ES.Length == 0)
                    {

                        try
                        {
                            wcINTF_01_ES = ConfigurationManager.AppSettings["INTF_01"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcINTF_01_ES = string.Empty;
                        }
                    }

                    return wcINTF_01_ES;
                }
            }

            /// <summary>
            /// Version de formato del archivo
            /// </summary>
            public static string INTF_05_ES
            {
                get
                {
                    if (wcINTF_05_ES == null || wcINTF_05_ES.Length == 0)
                    {

                        try
                        {
                            wcINTF_05_ES = ConfigurationManager.AppSettings["INTF_05"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcINTF_05_ES = string.Empty;
                        }
                    }

                    return wcINTF_05_ES;
                }
            }

            public static string INTF_07_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcINTF_07_ES))
                    {

                        try
                        {
                            wcINTF_07_ES = ConfigurationManager.AppSettings["INTF_07"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcINTF_07_ES = string.Empty;
                        }
                    }

                    return wcINTF_07_ES;
                }
            }

            public static string INTF_17_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcINTF_17_ES))
                    {

                        try
                        {
                            wcINTF_17_ES = ConfigurationManager.AppSettings["INTF_17"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcINTF_17_ES = string.Empty;
                        }
                    }

                    return wcINTF_17_ES;
                }
            }

            public static string INTF_33_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcINTF_33_ES))
                    {

                        try
                        {
                            wcINTF_33_ES = ConfigurationManager.AppSettings["INTF_33"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcINTF_33_ES = string.Empty;
                        }
                    }

                    return wcINTF_33_ES;
                }
            }

            /// <summary>
            /// Etiqueta de inicio del segmento de nombre(PN)
            /// </summary>
            public static string PN_Etiqueta_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcPN_Etiqueta_ES))
                    {

                        try
                        {
                            wcPN_Etiqueta_ES = ConfigurationManager.AppSettings["PN_Etiqueta"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcPN_Etiqueta_ES = string.Empty;
                        }
                    }

                    return wcPN_Etiqueta_ES;
                }
            }

            /// <summary>
            /// Etiqueta de inicio del segmento de direccion(PA)
            /// </summary>
            public static string PA_Etiqueta_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcPA_Etiqueta_ES))
                    {

                        try
                        {
                            wcPA_Etiqueta_ES = ConfigurationManager.AppSettings["PA_Etiqueta"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcPA_Etiqueta_ES = string.Empty;
                        }
                    }

                    return wcPA_Etiqueta_ES;
                }
            }

            /// <summary>
            /// Etiqueta de inicio del segmento de empleo(PE)
            /// </summary>
            public static string PE_Etiqueta_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcPE_Etiqueta_ES))
                    {

                        try
                        {
                            wcPE_Etiqueta_ES = ConfigurationManager.AppSettings["PE_Etiqueta"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcPE_Etiqueta_ES = string.Empty;
                        }
                    }

                    return wcPE_Etiqueta_ES;
                }
            }

            /// <summary>
            /// Etiqueta de inicio del segmento de cuentas(PE)
            /// </summary>
            public static string TL_Etiqueta_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTL_Etiqueta_ES))
                    {

                        try
                        {
                            wcTL_Etiqueta_ES = ConfigurationManager.AppSettings["TL_Etiqueta"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTL_Etiqueta_ES = string.Empty;
                        }
                    }

                    return wcTL_Etiqueta_ES;
                }
            }

            public static string TL_01_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTL_01_ES))
                    {

                        try
                        {
                            wcTL_01_ES = ConfigurationManager.AppSettings["TL_01"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTL_01_ES = string.Empty;
                        }
                    }

                    return wcTL_01_ES;
                }
            }

            public static string TL_02_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTL_02_ES))
                    {

                        try
                        {
                            wcTL_02_ES = ConfigurationManager.AppSettings["TL_02"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTL_02_ES = string.Empty;
                        }
                    }

                    return wcTL_02_ES;
                }
            }

            public static string TL_05_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTL_05_ES))
                    {
                        try
                        {
                            wcTL_05_ES = ConfigurationManager.AppSettings["TL_05"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTL_05_ES = string.Empty;
                        }
                    }

                    return wcTL_05_ES;
                }
            }

            public static string TL_08_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTL_08_ES))
                    {

                        try
                        {
                            wcTL_08_ES = ConfigurationManager.AppSettings["TL_08"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTL_08_ES = string.Empty;
                        }
                    }
                    return wcTL_08_ES;
                }
            }

            /// <summary>
            /// TODO: SOL53051 => Campos telefono y mail obligatorios
            /// </summary>
            public static string TL_52_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTL_52_ES))
                    {

                        try
                        {
                            wcTL_52_ES = ConfigurationManager.AppSettings["TL_52"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTL_52_ES = string.Empty;
                        }
                    }
                    return wcTL_52_ES;
                }
            }

            /// <summary>
            /// Etiqueta de inicio del segmento de cuentas(PE)
            /// </summary>
            public static string TL_99_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTL_99_ES))
                    {

                        try
                        {
                            wcTL_99_ES = ConfigurationManager.AppSettings["TL_99"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTL_99_ES = string.Empty;
                        }
                    }

                    return wcTL_99_ES;
                }
            }

            public static string TR_Etiqueta_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTR_Etiqueta_ES))
                    {

                        try
                        {
                            wcTR_Etiqueta_ES = ConfigurationManager.AppSettings["TR_Etiqueta"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTR_Etiqueta_ES = string.Empty;
                        }
                    }

                    return wcTR_Etiqueta_ES;
                }
            }

            public static string TR_72_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTR_72_ES))
                    {

                        try
                        {
                            wcTR_72_ES = ConfigurationManager.AppSettings["TR_72"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTR_72_ES = string.Empty;
                        }
                    }

                    return wcTR_72_ES;
                }
            }

            public static string TR_78_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTR_78_ES))
                    {

                        try
                        {
                            wcTR_78_ES = ConfigurationManager.AppSettings["TR_78"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTR_78_ES = string.Empty;
                        }
                    }

                    return wcTR_78_ES;
                }
            }

            public static string TR_94_ES
            {
                get
                {
                    if (string.IsNullOrEmpty(wcTR_94_ES))
                    {

                        try
                        {
                            wcTR_94_ES = ConfigurationManager.AppSettings["TR_94"];
                        }
                        catch (Exception oEx)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(oEx.ToString(), Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcTR_94_ES = string.Empty;
                        }
                    }

                    return wcTR_94_ES;
                }
            }

        #endregion

        #region Parametros del archivo PM

            public static string HD_HD
            {
                get
                {
                    if (wcHD_HD == null || wcHD_HD.Length == 0)
                    {
                        try
                        {
                            wcHD_HD = ConfigurationManager.AppSettings["HD_HD"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcHD_HD = string.Empty;
                        }
                    }

                    return wcHD_HD;
                }
            }

            public static string HD_00
            {
                get
                {
                    if (wcHD_00 == null || wcHD_00.Length == 0)
                    {
                        try
                        {
                            wcHD_00 = ConfigurationManager.AppSettings["HD_00"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcHD_00 = string.Empty;
                        }
                    }

                    return wcHD_00;
                }
            }

            public static string HD_01
            {
                get
                {
                    if (wcHD_01 == null || wcHD_01.Length == 0)
                    {
                        try
                        {
                            wcHD_01 = ConfigurationManager.AppSettings["HD_01"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcHD_01 = string.Empty;
                        }
                    }

                    return wcHD_01;
                }
            }

            public static string HD_02
            {
                get
                {
                    if (wcHD_02 == null || wcHD_02.Length == 0)
                    {
                        try
                        {
                            wcHD_02 = ConfigurationManager.AppSettings["HD_02"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcHD_02 = string.Empty;
                        }
                    }

                    return wcHD_02;
                }
            }

            public static string HD_03
            {
                get
                {
                    if (wcHD_03 == null || wcHD_03.Length == 0)
                    {
                        try
                        {
                            wcHD_03 = ConfigurationManager.AppSettings["HD_03"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcHD_03 = string.Empty;
                        }
                    }

                    return wcHD_03;
                }
            }

            public static string HD_06
            {
                get
                {
                    if (wcHD_06 == null || wcHD_06.Length == 0)
                    {
                        try
                        {
                            wcHD_06 = ConfigurationManager.AppSettings["HD_06"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcHD_06 = string.Empty;
                        }
                    }

                    return wcHD_06;
                }
            }

            public static string HD_07
            {
                get
                {
                    if (wcHD_07 == null || wcHD_07.Length == 0)
                    {
                        try
                        {
                            wcHD_07 = ConfigurationManager.AppSettings["HD_07"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcHD_07 = string.Empty;
                        }
                    }

                    return wcHD_07;
                }
            }

            public static string EM_11
            {
                get
                {
                    if (wcEM_11 == null || wcEM_11.Length == 0)
                    {
                        try
                        {
                            wcEM_11 = ConfigurationManager.AppSettings["EM_11"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcEM_11 = string.Empty;
                        }
                    }

                    return wcEM_11;
                }
            }

            public static string EM_12
            {
                get
                {
                    if (wcEM_12 == null || wcEM_12.Length == 0)
                    {
                        try
                        {
                            wcEM_12 = ConfigurationManager.AppSettings["EM_12"].ToString();
                        }
                        catch (Exception ex)
                        {
                            BLog log = BLog.Current;
                            if (log != null)
                            {
                                log.LogWrtMrg.Write(ex.Message, Definitions.CATEGORY_WC, 3, 1000);
                            }
                            wcEM_12 = string.Empty;
                        }
                    }

                    return wcEM_12;
                }
            }

        #endregion

    }

}
