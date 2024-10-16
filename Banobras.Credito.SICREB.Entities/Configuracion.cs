
namespace Banobras.Credito.SICREB.Entities
{
    public class Configuracion 
    {

        public int Id { get; private set; }
        public int Catsicofin { get; private set; }
        public int Catsic { get; private set; }
        public int IdUsuario { get; private set; }
        public int Calificacion { get; private set; }


        public Configuracion(int pId, int pCatSicofin, int pCatSic, int pIdUsuario, int pCalificacion)
        {
            this.Id = pId;
            this.Catsicofin = pCatSicofin;
            this.Catsic = pCatSic;
            this.IdUsuario = pIdUsuario;
            this.Calificacion = pCalificacion;
        }

        public Configuracion() { 
        
        }

    }
}
