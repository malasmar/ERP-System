using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
 

namespace CLiDashboard.Financial
{
    public class WeeklyAccountsBalance
    {
        public int VoucherYear { get; set; }
        public int VoucherWeek { get; set; }
        public decimal Bank { get; set; }
        public decimal Cash { get; set; }
        public decimal Supplier { get; set; }
        public decimal Client { get; set; }
        public DateTime VoucherDate { get; set; }
        public List<WeeklyAccountsBalance> GetList(string DB, DateTime FirstDate, DateTime LastDate)
        {
       
            List<WeeklyAccountsBalance> items = new List<WeeklyAccountsBalance>();
            string selQuery = "select top 100 percent * from dbo.fnDashFinancial_WeeklyAccountsBalance(@FirstDate,@LastDate) order by [VoucherYear],[VoucherWeek] ";
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
                    WeeklyAccountsBalance item = new WeeklyAccountsBalance();
                    item.VoucherYear = Convert.ToInt32(reader["VoucherYear"]);
                    item.VoucherWeek = Convert.ToInt32(reader["VoucherWeek"]);
                    item.Bank = Convert.ToDecimal(reader["Bank"]);
                    item.Cash = Convert.ToDecimal(reader["Cash"]);
                    item.Supplier =-1* Convert.ToDecimal(reader["Supplier"]);
                    item.Client = Convert.ToDecimal(reader["Client"]);
                    item.VoucherDate = iCore.FirstDateOfWeekISO8601(item.VoucherYear, item.VoucherWeek);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
