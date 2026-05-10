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
    public class Benefit
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Rewards { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Commission { get; set; }
        public decimal BenefitAmount { get; set; }
        public decimal Benefit1 { get; set; }
        public decimal Benefit2 { get; set; }
        public decimal Benefit3 { get; set; }
        public decimal EndService { get; set; }
        public decimal Vacation { get; set; }
        public decimal Ticket { get; set; }
        public Boolean Status { get; set; }
        public List<Benefit> GetList(string DB, int Year, int Month, string PaymentKind)
        {
            List<Benefit> items = new List<Benefit>();

            string selQuery = "select top 100 percent * from dbo.fnPayroll_UpdateBenefit(@Year,@Month,@PaymentKind) order by [Code] ";
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
                    Benefit item = new Benefit();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.BasicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                    item.Allowance = Convert.ToDecimal(reader["Allowance"]);
                    item.Rewards = Convert.ToDecimal(reader["Rewards"]);
                    item.Bonuses = Convert.ToDecimal(reader["Bonuses"]);
                    item.Commission = Convert.ToDecimal(reader["Commission"]);
                    item.BenefitAmount = Convert.ToDecimal(reader["Benefit"]);
                    item.Benefit1 = Convert.ToDecimal(reader["Benefit1"]);
                    item.Benefit2 = Convert.ToDecimal(reader["Benefit2"]);
                    item.Benefit3 = Convert.ToDecimal(reader["Benefit3"]);
                    item.EndService = Convert.ToDecimal(reader["EndService"]);
                    item.Vacation = Convert.ToDecimal(reader["Vacation"]);
                    item.Ticket = Convert.ToDecimal(reader["Ticket"]);
                    item.Status = Convert.ToBoolean(reader["Status"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

    }
}
