using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.PL
{
    public class OTP
    {
        public Guid? Key { get; set; }
        public Guid? UserKey { get; set; }
        public string ClientID { get; set; }
        public string Username { get; set; }
        public string Code { get; set; }
        public OTP GetItem(Guid? Key)
        {
            OTP item = new OTP();
            if(Key==null)
                return item;

            string selQuery = "select top 100 percent * from px_LoginOTP where [otp_Key]=@Key";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key",SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["otp_Key"]);
                    item.UserKey = iCore.IsDbNullRtNull(reader["otp_UserKey"]);
                    item.ClientID = Convert.ToString(reader["otp_ClientID"]);
                    item.Username = Convert.ToString(reader["otp_Username"]);
                    item.Code = Convert.ToString(reader["otp_Code"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(OTP item)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO px_LoginOTP");
                str.Append("([otp_Key]");
                str.Append(",[otp_UserKey]");
                str.Append(",[otp_ClientID]");
                str.Append(",[otp_Username]");
                str.Append(",[otp_Code])");
                str.Append(" VALUES ");
                str.Append("(@otp_Key");
                str.Append(",@otp_UserKey");
                str.Append(",@otp_ClientID");
                str.Append(",@otp_Username");
                str.Append(",@otp_Code)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@otp_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@otp_UserKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.UserKey);
                comm.Parameters.Add("@otp_ClientID", SqlDbType.NVarChar, 10).Value = item.ClientID ?? "";
                comm.Parameters.Add("@otp_Username", SqlDbType.NVarChar, 200).Value = item.Username ?? "";
                comm.Parameters.Add("@otp_Code", SqlDbType.NVarChar, 6).Value = item.Code ?? "";
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void UpdateCode(Guid? Key,string code)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update px_LoginOTP SET ");
                str.Append(" [otp_Code]=@otp_Code");
                str.Append(" WHERE otp_Key=@otp_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@otp_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@otp_Code", SqlDbType.NVarChar, 6).Value = code ?? "";
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Delete(Guid? Key)
        {
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                string delQuery = " Delete from [px_LoginOTP] where [otp_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
    }
}
