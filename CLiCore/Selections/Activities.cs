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
    public class Activities
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Activities> GetList(string DB, string xLan)
        {
            List<Activities> items = new List<Activities>();
            string selQuery = "select top 100 percent * from [finCard_Group_Activity] order by [grp_No] ";
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
                    Activities item = new Activities();
                    item.Key = iCore.IsDbNullRtNull(reader["grp_Key"]);
                    item.No = Convert.ToInt32(reader["grp_No"]);
                    item.Name1 = Convert.ToString(reader["grp_Name1"]);
                    item.Name2 = Convert.ToString(reader["grp_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2 == "" ? item.Name1 : item.Name2 ;
                            break;
                        case "ar":
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                        default:
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1  ;
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
