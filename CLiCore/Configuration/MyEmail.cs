using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Configuration
{
    public class MyEmail
    {
        public int RecNo { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string SMTP { get; set; }
        public Boolean UseDefaultCredentials { get; set; }
        public Boolean EnableSSL { get; set; }
        public Boolean IsBodyHtml { get; set; }

        public MyEmail GetItem(string DB)
        {
            MyEmail item = new MyEmail();
            string selQuery = "select top(1) * from com_Email";
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
                    item.RecNo = Convert.ToInt32(reader["em_RecNo"]);
                    item.Email = Convert.ToString(reader["em_Email"]);
                    item.UserName = Convert.ToString(reader["em_UserName"]);
                    item.Password = Convert.ToString(reader["em_Password"]);
                    item.Port = Convert.ToInt32(reader["em_Port"]);
                    item.SMTP = Convert.ToString(reader["em_SMTP"]);
                    item.UseDefaultCredentials = Convert.ToBoolean(reader["em_UseDefaultCredentials"]);
                    item.EnableSSL = Convert.ToBoolean(reader["em_EnableSSL"]);
                    item.IsBodyHtml = Convert.ToBoolean(reader["em_IsBodyHtml"]);
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB, MyEmail item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("delete from com_Email ");
                str.Append("INSERT INTO com_Email ");
                str.Append("([em_Email]");
                str.Append(",[em_UserName]");
                str.Append(",[em_Password]");
                str.Append(",[em_Port]");
                str.Append(",[em_SMTP]");
                str.Append(",[em_UseDefaultCredentials]");
                str.Append(",[em_EnableSSL]");
                str.Append(",[em_IsBodyHtml])");
                str.Append(" VALUES ");
                str.Append("(@em_Email");
                str.Append(",@em_UserName");
                str.Append(",@em_Password");
                str.Append(",@em_Port");
                str.Append(",@em_SMTP");
                str.Append(",@em_UseDefaultCredentials");
                str.Append(",@em_EnableSSL");
                str.Append(",@em_IsBodyHtml)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@em_Email", SqlDbType.NVarChar, 200).Value = item.Email ?? "";
                comm.Parameters.Add("@em_UserName", SqlDbType.NVarChar, 200).Value = item.UserName ?? "";
                comm.Parameters.Add("@em_Password", SqlDbType.NVarChar, 200).Value = item.Password ?? "";
                comm.Parameters.Add("@em_Port", SqlDbType.Int).Value = item.Port;
                comm.Parameters.Add("@em_SMTP", SqlDbType.NVarChar, 200).Value = item.SMTP ?? "";
                comm.Parameters.Add("@em_UseDefaultCredentials", SqlDbType.Bit).Value = item.UseDefaultCredentials;
                comm.Parameters.Add("@em_EnableSSL", SqlDbType.Bit).Value = item.EnableSSL;
                comm.Parameters.Add("@em_IsBodyHtml", SqlDbType.Bit).Value = item.IsBodyHtml;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
    }
}
