using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Platx
{
    public class UserDefaultData
    {
        public string Cash { get; set; }
        public string Prefix { get; set; }
        public string Branch { get; set; }
        public string CostCenter { get; set; }   
        public string Project { get; set; }
        public string Warehouse { get; set; }
    
        public UserDefaultData GetItem(string DB,string xLan,Guid? Key)
        {
            UserDefaultData item = new UserDefaultData();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnAppPlatx_UserDefaultData(@Key) ";
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
                    item.Cash = xLan=="en"? Convert.ToString(reader["CashName2"]): Convert.ToString(reader["CashName1"]);
                    item.Prefix = xLan == "en" ? Convert.ToString(reader["PrefixName2"]) : Convert.ToString(reader["PrefixName1"]); 
                    item.Branch = xLan == "en" ? Convert.ToString(reader["BranchName2"]) : Convert.ToString(reader["BranchName1"]); 
                    item.CostCenter = xLan == "en" ? Convert.ToString(reader["CostName2"]) : Convert.ToString(reader["CostName1"]); 
                    item.Project = xLan == "en" ? Convert.ToString(reader["ProjectName2"]) : Convert.ToString(reader["ProjectName1"]); 
                    item.Warehouse = xLan == "en" ? Convert.ToString(reader["Warehouse2"]) : Convert.ToString(reader["Warehouse1"]); 
                }
                reader.Close();
            }
            return item;
        }
    }
}
