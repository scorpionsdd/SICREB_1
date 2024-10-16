using System;
using System.Data;
using System.Data.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class DiaDataAccess : CatalogoBase<Dia, String>
    {

        //variables a emplear
        System.Globalization.CultureInfo CultureMX = new System.Globalization.CultureInfo("es-MX", true);

        public override string IdField { get { return "IdDia"; } }
        public override String ErrorGet { get { return " 1080 - Error al cargar los datos de la base de datos en catálogo Dias inhabiles."; } }
        public override String ErrorInsert { get { return "1081 - Error al insertar nuevo registro en catálogo Dias inhabiles."; } }
        public override String ErrorUpdate { get { return "1082 - Error al actualizar registro en catálogo Dias inhabiles."; } }
        public override String ErrorDelete { get { return "1083 - Error al borrar el registro en catálogo Dias inhabiles."; } }

        public override Dia GetEntity(IDataReader reader, out bool activo)
        {
            int idDia = Parser.ToNumber(reader["ID"]);
            string idententicadorDia = reader["DESCRIPCION"].ToString();
            DateTime dtFecha = DateTime.Parse(reader["FECHA"].ToString(), CultureMX);
            string fecha = dtFecha.ToLongDateString();
            DateTime dtFechaInhabil = DateTime.Parse(reader["FECHAINHABIL"].ToString(), CultureMX);
            string fechaInhabil = dtFechaInhabil.ToLongDateString();            
            //Dia dia = new Dia(idDia, idententicadorDia, fecha, fechaInhabil);
            Dia dia = new Dia(idDia, idententicadorDia, dtFecha, dtFechaInhabil);
            activo = true;

            return dia;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetDiaInhabil"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "CATALOGOS.F_CATALOGOS_UpdDiaInhabil"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "CATALOGOS.F_CATALOGOS_DelDiaInhabil"; }            
        }

        public override string StoredProcedureInsert
        {
            get { return "CATALOGOS.F_CATALOGOS_InsDiaInhabil"; }
        }

        public override void SetEntity(DbCommand cmd, Dia entityOld, Dia entityNew)
        {
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pIdententicadorDia", DbType.String, entityNew.IdententicadorDia);
            DB.AddInParameter(cmd, "pdtFecha", DbType.Date, entityNew.dtFecha.ToShortDateString());
            DB.AddInParameter(cmd, "pdtFechaInhabil", DbType.Date, entityNew.dtFechaInhabil.ToShortDateString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Dia entityToDelete)
        {
            DB.AddInParameter(cmd, "pId", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Dia entityToInsert)
        {
            DB.AddInParameter(cmd, "pIdententicadorDia", DbType.String, entityToInsert.IdententicadorDia);
            DB.AddInParameter(cmd, "pdtFecha", DbType.Date, entityToInsert.dtFecha.ToShortDateString());
            DB.AddInParameter(cmd, "pdtFechaInhabil", DbType.Date, entityToInsert.dtFechaInhabil.ToShortDateString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
