using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.Reports.CostCenter
{
    public class IncomeSummary
    {
        public int Kind { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Balance { get; set; }

        public List<IncomeSummary> GetList(string DB,Guid? Key)
        {
            List<IncomeSummary> items = new List<IncomeSummary>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.ReportFin_CostCenterIncomeSummary(@Key) order by [Kind] desc,[Code]";
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
                    IncomeSummary item = new IncomeSummary();
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
