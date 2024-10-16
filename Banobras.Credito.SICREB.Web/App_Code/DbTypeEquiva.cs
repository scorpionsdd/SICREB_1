using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using System.Data;

/// <summary>
/// Descripción breve de DbTypeEquiva
/// </summary>
public class DbTypeEquiva
{
    public   DbTypeEquiva()
	{	
    
    }
    public DbType ekiva(OracleDbType dbType)
    {	


        switch (dbType)
        {
            case OracleDbType.Varchar2: return DbType.AnsiString;
            case OracleDbType.Char: return DbType.AnsiStringFixedLength;
            case OracleDbType.Raw: return DbType.Binary;
            case OracleDbType.Byte: return DbType.Boolean;
            //case OracleDbType.Byte: return DbType.Byte;
            //case DbType.Currency: return OracleDbType.Decimal;
            //case DbType.Date: return OracleDbType.Date;
            //case DbType.DateTime: return OracleDbType.Date;
            case OracleDbType.Decimal: return DbType.Decimal;
          //case DbType.Double: return OracleDbType.Double;
            //case DbType.Guid: return OracleDbType.Raw;
//            case DbType.Int16: return OracleDbType.Int16;
            //case DbType.Int32: return OracleDbType.Int32;
            //case DbType.Int64: return OracleDbType.Int64;
            //case DbType.Object: return OracleDbType.Blob;
            //case DbType.SByte: return OracleDbType.Int16;
            //case DbType.Single: return OracleDbType.Single;
            //case DbType.String: return OracleDbType.NVarchar2;
            //case DbType.StringFixedLength: return OracleDbType.NChar;
            //case DbType.Time: return OracleDbType.TimeStamp;
            //case DbType.UInt16: return OracleDbType.Int16;
            //case DbType.UInt32: return OracleDbType.Int32;
            //case DbType.UInt64: return OracleDbType.Int64;
            //case DbType.VarNumeric: return OracleDbType.Decimal;
            default: return DbType.String;
        }//switdh
    }//ekiva
}