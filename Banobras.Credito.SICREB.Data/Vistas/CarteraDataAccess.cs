using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities.Vistas;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Entities.Types.PF;

namespace Banobras.Credito.SICREB.Data.Vistas
{

    public class CarteraDataAccess : OracleBase
    {

        private Enums.Persona persona;    
        private List<Cartera> allRecords = null;
        private List<PM_CR> allRecordsCRs = null;
        private List<PM_DE> allRecordsDEs = null;
        private List<PF_TL> allRecordsTLs = null;

        public CarteraDataAccess(Enums.Persona persona)
        {
            this.persona = persona;
        }

        #region Funciones del Segmento PM_CR

            public List<Cartera> GetRegistros(string periodo, Enums.Reporte reporte)
            {
                if (allRecords == null)
                {
                    allRecords = new List<Cartera>(); 
                    String query = string.Format("DATOSEXTERNOS.SP_ObtenerCarteraP{0}_{1}",(persona == Enums.Persona.Moral ? "M" : "F"), reporte);

                    //MASS. 04octubre2021 para pruebas en desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        query = string.Format("DATOSEXTERNOS_Z.SP_ObtenerCarteraP{0}_{1}", (persona == Enums.Persona.Moral ? "M" : "F"), reporte);
                    }
                
                    DbCommand cmd = DB.GetStoredProcCommand(query);
                    DB.AddInParameter(cmd, "pPeriodo", DbType.AnsiString, periodo);

                    using (IDataReader reader = DB.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Cartera item = new Cartera();

                            item.Status = Parser.ToNumber(reader["STATUS"]);
                            item.Rfc = reader["RFC"].ToString();
                            item.Rfc_Conse = Parser.ToNumber(reader["RFC_CONSE"]);
                            item.Credito = Parser.ToNumber(reader["CREDITO"]);
                            item.Auxiliar = reader["AUXILIAR"].ToString();
                            item.Credito_Anterior = reader["CREDITO_ANTERIOR"].ToString();
                            item.Cta_Contab_Capital = reader["CTA_CONTAB_CAPITAL"].ToString();
                            item.Fecha_Apertura = Parser.ToDateTime(reader["FECHA_APERTURA"]);
                            item.Fecha_Vencimiento = Parser.ToDateTime(reader["FECHA_VENCIMIENTO"]);
                            item.Tipo_Credito = Parser.ToNumber(reader["TIPO_CREDITO"]);
                            item.Saldo_Inicial = Parser.ToDouble(reader["SALDO_INICIAL"]);
                            item.Cve_Moneda = Parser.ToNumber(reader["CVE_MONEDA"]);
                            item.Moneda = reader["MONEDA"].ToString();
                            item.MonedaBuro = reader["MONEDA_BURO"].ToString();
                            item.Numero_Pagos = Parser.ToNumber(reader["NUMERO_PAGOS"]);
                            item.Numero_Pagos_Buro = reader["NUMERO_PAGOS_BURO"].ToString();
                            item.Frecuencia_Pago = reader["FRECUENCIA_PAGO"].ToString();
                            item.Frecuencia_Pago_Buro = reader["FRECUENCIA_PAGO_BURO"].ToString();
                            item.Importe_Pago = Parser.ToDouble(reader["IMPORTE_PAGO"]);
                            item.Fecha_Ultimo_Pago = Parser.ToDateTime(reader["FECHA_ULTIMO_PAGO"]);
                            item.Fecha_Ultima_Compra = Parser.ToDateTime(reader["FECHA_ULTIMA_COMPRA"]);
                            item.Fecha_Reestructura = Parser.ToDateTime(reader["FECHA_REESTRUCTURA"]);
                            item.Fecha_Cierre = Parser.ToDateTime(reader["FECHA_CIERRE"]);
                            item.Importe = Parser.ToDouble(reader["IMPORTE"]);
                            item.Quita = Parser.ToDouble(reader["QUITA"]);
                            item.Dacion_Pago = Parser.ToDouble(reader["DACION_PAGO"]);
                            item.Quebranto = Parser.ToDouble(reader["QUEBRANTO"]);
                            item.Dias_Vencimiento = Parser.ToNumber(reader["DIAS_VENCIMIENTO"]);
                            item.Num_Pag_Vencidos = Parser.ToNumber(reader["NUM_PAG_VENCIDOS"]);
                            item.Fec_Amort_Vencida = Parser.ToDateTime(reader["FEC_AMORT_VENCIDA"]);
                            item.Saldo_Insoluto = Parser.ToDouble(reader["SALDO_INSOLUTO"]);
                            item.Fecha = Parser.ToDateTime(reader["FECHA"]);
                            item.Pago_Cta_13 = Parser.ToDouble(reader["PAGO_CTA_13"]);
                            item.Pago_Cta_Orden = Parser.ToDouble(reader["PAGO_CTA_ORDEN"]);
                            item.Pago_Vigete = Parser.ToDouble(reader["PAGO_VIGENTE"]);
                            item.Calificacion = reader["CALIFICACION"].ToString();
                            item.Monto_Pagar = Parser.ToDouble(reader["MONTO_PAGAR"]);
                            item.Monto_Pagar_Vencido = Parser.ToDouble(reader["MONTO_PAGAR_VENCIDO"]);
                            allRecords.Add(item);
                           
                        }
                    }
                }

                return allRecords;
            }
       
            /// <summary>
            /// Carga los segmentos CRs de las vistas. StoredProcedures: SP_ObtenerCarteraPM_Mensual o _Semanal
            /// </summary>
            /// <param name="periodo">Periodo del reporte. (HD_05)</param>
            /// <param name="reporte">Mensual o Semanal</param>
            /// <param name="procesa">Indica si se inicia el proceso o se cargan los últimos datos procesados.</param>
            /// <param name="cuenta6378Activa">Resultado de "Cuenta 6378"</param>
            /// <param name="archivoId">Id del archivo (T_ARCHIVOS)</param>
            /// <returns>Colleccion de todos los creditos guardados en la BD</returns>
            public List<PM_CR> GetPM_CRs(string periodo, Enums.Reporte reporte, bool procesa, int cuenta6378Activa, int archivoId, string grupos)
            {
                if (allRecordsCRs == null)
                {
                    allRecordsCRs = new List<PM_CR>();
                    try
                    {
                        String query = string.Format("DATOSEXTERNOS.SP_ObtenerCarteraPM_{0}", reporte);

                        //MASS. 04octubre2021 para pruebas en desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            query = string.Format("DATOSEXTERNOS_Z.SP_ObtenerCarteraPM_{0}", reporte);
                        }                        

                        DbCommand cmd = DB.GetStoredProcCommand(query);
                        DB.AddInParameter(cmd, "pPeriodo", DbType.AnsiString, periodo);
                        DB.AddInParameter(cmd, "pProcesar", DbType.AnsiString, (procesa ? "1" : "0"));
                        DB.AddInParameter(cmd, "pArchivoId", DbType.Int32, archivoId);
                        DB.AddInParameter(cmd, "pGrupos", DbType.AnsiString, grupos);

                        using (IDataReader reader = DB.ExecuteReader(cmd))
                        {
                            #region procesando GetRegistros ...
                            while (reader.Read())
                            {

                                if (reader["CR_00"].ToString() == "DDI0812039SA")
                                {
                                    string Temporal = "DDI0812039SA";
                                }


                                DateTime FechaApertura = Parser.ToDateTime(reader["FECHA_APERTURA"]);
                                DateTime FechaVencimiento = Parser.ToDateTime(reader["FECHA_VENCIMIENTO"]);
                                DateTime FechaUltimoPago = Parser.ToDateTime(reader["FECHA_ULTIMO_PAGO"]);
                                DateTime FechaCierre = Parser.ToDateTime(reader["FECHA_CIERRE"]);
                                DateTime FechaPrimerIncumplimiento = Parser.ToDateTime(reader["FEC_AMORT_VENCIDA"]);

                                //<MASS 08/agosto/2018 ajustes PM-V05.
                                DateTime FechaIngresoCarteraVencida = Parser.ToDateTime(reader["FECHA_TRASPASO"]);
                                //</MASS>

                                double SaldoInicial = Parser.ToDouble(reader["SALDO_INICIAL"]);
                                double SaldoInsoluto = Parser.ToDouble(reader["SALDO_INSOLUTO"]);
                                double ImportePago = Parser.ToDouble(reader["IMPORTE_PAGO"]);
                                double Quita = Parser.ToDouble(reader["QUITA"]);
                                double Dacion = Parser.ToDouble(reader["DACION"]);
                                double Quebranto = Parser.ToDouble(reader["QUEBRANTO"]);

                                PM_CR item = new PM_CR();
                                item.CR_CR = reader["CR_CR"].ToString();
                                item.CR_00 = reader["CR_00"].ToString();
                                item.CR_01 = ""; // Sin Dato
                                item.CR_02 = reader["CR_02"].ToString();
                                item.CR_03 = reader["CR_03"].ToString();
                                item.CR_04 = AjustarCR_04_FechaApertura( FechaApertura );
                                item.CR_05 = AjustarCR_05_PlazoMeses( FechaApertura, FechaVencimiento );
                                item.CR_06 = reader["CR_06"].ToString();
                                item.CR_07 = AjustarCR_07_MontoAutorizadoCredito( SaldoInicial );
                                item.CR_08 = reader["CR_08"].ToString();
                                item.CR_09 = reader["CR_09"].ToString();
                                item.CR_10 = AjustarCR_10_FrecuenciaPagos(reader["CR_10"].ToString(), Parser.ToNumber(reader["CR_09"].ToString()), FechaApertura, FechaVencimiento); 
                                item.CR_11 = AjustarCR_11_ImportePago( ImportePago );
                                item.CR_12 = AjustarCR_12_FechaUltimoPago(FechaUltimoPago, FechaApertura);
                                item.CR_13 = ""; // Sin Dato
                                item.CR_14 = AjustarCR_14_PagoEfectivo(0);
                                item.CR_15 = AjustarCR_15_FechaLiquidacion( FechaCierre );
                                item.CR_16 = AjustarCR_16_Quita( Quita );
                                item.CR_17 = AjustarCR_17_DacionPago( Dacion );
                                item.CR_18 = AjustarCR_18_QuebrantoCastigo( Quebranto );
                                item.CR_19 = AjustarCR_19_ClaveObservacion(reader["CR_19"].ToString(), Parser.ToDouble(reader["Monto_vencido"]));
                                item.CR_20 = ""; // Sin Dato
                                item.CR_21 = AjustarCR_21_FechaPrimerIncumplimiento( FechaPrimerIncumplimiento );
                                item.CR_22 = AjustarCR_22_SaldoInsoluto(SaldoInsoluto, reader["CR_00"].ToString(), reader["CR_02"].ToString());
                                item.CR_23 = AjustarCR_23_CreditoMaximo( SaldoInicial ); // CAMPO NUEVO v4 - Temporalmente se coloca el SALDO INICIAL

                                //<MASS 08/noviembre/2017 ajustes PM-V05
                                //item.CR_24 = ""; // Sin Dato
                                item.CR_24 = AjustarCR_24_FechaIngresoCarteraVencida(FechaIngresoCarteraVencida);
                                item.CR_25 = ""; // Sin Dato
                                //</MASS>


                                // Informacion Complamentaria
                                item.Id = Parser.ToNumber(reader["ID"]);
                                item.EM_ID = Parser.ToNumber(reader["EM_ID"]);
                                item.Status = Parser.ToNumber(reader["STATUS"]);
                                item.DiasVencido = Parser.ToNumber(reader["Dias_Vencido"]);
                                
                                item.Auxiliar = reader["AUXILIAR"].ToString();
                                item.Calificacion = reader["CALIFICACION"].ToString();

                                item.Fecha_Cierre = FechaCierre;
                                item.Fecha_Reestructura = Parser.ToDateTime(reader["FECHA_REESTRUCTURA"]);
                                
                                item.Pago_Cta_13 = Parser.ToDouble(reader["PAGO_CTA_13"]);
                                item.Pago_Cta_Orden = Parser.ToDouble(reader["PAGO_CTA_ORDEN"]);
                                item.Pago_Vigente = Parser.ToDouble(reader["PAGO_VIGENTE"]);
                                item.MontoPagar = Parser.ToDouble(reader["Monto_Pagar"]);
                                item.MontoPagarVencido = Parser.ToDouble(reader["Monto_vencido"]);


                                if (reader["CR_19"].ToString().Trim() == "RV")
                                {
                                    if (Parser.ToDateTime(reader["FECHA_CIERRE"]) != default(DateTime))
                                        item.CR_13 = Parser.ToDateTime(reader["FECHA_CIERRE"]).ToString(Parser.FORMATO_FECHA);
                                    else
                                        item.CR_13 = "00000000";
                                }
                                else
                                {
                                    if (Parser.ToDateTime(reader["FECHA_REESTRUCTURA"]).ToString("dd/MM/yyyy") != "01/01/0001")
                                        item.CR_13 = Parser.ToDateTime(reader["FECHA_REESTRUCTURA"]).ToString(Parser.FORMATO_FECHA);
                                    else
                                        item.CR_13 = "00000000";
                                }

                                double pagoEfectivo = 0;

                                if (cuenta6378Activa > 0)
                                    pagoEfectivo = (item.Pago_Vigente + item.Pago_Cta_Orden + item.Pago_Cta_13);
                                else
                                    pagoEfectivo = (item.Pago_Vigente + item.Pago_Cta_13);

                                if (item.Status == 1)
                                    pagoEfectivo = pagoEfectivo - (Quita + Dacion + Quebranto);

                                allRecordsCRs.Add(item);
                            } //while
                            #endregion
                        } //using
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al cargar segmentos CR", ex);
                    }                
                }

                return allRecordsCRs;
            }

            /// <summary>
            /// Cancela segmentos CR. StoredProcedure: SP_INVALID_CR
            /// </summary>
            /// <param name="cr">Segmento CR a cancelar</param>
            public void InvalidateCRs(PM_CR cr)
            {
                if (cr.IsValid)
                {
                    throw new Exception("No se puede cancelar un segmento CR válido");
                }
            
                try
                {
                    string query = "DATOSEXTERNOS.SP_INVALID_CR";

                    //MASS. 04octubre2021 pra pruebas en desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        query = "DATOSEXTERNOS_Z.SP_INVALID_CR";
                    }

                    DbCommand cmd = DB.GetStoredProcCommand(query);

                    DB.AddInParameter(cmd, "pId", DbType.Int32, cr.Id);

                    DB.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cancelar segmentos CR", ex);
                }
            }

            private string AjustarCR_04_FechaApertura(DateTime FechaAperturaOriginal)
            {
                return FechaAperturaOriginal.ToString(Parser.FORMATO_FECHA);
            }

            private string AjustarCR_05_PlazoMeses(DateTime FechaAperturaOriginal, DateTime FechaVencimientoOriginal)
            {
                double TotalDias = (FechaVencimientoOriginal - FechaAperturaOriginal).TotalDays;
                double PlazoMeses = TotalDias / 30.4;
                PlazoMeses = Math.Round(PlazoMeses, 2);
                String PlazoM = PlazoMeses.ToString("###0.00");

                return PlazoM;
            }

            private string AjustarCR_07_MontoAutorizadoCredito(double SaldoInicialOriginal)
            {
                return Math.Round(SaldoInicialOriginal, 0).ToString("#");
            }

            private string AjustarCR_10_FrecuenciaPagos(string FrecuenciaPagoOrg, Int32 NumeroPagosOrg, DateTime FechaAperturaOrg, DateTime FechaVencimientoOrg)
            {
                double days = 0;
                string FrecuenciaPagos = "";
                
                switch (FrecuenciaPagoOrg.ToUpper())
                {
                    case "F1": // V
                        if (FechaVencimientoOrg > FechaAperturaOrg)
                        {
                            days = (FechaVencimientoOrg - FechaAperturaOrg).TotalDays;
                        }
                        
                        FrecuenciaPagos = days.ToString("#");
                        break;
                    
                    case "F2": // E
                        if (FechaVencimientoOrg > FechaAperturaOrg && NumeroPagosOrg > 0)
                        {
                            days = ((FechaVencimientoOrg - FechaAperturaOrg).TotalDays) / NumeroPagosOrg;
                        }

                        FrecuenciaPagos = days.ToString("#");
                        break;
                    
                    default:
                        FrecuenciaPagos = FrecuenciaPagoOrg;
                        break;
                }

                return FrecuenciaPagos;
            }

            private string AjustarCR_11_ImportePago(double ImportePagoOriginal)
            {
                return Math.Round(ImportePagoOriginal, 0).ToString("#");
            }

            private string AjustarCR_12_FechaUltimoPago(DateTime FechaUltimoPagoOriginal, DateTime FechaAperturaOriginal)
            {
                if (FechaUltimoPagoOriginal.ToString(Parser.FORMATO_FECHA) == "01010001")
                {
                    // Si la fecha de ultimo pago esta vacio, colocamos la fecha de apertura del credito
                    // Este ajuste se realiza sobre todo para que los creditos con pago de Saldo e Intereses
                    // al vencimiento no sean rechaados.
                    return FechaAperturaOriginal.ToString(Parser.FORMATO_FECHA);
                }
                else
                {
                    return FechaUltimoPagoOriginal.ToString(Parser.FORMATO_FECHA);
                }
            }

            private string AjustarCR_14_PagoEfectivo(double PagoEfectivoOriginal)
            {
                return "0"; 
            }

            private string AjustarCR_15_FechaLiquidacion(DateTime FechaCierreOriginal)
            {
                if (FechaCierreOriginal == null)
                {
                    return "00000000";
                }
                else if (FechaCierreOriginal.ToString("dd/MM/yyyy") == "01/01/0001")
                {
                    return "00000000";
                }
                else if (FechaCierreOriginal.ToString("dd/MM/yyyy") == "01/01/1900")
                {
                    return "00000000";
                }
                else
                {
                    return FechaCierreOriginal.ToString(Parser.FORMATO_FECHA);
                }
            }

            private string AjustarCR_16_Quita(double QuitaOriginal)
            {
                return Math.Round(QuitaOriginal, 0).ToString("#"); 
            }

            private string AjustarCR_17_DacionPago(double PacionPagoOriginal)
            {
                return Math.Round(PacionPagoOriginal, 0).ToString("#"); 
            }

            private string AjustarCR_18_QuebrantoCastigo(double QuebrantoCastigoOriginal)
            {
                return Math.Round(QuebrantoCastigoOriginal, 0).ToString("#");
            }

            private string AjustarCR_19_ClaveObservacion(string ClaveObservacion, double SaldoVencido)
            {
                // Solicitud SMS de Fecha 02052016
                // Adecuación que permita reportar los creditos de las bases de Personas Fisicas y Personas Morales  (Mensuales y Semanales)
                // con las clave de observacion SG = Demanda por el otorgante (registrada en el catalogo de creditos con clave de onservacion), 
                // únicamente cuando presenten saldo vencido, en caso de no reportar saldo vencido que sean reportados sin dicha clave.

                if (ClaveObservacion.Trim() == "SG" && SaldoVencido == 0)
                    return "";
                else
                    return ClaveObservacion;
                
            }

            private string AjustarCR_21_FechaPrimerIncumplimiento(DateTime FechaAmortVencida)
            {

                if (FechaAmortVencida == null)
                {
                    return "00000000";
                }
                else if (FechaAmortVencida.ToString("dd/MM/yyyy") == "01/01/0001")
                {
                    return "00000000";
                }
                else if (FechaAmortVencida.ToString("dd/MM/yyyy") == "01/01/1900")
                {
                    return "00000000";
                }
                else
                {
                    return FechaAmortVencida.ToString("ddMMyyyy");
                }
            }

            private string AjustarCR_22_SaldoInsoluto(double SaldoInsolutoOriginal, string RFCAcreditado, string NumeroCredito)
            {
                double DatoCantidad = 0;
                double.TryParse(Math.Round(SaldoInsolutoOriginal, 0).ToString("#"), out DatoCantidad);

                // 28/08/2015 Incorporamos el Ajuste de Bono Cupon Cero

                BonoCuponCeroDataAccess datosBonoCuponCero = new BonoCuponCeroDataAccess(Enums.Persona.Moral);
                List<BonoCuponCero> DatosCuponCero = null;
                DatosCuponCero = datosBonoCuponCero.GetRecords(false);

                foreach (BonoCuponCero ItemCuponCero in DatosCuponCero)
                {
                    if (RFCAcreditado == ItemCuponCero.Rfc && NumeroCredito == ItemCuponCero.Credito)
                    {
                        DatoCantidad = DatoCantidad - ItemCuponCero.MontoInversion;
                    }
                }

                return DatoCantidad.ToString("###################0");
            }

            private string AjustarCR_23_CreditoMaximo(double CreditoMaximoOriginal)
            {
                return Math.Round(CreditoMaximoOriginal, 0).ToString("###################0");
            }

            //<MASS 08/noviembre/2017 ajustes PM-V05
            private string AjustarCR_24_FechaIngresoCarteraVencida(DateTime FechaIngresoCarteraVencida)
            {
                if (FechaIngresoCarteraVencida == null)
                {
                    return "00000000";
                }
                else if (FechaIngresoCarteraVencida.ToString("dd/MM/yyyy") == "01/01/0001")
                {
                    return "00000000";
                }
                else if (FechaIngresoCarteraVencida.ToString("dd/MM/yyyy") == "01/01/1900")
                {
                    return "00000000";
                }
                else
                {
                    return FechaIngresoCarteraVencida.ToString("ddMMyyyy");
                }
            }

        #endregion

        #region Funciones del Segmento PM_DE

            /// <summary>
            /// Carga segmentos DE. StoredProcedures: SP_LOAD_DE
            /// </summary>
            /// <param name="procesar">Indica si se inicia el proceso o se cargan los últimos datos procesados.</param>
            /// <param name="archivoId">Id del archivo (T_ARCHIVOS)</param>
            /// <returns>Colección de todos los segmentos DE guardados en la BD</returns>
            public List<PM_DE> GetCRDetails(bool procesar, int archivoId, string grupos)
            {
                if (allRecordsDEs == null)
                {
                    try
                    {
                        allRecordsDEs = new List<PM_DE>();

                        string query = "DATOSEXTERNOS.SP_LOAD_DE";

                        //MASS. 04octubre2021 para pruebas en desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            query = "DATOSEXTERNOS_Z.SP_LOAD_DE";
                        }

                        DbCommand cmd = DB.GetStoredProcCommand(query);
                        DB.AddInParameter(cmd, "pProcesar", DbType.AnsiString, (procesar ? "1" : "0"));
                        DB.AddInParameter(cmd, "pArchivoId", DbType.Int32, archivoId);
                        DB.AddInParameter(cmd, "pGrupos", DbType.AnsiString, grupos);

                        using (IDataReader reader = DB.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                PM_DE de = new PM_DE();

                                de.Id = Parser.ToNumber(reader["ID"]);
                                de.DE_DE = reader["DE_DE"].ToString();
                                de.DE_00 = reader["DE_00"].ToString();
                                de.DE_01 = reader["DE_01"].ToString();
                                de.DE_02 = AjustarDE_02_DiasVencido(Parser.ToNumber(reader["DIAS_VENCIDO"]));
                                de.DE_03 = AjustarDE_03_Cantidad(Parser.ToDouble(reader["CANTIDAD"]), reader["DE_00"].ToString(), reader["DE_01"].ToString(), Parser.ToNumber(reader["DIAS_VENCIDO"]));
                                
                                //<MASS ajustes versión PM-05
                                //de.DE_04 = reader["DE_04"].ToString();  linea anterior comentada

                                if(reader["INTERES"] != DBNull.Value)
                                    de.DE_04 = AjustarDE_04_Interes(Parser.ToDouble(reader["INTERES"]));

                                de.DE_05 = "";
                                // Informacion Complamentaria
                                de.Vigente = reader["VIGENTE"].ToString();
                                de.Auxiliar = reader["AUXILIAR"].ToString();
                                de.CR_ID = Parser.ToNumber(reader["CR_ID"]);

                                allRecordsDEs.Add(de);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al cargar los segmentos DE", ex);
                    }
                }

                return allRecordsDEs;
            }

            /// <summary>
            /// Cancela segmentos DE. StoredProcedure: SP_INVALID_DE
            /// </summary>
            /// <param name="de">Segmento DE a cancelar.</param>
            public void InvalidateDEs(PM_DE de)
            {
                if (de.IsValid)
                {
                    throw new Exception("No se puede cancelar un segmento DE válido");
                }

                try
                {
                    string query = "DATOSEXTERNOS.SP_INVALID_DE";

                    //MASS. 04octubre2021 pra pruebas en desarrollo
                    if (WebConfig.MailFrom.StartsWith("desarrollo"))
                    {
                        query = "DATOSEXTERNOS_Z.SP_INVALID_DE";
                    }

                    DbCommand cmd = DB.GetStoredProcCommand(query);

                    DB.AddInParameter(cmd, "pId", DbType.Int32, de.Id);

                    DB.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cancelar segmentos DE", ex);
                }
            }

            private string AjustarDE_02_DiasVencido(int DiasVencidoOriginal)
            {
                // Si el numero es mayor a 999 devolvemos 999 debido a que el tamaño de la etiqueta solo admite hasta 3 Digitos.
                if (DiasVencidoOriginal > 999)
                {
                    return "999";
                }
                else
                {
                    return DiasVencidoOriginal.ToString();
                }

            }

            private string AjustarDE_03_Cantidad(double CantidadOriginal, string RFCAcreditado, string NumeroCredito, int DiasVencido)
            {
                double DatoCantidad = 0;
                double.TryParse(Math.Round(CantidadOriginal, 0).ToString("#"), out DatoCantidad);

                // 28/08/2015 Incorporamos el Ajuste de Bono Cupon Cero

                BonoCuponCeroDataAccess datosBonoCuponCero = new BonoCuponCeroDataAccess(Enums.Persona.Moral);
                List<BonoCuponCero> DatosCuponCero = null;
                DatosCuponCero = datosBonoCuponCero.GetRecords(false);

                foreach (BonoCuponCero ItemCuponCero in DatosCuponCero)
                {
                    if (RFCAcreditado == ItemCuponCero.Rfc && NumeroCredito == ItemCuponCero.Credito && DiasVencido == 0)
                    {
                        DatoCantidad = DatoCantidad - ItemCuponCero.MontoInversion;
                    }

                    // Revisar que ocurre si el Bono Cupon Cero es mayor al Saldo Vigente
                }

                return DatoCantidad.ToString();
            }

        //<MASS 31/octubre/2017 ajustes PM-V05>
            private string AjustarDE_04_Interes(double InteresOriginal)
            {
                double DatoCantidad = 0;
                double.TryParse(Math.Round(InteresOriginal, 0).ToString("#"), out DatoCantidad);

                return DatoCantidad.ToString();
            }
        //<MASS>

        #endregion

        #region Funciones del Segmento PF_TL

            /// <summary>
            /// Carga segmentos TL. StoredProcedures: SP_ObtenerCarteraPF_Mensual y Semanal
            /// </summary>
            /// <param name="periodo">Periodo del reporte. (HD_05)</param>
            /// <param name="tipoReporte">Mensual o Semanal</param>
            /// <param name="procesa">Indica si se inicia el proceso o se cargan los últimos datos procesados</param>
            /// <param name="INTF35">Fecha de reporte</param>
            /// <param name="archivoId">Id del archivo (T_ARCHIVOS)</param>
            /// <returns>Coleccion de todos los segmentos TL guardados en la BD</returns>
            public List<PF_TL> GetTLs(string periodo, Enums.Reporte tipoReporte, bool procesa, string INTF35, int archivoId, string grupos)
            {
                if (allRecordsTLs == null)
                {
                    allRecordsTLs = new List<PF_TL>();

                    try
                    {
                        //String query = string.Format("DATOSEXTERNOS.SP_ObtenerCarteraPF_{0}", tipoReporte);
                        String query = string.Format("BYPASS.SP_ObtenerCarteraPF_{0}", tipoReporte);

                        //MASS. 04octubre2021 para pruebas en desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            query = string.Format("BYPASS_Z.SP_ObtenerCarteraPF_{0}", tipoReporte);
                        }

                        DbCommand cmd = DB.GetStoredProcCommand(query);
                        DB.AddInParameter(cmd, "pPeriodo", DbType.AnsiString, periodo);
                        DB.AddInParameter(cmd, "pProcesar", DbType.AnsiString, (procesa ? "1" : "0"));
                        DB.AddInParameter(cmd, "pArchivoId", DbType.Int32, archivoId);
                        DB.AddInParameter(cmd, "pINTF35", DbType.AnsiString, INTF35);
                        //DB.AddInParameter(cmd, "pGrupos", DbType.AnsiString, grupos);

                        using (IDataReader reader = DB.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                PF_TL item = new PF_TL();

                                item.TL_Etiqueta = WebConfig.TL_Etiqueta_ES;
                                item.TL_01 = WebConfig.TL_01_ES;
                                item.TL_02 = WebConfig.TL_02_ES;
                                item.TL_04 = AjustarTL_04_NumeroCuentaCredito(Parser.ToNumber(reader["Credito"]) );
                                item.TL_05 = "I"; //WebConfig.TL_05_ES;
                                item.TL_06 = AjustarTL_06_TipoCuenta( Parser.ToNumber(reader["TIPO_CREDITO"]) );
                                item.TL_07 = AjustarTL_07_TipoContrato( Parser.ToNumber(reader["TIPO_CREDITO"]) );
                                item.TL_08 = "MX"; //WebConfig.TL_08_ES;
                                item.TL_10 = AjustarTL_10_NumeroPagos( Parser.ToNumber(reader["NUM_PAGOS"]) );
                                item.TL_11 = AjustarTL_11_FrecuenciaPagos( reader["FREC_PAGOS"].ToString() );
                                item.TL_12 = AjustarTL_12_MontoPagar( Parser.ToDouble(reader["IMPORTE_PAGO"]) );
                                item.TL_13 = AjustarTL_13_FechaAperturaCredito( Parser.ToDateTime(reader["FECHA_APERTURA"]) );
                                item.TL_14 = AjustarTL_14_FechaUltimoPago( Parser.ToDateTime(reader["FECHA_ULTIMO_PAGO"]) );
                                // Revisar por que estaba comentada la fecha de ultimo pago
                                //item.TL_14 = ""; // DTDU001 Dato Temporal Definido por el Usuario
                                item.TL_15 = AjustarTL_15_FechaUltimaCompra( Parser.ToDateTime(reader["FECHA_ULTIMA_COMPRA"]) );
                                item.TL_16 = AjustarTL_16_FechaCierre( Parser.ToDateTime(reader["FECHA_CIERRE"]) );
                                item.TL_17 = AjustarTL_17_FechaReporte( INTF35 );
                                item.TL_20 = AjustarTL_20_Garantia( item.TL_07 );
                                item.TL_21 = AjustarTL_21_CreditoMaximoAutorizado(Parser.ToDouble(reader["IMPORTE"]));
                                item.TL_22 = AjustarTL_22_SaldoActual( Parser.ToDouble(reader["SALDO_INSOLUTO"]) );
                                item.TL_24 = AjustarTL_24_SaldoVencido( Parser.ToDouble(reader["MONTO_PAGAR_VENCIDO"]) );
                                item.TL_25 = AjustarTL_25_NumeroPagosVencidos( Parser.ToNumber(reader["NUM_PAGOS_VENCIDOS"]) );
                                item.TL_26 = reader["FORMA_PAGO"].ToString();
                                item.TL_30 = AjustarTL_30_ClaveObservacion(reader["CALIFICACION"].ToString(), Parser.ToDouble(reader["MONTO_PAGAR_VENCIDO"]) );
                                item.TL_41 = AjustarTL_41_NumeroCuentaAnterior( reader["CREDITO_ANTERIOR"].ToString() );
                                item.TL_43 = AjustarTL_43_FechaPrimerIncumplimiento( Parser.ToDateTime(reader["FECHA_AMORT_VENCIDA"]) );
                                item.TL_44 = AjustarTL_44_SaldoInsoluto(Parser.ToDouble(reader["SALDO_INSOLUTO"]));
                                item.TL_45 = AjustarTL_45_MontoUltimoPago(0); // DTDU001 Dato Temporal Definido por el Usuario

                                item.TL_46 = AjustarTL_46_FechaTraspaso(Parser.ToDateTime(reader["FECHA_TRASPASO"]));
                                item.TL_47 = AjustarTL_47_Interes(Parser.ToDouble(reader["INTERES"]) );
                                item.TL_48 = reader["FORMA_PAGO"].ToString(); 
                                item.TL_49 = reader["DIAS_VENCIDOS"].ToString(); 

                                item.TL_50 = AjustarTL_50_PlazoMeses(Parser.ToDateTime(reader["FECHA_APERTURA"]), Parser.ToDateTime(reader["FECHA_VENCIMIENTO"]));
                                item.TL_51 = AjustarTL_51_MontoOriginacion(Parser.ToDouble(reader["SALDO_INICIAL"]));
                                //TODO: SOL53051 => Campos telefono y mail obligatorios
                                item.TL_52 = AjustarTL_52_Correo(reader["EMAIL"].ToString());
                                item.TL_99 = "FIN"; //WebConfig.TL_99_ES;


                                item.Id = Parser.ToNumber(reader["ID"]);
                                item.PN_ID = Parser.ToNumber(reader["PN_ID"]);
                                item.ArchivoId = Parser.ToNumber(reader["ARCHIVO_ID"]);
                                item.Auxiliar = reader["AUXILIAR"].ToString();
                                item.Rfc = reader["RFC"].ToString();
                                item.MontoPagar = Parser.ToDouble(reader["MONTO_PAGAR"]);                           
                                
                                allRecordsTLs.Add(item);
                            }
                        }

                        // Cargar los Montos!
                        // CargaMontos(allRecordsTLs); //JAGH se comenta pues valores los trae de consulta 02/04/13
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error procesando créditos de PF", ex);
                    }
                }

                return allRecordsTLs;
            }

        //Tiene 0 referencias, por lo que se infiere no se ocupa
            public List<PF_TL> GetTLs_2011(string periodo, Enums.Reporte tipoReporte, bool procesa, string INTF35, int archivoId, string grupos)
            {
                if (allRecordsTLs == null)
                {
                    allRecordsTLs = new List<PF_TL>();

                    try
                    {
                        String query = string.Format("BYPASS.SP_ObtenerPF_{0}_2011", tipoReporte);

                        //MASS. 04octubre2021 para pruebas en desarrollo
                        if (WebConfig.MailFrom.StartsWith("desarrollo"))
                        {
                            query = string.Format("BYPASS_Z.SP_ObtenerPF_{0}_2011", tipoReporte);
                        }


                        DbCommand cmd = DB.GetStoredProcCommand(query);
                        DB.AddInParameter(cmd, "pPeriodo", DbType.AnsiString, periodo);
                        DB.AddInParameter(cmd, "pProcesar", DbType.AnsiString, (procesa ? "1" : "0"));
                        DB.AddInParameter(cmd, "pArchivoId", DbType.Int32, archivoId);
                        DB.AddInParameter(cmd, "pINTF35", DbType.AnsiString, INTF35);
                        DB.AddInParameter(cmd, "pGrupos", DbType.AnsiString, grupos);

                        using (IDataReader reader = DB.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                PF_TL item = new PF_TL();

                                item.TL_Etiqueta = "TL";
                                item.TL_01 = "BB14210001";
                                item.TL_02 = "BANOBRAS";
                                item.TL_04 = AjustarTL_04_NumeroCuentaCredito(Parser.ToNumber(reader["Credito"]));
                                item.TL_05 = "I";
                                item.TL_06 = AjustarTL_06_TipoCuenta(Parser.ToNumber(reader["TIPO_CREDITO"]));
                                item.TL_07 = AjustarTL_07_TipoContrato(Parser.ToNumber(reader["TIPO_CREDITO"]));
                                item.TL_08 = "MX";
                                item.TL_10 = AjustarTL_10_NumeroPagos(Parser.ToNumber(reader["NUM_PAGOS"]));
                                item.TL_11 = AjustarTL_11_FrecuenciaPagos(reader["FREC_PAGOS"].ToString());
                                item.TL_12 = AjustarTL_12_MontoPagar(Parser.ToDouble(reader["IMPORTE_PAGO"]));
                                item.TL_13 = AjustarTL_13_FechaAperturaCredito(Parser.ToDateTime(reader["FECHA_APERTURA"]));
                                item.TL_14 = AjustarTL_14_FechaUltimoPago(Parser.ToDateTime(reader["FECHA_ULTIMO_PAGO"]));
                                // Revisar por que estaba comentada la fecha de ultimo pago
                                //item.TL_14 = ""; // DTDU001 Dato Temporal Definido por el Usuario
                                item.TL_15 = AjustarTL_15_FechaUltimaCompra(Parser.ToDateTime(reader["FECHA_ULTIMA_COMPRA"]));
                                item.TL_16 = AjustarTL_16_FechaCierre(Parser.ToDateTime(reader["FECHA_CIERRE"]));
                                item.TL_17 = AjustarTL_17_FechaReporte(INTF35);
                                item.TL_20 = AjustarTL_20_Garantia(item.TL_07);
                                item.TL_21 = AjustarTL_21_CreditoMaximoAutorizado(Parser.ToDouble(reader["IMPORTE"]));
                                item.TL_22 = AjustarTL_22_SaldoActual(Parser.ToDouble(reader["SALDO_INSOLUTO"]));
                                item.TL_24 = AjustarTL_24_SaldoVencido(Parser.ToDouble(reader["MONTO_PAGAR_VENCIDO"]));
                                item.TL_25 = AjustarTL_25_NumeroPagosVencidos(Parser.ToNumber(reader["NUM_PAGOS_VENCIDOS"]));
                                item.TL_26 = reader["FORMA_PAGO"].ToString();
                                item.TL_30 = AjustarTL_30_ClaveObservacion(reader["CALIFICACION"].ToString(), Parser.ToDouble(reader["MONTO_PAGAR_VENCIDO"]) );
                                item.TL_41 = AjustarTL_41_NumeroCuentaAnterior(reader["CREDITO_ANTERIOR"].ToString());
                                item.TL_43 = AjustarTL_43_FechaPrimerIncumplimiento(Parser.ToDateTime(reader["FECHA_AMORT_VENCIDA"]));
                                item.TL_44 = AjustarTL_44_SaldoInsoluto(Parser.ToDouble(reader["SALDO_INSOLUTO"]));
                                item.TL_45 = AjustarTL_45_MontoUltimoPago(0); // DTDU001 Dato Temporal Definido por el Usuario
                                item.TL_50 = AjustarTL_50_PlazoMeses(Parser.ToDateTime(reader["FECHA_APERTURA"]), Parser.ToDateTime(reader["FECHA_VENCIMIENTO"]));
                                item.TL_51 = AjustarTL_51_MontoOriginacion(Parser.ToDouble(reader["SALDO_INICIAL"]));
                                //TODO: SOL53051 => Campos telefono y mail obligatorios
                                item.TL_52 = AjustarTL_52_Correo(reader["EMAIL"].ToString());
                                item.TL_99 = "FIN";

                                item.Id = Parser.ToNumber(reader["ID"]);
                                item.PN_ID = Parser.ToNumber(reader["PN_ID"]);
                                item.ArchivoId = Parser.ToNumber(reader["ARCHIVO_ID"]);
                                item.Auxiliar = reader["AUXILIAR"].ToString();
                                item.Rfc = reader["RFC"].ToString();
                                item.MontoPagar = Parser.ToDouble(reader["MONTO_PAGAR"]);

                                allRecordsTLs.Add(item);
                            }
                        }

                        // Cargar los Montos!
                        CargaMontos(allRecordsTLs);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error procesando créditos de PF", ex);
                    }
                }

                return allRecordsTLs;
            }

            private void CargaMontos(List<PF_TL> tls)
            {

                List<PF_TL_Monto> montos = new List<PF_TL_Monto>();
                DbCommand cmd = DB.GetSqlStringCommand("select tl_id,cantidad,tipo, grupo from t_pf_tl_montos");

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        PF_TL_Monto monto = new PF_TL_Monto();

                        monto.TL_ID = Parser.ToNumber(reader["TL_ID"]);
                        monto.Cantidad = Parser.ToDouble(reader["CANTIDAD"]);
                        monto.Tipo = reader["TIPO"].ToString();
                        monto.Grupo = Parser.ToNumber(reader["GRUPO"]);

                        montos.Add(monto);
                    }
                }

                if (montos.Count > 0)
                {
                    foreach (PF_TL tl in tls)
                    {
                        var monto = ((from m in montos
                                      where m.TL_ID == tl.Id && m.Tipo == "PAGAR"
                                      select m)).DefaultIfEmpty();

                        var venc = (from m in montos
                                    where m.TL_ID == tl.Id && m.Tipo == "VENCIDO"
                                    select m).DefaultIfEmpty();

                        tl.Grupo = 0;
                        foreach (PF_TL_Monto montotmp in monto)
                        {
                            if (montotmp != default(PF_TL_Monto))
                            {
                                tl.MontoPagar += montotmp.Cantidad;
                                tl.Grupo = montotmp.Grupo;
                            }
                        }

                        foreach (PF_TL_Monto venctmp in venc)
                        {
                            if (venctmp != default(PF_TL_Monto))
                            {
                                Double SaldoVencido = Parser.ToDouble(tl.TL_24);

                                //tl.MontoPagarVencido += venctmp.Cantidad;
                                SaldoVencido += venctmp.Cantidad;
                                if (tl.Grupo <= 0)
                                {
                                    tl.Grupo = venctmp.Grupo;
                                }
                            }
                        }

                    }
                }
            }

            private string AjustarTL_04_NumeroCuentaCredito(int NumeroCreditoOriginal)
            {
                return NumeroCreditoOriginal.ToString(); ;
            }

            private string AjustarTL_06_TipoCuenta(int TipoCuentaOriginal)
            {
                return (TipoCuentaOriginal == 1310) ? "M" : "I";
            }

            private string AjustarTL_07_TipoContrato(int TipoCuentaOriginal)
            {
                return (TipoCuentaOriginal == 1310) ? "RE" : "PL";
            }

            private string AjustarTL_10_NumeroPagos(int NumeroPagosOrginal)
            {
                return NumeroPagosOrginal.ToString();
            }

            private string AjustarTL_11_FrecuenciaPagos(string FrecuenciaPagosOriginal)
            {
                return FrecuenciaPagosOriginal;
            }

            private string AjustarTL_12_MontoPagar(double MontoPagarOriginal)
            {
                return Math.Round(MontoPagarOriginal, 0).ToString();
            }

            private string AjustarTL_13_FechaAperturaCredito(DateTime FechaAperturaOriginal)
            {
                return (FechaAperturaOriginal == default(DateTime)) ? "" : FechaAperturaOriginal.ToString(Parser.FORMATO_FECHA);
            }

            private string AjustarTL_14_FechaUltimoPago(DateTime FechaUltimoPagoOriginal)
            {
                return (FechaUltimoPagoOriginal == default(DateTime)) ? "" : FechaUltimoPagoOriginal.ToString(Parser.FORMATO_FECHA);
            }

            private string AjustarTL_15_FechaUltimaCompra(DateTime FechaUltimaCompraOriginal)
            {
                return (FechaUltimaCompraOriginal == default(DateTime)) ? "" : FechaUltimaCompraOriginal.ToString(Parser.FORMATO_FECHA);
            }

            private string AjustarTL_16_FechaCierre(DateTime FechaCierreOriginal)
            {
                return (FechaCierreOriginal == default(DateTime)) ? "" : FechaCierreOriginal.ToString(Parser.FORMATO_FECHA);
            }

            private string AjustarTL_17_FechaReporte(string FechaReporteOriginal)
            {
                return FechaReporteOriginal;
            }

            private string AjustarTL_20_Garantia(string GarantiaOriginal)
            {
                if (GarantiaOriginal == "RE")
                {
                   return WebConfig.TipoCreditoRE;
                }
                else
                {
                    return WebConfig.TipoCreditoPL;
                }
            }

            private string AjustarTL_21_CreditoMaximoAutorizado(double CreditoAutorizadoOriginal)
            {
                double TL_21 = 0;
                double.TryParse(Math.Round(CreditoAutorizadoOriginal, 0).ToString("##########"), out TL_21);                                  
                return TL_21.ToString();               
            }

            private string AjustarTL_22_SaldoActual(Double SaldoActualOriginal)
            {
                double TL_22 = 0;
                double.TryParse(Math.Round(SaldoActualOriginal, 0).ToString("##########"), out TL_22);
                return TL_22.ToString();
            }

            private string AjustarTL_24_SaldoVencido(Double SaldoVencidoOriginal)
            {
                return Math.Round(SaldoVencidoOriginal, 0).ToString();
            }

            private string AjustarTL_25_NumeroPagosVencidos(int PagosVencidosOriginal)
            {
                return PagosVencidosOriginal.ToString();
            }

            private string AjustarTL_30_ClaveObservacion(string ClaveObservacion, double SaldoVencido )
            {
                // Solicitud SMS de Fecha 02052016
                // Adecuación que permita reportar los creditos de las bases de Personas Fisicas y Personas Morales  (Mensuales y Semanales)
                // con las clave de observacion SG = Demanda por el otorgante (registrada en el catalogo de creditos con clave de onservacion), 
                // únicamente cuando presenten saldo vencido, en caso de no reportar saldo vencido que sean reportados sin dicha clave.

                if (ClaveObservacion.Trim() == "SG" && SaldoVencido == 0)
                    return "";
                else
                    return ClaveObservacion;

                return ClaveObservacion;
            }

            private string AjustarTL_41_NumeroCuentaAnterior(string CuentaAnteriorOriginal)
            {
                         
                // JAGH 01/04/13 modificado a solicitud de usuario se cierra espacio en txt si valor = 0
                if (CuentaAnteriorOriginal.Length == 0)
                {
                    return string.Empty;
                }

                return CuentaAnteriorOriginal.ToString();
            }

            private string AjustarTL_43_FechaPrimerIncumplimiento(DateTime FechaIncumplimientoOriginal)
            {

                if ((FechaIncumplimientoOriginal == null))
                {
                    return "";
                }
                else if ((FechaIncumplimientoOriginal.ToString("dd/MM/yyyy") == "01/01/0001"))
                {
                    return "01011900";
                }
                else if ((FechaIncumplimientoOriginal.ToString("dd/MM/yyyy") == "01/01/1900"))
                {
                    return "01011900";
                }
                else
                {
                    return FechaIncumplimientoOriginal.ToString("ddMMyyyy");
                }               
            }

            private string AjustarTL_44_SaldoInsoluto(Double SaldoInsolutoOriginal)
            {
                double TL_44 = 0;
                double.TryParse(Math.Round(SaldoInsolutoOriginal, 0).ToString("##########"), out TL_44);

                return TL_44.ToString();
            }

            private string AjustarTL_45_MontoUltimoPago(Double UltimoPagoOriginal)
            {
                double TL_44 = 0;
                double.TryParse(Math.Round(UltimoPagoOriginal, 0).ToString("##########"), out TL_44);

                return TL_44.ToString();
            }

            private string AjustarTL_46_FechaTraspaso(DateTime FechaTraspaso)
            {

                if ((FechaTraspaso == null))
                {
                    return "";
                }
                else if ((FechaTraspaso.ToString("dd/MM/yyyy") == "01/01/0001"))
                {
                    return "01011900";
                }
                else if ((FechaTraspaso.ToString("dd/MM/yyyy") == "01/01/1900"))
                {
                    return "01011900";
                }
                else
                {
                    return FechaTraspaso.ToString("ddMMyyyy");
                }
            }

            private string AjustarTL_47_Interes(Double interes)
            {
                double TL_47 = 0;
                double.TryParse(Math.Round(interes, 0).ToString("##########"), out TL_47);

                return TL_47.ToString();
            }

            private string AjustarTL_50_PlazoMeses(DateTime FechaAperturaOriginal, DateTime FechaVencimientoOriginal)
            {
                double TotalDias = (FechaVencimientoOriginal - FechaAperturaOriginal).TotalDays;
                double PlazoMeses = TotalDias / 30.4;//30.4 ORIGINAL
                PlazoMeses = Math.Round(PlazoMeses, 2);
                String PlazoM = PlazoMeses.ToString("###0.00");

                return PlazoM;
                //return "10.0";
            }

            private string AjustarTL_51_MontoOriginacion(Double SaldoInicialOriginal)
            {
                double TL_51 = 0;
                double.TryParse(Math.Round(SaldoInicialOriginal, 0).ToString("##########"), out TL_51);
                return TL_51.ToString();
            }

            private string AjustarTL_52_Correo(string correoOriginal)
            {
                return correoOriginal;
            }

        #endregion

    }

}
