using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.Reports.Advance
{
    public class TransactionDetails
    {
        public int Row { get; set; }
        public int DocumentKind { get; set; }
        public Guid? OperationKey { get; set; }
        public DateTime? Date { get; set; }
        public int No { get; set; }
        public string VoucherDescription { get; set; }
        public int Index { get; set; }
        public int Kind1 { get; set; }
        public int Type1 { get; set; }
        public Guid? Key1 { get; set; }
        public string Code1 { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Kind2 { get; set; }
        public int Type2 { get; set; }
        public Guid? Key2 { get; set; }
        public string Acc2Code { get; set; }
        public string Acc2Name1 { get; set; }
        public string Acc2Name2 { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public decimal Amount { get; set; }
        public decimal vatRate { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }

        public List<TransactionDetails> GetList(string DB,int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<TransactionDetails> items = new List<TransactionDetails>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_TransactionsDetails(@FirstDate,@LastDate) order by [Date],[DocumentKind],[No]";
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
                    TransactionDetails item = new TransactionDetails();
                    item.Row = Convert.ToInt32(reader["Row"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.VoucherDescription = Convert.ToString(reader["VoucherDescription"]);
                    item.Index = Convert.ToInt32(reader["Index"]);
                    item.Kind1 = Convert.ToInt32(reader["Kind1"]);
                    item.Type1 = Convert.ToInt32(reader["Type1"]);
                    item.Key1 = iCore.IsDbNullRtNull(reader["Key1"]);
                    item.Code1 = Convert.ToString(reader["Code1"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Kind2 = Convert.ToInt32(reader["Kind2"]);
                    item.Type2 = Convert.ToInt32(reader["Type2"]);
                    item.Key2 = iCore.IsDbNullRtNull(reader["Key2"]);
                    item.Acc2Code = Convert.ToString(reader["Acc2Code"]);
                    item.Acc2Name1 = Convert.ToString(reader["Acc2Name1"]);
                    item.Acc2Name2 = Convert.ToString(reader["Acc2Name2"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
