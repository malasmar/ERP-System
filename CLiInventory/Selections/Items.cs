using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Selections
{
    public class Items
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
        public string xDisplay { get; set; }
        public static List<Items> GetList(string DB, string xLan)
        {
            List<Items> items = new List<Items>();
            string selQuery = "select top 100 percent [item_Key] as [Key],[item_Code] as [Code],[item_Name1] as [Name1],[item_Name2] as [Name2] from [invCard_StockItem] order by [item_Code] ";
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
                    Items item = new Items();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.Code + ")";
                            item.xDisplay = (item.Name2 == "" ? item.Name1 : item.Name2);
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
