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
    public class AccountStatment
    {
        public Guid? Key { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? AccountKey { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public DateTime? Date { get; set; }
        public int No { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }

        public List<AccountStatment> GetList(string DB,Guid? Key, DateTime FirstDate, DateTime LastDate, bool Opening)
        {
            decimal Balance=0;
            List<AccountStatment> items = new List<AccountStatment>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnaccReport_AccountStatment(@Key,@FirstDate,@LastDate,@Opening) order by [Date],[No],[DocumentKind],[VoucherNo] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                com.Parameters.Add("@Opening", SqlDbType.Bit).Value = Opening;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountStatment item = new AccountStatment();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.DebitBalance = Convert.ToDecimal(reader["DebitBalance"]);
                    item.CreditBalance = Convert.ToDecimal(reader["CreditBalance"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
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
