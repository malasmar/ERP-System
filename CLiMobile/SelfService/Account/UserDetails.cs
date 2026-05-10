using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Account
{
    public class UserDetails
    {
        public Guid? Key { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid? Subscribe { get; set; }
        public string Database { get; set; }
        public string DbDisplay1 { get; set; }
        public string DbDisplay2 { get; set; }
        public Boolean Disable { get; set; }

        public UserDetails GetItem(string Key)
        {
            UserDetails item = new UserDetails();
            if (Key == "" || Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnpx_SelfServiceLogin(@Key) ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.NVarChar, 500).Value = Key??"";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Phone = Convert.ToString(reader["Phone"]);
                    item.Email = Convert.ToString(reader["Email"]);
                    item.Password = Convert.ToString(reader["Password"]);
                    item.Subscribe = iCore.IsDbNullRtNull(reader["Subscribe"]);
                    item.Database = Convert.ToString(reader["Database"]);
                    item.DbDisplay1 = Convert.ToString(reader["DbDisplay1"]);
                    item.DbDisplay2 = Convert.ToString(reader["DbDisplay2"]);
                    item.Disable = Convert.ToBoolean(reader["Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public UserDetails GetItem(Guid? Key)
        {
            UserDetails item = new UserDetails();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnpx_SelfServiceUser(@Key) ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Phone = Convert.ToString(reader["Phone"]);
                    item.Email = Convert.ToString(reader["Email"]);
                    item.Password = Convert.ToString(reader["Password"]);
                    item.Subscribe = iCore.IsDbNullRtNull(reader["Subscribe"]);
                    item.Database = Convert.ToString(reader["Database"]);
                    item.DbDisplay1 = Convert.ToString(reader["DbDisplay1"]);
                    item.DbDisplay2 = Convert.ToString(reader["DbDisplay2"]);
                    item.Disable = Convert.ToBoolean(reader["Disable"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
