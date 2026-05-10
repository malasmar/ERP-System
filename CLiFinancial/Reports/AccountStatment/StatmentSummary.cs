using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Reports.AccountStatment
{
    public class StatmentSummary
    {
        public Guid? OperationKey { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public int MonthlyNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public int T { get; set; }
        public string strMonthlyNo { get; set; }

        public List<StatmentSummary> GetList(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            decimal Balance = 0;
            List<StatmentSummary> items = new List<StatmentSummary>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.ReportFin_AccountStatmentSummary(@Key,@FirstDate,@LastDate,@Opening) order by [VoucherDate],[DocumentKind],[VoucherNo]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(FirstDate);
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = iCore.IsNullRtDbNull(LastDate);
                com.Parameters.Add("@Opening", SqlDbType.Bit).Value = Opening;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    StatmentSummary item = new StatmentSummary();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.DebitBalance = Convert.ToDecimal(reader["DebitBalance"]);
                    item.CreditBalance = Convert.ToDecimal(reader["CreditBalance"]);
                    item.T = Convert.ToInt32(reader["T"]);
                    item.strMonthlyNo = item.VoucherDate.Value.ToString("MM") + "-" + item.MonthlyNo.ToString("0000");
                    Balance += item.Debit - item.Credit;
                    if (Balance > 0)
                    {
                        item.DebitBalance = Balance;
                    }
                    else
                    {
                        item.CreditBalance = Math.Abs(Balance);
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
