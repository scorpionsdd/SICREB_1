using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Data.Catalogos
{

    public class SEPOMEXDataAccess:CatalogoBase<SEPOMEX, int>
    {

        public override string IdField { get { return "idSepomex"; } }
        public override String ErrorGet { get { return " 1062 - Error al cargar los datos de la base de datos en catálogo Sepomex."; } }
        public override String ErrorInsert { get { return " - No Aplica   Error al insertar nuevo registro en catálogo Sepomex."; } }
        public override String ErrorUpdate { get { return " - No Aplica   Error al actualizar registro en catálogo Sepomex."; } }
        public override String ErrorDelete { get { return " - No Aplica   Error al borrar el registro en catálogo Sepomex."; } }

        public SEPOMEXDataAccess()
        {
        }

        public override SEPOMEX GetEntity(System.Data.IDataReader reader, out bool activo)
        {
            int mId=Parser.ToNumber(reader["id"]) ;
            string mDCodigo = reader["D_CODIGO"].ToString();
            string mDAsenta = reader["D_ASENTA"].ToString();
            string mDTipoAsenta= reader["D_TIPO_ASENTA"].ToString();
            string mDMnpio= reader["D_MNPIO"].ToString();
            string mDEstado= reader["D_ESTADO"].ToString();
            string mDCiudad= reader["D_CIUDAD"].ToString();
            string mDCP= reader["D_CP"].ToString();
            string mCEstado= reader["C_ESTADO"].ToString();
            string mCOficina= reader["C_OFICINA"].ToString();
            string mCCP= reader["C_CP"].ToString();
            string mCTipoAsenta= reader["C_TIPO_ASENTA"].ToString();
            string mCMnpio= reader["C_MNPIO"].ToString();
            string mIdAsentaCpcons= reader["ID_ASENTA_CPCONS"].ToString();
            string mDZona= reader["D_ZONA"].ToString();
            string mCCveCiudad = reader["C_CVE_CIUDAD"].ToString();

            SEPOMEX sm = new SEPOMEX(mId, mDCodigo, mDAsenta,mDTipoAsenta,mDMnpio,mDEstado,mDCiudad,mDCP,mCEstado,mCOficina,mCCP,mCTipoAsenta,mCMnpio,mIdAsentaCpcons,mDZona,mCCveCiudad);
            activo = true;

            return sm;
        }

        public List<SEPOMEX> GetSEPOMEXPorCP(string CP, bool soloActivos)
        {
            List<SEPOMEX> toReturn = new List<SEPOMEX>();
            try
            {
                DbCommand cmd = DB.GetStoredProcCommand(StoredProcedure);

                DB.AddInParameter(cmd, "idSepomex", DbType.AnsiString, 0);
                DB.AddInParameter(cmd, "codigo", DbType.AnsiString, CP);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        bool activo;
                        SEPOMEX entity = GetEntity(reader, out activo);

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
                throw new Exception("Error al Obtener los Datos de SEPOMEX por Codigo Postal", ex);
            }
        }

        public override string StoredProcedure
        {
            get { return "CATALOGOS.SP_CATALOGOS_GetSEPOMEX"; }
        }

        public override string StoredProcedureUpdate
        {
            get { return "StoreProcedure a Utilizar"; }
        }
        
        public override void SetEntity(DbCommand cmd, SEPOMEX entityOld, SEPOMEX entityNew)
        {
            throw new NotImplementedException();
        }

    }

}
