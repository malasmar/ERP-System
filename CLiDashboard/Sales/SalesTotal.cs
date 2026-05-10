using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Sales
{
    public class SalesTotal
    {
        public decimal Sales { get; set; }
        public decimal Return { get; set; }
        public decimal SalesQty { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal Discount { get; set; }
        public SalesTotal GetList(string DB, DateTime FirstDate,DateTime LastDate)
        {

            SalesTotal item = new SalesTotal();
            string selQuery = "select top 100 percent * from dbo.fnDashSales_SalesTotal(@FirstDate,@LastDate)  ";
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
                    item.Sales = Convert.ToDecimal(reader["Sales"]);
                    item.Return = Convert.ToDecimal(reader["Return"]);
                    item.SalesQty = Convert.ToDecimal(reader["SalesQty"]);
                    item.ReturnQty = Convert.ToDecimal(reader["ReturnQty"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
