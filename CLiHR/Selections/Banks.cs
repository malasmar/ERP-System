using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiHR.Selections
{
    public class Banks
    {
        public Guid? Key { get; set; }
        public string ID { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Banks> GetList(string DB, string xLan)
        {
            List<Banks> items = new List<Banks>();
            string selQuery = "select top 100 percent [Bank_Key],[Bank_ID],[Bank_Name1],[Bank_Name2] from [HRCard_BankNames] order by [Bank_ID] ";
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
                    Banks item = new Banks();
                    item.Key = iCore.IsDbNullRtNull(reader["Bank_Key"]);
                    item.ID = Convert.ToString(reader["Bank_ID"]);
                    item.Name1 = Convert.ToString(reader["Bank_Name1"]);
                    item.Name2 = Convert.ToString(reader["Bank_Name2"]);
                 
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
