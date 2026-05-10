using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiHR.Cards
{
    public class ShiftList
    {
        public string Code { get; set; }
        public string ArName { get; set; }
        public string EnName { get; set; }
        public DateTime? StartTime { get; set; }
        public Boolean NextDay { get; set; }
        public DateTime? EndTime { get; set; }
        public int AllownceEnter { get; set; }
        public int AllownceExit { get; set; }
        public decimal TotHour { get; set; }
        public int WeekEndDays { get; set; }
        public  List<ShiftList> GetList(string DB)
        {
            List<ShiftList> SHLL = new List<ShiftList>();
            string selQuery = "select top 100 percent * from [List_hrCard_ShiftCard] order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ShiftList item = new ShiftList();
                    item.Code = (string)reader["Code"];
                    item.ArName = (string)reader["ArName"];
                    item.EnName = (string)reader["EnName"];
                    item.StartTime = iCore.IsDbNullRtNullDate(DateTime.Parse(reader["StartTime"].ToString()));
                    item.NextDay = (Boolean)reader["NextDay"];
                    item.EndTime = iCore.IsDbNullRtNullDate(DateTime.Parse(reader["EndTime"].ToString()));
                    item.AllownceEnter = (int)reader["AllownceEnter"];
                    item.AllownceExit = (int)reader["AllownceExit"];
                    item.TotHour = (decimal)reader["TotHour"];
                    SHLL.Add(item);
                }
                reader.Close();
            }
            return SHLL;
        }
        public static void Delete(string DB, string Key)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "Delete from [hrCard_ShiftCard] where [SC_Code]=@Key";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Key", SqlDbType.NVarChar, 25).Value = iCore.IsNullRtDbNull(Key);
                con.Open();
                command.ExecuteNonQuery();
            }
        }
        public  ShiftList GetItem(string DB, string Key)
        {
            ShiftList item = new ShiftList();
            string selQuery = "select top 100 percent * from [List_hrCard_ShiftCard] where [Code]=@Key";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.NVarChar, 25).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Code = (string)reader["Code"];
                    item.ArName = (string)reader["ArName"];
                    item.EnName = (string)reader["EnName"];
                    item.StartTime = iCore.IsDbNullRtNullDate(DateTime.Parse(reader["StartTime"].ToString()));
                    item.NextDay = (Boolean)reader["NextDay"];
                    item.EndTime = iCore.IsDbNullRtNullDate(DateTime.Parse(reader["EndTime"].ToString()));
                    item.AllownceEnter = (int)reader["AllownceEnter"];
                    item.AllownceExit = (int)reader["AllownceExit"];
                    item.TotHour = (decimal)reader["TotHour"];
                }
                reader.Close();
                return item;
            }
        }
        public static void Insert(string DB, ShiftList SHLL)
        {
            Delete(DB, SHLL.Code);
            bool w0, w1, w2, w3, w4, w5, w6;
            w0 = false;
            w1 = false;
            w2 = false;
            w3 = false;
            w4 = false;
            w5 = false;
            w6 = false;
            switch (SHLL.WeekEndDays)
            {
                case 0:
                    w0 = true;
                    break;
                case 1:
                    w1 = true;
                    break;
                case 2:
                    w2 = true;
                    break;
                case 3:
                    w3 = true;
                    break;
                case 4:
                    w4 = true;
                    break;
                case 5:
                    w5 = true;
                    break;
                case 6:
                    w6 = true;
                    break;
            }
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                for (int i = 0; i <= 6; i++)
                {
                    System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                    ScStr.Clear();
                    ScStr.Append("INSERT INTO hrCard_ShiftCard");
                    ScStr.Append("([SC_SortingKey]");
                    ScStr.Append(",[SC_Code]");
                    ScStr.Append(",[SC_arName]");
                    ScStr.Append(",[SC_enName]");
                    ScStr.Append(",[SC_ST]");
                    ScStr.Append(",[SC_ND]");
                    ScStr.Append(",[SC_ET]");
                    ScStr.Append(",[SC_AllEnter]");
                    ScStr.Append(",[SC_AllExit]");
                    ScStr.Append(",[SC_ShiftDay]");
                    ScStr.Append(",[SC_DayIsWeekEnd]");
                    ScStr.Append(",[SC_S1StartTime]");
                    ScStr.Append(",[SC_S1InNextDay]");
                    ScStr.Append(",[SC_S1EndTime]");
                    ScStr.Append(",[SC_S2StartTime]");
                    ScStr.Append(",[SC_S2InNextDay]");
                    ScStr.Append(",[SC_S2EndTime]");
                    ScStr.Append(",[SC_DailyBreak]");
                    ScStr.Append(",[SC_TotalMinute])");
                    ScStr.Append(" VALUES ");
                    ScStr.Append("(@SC_SortingKey");
                    ScStr.Append(",@SC_Code");
                    ScStr.Append(",@SC_arName");
                    ScStr.Append(",@SC_enName");
                    ScStr.Append(",@SC_ST");
                    ScStr.Append(",@SC_ND");
                    ScStr.Append(",@SC_ET");
                    ScStr.Append(",@SC_AllEnter");
                    ScStr.Append(",@SC_AllExit");
                    ScStr.Append(",@SC_ShiftDay");
                    ScStr.Append(",@SC_DayIsWeekEnd");
                    ScStr.Append(",@SC_S1StartTime");
                    ScStr.Append(",@SC_S1InNextDay");
                    ScStr.Append(",@SC_S1EndTime");
                    ScStr.Append(",@SC_S2StartTime");
                    ScStr.Append(",@SC_S2InNextDay");
                    ScStr.Append(",@SC_S2EndTime");
                    ScStr.Append(",@SC_DailyBreak");
                    ScStr.Append(",@SC_TotalMinute)");
                    SqlCommand ScCom = new SqlCommand();
                    ScCom.Connection = con;
                    ScCom.CommandType = CommandType.Text;
                    ScCom.CommandText = ScStr.ToString();
                    ScCom.Parameters.Clear();
                    ScCom.Parameters.Add("@SC_SortingKey", SqlDbType.Int).Value = i;
                    ScCom.Parameters.Add("@SC_Code", SqlDbType.NVarChar, 25).Value = SHLL.Code ?? string.Empty;
                    ScCom.Parameters.Add("@SC_arName", SqlDbType.NVarChar, 100).Value = SHLL.ArName ?? string.Empty;
                    ScCom.Parameters.Add("@SC_enName", SqlDbType.NVarChar, 100).Value = SHLL.EnName ?? string.Empty;
                    ScCom.Parameters.Add("@SC_ST", SqlDbType.NVarChar, 5).Value = SHLL.StartTime.Value.ToString("HH:mm");
                    ScCom.Parameters.Add("@SC_ND", SqlDbType.Bit).Value = SHLL.NextDay;
                    ScCom.Parameters.Add("@SC_ET", SqlDbType.NVarChar, 5).Value = SHLL.EndTime.Value.ToString("HH:mm");
                    ScCom.Parameters.Add("@SC_AllEnter", SqlDbType.Int).Value = SHLL.AllownceEnter;
                    ScCom.Parameters.Add("@SC_AllExit", SqlDbType.Int).Value = SHLL.AllownceExit;
                    if (i == 0)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Saturday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = w0;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.StartTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.NextDay;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.EndTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = false;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = 0;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.EndTime - SHLL.StartTime).Value.TotalMinutes;
                    }
                    if (i == 1)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Sunday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = w1;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.StartTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.NextDay;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.EndTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = false;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = 0;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.EndTime - SHLL.StartTime).Value.TotalMinutes;
                    }
                    if (i == 2)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Monday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = w2;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.StartTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.NextDay;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.EndTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = false;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = 0;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.EndTime - SHLL.StartTime).Value.TotalMinutes;
                    }
                    if (i == 3)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Tuesday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = w3;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.StartTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.NextDay;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.EndTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = false;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = 0;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.EndTime - SHLL.StartTime).Value.TotalMinutes;
                    }
                    if (i == 4)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Wednesday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = w4;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.StartTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.NextDay;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.EndTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = false;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = 0;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.EndTime - SHLL.StartTime).Value.TotalMinutes;
                    }
                    if (i == 5)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Thursday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = w5;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.StartTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.NextDay;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.EndTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = false;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = 0;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.EndTime - SHLL.StartTime).Value.TotalMinutes;
                    }
                    if (i == 6)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Friday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = w6;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.StartTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.NextDay;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.NVarChar, 5).Value = iCore.IsNullRtDbNull(SHLL.EndTime.Value.ToString("HH:mm"));
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = false;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.NVarChar, 5).Value = DBNull.Value;
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = 0;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.EndTime - SHLL.StartTime).Value.TotalMinutes;
                    }
                    ScCom.ExecuteNonQuery();
                }
            }
        }
    }
}
