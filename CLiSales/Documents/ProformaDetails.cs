using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiSales.Documents
{
    public class ProformaDetails
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public Guid? Key { get; set; }
        public int Index { get; set; }
        public int ItemKind { get; set; }
        public Guid? ItemKey { get; set; }
        public int Color { get; set; }
        public int Size { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Bonus { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal vatAmount { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }
        public JsonData.AccountDetails jnAccount { get; set; }
        public JsonData.vatDetails jnvatDetails { get; set; }
        public List<ProformaDetails> GetList(string DB,string xLan,Guid? Key)
        {
            List<ProformaDetails> items = new List<ProformaDetails>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from SalesDocument_ProformaDetails where [sal_OperationKey]=@key order by [sal_Index] ";
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
                    ProformaDetails item = new ProformaDetails();
                    item.RecNo = Convert.ToInt32(reader["sal_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["sal_OperationKey"]);
                    item.Key = iCore.IsDbNullRtNull(reader["sal_Key"]);
                    item.Index = Convert.ToInt32(reader["sal_Index"]);
                    item.ItemKind = Convert.ToInt32(reader["sal_ItemKind"]);
                    item.ItemKey = iCore.IsDbNullRtNull(reader["sal_ItemKey"]);
                    item.Color = Convert.ToInt32(reader["sal_Color"]);
                    item.Size = Convert.ToInt32(reader["sal_Size"]);
                    item.Unit = Convert.ToString(reader["sal_Unit"]);
                    item.UnitPrice = Convert.ToDecimal(reader["sal_UnitPrice"]);
                    item.Quantity = Convert.ToDecimal(reader["sal_Quantity"]);
                    item.Bonus = Convert.ToDecimal(reader["sal_Bonus"]);
                    item.Amount = Convert.ToDecimal(reader["sal_Amount"]);
                    item.Discount = Convert.ToDecimal(reader["sal_Discount"]);
                    item.vatAmount = Convert.ToDecimal(reader["sal_vatAmount"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["sal_vatKey"]);
                    item.vatRate = Convert.ToDecimal(reader["sal_vatRate"]);
                    item.Total = Convert.ToDecimal(reader["sal_Total"]);
                    item.Description = Convert.ToString(reader["sal_Description"]);
                    item.jnAccount = new JsonData.AccountDetails().GetItem(DB, xLan, item.ItemKey);
                    item.jnvatDetails = new JsonData.vatDetails().GetItem(DB, xLan, item.vatKey);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
