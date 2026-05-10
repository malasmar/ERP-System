using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Reports.Ages
{
    public class AgesDetails
    {

        public Guid? Key { get; set; }
        public int DocumentKind { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? DueDate { get; set; }
        public int DueDays { get; set; }
        public decimal Total { get; set; }
        public decimal ClosedAmount { get; set; }
        public decimal Remaining { get; set; }

        public List<AgesDetails> GetList(string DB,Guid? Key)
        {
            List<AgesDetails> items = new List<AgesDetails>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.ReportFin_AgesDetails(@Key) where [Total]-[ClosedAmount]>0 order by [InvoiceDate],[InvoiceNo]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key",SqlDbType.UniqueIdentifier).Value= Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AgesDetails item = new AgesDetails();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.DueDate = iCore.IsDbNullRtNullDate(reader["DueDate"]);
                    item.DueDays = Convert.ToInt32(reader["DueDays"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.ClosedAmount = Convert.ToDecimal(reader["ClosedAmount"]);
                    item.Remaining = item.Total - item.ClosedAmount;
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
