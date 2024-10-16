using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules.PF
{

    public class PF_INTF_Rules
    {

        public void LoadINTF(PF_Cinta cinta, bool mensual)
        {

            DateTime mesActual = DateTime.Now;
            DateTime mesAnterior = Convert.ToDateTime("01/" + mesActual.Month.ToString("00") + "/" + mesActual.Year.ToString("0000"));
            string FechaReporte = "";

            // JAGH 04/03/13 obtener fecha de cintas
            if (mensual)
            {
                FechaReporte = Obtener_Fecha_Cintas.fecha_Cinta_Mensual();
            }
            else
            {
                FechaReporte = Obtener_Fecha_Cintas.periodo_Cinta_Semanal();
            }

            PF_INTF intf = new PF_INTF();
            intf.INTF_01 = WebConfig.INTF_01_ES;
            intf.INTF_05 = WebConfig.INTF_05_ES;
            intf.INTF_07 = WebConfig.INTF_07_ES;
            intf.INTF_17 = WebConfig.INTF_17_ES;
            intf.INTF_33 = WebConfig.INTF_33_ES;
            intf.INTF_35 = FechaReporte;
            intf.INTF_43 = string.Empty.PadRight(10, '0');
            intf.INTF_53 = string.Empty.PadRight(98, ' ');

            cinta.INTF = intf;
        }

        public void GuardarValores(PF_INTF intf, int archivoId)
        {
            //LAS ETIQUETAS NO ESTAN GUARDADAS. SI EN UN FUTURO ES NECESARIO GUARDAR TODA LA INFORMACION DEL SEGMENTO, SERA NECESARIO AGREGAR LAS ETIQUETAS COMPLETAS Y DESCOMENTAR EL SIEGUIENTE CODIGO!
            //ValoresArchivoDataAccess valores = new ValoresArchivoDataAccess();

            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "INTF").Id, "INTF"));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "01").Id, intf.INTF_01));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "05").Id, intf.INTF_05));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "07").Id, intf.INTF_07));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "17").Id, intf.INTF_17));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "33").Id, intf.INTF_33));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "35").Id, intf.INTF_35));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "43").Id, intf.INTF_43));
            //valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(intf.Etiquetas, "53").Id, intf.INTF_53));    
        }

        public void LoadINTFDesdeArchivo(PF_Cinta cinta, List<ValorArchivo> valores)
        {
            PF_INTF intf = new PF_INTF(cinta);

            //intf.INTF_01 = Util.GetValorArchivo(valores, intf.Etiquetas, "01");
            //intf.INTF_05 = Util.GetValorArchivo(valores, intf.Etiquetas, "05");
            //intf.INTF_07 = Util.GetValorArchivo(valores, intf.Etiquetas, "07");
            //intf.INTF_17 = Util.GetValorArchivo(valores, intf.Etiquetas, "17");
            //intf.INTF_33 = Util.GetValorArchivo(valores, intf.Etiquetas, "33");
            //intf.INTF_35 = Util.GetValorArchivo(valores, intf.Etiquetas, "35");
            //intf.INTF_43 = Util.GetValorArchivo(valores, intf.Etiquetas, "43");
            //intf.INTF_53 = Util.GetValorArchivo(valores, intf.Etiquetas, "53");

            cinta.INTF = intf;
        }
        
    }

}
