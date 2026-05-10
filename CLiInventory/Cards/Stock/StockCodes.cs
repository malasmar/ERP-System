using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Cards.Stock
{
    public class StockCodes
    {
        public Guid? Key { get; set; }
        public Guid? Item { get; set; }
        public string Code { get; set; }
        public List<StockCodes> GetList(string DB,Guid? Key)
        {
            List<StockCodes> items = new List<StockCodes>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from invStock_ItemCodes where [cod_Item]=@Key order by [cod_Code]";
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
                    StockCodes item = new StockCodes();
                    item.Key = iCore.IsDbNullRtNull(reader["cod_Key"]);
                    item.Item = iCore.IsDbNullRtNull(reader["cod_Item"]);
                    item.Code = Convert.ToString(reader["cod_Code"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public static void Insert(string DB, StockCodes item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO invStock_ItemCodes");
                str.Append("([cod_Item]");
                str.Append(",[cod_Code])");
                str.Append(" VALUES ");
                str.Append("(@cod_Item");
                str.Append(",@cod_Code)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cod_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                comm.Parameters.Add("@cod_Code", SqlDbType.NVarChar, 200).Value = item.Code ?? "";
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, StockCodes item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update invStock_ItemCodes SET ");
                str.Append("[cod_Item]=@cod_Item");
                str.Append(",[cod_Code]=@cod_Code");
                str.Append(" WHERE cod_Key=@cod_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cod_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@cod_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Item);
                comm.Parameters.Add("@cod_Code", SqlDbType.NVarChar, 200).Value = item.Code ?? "";
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Delete(string DB, Guid? Key)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [invStock_ItemCodes] where [cod_Key]=@Key";
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
