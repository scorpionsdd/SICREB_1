using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities
{
    public class ClientesPF
    {
        public string Rfc { get; private set; }
        public string Curp { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido_paterno { get; private set; }
        public string Apellido_materno { get; private set; }
        public string Nacionalidad_clave { get; private set; }
        public string Nacionalidad { get; private set; }
        public string Calle { get; private set; }
        public string Num_ext { get; private set; }
        public string Num_int { get; private set; }
        public string Colonia { get; private set; }
        public string Municipio_clave { get; private set; }
        public string Municipio { get; private set; }
        public string Ciudad { get; private set; }
        public string Estado_clave { get; private set; }
        public string Estado { get; private set; }
        public string Codigo_postal { get; private set; }
        public string Telefonos { get; private set; }
        public string Pais_clave { get; private set; }
        public string Pais { get; private set; }
        public string Tipo_cliente_clave { get; private set; }
        public string Tipo_cliente { get; private set; }
        public string Fecha_nac { get; private set; }
        //public DateTime dtFecha_nac { get; private set; }
        public string Estado_civil_clave { get; private set; }
        public string Estado_civil { get; private set; }
        public string Sexo { get; private set; }
        public string Usuario_alta { get; private set; }
        public string Consecutivo { get; private set; }
        public string Estatus { get; private set; }
        public string Id_tipo_cliente { get; private set; }

        public ClientesPF(string pRfc, string pCurp, string pNombre, string pApellido_paterno, string pApellido_materno, string pNacionalidad_clave, string pNacionalidad, string pCalle, string pNum_ext, string pNum_int, string pColonia, string pMunicipio_clave, string pMunicipio, string pCiudad, string pEstado_clave, string pEstado, string pCodigo_postal, string pTelefonos, string pPais_clave, string pPais, string pTipo_cliente_clave, string pTipo_cliente, string pFecha_nac, string pEstado_civil_clave, string pEstado_civil, string pSexo, string pUsuario_alta, string pConsecutivo, string pEstatus, string pId_tipo_cliente)
        {
            this.Rfc = pRfc;
            this.Curp = pCurp;
            this.Nombre = pNombre;
            this.Apellido_paterno = pApellido_paterno;
            this.Apellido_materno = pApellido_materno;
            this.Nacionalidad_clave = pNacionalidad_clave;
            this.Nacionalidad = pNacionalidad;
            this.Calle = pCalle;
            this.Num_ext = pNum_ext;
            this.Num_int = pNum_int;
            this.Colonia = pColonia;
            this.Municipio_clave = pMunicipio_clave;
            this.Municipio = pMunicipio;
            this.Ciudad = pCiudad;
            this.Estado_clave = pEstado_clave;
            this.Estado = pEstado;
            this.Codigo_postal = pCodigo_postal;
            this.Telefonos = pTelefonos;
            this.Pais_clave = pPais_clave;
            this.Pais = pPais;
            this.Tipo_cliente_clave = pTipo_cliente_clave;
            this.Tipo_cliente = pTipo_cliente;
            this.Fecha_nac = pFecha_nac;
            this.Estado_civil_clave = pEstado_civil_clave;
            this.Estado_civil = pEstado_civil;
            this.Sexo = pSexo;
            this.Usuario_alta = pUsuario_alta;
            this.Consecutivo = pConsecutivo;
            this.Estatus = pEstatus;
            this.Id_tipo_cliente = pId_tipo_cliente;
        }



    }
}
