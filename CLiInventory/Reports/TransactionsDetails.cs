using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Reports
{
    public class TransactionsDetails
    {
        public int Row { get; set; }
        public Guid? OperationKey { get; set; }
        public DateTime? Date { get; set; }
        public int No { get; set; }
        public Guid? SalesPerson { get; set; }
        public int Warehouse { get; set; }
        public int CloseKind { get; set; }
        public Guid? Account { get; set; }
        public Guid? CurrentAccount { get; set; }
        public string InvoiceDescription { get; set; }
        public int Index { get; set; }
        public int ItemKind { get; set; }
        public string ItemCode { get; set; }
        public string ItemName1 { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal vatRate { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public string LineDescription { get; set; }
        public decimal SubTotal { get; set; }
        public List<TransactionsDetails> GetList(string DB,int DocKind,int Year)
        {
            DateTime FirstDate = new DateTime(Year,1,1);
                DateTime LastDate = new DateTime(Year, 12, 31);
            List<TransactionsDetails> items = new List<TransactionsDetails>();
            string selQuery = "select top 100 percent * from dbo.ReportInv_TransactionsDetails(@DocKind,@FirstDate,@LastDate) order by [Date],[No],[Index]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@DocKind", SqlDbType.Int).Value = DocKind;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = LastDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TransactionsDetails item = new TransactionsDetails();
                     item.Row = Convert.ToInt32(reader["Row"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.SalesPerson = iCore.IsDbNullRtNull(reader["SalesPerson"]);
                    item.Warehouse = Convert.ToInt32(reader["Warehouse"]);
                    item.CloseKind = Convert.ToInt32(reader["CloseKind"]);
                    item.Account = iCore.IsDbNullRtNull(reader["Account"]);
                    item.CurrentAccount = iCore.IsDbNullRtNull(reader["CurrentAccount"]);
                    item.InvoiceDescription = Convert.ToString(reader["InvoiceDescription"]);
                    item.Index = Convert.ToInt32(reader["Index"]);
                    item.ItemKind = Convert.ToInt32(reader["ItemKind"]);
                    item.ItemCode = Convert.ToString(reader["ItemCode"]);
                    item.ItemName1 = Convert.ToString(reader["ItemName1"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.LineDescription = Convert.ToString(reader["LineDescription"]);
                    item.SubTotal = item.Amount - item.Discount;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
