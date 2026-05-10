using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Account
{
    public class Subscribe
    {
        public Guid? Key { get; set; }
        public Guid? Client { get; set; }
        public string ID { get; set; }
        public int Kind { get; set; }
        public DateTime? Create { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public Boolean Disable { get; set; }
        public int Users { get; set; }

        public Subscribe GetItem(string Key)
        {
            Subscribe item = new Subscribe();
            
            string selQuery = "select top 100 percent * from px_Subscribe where [sub_ID]=@ID ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@ID", SqlDbType.NVarChar, 10).Value = Key ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                   
                    item.Key = iCore.IsDbNullRtNull(reader["sub_Key"]);
                    item.Client = iCore.IsDbNullRtNull(reader["sub_Client"]);
                    item.ID = (string)reader["sub_ID"];
                    item.Kind = (int)reader["sub_Kind"];
                    item.Create = iCore.IsDbNullRtNullDate(reader["sub_Create"]);
                    item.JoinDate = iCore.IsDbNullRtNullDate(reader["sub_JoinDate"]);
                    item.EndDate = iCore.IsDbNullRtNullDate(reader["sub_EndDate"]);
                    item.Status = (int)reader["sub_Status"];
                    item.Disable = (Boolean)reader["sub_Disable"];
                    item.Users = (int)reader["sub_Users"];
                   
                }
                reader.Close();
            }
            return item;
        }
        public Subscribe GetItem(Guid? Key)
        {
            Subscribe item = new Subscribe();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from px_Subscribe where [sub_Key]=@Key ";
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

                    item.Key = iCore.IsDbNullRtNull(reader["sub_Key"]);
                    item.Client = iCore.IsDbNullRtNull(reader["sub_Client"]);
                    item.ID = (string)reader["sub_ID"];
                    item.Kind = (int)reader["sub_Kind"];
                    item.Create = iCore.IsDbNullRtNullDate(reader["sub_Create"]);
                    item.JoinDate = iCore.IsDbNullRtNullDate(reader["sub_JoinDate"]);
                    item.EndDate = iCore.IsDbNullRtNullDate(reader["sub_EndDate"]);
                    item.Status = (int)reader["sub_Status"];
                    item.Disable = (Boolean)reader["sub_Disable"];
                    item.Users = (int)reader["sub_Users"];

                }
                reader.Close();
            }
            return item;
        }

    }
}
