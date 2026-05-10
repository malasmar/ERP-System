using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiDashboard.Sales;

namespace CLiDashboard.Purchase
{
    public class MonthlyPurchasing
    {
        public int Month { get; set; }
 
        public decimal Purchase { get; set; }
        public decimal Return { get; set; }
        public List<MonthlyPurchasing> GetList(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<MonthlyPurchasing> items = new List<MonthlyPurchasing>();
            string selQuery = "select top 100 percent * from dbo.fnDashPurchase_MonthlyPurchase(@FirstDate,@LastDate) order by [Month]";
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
                    MonthlyPurchasing item = new MonthlyPurchasing();
                    item.Month = Convert.ToInt32(reader["Month"]);
         
                    item.Purchase = Convert.ToDecimal(reader["Purchase"]);
                    item.Return = Convert.ToDecimal(reader["Return"]);
                    items.Add(item);
                }
                reader.Close();
            }
            for(int i = 1; i <= 12; ++i)
            {
                if (items.Count(x => x.Month == i) == 0)
                {
                    items.Add(new MonthlyPurchasing() { Month = i, Purchase = 0, Return = 0 });
                }
            }
            return items.OrderBy(x=>x.Month).ToList();
        }
    }
}
