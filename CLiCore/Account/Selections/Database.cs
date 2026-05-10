using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLiCore;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Account.Selections
{
    public class Database
    {
        public Guid? Key { get; set; }
        public string? DatabaseName { get; set; }
        public string? Name1 { get; set; }
        public string? Name2 { get; set; }
        public string? Display { get; set; }
        public List<Database> GetList(string xLan,Guid? Key)
        {
            List<Database> items = new List<Database>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from px_SubscribeDataBase where [adb_Subscribe]=@Key";
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
                    Database item = new Database();
                    item.Key = iCore.IsDbNullRtNull(reader["adb_Key"]);
                    item.Name1 = (string)reader["adb_arDisplay"]??"";
                    item.Name2 = (string)reader["adb_enDisplay"]??"";
                    item.DatabaseName = (string)reader["adb_Database"] ?? "";
                    switch (xLan)
                    {
                        case "ar":
                            item.Display = item. Name1;
                            break;
                        case "en":
                            item.Display = item. Name2;
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Database GetItem(Guid? Key)
        {
            Database item = new Database();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from px_SubscribeDataBase where [adb_Key]=@Key";
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
                 
                    item.Key = iCore.IsDbNullRtNull(reader["adb_Key"]);
                    item.Name1 = (string)reader["adb_arDisplay"] ?? "";
                    item.Name2 = (string)reader["adb_enDisplay"] ?? "";
                    item.DatabaseName = (string)reader["adb_Database"] ?? "";
         
                }
                reader.Close();
            }
            return item;
        }
        public static List<Database> GetData(string xLan, Guid? Key)
        {
            List<Database> items = new List<Database>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from px_SubscribeDataBase where [adb_Subscribe]=@Key";
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
                    Database item = new Database();
                    item.Key = iCore.IsDbNullRtNull(reader["adb_Key"]);
                    item.Name1 = (string)reader["adb_arDisplay"] ?? "";
                    item.Name2 = (string)reader["adb_enDisplay"] ?? "";
                    item.DatabaseName = (string)reader["adb_Database"] ?? "";
                    switch (xLan)
                    {
                        case "ar":
                            item.Display = item.Name1;
                            break;
                        case "en":
                            item.Display = item.Name2;
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
