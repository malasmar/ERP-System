using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Purchase
{
    public class SuppliersBalance
    {
        public int DebitCount { get; set; }
        public int CreditCount { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public SuppliersBalance GetList(string DB)
        {
            SuppliersBalance item = new SuppliersBalance();
            string selQuery = "select top 100 percent * from dbo.fnDashPurchase_SuppliersBalance()  ";
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
                    item.DebitCount = Convert.ToInt32(reader["DebitCount"]);
                    item.CreditCount = Convert.ToInt32(reader["CreditCount"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
