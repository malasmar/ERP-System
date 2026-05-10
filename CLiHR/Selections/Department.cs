using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiHR.Selections
{
    public class Department
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Department> GetList(string DB, string xLan)
        {
            List<Department> items = new List<Department>();
            string selQuery = "select top 100 percent [dep_Key],[dep_No],[dep_Name1],[dep_Name2] from [hrCard_Department] order by [dep_No] ";
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
                    Department item = new Department();
                    item.Key = iCore.IsDbNullRtNull(reader["dep_Key"]);
                    item.No = Convert.ToInt32(reader["dep_No"]);
                    item.Name1 = Convert.ToString(reader["dep_Name1"]);
                    item.Name2 = Convert.ToString(reader["dep_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2);
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1);
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
