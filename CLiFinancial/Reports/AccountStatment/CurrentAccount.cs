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
    public class CurrentAccount
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public int MonthlyNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public int CounterKind { get; set; }
        public Guid? CounterKey { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public int T { get; set; }
        public int AccountType { get; set; }
        public string strMonthlyNo { get; set; }
        public List<CurrentAccount> GetList(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening)
        {
            decimal Balance = 0;
            List<CurrentAccount> items = new List<CurrentAccount>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.ReportFin_AccountStatment(@Key,@FirstDate,@LastDate,@Opening) order by [VoucherDate],[DocumentKind],[VoucherNo]";
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
                    CurrentAccount item = new CurrentAccount();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.CounterKind = Convert.ToInt32(reader["CounterKind"]);
                    item.CounterKey = iCore.IsDbNullRtNull(reader["CounterKey"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
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
        public List<CurrentAccount> GetList(string DB, Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening,string Kind)
        {
            decimal Balance = 0;
            List<CurrentAccount> items = new List<CurrentAccount>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from dbo.ReportFin_EmployeeStatment(@Key,@FirstDate,@LastDate,@Opening,@Kind) order by [VoucherDate],[DocumentKind],[VoucherNo]";
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
                com.Parameters.Add("@Kind", SqlDbType.NVarChar,25).Value = Kind??"";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CurrentAccount item = new CurrentAccount();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.CounterKind = Convert.ToInt32(reader["CounterKind"]);
                    item.CounterKey = iCore.IsDbNullRtNull(reader["CounterKey"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
                    item.T = Convert.ToInt32(reader["T"]);
                    item.AccountType = Convert.ToInt32(reader["AccountType"]);
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
