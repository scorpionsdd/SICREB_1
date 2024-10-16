using Banobras.Credito.SICREB.Entities.Types;

namespace Banobras.Credito.SICREB.Entities
{
    public class ValidacionEntry
    {
        public ErrorWarning ErrorWarning { get; private set; }
        public ISegmentoType Rechazar { get; private set; }

        public ValidacionEntry(ErrorWarning error, ISegmentoType rechazar)
        {
            this.ErrorWarning = error;
            this.Rechazar = rechazar;
        }
    }
}
