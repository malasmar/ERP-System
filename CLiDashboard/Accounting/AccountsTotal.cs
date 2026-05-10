using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiDashboard.Sales;

namespace CLiDashboard.Accounting
{
    public class AccountsTotal
    {
        public int Parent { get; set; }
        public int Transaction { get; set; }
        public int Total { get; set; }

        public AccountsTotal GetList(string DB)
        {

            AccountsTotal item = new AccountsTotal();
            string selQuery = "select top 100 percent * from dbo.fnDashAccounting_AccountsTotal()  ";
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
                    item.Parent = Convert.ToInt32(reader["Parent"]);
                    item.Transaction = Convert.ToInt32(reader["Transaction"]);
                    item.Total = Convert.ToInt32(reader["Total"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
