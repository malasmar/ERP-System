using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiCore.Platx
{
    public class Technical
    {
        public Guid? Key { get; set; }
        public Guid? Subscribe { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Passwoard { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid? DataBase { get; set; }
        public Guid? City { get; set; }
        public Boolean Disable { get; set; }

        public List<Technical> GetList(Guid Key)
        {
            List<Technical> items = new List<Technical>();
            string selQuery = "select top 100 percent * from PxTracking_Technical where  [Tech_Subscribe]=@Key order by [Tech_No] ";
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
                    Technical item = new Technical();
                    item.Key = iCore.IsDbNullRtNull(reader["Tech_Key"]);
                    item.Subscribe = iCore.IsDbNullRtNull(reader["Tech_Subscribe"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["Tech_CreateDate"]);
                    item.UpdateDate = iCore.IsDbNullRtNullDate(reader["Tech_UpdateDate"]);
                    item.No = Convert.ToInt32(reader["Tech_No"]);
                    item.Name1 = Convert.ToString(reader["Tech_Name1"]);
                    item.Name2 = Convert.ToString(reader["Tech_Name2"]);
                    item.Passwoard = Convert.ToString(reader["Tech_Passwoard"]);
                    item.Email = Convert.ToString(reader["Tech_Email"]);
                    item.Phone = Convert.ToString(reader["Tech_Phone"]);
                    item.DataBase = iCore.IsDbNullRtNull(reader["Tech_DataBase"]);
                    item.City = iCore.IsDbNullRtNull(reader["Tech_City"]);
                    item.Disable = Convert.ToBoolean(reader["Tech_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Technical GetItem(Guid? Key,Guid subscribe)
        {
            Technical item = new Technical();
            item.Subscribe = subscribe;
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from PxTracking_Technical where [Tech_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Tech_Key"]);
                    item.Subscribe = iCore.IsDbNullRtNull(reader["Tech_Subscribe"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["Tech_CreateDate"]);
                    item.UpdateDate = iCore.IsDbNullRtNullDate(reader["Tech_UpdateDate"]);
                    item.No = Convert.ToInt32(reader["Tech_No"]);
                    item.Name1 = Convert.ToString(reader["Tech_Name1"]);
                    item.Name2 = Convert.ToString(reader["Tech_Name2"]);
                    item.Passwoard = Convert.ToString(reader["Tech_Passwoard"]);
                    item.Email = Convert.ToString(reader["Tech_Email"]);
                    item.Phone = Convert.ToString(reader["Tech_Phone"]);
                    item.DataBase = iCore.IsDbNullRtNull(reader["Tech_DataBase"]);
                    item.City = iCore.IsDbNullRtNull(reader["Tech_City"]);
                    item.Disable = Convert.ToBoolean(reader["Tech_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Technical item)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO PxTracking_Technical");
                str.Append("([Tech_Subscribe]");
                str.Append(",[Tech_CreateDate]");
                str.Append(",[Tech_UpdateDate]");
                str.Append(",[Tech_No]");
                str.Append(",[Tech_Name1]");
                str.Append(",[Tech_Name2]");
                str.Append(",[Tech_Passwoard]");
                str.Append(",[Tech_Email]");
                str.Append(",[Tech_Phone]");
                str.Append(",[Tech_DataBase]");
                str.Append(",[Tech_City]");
                str.Append(",[Tech_Disable])");
                str.Append(" VALUES ");
                str.Append("(@Tech_Subscribe");
                str.Append(",@Tech_CreateDate");
                str.Append(",@Tech_UpdateDate");
                str.Append(",(select top 100 percent isnull(max(x.Tech_No +1),1) from PxTracking_Technical x  )");
                str.Append(",@Tech_Name1");
                str.Append(",@Tech_Name2");
                str.Append(",@Tech_Passwoard");
                str.Append(",@Tech_Email");
                str.Append(",@Tech_Phone");
                str.Append(",@Tech_DataBase");
                str.Append(",@Tech_City");
                str.Append(",@Tech_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Tech_Subscribe", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Subscribe);
                comm.Parameters.Add("@Tech_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@Tech_UpdateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.UpdateDate);
                comm.Parameters.Add("@Tech_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Tech_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Tech_Passwoard", SqlDbType.NVarChar, 200).Value = item.Passwoard ?? "";
                comm.Parameters.Add("@Tech_Email", SqlDbType.NVarChar, 500).Value = item.Email ?? "";
                comm.Parameters.Add("@Tech_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@Tech_DataBase", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.DataBase);
                comm.Parameters.Add("@Tech_City", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.City);
                comm.Parameters.Add("@Tech_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Technical item)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update PxTracking_Technical SET ");
                str.Append("[Tech_Subscribe]=@Tech_Subscribe");
                str.Append(",[Tech_CreateDate]=@Tech_CreateDate");
                str.Append(",[Tech_UpdateDate]=@Tech_UpdateDate");
                str.Append(",[Tech_Name1]=@Tech_Name1");
                str.Append(",[Tech_Name2]=@Tech_Name2");
                str.Append(",[Tech_Email]=@Tech_Email");
                str.Append(",[Tech_Phone]=@Tech_Phone");
                str.Append(",[Tech_DataBase]=@Tech_DataBase");
                str.Append(",[Tech_City]=@Tech_City");
                str.Append(",[Tech_Disable]=@Tech_Disable");
                str.Append(" WHERE Tech_Key=@Tech_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Tech_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@Tech_Subscribe", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Subscribe);
                comm.Parameters.Add("@Tech_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.CreateDate);
                comm.Parameters.Add("@Tech_UpdateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.UpdateDate);
                comm.Parameters.Add("@Tech_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Tech_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Tech_Email", SqlDbType.NVarChar, 500).Value = item.Email ?? "";
                comm.Parameters.Add("@Tech_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                comm.Parameters.Add("@Tech_DataBase", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.DataBase);
                comm.Parameters.Add("@Tech_City", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.City);
                comm.Parameters.Add("@Tech_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                string delQuery = " Delete from [PxTracking_Technical] where [Tech_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
