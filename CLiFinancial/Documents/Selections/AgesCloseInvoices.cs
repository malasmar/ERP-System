using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Documents.Selections
{
    public class AgesCloseInvoices
    {
        public Guid? OperationKey { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int MonthlyNo { get; set; }
        public DateTime? DueDate { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public Boolean Retrned { get; set; }
        public decimal ClosedAmount { get; set; }
        public decimal ReturnedAmount { get; set; }
        public decimal Total { get; set; }
        public decimal RemAmount { get; set; }
        public decimal TotalRem { get; set; }
        public decimal xAmount { get; set; }
        public int RemDays { get; set; }
        public List<AgesCloseInvoices> GetList(string DB,int DocumentKind,Guid? Key)
        {
            List<AgesCloseInvoices> items = new List<AgesCloseInvoices>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fninvDocuments_AgesInvoices(@DocumentKind,@Key) order by [DueDate]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key",SqlDbType.UniqueIdentifier).Value=Key;
                com.Parameters.Add("@DocumentKind", SqlDbType.Int).Value = DocumentKind;

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AgesCloseInvoices item = new AgesCloseInvoices();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["DueDate"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Currency = Convert.ToString(reader["Currency"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["BonusAmount"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["BonusQuantity"]);
                    item.Retrned = Convert.ToBoolean(reader["Retrned"]);
                    item.ClosedAmount = Convert.ToDecimal(reader["ClosedAmount"]);
                    item.ReturnedAmount = Convert.ToDecimal(reader["ReturnedAmount"]);
                    item.Total = item.Subtotal + item.vatAmount - item.Discount;
                    item.RemAmount = item.Total - item.ClosedAmount;
                    item.TotalRem = item.Total - item.ClosedAmount - item.ReturnedAmount;
                    if (item.DueDate.HasValue)
                        item.RemDays = Convert.ToInt32((item.DueDate.Value - DateTime.Now).TotalDays + 1);
                    else
                        item.RemDays = Convert.ToInt32((item.InvoiceDate.Value - DateTime.Now).TotalDays + 1);


                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
