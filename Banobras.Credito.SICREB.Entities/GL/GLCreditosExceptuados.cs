using System;

namespace Banobras.Credito.SICREB.Entities.GL
{

    public class GLCreditosExceptuados
    {
        public Int64 Id { get; set; }
        public String CREDITO { get; set; }
        public String MOTIVO { get; set; }
        public String ESTATUS { get; set; }
    }

}
