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
    public class Biometric
    {
        public Guid? Key { get; set; }
        
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<Biometric> GetList(string DB, string xLan)
        {
            List<Biometric> items = new List<Biometric>();
            string selQuery = "select top 100 percent [BD_Key],[BD_Name1],[BD_Name2] from [hrAttendance_Biometric] order by [BD_Name1] ";
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
                    Biometric item = new Biometric();
                    item.Key = iCore.IsDbNullRtNull(reader["BD_Key"]);
                  
                    item.Name1 = Convert.ToString(reader["BD_Name1"]);
                    item.Name2 = Convert.ToString(reader["BD_Name2"]);
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
