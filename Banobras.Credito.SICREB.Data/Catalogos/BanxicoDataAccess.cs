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

    public class BanxicoDataAccess : CatalogoBase<Banxico, int>
    {

        private List<BanxicoTipo> tipos = null;
        public override string IdField { get { return "IdBanxico"; } }
        public override string ClaveField{ get { return "claveBanxico"; } }
        public override String ErrorGet { get { return "1011 - Error al cargar los datos de la base de datos en catálogo Banxico."; } }
        public override String ErrorInsert { get { return "1012 - Error al insertar nuevo registro en catálogo Banxico."; } }
        public override String ErrorUpdate { get { return "1013 - Error al actualizar registro en catálogo Banxico."; } }
        public override String ErrorDelete { get { return "1014 - Error al borrar el registro en catálogo Banxico."; } }

        public BanxicoDataAccess()
            : base()
        {
            BanxicoTiposDataAccess tiposData = new BanxicoTiposDataAccess();
            tipos = tiposData.GetRecords(true);

            if (tipos == null)
                tipos = new List<BanxicoTipo>();
        }

        public override Banxico GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            int claveCLIC = Parser.ToNumber(reader["ID_CLIC"]);           
            int claveBuro = Parser.ToNumber(reader["CLAVE"]);
            string actividad = reader["ACTIVIDAD"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));
            int tipoID = Parser.ToNumber(reader["IDTIPO"]);

            BanxicoTipo tipo;

            if (tipos.Where(t => t.Id == tipoID).SingleOrDefault() == null)
            {
                tipo = new BanxicoTipo(tipoID, "", Enums.Estado.Inactivo);
            }
            else
            {
                 tipo = tipos.Where(t => t.Id == tipoID).SingleOrDefault();
            }            
           
            Banxico ban = new Banxico(idValue, claveCLIC, claveBuro, actividad, tipo, estado);
            activo = (ban.Estatus == Enums.Estado.Activo);

            return ban;
        }
        
        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetClavesBanxico"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKBANXICO.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKBANXICO.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKBANXICO.SpDelete"; }
        }
        
        public override void SetEntity(DbCommand cmd, Banxico entityOld, Banxico entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCLAVE", DbType.Int32, entityNew.ClaveBuro);
            DB.AddInParameter(cmd, "pACTIVIDAD", DbType.AnsiString, entityNew.Actividad);
            DB.AddInParameter(cmd, "pIDTIPO", DbType.Int32, entityNew.Tipo.Id);
            DB.AddInParameter(cmd, "pID_CLIC", DbType.Int32, entityNew.ClaveCLIC);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Banxico entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Banxico entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pCLAVE", DbType.Int32, entityToInsert.ClaveBuro);
            DB.AddInParameter(cmd, "pACTIVIDAD", DbType.AnsiString, entityToInsert.Actividad);
            DB.AddInParameter(cmd, "pIDTIPO", DbType.Int32, entityToInsert.Tipo.Id);
            DB.AddInParameter(cmd, "pID_CLIC", DbType.Int32, entityToInsert.ClaveCLIC);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }
       
    }

}
