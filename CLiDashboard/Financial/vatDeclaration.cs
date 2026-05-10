using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using System.Security.AccessControl;

namespace CLiDashboard.Financial
{
    public class vatDeclaration
    {
        public decimal Sales { get; set; }
        public decimal Purchase { get; set; }

        public vatDeclaration GetList(string DB, DateTime FirstDate, DateTime LastDate)
        {
            vatDeclaration item = new vatDeclaration();
            string selQuery = "select top 100 percent * from dbo.fnDashFinancial_vatDeclaration(@FirstDate,@LastDate)  ";
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
                    item.Sales = Convert.ToDecimal(reader["Sales"]);
                    item.Purchase = Convert.ToDecimal(reader["Purchase"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
