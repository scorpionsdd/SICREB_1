using System.Collections.Generic;

namespace Banobras.Credito.SICREB.Entities.GL
{

    public class Cinta_Personas_Morales_Gpos_Lcryc
    {
        public Segmento_HD_Gpos_Lcryc Segmegto_HD { get; set; }
        public List<Segmento_EM_Gpos_Lcryc> Segmegto_EM { get; set; }
        public List<Segmento_AC_Gpos_Lcryc> Segmegto_AC { get; set; }
        public List<Segmento_CR_Gpos_Lcryc> Segmegto_CR { get; set; }
        public List<Segmento_DE_Gpos_Lcryc> Segmegto_DE { get; set; }
        public List<Segmento_AV_Gpos_Lcryc> Segmegto_AV { get; set; }
        public Segmento_TS_Gpos_Lcryc Segmegto_TS { get; set; }      
    }

}
