using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.PL
{
    public class Whatsapp
    {
        public Guid? Key { get; set; }
        public string APIURL { get; set; }
        public string InstanceID { get; set; }
        public string Token { get; set; }
        public Boolean Status { get; set; }
        public Whatsapp GetItem()
        {
            Whatsapp item = new Whatsapp();
            string selQuery = "select top(1) * from px_Whatsapp";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(iCore.Conn)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Key = iCore.IsDbNullRtNull(reader["com_Key"]);
                    item.APIURL = Convert.ToString(reader["com_APIURL"]);
                    item.InstanceID = Convert.ToString(reader["com_InstanceID"]);
                    item.Token = Convert.ToString(reader["com_Token"]);
                    item.Status = Convert.ToBoolean(reader["com_Status"]);
                }
                reader.Close();
            }
            return item;
        }
     
    }
}
