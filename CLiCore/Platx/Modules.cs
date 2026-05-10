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
    public class Modules
    {
        public string id { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public menustate state { get; set; }
        public List<Modules> children { get; set; }

        public   List<Modules> GetList(string DB, string Lan, int IsParent, string Parent, Guid? User)
        {
            List<Modules> SHLL = new List<Modules>();
            string selQuery = "SELECT TOP 100 PERCENT  * from dbo.fnPlx_UserModules(@IsParent,@Parent,@User)";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@IsParent", SqlDbType.Int).Value = IsParent;
                com.Parameters.Add("@Parent", SqlDbType.NVarChar, 200).Value = Parent;
                com.Parameters.Add("@User", SqlDbType.UniqueIdentifier).Value =iCore.IsNullRtDbNull(User);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Modules item = new Modules();
                    item.id = (string)reader["ID"];
                    switch (Lan)
                    {
                        case "en":
                            item.text = (string)reader["English"];
                            break;
                        case "ar":
                            item.text = (string)reader["Arabic"];
                            break;
                    }

                    item.state = new menustate() { selected = (bool)reader["Selected"], opened = false };
                    item.children = new Modules().GetList(DB, Lan, 0, item.id, User);
                    SHLL.Add(item);
                }
                reader.Close();
            }
            return SHLL;
        }
        public static void InsertUserRights(string DB, Guid? Key, List<Modules> items)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                con.Open();
                ScStr.Clear();
                ScStr.Append("delete from SHL_UserModualDetails where [sys_UserKey]=@Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = ScStr.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                comm.ExecuteNonQuery();
                foreach (Modules item in items)
                {
                    ScStr.Clear();
                    ScStr.Append("INSERT INTO SHL_UserModualDetails");
                    ScStr.Append("([sys_UserKey]");
                    ScStr.Append(",[sys_MenuId])");
                    ScStr.Append(" VALUES ");
                    ScStr.Append("(@sys_UserKey");
                    ScStr.Append(",@sys_MenuId)");
                    comm = new SqlCommand();
                    comm.Connection = con;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = ScStr.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@sys_UserKey", SqlDbType.UniqueIdentifier).Value = Key;
                    comm.Parameters.Add("@sys_MenuId", SqlDbType.NVarChar, 200).Value = item.id ?? "";
                    comm.ExecuteNonQuery();
                }
            }
        }
    }
}
