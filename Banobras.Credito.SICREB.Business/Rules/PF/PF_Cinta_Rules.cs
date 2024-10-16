using Banobras.Credito.SICREB.Business.Repositorios;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Data.Vistas;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Entities.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Business.Rules.PF
{
    public class PF_Cinta_Rules
    {
        private PF_INTF_Rules intfRules = null;
        private PF_PN_Rules pnRules = null;
        private PF_TL_Rules tlRules = null;
        private PF_TR_Rules trRules = null;

        public PF_Cinta_Rules()
        {
            intfRules = new PF_INTF_Rules();
            pnRules = new PF_PN_Rules();
            tlRules = new PF_TL_Rules();
            trRules = new PF_TR_Rules();
        }

        public PF_Cinta GeneraArchivo(Enums.Reporte tipoReporte, string grupos)
        {
            PF_Cinta cinta = new PF_Cinta();
            try {
                Archivo arch = new Archivo(0, string.Empty, Enums.Persona.Fisica, DateTime.Now, null);

                ArchivosDataAccess archivosAccess = new ArchivosDataAccess(Enums.Persona.Fisica);
                int archivoId = archivosAccess.AgregaArchivoInicial(arch);
                arch.Id = archivoId;

                //PF_Cinta 
                cinta = CrearCinta(tipoReporte, archivoId, grupos);
                Validator.Validator val = new Validator.Validator(cinta);
                val.Valida();
                trRules.LoadTRs(cinta);

                cinta = ValidaCintaPF(cinta);

                arch.Nombre = string.Format("PF_{0}_{1}.txt", cinta.INTF.INTF_35, tipoReporte);
                arch.ContenidoArchivo = new StringBuilder(cinta.ToString());
                arch.SetEstadisticas(cinta.Correctos, cinta.NumErrores, cinta.NumWarnings);

                archivosAccess.ActualizaArchivo(arch);
                GuardarValores(cinta);

                cinta.ArchivoId = archivoId;
            }catch(Exception ex) {
                Console.WriteLine("Error: " + ex.Message);  
            }            
            return cinta;
        }

        //Tiene 0 referencias, por lo que se infiere no se ocupa
        //SICREB-INICIO-VHCC OCT-2012
        /// <summary>
        /// Metodo que genera el archivo de Persona Fisica.
        /// </summary>
        /// <param name="tipoReporte">Define el reporte Mensual o Semanal</param>
        /// <returns>Objeto de segmentos</returns>
        public PF_Cinta GeneraArchivo(Enums.Reporte tipoReporte, int archivoId, string grupos)
        {
            Archivo arch = new Archivo(0, string.Empty, Enums.Persona.Fisica, DateTime.Now, null);

            ArchivosDataAccess archivosAccess = new ArchivosDataAccess(Enums.Persona.Fisica);
            //int archivoId = archivosAccess.AgregaArchivoInicial(arch);
            arch.Id = archivoId;

            PF_Cinta cinta = CrearCinta(tipoReporte, archivoId, grupos);

            Validator.Validator val = new Validator.Validator(cinta);
            val.Valida();
            trRules.LoadTRs(cinta);
            cinta = ValidaCintaPF(cinta);
            arch.Nombre = string.Format("PF_{0}_{1}.txt", cinta.INTF.INTF_35, tipoReporte);
            arch.ContenidoArchivo = new StringBuilder(cinta.ToString());
            arch.SetEstadisticas(cinta.Correctos, cinta.NumErrores, cinta.NumWarnings);

            archivosAccess.ActualizaArchivo(arch);

            GuardarValores(cinta);

            cinta.ArchivoId = archivoId;
            return cinta;

        }

        public PF_Cinta ValidaCintaPF(PF_Cinta cinta)
        {
           foreach (PF_PN pn in cinta.PNs)
           {
               foreach (PF_PA pa in pn.PAs)
               {
                   if (pa.IsValid == false)
                       pn.IsValid = false;
               }
               foreach (PF_TL tl in pn.TLs)
               {
                   if (tl.IsValid == false)
                       pn.IsValid = false;
               }
           }
           foreach (PF_PN pn in cinta.PNs)
           {
               if (pn.IsValid == false)
               {
                   foreach (PF_PA pa in pn.PAs)                   
                       pa.IsValid = false;

                   foreach (PF_TL tl in pn.TLs)
                       tl.IsValid = false;                                              
               }
           }

           return cinta;
        }
        //SICREB-FIN-VHCC OCT-2012

        //private PF_Cinta CrearCinta(Enums.Reporte tipoReporte, int archivoId, string grupos)
        //{
        //    PF_Cinta cinta = LoadPFCinta();
        //    cinta.ArchivoId = archivoId;

        //    intfRules.LoadINTF(cinta, (tipoReporte == Enums.Reporte.Mensual));
        //    pnRules.LoadPNs(cinta, tipoReporte, true, grupos);

        //    string periodo = Parser.ToDateTime(cinta.INTF.INTF_35).ToString("yyyyMM");

        //    CarteraDataAccess carteraData = new CarteraDataAccess(Enums.Persona.Fisica);
        //    foreach (PF_PN pn in cinta.PNs)
        //    {
        //        var tls = (from t in carteraData.GetTLs(periodo, tipoReporte, true, cinta.INTF.INTF_35, cinta.ArchivoId, grupos)
        //                   where t.PN_ID.Equals(pn.Id)
        //                   select t).ToList();

        //        foreach (PF_TL tl in tls)
        //            tl.Parent = pn;

        //        pn.TLs.AddRange(tls);
        //    }
        //    for (int i = 0; i < cinta.PNs.Count; i++)
        //    {
        //        if (cinta.PNs[i].TLs.Count == 0 || cinta.PNs[i].PAs.Count == 0)
        //        {
        //            cinta.PNs.RemoveAt(i);
        //            i--;
        //        }
        //    }


        //    return cinta;
        //}

        //se va solo una vez a la BD para los TLs  JAGH 01/02/13
        private PF_Cinta CrearCinta(Enums.Reporte tipoReporte, int archivoId, string grupos)
        {
            PF_Cinta cinta = LoadPFCinta();
            cinta.ArchivoId = archivoId;

            intfRules.LoadINTF(cinta, (tipoReporte == Enums.Reporte.Mensual));
            pnRules.LoadPNs(cinta, tipoReporte, true, grupos);

            string periodo = Parser.ToDateTime(cinta.INTF.INTF_35).ToString("yyyyMM");

            CarteraDataAccess carteraData = new CarteraDataAccess(Enums.Persona.Fisica);

            //JAGH se genera solo una vez el store para traer creditos 02/04/13
            List<PF_TL> ITLs = new List<PF_TL>();
            ITLs = carteraData.GetTLs(periodo, tipoReporte, true, cinta.INTF.INTF_35, cinta.ArchivoId, grupos).ToList();

            foreach (PF_PN pn in cinta.PNs)
            {
                var tls = (from t in ITLs //carteraData.GetTLs(periodo, tipoReporte, true, cinta.INTF.INTF_35, cinta.ArchivoId, grupos)
                           where t.PN_ID.Equals(pn.Id)
                           select t).ToList();

                foreach (PF_TL tl in tls)
                    tl.Parent = pn;

                pn.TLs.AddRange(tls);
            }
            for (int i = 0; i < cinta.PNs.Count; i++)
            {
                if (cinta.PNs[i].TLs.Count == 0 || cinta.PNs[i].PAs.Count == 0)
                {
                    cinta.PNs.RemoveAt(i);
                    i--;
                }
            }

            return cinta;
        }

        private PF_Cinta CrearCinta_2011(Enums.Reporte tipoReporte, int archivoId, string grupos)
        {
            PF_Cinta cinta = LoadPFCinta();
            cinta.ArchivoId = archivoId;

            intfRules.LoadINTF(cinta, (tipoReporte == Enums.Reporte.Mensual));
            pnRules.LoadPNs(cinta, tipoReporte, true, grupos);

            string periodo = Parser.ToDateTime(cinta.INTF.INTF_35).ToString("yyyyMM");

            CarteraDataAccess carteraData = new CarteraDataAccess(Enums.Persona.Fisica);
            foreach (PF_PN pn in cinta.PNs)
            {
                var tls = (from t in carteraData.GetTLs_2011(periodo, tipoReporte, true, cinta.INTF.INTF_35, cinta.ArchivoId, grupos)
                           where t.PN_ID.Equals(pn.Id)
                           select t).ToList();

                foreach (PF_TL tl in tls)
                    tl.Parent = pn;

                pn.TLs.AddRange(tls);
            }
            for (int i = 0; i < cinta.PNs.Count; i++)
            {
                if (cinta.PNs[i].TLs.Count == 0 || cinta.PNs[i].PAs.Count == 0)
                {
                    cinta.PNs.RemoveAt(i);
                    i--;
                }
            }
            return cinta;
        }


        private PF_Cinta LoadPFCinta()
        {

            SegmentosDataAccess segs = new SegmentosDataAccess();
            List<Segmento> segmentos = segs.GetRecords(true);

            EtiquetasDataAccess etis = new EtiquetasDataAccess();
            List<Etiqueta> etiquetas = etis.GetRecords(true);

            ValidacionesDataAccess vals = new ValidacionesDataAccess(Enums.Persona.Fisica);
            List<Validacion> validaciones = vals.GetRecords(true);

            return new PF_Cinta(segmentos, etiquetas, validaciones);
        }

        //cambiar a private ya que este lista la data access de las vistas (que puedas cargar desde la base de datos)
        public void GuardarValores(PF_Cinta cinta)
        {

            List<PF_PN> invalidPNs = new List<PF_PN>();
            List<PF_PA> invalidPAs = new List<PF_PA>();
            List<PF_TL> invalidTLs = new List<PF_TL>();

            foreach (PF_PN pn in cinta.PNs)
            {
                if (!pn.IsValid)
                {
                    invalidPNs.Add(pn);
                }
                foreach (PF_PA pa in pn.PAs)
                {
                    if (!pa.IsValid)
                    {
                        invalidPAs.Add(pa);
                    }
                }
                foreach (PF_TL tl in pn.TLs)
                {
                    if (!tl.IsValid)
                    {
                        invalidTLs.Add(tl);
                    }
                }
            }


            foreach (PF_PN pn in cinta.PNs)
            {
                foreach (PF_PA pa in pn.PAs)
                {                    
                    if (!pa.IsValid)
                    {
                        pn.IsValid = false;
                    }
                }
            } 

            pnRules.GuardarValores(invalidPNs);
            pnRules.GuardarValores(invalidPAs);
            tlRules.GuardarValores(invalidTLs);
        }

        //Tiene 0 referencias, por lo que se infiere no se ocupa
        public PF_Cinta LoadCintaBD(int archivoId)
        {
            PF_Cinta cinta = LoadPFCinta();
            cinta.ArchivoId = archivoId;

            pnRules.LoadPNs(cinta, Enums.Reporte.Mensual, false, "");
            Archivos_Rules ar = new Archivos_Rules(Enums.Persona.Fisica);
            Archivo archivo = ar.GetUltimoArchivo();
            CarteraDataAccess carteraData = new CarteraDataAccess(Enums.Persona.Fisica);
            archivo.Nombre.Substring(3, 8);

            //JAGH se genera solo una vez el store para traer creditos 02/04/13
            List<PF_TL> ITLs = new List<PF_TL>();
            ITLs = carteraData.GetTLs("", Enums.Reporte.Mensual, false, string.Format("{0}/{1}/{2}", archivo.Nombre.Substring(3, 2), archivo.Nombre.Substring(5, 2), archivo.Nombre.Substring(7, 4)), archivoId, "");

            foreach (PF_PN pn in cinta.PNs)
            {
                var tls = (from t in ITLs 
                           where t.PN_ID.Equals(pn.Id)
                           select t).ToList();

                foreach (PF_TL tl in tls)
                    tl.Parent = pn;

                pn.TLs.AddRange(tls);
            }

            for (int i = 0; i < cinta.PNs.Count; i++)
            {
                if (cinta.PNs[i].TLs.Count == 0 || cinta.PNs[i].PAs.Count == 0)
                {
                    cinta.PNs.RemoveAt(i);
                    i--;
                }
            }
            return cinta;
        }

    }
}
