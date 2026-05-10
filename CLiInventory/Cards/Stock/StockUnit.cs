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
    public class StockUnit
    {
        public Guid? Key { get; set; }
        public Guid? Item { get; set; }
        public string Unit { get; set; }
        public int RateKind { get; set; }
        public decimal RateValue { get; set; }
        public Boolean Disable { get; set; }
        public List<StockUnit> GetList(string DB,Guid? ItemKey)
        {
            List<StockUnit> items = new List<StockUnit>();
            if (ItemKey == null)
                return items;

            string selQuery = "select top 100 percent * from InvStock_Unit where [sunit_Item]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = ItemKey;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    StockUnit item = new StockUnit();
                    item.Key = iCore.IsDbNullRtNull(reader["sunit_Key"]);
                    item.Item = iCore.IsDbNullRtNull(reader["sunit_Item"]);
                    item.Unit = Convert.ToString(reader["sunit_Unit"]);
                    item.RateKind = Convert.ToInt32(reader["sunit_RateKind"]);
                    item.RateValue = Convert.ToDecimal(reader["sunit_RateValue"]);
                    item.Disable = Convert.ToBoolean(reader["sunit_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public StockUnit GetItem(string DB,Guid? Key,Guid? Item)
        {
            StockUnit item = new StockUnit();
            item.Item = Item;
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from InvStock_Unit where [sunit_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["sunit_Key"]);
                    item.Item = iCore.IsDbNullRtNull(reader["sunit_Item"]);
                    item.Unit = Convert.ToString(reader["sunit_Unit"]);
                    item.RateKind = Convert.ToInt32(reader["sunit_RateKind"]);
                    item.RateValue = Convert.ToDecimal(reader["sunit_RateValue"]);
                    item.Disable = Convert.ToBoolean(reader["sunit_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, StockUnit item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO InvStock_Unit");
                str.Append("([sunit_Item]");
                str.Append(",[sunit_Unit]");
                str.Append(",[sunit_RateKind]");
                str.Append(",[sunit_RateValue]");
                str.Append(",[sunit_Disable])");
                str.Append(" VALUES ");
                str.Append("(@sunit_Item");
                str.Append(",@sunit_Unit");
                str.Append(",@sunit_RateKind");
                str.Append(",@sunit_RateValue");
                str.Append(",@sunit_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@sunit_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                comm.Parameters.Add("@sunit_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                comm.Parameters.Add("@sunit_RateKind", SqlDbType.Int).Value = item.RateKind;
                comm.Parameters.Add("@sunit_RateValue", SqlDbType.Decimal).Value = item.RateValue;
                comm.Parameters.Add("@sunit_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, StockUnit item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update InvStock_Unit SET ");
                str.Append("[sunit_Item]=@sunit_Item");
                str.Append(",[sunit_Unit]=@sunit_Unit");
                str.Append(",[sunit_RateKind]=@sunit_RateKind");
                str.Append(",[sunit_RateValue]=@sunit_RateValue");
                str.Append(",[sunit_Disable]=@sunit_Disable");
                str.Append(" WHERE sunit_Key=@sunit_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@sunit_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@sunit_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                comm.Parameters.Add("@sunit_Unit", SqlDbType.NVarChar, 25).Value = item.Unit ?? "";
                comm.Parameters.Add("@sunit_RateKind", SqlDbType.Int).Value = item.RateKind;
                comm.Parameters.Add("@sunit_RateValue", SqlDbType.Decimal).Value = item.RateValue;
                comm.Parameters.Add("@sunit_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static void Delete(string DB, Guid? Key)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [InvStock_Unit] where [sunit_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


    }
}
