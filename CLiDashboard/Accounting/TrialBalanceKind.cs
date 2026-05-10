using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Accounting
{
    public class TrialBalanceKind
    {
        public decimal Assets { get; set; }
        public decimal Liability { get; set; }
        public decimal Equty { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }

        public TrialBalanceKind GetList(string DB, DateTime FirstDate, DateTime LastDate)
        {

            TrialBalanceKind item = new TrialBalanceKind();
            string selQuery = "select top 100 percent * from dbo.fnDashAccounting_TrialBalanceKind(@FirstDate,@LastDate) ";
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
                    item.Assets = Convert.ToDecimal(reader["Assets"]);
                    item.Liability = Convert.ToDecimal(reader["Liability"]);
                    item.Equty = Convert.ToDecimal(reader["Equty"]);
                    item.Revenue = Convert.ToDecimal(reader["Revenue"]);
                    item.Expenses = Convert.ToDecimal(reader["Expenses"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
