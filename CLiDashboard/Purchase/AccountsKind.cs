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
    public class AccountsKind
    {
        public decimal Credit { get; set; }
        public decimal Cash { get; set; }
        public decimal Bank { get; set; }
        public decimal Discount { get; set; }
        public AccountsKind GetList(string DB, DateTime FirstDate, DateTime LastDate)
        {
            
            AccountsKind item = new AccountsKind();
            string selQuery = "select top 100 percent * from dbo.fnDashPurchase_CloseAccounts(@FirstDate,@LastDate)  ";
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
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.Cash = Convert.ToDecimal(reader["Cash"]);
                    item.Bank = Convert.ToDecimal(reader["Bank"]);
                    item.Discount = Convert.ToDecimal(reader["Discount"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
