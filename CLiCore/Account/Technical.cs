using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Account
{
    public class Technical
    {
        public Guid? Key { get; set; }
        public Guid? Subscribe { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Boolean Disable { get; set; }
        public Guid? DbKey { get; set; }
        public string DbDisplay1 { get; set; }
        public string DbDisplay2 { get; set; }
        public string Database { get; set; }

        public Technical Login(string Key,Guid? Subscribe)
        {
            Technical item = new Technical();
            string selQuery = "select top 100 percent * from dbo.fnpx_AppTechnicalLogin(@Key,@Subscribe) ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.NVarChar, 255).Value = Key ?? "";
                com.Parameters.Add("@Subscribe", SqlDbType.UniqueIdentifier).Value = Subscribe;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Subscribe = iCore.IsDbNullRtNull(reader["Subscribe"]);
                    item.No = (int)reader["No"];
                    item.Name1 = (string)reader["Name1"];
                    item.Name2 = (string)reader["Name2"];
                    item.Password = (string)reader["Password"];
                    item.Email = (string)reader["Email"];
                    item.Phone = (string)reader["Phone"];
                    item.Disable = (Boolean)reader["Disable"];
                    item.DbKey = iCore.IsDbNullRtNull(reader["DbKey"]);
                    item.DbDisplay1 = (string)reader["DbDisplay1"];
                    item.DbDisplay2 = (string)reader["DbDisplay2"];
                    item.Database = (string)reader["Database"];
                }
                reader.Close();
            }
            return item;
        }
       
    }
}
