using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Documents
{
    public class Transaction
    {
        public Guid? OperationKey { get; set; }
        public DateTime? CreateDate { get; set; }
        public int DocumentKind { get; set; }
        public int No { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public int Rows { get; set; }
        public int Document { get; set; }
        public int VoucherNo { get; set; }
        public List<Transaction> GetList(string DB,int Year)
        {
            DateTime first = new DateTime(Year, 1, 1);
            DateTime last = new DateTime(Year, 12, 31);
            List<Transaction> items = new List<Transaction>();
            string selQuery = "select top 100 percent * from dbo.fnaccDocuments_GeneralLedgerList(@FirstDate,@LastDate) order by [Date] desc,[No] desc ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = first;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Transaction item = new Transaction();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["CreateDate"]);
                    item.DocumentKind = Convert.ToInt32(reader["DocumentKind"]);
                    item.No = Convert.ToInt32(reader["No"]);
                    item.Date = iCore.IsDbNullRtNullDate(reader["Date"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Rows = Convert.ToInt32(reader["Rows"]);
                    item.Document = Convert.ToInt32(reader["Document"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
