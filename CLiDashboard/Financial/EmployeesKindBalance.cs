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
    public class EmployeesKindBalance
    {
        public decimal Salaries { get; set; }
        public decimal Advance { get; set; }
        public decimal Loan { get; set; }

        public EmployeesKindBalance GetList(string DB)
        {
            EmployeesKindBalance item = new EmployeesKindBalance();
            string selQuery = "select top 100 percent * from dbo.fnDashFinancial_EmployeesAccountKind()  ";
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
                    item.Salaries = Convert.ToDecimal(reader["Salaries"]);
                    item.Advance = Convert.ToDecimal(reader["Advance"]);
                    item.Loan = Convert.ToDecimal(reader["Loan"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
