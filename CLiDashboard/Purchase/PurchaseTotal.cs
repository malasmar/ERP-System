using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Purchase
{
    public class PurchaseTotal
    {
        public decimal Purchase { get; set; }
        public decimal Return { get; set; }
        public decimal PurchaseQty { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal Discount { get; set; }
        public PurchaseTotal GetList(string DB, DateTime FirstDate,DateTime LastDate)
        {

            PurchaseTotal item = new PurchaseTotal();
            string selQuery = "select top 100 percent * from dbo.fnDashPurchase_PurchaseTotal(@FirstDate,@LastDate)  ";
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
                    item.Purchase = Convert.ToDecimal(reader["Purchase"]);
                    item.Return = Convert.ToDecimal(reader["Return"]);
                    item.PurchaseQty = Convert.ToDecimal(reader["PurchaseQty"]);
                    item.ReturnQty = Convert.ToDecimal(reader["ReturnQty"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
