using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiHR.Salaries
{
    public class Deduction
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Penalty { get; set; }
        public decimal Insurance { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal Deduction1 { get; set; }
        public decimal Deduction2 { get; set; }
        public decimal Deduction3 { get; set; }
        public decimal Advance { get; set; }
        public decimal Loan { get; set; }
        public Boolean Status { get; set; }
        public List<Deduction> GetList(string DB, int Year, int Month, string PaymentKind)
        {
            List<Deduction> items = new List<Deduction>();

            string selQuery = "select top 100 percent * from dbo.fnPayroll_UpdateDeduction(@Year,@Month,@PaymentKind) order by [Code] ";
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
                    Deduction item = new Deduction();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Penalty = Convert.ToDecimal(reader["Penalty"]);
                    item.Insurance = Convert.ToDecimal(reader["Insurance"]);
                    item.DeductionAmount = Convert.ToDecimal(reader["Deduction"]);
                    item.Deduction1 = Convert.ToDecimal(reader["Deduction1"]);
                    item.Deduction2 = Convert.ToDecimal(reader["Deduction2"]);
                    item.Deduction3 = Convert.ToDecimal(reader["Deduction3"]);
                    item.Advance = Convert.ToDecimal(reader["Advance"]);
                    item.Loan = Convert.ToDecimal(reader["Loan"]);
                    item.Status = Convert.ToBoolean(reader["Status"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
