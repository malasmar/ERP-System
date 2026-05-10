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
    public class CostCenter
    {
        public Guid? Key { get; set; }
        public Guid? Parent { get; set; }
        public int Kind { get; set; }
        public Boolean Transaction { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Comment { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public Boolean Disable { get; set; }
        public List<CostCenter> GetList(string DB)
        {
            List<CostCenter> items = new List<CostCenter>();
            string selQuery = "select top 100 percent * from finCard_CostCenter order by [cst_Code] ";
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
                    CostCenter item = new CostCenter();
                    item.Key = iCore.IsDbNullRtNull(reader["cst_Key"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["cst_Parent"]);
                    item.Kind = Convert.ToInt32(reader["cst_Kind"]);
                    item.Transaction = Convert.ToBoolean(reader["cst_Transaction"]);
                    item.Code = Convert.ToString(reader["cst_Code"]);
                    item.Name1 = Convert.ToString(reader["cst_Name1"]);
                    item.Name2 = Convert.ToString(reader["cst_Name2"]);
                    item.Comment = Convert.ToString(reader["cst_Comment"]);
                    item.Phone1 = Convert.ToString(reader["cst_Phone1"]);
                    item.Phone2 = Convert.ToString(reader["cst_Phone2"]);
                    item.Disable = Convert.ToBoolean(reader["cst_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public CostCenter GetItem(string DB, Guid? Key)
        {
            CostCenter item = new CostCenter();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from finCard_CostCenter where [cst_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["cst_Key"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["cst_Parent"]);
                    item.Kind = Convert.ToInt32(reader["cst_Kind"]);
                    item.Transaction = Convert.ToBoolean(reader["cst_Transaction"]);
                    item.Code = Convert.ToString(reader["cst_Code"]);
                    item.Name1 = Convert.ToString(reader["cst_Name1"]);
                    item.Name2 = Convert.ToString(reader["cst_Name2"]);
                    item.Comment = Convert.ToString(reader["cst_Comment"]);
                    item.Phone1 = Convert.ToString(reader["cst_Phone1"]);
                    item.Phone2 = Convert.ToString(reader["cst_Phone2"]);
                    item.Disable = Convert.ToBoolean(reader["cst_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, CostCenter item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finCard_CostCenter");
                str.Append("([cst_Parent]");
                str.Append(",[cst_Kind]");
                str.Append(",[cst_Transaction]");
                str.Append(",[cst_Code]");
                str.Append(",[cst_Name1]");
                str.Append(",[cst_Name2]");
                str.Append(",[cst_Comment]");
                str.Append(",[cst_Phone1]");
                str.Append(",[cst_Phone2]");
                str.Append(",[cst_Disable])");
                str.Append(" VALUES ");
                str.Append("(@cst_Parent");
                str.Append(",@cst_Kind");
                str.Append(",@cst_Transaction");
                str.Append(",@cst_Code");
                str.Append(",@cst_Name1");
                str.Append(",@cst_Name2");
                str.Append(",@cst_Comment");
                str.Append(",@cst_Phone1");
                str.Append(",@cst_Phone2");
                str.Append(",@cst_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cst_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@cst_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@cst_Transaction", SqlDbType.Bit).Value = item.Transaction;
                comm.Parameters.Add("@cst_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@cst_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cst_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cst_Comment", SqlDbType.NVarChar, 200).Value = item.Comment ?? "";
                comm.Parameters.Add("@cst_Phone1", SqlDbType.NVarChar, 15).Value = item.Phone1 ?? "";
                comm.Parameters.Add("@cst_Phone2", SqlDbType.NVarChar, 15).Value = item.Phone2 ?? "";
                comm.Parameters.Add("@cst_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, CostCenter item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update finCard_CostCenter SET ");
                str.Append("[cst_Parent]=@cst_Parent");
                str.Append(",[cst_Kind]=@cst_Kind");
                str.Append(",[cst_Transaction]=@cst_Transaction");
                str.Append(",[cst_Code]=@cst_Code");
                str.Append(",[cst_Name1]=@cst_Name1");
                str.Append(",[cst_Name2]=@cst_Name2");
                str.Append(",[cst_Comment]=@cst_Comment");
                str.Append(",[cst_Phone1]=@cst_Phone1");
                str.Append(",[cst_Phone2]=@cst_Phone2");
                str.Append(",[cst_Disable]=@cst_Disable");
                str.Append(" WHERE cst_Key=@cst_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cst_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@cst_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@cst_Kind", SqlDbType.Int).Value = item.Kind;
                comm.Parameters.Add("@cst_Transaction", SqlDbType.Bit).Value = item.Transaction;
                comm.Parameters.Add("@cst_Code", SqlDbType.NVarChar, 50).Value = item.Code ?? "";
                comm.Parameters.Add("@cst_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cst_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cst_Comment", SqlDbType.NVarChar, 200).Value = item.Comment ?? "";
                comm.Parameters.Add("@cst_Phone1", SqlDbType.NVarChar, 15).Value = item.Phone1 ?? "";
                comm.Parameters.Add("@cst_Phone2", SqlDbType.NVarChar, 15).Value = item.Phone2 ?? "";
                comm.Parameters.Add("@cst_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [finCard_CostCenter] where [cst_Key]=@Key";
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
