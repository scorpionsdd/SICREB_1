﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities;
using System.Data.Common;
using System.Data;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{
    public class CruceUsuarioRolDataAccess : CatalogoBase<CruceUsuarioRol, int>
    {
        public CruceUsuarioRolDataAccess(string Login)
            : base(Login)
        { 
        
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetUseRol"; }
        }

        public override string IdField
        {
            get { return "pLogin"; }
        }

        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Cuentas."; } }
        public override String ErrorInsert { get { return "0000 - No Aplica."; } }
        public override String ErrorUpdate { get { return "0000 - No Aplica."; } }
        public override String ErrorDelete { get { return "0000 -No Aplica."; } }

        public override CruceUsuarioRol GetEntity(IDataReader reader, out bool activo)
        {

            int idValue = Parser.ToNumber(reader["ID"]);
            int idRol = Parser.ToNumber(reader["ID_ROL"]);
            int idUsuario = Parser.ToNumber(reader["ID_USUARIO"]);
            string login = reader["LOGIN"].ToString();
            string rol = reader["ROL"].ToString();

            CruceUsuarioRol usuarioRol = new CruceUsuarioRol(idValue, idRol, idUsuario, login, rol);
            activo = true;

            return usuarioRol;
        }

        public override string StoredProcedureUpdate 
        {
            get { return " NO APLICA "; }
        }
        public override string StoredProcedureInsert
        {
            get { return " NO APLICA "; }
        }
        public override string StoredProcedureDelete
        {
            get { return " NO APLICA "; }
        }
        public override void SetEntity(DbCommand cmd, CruceUsuarioRol entityOld, CruceUsuarioRol entityNew)
        {
            //NO APLICA
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }
        public override void SetEntityDelete(DbCommand cmd, CruceUsuarioRol entityToDelete)
        {
            //NO APLICA
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }
        public override void SetEntityInsert(DbCommand cmd, CruceUsuarioRol entityToInsert)
        {
            //NO APLICA
            DB.AddInParameter(cmd, "pId", DbType.AnsiString, entityToInsert.Id);
        }
    }
}
