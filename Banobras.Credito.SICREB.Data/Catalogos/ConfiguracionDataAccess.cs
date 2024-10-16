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
    public class ConfiguracionDataAccess : CatalogoBase<Configuracion, int>
    {


        public override string IdField { get { return "Id"; } }
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Configuración Act."; } }
        public override String ErrorInsert { get { return "1040 - Error al insertar nuevo registro en catálogo Configuración Act."; } }
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Configuración Act."; } }
        public override String ErrorDelete { get { return "1042 - Error al borrar el registro en catálogo Configuración Act."; } }


        private const string AGREGA = "PACCONFIG.SpInsert";

        public ConfiguracionDataAccess()
            : base()
        {

        }

        public override Configuracion GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            Configuracion conf = new Configuracion(1, 0, 0, 1, 0);
            activo = true;
            return conf;
        }

        public override string StoredProcedure
        {
            get { return ""; }
        }

        public override string StoredProcedureUpdate
        {
            get { return ""; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACCONFIG.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return ""; }
        }

        public string StoredProcedureSelect
        {
            get { return "PACCONFIG.SP_SELECT_CASE"; }
        }

        public Configuracion ObtenerConfiguracion(String datos)
        {
            Configuracion toReturn = new Configuracion();
            DbCommand cmd = DB.GetStoredProcCommand(StoredProcedureSelect);
            DB.AddInParameter(cmd, "pPalabra", DbType.String, datos);

            using (IDataReader reader = DB.ExecuteReader(cmd))
            {

                //recorres los resultados
                while (reader.Read())
                {
                    toReturn = GetEntity(reader);
                }

            }
            return toReturn;

        }

        public Configuracion GetEntity(IDataReader reader)
        {

            int Id = Convert.ToInt32(reader["ID"]);
            int Catsicofin = Convert.ToInt32(reader["CATSICOFIN"].ToString());
            int Catsic = Convert.ToInt32(reader["CATSIC"].ToString());
            int IdUsuario = Convert.ToInt32(reader["IDUSUARIO"]);
            DateTime fecha = Convert.ToDateTime(reader["FECHA_ACTUALIZACION"]);
            int Calificacion = Convert.ToInt32(reader["CALIFICACION"].ToString());

            Configuracion config = new Configuracion(Id, Catsicofin, Catsic, IdUsuario, Calificacion);

            return config;
        }



        public override void SetEntity(DbCommand cmd, Configuracion entityOld, Configuracion entityNew)
        {

        }

        public override void SetEntityDelete(DbCommand cmd, Configuracion entityToDelete)
        {

        }

        public override void SetEntityInsert(DbCommand cmd, Configuracion entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pSICOFIN", DbType.AnsiString, entityToInsert.Catsicofin.ToString());
            DB.AddInParameter(cmd, "pSIC", DbType.AnsiString, entityToInsert.Catsic.ToString());
            DB.AddInParameter(cmd, "pCALIFIC", DbType.AnsiString, entityToInsert.Calificacion.ToString());
            DB.AddInParameter(cmd, "IdUsuario", DbType.Int32, entityToInsert.IdUsuario);

            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }



        public static bool insertConfiguracion(int pSICOFIN, int pSIC, int pCALIFIC, int IdUsuario)
        {
            bool resp;
            try
            {

                DbCommand cmd = DB.GetStoredProcCommand(ConfiguracionDataAccess.AGREGA);
                DB.AddInParameter(cmd, "pSICOFIN", System.Data.DbType.AnsiString, pSICOFIN);
                DB.AddInParameter(cmd, "pSIC", System.Data.DbType.AnsiString, pSIC);
                DB.AddInParameter(cmd, "pCALIFIC", System.Data.DbType.AnsiString, pCALIFIC);
                DB.AddInParameter(cmd, "IdUsuario", System.Data.DbType.Int32, IdUsuario);


                DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
                DB.ExecuteNonQuery(cmd);
                if ((int)DB.GetParameterValue(cmd, "return") > 0)
                    resp = true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                resp = false;
            }

            return resp;
        }


    }
}
