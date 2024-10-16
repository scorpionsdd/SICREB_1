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
    public class CuentasActDataAccess : CatalogoBase<CuentasAct, int>
    {

        //MASS 14/06/13 se integra Act en los mensajes
        public override string IdField{ get { return "IdCuenta"; } }
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Cuentas Act."; } }
        public override String ErrorInsert { get { return "1040 - Error al insertar nuevo registro en catálogo Cuentas Act."; } }
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Cuentas Act."; } }
        public override String ErrorDelete { get { return "1042 - Error al borrar el registro en catálogo Cuentas Act."; } }

        public CuentasActDataAccess(Enums.Persona pers)
            : base(pers)
        {

        }

        public override CuentasAct GetEntity(IDataReader reader, out bool activo)
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

            //MASS 14/06/13            
            CuentasAct cuenta = new CuentasAct(idValue, codigo, descripcion, sector, rol, clasificacion, descripcionClasif, formatoSic, formatoSicofin, persona, grupo, estado, tipocuenta);
            activo = (cuenta.Estatus == Enums.Estado.Activo);

            return cuenta;
        }

        public override string StoredProcedure
        {          
            get { return "PACKCUENTAS_ACT.SP_CATALOGOS_GetCuentasAct"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return "PACKCUENTAS_ACT.SpUpdate"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PACKCUENTAS_ACT.SpInsert"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PACKCUENTAS_ACT.SpDelete"; }
        }

        public override void SetEntity(DbCommand cmd, CuentasAct entityOld, CuentasAct entityNew)
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

        public override void SetEntityDelete(DbCommand cmd, CuentasAct entityToDelete)
        {
            //parametros para especificar el borrado 
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, CuentasAct entityToInsert)
        {
            //parametros para insertar 
            DB.AddInParameter(cmd, "pCODIGO", DbType.AnsiString, entityToInsert.Codigo.ToString());
            DB.AddInParameter(cmd, "pDESCRIPCION", DbType.AnsiString, entityToInsert.Descripcion.ToString());
            DB.AddInParameter(cmd, "pSECTOR", DbType.AnsiString, entityToInsert.Sector.ToString());
            DB.AddInParameter(cmd, "pROL", DbType.AnsiString, entityToInsert.Rol.ToString());
            DB.AddInParameter(cmd, "pID_CLASIFICACION", DbType.Int32, Parser.ToNumber(entityToInsert.IdClasificacion));
            DB.AddInParameter(cmd, "pFORMATO_SIC", DbType.AnsiString, entityToInsert.FormatSic.ToString());
            DB.AddInParameter(cmd, "pFORMATO_SICOFIN", DbType.AnsiString, entityToInsert.FormatSicofin.ToString());
            DB.AddInParameter(cmd, "pESTATUS", DbType.AnsiString, Util.SetEstado(entityToInsert.Estatus.ToString()));
            DB.AddInParameter(cmd, "pPERSONA", DbType.AnsiString, Util.SetPersona(entityToInsert.Persona.ToString()));
            DB.AddInParameter(cmd, "pGRUPO", DbType.Int32, Parser.ToNumber(entityToInsert.Grupo));
            DB.AddInParameter(cmd, "pTipoCuenta", DbType.AnsiString, entityToInsert.TipoCuenta);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        //MASS 14/06/13 ??
        public int CountCuenta6378()
        {
            DbCommand cmd =  DB.GetStoredProcCommand("CATALOGOS.SP_CATALOGOS_CountCuentaGrupo");
            DB.AddInParameter(cmd, "pGrupo", DbType.Int32, 6378);
            DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, (pers == Enums.Persona.Moral ? "M" : "F"));
            using (IDataReader reader = DB.ExecuteReader(cmd))
            {
                if (reader.Read())
                {
                    return Parser.ToNumber(reader[0]);
                }
            }
            return 0;
        }

        public List<int> GetGrupos()
        {
            List<int> toReturn = new List<int>();
            try
            {
                //MASS 14/06/13
                string query = "SELECT distinct grupo from T_CUENTAS_ACT WHERE ESTATUS = '1'";
                DbCommand cmd = DB.GetSqlStringCommand(query);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        int result = Parser.ToNumber(reader["GRUPO"]);

                        if (result != 0)
                        {
                            toReturn.Add(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar grupos de cuentas", ex);
            }

            return toReturn;
        }

    }

}
