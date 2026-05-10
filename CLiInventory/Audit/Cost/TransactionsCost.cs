using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Audit.Cost
{
    public class TransactionsCost
    {
        public Guid? OperationKey { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int SourceWarehouse { get; set; }
        public int TargetWarehouse { get; set; }
        public int DocumentKind { get; set; }
        public int InvoiceKind { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int MonthlyNo { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public int Source { get; set; }
        public decimal ItemsCost { get; set; }
        public List<TransactionsCost> GetList(string DB,int DocKind,int Year)
        {
            DateTime First = new DateTime(Year, 1, 1);
            DateTime Last = new DateTime(Year, 12, 31);

            List<TransactionsCost> items = new List<TransactionsCost>();
            string selQuery = "select top 100 percent * from dbo.fninvAudit_TransactionsCost(@DocKind,@First,@Last) order by [InvoiceDate] desc,[InvoiceNo] desc";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TransactionsCost item = new TransactionsCost();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Branch = Convert.ToInt32(reader["Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["Prefix"]);
                    item.SourceWarehouse = Convert.ToInt32(reader["SourceWarehouse"]);
                    item.TargetWarehouse = Convert.ToInt32(reader["TargetWarehouse"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.InvoiceKind = Convert.ToInt32(reader["InvoiceKind"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Currency = Convert.ToString(reader["Currency"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.BonusAmount = Convert.ToDecimal(reader["BonusAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.BonusQuantity = Convert.ToDecimal(reader["BonusQuantity"]);
                    item.Source = Convert.ToInt32(reader["Source"]);
                    item.ItemsCost = Convert.ToDecimal(reader["ItemsCost"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
