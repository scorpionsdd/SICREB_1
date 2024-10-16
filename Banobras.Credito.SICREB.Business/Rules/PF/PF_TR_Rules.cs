using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Types;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Data.Transaccionales;

namespace Banobras.Credito.SICREB.Business.Rules.PF
{

    public class PF_TR_Rules
    {

        public void LoadTRs(PF_Cinta cinta)
        {
            PF_TR tr = new PF_TR(cinta);
            tr.CalcularTR(); // Calcular Valores Totales
            
            tr.TR_Etiqueta = WebConfig.TR_Etiqueta_ES;
            tr.TR_78 = WebConfig.TR_78_ES;
            tr.TR_72 = WebConfig.TR_72_ES;
            tr.TR_94 = WebConfig.TR_94_ES;
            
            cinta.TR = tr;
        }

        public void GuardarValores(PF_TR tr, int archivoId)
        {
            ValoresArchivoDataAccess valores = new ValoresArchivoDataAccess();

            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "05").Id, tr.TR_05));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "19").Id, tr.TR_19));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "33").Id, tr.TR_33));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "36").Id, tr.TR_36));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "45").Id, tr.TR_45));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "54").Id, tr.TR_54));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "63").Id, tr.TR_63));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "72").Id, tr.TR_72));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "78").Id, tr.TR_78));
            valores.AddValorArchivo(new ValorArchivo(0, archivoId, Util.GetEtiqueta(tr.Etiquetas, "94").Id, tr.TR_94));            
        }

        public void LoadTRDesdeArchivo(PF_Cinta cinta, List<ValorArchivo> valores)
        {
            PF_TR tr = new PF_TR(cinta);

            tr.TR_05 = Util.GetValorArchivo(valores, tr.Etiquetas, "05");
            tr.TR_19 = Util.GetValorArchivo(valores, tr.Etiquetas, "19");
            tr.TR_33 = Util.GetValorArchivo(valores, tr.Etiquetas, "33");
            tr.TR_36 = Util.GetValorArchivo(valores, tr.Etiquetas, "36");
            tr.TR_45 = Util.GetValorArchivo(valores, tr.Etiquetas, "45");
            tr.TR_54 = Util.GetValorArchivo(valores, tr.Etiquetas, "54");
            tr.TR_63 = Util.GetValorArchivo(valores, tr.Etiquetas, "63");
            tr.TR_72 = WebConfig.TR_78_ES;
            tr.TR_78 = WebConfig.TR_72_ES;
            tr.TR_94 = WebConfig.TR_94_ES;

            cinta.TR = tr;
        }

    }

}
