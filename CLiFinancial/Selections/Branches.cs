using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Selections
{
    public class Branches
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Branches> GetList(string DB, string xLan)
        {
            List<Branches> items = new List<Branches>();
            string selQuery = "select top 100 percent * from [com_Branch] order by [brh_No] ";
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
                    Branches item = new Branches();
                    item.Key = iCore.IsDbNullRtNull(reader["brh_Key"]);
                    item.No = Convert.ToInt32(reader["brh_No"]);
                    item.Name1 = Convert.ToString(reader["brh_Name1"]);
                    item.Name2 = Convert.ToString(reader["brh_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
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
