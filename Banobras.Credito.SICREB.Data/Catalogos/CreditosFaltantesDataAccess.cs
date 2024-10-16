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

    public class CreditosFaltantesDataAccess : CatalogoBase<CreditosFaltantes, int>
    {

        public override string IdField { get { return "IdCuenta"; } } 
        public override String ErrorGet { get { return " 1039 - Error al cargar los datos de la base de datos en catálogo Créditos Faltantes."; } }
        public override String ErrorInsert { get { return "1040 - Error al insertar nuevo registro en catálogo Créditos Faltantes."; } }
        public override String ErrorUpdate { get { return "1041 - Error al actualizar registro en catálogo Créditos Faltantes."; } }
        public override String ErrorDelete { get { return "1042 - Error al borrar el registro en catálogo Créditos Faltantes."; } }
        
        public CreditosFaltantesDataAccess()
            : base()
        {
        }

        public override CreditosFaltantes GetEntity(IDataReader reader, out bool activo)
        {
            int id = 0;
            string credito = reader["credito"].ToString();
            string comentarios = reader["comentarios"].ToString();            
            string fecha = reader["fecha"].ToString();           
            
            //MASS 01/07/13          
            CreditosFaltantes cuenta = new CreditosFaltantes(id, credito, comentarios, fecha);            
            activo = true;
            return cuenta;
        }

        public override string StoredProcedure
        {
            get { return "SP_GET_CREDITOS_FALTANTES"; }
        }

        public override string StoredProcedureUpdate 
        {
            get { return ""; }
        }

        public override string StoredProcedureInsert
        {
            get { return ""; }
        }

        public override string StoredProcedureDelete
        {
            get { return ""; }
        }

        public override void SetEntity(DbCommand cmd, CreditosFaltantes entityOld, CreditosFaltantes entityNew)
        {            
            DB.AddInParameter(cmd, "pid", DbType.Int32, entityOld.id);
            DB.AddInParameter(cmd, "pcredito", DbType.AnsiString, entityNew.credito.ToString());            
            DB.AddInParameter(cmd, "pcomentarios", DbType.AnsiString, entityNew.comentarios.ToString());            
            DB.AddInParameter(cmd, "pfecha", DbType.AnsiString, entityNew.fecha.ToString());
            DB.AddParameter(cmd, "return", DbType.Int32, ParameterDirection.ReturnValue, String.Empty, DataRowVersion.Current, Convert.DBNull);
        }   
            
    }

}
