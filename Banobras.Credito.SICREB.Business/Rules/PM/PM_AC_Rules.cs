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
    public class PM_AC_Rules
    {

        private AccionistaDataAccess dataAC;

        public PM_AC_Rules()
        {
            dataAC = new AccionistaDataAccess(Enums.Persona.Moral);
        }

        public void LoadACs(PM_EM em)
        {
            List<Accionista> accionistas = dataAC.GetAccionistaPorCompania(em.EM_00, true);

            foreach (Accionista itemAC in accionistas)
            {
                PM_AC ac = new PM_AC(em);
                DatosFijos(ac);

                string NumAccionista = "1";
                switch (itemAC.Persona)
                {
                    case Enums.Persona.Moral: NumAccionista = "1"; break;
                    case Enums.Persona.Fisica: NumAccionista = "2"; break;
                    case Enums.Persona.Fideicomiso: NumAccionista = "3"; break;
                    case Enums.Persona.Gobierno: NumAccionista = "4"; break;
                    default: NumAccionista = "1"; break;
                }                

                ac.AC_00 = itemAC.RfcAccionista;
                ac.AC_03 = itemAC.NombreCompania;
                ac.AC_04 = itemAC.Nombre;
                ac.AC_05 = itemAC.SNombre;
                ac.AC_06 = itemAC.ApellidoP;
                ac.AC_07 = itemAC.ApellidoM;
                ac.AC_08 = itemAC.Porcentaje.ToString("00");
                ac.AC_09 = itemAC.Direccion;
                ac.AC_11 = itemAC.ColoniaPoblacion;
                ac.AC_12 = itemAC.DelegacionMunicipio;
                ac.AC_13 = itemAC.Ciudad;
                ac.AC_14 = itemAC.EstadoMexico;
                ac.AC_15 = itemAC.CodigoPostal;
                ac.AC_19 = NumAccionista;
                ac.AC_20 = itemAC.EstadoExtranjero;
                ac.AC_21 = itemAC.PaisOrigenDomicilio;

                //<MASS 09-nov-2017  RFC de accionista extranjero. limite 13 caracteres.
                if (ac.EsExtranjero)
                {
                    if (itemAC.RfcAccionista.Length > 13)
                        ac.AC_00 = itemAC.RfcAccionista.Substring(0, 13);
                }
                //</MASS>

                em.ACs.Add(ac);
            }
        }

        public void LoadACsDesdeArchivo(PM_EM em, List<ValorArchivo> valores)
        {
            PM_AC ac = new PM_AC(em);

            var valEM = (from v in valores
                         where ac.Etiquetas.Where(a => a.Id == v.EtiquetaId).FirstOrDefault() != default(Etiqueta)
                         select v).ToList();

            
            bool primeraVez = true;
            foreach (ValorArchivo va in valEM)
            {
                switch (Util.GetEtiqueta(ac.Etiquetas, va.EtiquetaId).Codigo)
                {
                    case "AC":
                        if (!primeraVez)
                        {
                            em.ACs.Add(ac);
                            ac = new PM_AC(em);
                        }
                        primeraVez = false;
                        ac.AC_AC = va.Valor;
                        break;
                    case "00":  ac.AC_00 = va.Valor;  break;
                    case "01":  ac.AC_01 = va.Valor;  break;
                    case "02":  ac.AC_02 = va.Valor;  break;
                    case "03":  ac.AC_03 = va.Valor;  break;
                    case "04":  ac.AC_04 = va.Valor;  break;
                    case "05":  ac.AC_05 = va.Valor;  break;
                    case "06":  ac.AC_06 = va.Valor;  break;
                    case "07":  ac.AC_07 = va.Valor;  break;
                    case "08":  ac.AC_08 = va.Valor;  break;
                    case "09":  ac.AC_09 = va.Valor;  break;
                    case "10":  ac.AC_10 = va.Valor;  break;
                    case "11":  ac.AC_11 = va.Valor;  break;
                    case "12":  ac.AC_12 = va.Valor;  break;
                    case "13":  ac.AC_13 = va.Valor;  break;
                    case "14":  ac.AC_14 = va.Valor;  break;
                    case "15":  ac.AC_15 = va.Valor;  break;
                    case "16":  ac.AC_15 = va.Valor;  break;
                    case "17":  ac.AC_17 = va.Valor;  break;
                    case "18":  ac.AC_18 = va.Valor;  break;
                    case "19":  ac.AC_19 = va.Valor;  break;
                    case "20":  ac.AC_20 = va.Valor;  break;
                    case "21":  ac.AC_21 = va.Valor;  break;
                    case "22":  ac.AC_22 = va.Valor;  break;
                }
            }

            em.ACs.Add(ac);
        }

        public void DatosFijos(PM_AC ac)
        {
            ac.AC_AC = "AC";
        }

        public void GuardarValores(PM_AC ac, int archivoId)
        {
            ValoresArchivoDataAccess valores = new ValoresArchivoDataAccess();

            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "AC").Id, ac.AC_AC));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "00").Id, ac.AC_00));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "01").Id, ac.AC_01));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "02").Id, ac.AC_02));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "03").Id, ac.AC_03));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "04").Id, ac.AC_04));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "05").Id, ac.AC_05));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "06").Id, ac.AC_06));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "07").Id, ac.AC_07));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "08").Id, ac.AC_08));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "09").Id, ac.AC_09));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "10").Id, ac.AC_10));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "11").Id, ac.AC_11));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "12").Id, ac.AC_12));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "13").Id, ac.AC_13));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "14").Id, ac.AC_14));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "15").Id, ac.AC_15));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "16").Id, ac.AC_16));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "17").Id, ac.AC_17));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "18").Id, ac.AC_18));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "19").Id, ac.AC_19));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "20").Id, ac.AC_20));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "21").Id, ac.AC_21));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ac.Etiquetas, "22").Id, ac.AC_22));            
        }

    }

}
