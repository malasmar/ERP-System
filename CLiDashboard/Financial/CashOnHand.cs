using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Financial
{
    public class CashOnHand
    {
        public decimal Bank { get; set; }
        public decimal CashBox { get; set; }
        public decimal CashPerson { get; set; }

        public CashOnHand GetList(string DB)
        {

            CashOnHand item = new CashOnHand();
            string selQuery = "select top 100 percent * from dbo.fnDashFinancial_CashOnHand()  ";
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
                    item.Bank = Convert.ToDecimal(reader["Bank"]);
                    item.CashBox = Convert.ToDecimal(reader["CashBox"]);
                    item.CashPerson = Convert.ToDecimal(reader["CashPerson"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
