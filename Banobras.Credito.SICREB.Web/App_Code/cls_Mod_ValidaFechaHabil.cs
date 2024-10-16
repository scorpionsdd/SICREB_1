using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using Banobras.Credito.SICREB.Data;

/// <summary>
/// Descripción breve de cls_Mod_ValidaFechaHabil
/// </summary>
public class cls_Mod_ValidaFechaHabil : OracleBase
{
    public DateTime ValidaFechaHabil(DateTime dt, Dir direccion)
    {
        DataSet MyDS = new DataSet();
        int Sid = 0;
        int SFER_FEC_MES = 0;
        int SFER_FEC_DIA = 0;
        bool bandera = false;
        DateTime dtRegreso = new DateTime();
        //DateTime dtRegreso = new DateTime();
        int intDias = 0;

        if (Convert.ToDateTime(dt).DayOfWeek == DayOfWeek.Saturday)
        {
            if (direccion == Dir.Adelante)
            {
                intDias = 2;
                dt = dt.AddDays(intDias);
            }
            else if (direccion == Dir.Atras)
            {
                intDias = -1;
                dt = dt.AddDays(intDias);
            }
        }
        else if (Convert.ToDateTime(dt).DayOfWeek == DayOfWeek.Sunday)
        {
            if (direccion == Dir.Adelante)
            {
                intDias = 1;
                dt = dt.AddDays(intDias);
            }
            else if (direccion == Dir.Atras)
            {
                intDias = -2;
                dt = dt.AddDays(intDias);
            }
        }
        DataTable dtDiasFestivos = new DataTable();

        string query = string.Format("SP_TRANS_FERIADO");

        DbCommand cmd = DB.GetStoredProcCommand(query);
        DB.AddInParameter(cmd, "VFER_FEC_MES", DbType.Int32, Int32.Parse(dt.Month.ToString()));
        DB.AddInParameter(cmd, "VFER_FEC_DIA", DbType.Int32, Int32.Parse(dt.Day.ToString()));
        //ID,FER_FEC_MES, FER_FEC_DIA
        using (IDataReader reader = DB.ExecuteReader(cmd))
        {

            if (reader.Read())
            {
                Sid = int.Parse(reader["ID"].ToString());
                SFER_FEC_MES = int.Parse(reader["VFER_FEC_MES"].ToString());
                SFER_FEC_DIA = int.Parse(reader["VFER_FEC_DIA"].ToString());

            }

        }
        if (Sid != 0)
        {    //bandera = true;
            if (direccion == Dir.Adelante)
            {
                intDias = 1;
                dt = dt.AddDays(intDias);
            }
            else if (direccion == Dir.Atras)
            {
                intDias = -1;
                dt = dt.AddDays(intDias);
            }

            dtRegreso = ValidaFechaHabil(dt, direccion);
        }

        return dt;
    }

    public enum DateInterval
    {
        Year,
        Month,
        Day,
        Hour,
        Minute,
        Second,
        Quarter

    }

    public static DateTime DateAdd(DateInterval interval, DateTime dt, Int32 val)
    {
        if (interval == DateInterval.Year)
            return dt.AddYears(val);
        else if (interval == DateInterval.Month)
            return dt.AddMonths(val);
        else if (interval == DateInterval.Day)
            return dt.AddDays(val);
        else if (interval == DateInterval.Hour)
            return dt.AddHours(val);
        else if (interval == DateInterval.Minute)
            return dt.AddMinutes(val);
        else if (interval == DateInterval.Second)
            return dt.AddSeconds(val);
        else if (interval == DateInterval.Quarter)
            return dt.AddMonths(val * 3);
        else
            return dt;
    }


    public enum Dir
    {
        Adelante,
        Atras
    }

}