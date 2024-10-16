using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class AvalDataAccess : CatalogoBase<Aval, string>
    {

        private Enums.Persona pers = Enums.Persona.Moral;
        public override string IdField { get { return "rfcAval"; } }
        public override String ErrorGet { get { return "1003 - Error al cargar los datos de la base de datos en catálogo Avales."; } }
        public override String ErrorInsert { get { return "1004 - Error al insertar nuevo registro en catálogo Avales."; } }
        public override String ErrorUpdate { get { return "1005 - Error al actualizar registro en catálogo Avales."; } }
        public override String ErrorDelete { get { return "1006 - Error al borrar el registro en catálogo Avales."; } }

        public AvalDataAccess(Enums.Persona persona)
            : base(persona)
        {
            this.pers = persona;
        }

        public override Aval GetEntity(System.Data.IDataReader reader, out bool activo)
        {

            int id = Parser.ToNumber(reader["ID"]);
            string Credito = reader["CREDITO"].ToString().Trim();
            string rfcAcreditado = reader["RFC_ACREDITADO"].ToString().Trim();
            string rfcAval = reader["RFC_AVAL"].ToString().Trim();
            string NombreCompania = reader["NOMBRE_COMPANIA"].ToString().Trim();
            string Nombre = reader["NOMBRE"].ToString().Trim();
            string SNombre = reader["SEGUNDO_NOMBRE"].ToString().Trim();
            string ApellidoP = reader["APELLIDO_PATERNO"].ToString().Trim();
            string ApellidoM = reader["APELLIDO_MATERNO"].ToString().Trim();
            string Direccion = reader["DIRECCION"].ToString().Trim();
            string ColoniaPoblacion = reader["COLONIA_POBLACION"].ToString().Trim();
            string DelegacionMunicipio = reader["DELEGACION_MUNICIPIO"].ToString().Trim();
            string Ciudad = reader["CIUDAD"].ToString().Trim();
            string EstadoMexico = reader["ESTADO_MEXICO"].ToString().Trim();
            string CodigoPostal = reader["CODIGO_POSTAL"].ToString().Trim();
            Enums.Persona TipoAval = Util.GetPersona(Parser.ToChar(reader["TIPO_AVAL"]));
            string EstadoExtranjero = reader["ESTADO_EXTRANJERO"].ToString().Trim();
            string PaisOrigenDomicilio = reader["PAIS_ORIGEN_DOMICILIO"].ToString().Trim();
            Enums.TipoOperacion TipoOperacion = Util.GetTipoOperacion(Parser.ToChar(reader["TIPO_OPERACION"]));
            Enums.Estado Estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            Aval acc = new Aval(id, Credito, rfcAcreditado, rfcAval, NombreCompania, Nombre, SNombre, ApellidoP, ApellidoM, Direccion, ColoniaPoblacion, DelegacionMunicipio, Ciudad, EstadoMexico, CodigoPostal, TipoAval, EstadoExtranjero, PaisOrigenDomicilio, TipoOperacion, Estatus);
            activo = acc.Estatus == Enums.Estado.Activo;
            return acc;
        }

        public List<Aval> GetAvalPorCredito(string numCredito, bool soloActivos)
        {
            List<Aval> toReturn = new List<Aval>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);

                DB.AddInParameter(cmd, "Creditos", DbType.AnsiString, numCredito);
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (pers == Enums.Persona.Moral ? 'M' : 'F'));

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        Aval entity = GetEntity(reader, out activo);

                        //si se requieren todos O (si solo los activos y el registro actual es activo).. lo agregamos
                        if (!soloActivos || activo)
                        {
                            toReturn.Add(entity);
                        }
                    }
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener los Datos del Aval por Credito", ex);
            }
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetAvales"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKAVALES.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKAVALES.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKAVALES.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, Aval entityOld, Aval entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityNew.RfcAcreditado);
            DB.AddInParameter(cmd, "pRFC_AVAL", DbType.AnsiString, entityNew.RfcAval);
            DB.AddInParameter(cmd, "pNOMBRE_COMPANIA", DbType.AnsiString, entityNew.NombreCompania);
            DB.AddInParameter(cmd, "pNOMBRE", DbType.AnsiString, entityNew.Nombre);
            DB.AddInParameter(cmd, "pSEGUNDO_NOMBRE", DbType.AnsiString, entityNew.SNombre);
            DB.AddInParameter(cmd, "pAPELLIDO_PATERNO", DbType.AnsiString, entityNew.ApellidoP);
            DB.AddInParameter(cmd, "pAPELLIDO_MATERNO", DbType.AnsiString, entityNew.ApellidoM);
            DB.AddInParameter(cmd, "pDIRECCION", DbType.AnsiString, entityNew.Direccion);
            DB.AddInParameter(cmd, "pCOLONIA_POBLACION", DbType.AnsiString, entityNew.ColoniaPoblacion);
            DB.AddInParameter(cmd, "pDELEGACION_MUNICIPIO", DbType.AnsiString, entityNew.DelegacionMunicipio);
            DB.AddInParameter(cmd, "pCIUDAD", DbType.AnsiString, entityNew.Ciudad);
            DB.AddInParameter(cmd, "pESTADO_MEXICO", DbType.AnsiString, entityNew.EstadoMexico);
            DB.AddInParameter(cmd, "pCODIGO_POSTAL", DbType.AnsiString, entityNew.CodigoPostal);
            DB.AddInParameter(cmd, "pTIPO_AVAL", DbType.AnsiString, Util.SetPersona(entityNew.TipoAval.ToString()));
            DB.AddInParameter(cmd, "pESTADO_EXTRANJERO", DbType.AnsiString, entityNew.EstadoExtranjero);
            DB.AddInParameter(cmd, "pPAIS_ORIGEN_DOMICILIO", DbType.AnsiString, entityNew.PaisOrigenDomicilio);
            DB.AddInParameter(cmd, "pTIPO_OPERACION", DbType.AnsiString, Util.SetTipoOperacion(entityNew.TipoOperacion.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Aval entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToDelete.Credito);
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityToDelete.RfcAcreditado);
            DB.AddInParameter(cmd, "pRFC_AVAL", DbType.AnsiString, entityToDelete.RfcAval);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Aval entityToInsert)
        {
            //parametros para insertar
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityToInsert.RfcAcreditado);
            DB.AddInParameter(cmd, "pRFC_AVAL", DbType.AnsiString, entityToInsert.RfcAval);
            DB.AddInParameter(cmd, "pNOMBRE_COMPANIA", DbType.AnsiString, entityToInsert.NombreCompania);
            DB.AddInParameter(cmd, "pNOMBRE", DbType.AnsiString, entityToInsert.Nombre);
            DB.AddInParameter(cmd, "pSEGUNDO_NOMBRE", DbType.AnsiString, entityToInsert.SNombre);
            DB.AddInParameter(cmd, "pAPELLIDO_PATERNO", DbType.AnsiString, entityToInsert.ApellidoP);
            DB.AddInParameter(cmd, "pAPELLIDO_MATERNO", DbType.AnsiString, entityToInsert.ApellidoM);
            DB.AddInParameter(cmd, "pDIRECCION", DbType.AnsiString, entityToInsert.Direccion);
            DB.AddInParameter(cmd, "pCOLONIA_POBLACION", DbType.AnsiString, entityToInsert.ColoniaPoblacion);
            DB.AddInParameter(cmd, "pDELEGACION_MUNICIPIO", DbType.AnsiString, entityToInsert.DelegacionMunicipio);
            DB.AddInParameter(cmd, "pCIUDAD", DbType.AnsiString, entityToInsert.Ciudad);
            DB.AddInParameter(cmd, "pESTADO_MEXICO", DbType.AnsiString, entityToInsert.EstadoMexico);
            DB.AddInParameter(cmd, "pCODIGO_POSTAL", DbType.AnsiString, entityToInsert.CodigoPostal);
            DB.AddInParameter(cmd, "pTIPO_AVAL", DbType.AnsiString, Util.SetPersona(entityToInsert.TipoAval.ToString()));
            DB.AddInParameter(cmd, "pESTADO_EXTRANJERO", DbType.AnsiString, entityToInsert.EstadoExtranjero);
            DB.AddInParameter(cmd, "pPAIS_ORIGEN_DOMICILIO", DbType.AnsiString, entityToInsert.PaisOrigenDomicilio);
            DB.AddInParameter(cmd, "pTIPO_OPERACION", DbType.AnsiString, Util.SetTipoOperacion(entityToInsert.TipoOperacion.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
