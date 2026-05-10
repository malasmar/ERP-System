using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Documents
{
    public class TransactionBatch
    {
        public Guid? Key { get; set; }
        public string Batch { get; set; }
        public string Lot { get; set; }
        public DateTime? Expiry { get; set; }
        public decimal Price { get; set; }
        public Guid? vatKey { get; set; }
        public decimal Total { get; set; }
        public int PartRate { get; set; }
        public decimal PartPrice { get; set; }
        public TransactionBatch GetItem(string DB, Guid? Key)
        {
            TransactionBatch item = new TransactionBatch();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from InvStock_Batch where batch_Key=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["batch_Key"]);
                    item.Batch = Convert.ToString(reader["batch_Batch"]);
                    item.Lot = Convert.ToString(reader["batch_Lot"]);
                    item.Expiry = iCore.IsDbNullRtNullDate(reader["batch_Expiry"]);
                    item.Price = Convert.ToDecimal(reader["batch_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["batch_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["batch_Total"]);
                    item.PartRate = Convert.ToInt32(reader["batch_PartRate"]);
                    item.PartPrice = Convert.ToDecimal(reader["batch_PartPrice"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
