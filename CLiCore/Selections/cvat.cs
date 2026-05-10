using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Selections
{
    public class cvat
    {
        public string CompanyName { get; set; }
        public string vatRNo { get; set; }
        public string CR { get; set; }
        public cvat GetItem(string DB)
        {
            cvat item = new cvat();
            string selQuery = "select top(1) [com_Name1],[com_RC],[com_vatRegNo] from com_Profile";
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
                    item.CompanyName = (string)reader["com_Name1"];
                    item.CR = (string)reader["com_RC"];
                    item.vatRNo = (string)reader["com_vatRegNo"];
                }
                reader.Close();
            }
            return item;
        }

    }
}
