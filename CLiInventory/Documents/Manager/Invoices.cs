using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Documents.Manager
{
    public class Invoices
    {
        public Guid? OperationKey { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int SourceWarehouse { get; set; }
        public int TargetWarehouse { get; set; }
        public int DocumentKInd { get; set; }
        public int InvoiceKind { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int MonthlyNo { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? DueDate { get; set; }
        public int AccountKind { get; set; }
        public Guid? AccountKey { get; set; }
        public Guid? CurrentKey { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public Boolean IncFxd { get; set; }
        public Boolean IncExp { get; set; }
        public Boolean Posted { get; set; }
        public Boolean Retrned { get; set; }
        public decimal ClosedAmount { get; set; }
        public decimal ReturnedAmount { get; set; }
        public decimal Total { get; set; }
        public decimal RemAmount { get; set; }
        public int Source { get; set; }
        public List<Invoices> GetList(string DB,Guid? UserKey,int Year,int DocumentKind)
        {
            DateTime first = new DateTime(Year, 1, 1);
            DateTime last = new DateTime(Year, 12, 31);
            List<Invoices> items = new List<Invoices>();
            Guid? UserPrefix = null;
            if (xConfig.UserPrefixFilter(UserKey) == true)
            {
                UserPrefix = xConfig.UserPrefix(UserKey);
            }
            string selQuery = "select top 100 percent * from dbo.fninvDocuments_ListInvoices(@FirstDate,@LastDate,@DocumentKind) where ([Prefix]=@Prefix or @Prefix is null ) order by [DocumentKind],[InvoiceDate] desc,[InvoiceNo] desc ";
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
                com.Parameters.Add("@Prefix", SqlDbType.UniqueIdentifier).Value =iCore.IsNullRtDbNull(UserPrefix);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Invoices item = new Invoices();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["SourceWarehouse"]);
                    item.TargetWarehouse = Convert.ToInt32(reader["TargetWarehouse"]);
                    item.DocumentKInd = Convert.ToInt32(reader["DocumentKInd"]);
                    item.InvoiceKind = Convert.ToInt32(reader["InvoiceKind"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["DueDate"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.CurrentKey = iCore.IsDbNullRtNull(reader["CurrentKey"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Currency = Convert.ToString(reader["Currency"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["BonusAmount"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["BonusQuantity"]);
                    item.IncFxd = Convert.ToBoolean(reader["IncFxd"]);
                    item.IncExp = Convert.ToBoolean(reader["IncExp"]);
                    item.Posted = Convert.ToBoolean(reader["Posted"]);
                    item.Retrned = Convert.ToBoolean(reader["Retrned"]);
                    item.ClosedAmount = Convert.ToDecimal(reader["ClosedAmount"]);
                    item.ReturnedAmount = Convert.ToDecimal(reader["ReturnedAmount"]);
                    item.Source = Convert.ToInt32(reader["Source"]);
                    item.Total = item.Subtotal + item.vatAmount - item.Discount;
                    item.RemAmount = item.Total - item.ClosedAmount - item.ReturnedAmount;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
