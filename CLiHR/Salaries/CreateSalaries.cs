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
    public class CreateSalaries
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Gender { get; set; }
        public int NatKind { get; set; }
        public int PaymentType { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public int Status { get; set; }
        public Guid? Structure { get; set; }
        public Guid? Department { get; set; }
        public Guid? City { get; set; }
        public Guid? Branch { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public decimal Total { get; set; }
        public List<CreateSalaries> GetList(string DB,int Year,int Month,string PaymentKind)
        {
            List<CreateSalaries> items = new List<CreateSalaries>();

            string selQuery = "select top 100 percent * from dbo.fnPayroll_CreateSalaries(@Year,@Month,@PaymentKind) order by [Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                com.Parameters.Add("@PaymentKind", SqlDbType.NVarChar,255).Value = PaymentKind??"";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CreateSalaries item = new CreateSalaries();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Gender = Convert.ToBoolean(reader["Gender"]);
                    item.NatKind = Convert.ToInt32(reader["NatKind"]);
                    item.PaymentType = Convert.ToInt32(reader["PaymentType"]);
                    item.BasicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                    item.Allowances = Convert.ToDecimal(reader["Allowances"]);
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.Structure = iCore.IsDbNullRtNull(reader["Structure"]);
                    item.Department = iCore.IsDbNullRtNull(reader["Department"]);
                    item.City = iCore.IsDbNullRtNull(reader["City"]);
                    item.Branch = iCore.IsDbNullRtNull(reader["Branch"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    item.Total = item.BasicSalary + item.Allowances;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
