using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiSales.JsonData
{
    public class Project
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
        public string Display { get; set; }

        public Project GetItem(string DB,string xLan,Guid? Key)
        {
            Project item = new Project();
            item.Display = "";
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from dbo.fnaccJson_Project(@Key) ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Mobile = Convert.ToString(reader["Mobile"]);
                    item.Phone = Convert.ToString(reader["Phone"]);
                    item.ContactPerson = Convert.ToString(reader["ContactPerson"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = Convert.ToString(reader["Name2"]);
                            break;
                        case "ar":
                            item.Display = Convert.ToString(reader["Name1"]);
                            break;
                        default:
                            item.Display = Convert.ToString(reader["Name1"]) ;
                            break;
                    }
                }
                reader.Close();
            }
            return item;
        }
    }
}
