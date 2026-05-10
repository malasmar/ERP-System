using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Data
{
    public class Structure
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Structure GetItem(string DB, Guid? Key)
        {
            Structure item = new Structure();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent [str_Name1],[str_Name2] from HRStructure_Organizational where [str_Key]=@Key";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Name1 = Convert.ToString(reader["str_Name1"]);
                    item.Name2 = Convert.ToString(reader["str_Name2"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
