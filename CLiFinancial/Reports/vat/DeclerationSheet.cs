using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial.Reports.vat
{
    public class DeclerationSheet
    {
        public Guid? OperationKey { get; set; }
        public int DocumentKind { get; set; }
        public int VoucherNo { get; set; }
        public int MonthlyNo { get; set; }
        public DateTime? Date { get; set; }
        public Guid? vatKey { get; set; }
        public int Kind { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }
        public string CurrentName { get; set; }
        public string vatRegNo { get; set; }
        public string strMonthlyNo { get; set; }
        public List<DeclerationSheet> GetList(string DB, DateTime FirstDate, DateTime LastDate)
        {
            List<DeclerationSheet> items = new List<DeclerationSheet>();
            string selQuery = "select top 100 percent * from dbo.ReportFin_vatDeclerationSheet(@FirstDate,@LastDate) order by [Date],[VoucherNo],[DocumentKind]";
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
                    DeclerationSheet item = new DeclerationSheet();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Rate = Convert.ToDecimal(reader["Rate"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.CurrentName = Convert.ToString(reader["CurrentName"]);
                    item.vatRegNo = Convert.ToString(reader["vatRegNo"]);
                    item.strMonthlyNo = item.Date.Value.ToString("MM") + "-" + item.MonthlyNo.ToString("0000");
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
       
    }
}
