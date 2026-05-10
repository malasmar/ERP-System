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
    public class ItemKindTotal
    {
        public decimal Stock { get; set; }
        public decimal Service { get; set; }
        public decimal Fixture { get; set; }

        public ItemKindTotal GetList(string DB, DateTime FirstDate, DateTime LastDate)
        {
          
            ItemKindTotal item = new ItemKindTotal();
            string selQuery = "select top 100 percent * from dbo.fnDashPurchase_ItemKindTotal(@FirstDate,@LastDate)  ";
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
                    item.Stock = Convert.ToDecimal(reader["Stock"]);
                    item.Service = Convert.ToDecimal(reader["Service"]);
                    item.Fixture = Convert.ToDecimal(reader["Fixture"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
