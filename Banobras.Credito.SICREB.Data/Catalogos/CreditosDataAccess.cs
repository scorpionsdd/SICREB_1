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

    public class CreditosDataAccess : CatalogoBase<Entities.TipoCredito, string>
    {

        public override string IdField { get { return "IdCredito"; } }
        public override String ErrorGet { get { return " - Error al cargar los datos de la base de datos en catálogo Creditos."; } }
        public override String ErrorInsert { get { return " - Error al insertar nuevo registro en catálogo Creditos."; } }
        public override String ErrorUpdate { get { return " - Error al actualizar registro en catálogo Creditos."; } }
        public override String ErrorDelete { get { return " - Error al borrar el registro en catálogo Creditos."; } }

        public override Entities.TipoCredito GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int idValue = Parser.ToNumber(reader["ID"]);
            string tipoCredito = reader["TIPO_CREDITO"].ToString();
            string descripcion = reader["DESCRIPCION"].ToString();
            string nombreGenerico = reader["NOMBRE_GENERICO"].ToString();

            Enums.Estado estado = Util.GetEstado(Parser.ToChar(reader["Estatus"]));

            Entities.TipoCredito credito = new Entities.TipoCredito(idValue, tipoCredito, descripcion, nombreGenerico, estado);
            activo = (credito.Estatus == Enums.Estado.Activo);

            return credito;
        }

        public List<int> CreditosWarnings(string segmento, string etiqueta)
        {
            List<int> toReturn = new List<int>();

            DbCommand cmd = DB.GetStoredProcCommand("CATALOGOS.SP_CATALOGOS_GetValTipoCredito");
            DB.AddInParameter(cmd, "pSegmento", DbType.AnsiString, segmento);
            DB.AddInParameter(cmd, "pEtiqueta", DbType.AnsiString, etiqueta);

            using (IDataReader reader = DB.ExecuteReader(cmd))
            {

                while (reader.Read())
                {
                    int credito = Parser.ToNumber(reader["CODIGO"]);

                    toReturn.Add(credito);
                }
            }

            return toReturn;
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetCreditos"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "PACKAVISORECHAZO.SpUpdate"; }
        }

        public override void SetEntity(DbCommand cmd, Entities.TipoCredito entityOld, Entities.TipoCredito entityNew)
        {
            //Ejemplo
            DB.AddInParameter(cmd, "nombre", DbType.AnsiString, entityOld.Descripcion);
            DB.AddInParameter(cmd, "segudnodato", DbType.AnsiString, entityNew.NombreGenerico);
        }

    }

}
