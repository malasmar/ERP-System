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
    public class Department
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Department GetItem(string DB, Guid? Key)
        {
            Department item = new Department();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent [dep_Name1],[dep_Name2] from hrCard_Department where [dep_Key]=@Key";
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
                    item.Name1 = Convert.ToString(reader["dep_Name1"]);
                    item.Name2 = Convert.ToString(reader["dep_Name2"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
