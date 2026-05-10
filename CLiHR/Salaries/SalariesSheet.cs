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
    public class SalariesSheet
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Benefit { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deduction { get; set; }
        public decimal Insurance { get; set; }
        public decimal Advance { get; set; }
        public Boolean Status { get; set; }
        public decimal BenefitTotal { get; set; }
        public decimal DeductionTotal { get; set; }
        public decimal NetSalary { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Branch { get; set; }
        public Guid? Structure { get; set; }
        public Guid? Department { get; set; }


        public List<SalariesSheet> GetList(string DB, int Year, int Month, string PaymentKind)
        {
            List<SalariesSheet> items = new List<SalariesSheet>();

            string selQuery = "select top 100 percent * from dbo.fnPayroll_SalariesSheet(@Year,@Month,@PaymentKind) order by [Code] ";
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
                    SalariesSheet item = new SalariesSheet();
                    item.Key = iCore.IsDbNullRtNull(reader["sal_Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.BasicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                    item.Benefit = Convert.ToDecimal(reader["Benefit"]);
                    item.Allowances = Convert.ToDecimal(reader["Allowances"]);
                    item.Deduction = Convert.ToDecimal(reader["Deduction"]);
                    item.Insurance = Convert.ToDecimal(reader["Insurance"]);
                    item.Advance = Convert.ToDecimal(reader["Advance"]);
                    item.Status = Convert.ToBoolean(reader["Status"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Branch = iCore.IsDbNullRtNull(reader["Branch"]);
                    item.Structure = iCore.IsDbNullRtNull(reader["Structure"]);
                    item.Department = iCore.IsDbNullRtNull(reader["Department"]);
                    item.BenefitTotal = item.BasicSalary + item.Benefit + item.Allowances;
                    item.DeductionTotal = item.Deduction + item.Insurance + item.Advance;
                    item.NetSalary = item.BenefitTotal - item.DeductionTotal;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
