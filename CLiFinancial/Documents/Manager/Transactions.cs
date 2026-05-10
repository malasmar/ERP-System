using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Documents.Manager
{
    public class Transactions
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public Guid? Category { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int MonthlyNo { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int AccountKind { get; set; }
        public Guid? AccountKey { get; set; }
        public string Description { get; set; }
        public string TransactionNo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public int Rows { get; set; }
        public Boolean IcloudExp { get; set; }
        public Boolean Posted { get; set; }
        public string strMonthlyNo { get; set; }
        public List<Transactions> GetList(string DB,int Year,int DocumentKind)
        {
            DateTime first = new DateTime(Year, 1, 1);
            DateTime last = new DateTime(Year, 12, 31);
            List<Transactions> items = new List<Transactions>();
            string selQuery = "select top 100 percent * from dbo.fnfinDocuments_ListTransaction(@FirstDate,@LastDate,@DocumentKind) order by [DocumentKind],[VoucherDate] desc,[VoucherNo] desc ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = first;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = last;
                com.Parameters.Add("@DocumentKind", SqlDbType.Int).Value = DocumentKind;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Transactions item = new Transactions();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.Category = iCore.IsDbNullRtNull(reader["Category"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.ReferenceDate = iCore.IsDbNullRtNullDate(reader["ReferenceDate"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["DueDate"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.TransactionNo = Convert.ToString(reader["TransactionNo"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Rows = Convert.ToInt32(reader["Rows"]);
                    item.IcloudExp = Convert.ToBoolean(reader["IcloudExp"]);
                    item.Posted = Convert.ToBoolean(reader["Posted"]);
                    item.strMonthlyNo = item.VoucherDate.Value.ToString("MM") + "-" + item.MonthlyNo.ToString("0000")   ;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
