using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
 

namespace CLiCore.Shared
{
    public class UserDefaultData
    {
        public Guid? Key { get; set; }
        public Guid? Employee { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public Guid? Cash { get; set; }
        public int WarehouseNo { get; set; }
 

        public static UserDefaultData GetItem(Guid Key)
        {
            UserDefaultData item = new UserDefaultData();
            string selQuery = "select top 100 percent [user_Key],[user_Employee],[user_Branch],[user_Prefix],[user_CostCenter],[user_Project],[user_Cash],[user_WarehouseNo] from px_Users where  [user_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
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
                    item.Key = iCore.IsDbNullRtNull(reader["user_Key"]);
                    item.Employee = iCore.IsDbNullRtNull(reader["user_Employee"]);
                    item.Branch = Convert.ToInt32(reader["user_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["user_Prefix"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["user_CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["user_Project"]);
                    item.Cash = iCore.IsDbNullRtNull(reader["user_Cash"]);
                    item.WarehouseNo = Convert.ToInt32(reader["user_WarehouseNo"]);  
                }
                reader.Close();
            }
            return item;
        }
    }
}
