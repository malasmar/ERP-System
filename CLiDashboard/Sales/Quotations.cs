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
    public class Quotations
    {
        public int Count { get; set; }
        public int Invoiced { get; set; }
        public Quotations GetList(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            Quotations item = new Quotations();
            string selQuery = "select top 100 percent * from dbo.fnDashSales_Quotations(@FirstDate,@LastDate)  ";
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
                    item.Count = Convert.ToInt32(reader["Quotations"]);
                    item.Invoiced = Convert.ToInt32(reader["Invoiced"]);
                }
                reader.Close();
            }
            return item;
        }
        public Quotations Proforma(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            Quotations item = new Quotations();
            string selQuery = "select top 100 percent * from dbo.fnDashSales_Proforma(@FirstDate,@LastDate)  ";
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
                    item.Count = Convert.ToInt32(reader["Quotations"]);
                    item.Invoiced = Convert.ToInt32(reader["Invoiced"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
