using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiDashboard
{
    public class core
    {
        public static decimal TotalVAT(string DB)
        {
            decimal result = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "SELECT TOP 100 PERCENT ISNULL(SUM(vat_Debit-vat_Credit),0.00) FROM vatDocument_Transaction";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
    }
}
