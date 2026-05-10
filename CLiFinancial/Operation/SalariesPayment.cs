using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.Operation
{
    public class SalariesPayment
    {
        public Guid? Key { get; set; }
        public Guid? Employee { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Salary { get; set; }
        public decimal Closed { get; set; }
        public decimal Amount { get; set; }

        public List<SalariesPayment> GetList(string DB, int Year, int Month, string PaymentKind)
        {
            List<SalariesPayment> items = new List<SalariesPayment>();
            string selQuery = "select top 100 percent * from dbo.fnPayroll_SalariesPayment(@Year,@Month,@PaymentKind) where ([Salary]-[Closed])>0 order by [Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                com.Parameters.Add("@PaymentKind", SqlDbType.NVarChar, 255).Value = PaymentKind ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SalariesPayment item = new SalariesPayment();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Employee = iCore.IsDbNullRtNull(reader["Employee"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Salary = Convert.ToDecimal(reader["Salary"]);
                    item.Closed = Convert.ToDecimal(reader["Closed"]);
                    item.Amount = item.Salary - item.Closed;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
