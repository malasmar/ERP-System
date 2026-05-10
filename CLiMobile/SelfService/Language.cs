using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService
{
    public class Language
    {
        public int Key { get; set; }
        public string English { get; set; }
        public string Arabic { get; set; }
        public List<Language> GetList()
        {
            List<Language> items = new List<Language>();
            string selQuery = "select top 100 percent * from Application_Language_SelfService order by [Lan_Key] ";
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
                    Language item = new Language();
                    item.Key = Convert.ToInt32(reader["Lan_Key"]);
                    item.Arabic = Convert.ToString(reader["Lan_Arabic"]);
                    item.English = Convert.ToString(reader["Lan_English"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<Language> Technical()
        {
            List<Language> items = new List<Language>();
            string selQuery = "select top 100 percent * from Application_Language_Technical order by [Lan_Key] ";
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
                    Language item = new Language();
                    item.Key = Convert.ToInt32(reader["Lan_Key"]);
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
