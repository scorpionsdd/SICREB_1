
namespace Banobras.Credito.SICREB.Entities
{
    public class Conciliacion
    {
        public int Id { get; set; }
        public string Credito { get; set; }
        public double SaldoVigenteOriginal { get; set; }
        public int SaldoVigente { get; set; }
        public double SaldoVencidoOriginal { get; set; }
        public int SaldoVencido { get; set; }
        public int IdArchivo { get; set; }
        public string Auxiliar { get; set; }
        public string Rfc { get; set; }
    }
}
