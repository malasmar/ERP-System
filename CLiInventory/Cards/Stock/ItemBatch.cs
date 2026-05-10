using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Cards.Stock
{
    public class ItemBatch
    {
        public Guid? Key { get; set; }
        public Guid? Item { get; set; }
        public string Batch { get; set; }
        public string Lot { get; set; }
        public DateTime? Expiry { get; set; }
        public decimal Price { get; set; }
        public Guid? vatKey { get; set; }
        public decimal Total { get; set; }
        public Boolean Disable { get; set; }
        public int PartRate { get; set; }
        public decimal PartPrice { get; set; }
        public List<ItemBatch> GetList(string DB,Guid? Item)
        {
            List<ItemBatch> items = new List<ItemBatch>();
            if (Item == null)
                return items;
            string selQuery = "select top 100 percent * from InvStock_Batch where [batch_Item]=@Key order by [batch_Expiry] desc";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Item;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ItemBatch item = new ItemBatch();
                    item.Key = iCore.IsDbNullRtNull(reader["batch_Key"]);
                    item.Item = iCore.IsDbNullRtNull(reader["batch_Item"]);
                    item.Batch = Convert.ToString(reader["batch_Batch"]);
                    item.Lot = Convert.ToString(reader["batch_Lot"]);
                    item.Expiry = iCore.IsDbNullRtNullDate(reader["batch_Expiry"]);
                    item.Price = Convert.ToDecimal(reader["batch_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["batch_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["batch_Total"]);
                    item.Disable = Convert.ToBoolean(reader["batch_Disable"]);
                    item.PartRate = Convert.ToInt32(reader["batch_PartRate"]);
                    item.PartPrice = Convert.ToDecimal(reader["batch_PartPrice"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public ItemBatch GetItem(string DB,Guid? Key,Guid? Item)
        {
            ItemBatch item = new ItemBatch();
            item.Item = Item;
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
                    item.Item = iCore.IsDbNullRtNull(reader["batch_Item"]);
                    item.Batch = Convert.ToString(reader["batch_Batch"]);
                    item.Lot = Convert.ToString(reader["batch_Lot"]);
                    item.Expiry = iCore.IsDbNullRtNullDate(reader["batch_Expiry"]);
                    item.Price = Convert.ToDecimal(reader["batch_Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["batch_vatKey"]);
                    item.Total = Convert.ToDecimal(reader["batch_Total"]);
                    item.Disable = Convert.ToBoolean(reader["batch_Disable"]);
                    item.PartRate = Convert.ToInt32(reader["batch_PartRate"]);
                    item.PartPrice = Convert.ToDecimal(reader["batch_PartPrice"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, ItemBatch item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO InvStock_Batch");
                str.Append("([batch_Item]");
                str.Append(",[batch_Batch]");
                str.Append(",[batch_Lot]");
                str.Append(",[batch_Expiry]");
                str.Append(",[batch_Price]");
                str.Append(",[batch_vatKey]");
                str.Append(",[batch_Total]");
                str.Append(",[batch_Disable]");
                str.Append(",[batch_PartRate]");
                str.Append(",[batch_PartPrice])");
                str.Append(" VALUES ");
                str.Append("(@batch_Item");
                str.Append(",@batch_Batch");
                str.Append(",@batch_Lot");
                str.Append(",@batch_Expiry");
                str.Append(",@batch_Price");
                str.Append(",@batch_vatKey");
                str.Append(",@batch_Total");
                str.Append(",@batch_Disable");
                str.Append(",@batch_PartRate");
                str.Append(",@batch_PartPrice)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@batch_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                comm.Parameters.Add("@batch_Batch", SqlDbType.NVarChar, 50).Value = item.Batch ?? "";
                comm.Parameters.Add("@batch_Lot", SqlDbType.NVarChar, 50).Value = item.Lot ?? "";
                comm.Parameters.Add("@batch_Expiry", SqlDbType.Date).Value = item.Expiry;
                comm.Parameters.Add("@batch_Price", SqlDbType.Decimal).Value = item.Price;
                comm.Parameters.Add("@batch_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                comm.Parameters.Add("@batch_Total", SqlDbType.Decimal).Value = item.Total;
                comm.Parameters.Add("@batch_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@batch_PartRate", SqlDbType.Int).Value = item.PartRate;
                comm.Parameters.Add("@batch_PartPrice", SqlDbType.Decimal).Value = item.PartPrice;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, ItemBatch item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update InvStock_Batch SET ");
                str.Append("[batch_Item]=@batch_Item");
                str.Append(",[batch_Batch]=@batch_Batch");
                str.Append(",[batch_Lot]=@batch_Lot");
                str.Append(",[batch_Expiry]=@batch_Expiry");
                str.Append(",[batch_Price]=@batch_Price");
                str.Append(",[batch_vatKey]=@batch_vatKey");
                str.Append(",[batch_Total]=@batch_Total");
                str.Append(",[batch_Disable]=@batch_Disable");
                str.Append(",[batch_PartRate]=@batch_PartRate");
                str.Append(",[batch_PartPrice]=@batch_PartPrice");
                str.Append(" WHERE batch_Key=@batch_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@batch_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@batch_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                comm.Parameters.Add("@batch_Batch", SqlDbType.NVarChar, 50).Value = item.Batch ?? "";
                comm.Parameters.Add("@batch_Lot", SqlDbType.NVarChar, 50).Value = item.Lot ?? "";
                comm.Parameters.Add("@batch_Expiry", SqlDbType.Date).Value = item.Expiry;
                comm.Parameters.Add("@batch_Price", SqlDbType.Decimal).Value = item.Price;
                comm.Parameters.Add("@batch_vatKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.vatKey);
                comm.Parameters.Add("@batch_Total", SqlDbType.Decimal).Value = item.Total;
                comm.Parameters.Add("@batch_Disable", SqlDbType.Bit).Value = item.Disable;
                comm.Parameters.Add("@batch_PartRate", SqlDbType.Int).Value = item.PartRate;
                comm.Parameters.Add("@batch_PartPrice", SqlDbType.Decimal).Value = item.PartPrice;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [InvStock_Batch] where [batch_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
