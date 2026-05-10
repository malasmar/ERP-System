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
    public class Location
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid Project { get; set; }
        public List<Location> GetList(string DB)
        {
            List<Location> items = new List<Location>();
            string selQuery = "select top 100 percent * from hrCard_Location where [Loc_Disable]=0 order by [loc_No] ";
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
                    Location item = new Location();
                    item.Key = iCore.IsDbNullRtNull(reader["loc_Key"]);
                    item.No = Convert.ToInt32(reader["Loc_No"]);
                    item.Name1 = Convert.ToString(reader["Loc_Name1"]);
                    item.Name2 = Convert.ToString(reader["Loc_Name2"]);
                    item.Latitude = Convert.ToDouble(reader["Loc_Latitude"]);
                    item.Longitude = Convert.ToDouble(reader["Loc_Longitude"]);
                
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
