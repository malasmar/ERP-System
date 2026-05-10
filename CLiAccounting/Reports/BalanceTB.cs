using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Reports
{
    public class BalanceTB
    {
        public decimal oDebit { get; set; }
        public decimal oCredit { get; set; }
        public decimal TDebit { get; set; }
        public decimal TCredit { get; set; }
     
        public BalanceTB GetItem(string DB,DateTime First,DateTime Last)
        {
            
            BalanceTB item = new BalanceTB();
            string selQuery = "select top 100 percent * from dbo.fnaccReports_BalanceTB(@First,@Last) ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.oDebit = Convert.ToDecimal(reader["oDebit"]);
                    item.oCredit = Convert.ToDecimal(reader["oCredit"]);
                    item.TDebit = Convert.ToDecimal(reader["TDebit"]);
                    item.TCredit = Convert.ToDecimal(reader["TCredit"]);
                    
                }
                reader.Close();
            }
            return item;
        }
    }
}
