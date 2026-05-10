using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLiCore;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using CLiCore.Platx;

namespace CLiCore.Account
{
    public class core
    {
        public static List<string> UserAccessPermissions(string DB, Guid? Key)
        {
            List<string> result = new List<string>();
            string selQuery = "SELECT top 100 percent * from dbo.sysUser_AccessPermissions(@Key)  ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader[0].ToString()??"");
                };
                reader.Close();
            }
            return result;
        }
        public static List<string> SubscribeModuals(Guid? Key)
        {
            List<string> result = new List<string>();
            string selQuery = "SELECT top 100 percent [sys_ModuleId] from  [px_SubscribeModules] where [sys_Subscribe]=@Key  ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader[0].ToString()??"");
                };
                reader.Close();
            }
            return result;
        }
        public static bool CheckUserIfDisable(Guid? Key)
        {
            bool result = false;
            string selQuery = "SELECT top 100 percent [user_Disable] from [px_Users] where [user_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (bool)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static bool CheckUserPassword(Guid? UserKey, string Password)
        {
            string Pass = "";
            string selQuery = "SELECT top 100 percent [user_Passwoard] from [px_Users] where [user_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = UserKey;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Pass = Convert.ToString(reader[0]) ?? "";
                };
                reader.Close();
            }

            return Pass == Password;
        }

        public static void UpdateUserPassword(Guid? UserKey, string Password)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update px_Users SET ");
                str.Append(" [user_Passwoard]=@user_Passwoard");
                str.Append(" WHERE user_Key=@user_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@user_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(UserKey);
                comm.Parameters.Add("@user_Passwoard", SqlDbType.NVarChar, 200).Value = Password ?? "";
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
    }
}
