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
    public class ClientCategories
    {

        public Guid Key { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public string Display { get; set; }
        public List<ClientCategories> GetList(string DB)
        {
            List<ClientCategories> items = new List<ClientCategories>();
            string selQuery = "select top 100 percent * from [AppSales_ClientCategories]  ";
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
                    ClientCategories item = new ClientCategories();
                    item.Key = (Guid)reader["cat_Key"];
                    item.ArabicName = Convert.ToString(reader["cat_Name1"]);
                    item.EnglishName = Convert.ToString(reader["cat_Name2"]);
                
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
