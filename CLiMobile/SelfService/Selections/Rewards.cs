using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Selections
{
    public class Rewards
    {
        public Guid? Key { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }

        public   List<Rewards> GetList(string DB)
        {
            List<Rewards> items = new List<Rewards>();
            string selQuery = "select top 100 percent * from [hrCard_Categories_Reward] where [crew_Disable]=0 order by [crew_No] ";
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
                    Rewards item = new Rewards();
                    item.Key = iCore.IsDbNullRtNull(reader["crew_Key"]);
                    item.Name1 = Convert.ToString(reader["crew_Name1"]) ?? "";
                    item.Name2 = Convert.ToString(reader["crew_Name2"]) ?? "";
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
