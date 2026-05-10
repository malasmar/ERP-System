using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Selections
{
    public class DownloadItems
    {
        public string Category { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Unit { get; set; }
        public List<DownloadItems> GetList(string DB)
        {
            List<DownloadItems> items = new List<DownloadItems>();
            string selQuery = "select top 100 percent * from dbo.fninvSelection_DownloadItems() order by [Category],[code]";
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
                    DownloadItems item = new DownloadItems();
                    item.Category = Convert.ToString(reader["Category"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
