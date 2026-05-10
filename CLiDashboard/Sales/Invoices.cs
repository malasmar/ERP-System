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
    public class Invoices
    {
        public int SalesInvoices { get; set; }
        public int ReturnInvoices { get; set; }
        public int DicountInvoices { get; set; }
        public Invoices GetList(string DB, int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            Invoices item = new Invoices();
            string selQuery = "select top 100 percent * from dbo.fnDashSales_Invoices(@FirstDate,@LastDate)  ";
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
                    item.SalesInvoices = Convert.ToInt32(reader["SalesInvoices"]);
                    item.ReturnInvoices = Convert.ToInt32(reader["ReturnInvoices"]);
                    item.DicountInvoices = Convert.ToInt32(reader["DicountInvoices"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
