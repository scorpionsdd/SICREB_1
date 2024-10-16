using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Banobras.Credito.SICREB.Entities.Vistas
{
    public class ActividadDatos
    {
        public int ID_USUARIO { get; set; }
        public string ROL { get; set; }
        public string DETALLE { get; set; }
        public string FECHA_REGISTRO { get; set; }
        public string HORA_REGISTRO { get; set; }
        public string ESTATUS { get; set; }
        public string DESCRIPCION { get; set; }


        public ActividadDatos(int ID_USUARIO, string ROL, string NOMBRE_CATALOGO, string FECHA_REGISTRO, string HORA_REGISTRO, string ESTATUS, string DESCRIPCION)
        {
            this.ID_USUARIO = ID_USUARIO;
            this.ROL = ROL;
            this.DETALLE = NOMBRE_CATALOGO;
            this.FECHA_REGISTRO = FECHA_REGISTRO;
            this.HORA_REGISTRO = HORA_REGISTRO;
            this.ESTATUS = ESTATUS;
            this.DESCRIPCION = DESCRIPCION;

        }
        public ActividadDatos(IDataReader reader)
        {

            if (reader["ID_USUARIO"] != null)
            {
                this.ID_USUARIO = Convert.ToInt32(reader["ID_USUARIO"]);
            }
            if (reader["ROL"] != null)
            {
                this.ROL = Convert.ToString(reader["ROL"]);
            }
            if (reader["DETALLE"] != null)
            {
                this.DETALLE = Convert.ToString(reader["DETALLE"]);
            }
            if (reader["FECHA"] != null)
            {
                this.FECHA_REGISTRO = Convert.ToString(reader["FECHA"]);
            }

           
            if (reader["DESCRIPCION"] != null)
            {
                this.DESCRIPCION = Convert.ToString(reader["DESCRIPCION"]);

            }


        }
    }
}

