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
    public class SalesPersonCount
    {
        public int Total { get; set; }
        public int SalesPerson { get; set; }
        public int OutSalesPerson { get; set; }
        public SalesPersonCount GetList(string DB)
        {
            SalesPersonCount item = new SalesPersonCount();
            string selQuery = "select top 100 percent * from dbo.fnDashSales_SalesPersonCount()  ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Total = Convert.ToInt32(reader["Total"]);
                    item.SalesPerson = Convert.ToInt32(reader["SalesPerson"]);
                    item.OutSalesPerson = Convert.ToInt32(reader["OutSalesPerson"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
