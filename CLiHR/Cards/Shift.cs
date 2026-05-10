using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiCore.Account;

namespace CLiHR.Cards
{
    public class Shift
    {
        #region "Propereties"
        public Guid SC_Key { get; set; }
        public int SC_SortingKey { get; set; }
        public string SC_Code { get; set; }
        public string SC_arName { get; set; }
        public string SC_enName { get; set; }
        public TimeSpan? SC_ST { get; set; }
        public Boolean SC_ND { get; set; }
        public TimeSpan? SC_ET { get; set; }
        public int SC_AllEnter { get; set; }
        public int SC_AllExit { get; set; }
        public string SC_ShiftDay_0 { get; set; }
        public Boolean SC_DayIsWeekEnd_0 { get; set; }
        public TimeSpan? SC_S1StartTime_0 { get; set; }
        public Boolean SC_S1InNextDay_0 { get; set; }
        public TimeSpan? SC_S1EndTime_0 { get; set; }
        public TimeSpan? SC_S2StartTime_0 { get; set; }
        public Boolean SC_S2InNextDay_0 { get; set; }
        public TimeSpan? SC_S2EndTime_0 { get; set; }
        public int SC_DailyBreak_0 { get; set; }
        public decimal SC_TotalMinute_0 { get; set; }
        public string SC_ShiftDay_1 { get; set; }
        public Boolean SC_DayIsWeekEnd_1 { get; set; }
        public TimeSpan? SC_S1StartTime_1 { get; set; }
        public Boolean SC_S1InNextDay_1 { get; set; }
        public TimeSpan? SC_S1EndTime_1 { get; set; }
        public TimeSpan? SC_S2StartTime_1 { get; set; }
        public Boolean SC_S2InNextDay_1 { get; set; }
        public TimeSpan? SC_S2EndTime_1 { get; set; }
        public int SC_DailyBreak_1 { get; set; }
        public decimal SC_TotalMinute_1 { get; set; }
        public string SC_ShiftDay_2 { get; set; }
        public Boolean SC_DayIsWeekEnd_2 { get; set; }
        public TimeSpan? SC_S1StartTime_2 { get; set; }
        public Boolean SC_S1InNextDay_2 { get; set; }
        public TimeSpan? SC_S1EndTime_2 { get; set; }
        public TimeSpan? SC_S2StartTime_2 { get; set; }
        public Boolean SC_S2InNextDay_2 { get; set; }
        public TimeSpan? SC_S2EndTime_2 { get; set; }
        public int SC_DailyBreak_2 { get; set; }
        public decimal SC_TotalMinute_2 { get; set; }
        public string SC_ShiftDay_3 { get; set; }
        public Boolean SC_DayIsWeekEnd_3 { get; set; }
        public TimeSpan? SC_S1StartTime_3 { get; set; }
        public Boolean SC_S1InNextDay_3 { get; set; }
        public TimeSpan? SC_S1EndTime_3 { get; set; }
        public TimeSpan? SC_S2StartTime_3 { get; set; }
        public Boolean SC_S2InNextDay_3 { get; set; }
        public TimeSpan? SC_S2EndTime_3 { get; set; }
        public int SC_DailyBreak_3 { get; set; }
        public decimal SC_TotalMinute_3 { get; set; }
        public string SC_ShiftDay_4 { get; set; }
        public Boolean SC_DayIsWeekEnd_4 { get; set; }
        public TimeSpan? SC_S1StartTime_4 { get; set; }
        public Boolean SC_S1InNextDay_4 { get; set; }
        public TimeSpan? SC_S1EndTime_4 { get; set; }
        public TimeSpan? SC_S2StartTime_4 { get; set; }
        public Boolean SC_S2InNextDay_4 { get; set; }
        public TimeSpan? SC_S2EndTime_4 { get; set; }
        public int SC_DailyBreak_4 { get; set; }
        public decimal SC_TotalMinute_4 { get; set; }
        public string SC_ShiftDay_5 { get; set; }
        public Boolean SC_DayIsWeekEnd_5 { get; set; }
        public TimeSpan? SC_S1StartTime_5 { get; set; }
        public Boolean SC_S1InNextDay_5 { get; set; }
        public TimeSpan? SC_S1EndTime_5 { get; set; }
        public TimeSpan? SC_S2StartTime_5 { get; set; }
        public Boolean SC_S2InNextDay_5 { get; set; }
        public TimeSpan? SC_S2EndTime_5 { get; set; }
        public int SC_DailyBreak_5 { get; set; }
        public decimal SC_TotalMinute_5 { get; set; }
        public string SC_ShiftDay_6 { get; set; }
        public Boolean SC_DayIsWeekEnd_6 { get; set; }
        public TimeSpan? SC_S1StartTime_6 { get; set; }
        public Boolean SC_S1InNextDay_6 { get; set; }
        public TimeSpan? SC_S1EndTime_6 { get; set; }
        public TimeSpan? SC_S2StartTime_6 { get; set; }
        public Boolean SC_S2InNextDay_6 { get; set; }
        public TimeSpan? SC_S2EndTime_6 { get; set; }
        public int SC_DailyBreak_6 { get; set; }
        public decimal SC_TotalMinute_6 { get; set; }
        #endregion "Propereties"
        public  Shift GetItem(string DB, string Key)
        {
            Shift item = new Shift();
            string selQuery = "select top 100 percent * from hrAttendance_Shift where [SC_Code]=@Key order by [SC_SortingKey]";
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
                    if ((int)reader["SC_SortingKey"] == 0)
                    {
                        item.SC_Key = (Guid)reader["SC_Key"];
                    
                        item.SC_SortingKey = (int)reader["SC_SortingKey"];
                        item.SC_Code = (string)reader["SC_Code"];
                        item.SC_arName = (string)reader["SC_arName"];
                        item.SC_enName = (string)reader["SC_enName"];
                        item.SC_ST = iCore.IsDbNullRtNullTime(reader["SC_ST"]);
                        item.SC_ND = (Boolean)reader["SC_ND"];
                        item.SC_ET = iCore.IsDbNullRtNullTime(reader["SC_ET"]); ;
                        item.SC_AllEnter = (int)reader["SC_AllEnter"];
                        item.SC_AllExit = (int)reader["SC_AllExit"];
                        item.SC_ShiftDay_0 = (string)reader["SC_ShiftDay"];
                        item.SC_DayIsWeekEnd_0 = (Boolean)reader["SC_DayIsWeekEnd"];
                        item.SC_S1StartTime_0 = iCore.IsDbNullRtNullTime(reader["SC_S1StartTime"]);
                        item.SC_S1InNextDay_0 = (Boolean)reader["SC_S1InNextDay"];
                        item.SC_S1EndTime_0 = iCore.IsDbNullRtNullTime(reader["SC_S1EndTime"]);
                        item.SC_S2StartTime_0 = iCore.IsDbNullRtNullTime(reader["SC_S2StartTime"]);
                        item.SC_S2InNextDay_0 = (Boolean)reader["SC_S2InNextDay"];
                        item.SC_S2EndTime_0 = iCore.IsDbNullRtNullTime(reader["SC_S2EndTime"]);
                        item.SC_DailyBreak_0 = (int)reader["SC_DailyBreak"];
                        item.SC_TotalMinute_0 = (decimal)reader["SC_TotalMinute"];
                    }
                    if ((int)reader["SC_SortingKey"] == 1)
                    {
                        item.SC_ShiftDay_1 = (string)reader["SC_ShiftDay"];
                        item.SC_DayIsWeekEnd_1 = (Boolean)reader["SC_DayIsWeekEnd"];
                        item.SC_S1StartTime_1 = iCore.IsDbNullRtNullTime(reader["SC_S1StartTime"]);
                        item.SC_S1InNextDay_1 = (Boolean)reader["SC_S1InNextDay"];
                        item.SC_S1EndTime_1 = iCore.IsDbNullRtNullTime(reader["SC_S1EndTime"]);
                        item.SC_S2StartTime_1 = iCore.IsDbNullRtNullTime(reader["SC_S2StartTime"]);
                        item.SC_S2InNextDay_1 = (Boolean)reader["SC_S2InNextDay"];
                        item.SC_S2EndTime_1 = iCore.IsDbNullRtNullTime(reader["SC_S2EndTime"]);
                        item.SC_DailyBreak_1 = (int)reader["SC_DailyBreak"];
                        item.SC_TotalMinute_1 = (decimal)reader["SC_TotalMinute"];
                    }
                    if ((int)reader["SC_SortingKey"] == 2)
                    {
                        item.SC_ShiftDay_2 = (string)reader["SC_ShiftDay"];
                        item.SC_DayIsWeekEnd_2 = (Boolean)reader["SC_DayIsWeekEnd"];
                        item.SC_S1StartTime_2 = iCore.IsDbNullRtNullTime(reader["SC_S1StartTime"]);
                        item.SC_S1InNextDay_2 = (Boolean)reader["SC_S1InNextDay"];
                        item.SC_S1EndTime_2 = iCore.IsDbNullRtNullTime(reader["SC_S1EndTime"]);
                        item.SC_S2StartTime_2 = iCore.IsDbNullRtNullTime(reader["SC_S2StartTime"]);
                        item.SC_S2InNextDay_2 = (Boolean)reader["SC_S2InNextDay"];
                        item.SC_S2EndTime_2 = iCore.IsDbNullRtNullTime(reader["SC_S2EndTime"]);
                        item.SC_DailyBreak_2 = (int)reader["SC_DailyBreak"];
                        item.SC_TotalMinute_2 = (decimal)reader["SC_TotalMinute"];
                    }
                    if ((int)reader["SC_SortingKey"] == 3)
                    {
                        item.SC_ShiftDay_3 = (string)reader["SC_ShiftDay"];
                        item.SC_DayIsWeekEnd_3 = (Boolean)reader["SC_DayIsWeekEnd"];
                        item.SC_S1StartTime_3 = iCore.IsDbNullRtNullTime(reader["SC_S1StartTime"]);
                        item.SC_S1InNextDay_3 = (Boolean)reader["SC_S1InNextDay"];
                        item.SC_S1EndTime_3 = iCore.IsDbNullRtNullTime(reader["SC_S1EndTime"]);
                        item.SC_S2StartTime_3 = iCore.IsDbNullRtNullTime(reader["SC_S2StartTime"]);
                        item.SC_S2InNextDay_3 = (Boolean)reader["SC_S2InNextDay"];
                        item.SC_S2EndTime_3 = iCore.IsDbNullRtNullTime(reader["SC_S2EndTime"]);
                        item.SC_DailyBreak_3 = (int)reader["SC_DailyBreak"];
                        item.SC_TotalMinute_3 = (decimal)reader["SC_TotalMinute"];
                    }
                    if ((int)reader["SC_SortingKey"] == 4)
                    {
                        item.SC_ShiftDay_4 = (string)reader["SC_ShiftDay"];
                        item.SC_DayIsWeekEnd_4 = (Boolean)reader["SC_DayIsWeekEnd"];
                        item.SC_S1StartTime_4 = iCore.IsDbNullRtNullTime(reader["SC_S1StartTime"]);
                        item.SC_S1InNextDay_4 = (Boolean)reader["SC_S1InNextDay"];
                        item.SC_S1EndTime_4 = iCore.IsDbNullRtNullTime(reader["SC_S1EndTime"]);
                        item.SC_S2StartTime_4 = iCore.IsDbNullRtNullTime(reader["SC_S2StartTime"]);
                        item.SC_S2InNextDay_4 = (Boolean)reader["SC_S2InNextDay"];
                        item.SC_S2EndTime_4 = iCore.IsDbNullRtNullTime(reader["SC_S2EndTime"]);
                        item.SC_DailyBreak_4 = (int)reader["SC_DailyBreak"];
                        item.SC_TotalMinute_4 = (decimal)reader["SC_TotalMinute"];
                    }
                    if ((int)reader["SC_SortingKey"] == 5)
                    {
                        item.SC_ShiftDay_5 = (string)reader["SC_ShiftDay"];
                        item.SC_DayIsWeekEnd_5 = (Boolean)reader["SC_DayIsWeekEnd"];
                        item.SC_S1StartTime_5 = iCore.IsDbNullRtNullTime(reader["SC_S1StartTime"]);
                        item.SC_S1InNextDay_5 = (Boolean)reader["SC_S1InNextDay"];
                        item.SC_S1EndTime_5 = iCore.IsDbNullRtNullTime(reader["SC_S1EndTime"]);
                        item.SC_S2StartTime_5 = iCore.IsDbNullRtNullTime(reader["SC_S2StartTime"]);
                        item.SC_S2InNextDay_5 = (Boolean)reader["SC_S2InNextDay"];
                        item.SC_S2EndTime_5 = iCore.IsDbNullRtNullTime(reader["SC_S2EndTime"]);
                        item.SC_DailyBreak_5 = (int)reader["SC_DailyBreak"];
                        item.SC_TotalMinute_5 = (decimal)reader["SC_TotalMinute"];
                    }
                    if ((int)reader["SC_SortingKey"] == 6)
                    {
                        item.SC_ShiftDay_6 = (string)reader["SC_ShiftDay"];
                        item.SC_DayIsWeekEnd_6 = (Boolean)reader["SC_DayIsWeekEnd"];
                        item.SC_S1StartTime_6 = iCore.IsDbNullRtNullTime(reader["SC_S1StartTime"]);
                        item.SC_S1InNextDay_6 = (Boolean)reader["SC_S1InNextDay"];
                        item.SC_S1EndTime_6 = iCore.IsDbNullRtNullTime(reader["SC_S1EndTime"]);
                        item.SC_S2StartTime_6 = iCore.IsDbNullRtNullTime(reader["SC_S2StartTime"]);
                        item.SC_S2InNextDay_6 = (Boolean)reader["SC_S2InNextDay"];
                        item.SC_S2EndTime_6 = iCore.IsDbNullRtNullTime(reader["SC_S2EndTime"]);
                        item.SC_DailyBreak_6 = (int)reader["SC_DailyBreak"];
                        item.SC_TotalMinute_6 = (decimal)reader["SC_TotalMinute"];
                    }
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB, Shift SHLL)
        {
            Delete(DB, SHLL.SC_Code);
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                for (int i = 0; i <= 6; i++)
                {
                    System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                    ScStr.Clear();
                    ScStr.Append("INSERT INTO hrAttendance_Shift ");
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
                    ScCom.Parameters.Add("@SC_Code", SqlDbType.NVarChar, 25).Value = SHLL.SC_Code ?? string.Empty;
                    ScCom.Parameters.Add("@SC_arName", SqlDbType.NVarChar, 100).Value = SHLL.SC_arName ?? string.Empty;
                    ScCom.Parameters.Add("@SC_enName", SqlDbType.NVarChar, 100).Value = SHLL.SC_enName ?? string.Empty;
                    ScCom.Parameters.Add("@SC_ST", SqlDbType.Time).Value = SHLL.SC_ST;
                    ScCom.Parameters.Add("@SC_ND", SqlDbType.Bit).Value = SHLL.SC_ND;
                    ScCom.Parameters.Add("@SC_ET", SqlDbType.Time).Value = SHLL.SC_ET;
                    ScCom.Parameters.Add("@SC_AllEnter", SqlDbType.Int).Value = SHLL.SC_AllEnter;
                    ScCom.Parameters.Add("@SC_AllExit", SqlDbType.Int).Value = SHLL.SC_AllExit;
                    if (i == 0)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Saturday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = SHLL.SC_DayIsWeekEnd_0;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1StartTime_0);
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.SC_S1InNextDay_0;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1EndTime_0);
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2StartTime_0);
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = SHLL.SC_S2InNextDay_0;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2EndTime_0);
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = SHLL.SC_DailyBreak_0;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.SC_S2EndTime_0 - SHLL.SC_S1EndTime_0).HasValue == true ? (SHLL.SC_S2EndTime_0 - SHLL.SC_S1EndTime_0).Value.TotalMinutes : 0;
                    }
                    if (i == 1)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Sunday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = SHLL.SC_DayIsWeekEnd_1;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1StartTime_1);
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.SC_S1InNextDay_1;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1EndTime_1);
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2StartTime_1);
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = SHLL.SC_S2InNextDay_1;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2EndTime_1);
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = SHLL.SC_DailyBreak_1;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.SC_S2EndTime_1 - SHLL.SC_S1EndTime_1).HasValue == true ? (SHLL.SC_S2EndTime_1 - SHLL.SC_S1EndTime_1).Value.TotalMinutes : 0;
                    }
                    if (i == 2)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Monday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = SHLL.SC_DayIsWeekEnd_2;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1StartTime_2);
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.SC_S1InNextDay_2;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1EndTime_2);
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2StartTime_2);
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = SHLL.SC_S2InNextDay_2;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2EndTime_2);
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = SHLL.SC_DailyBreak_2;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.SC_S2EndTime_2 - SHLL.SC_S1EndTime_2).HasValue == true ? (SHLL.SC_S2EndTime_2 - SHLL.SC_S1EndTime_2).Value.TotalMinutes : 0;
                    }
                    if (i == 3)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Tuesday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = SHLL.SC_DayIsWeekEnd_3;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1StartTime_3);
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.SC_S1InNextDay_3;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1EndTime_3);
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2StartTime_3);
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = SHLL.SC_S2InNextDay_3;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2EndTime_3);
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = SHLL.SC_DailyBreak_3;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.SC_S2EndTime_3 - SHLL.SC_S1EndTime_3).HasValue == true ? (SHLL.SC_S2EndTime_3 - SHLL.SC_S1EndTime_3).Value.TotalMinutes : 0;
                    }
                    if (i == 4)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Wednesday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = SHLL.SC_DayIsWeekEnd_4;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1StartTime_4);
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.SC_S1InNextDay_4;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1EndTime_4);
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2StartTime_4);
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = SHLL.SC_S2InNextDay_4;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2EndTime_4);
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = SHLL.SC_DailyBreak_4;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.SC_S2EndTime_4 - SHLL.SC_S1EndTime_4).HasValue == true ? (SHLL.SC_S2EndTime_4 - SHLL.SC_S1EndTime_4).Value.TotalMinutes : 0;
                    }
                    if (i == 5)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Thursday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = SHLL.SC_DayIsWeekEnd_5;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1StartTime_5);
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.SC_S1InNextDay_5;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1EndTime_5);
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2StartTime_5);
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = SHLL.SC_S2InNextDay_5;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2EndTime_5);
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = SHLL.SC_DailyBreak_5;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.SC_S2EndTime_5 - SHLL.SC_S1EndTime_5).HasValue == true ? (SHLL.SC_S2EndTime_5 - SHLL.SC_S1EndTime_5).Value.TotalMinutes : 0;
                    }
                    if (i == 6)
                    {
                        ScCom.Parameters.Add("@SC_ShiftDay", SqlDbType.NVarChar, 9).Value = "Friday";
                        ScCom.Parameters.Add("@SC_DayIsWeekEnd", SqlDbType.Bit).Value = SHLL.SC_DayIsWeekEnd_6;
                        ScCom.Parameters.Add("@SC_S1StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1StartTime_6);
                        ScCom.Parameters.Add("@SC_S1InNextDay", SqlDbType.Bit).Value = SHLL.SC_S1InNextDay_6;
                        ScCom.Parameters.Add("@SC_S1EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S1EndTime_6);
                        ScCom.Parameters.Add("@SC_S2StartTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2StartTime_6);
                        ScCom.Parameters.Add("@SC_S2InNextDay", SqlDbType.Bit).Value = SHLL.SC_S2InNextDay_6;
                        ScCom.Parameters.Add("@SC_S2EndTime", SqlDbType.Time).Value = iCore.IsNullRtDbNull(SHLL.SC_S2EndTime_6);
                        ScCom.Parameters.Add("@SC_DailyBreak", SqlDbType.Int).Value = SHLL.SC_DailyBreak_6;
                        ScCom.Parameters.Add("@SC_TotalMinute", SqlDbType.Decimal).Value = (SHLL.SC_S2EndTime_6 - SHLL.SC_S1EndTime_6).HasValue == true ? (SHLL.SC_S2EndTime_6 - SHLL.SC_S1EndTime_6).Value.TotalMinutes : 0;
                    }
                    ScCom.ExecuteNonQuery();
                }
            }
        }
        public static void Delete(string DB, string Key)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "Delete from [hrAttendance_Shift] where [SC_Code]=@Key";
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
    }
}
