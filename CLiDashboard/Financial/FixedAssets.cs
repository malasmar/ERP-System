using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiDashboard.Financial
{
    public class Fixture
    {
        public decimal FixedAssets { get; set; }
        public decimal Depreciation { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amortization { get; set; }
        public Fixture GetItem(string DB)
        {
            Fixture item = new Fixture();
            string selQuery = "select top 100 percent * from dbo.fnDashFinancial_FixedAssets() ";
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
                    item.FixedAssets = Convert.ToDecimal(reader["FixedAssets"]);
                    item.Depreciation = Convert.ToDecimal(reader["Depreciation"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Amortization = Convert.ToDecimal(reader["Amortization"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
