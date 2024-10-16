using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Vistas;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Business.Repositorios;

namespace Banobras.Credito.SICREB.Business.Rules.PM
{

    public class PM_Cinta_Rules
    {

        private PM_HD_Rules hdRules;
        private PM_EM_Rules emRules;
        private PM_AC_Rules acRules;
        private PM_CR_Rules crRules;
        private PM_AV_Rules avRules;
        private PM_TS_Rules tsRules;

        public PM_Cinta_Rules()
        {
            hdRules = new PM_HD_Rules();
            emRules = new PM_EM_Rules();
            acRules = new PM_AC_Rules();
            crRules = new PM_CR_Rules();
            avRules = new PM_AV_Rules();
            tsRules = new PM_TS_Rules();
        }

        #region Metodos Publicos

            /// <summary>
            /// Metodo que genera el archivo de Persona Moral.
            /// </summary>
            /// <param name="tipoReporte">Define el reporte Mensual o Semanal</param>
            /// <returns>Objeto de segmentos</returns>
            public PM_Cinta GeneraArchivo(Enums.Reporte tipoReporte, string grupos)
            {

                // Genera el registro del archivo TXT
                Archivo arch = new Archivo(0, string.Empty, Enums.Persona.Moral, DateTime.Now, null);
                ArchivosDataAccess archivosAccess = new ArchivosDataAccess(Enums.Persona.Moral);

                int archivoId = archivosAccess.AgregaArchivoInicial(arch);
                arch.Id = archivoId;

                // Genera la Cinta de las vistas
                PM_Cinta cinta = CrearCinta(tipoReporte, archivoId, grupos);

                // Validamos la informacion de la Cinta
                Validator.Validator val = new Validator.Validator(cinta);
                val.Valida();
                Valida_Cinta_PM_Hijos(cinta);

                // Calculamos los totales finales una vez validada cinta
                tsRules.LoadTS(cinta); 

                // Actualizamos los datos del archivo TXT
                arch.Nombre = string.Format("PM_{0}_{1}.txt", cinta.HD.HD_05, tipoReporte);
                arch.ContenidoArchivo = new StringBuilder(cinta.ToString());
                arch.SetEstadisticas(cinta.Correctos, cinta.NumErrores, cinta.NumWarnings);
                archivosAccess.ActualizaArchivo(arch);

                // Actualizamos los segmentos inválidos
                GuardarValores(cinta);

                return cinta;
            }

            public void Valida_Cinta_PM_Hijos(PM_Cinta cinta)
            {

                // Validamos que los Creditos tengan al menos un Detalle valido
                int DE_Valido = 0;

                foreach (PM_EM em in cinta.EMs)
                {
                    if (em.IsValid)
                    {
                        foreach (PM_CR cr in em.CRs)
                        {
                            if (cr.IsValid)
                            {
                                foreach (PM_DE de in cr.DEs)
                                {
                                    if (de.IsValid)
                                        DE_Valido++;
                                }

                                if (DE_Valido == 0)
                                    cr.IsValid = false;

                                DE_Valido = 0;
                            }
                        }
                    }
                }

                // Validamos que los Acreditados tengan al menos un Credito Valido, si el credito no es valido
                // por consecuencia debemos invalidar los detalles de dicho credito
                int CR_Valido = 0;

                foreach (PM_EM em in cinta.EMs)
                {
                    if (em.IsValid)
                    {
                        foreach (PM_CR cr in em.CRs)
                        {
                            if (cr.IsValid)
                                CR_Valido++;
                            else
                            {
                                //se invalidan DEs del credito 
                                foreach (PM_DE de in cr.DEs)
                                {
                                    de.IsValid = false;
                                }
                            }
                        }

                        if (CR_Valido == 0)
                            em.IsValid = false;

                        CR_Valido = 0;
                    }
                }
            }

        #endregion

        #region Metodos Privados
        
            private PM_Cinta CrearCinta(Enums.Reporte tipoReporte, int archivoId, string grupos)
            {

                PM_Cinta cinta = LoadPMCinta();
                cinta.ArchivoId = archivoId;
                hdRules.LoadHD(cinta, (tipoReporte == Enums.Reporte.Mensual));
                emRules.LoadEMs(cinta, tipoReporte, true, grupos);

                CuentasDataAccess cuentas = new CuentasDataAccess(Enums.Persona.Moral);
                string periodo = Parser.ToDateTime(cinta.HD.HD_05, Parser.FORMATO_FECHA).ToString("yyyyMM");
                int cuenta6378Activa = cuentas.CountCuenta6378();
                CarteraDataAccess carteraData = new CarteraDataAccess(Enums.Persona.Moral);

                // Segunda Linea Tardada, Se optimiza código por Mariana
                List<PM_CR> creditos = carteraData.GetPM_CRs(periodo, tipoReporte, true, cuenta6378Activa, archivoId, grupos);
                List<PM_DE> creditosDetalle = carteraData.GetCRDetails(true, archivoId, grupos);
            
                foreach (PM_EM em in cinta.EMs)
                {
                    try
                    {
                        acRules.LoadACs(em); 
                        crRules.LoadCRs(em, creditos);
                        foreach (PM_CR cr in em.CRs)
                        {

                            var des = (from d in creditosDetalle
                                       where d.Auxiliar.Equals(cr.Auxiliar) orderby d.DE_02
                                       select d).ToList();                        

                            foreach (PM_DE de in des)
                            {
                                de.Parent = cr;
                                cr.DEs.Add(de);
                            }

                            if (cr.DEs.Count == 0)                        
                                cr.IsValid = false;

                            avRules.LoadAVs(cr);

                            DateTime dt_fecha_maxima_refi = new DateTime();
                            DateTime.TryParse(cr.Fecha_Maxima_Refinanciamiento.ToString(), out dt_fecha_maxima_refi);                       

                        } // Fin del ciclo PM_CR

                        // La direccion del Acreditado debe ser de maximo 80 caracteres si es mayor eliminamos los caracteres sobrentes
                        // Si la Direccion es mayor a 40 caracteres la divimos en 2 partes y las guardamos en las etiquetas EM_13 y EM_14
                        
                        // YA NO SE REQUIERE SE HACE EN EL DATA ACCESS
                        //if (em.Direccion.Length >= 80)
                        //{
                        //    string subStr_direccion = em.Direccion.Substring(0, 80);
                        //    if (subStr_direccion.Length > 40)
                        //    {
                        //        em.EM_13 = subStr_direccion.Substring(0, 40);
                        //        em.EM_14 = subStr_direccion.Substring(41);
                        //    }
                        //}


                        Banobras.Credito.SICREB.Data.V_SICALCDataAccess seg = new Banobras.Credito.SICREB.Data.V_SICALCDataAccess();
                        em.EM_09 = seg.GetCalificacionAltoRiesgoAccess(em.EM_00);                     
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
           
                } // Fin del ciclo PM_EM 
            
                return cinta;
            }

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

            private void GuardarValores(PM_Cinta cinta)
            {

                List<PM_EM> invalidEMs = new List<PM_EM>();
                List<PM_CR> invalidCRs = new List<PM_CR>();
                List<PM_DE> invalidDEs = new List<PM_DE>();

                foreach (PM_EM em in cinta.EMs)
                {
                    if (!em.IsValid)
                    {
                        invalidEMs.Add(em);
                    }

                    foreach (PM_CR cr in em.CRs)
                    {
                        if (!cr.IsValid)
                        {
                            invalidCRs.Add(cr);
                        }

                        foreach (PM_DE de in cr.DEs)
                        {
                            if (!de.IsValid)
                            {
                                invalidDEs.Add(de);
                            }
                        }
                    }
                }

                emRules.GuardaValores(invalidEMs);
                crRules.GuardarValores(invalidCRs);
                crRules.GuardarValores(invalidDEs);
            }

        #endregion

        #region VALIDAR SI SE OCUPAN

        public PM_Cinta GeneraArchivo(Enums.Reporte tipoReporte, int archivoId, string grupos)
        {

            // Genera el registro del archivo TXT
            Archivo arch = new Archivo(0, string.Empty, Enums.Persona.Moral, DateTime.Now, null);
            ArchivosDataAccess archivosAccess = new ArchivosDataAccess(Enums.Persona.Moral);
            arch.Id = archivoId;

            // Genera la cinta de las vistas
            PM_Cinta cinta = CrearCinta(tipoReporte, archivoId, grupos);

            // Validamos la informacion de la Cinta
            Validator.Validator val = new Validator.Validator(cinta);
            val.Valida();
            Valida_Cinta_PM_Hijos(cinta);

            //*********
            // REVISAR POR QUE EN ESTE METODO NO SE CALCULAN TOTALES COMO EN EL OTRO.
            //*********

            // Actualizamos los datos del archivo TXT
            arch.Nombre = string.Format("PM_{0}_{1}.txt", cinta.HD.HD_05, tipoReporte);
            arch.ContenidoArchivo = new StringBuilder(cinta.ToString());
            arch.SetEstadisticas(cinta.Correctos, cinta.NumErrores, cinta.NumWarnings);
            archivosAccess.ActualizaArchivo(arch);

            // Actualizamos los segmentos inválidos
            GuardarValores(cinta);

            return cinta;
        }


        // Se utilizaba para el reporte de EXCEL de registros procesados en el archivo PM
        // Sin embargo consultaba nuevamente la informacion de la base de datos,
        // se cambio el proceso para que se tome la informacion directamente del archivo PM generado.
        // por lo tanto si este metodo no se ocupa en otro lado, queda obsoleto.
        // Pendiente validar si se ocupa en algun otro lado, de lo contrario eliminar
        public PM_Cinta LoadCintaBD(int archivoId)
        {
            PM_Cinta cinta = LoadPMCinta();
            cinta.ArchivoId = archivoId;
            emRules.LoadEMs(cinta, Enums.Reporte.Mensual, false, "");

            CarteraDataAccess carteraData = new CarteraDataAccess(Enums.Persona.Moral);
            List<PM_CR> creditos = carteraData.GetPM_CRs("", Enums.Reporte.Mensual, false, 0, archivoId, "");
            List<PM_DE> creditosDetalle = carteraData.GetCRDetails(false, archivoId, "");

            foreach (PM_EM em in cinta.EMs)
            {
                //acRules.LoadACs(em); // APA Agregar
                crRules.LoadCRs(em, creditos);

                foreach (PM_CR cr in em.CRs)
                {
                    var des = (from d in creditosDetalle
                               where d.Auxiliar.Equals(cr.Auxiliar)
                               orderby d.DE_02
                               select d).ToList();

                    foreach (PM_DE de in des)
                    {
                        de.Parent = cr;
                        cr.DEs.Add(de);
                    }

                    //avRules.LoadAVs(cr);
                }

                // YA NO SE REQUIERE SE HACE EN EL DATA ACCESS
                //string subStr_direccion = "";
                //if (em.Direccion.Length >= 80)
                //{

                //    subStr_direccion = em.Direccion.Substring(0, 80);
                //    if (subStr_direccion.Length > 40)
                //    {
                //        em.EM_13 = subStr_direccion.Substring(0, 40);
                //        em.EM_14 = subStr_direccion.Substring(41);
                //    }
                //    else
                //    {
                //        //// Direccion > 40
                //    }
                //}// Direccion > 80

                Banobras.Credito.SICREB.Data.V_SICALCDataAccess seg = new Banobras.Credito.SICREB.Data.V_SICALCDataAccess();
                em.EM_09 = seg.GetCalificacionAltoRiesgoAccess(em.EM_00);
            }

            for (int i = 0; i < cinta.EMs.Count; i++)
            {
                if (cinta.EMs[i].CRs.Count == 0)
                {
                    cinta.EMs.RemoveAt(i);
                    i--;
                }
            }

            return cinta;
        }

        /// <summary>
        /// Carga los segmentos de la base de datos.
        /// </summary>
        /// <param name="archivoId">Indica el ID del archivo que se desea cargar.</param>
        /// <returns>Objeto de los segmentos</returns>
        public PM_Cinta LoadCintaBD_deprecated(int archivoId)
        {
            PM_Cinta cinta = LoadPMCinta();
            cinta.ArchivoId = archivoId;
            emRules.LoadEMs(cinta, Enums.Reporte.Mensual, false, "");

            CarteraDataAccess carteraData = new CarteraDataAccess(Enums.Persona.Moral);
            List<PM_CR> creditos = carteraData.GetPM_CRs("", Enums.Reporte.Mensual, false, 0, archivoId, "");

            foreach (PM_EM em in cinta.EMs)
            {
                //acRules.LoadACs(em);    //APA Agregar
                crRules.LoadCRs(em, creditos);

                foreach (PM_CR cr in em.CRs)
                {
                    var des = (from d in carteraData.GetCRDetails(false, archivoId, "")
                               where d.Auxiliar.Equals(cr.Auxiliar)
                               select d).ToList();

                    PM_DE detalleVigente = null;
                    PM_DE detalleVencido = null;

                    if (cr.CR_02.Equals("1489") || cr.CR_02.Equals("1830"))
                        Console.WriteLine("aqui");

                    foreach (PM_DE de in des)
                    {
                        if (Parser.ToNumber(de.DE_02) > 0)
                        {
                            if (detalleVencido == null)
                            {
                                detalleVencido = de;
                                detalleVencido.Parent = cr;
                            }
                            else
                            {
                                // detalleVencido.Cantidad += de.Cantidad; // ORIGINAL
                                detalleVencido.DE_02 = Convert.ToString(Parser.ToNumber(detalleVencido.DE_02) + Parser.ToNumber(de.DE_02));
                            }                                
                        }
                        else
                        {
                            if (detalleVigente == null)
                            {
                                detalleVigente = de;
                                detalleVigente.Parent = cr;
                            }
                            else
                            {
                                // detalleVigente.Cantidad += de.Cantidad; // ORIGINAL
                                detalleVigente.DE_02 = Convert.ToString(Parser.ToNumber(detalleVigente.DE_02) + Parser.ToNumber(de.DE_02));
                            }
                                
                        }
                    }

                    if (detalleVigente != null)
                    {
                        if (Parser.ToNumber(detalleVigente.DE_03) != 0)
                        {
                            cr.DEs.Add(detalleVigente);
                        }
                        else
                        {
                            if (detalleVigente.TypedParent.CR_15 != "00000000")
                            {
                                cr.DEs.Add(detalleVigente);
                            }
                        }
                    }
                    if (detalleVencido != null)
                    {
                        if (Parser.ToNumber(detalleVencido.DE_03) != 0)
                        {
                            cr.DEs.Add(detalleVencido);
                        }
                        else
                        {
                            if (detalleVencido.TypedParent.CR_15 != "00000000")
                            {
                                cr.DEs.Add(detalleVencido);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < cinta.EMs.Count; i++)
            {
                if (cinta.EMs[i].CRs.Count == 0)
                {
                    cinta.EMs.RemoveAt(i);
                    i--;
                }
            }

            return cinta;
        }

        //private void EliminaInvalidos(PM_Cinta cinta)
        //{
        //    foreach (PM_EM em in cinta.EMs)
        //    {

        //    }
        //}

        private DateTime get_menorFechas(PM_CR cr)
        {

            if ((cr.Fecha_Maxima_Refinanciamiento < cr.Fecha_Final_Gracia_Intereses) && (cr.Fecha_Maxima_Refinanciamiento < cr.Fecha_Final_Gracia_Capital))
            {
                // cr.Fecha_Maxima_Refinanciamiento 
                return cr.Fecha_Maxima_Refinanciamiento;
            }
            else if ((cr.Fecha_Final_Gracia_Capital < cr.Fecha_Maxima_Refinanciamiento) && (cr.Fecha_Final_Gracia_Capital < cr.Fecha_Final_Gracia_Intereses))
            {
                // Fecha_Final_Gracia_Capital
                return cr.Fecha_Final_Gracia_Capital;
            }
            else if ((cr.Fecha_Final_Gracia_Intereses < cr.Fecha_Maxima_Refinanciamiento) && (cr.Fecha_Final_Gracia_Intereses < cr.Fecha_Final_Gracia_Capital))
            {
                // cr.Fecha_Final_Gracia_Intereses
                return cr.Fecha_Final_Gracia_Intereses;
            }
            else
            {
                return new DateTime();
            }

        }

        #endregion

    }

}
