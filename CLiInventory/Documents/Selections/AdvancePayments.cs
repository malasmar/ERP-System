using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Documents.Selections
{
    public class AdvancePayments
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int VoucherNo { get; set; }
        public Guid? RowKey { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal xAmount { get; set; }
        public List<AdvancePayments> GetList(string DB,Guid? Key)
        {
            List<AdvancePayments> items = new List<AdvancePayments>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnfinSelection_InvoiceAdvancePayments(@Key) where [Amount]>0 order by [VoucherDate]";
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
                    AdvancePayments item = new AdvancePayments();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["OperationKey"]);
                    item.VoucherDate = iCore.IsDbNullRtNullDate(reader["VoucherDate"]);
                    item.VoucherNo = Convert.ToInt32(reader["VoucherNo"]);
                    item.RowKey = iCore.IsDbNullRtNull(reader["RowKey"]);
                    item.Description = Convert.ToString(reader["Description"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
