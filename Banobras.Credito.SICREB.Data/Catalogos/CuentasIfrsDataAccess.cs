using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using System.Data;

namespace Banobras.Credito.SICREB.Data.Catalogos
{
    public class CuentasIfrsDataAccess: CatalogoBase<CuentasIfrs, int>
    {
        public override string IdField { get { return "IdCuenta"; } } 
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Cuentas IFRS."; } }
        public override String ErrorInsert { get { return "1040 - Error al insertar nuevo registro en catálogo Cuentas IFRS."; } }
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Cuentas IFRS."; } }
        public override String ErrorDelete { get { return "1042 - Error al borrar el registro en catálogo Cuentas IFRS."; } }

        public CuentasIfrsDataAccess()
            : base()
        {
        }

        //public CuentasIfrsDataAccess(Enums.Persona pers)
        //    : base(pers)
        //{
        //}

        public override CuentasIfrs GetEntity(IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            int clasificacion = Parser.ToNumber(reader["ID_CLASIFICACION"]);
            string descripcionClasif = reader["descripcionClasif"].ToString();
            int grupo = Parser.ToNumber(reader["GRUPO"]);
            string codigo = reader["CODIGO"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();
            string sector = reader["SECTOR"].ToString();
            string rol = reader["ROL"].ToString();
            string formatoSic = reader["FORMATO_SIC"].ToString();
            string formatoSicofin = reader["FORMATO_SICOFIN"].ToString();
            Enums.Persona persona = Util.GetPersona(Parser.ToChar(reader["Persona"]));
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));
            string tipocuenta = reader["Tipo"].ToString();
                        
            CuentasIfrs cuenta = new CuentasIfrs(idValue, codigo, descripcion, sector, rol, clasificacion, descripcionClasif, formatoSic, formatoSicofin, persona, grupo, estado, tipocuenta);
            activo = (cuenta.Estatus == Enums.Estado.Activo);

            return cuenta;
        }

        public override string StoredProcedure
        {
            get { return "PACKCUENTAS_IFRS.SP_CATALOGOS_GetCuentasAct"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKCUENTAS_IFRS.SpUpdate"; }
        }
        public override string StoredProcedureInsert
        {
            get { return "PACKCUENTAS_IFRS.SpInsert"; }
        }
        public override string StoredProcedureDelete
        {
            get { return "PACKCUENTAS_IFRS.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, CuentasIfrs entityOld, CuentasIfrs entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityOld.Id);
            DB.AddInParameter(cmd, "pCODIGO", DbType.AnsiString, entityNew.Codigo.ToString());
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityNew.Descripcion.ToString());
            DB.AddInParameter(cmd, "pSECTOR", DbType.AnsiString, entityNew.Sector.ToString());
            DB.AddInParameter(cmd, "pROL", DbType.AnsiString, entityNew.Rol.ToString());
            DB.AddInParameter(cmd, "pID_CLASIFICACION", DbType.Int32, entityNew.IdClasificacion);
            DB.AddInParameter(cmd, "pFORMATO_SIC", DbType.AnsiString, entityNew.FormatSic);
            DB.AddInParameter(cmd, "pFORMATO_SICOFIN", DbType.AnsiString, entityNew.FormatSicofin);
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityNew.Estatus.ToString()));
            DB.AddInParameter(cmd, "pPERSONA", DbType.AnsiString, Util.SetPersona(entityNew.Persona.ToString()));
            DB.AddInParameter(cmd, "pGRUPO", DbType.AnsiString, entityNew.Grupo);
            DB.AddInParameter(cmd, "pTipoCuenta", DbType.AnsiString, entityNew.TipoCuenta);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }   

    }
}
