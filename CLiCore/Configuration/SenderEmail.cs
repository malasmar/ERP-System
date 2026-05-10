using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Configuration
{
	public class SenderEmail
	{
        public int RecNo { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string SMTP { get; set; }
        public Boolean UseDefaultCredentials { get; set; }
        public Boolean EnableSsl { get; set; }
        public Boolean IsBodyHtml { get; set; }
        public SenderEmail GetItem()
        {
            SenderEmail item = new SenderEmail();
            string selQuery = "select top(1) * from px_SenderEmail";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
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
                    item.EnableSsl = Convert.ToBoolean(reader["em_EnableSsl"]);
                    item.IsBodyHtml = Convert.ToBoolean(reader["em_IsBodyHtml"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
