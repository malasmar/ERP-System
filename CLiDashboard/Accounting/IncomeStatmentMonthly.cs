using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiDashboard.Sales;
using CLiDashboard.Purchase;

namespace CLiDashboard.Accounting
{
    public class IncomeStatmentMonthly
    {
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
    
        public List<IncomeStatmentMonthly> GetList(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<IncomeStatmentMonthly> items = new List<IncomeStatmentMonthly>();
            string selQuery = "select top 100 percent * from dbo.fnDashAccounting_IncomeStatmentMonthly(@FirstDate,@LastDate) order by [Month]";
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
                    IncomeStatmentMonthly item = new IncomeStatmentMonthly();
                    item.Month = Convert.ToInt32(reader["Month"]);
             
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Revenue = Convert.ToDecimal(reader["Revenue"]);
                    item.Expenses = Convert.ToDecimal(reader["Expenses"]);
                    items.Add(item);
                }
                reader.Close();
            }
            for (int i = 1; i <= 12; ++i)
            {
                if (items.Count(x => x.Month == i) == 0)
                {
                    items.Add(new IncomeStatmentMonthly() { Month = i, Amount = 0, Expenses = 0,Revenue=0 });
                }
            }
            return items.OrderBy(x => x.Month).ToList();
         
        }
    }
}
