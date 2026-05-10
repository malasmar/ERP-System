using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Cards.Tag
{
    public class Price
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<Price> GetList(string DB)
        {
            List<Price> items = new List<Price>();
            string selQuery = "select top 100 percent * from invStockTag_Price order by [Price_No]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Price item = new Price();
                    item.Key = iCore.IsDbNullRtNull(reader["Price_Key"]);
                    item.No = Convert.ToInt32(reader["Price_No"]);
                    item.Name1 = Convert.ToString(reader["Price_Name1"]);
                    item.Name2 = Convert.ToString(reader["Price_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["Price_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Price GetItem(string DB,Guid? Key)
        {
            Price item = new Price();
            item.No = MaxOrder(DB);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from invStockTag_Price where [Price_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Price_Key"]);
                    item.No = Convert.ToInt32(reader["Price_No"]);
                    item.Name1 = Convert.ToString(reader["Price_Name1"]);
                    item.Name2 = Convert.ToString(reader["Price_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["Price_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Price item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO invStockTag_Price");
                str.Append("([Price_No]");
                str.Append(",[Price_Name1]");
                str.Append(",[Price_Name2]");
                str.Append(",[Price_Disable])");
                str.Append(" VALUES ");
                str.Append("(@Price_No");
                str.Append(",@Price_Name1");
                str.Append(",@Price_Name2");
                str.Append(",@Price_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Price_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@Price_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Price_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Price_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Price item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update invStockTag_Price SET ");
                str.Append("[Price_No]=@Price_No");
                str.Append(",[Price_Name1]=@Price_Name1");
                str.Append(",[Price_Name2]=@Price_Name2");
                str.Append(",[Price_Disable]=@Price_Disable");
                str.Append(" WHERE Price_Key=@Price_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Price_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@Price_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@Price_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Price_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Price_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([Price_No])+1,1) from [invStockTag_Price]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.CommandText = selQuery;
                comm.CommandType = CommandType.Text;
                comm.Connection = con;
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    res = (int)reader[0];
                }
                reader.Close();
            }
            return res;
        }

        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [invStockTag_Price] where [Price_Key]=@Key";
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
