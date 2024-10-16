using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Validacion
    {
        public int Id { get; private set; }
        public Enums.Rechazo Tipo { get; private set; }
        public bool Aplicable { get; private set; }
        public Enums.Persona Persona { get; private set; }
        public string Mensaje { get; private set; }
        public Enums.Estado Estatus { get; private set; }
        public string Codigo { get; private set; }
        public int Etiqueta_Id { get; private set; }
        public string Etiqueta { get; set; }
        public string Campo { get; set; }

        public Validacion(int id, Enums.Rechazo tipo, bool aplicable, Enums.Persona persona, string mensaje, Enums.Estado estatus, string codigo, int etiquetaId,string etiqueta,string campo)
        {
            this.Id = id;
            this.Tipo = tipo;
            this.Aplicable = aplicable;
            this.Persona = persona;
            this.Mensaje = mensaje;
            this.Estatus = estatus;
            this.Codigo = codigo;
            this.Etiqueta_Id = etiquetaId;
            this.Etiqueta = etiqueta;
            this.Campo = campo;

        }
    }
}
