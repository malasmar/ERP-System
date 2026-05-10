using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService.Salaries
{
    public class SalariesHistory
    {
        public Guid? Key { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Benefit { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deduction { get; set; }
        public decimal Insurance { get; set; }
        public decimal Advance { get; set; }
        public decimal BenTotal { get; set; }
        public decimal DedTotal { get; set; }
        public decimal NetAmount { get; set; }
        public List<SalariesHistory> GetList(string DB,Guid? Key)
        {
            List<SalariesHistory> items = new List<SalariesHistory>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnAppSelfService_SalariesHistory(@Key) order by [Year] desc,[Month] desc";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SalariesHistory item = new SalariesHistory();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Year = Convert.ToInt32(reader["Year"]);
                    item.Month = Convert.ToInt32(reader["Month"]);
                    item.BasicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                    item.Benefit = Convert.ToDecimal(reader["Benefit"]);
                    item.Allowances = Convert.ToDecimal(reader["Allowances"]);
                    item.Deduction = Convert.ToDecimal(reader["Deduction"]);
                    item.Insurance = Convert.ToDecimal(reader["Insurance"]);
                    item.Advance = Convert.ToDecimal(reader["Advance"]);
                    item.BenTotal = item.BasicSalary + item.Benefit + item.Allowances;
                    item.DedTotal = item.Deduction + item.Advance + item.Insurance;
                    item.NetAmount = item.BenTotal - item.DedTotal;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
