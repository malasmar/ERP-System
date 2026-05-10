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
    public class MonthlyNetSales
    {
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
        public MonthlyNetSales GetList(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            MonthlyNetSales item = new MonthlyNetSales();
            string selQuery = "select top 100 percent * from dbo.fnDashSales_MonthlyNetSales(@FirstDate,@LastDate)   ";
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
                    item.Jan = Convert.ToDecimal(reader["Jan"]);
                    item.Feb = Convert.ToDecimal(reader["Feb"]);
                    item.Mar = Convert.ToDecimal(reader["Mar"]);
                    item.Apr = Convert.ToDecimal(reader["Apr"]);
                    item.May = Convert.ToDecimal(reader["May"]);
                    item.Jun = Convert.ToDecimal(reader["Jun"]);
                    item.Jul = Convert.ToDecimal(reader["Jul"]);
                    item.Aug = Convert.ToDecimal(reader["Aug"]);
                    item.Sep = Convert.ToDecimal(reader["Sep"]);
                    item.Oct = Convert.ToDecimal(reader["Oct"]);
                    item.Nov = Convert.ToDecimal(reader["Nov"]);
                    item.Dec = Convert.ToDecimal(reader["Dec"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
