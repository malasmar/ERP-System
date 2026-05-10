using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Reports
{
    public class BalanceStatment
    {
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string parent { get; set; }
        public int Level { get; set; }
        public int Kind { get; set; }
        public int T { get; set; }
        public decimal PreviewAmount { get; set; }
        public decimal Amount { get; set; }
        public List<BalanceStatment> GetList(string DB, DateTime FirstDate, DateTime LastDate, int GroupLevel, int TransactionLevel)
        {
            List<BalanceStatment> items = new List<BalanceStatment>();
            string selQuery = "select top 100 percent * from dbo.fnaccReport_BalanceStatment(@FirstDate,@LastDate,@GroupLevel,@TransactionLevel) order by [Kind],[Code],[Level] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                com.Parameters.Add("@GroupLevel", SqlDbType.Int).Value = GroupLevel;
                com.Parameters.Add("@TransactionLevel", SqlDbType.Int).Value = TransactionLevel;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BalanceStatment item = new BalanceStatment();
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.parent = Convert.ToString(reader["parent"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.T = Convert.ToInt32(reader["T"]);
                    item.PreviewAmount = Convert.ToDecimal(reader["PreviewAmount"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
