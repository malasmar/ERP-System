using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Platx
{
    public class Whatsapp
    {
        public Guid? Key { get; set; }
        public string APIURL { get; set; }
        public string InstanceID { get; set; }
        public string Token { get; set; }
        public Boolean Status { get; set; }
        public Whatsapp GetItem(string DB)
        {
            Whatsapp item = new Whatsapp();
            string selQuery = "select top(1) * from com_Whatsapp";
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
        public static void Insert(string DB, Whatsapp item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("delete from com_Whatsapp ");
                str.Append("INSERT INTO com_Whatsapp ");
                str.Append("([com_APIURL]");
                str.Append(",[com_InstanceID]");
                str.Append(",[com_Token]");
                str.Append(",[com_Status])");
                str.Append(" VALUES ");
                str.Append("(@com_APIURL");
                str.Append(",@com_InstanceID");
                str.Append(",@com_Token");
                str.Append(",@com_Status)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@com_APIURL", SqlDbType.NVarChar, 500).Value = item.APIURL ?? "";
                comm.Parameters.Add("@com_InstanceID", SqlDbType.NVarChar, 500).Value = item.InstanceID ?? "";
                comm.Parameters.Add("@com_Token", SqlDbType.NVarChar, 1000).Value = item.Token ?? "";
                comm.Parameters.Add("@com_Status", SqlDbType.Bit).Value = item.Status;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
    }
}
