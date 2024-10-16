using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{
    
    /// <summary>
    /// Esta clase no se puede instanciar!. 
    /// Pero todas las clases de Datos (de los catalogos únicamente) deben heredar de esta clase.
    /// </summary>
    public class CatalogosDataAccess2 : OracleBase
    {
        //TODAVIA NO IMPLEMENTO:  ,Banxico, Contrato, Cuenta, Institucion, Responsabilidad, Segmento
        public enum CatalogoGenerico { Nacionalidad, Moneda, Rechazo, EstadoMex, CalifCartera, Observacion, Creditos,
                                       Num_Pagos, Frecuencias, Institucion, Exceptuados, Contrato, Cuenta,  
                                       Responsabilidad, Segmento
                                     };

        Dictionary<CatalogoGenerico, String> StoredProcedureCommand = new Dictionary<CatalogoGenerico, string>(){ 
                                    { CatalogoGenerico.Nacionalidad, "CATALOGOS.SP_CATALOGOS_GetClavesPais"},
                                    { CatalogoGenerico.Moneda, "CATALOGOS.SP_CATALOGOS_GetMonedas" },
                                    { CatalogoGenerico.Rechazo, "CATALOGOS." }, 
                                    { CatalogoGenerico.EstadoMex, "CATALOGOS.SP_CATALOGOS_GetEstados" },
                                    { CatalogoGenerico.CalifCartera, "CATALOGOS.SP_CATALOGOS_GetCalificaciones" },
                                    { CatalogoGenerico.Observacion, "CATALOGOS." },
                                    { CatalogoGenerico.Num_Pagos, "CATALOGOS.SP_CATALOGOS_GetNumPagos" },
                                    { CatalogoGenerico.Frecuencias, "CATALOGOS.SP_CATALOGOS_GetFrecuenciasPag" },
                                    { CatalogoGenerico.Creditos, "CATALOGOS.SP_CATALOGOS_GetCreditos" },
                                    { CatalogoGenerico.Institucion, "CATALOGOS.SP_CATALOGOS_GetInstituciones" },
                                    { CatalogoGenerico.Exceptuados, "CATALOGOS.SP_CATALOGOS_GetExceptuados" },
                                    //{ CatalogoGenerico.Banxico, "CATALOGOS.SP_CATALOGOS_GetClavesBanxico" },
                                    { CatalogoGenerico.Contrato, "CATALOGOS.SP_CATALOGOS_GetContratos" },
                                    { CatalogoGenerico.Cuenta, "CATALOGOS.SP_CATALOGOS_GetCuentas" },
                                    { CatalogoGenerico.Responsabilidad, "CATALOGOS.SP_CATALOGOS_GetRespons" },
                                    { CatalogoGenerico.Segmento, "CATALOGOS." }, 
                                };

        #region Listas para especificar los campos necesarios

        //lista de catalogos que NO tienen campo descripcion
        List<CatalogoGenerico> noDescripcion = new List<CatalogoGenerico>() { CatalogoGenerico.Num_Pagos, CatalogoGenerico.Frecuencias };
        
        //lista de catalogos que NO tienen clave interna
        List<CatalogoGenerico> noClaveInterna = new List<CatalogoGenerico>(){ CatalogoGenerico.CalifCartera, 
                                                                              CatalogoGenerico.Observacion, 
                                                                              CatalogoGenerico.Creditos, 
                                                                              CatalogoGenerico.Rechazo, 
                                                                              CatalogoGenerico.Institucion, 
                                                                              CatalogoGenerico.Exceptuados 
                                                                            };
        
        //lista de catalogos que SI tienen persona
        List<CatalogoGenerico> siPersona = new List<CatalogoGenerico>() { CatalogoGenerico.Observacion, 
                                                                          CatalogoGenerico.Nacionalidad, 
                                                                          CatalogoGenerico.EstadoMex, 
                                                                          CatalogoGenerico.Rechazo 
                                                                        };
        
        #endregion

        private List<Catalogo> allCatalogos = null;
        private readonly string storedProc;
        private readonly string idField = "ID";
        private readonly string codigoField = "CLAVE_EXTERNA";
        private string codigoIntField = "CLAVE_INTERNA";
        private string descField = "DESCRIPCION";
        private readonly string estatusField = "ESTATUS";
        private readonly string abrevacionField = String.Empty;
        private readonly string persona = "M";
        private bool setPersona = false;

        #region Constructores

        /// <summary>
        /// Constructor. Inicializa el nombre de stored procedure a ejecutar y el nombre de los campos
        /// </summary>
        /// <param name="storedProcCommand">Stored proc. del catalogo. Debe regresar todos los registros de la tabla</param>
        public CatalogosDataAccess2(CatalogoGenerico catalogo, Enums.Persona persona)
        {

            this.storedProc = StoredProcedureCommand[catalogo];
            this.persona = (persona == Enums.Persona.Moral) ? "M" : "F";
            InicializaFields(catalogo);
            
        }

        /// <summary>
        /// Constructor. Inicializa el nombre de stored procedure a ejecutar y el nombre de los campos
        /// </summary>
        /// <param name="storedProcCommand">Stored proc. del catalogo. Debe regresar todos los registros de la tabla</param>
        /// <param name="idField">Nombre del campo id</param>
        /// <param name="codigoField">Nombre del campo Codigo</param>
        /// <param name="descField">Nombre del campo Descripcion</param>
        public CatalogosDataAccess2(CatalogoGenerico catalogo, Enums.Persona persona, string idField, string codigoExtField, string codigoIntField, string descField)
        {
            this.storedProc = StoredProcedureCommand[catalogo];
            this.idField = idField;
            this.codigoField = codigoExtField;
            this.codigoIntField = codigoIntField;
            this.descField = descField;
            this.persona = (persona == Enums.Persona.Moral) ? "M" : "F";

            InicializaFields(catalogo);
        }

        #endregion

        #region Datos

        public List<Catalogo> GetRegistros(bool soloActivos)
        {
            return GetRegistros(soloActivos, 0);
        }

        /// <summary>
        /// Regresa lista de registros del catalogo.
        /// </summary>
        /// <param name="soloActivos">Indica si debe regresar únicamente registros Activos.</param>
        /// <returns></returns>
        public List<Catalogo> GetRegistros(bool soloActivos, int id)
        {
           
            if (allCatalogos == null)
            {
                allCatalogos = new List<Catalogo>();

                DbCommand cmd = DB.GetStoredProcCommand(this.storedProc);

                if (setPersona)
                {
                    DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, persona);
                }

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {

                    while (reader.Read())
                    {
                        //los campos que siempre estan presentes
                        int idValue = Parser.ToNumber(reader[idField]);
                        string codigo = reader[codigoField].ToString();
                        char estatus = Parser.ToChar(reader[estatusField]);

                        //Los campos que pueden NO estar para ciertos stored procedures
                        string codigoInt = (String.IsNullOrWhiteSpace(codigoIntField) ? codigo : reader[codigoIntField].ToString());
                        string descripcion = (String.IsNullOrWhiteSpace(descField) ? String.Empty : reader[descField].ToString());
                        string abreviacion = (String.IsNullOrWhiteSpace(abrevacionField) ? String.Empty : reader[abrevacionField].ToString());

                        allCatalogos.Add(new Catalogo(id, codigo, codigoInt, descripcion, (estatus == '1' ? Enums.Estado.Activo : Enums.Estado.Inactivo)));
                    }
                }
            }

            if (soloActivos)
            {
                return allCatalogos
                        .Where(i => i.Estado.Equals(Enums.Estado.Activo))
                        .ToList();
            }

            return allCatalogos;
        }
        
        /// <summary>
        /// Obtiene un registro en especifico que busca por Id.
        /// </summary>
        /// <param name="id">Id del registro que se desea obtener</param>
        /// <returns>Registro del catalogo</returns>
        public Catalogo GetItem(int id)
        {
            return this.GetRegistros(false, id).FirstOrDefault();
            //List<Catalogo> items = GetRegistros(false);

            //var inst = items
            //            .Where(i => i.Id == id)
            //            .FirstOrDefault();

            //return inst;
        }

        /// <summary>
        /// Obtiene un registro especifico que busca por Codigo
        /// </summary>
        /// <param name="codigo">COdigo del registro que se desea obtener</param>
        /// <returns>Registro del catalogo</returns>
        public Catalogo GetItem(string codigo)
        {
            //Prioridad a los activos
            var toReturn = GetItem(codigo, true);

            if(toReturn != default(Catalogo))
                return toReturn;
            else
                return GetItem(codigo, false);  
        }

        private Catalogo GetItem(string codigo, bool soloActivos)
        {
            List<Catalogo> items = GetRegistros(soloActivos);

            var inst = items
                        .Where(i => i.CodigoExterno.Equals(codigo, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault();
            return inst;
        }

        #endregion

        private void InicializaFields(CatalogoGenerico catalogo)
        {
            //Descripcion:   La mayoría tiene descripción:
            if(noDescripcion.Contains(catalogo))
            {
                descField = String.Empty;
            }

            //Clave Interna     Inicializado = "Clave_Interna"
            if (noClaveInterna.Contains(catalogo)) 
            {
                codigoIntField = String.Empty;
            }

            //Persona   saber si el parametro pPersona debe agregarse!
            if (siPersona.Contains(catalogo))
            {
                setPersona = true;  
            }
        }

    }

}
