using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiSales.Reports
{
    public class ItemsSales
    {
        public Guid? Key { get; set; }
        public int Kind { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public decimal RQuantity { get; set; }
        public decimal RAmount { get; set; }
        public decimal RDiscount { get; set; }
        public decimal RvatAmount { get; set; }
        public decimal RTotal { get; set; }
        public decimal Net { get; set; }
        public List<ItemsSales> GetList(string DB,DateTime FirstDate,DateTime LastDate)
        {
            List<ItemsSales> items = new List<ItemsSales>();
            string selQuery = "select top 100 percent * from dbo.ReportSales_ItemsSales(@FirstDate,@LastDate) order by [Kind],[Code] ";
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
                    ItemsSales item = new ItemsSales();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.RQuantity = Convert.ToDecimal(reader["RQuantity"]);
                    item.RAmount = Convert.ToDecimal(reader["RAmount"]);
                    item.RDiscount = Convert.ToDecimal(reader["RDiscount"]);
                    item.RvatAmount = Convert.ToDecimal(reader["RvatAmount"]);
                    item.RTotal = Convert.ToDecimal(reader["RTotal"]);
                    item.Net = item.Amount - item.Discount - (item.RAmount + item.RDiscount);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<ItemsSales> ReturnSales(string DB, DateTime FirstDate, DateTime LastDate)
        {
            List<ItemsSales> items = new List<ItemsSales>();
            string selQuery = "select top 100 percent * from dbo.ReportSales_ItemsSales(@FirstDate,@LastDate) where [RQuantity] >0 order by [Kind],[Code] ";
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
                    ItemsSales item = new ItemsSales();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.RQuantity = Convert.ToDecimal(reader["RQuantity"]);
                    item.RAmount = Convert.ToDecimal(reader["RAmount"]);
                    item.RDiscount = Convert.ToDecimal(reader["RDiscount"]);
                    item.RvatAmount = Convert.ToDecimal(reader["RvatAmount"]);
                    item.RTotal = Convert.ToDecimal(reader["RTotal"]);
                    item.Net = item.Amount - item.RAmount;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
