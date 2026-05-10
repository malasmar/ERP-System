using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Sales.Platx
{
    public class VersionDetails
    {
        public string Version { get; set; }
        public bool Blocked { get; set; }
        public bool Mandatory { get; set; }
        public string URL { get; set; }
        public VersionDetails GetItem()
        {
            VersionDetails item = new VersionDetails();
            string selQuery = "select top(1) * from Application_Version";
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
                    item.Version = Convert.ToString(reader["ver_Version"]);
                    item.Blocked = Convert.ToBoolean(reader["ver_Blocked"]);
                    item.Mandatory = Convert.ToBoolean(reader["ver_Mandatory"]);
                    item.URL = Convert.ToString(reader["ver_URL"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
