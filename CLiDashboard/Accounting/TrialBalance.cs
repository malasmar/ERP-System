using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiDashboard.Accounting
{
    public class TrialBalance
    {
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public TrialBalance GetList(string DB, DateTime FirstDate, DateTime LastDate)
        {
         
            TrialBalance item = new TrialBalance();
            string selQuery = "select top 100 percent * from dbo.fnDashAccounting_TrialBalance(@FirstDate,@LastDate) ";
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
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                }
                reader.Close();
            }
            return item;
        }
        public TrialBalance GetOpening(string DB, DateTime FirstDate)
        {
             
      
            TrialBalance item = new TrialBalance();
            string selQuery = "select top 100 percent * from dbo.fnDashAccounting_TrialBalanceOpening(@FirstDate) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = FirstDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
