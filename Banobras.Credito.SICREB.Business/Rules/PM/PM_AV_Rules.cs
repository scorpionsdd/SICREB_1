using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Data.Catalogos;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules.PM
{

    class PM_AV_Rules
    {
        
        private AvalDataAccess dataAV;

        public PM_AV_Rules()
        {
            dataAV = new AvalDataAccess(Enums.Persona.Moral);
        }

        public void LoadAVs(PM_CR cr)
        {
            List<Aval> avales = dataAV.GetAvalPorCredito(cr.CR_02, true);

            foreach (Aval itemAV in avales)
            {

                if (cr.CR_02 == itemAV.Credito)
                {
                    PM_AV av = new PM_AV(cr);
                    DatosFijos(av);

                    string NumAval = "1";
                    switch (itemAV.TipoAval)
                    {
                        case Enums.Persona.Moral: NumAval = "1"; break;
                        case Enums.Persona.Fisica: NumAval = "2"; break;
                        case Enums.Persona.Fideicomiso: NumAval = "3"; break;
                        case Enums.Persona.Gobierno: NumAval = "4"; break;
                        default: NumAval = "1"; break;
                    }

                    av.AV_00 = itemAV.RfcAval ;
                    av.AV_03 = itemAV.NombreCompania;
                    av.AV_04 = itemAV.Nombre;
                    av.AV_05 = itemAV.SNombre;
                    av.AV_06 = itemAV.ApellidoP;
                    av.AV_07 = itemAV.ApellidoM;
                    av.AV_08 = itemAV.Direccion;
                    av.AV_10 = itemAV.ColoniaPoblacion;
                    av.AV_11 = itemAV.DelegacionMunicipio;
                    av.AV_12 = itemAV.Ciudad;
                    av.AV_13 = itemAV.EstadoMexico;
                    av.AV_14 = itemAV.CodigoPostal;
                    av.AV_18 = NumAval;
                    av.AV_19 = itemAV.EstadoExtranjero;
                    av.AV_20 = itemAV.PaisOrigenDomicilio;
                    cr.AVs.Add(av);
                }

            }
        }

        public void LoadAVsDesdeArchivo(PM_CR cr, List<ValorArchivo> valores)
        {
            PM_AV av = new PM_AV(cr);

            var valEM = (from v in valores
                         where av.Etiquetas.Where(a => a.Id == v.EtiquetaId).FirstOrDefault() != default(Etiqueta)
                         select v).ToList();


            bool primeraVez = true;
            foreach (ValorArchivo va in valEM)
            {
                switch (Util.GetEtiqueta(av.Etiquetas, va.EtiquetaId).Codigo)
                {
                    case "AV":  if (!primeraVez)
                                {
                                    cr.AVs.Add(av);
                                    av = new PM_AV(cr);
                                }
                                primeraVez = false;
                                av.AV_AV = va.Valor;
                                break;
                    case "00":  av.AV_00 = va.Valor;  break;
                    case "01":  av.AV_01 = va.Valor;  break;
                    case "02":  av.AV_02 = va.Valor;  break;
                    case "03":  av.AV_03 = va.Valor;  break;
                    case "04":  av.AV_04 = va.Valor;  break;
                    case "05":  av.AV_05 = va.Valor;  break;
                    case "06":  av.AV_06 = va.Valor;  break;
                    case "07":  av.AV_07 = va.Valor;  break;
                    case "08":  av.AV_08 = va.Valor;  break;
                    case "09":  av.AV_09 = va.Valor;  break;
                    case "10":  av.AV_10 = va.Valor;  break;
                    case "11":  av.AV_11 = va.Valor;  break;
                    case "12":  av.AV_12 = va.Valor;  break;
                    case "13":  av.AV_13 = va.Valor;  break;
                    case "14":  av.AV_14 = va.Valor;  break;
                    case "15":  av.AV_15 = va.Valor;  break;
                    case "16":  av.AV_15 = va.Valor;  break;
                    case "17":  av.AV_17 = va.Valor;  break;
                    case "18":  av.AV_18 = va.Valor;  break;
                    case "19":  av.AV_19 = va.Valor;  break;
                    case "20":  av.AV_20 = va.Valor;  break;
                    case "21":  av.AV_21 = va.Valor;  break;
                }
            }

            cr.AVs.Add(av);
        }

        public void DatosFijos(PM_AV av)
        {
            av.AV_AV = "AV";
        }

        public void GuardarValores(PM_AV av, int archivoId)
        {
            ValoresArchivoDataAccess valores = new ValoresArchivoDataAccess();

            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "AV").Id, av.AV_AV));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "00").Id, av.AV_00));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "01").Id, av.AV_01));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "02").Id, av.AV_02));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "03").Id, av.AV_03));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "04").Id, av.AV_04));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "05").Id, av.AV_05));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "06").Id, av.AV_06));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "07").Id, av.AV_07));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "08").Id, av.AV_08));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "09").Id, av.AV_09));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "10").Id, av.AV_10));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "11").Id, av.AV_11));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "12").Id, av.AV_12));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "13").Id, av.AV_13));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "14").Id, av.AV_14));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "15").Id, av.AV_15));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "16").Id, av.AV_16));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "17").Id, av.AV_17));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "18").Id, av.AV_18));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "19").Id, av.AV_19));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "20").Id, av.AV_20));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(av.Etiquetas, "21").Id, av.AV_21));
        }
        
    }

}
