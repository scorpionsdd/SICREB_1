using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Banobras.Credito.SICREB.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules.PM
{
    public class PM_HD_Rules
    {

        public void LoadHD(PM_Cinta cinta, bool mensual)
        {

            string fecha = string.Empty;
            string periodo = string.Empty;

            // Obtenemos la fecha y el periodo de la Cinta desde la BD
            if (mensual)
            {
                fecha =  Obtener_Fecha_Cintas.fecha_Cinta_Mensual();
                periodo = Obtener_Fecha_Cintas.periodo_Cinta_Mensual();                
            }
            else
            {
                fecha = Obtener_Fecha_Cintas.periodo_Cinta_Semanal();
                periodo = Obtener_Fecha_Cintas.periodo_Cinta_Semanal();            
            }

            PM_HD hd = new PM_HD(cinta);

            hd.HD_HD = WebConfig.HD_HD;
            hd.HD_00 = WebConfig.HD_00;
            hd.HD_01 = WebConfig.HD_01;
            hd.HD_02 = WebConfig.HD_02;
            hd.HD_03 = WebConfig.HD_03;
            hd.HD_04 = fecha;
            hd.HD_05 = periodo;
            hd.HD_06 = WebConfig.HD_06;
            hd.HD_07 = WebConfig.HD_07;

            cinta.HD = hd;
        }

        public void LoadHDDesdeArchivo(PM_Cinta cinta, List<ValorArchivo> valores)
        {
            PM_HD hd = new PM_HD(cinta);

            var valHD = (from v in valores
                         where hd.Etiquetas.Where(h => h.Id == v.EtiquetaId).FirstOrDefault() != default(Etiqueta)
                         select v).ToList();

            hd.HD_HD = Util.GetValorArchivo(valHD, hd.Etiquetas, "HD");
            hd.HD_00 = Util.GetValorArchivo(valHD, hd.Etiquetas, "00");
            hd.HD_01 = Util.GetValorArchivo(valHD, hd.Etiquetas, "01");
            hd.HD_02 = Util.GetValorArchivo(valHD, hd.Etiquetas, "02");
            hd.HD_03 = Util.GetValorArchivo(valHD, hd.Etiquetas, "03");
            hd.HD_04 = Util.GetValorArchivo(valHD, hd.Etiquetas, "04");
            hd.HD_05 = Util.GetValorArchivo(valHD, hd.Etiquetas, "05");
            hd.HD_06 = Util.GetValorArchivo(valHD, hd.Etiquetas, "06");
            hd.HD_07 = Util.GetValorArchivo(valHD, hd.Etiquetas, "07");
            hd.HD_08 = Util.GetValorArchivo(valHD, hd.Etiquetas, "08");

            cinta.HD = hd;
        }

        public void GuardarValores(PM_HD hd, int archivoId)
        {
            ValorArchivoCollection valoresList = new ValorArchivoCollection();
            ValoresArchivoDataAccess valores = new ValoresArchivoDataAccess();

            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "HD").Id, hd.HD_HD));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "00").Id, hd.HD_00));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "01").Id, hd.HD_01));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "02").Id, hd.HD_02));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "03").Id, hd.HD_03));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "04").Id, hd.HD_04));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "05").Id, hd.HD_05));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "06").Id, hd.HD_06));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "07").Id, hd.HD_07));
            valoresList.Add(new ValorArchivo(0, archivoId, Util.GetEtiqueta(hd.Etiquetas, "08").Id, hd.HD_08));

            string msg;
            valores.AddValorArchivo(valoresList, out msg);
        }

    }

}
