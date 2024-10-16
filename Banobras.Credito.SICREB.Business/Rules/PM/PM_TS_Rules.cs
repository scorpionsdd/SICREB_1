using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Business.Rules.PM
{
    public class PM_TS_Rules
    {
        public void LoadTS(PM_Cinta cinta)
        {
            PM_TS ts = new PM_TS(cinta);
            ts.TS_TS = "TS";

            ////trae todas las empresas validas
            //List<PM_EM> emValidas = (from e in cinta.EMs
            //                         where e.IsValid == true
            //                         select e).ToList();
            //ts.TS_00 = emValidas.Count.ToString();

            /*JAGH  13/03/13
             * no tomara en cuenta los registros que despues pudieran invalidarse
             * y trae todas suma de monto de las empresas con detalles de creditos vigentes
             */
            List<PM_EM> validEMs = new List<PM_EM>();
            int iValidEM, iValidCR, iValidDE;
            double sumaCuenta = 0;

            //recorre cinta para encontrar todos los registros validos
            foreach (PM_EM em in cinta.EMs)
            {
                //resetea valores
                iValidEM = 0;
                iValidCR = 0;
                iValidDE = 0;
                //verifica si EM es valido e incrementa variable
                if (em.IsValid)
                {
                    iValidEM++;
                    //for CR
                    foreach (PM_CR cr in em.CRs)
                    {
                        //verifica si CR es valido e incrementa variable
                        if (cr.IsValid)
                        {
                            iValidCR++;
                            //for DE
                            foreach (PM_DE de in cr.DEs)
                            {
                                //verifica si DE es valido e incrementa variable
                                if (de.IsValid)
                                {
                                    iValidDE++;
                                    sumaCuenta += Parser.ToDouble(de.DE_03);

                                    ////JAGH 14/03/13 se comenta pues debe sumar todo
                                    //if (Parser.ToNumber(de.DE_02) == 0)
                                    //{
                                    //    sumaCuenta += Parser.ToDouble(de.DE_03);
                                    //}
                                }
                            }//fin for DE
                        }
                    }//fin for CR

                }

                //debe tener por lo menos un EM, CR y DE valido
                if (iValidEM > 0 && iValidCR > 0 && iValidDE > 0)
                    validEMs.Add(em);

            }
            //fin recorre cinta para encontrar todos los registros validos

            //llena etiquetas correspondientes
            ts.TS_00 = validEMs.Count.ToString();
            ts.TS_01 = sumaCuenta.ToString("#");

            //trae todas las empresas con detalles de creditos vigentes
            //double sumaCuenta = 0;
            //foreach (PM_EM em in validEMs)
            //{
            //    foreach (PM_CR cr in em.CRs)
            //    {
            //        foreach (PM_DE de in cr.DEs)
            //        {
            //            if (Parser.ToNumber(de.DE_02) == 0)
            //            {
            //                sumaCuenta += Parser.ToDouble(de.DE_03);
            //            }
            //        }
            //    }
            //}
            //ts.TS_01 = sumaCuenta.ToString("#");

            cinta.TS = ts;
        }
        public void LoadTSDesdeArchivo(PM_Cinta cinta, List<ValorArchivo> valoresTS)
        {
            PM_TS ts = new PM_TS(cinta);

            var valTS = (from v in valoresTS
                         where ts.Etiquetas.Where(t => t.Id == v.EtiquetaId).FirstOrDefault() != default(Etiqueta)
                         select v).ToList();

            ts.TS_TS = Util.GetValorArchivo(valTS, ts.Etiquetas, "TS");
            ts.TS_00 = Util.GetValorArchivo(valTS, ts.Etiquetas, "00");
            ts.TS_01 = Util.GetValorArchivo(valTS, ts.Etiquetas, "01");
            ts.TS_02 = Util.GetValorArchivo(valTS, ts.Etiquetas, "02");

            cinta.TS = ts;
        }

        public void GuardarValores(PM_TS ts, int archivoId)
        {
            ValorArchivoCollection valoresList = new ValorArchivoCollection();
            //100 bytes
            ValoresArchivoDataAccess valores = new ValoresArchivoDataAccess();

            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ts.Etiquetas, "TS").Id, ts.TS_TS));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ts.Etiquetas, "00").Id, ts.TS_00));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ts.Etiquetas, "01").Id, ts.TS_01));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ts.Etiquetas, "02").Id, ts.TS_02));

            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ts.Etiquetas, "TS").Id, ts.TS_TS));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ts.Etiquetas, "00").Id, ts.TS_00));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ts.Etiquetas, "01").Id, ts.TS_01));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(ts.Etiquetas, "02").Id, ts.TS_02));

            string msg;
            valores.AddValorArchivo(valoresList, out msg);

        }
    }
}
