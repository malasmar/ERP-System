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
    public class MonthlySales
    {
        public Guid? Key { get; set; }
        public int Kind { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Unit { get; set; }
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
        public decimal Net { get; set; }
        public List<MonthlySales> MonthlyNetSales(string DB,int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<MonthlySales> items = new List<MonthlySales>();
            string selQuery = "select top 100 percent * from dbo.ReportSales_MonthlyItemsNetSales(@FirstDate,@LastDate) order by [Kind],[Code] ";
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
                    MonthlySales item = new MonthlySales();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
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
                    item.Net = item.Jan + item.Feb + item.Mar + item.Apr + item.May + item.Jun + item.Jul + item.Aug + item.Sep + item.Oct + item.Nov + item.Dec;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public List<MonthlySales> MonthlyQuantity(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<MonthlySales> items = new List<MonthlySales>();
            string selQuery = "select top 100 percent * from dbo.ReportSales_MonthlyItemsQuantity(@FirstDate,@LastDate) order by [Kind],[Code] ";
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
                    MonthlySales item = new MonthlySales();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
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
                    item.Net = item.Jan + item.Feb + item.Mar + item.Apr + item.May + item.Jun + item.Jul + item.Aug + item.Sep + item.Oct + item.Nov + item.Dec;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
