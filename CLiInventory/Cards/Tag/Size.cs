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
    public class Size
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<Size> GetList(string DB)
        {
            List<Size> items = new List<Size>();
            string selQuery = "select top 100 percent * from invStockTag_Size order by [Size_No] ";
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
                    Size item = new Size();
                    item.Key = iCore.IsDbNullRtNull(reader["Size_Key"]);
                    item.No = Convert.ToInt32(reader["Size_No"]);
                    item.Name1 = Convert.ToString(reader["Size_Name1"]);
                    item.Name2 = Convert.ToString(reader["Size_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["Size_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Size GetItem(string DB,Guid? Key)
        {
            Size item = new Size();
            item.No = MaxOrder(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from invStockTag_Size where [Size_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Size_Key"]);
                    item.No = Convert.ToInt32(reader["Size_No"]);
                    item.Name1 = Convert.ToString(reader["Size_Name1"]);
                    item.Name2 = Convert.ToString(reader["Size_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["Size_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Size item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO invStockTag_Size");
                str.Append("([Size_No]");
                str.Append(",[Size_Name1]");
                str.Append(",[Size_Name2]");
                str.Append(",[Size_Disable])");
                str.Append(" VALUES ");
                str.Append("(@Size_No");
                str.Append(",@Size_Name1");
                str.Append(",@Size_Name2");
                str.Append(",@Size_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Size_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@Size_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Size_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Size_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Size item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update invStockTag_Size SET ");
                str.Append("[Size_No]=@Size_No");
                str.Append(",[Size_Name1]=@Size_Name1");
                str.Append(",[Size_Name2]=@Size_Name2");
                str.Append(",[Size_Disable]=@Size_Disable");
                str.Append(" WHERE Size_Key=@Size_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Size_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@Size_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@Size_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Size_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Size_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([Size_No])+1,1) from [invStockTag_Size]";
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
                string delQuery = " Delete from [invStockTag_Size] where [Size_Key]=@Key";
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
