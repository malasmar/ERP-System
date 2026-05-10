using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.CheckIntegration
{
    public class FinancialAccounts
    {
        public int Kind { get; set; }
        public Guid? Key { get; set; }
        public int Type { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Guid? Account { get; set; }
        public Guid? Parent { get; set; }
        public List<FinancialAccounts> GetList(string DB)
        {
            List<FinancialAccounts> items = new List<FinancialAccounts>();
            string selQuery = "select top 100 percent * from dbo.fnInvAudit_CardsIntegration() order by [Kind],[Code]";
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
                    FinancialAccounts item = new FinancialAccounts();
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Type = Convert.ToInt32(reader["Type"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Account = iCore.IsDbNullRtNull(reader["Account"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["Parent"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
