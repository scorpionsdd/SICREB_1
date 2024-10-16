
namespace Banobras.Credito.SICREB.Entities
{
    
    public class EstadoCivil
    {
        public int Id { get; private set; }
        public int IdClic { get; private set; }
        public string DescripcionClic { get; private set; }
        public string ClaveBuro { get; private set; }

        public EstadoCivil(int Id, int IdClic, string DescripcionClic, string ClaveBuro)
        {
            this.Id = Id;
            this.IdClic = IdClic;
            this.DescripcionClic = DescripcionClic;
            this.ClaveBuro = ClaveBuro;
        }
        
    }

}
