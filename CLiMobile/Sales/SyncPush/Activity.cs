using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Sales.SyncPush
{
    public class Activity
    {

        public Guid Key { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public string Display { get; set; }
        public List<Activity> GetList(string DB)
        {
            List<Activity> items = new List<Activity>();
            string selQuery = "select top 100 percent * from finCard_Group_Activity where grp_Disable=0";
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
                    Activity item = new Activity();
                    item.Key = (Guid)reader["grp_Key"];
                    item.ArabicName = Convert.ToString(reader["grp_Name1"]);
                    item.EnglishName = Convert.ToString(reader["grp_Name2"]);
                
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
