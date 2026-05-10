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
    public class Cities
    {

        public Guid Key { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public string Display { get; set; }
        public List<Cities> GetList(string DB)
        {
            List<Cities> items = new List<Cities>();
            string selQuery = "select top 100 percent * from [com_City]  ";
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
                    Cities item = new Cities();
                    item.Key = (Guid)reader["cit_Key"];
                    item.ArabicName = Convert.ToString(reader["cit_Name1"]);
                    item.EnglishName = Convert.ToString(reader["cit_Name2"]);
                
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
