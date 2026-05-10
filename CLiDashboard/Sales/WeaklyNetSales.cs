using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Sales
{
    public class WeaklyNetSales
    {
        public decimal Amount { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int Week { get; set; }
        public List<WeaklyNetSales> GetList(string DB,int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<WeaklyNetSales> items = new List<WeaklyNetSales>();
            string selQuery = "select top 100 percent * from dbo.fnDashSales_WeeklyNetSales(@FirstDate,@LastDate) order by [Week] ";
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
                    WeaklyNetSales item = new WeaklyNetSales();
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Week = Convert.ToInt32(reader["Week"]);
                   item.VoucherDate = iCore.FirstDateOfWeekISO8601(Year,item.Week) ;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
