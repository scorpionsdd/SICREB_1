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

    public class AccionistaDataAccess : CatalogoBase<Accionista, string>
    {

        private Enums.Persona pers = Enums.Persona.Moral;
        public override string IdField{ get { return "rfcaccionista"; } }
        public override String ErrorGet { get { return "1003 - Error al cargar los datos de la base de datos en catálogo Accionistas."; } }
        public override String ErrorInsert { get { return "1004 -  Error al insertar nuevo registro en catálogo Accionistas."; } }
        public override String ErrorUpdate { get { return "1005 - Error al actualizar registro en catálogo Accionistas."; } }
        public override String ErrorDelete { get { return "1006 - Error al borrar el registro en catálogo Accionistas."; } }

        public AccionistaDataAccess(Enums.Persona persona)
            : base(persona)
        {
            this.pers = persona;
        }

        public override Accionista GetEntity(System.Data.IDataReader reader, out bool activo)
        {

            int id = Parser.ToNumber(reader["ID"]);
            string rfcAcreditado = reader["RFC_ACREDITADO"].ToString().Trim();
            string rfcAccionista = reader["RFC_ACCIONISTA"].ToString().Trim();
            string NombreCompania = reader["NOMBRE_COMPANIA"].ToString().Trim();
            string Nombre = reader["NOMBRE"].ToString().Trim();
            string SNombre = reader["SEGUNDO_NOMBRE"].ToString().Trim();
            string ApellidoP = reader["APELLIDO_PATERNO"].ToString().Trim();
            string ApellidoM = reader["APELLIDO_MATERNO"].ToString().Trim();
            int Porcentaje = AjustarPorcentaje(reader["PORCENTAJE_PARTICIPACION"].ToString().Trim());
            string Direccion = reader["DIRECCION"].ToString().Trim();
            string ColoniaPoblacion = reader["COLONIA_POBLACION"].ToString().Trim();
            string DelegacionMunicipio = reader["DELEGACION_MUNICIPIO"].ToString().Trim();
            string Ciudad = reader["CIUDAD"].ToString().Trim();
            string EstadoMexico = reader["ESTADO_MEXICO"].ToString().Trim();
            string CodigoPostal = reader["CODIGO_POSTAL"].ToString().Trim();
            Enums.Persona Persona = Util.GetPersona(Parser.ToChar(reader["PERSONA"]));
            string EstadoExtranjero = reader["ESTADO_EXTRANJERO"].ToString().Trim();
            string PaisOrigenDomicilio = reader["PAIS_ORIGEN_DOMICILIO"].ToString().Trim();
            Enums.Estado Estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            Accionista acc = new Accionista(id, rfcAcreditado, rfcAccionista, NombreCompania, Nombre, SNombre, ApellidoP, ApellidoM, Porcentaje, Direccion, ColoniaPoblacion, DelegacionMunicipio, Ciudad, EstadoMexico, CodigoPostal, Persona, EstadoExtranjero, PaisOrigenDomicilio, Estatus);
            activo = acc.Estatus == Enums.Estado.Activo;
            return acc;
        }

        public List<Accionista> GetAccionistaPorCompania(string rfcAcreditado, bool soloActivos)
        {
            List<Accionista> toReturn = new List<Accionista>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);

                DB.AddInParameter(cmd, "RFCEmpresa", DbType.AnsiString, rfcAcreditado);
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (pers == Enums.Persona.Moral ? 'M' : 'F'));

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        Accionista entity = GetEntity(reader, out activo);

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
                throw new Exception("Error al Obtener los Datos del Accionista por Compañia", ex);
            }
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetAccionistas"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKACCIONISTAS.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKACCIONISTAS.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKACCIONISTAS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, Accionista entityOld, Accionista entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityNew.RfcAcreditado);
            DB.AddInParameter(cmd, "pRFC_ACCIONISTA", DbType.AnsiString, entityNew.RfcAccionista);
            DB.AddInParameter(cmd, "pNOMBRE_COMPANIA", DbType.AnsiString, entityNew.NombreCompania);
            DB.AddInParameter(cmd, "pNOMBRE", DbType.AnsiString, entityNew.Nombre);
            DB.AddInParameter(cmd, "pSEGUNDO_NOMBRE", DbType.AnsiString, entityNew.SNombre);
            DB.AddInParameter(cmd, "pAPELLIDO_PATERNO", DbType.AnsiString, entityNew.ApellidoP);
            DB.AddInParameter(cmd, "pAPELLIDO_MATERNO", DbType.AnsiString, entityNew.ApellidoM);
            DB.AddInParameter(cmd, "pPORCENTAJE_PARTICIPACION", DbType.Int32, entityNew.Porcentaje);
            DB.AddInParameter(cmd, "pDIRECCION", DbType.AnsiString, entityNew.Direccion);
            DB.AddInParameter(cmd, "pCOLONIA_POBLACION", DbType.AnsiString, entityNew.ColoniaPoblacion);
            DB.AddInParameter(cmd, "pDELEGACION_MUNICIPIO", DbType.AnsiString, entityNew.DelegacionMunicipio);
            DB.AddInParameter(cmd, "pCIUDAD", DbType.AnsiString, entityNew.Ciudad);
            DB.AddInParameter(cmd, "pESTADO_MEXICO", DbType.AnsiString, entityNew.EstadoMexico);
            DB.AddInParameter(cmd, "pCODIGO_POSTAL", DbType.AnsiString, entityNew.CodigoPostal);
            DB.AddInParameter(cmd, "pPERSONA", DbType.AnsiString, Util.SetPersona(entityNew.Persona.ToString()));
            DB.AddInParameter(cmd, "pESTADO_EXTRANJERO", DbType.AnsiString, entityNew.EstadoExtranjero);
            DB.AddInParameter(cmd, "pPAIS_ORIGEN_DOMICILIO", DbType.AnsiString, entityNew.PaisOrigenDomicilio);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Accionista entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityToDelete.RfcAcreditado);
            DB.AddInParameter(cmd, "pRFC_ACCIONISTA", DbType.AnsiString, entityToDelete.RfcAccionista);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Accionista entityToInsert)
        {
            //parametros para insertar
            DB.AddInParameter(cmd, "pRFC_ACREDITADO", DbType.AnsiString, entityToInsert.RfcAcreditado);
            DB.AddInParameter(cmd, "pRFC_ACCIONISTA", DbType.AnsiString, entityToInsert.RfcAccionista);
            DB.AddInParameter(cmd, "pNOMBRE_COMPANIA", DbType.AnsiString, entityToInsert.NombreCompania);
            DB.AddInParameter(cmd, "pNOMBRE", DbType.AnsiString, entityToInsert.Nombre);
            DB.AddInParameter(cmd, "pSEGUNDO_NOMBRE", DbType.AnsiString, entityToInsert.SNombre);
            DB.AddInParameter(cmd, "pAPELLIDO_PATERNO", DbType.AnsiString, entityToInsert.ApellidoP);
            DB.AddInParameter(cmd, "pAPELLIDO_MATERNO", DbType.AnsiString, entityToInsert.ApellidoM);
            DB.AddInParameter(cmd, "pPORCENTAJE_PARTICIPACION", DbType.Int32, entityToInsert.Porcentaje);
            DB.AddInParameter(cmd, "pDIRECCION", DbType.AnsiString, entityToInsert.Direccion);
            DB.AddInParameter(cmd, "pCOLONIA_POBLACION", DbType.AnsiString, entityToInsert.ColoniaPoblacion);
            DB.AddInParameter(cmd, "pDELEGACION_MUNICIPIO", DbType.AnsiString, entityToInsert.DelegacionMunicipio);
            DB.AddInParameter(cmd, "pCIUDAD", DbType.AnsiString, entityToInsert.Ciudad);
            DB.AddInParameter(cmd, "pESTADO_MEXICO", DbType.AnsiString, entityToInsert.EstadoMexico);
            DB.AddInParameter(cmd, "pCODIGO_POSTAL", DbType.AnsiString, entityToInsert.CodigoPostal);
            DB.AddInParameter(cmd, "pPERSONA", DbType.AnsiString, Util.SetPersona(entityToInsert.Persona.ToString()));
            DB.AddInParameter(cmd, "pESTADO_EXTRANJERO", DbType.AnsiString, entityToInsert.EstadoExtranjero);
            DB.AddInParameter(cmd, "pPAIS_ORIGEN_DOMICILIO", DbType.AnsiString, entityToInsert.PaisOrigenDomicilio);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        private int AjustarPorcentaje(string CadenaPorcentaje)
        {
            int Porcentaje = 0;
            decimal NumDecimal = Convert.ToDecimal(CadenaPorcentaje);
            Porcentaje = Decimal.ToInt32(Math.Round(NumDecimal, 0));

            // Por regla solo se pueden dos digitos por lo que el 100 pasa a ser 99
            if (Porcentaje == 100)
            { Porcentaje = 99; }

            return Porcentaje;
        }

    }

}
