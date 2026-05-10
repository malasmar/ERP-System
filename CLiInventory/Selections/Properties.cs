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
    public class Properties
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }
        public string xDisplay { get; set; }
        public static List<Properties> Colors(string DB, string xLan)
        {
            List<Properties> items = new List<Properties>();
            string selQuery = "select top 100 percent [color_Key] as [Key],[color_No] as [No],[color_Name1] as [Name1],[color_Name2] as [Name2] from [invStockTag_Color] order by [color_No] ";
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
                    Properties item = new Properties();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            item.xDisplay = (item.Name2 == "" ? item.Name1 : item.Name2);
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public static List<Properties> Sizes(string DB, string xLan)
        {
            List<Properties> items = new List<Properties>();
            string selQuery = "select top 100 percent [size_Key] as [Key],[size_No] as [No],[size_Name1] as [Name1],[size_Name2] as [Name2] from [invStockTag_size] order by [size_No] ";
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
                    Properties item = new Properties();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.No.ToString() + ")";
                            item.xDisplay = (item.Name2 == "" ? item.Name1 : item.Name2);
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
                            item.xDisplay = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.No.ToString() + ")";
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
