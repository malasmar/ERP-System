using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Reports.vat
{
    public class vatStatment
    {
        public Guid? OperationKey { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public int MonthlyNo { get; set; }
        public DateTime? Date { get; set; }
        public Guid? vatKey { get; set; }
        public int Kind { get; set; }
        public decimal Rate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public string vatRegNo { get; set; }
        public string AccountName { get; set; }

        public string Description { get; set; }
        public string strMonthlyNo { get; set; }
        public List<vatStatment> GetListDetails(string DB, DateTime FirstDate, DateTime LastDate)
        {
            decimal Balance = 0;
            List<vatStatment> items = new List<vatStatment>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_vatStatmentDetails(@FirstDate,@LastDate) order by [Date],[VoucherNo],[DocumentKind]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    vatStatment item = new vatStatment();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Rate = Convert.ToDecimal(reader["Rate"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
           
                    item.vatRegNo = Convert.ToString(reader["vatRegNo"]);
                    item.AccountName = Convert.ToString(reader["AccountName"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.strMonthlyNo = item.Date.Value.ToString("MM") + "-" + item.MonthlyNo.ToString("0000");
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
        public List<vatStatment> GetListSummary(string DB, DateTime FirstDate, DateTime LastDate)
        {
            decimal Balance = 0;
            List<vatStatment> items = new List<vatStatment>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_vatStatment(@FirstDate,@LastDate) order by [Date],[VoucherNo],[DocumentKind]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    vatStatment item = new vatStatment();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Rate = Convert.ToDecimal(reader["Rate"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
             
                    item.Description = Convert.ToString(reader["Description"]);
                    item.strMonthlyNo = item.Date.Value.ToString("MM") + "-" + item.MonthlyNo.ToString("0000");
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
