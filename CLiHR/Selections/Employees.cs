using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CLiCore;

namespace CLiHR.Selections
{
    public class Employees
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Employees> GetList(string DB, string xLan)
        {
            List<Employees> items = new List<Employees>();
            string selQuery = "select top 100 percent [emp_Key],[emp_Code],[emp_Name1],[emp_Name2] from [hrCard_Employee] order by [emp_Code] ";
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
                    Employees item = new Employees();
                    item.Key = iCore.IsDbNullRtNull(reader["emp_Key"]);
                    item.Code = Convert.ToString(reader["emp_Code"]);
                    item.Name1 = Convert.ToString(reader["emp_Name1"]);
                    item.Name2 = Convert.ToString(reader["emp_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2 == "" ? item.Name1 : item.Name2;
                            break;
                        case "ar":
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                        default:
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public static List<Employees> GetList(string DB, string xLan, int Kind)
        {
            List<Employees> items = new List<Employees>();
            string selQuery = "select top 100 percent [emp_Key],[emp_Code],[emp_Name1],[emp_Name2] from [hrCard_Employee] where [emp_JobKind]=@Kind order by [emp_Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Kind", SqlDbType.Int).Value = Kind;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Employees item = new Employees();
                    item.Key = iCore.IsDbNullRtNull(reader["emp_Key"]);
                    item.Code = Convert.ToString(reader["emp_Code"]);
                    item.Name1 = Convert.ToString(reader["emp_Name1"]);
                    item.Name2 = Convert.ToString(reader["emp_Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2 == "" ? item.Name1 : item.Name2;
                            break;
                        case "ar":
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                        default:
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
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
