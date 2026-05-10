using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.CardsInfo
{
    public class User
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string? Name1 { get; set; }
        public string? Name2 { get; set; }
        public string? Display { get; set; }
        public User GetItem(string xLan, Guid? Key)
        {
            User item = new User();
         
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent [user_Key],[user_No],[user_Name1],[user_Name2] from  [px_Users] where [user_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
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
                    item.Key = iCore.IsDbNullRtNull(reader["user_Key"]);
                    item.No = Convert.ToInt32(reader["user_No"]);
                    item.Name1 = Convert.ToString(reader["user_Name1"])??"";
                    item.Name2 = Convert.ToString(reader["user_Name2"])??"";
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
                }
                reader.Close();
            }
            return item;
        }
        public User GetItem(string xLan, int Key)
        {
            User item = new User();

            if (Key == 0)
                return item;
            string selQuery = "select top 100 percent [user_Key],[user_No],[user_Name1],[user_Name2] from  [px_Users] where [user_No]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.Int).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["user_Key"]);
                    item.No = Convert.ToInt32(reader["user_No"]);
                    item.Name1 = Convert.ToString(reader["user_Name1"]) ?? "";
                    item.Name2 = Convert.ToString(reader["user_Name2"]) ?? "";
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
                }
                reader.Close();
            }
            return item;
        }
    }
}
