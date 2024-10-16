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

    public class PaisesDataAccess : CatalogoBase<Pais, int>
    {

        public override string IdField { get { return "CATALOGOS.IdPais"; } }
        public override String ErrorGet { get { return " 1068 - Error al cargar los datos de la base de datos en catálogo Paises."; } }
        public override String ErrorInsert { get { return "1069 - Error al insertar nuevo registro en catálogo Paises."; } }
        public override String ErrorUpdate { get { return "1070 - Error al actualizar registro en catálogo Paises."; } }
        public override String ErrorDelete { get { return "1071 - Error al borrar el registro en catálogo Paises."; } }

        public PaisesDataAccess(Enums.Persona pers)
            : base(pers)
        {
        }

        public override Pais GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            int claveSIC = Parser.ToNumber(reader["CLAVE_SIC"]);
            string claveBuro = reader["CLAVE_BURO"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();

            Enums.Persona persona = Util.GetPersona(Parser.ToChar(reader["Persona"]));
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            Pais pais = new Pais(idValue, claveSIC, claveBuro, descripcion, persona, estado);
            activo = (pais.Estatus == Enums.Estado.Activo);

            return pais;
        }

        public List<Pais> GetPaisPorClaveBuro(Enums.Persona Persona, string IdPais, string ClaveBuro, bool soloActivos)
        {
            List<Pais> toReturn = new List<Pais>();
            try
            {
                string pPersona = (Persona == Enums.Persona.Moral) ? "M" : "F";

                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);
                DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, pPersona);
                DB.AddInParameter(cmd, "IdPais", DbType.AnsiString, IdPais);
                DB.AddInParameter(cmd, "Clave", DbType.AnsiString, ClaveBuro);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        Pais entity = GetEntity(reader, out activo);

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
                throw new Exception("Error al obtener los datos del Pais por Clave del Buro", ex);
            }
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetClavesPais"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "CATALOGOS.F_CATALOGOS_UpdNacional"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "CATALOGOS.F_CATALOGOS_InsNacional"; }

        }

        public override string StoredProcedureDelete
        {
            get { return "CATALOGOS.F_CATALOGOS_DelNacional"; }
        }

        public override void SetEntity(DbCommand cmd, Pais entityOld, Pais entityNew)
        {
            DB.AddInParameter(cmd, "idnacionalidad", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pclaveburo", DbType.AnsiString, entityNew.ClaveBuro);
            DB.AddInParameter(cmd, "pclavesic", DbType.AnsiString, entityNew.ClaveSIC);
            DB.AddInParameter(cmd, "pdescripcion", DbType.AnsiString, entityNew.Descripcion);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Pais entityToDelete)
        {
            DB.AddInParameter(cmd, "idnacionalidad", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Pais entityToInsert)
        {
            DB.AddInParameter(cmd, "pclaveburo", DbType.String, entityToInsert.ClaveBuro);
            DB.AddInParameter(cmd, "pclavesic", DbType.Int32, entityToInsert.ClaveSIC);
            DB.AddInParameter(cmd, "pdescripcion", DbType.String, entityToInsert.Descripcion);
            DB.AddInParameter(cmd, "ppersona", DbType.String,Util.SetPersona(entityToInsert.TipoPersona.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
