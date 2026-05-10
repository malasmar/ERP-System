using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Attendance
{
    public class EmployeeAttendance
    {
        public DateTime? Date { get; set; }
        public int Status { get; set; }
        public DateTime? Login { get; set; }
        public DateTime? Logout { get; set; }
        public string LocationName1 { get; set; }
        public string LocationName2 { get; set; }
        public string Display { get; set; }
        public string Duration { get; set; }
        public List<EmployeeAttendance> GetList(string DB,Guid? Key)
        {
            DateTime LastDate = DateTime.UtcNow.AddMinutes(180);
            DateTime FirstDate = LastDate.AddDays(-30);
            List<EmployeeAttendance> items = new List<EmployeeAttendance>();
            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_Att_Employee(@Key,@FirstDate,@LastDate) order by [Date] desc";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@FirstDate",SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeAttendance item = new EmployeeAttendance();
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Login = iCore.IsDbNullRtNullDate(reader["Login"]);
                    item.Logout = iCore.IsDbNullRtNullDate(reader["Logout"]);
                    item.LocationName1 = Convert.ToString(reader["LocationName1"]);
                    item.LocationName2 = Convert.ToString(reader["LocationName2"]);
                    item.Duration = Convert.ToString(reader["Duration"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
