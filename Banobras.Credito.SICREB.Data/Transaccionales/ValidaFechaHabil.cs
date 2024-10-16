using System;

namespace Banobras.Credito.SICREB.Data.Transaccionales
{
    public class ValidaFechaHabil : OracleBase
    {
        public enum Dir { Adelante, Atras }
        public enum DateInterval { Year, Month, Day, Hour, Minute, Second, Quarter }

        public DateTime ValidaFecha(DateTime dt, Dir direccion)
        {
            System.Data.DataSet MyDS = new System.Data.DataSet();
            int Sid = 0;
            int SFER_FEC_MES = 0;
            int SFER_FEC_DIA = 0;
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

            System.Data.DataTable dtDiasFestivos = new System.Data.DataTable();
            string query = string.Format("SP_TRANS_FERIADO");

            System.Data.Common.DbCommand cmd = DB.GetStoredProcCommand(query);
            DB.AddInParameter(cmd, "VFER_FEC_MES", System.Data.DbType.Int32, Int32.Parse(dt.Month.ToString()));
            DB.AddInParameter(cmd, "VFER_FEC_DIA", System.Data.DbType.Int32, Int32.Parse(dt.Day.ToString()));
            
            //ID,FER_FEC_MES, FER_FEC_DIA
            using (System.Data.IDataReader reader = DB.ExecuteReader(cmd))
            {

                if (reader.Read())
                {
                    Sid = int.Parse(reader["ID"].ToString());
                    SFER_FEC_MES = int.Parse(reader["FER_FEC_MES"].ToString());
                    SFER_FEC_DIA = int.Parse(reader["FER_FEC_DIA"].ToString());
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

                dt = ValidaFecha(dt, direccion);
            }

            return dt;
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

    }

}
