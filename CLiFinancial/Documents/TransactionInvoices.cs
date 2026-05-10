using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiFinancial.Documents
{
    public class TransactionInvoices
    {
        public Guid? Key { get; set; }
        public Guid? InvoiceKey { get; set; }
        public decimal Amount { get; set; }
        public Guid? OperationKey { get; set; }
           public List<TransactionInvoices> GetList(string DB,Guid? Key)
        {
            List<TransactionInvoices> items = new List<TransactionInvoices>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from finDocument_TransactionInvoices where [cls_OperationKey]=@Key ";
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
                    TransactionInvoices item = new TransactionInvoices();
                    item.OperationKey = iCore.IsDbNullRtNull(reader["cls_OperationKey"]);
                    item.Key = iCore.IsDbNullRtNull(reader["cls_RowKey"]);
                    item.InvoiceKey = iCore.IsDbNullRtNull(reader["cls_InvoiceKey"]);
                    item.Amount = Convert.ToDecimal(reader["cls_Amount"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
