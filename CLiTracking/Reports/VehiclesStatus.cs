using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiTracking.Reports
{
    public class VehiclesStatus
    {
        public DateTime? Date { get; set; }
        public int No { get; set; }
        public Guid? Client { get; set; }
        public string Plate { get; set; }
        public int Status { get; set; }
        public DateTime? Install { get; set; }
        public Guid? Technical { get; set; }
        public List<VehiclesStatus> GetList(string DB,int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<VehiclesStatus> items = new List<VehiclesStatus>();
            string selQuery = "select top 100 percent * from dbo.ReportTracking_VehiclesStatus(@FirstDate,@LastDate) order by [Date],[Client] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    VehiclesStatus item = new VehiclesStatus();
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Client = iCore.IsDbNullRtNull(reader["Client"]);
                    item.Plate = Convert.ToString(reader["Plate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Install = iCore.IsDbNullRtNullDate(reader["Install"]);
                    item.Technical = iCore.IsDbNullRtNull(reader["Technical"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<VehiclesStatus> VehiclesInstalled(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<VehiclesStatus> items = new List<VehiclesStatus>();
            string selQuery = "select top 100 percent * from dbo.ReportTracking_VehiclesInstalled(@FirstDate,@LastDate) order by [Date],[Client] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    VehiclesStatus item = new VehiclesStatus();
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Client = iCore.IsDbNullRtNull(reader["Client"]);
                    item.Plate = Convert.ToString(reader["Plate"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Install = iCore.IsDbNullRtNullDate(reader["Install"]);
                    item.Technical = iCore.IsDbNullRtNull(reader["Technical"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
