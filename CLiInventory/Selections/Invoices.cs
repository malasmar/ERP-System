using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Selections
{
    public class Invoices
    {
        public Guid? Key { get; set; }
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
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }  
        public List<Invoices> GetList(string DB,int DocKind)
        {
            List<Invoices> items = new List<Invoices>();
            string selQuery = "select top 100 percent * from dbo.fninvSelections_Invoices(@DocKind) order by [InvoiceDate] desc,[InvoiceNo] desc";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Invoices item = new Invoices();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
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
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
