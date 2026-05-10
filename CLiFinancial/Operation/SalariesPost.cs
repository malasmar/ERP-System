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
    public class SalariesPost
    {
        public int Kind { get; set; }
        public int Type { get; set; }
        public string Code { get; set; }
        public Guid? Account { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public Guid? CostCenter { get; set; }
        public Guid? Project { get; set; }
        public List<SalariesPost> GetList(string DB,int Year,int Month)
        {
            List<SalariesPost> items = new List<SalariesPost>();
            string selQuery = "select top 100 percent * from dbo.fnFinOperation_SalariesPost(@Year,@Month) where (Debit+Credit)>0 order by [Kind],[Type],[code],[Debit] asc";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Year",SqlDbType.Int).Value=Year;
                com.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SalariesPost item = new SalariesPost();
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Type = Convert.ToInt32(reader["Type"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Account = iCore.IsDbNullRtNull(reader["Account"]);
                    item.Debit = Convert.ToDecimal(reader["Debit"]);
                    item.Credit = Convert.ToDecimal(reader["Credit"]);
                    item.CostCenter = iCore.IsDbNullRtNull(reader["CostCenter"]);
                    item.Project = iCore.IsDbNullRtNull(reader["Project"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
