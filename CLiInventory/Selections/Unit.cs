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
    public class Unit
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
    
        public static List<Unit> GetList(string DB, string xLan)
        {
            List<Unit> items = new List<Unit>();
            string selQuery = "select top 100 percent * from [invCard_Unit] order by [unit_Code] ";
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
                    Unit item = new Unit();
                    item.Key = iCore.IsDbNullRtNull(reader["unit_Key"]);
                    item.Code = Convert.ToString(reader["unit_Code"]);
                    item.Name1 = Convert.ToString(reader["unit_Name1"]);
                    item.Name2 = Convert.ToString(reader["unit_Name2"]);
            
                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2;
                            break;
                        case "ar":
                            item.Display = item.Name1;
                            break;
                        default:
                            item.Display = item.Name1;
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
