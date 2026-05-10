using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Documents.Selections
{
    public class Invoices
    {
        public Guid? OperationKey { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int SourceWarehouse { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int MonthlyNo { get; set; }
        public string ReferenceNo { get; set; }
        public int AccountKind { get; set; }
        public Guid? AccountKey { get; set; }
        public Guid? CurrentKey { get; set; }
        public string Currency { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public Boolean Posted { get; set; }
        public Boolean Retrned { get; set; }
        public decimal ClosedAmount { get; set; }
        public decimal ReturnedAmount { get; set; }
        public decimal Total { get; set; }
        public decimal RemAmount { get; set; }
        public Guid? SalesPerson { get; set; }
        public List<Invoices> GetList(string DB, int DocumentKind)
        {
            List<Invoices> items = new List<Invoices>();
            string selQuery = "select top 100 percent * from dbo.fninvDocuments_ListShortInvoices(@DocumentKind) order by [InvoiceDate] desc,[InvoiceNo] desc ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@DocumentKind", SqlDbType.Int).Value = DocumentKind;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Invoices item = new Invoices();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["SourceWarehouse"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.ReferenceNo = Convert.ToString(reader["ReferenceNo"]);
                    item.AccountKind = Convert.ToInt32(reader["AccountKind"]);
                    item.AccountKey = iCore.IsDbNullRtNull(reader["AccountKey"]);
                    item.CurrentKey = iCore.IsDbNullRtNull(reader["CurrentKey"]);
                    item.Currency = Convert.ToString(reader["Currency"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["BonusAmount"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["BonusQuantity"]);
                    item.Posted = Convert.ToBoolean(reader["Posted"]);
                    item.Retrned = Convert.ToBoolean(reader["Retrned"]);
                    item.ClosedAmount = Convert.ToDecimal(reader["ClosedAmount"]);
                    item.ReturnedAmount = Convert.ToDecimal(reader["ReturnedAmount"]);
                    item.Total = item.Subtotal + item.vatAmount - item.Discount;
                    item.RemAmount = item.Total - item.ClosedAmount;
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["SalesPerson"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
