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

    public class BonoCuponCeroDataAccess : CatalogoBase<BonoCuponCero, string>
    {
        private Enums.Persona pers = Enums.Persona.Moral;
        public override string IdField { get { return "IdCupon"; } }
        public override String ErrorGet { get { return "1003 - Error al cargar los datos de la base de datos en catálogo Bono Cupon Cero."; } }
        public override String ErrorInsert { get { return "1004 - Error al insertar nuevo registro en catálogo Bono Cupon Cero."; } }
        public override String ErrorUpdate { get { return "1005 - Error al actualizar registro en catálogo Bono Cupon Cero."; } }
        public override String ErrorDelete { get { return "1006 - Error al borrar el registro en catálogo Bono Cupon Cero."; } }

        public BonoCuponCeroDataAccess(Enums.Persona persona)
            : base(persona)
        {
            this.pers = persona;
        }

        public override BonoCuponCero GetEntity(System.Data.IDataReader reader, out bool activo)
        {

            int id = Parser.ToNumber(reader["ID"]);
            string Credito = reader["CREDITO"].ToString().Trim();
            string rfc = reader["RFC"].ToString().Trim();
            string NombreAcreditado = reader["NOMBRE_ACREDITADO"].ToString().Trim();
            double MontoInversion = Parser.ToDouble( reader["MONTO_INVERSION"].ToString().Trim() );
            Enums.Estado Estatus = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));

            BonoCuponCero bcc = new BonoCuponCero(id, Credito, rfc, NombreAcreditado, MontoInversion, Estatus);
            activo = bcc.Estatus == Enums.Estado.Activo;
            return bcc;
        }

        public List<BonoCuponCero> GetCupoCeroPorCredito(string numCredito, bool soloActivos)
        {
            List<BonoCuponCero> toReturn = new List<BonoCuponCero>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);

                DB.AddInParameter(cmd, "pCredito", DbType.AnsiString, numCredito);
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (pers == Enums.Persona.Moral ? 'M' : 'F'));

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        BonoCuponCero entity = GetEntity(reader, out activo);

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
                throw new Exception("Error al Obtener los Datos del Bono Cupon Cero por Credito", ex);
            }
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetBonoCuponCero"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKCUPONCERO.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCUPONCERO.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCUPONCERO.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, BonoCuponCero entityOld, BonoCuponCero entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityNew.Rfc);
            DB.AddInParameter(cmd, "pNOMBRE_ACREDITADO", DbType.AnsiString, entityNew.NombreAcreditado);
            DB.AddInParameter(cmd, "pMONTO_INVERSION", DbType.Double, entityNew.MontoInversion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, BonoCuponCero entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToDelete.Credito);
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityToDelete.Rfc);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, BonoCuponCero entityToInsert)
        {
            //parametros para insertar
            DB.AddInParameter(cmd, "pCREDITO", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityToInsert.Rfc);
            DB.AddInParameter(cmd, "pNOMBRE_ACREDITADO", DbType.AnsiString, entityToInsert.NombreAcreditado);
            DB.AddInParameter(cmd, "pMONTO_INVERSION", DbType.Double, entityToInsert.MontoInversion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
