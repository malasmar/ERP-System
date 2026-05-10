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
    public class UnpostedVouchers
    {
        public Guid? Key { get; set; }
        public int DocumentKind { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int VoucherNo { get; set; }
        public int MonthlyNo { get; set; }
        public string Description { get; set; }
        public decimal Subtotal { get; set; }
        public decimal vatAmount { get; set; }
        public decimal Total { get; set; }
        public List<UnpostedVouchers> GetList(string DB,int Year)
        {
            List<UnpostedVouchers> items = new List<UnpostedVouchers>();
            string selQuery = "select top 100 percent * from dbo.fnFinOperation_UnpostedVouchers(@Year) order by [VoucherDate],[DocumentKind],[VoucherNo] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    UnpostedVouchers item = new UnpostedVouchers();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.MonthlyNo = Convert.ToInt32(reader["MonthlyNo"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Subtotal = Convert.ToDecimal(reader["Subtotal"]);
                    item.vatAmount = Convert.ToDecimal(reader["vatAmount"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
