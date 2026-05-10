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
    public class Profile
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string ProfileImage { get; set; }
        public Profile GetItem(string DB,Guid? Key)
        {
            Profile item = new Profile();
            if(Key==null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_EmployeeProfile(@Key) ";
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
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name = Convert.ToString(reader["Name"]);
                    item.JobTitle = Convert.ToString(reader["JobTitle"]);
                    item.ProfileImage = Convert.ToString(reader["ProfileImage"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
