using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiCore.Selections
{
    public class Cities
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Cities> GetList(string DB, string xLan)
        {
            List<Cities> items = new List<Cities>();
            string selQuery = "select top 100 percent * from [com_City] order by [cit_No] ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["cit_Key"]);
                    item.No = Convert.ToInt32(reader["cit_No"]);
                    item.Name1 = Convert.ToString(reader["cit_Name1"]);
                    item.Name2 = Convert.ToString(reader["cit_Name2"]);
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
        public static Cities GetItem(string DB, string xLan,Guid? Key)
        {
            Cities item = new Cities();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from [com_City] where [cit_Key]=@Key order by [cit_No] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                 
                    item.Key = iCore.IsDbNullRtNull(reader["cit_Key"]);
                    item.No = Convert.ToInt32(reader["cit_No"]);
                    item.Name1 = Convert.ToString(reader["cit_Name1"]);
                    item.Name2 = Convert.ToString(reader["cit_Name2"]);
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
                  
                }
                reader.Close();
            }
            return item;
        }
    }
}
