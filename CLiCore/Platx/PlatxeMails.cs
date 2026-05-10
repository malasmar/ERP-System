using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Platx
{
    public class PlatxeMails
    {
        public int RecNo { get; set; }
        public string Key { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string SMTP { get; set; }
        public Boolean UseDefaultCredentials { get; set; }
        public Boolean EnableSSL { get; set; }
        public Boolean IsBodyHtml { get; set; }
        public PlatxeMails? GetItem(string Key)
        {
            if (Key == "" || Key == null)
                return null;

            PlatxeMails item = new PlatxeMails();
            string selQuery = "select top 100 percent * from px_SenderEmails where [em_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.NVarChar, 25).Value = Key ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.RecNo = Convert.ToInt32(reader["em_RecNo"]);
                    item.Key = Convert.ToString(reader["em_Key"]);
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

    }
}
