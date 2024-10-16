using System;
using System.Data;
using System.Data.Common;
using Banobras.Credito.SICREB.Common;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class IdentificadorDataAccess : CatalogoBase<Identificador, String>
    {

        private readonly Enums.Persona pers = Enums.Persona.Moral;
        public override string IdField{ get { return "IdIdentificador"; } }
        public override String ErrorGet { get { return " 1080 - Error al cargar los datos de la base de datos en catálogo dígito identificador."; } }
        public override String ErrorInsert { get { return "1081 - Error al insertar nuevo registro en catálogo dígito identificador."; } }
        public override String ErrorUpdate { get { return "1082 - Error al actualizar registro en catálogo dígito identificador."; } }
        public override String ErrorDelete { get { return "1083 - Error al borrar el registro en catálogo dígito identificador."; } }

        public IdentificadorDataAccess(Enums.Persona persona)
            : base(persona)
        {
            this.pers = persona;
        }

        public override Identificador GetEntity(IDataReader reader, out bool activo)
        {
            int idIdentificador = Parser.ToNumber(reader["ID"]);
            string rfcIdentificador = reader["RFC"].ToString();
            string creditoIdentificador = reader["CREDITO"].ToString();
            string digitoIdentificador = reader["IDENTIFICADOR"].ToString();
            string nombreIdentificador = reader["NOMBRE"].ToString();
            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["ESTATUS"]));
            Enums.Persona persona = Util.GetPersona(Parser.ToChar(reader["PERSONA"]));

            Identificador identificador = new Identificador(idIdentificador, rfcIdentificador, creditoIdentificador, digitoIdentificador, nombreIdentificador, estado, persona);
            activo = true;

            return identificador;
        }

        public override string StoredProcedure
        {
            get { return "PK_IDENTIFICADOR.SP_GetIdentificador"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PK_IDENTIFICADOR.F_UpdIdentificador"; }
        }

        public override string StoredProcedureDelete
        {
            get { return "PK_IDENTIFICADOR.F_DelIdentificador"; }
        }

        public override string StoredProcedureInsert
        {
            get { return "PK_IDENTIFICADOR.F_InsIdentificador"; }
        }

        public override void SetEntity(DbCommand cmd, Identificador entityOld, Identificador entityNew)
        {
            DB.AddInParameter(cmd, "pID", DbType.Int32, entityNew.Id);
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityNew.Rfc);
            DB.AddInParameter(cmd, "pCredito", DbType.AnsiString, entityNew.Credito);
            DB.AddInParameter(cmd, "pDigitoIdentificador", DbType.AnsiString, entityNew.DigitoIdentificador);
            DB.AddInParameter(cmd, "pNombre", DbType.AnsiString, entityNew.Nombre);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityDelete(DbCommand cmd, Identificador entityToDelete)
        {
            DB.AddInParameter(cmd, "pID", DbType.AnsiString, entityToDelete.Id);
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

        public override void SetEntityInsert(DbCommand cmd, Identificador entityToInsert)
        {
            DB.AddInParameter(cmd, "pRFC", DbType.AnsiString, entityToInsert.Rfc);
            DB.AddInParameter(cmd, "pCredito", DbType.AnsiString, entityToInsert.Credito);
            DB.AddInParameter(cmd, "pDigitoIdentificador", DbType.AnsiString, entityToInsert.DigitoIdentificador);
            DB.AddInParameter(cmd, "pNombre", DbType.AnsiString, entityToInsert.Nombre);
            DB.AddInParameter(cmd, "pPersona", DbType.AnsiString, Util.SetPersona(entityToInsert.Persona.ToString()));
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }

    }

}
