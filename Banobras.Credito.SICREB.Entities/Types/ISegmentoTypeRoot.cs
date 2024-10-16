using System.Collections.Generic;

namespace Banobras.Credito.SICREB.Entities.Types
{

    /// <summary>
    /// Interfaz para los tipos de segmentos
    /// </summary>
    /// <typeparam name="T">Especifica el tipo de segmento del root del registro.</typeparam>
    /// <typeparam name="U">Especifica el tipo de segmento del Padre</typeparam>
    public interface ISegmentoTypeRoot<T, U> : ISegmentoType
    {
        T MainRoot { get; }
        U TypedParent { get; }
    }

    public interface ISegmentoType
    {
        bool IsValid { get; set; }
        int Correctos { get; set; }
        int AuxId { get; set; }
        ISegmentoType Parent { get; set; }
        List<Etiqueta> Etiquetas { get; }
        List<Validacion> Validaciones { get; }
    }
   
}
