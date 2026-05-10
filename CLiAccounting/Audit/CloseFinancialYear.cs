using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiAccounting.Audit
{
    public class CloseFinancialYear
    {
        public Guid? Key { get; set; }
        public int Kind { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; } 
        public string Name2 { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public List<CloseFinancialYear> GetList(string DB,int Year)
        {
            DateTime FirstDate = new DateTime(Year, 1, 1);
            DateTime LastDate = new DateTime(Year, 12, 31);
            List<CloseFinancialYear> items = new List<CloseFinancialYear>();
            string selQuery = "select top 100 percent * from dbo.fnaccAudit_CloseFinancialYear(@FirstDate,@LastDate) order by [Kind],[Code]";
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
                    CloseFinancialYear item = new CloseFinancialYear();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
