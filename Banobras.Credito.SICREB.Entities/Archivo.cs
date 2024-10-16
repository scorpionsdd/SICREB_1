using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class Archivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Enums.Persona TipoPersona { get; private set; }
        public DateTime Fecha { get; private set; }
        public StringBuilder ContenidoArchivo { get; set; }
        public byte[] BytesArchivo { get; set; }

        public int Reg_Correctos { get; private set; }
        public int Reg_Errores { get; private set; }
        public int Reg_Advertencias { get; private set; }


        public Archivo(int id, string nombre, Enums.Persona tipoPersona, DateTime fecha, StringBuilder archivo)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.TipoPersona = tipoPersona;
            this.Fecha = fecha;
            this.ContenidoArchivo = archivo;
        }

        public Archivo()
        {
            //test baad eliminar
        }

        public void SetEstadisticas(int correctos, int errores, int advertencias)
        {
            this.Reg_Correctos = correctos;
            this.Reg_Errores = errores;
            this.Reg_Advertencias = advertencias;
        }

        public byte[] GetByteArchivo()
        {
            string textoArchivo = string.Empty;

            if (this.ContenidoArchivo != null)
            {
                textoArchivo = this.ContenidoArchivo.ToString();
            }

            //JAGH 21/03/13 se eliminan saltos de linea en textoArchivo
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArchivo = encoding.GetBytes(textoArchivo.Replace("\r\n",  string.Empty).Replace("\r",  string.Empty).Replace("\n",  string.Empty));

            return byteArchivo;
        }

    }
}
