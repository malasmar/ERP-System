using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiAccounting.Cards
{
    public class BalanceSheetItems
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Parent { get; set; }
        public int Kind { get; set; }
        public Boolean Transaction { get; set; }
        public Boolean Disable { get; set; }
        public List<BalanceSheetItems> GetList(string DB)
        {
            List<BalanceSheetItems> items = new List<BalanceSheetItems>();
            string selQuery = "select top 100 percent * from accCard_BalanceSheetItems order by [bsi_Code] ";
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
                    BalanceSheetItems item = new BalanceSheetItems();
                    item.Key = iCore.IsDbNullRtNull(reader["bsi_Key"]);
                    item.Code = Convert.ToString(reader["bsi_Code"]);
                    item.Name1 = Convert.ToString(reader["bsi_Name1"]);
                    item.Name2 = Convert.ToString(reader["bsi_Name2"]);
                    item.Parent = Convert.ToString(reader["bsi_Parent"]);
                    item.Kind = Convert.ToInt32(reader["bsi_Kind"]);
                    item.Transaction = Convert.ToBoolean(reader["bsi_Transaction"]);
                    item.Disable = Convert.ToBoolean(reader["bsi_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public BalanceSheetItems GetItem(string DB,Guid? Key)
        {
            BalanceSheetItems item = new BalanceSheetItems();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from accCard_BalanceSheetItems where [bsi_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["bsi_Key"]);
                    item.Code = Convert.ToString(reader["bsi_Code"]);
                    item.Name1 = Convert.ToString(reader["bsi_Name1"]);
                    item.Name2 = Convert.ToString(reader["bsi_Name2"]);
                    item.Parent = Convert.ToString(reader["bsi_Parent"]);
                    item.Kind = Convert.ToInt32(reader["bsi_Kind"]);
                    item.Transaction = Convert.ToBoolean(reader["bsi_Transaction"]);
                    item.Disable = Convert.ToBoolean(reader["bsi_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, BalanceSheetItems item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO accCard_BalanceSheetItems");
                str.Append("([bsi_Code]");
                str.Append(",[bsi_Name1]");
                str.Append(",[bsi_Name2]");
                str.Append(",[bsi_Parent]");
                str.Append(",[bsi_Kind]");
                str.Append(",[bsi_Transaction]");
                str.Append(",[bsi_Disable])");
                str.Append(" VALUES ");
                str.Append("(@bsi_Code");
                str.Append(",@bsi_Name1");
                str.Append(",@bsi_Name2");
                str.Append(",@bsi_Parent");
                str.Append(",@bsi_Kind");
                str.Append(",@bsi_Transaction");
                str.Append(",@bsi_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@bsi_Code", SqlDbType.NVarChar, 15).Value = item.Code ?? "";
                comm.Parameters.Add("@bsi_Name1", SqlDbType.NVarChar, 250).Value = item.Name1 ?? "";
                comm.Parameters.Add("@bsi_Name2", SqlDbType.NVarChar, 250).Value = item.Name2 ?? "";
                comm.Parameters.Add("@bsi_Parent", SqlDbType.NVarChar, 15).Value = item.Parent ?? "";
                comm.Parameters.Add("@bsi_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@bsi_Transaction", SqlDbType.Bit).Value = item.Transaction;
                comm.Parameters.Add("@bsi_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, BalanceSheetItems item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update accCard_BalanceSheetItems SET ");
                str.Append("[bsi_Code]=@bsi_Code");
                str.Append(",[bsi_Name1]=@bsi_Name1");
                str.Append(",[bsi_Name2]=@bsi_Name2");
                str.Append(",[bsi_Parent]=@bsi_Parent");
                str.Append(",[bsi_Kind]=@bsi_Kind");
                str.Append(",[bsi_Transaction]=@bsi_Transaction");
                str.Append(",[bsi_Disable]=@bsi_Disable");
                str.Append(" WHERE bsi_Key=@bsi_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@bsi_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@bsi_Code", SqlDbType.NVarChar, 15).Value = item.Code ?? "";
                comm.Parameters.Add("@bsi_Name1", SqlDbType.NVarChar, 250).Value = item.Name1 ?? "";
                comm.Parameters.Add("@bsi_Name2", SqlDbType.NVarChar, 250).Value = item.Name2 ?? "";
                comm.Parameters.Add("@bsi_Parent", SqlDbType.NVarChar, 15).Value = item.Parent ?? "";
                comm.Parameters.Add("@bsi_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@bsi_Transaction", SqlDbType.Bit).Value = item.Transaction;
                comm.Parameters.Add("@bsi_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static void Delete(string DB, Guid? Key)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [accCard_BalanceSheetItems] where [bsi_Key]=@Key";
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
