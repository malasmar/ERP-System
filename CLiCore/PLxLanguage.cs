using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CLiCore
{
    public class PLxLanguage
    {
        public string Key { get; set; }
        public string English { get; set; }
        public string Arabic { get; set; }


        public List<PLxLanguage> GetList()
        {
            List<PLxLanguage> items = new List<PLxLanguage>();
            string selQuery = "select top 100 percent * from px_Language order by [Lan_RecNo]";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PLxLanguage item = new PLxLanguage();
                  
                    item.Key = Convert.ToString(reader["Lan_Key"]);
                    item.Arabic = Convert.ToString(reader["Lan_Arabic"]);
                    item.English = Convert.ToString(reader["Lan_English"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
