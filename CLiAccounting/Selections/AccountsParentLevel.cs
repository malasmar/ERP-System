using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Selections
{
    public class AccountsParentLevel
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Display { get; set; }

        public static List<AccountsParentLevel> GetList(string DB,string xLan)
        {
            List<AccountsParentLevel> items = new List<AccountsParentLevel>();
            string selQuery = "select top 100 percent * from dbo.fnaccSelections_ParentLevel() order by [Code] ";
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
                    AccountsParentLevel item = new AccountsParentLevel();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = Convert.ToString(reader["Name2"]) + " (" + item.Code + ")";
                            break;
                        case "ar":
                            item.Display = Convert.ToString(reader["Name1"]) + " (" + item.Code + ")";
                            break;
                        default:
                            item.Display = Convert.ToString(reader["Name1"]) + " (" + item.Code + ")";
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
