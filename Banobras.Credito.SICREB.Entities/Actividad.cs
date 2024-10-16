using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Banobras.Credito.SICREB.Entities;


namespace Banobras.Credito.SICREB.Entities
{
    public class Actividad
    {
        public int IdAct { get; set; }
        public int IdUsuario { get; set; }
        public string detalle { get; set; }
        public DateTime fecha { get; set; }
        public string descripcion { get; set; }
        public string antes { get; set; }
        public string despues { get; set; }
        public string Catalogo { get; set; }
      
        public string login { get; set; }
        public string rol { get; set; } //JAGH 30/04/2013 se agrega

        public Actividad(int idActvidad, int idUsuario, string detalle, DateTime fecha)
        {
            this.IdAct = idActvidad;
            this.IdUsuario = idUsuario;
            this.detalle = detalle;
            this.fecha = fecha;
        }

        public Actividad(IDataReader reader)
        {

            if (reader["descripcion"] != null)
            {
                this.descripcion = Convert.ToString(reader["descripcion"]);
            }
            //JAGH 30/04/2013 se modifica para asignar rol
            if (reader["rol"] != null)
            {
                this.rol = Convert.ToString(reader["rol"]);
            }
            //JAGH 30/04/2013 se agrega para asignar usuario -login-
            if (reader["login"] != null)
            {
                this.login = Convert.ToString(reader["login"]);
            }
            if (reader["detalle"] != null)
            {
                this.detalle = Convert.ToString(reader["detalle"]);
            }
            if (reader["FECHA"] != null)
            {
                this.fecha = Convert.ToDateTime(reader["FECHA"]);
            }
            this.IdUsuario = Convert.ToInt32(reader["ID_USUARIO"]);
            if (reader["DATOSANTES"] != null)
            {
                this.antes = Convert.ToString(reader["DATOSANTES"]);
            }
            if (reader["DATOSDESPUES"] != null)
            {
                this.despues = Convert.ToString(reader["DATOSDESPUES"]);
            }
            if (reader["CATALOGO"] != null)
            {
                this.Catalogo = Convert.ToString(reader["CATALOGO"]);
            }

        }
    }
}
