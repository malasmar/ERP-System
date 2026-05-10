using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Audit.Cost
{
    public class ItemCostDetails
    {
        public int RecNo { get; set; }
        public Guid? Key { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal Balance { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LastCost { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Additional { get; set; }
        public List<ItemCostDetails> GetList(string DB,Guid? Key)
        {
            List<ItemCostDetails> items = new List<ItemCostDetails>();
            if(Key==null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fnInvAudit_ItemCostDetails(@Key) order by [InvoiceDate],[InvoiceNo] ";
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
                    ItemCostDetails item = new ItemCostDetails();
                    item.RecNo = Convert.ToInt32(reader["RecNo"]);
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.InvoiceNo = Convert.ToInt32(reader["InvoiceNo"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    item.Balance = Convert.ToDecimal(reader["Balance"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    item.LastCost = Convert.ToDecimal(reader["LastCost"]);
                    item.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                    item.Additional = Convert.ToDecimal(reader["Additional"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
