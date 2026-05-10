using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiDashboard.Sales;

namespace CLiDashboard.Accounting
{
    public class IncomeStatmentWeekly
    {
        public int Week { get; set; }
        public decimal Amount { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
        public DateTime VoucherDate { get; set; }
        public List<IncomeStatmentWeekly> GetList(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<IncomeStatmentWeekly> items = new List<IncomeStatmentWeekly>();
            string selQuery = "select top 100 percent * from dbo.fnDashAccounting_IncomeStatmentWeekly(@FirstDate,@LastDate) order by [Week] ";
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
                    IncomeStatmentWeekly item = new IncomeStatmentWeekly();
                    item.Week = Convert.ToInt32(reader["Week"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Revenue = Convert.ToDecimal(reader["Revenue"]);
                    item.Expenses = Convert.ToDecimal(reader["Expenses"]);
                    item.VoucherDate = iCore.FirstDateOfWeekISO8601(Year, item.Week);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
