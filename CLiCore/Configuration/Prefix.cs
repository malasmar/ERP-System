using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
namespace CLiCore.Configuration
{
    public class Prefix
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<Prefix> GetList(string DB)
        {
            List<Prefix> items = new List<Prefix>();
            string selQuery = "select top 100 percent * from com_Prefix";
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
                    Prefix item = new Prefix();
                    item.Key = iCore.IsDbNullRtNull(reader["prfx_Key"]);
                    item.No = Convert.ToInt32(reader["prfx_No"]);
                    item.Name1 = Convert.ToString(reader["prfx_Name1"]);
                    item.Name2 = Convert.ToString(reader["prfx_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["prfx_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Prefix GetItem(string DB,Guid? Key)
        {
            Prefix item = new Prefix();
            item.No = MaxOrder(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from com_Prefix";
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
                    item.Key = iCore.IsDbNullRtNull(reader["prfx_Key"]);
                    item.No = Convert.ToInt32(reader["prfx_No"]);
                    item.Name1 = Convert.ToString(reader["prfx_Name1"]);
                    item.Name2 = Convert.ToString(reader["prfx_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["prfx_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Prefix item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO com_Prefix");
                str.Append("([prfx_No]");
                str.Append(",[prfx_Name1]");
                str.Append(",[prfx_Disable])");
                str.Append(" VALUES ");
                str.Append("(@prfx_No");
                str.Append(",@prfx_Name1");
                str.Append(",@prfx_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@prfx_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@prfx_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@prfx_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@prfx_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Prefix item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update com_Prefix SET ");
                str.Append("[prfx_No]=@prfx_No");
                str.Append(",[prfx_Name1]=@prfx_Name1");
                str.Append(",[prfx_Name2]=@prfx_Name2");
                str.Append(",[prfx_Disable]=@prfx_Disable");
                str.Append(" WHERE prfx_Key=@prfx_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@prfx_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@prfx_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@prfx_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@prfx_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@prfx_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([prfx_No])+1,1) from [com_Prefix]";
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
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("delete from [com_Prefix] where [prfx_Key]=@Key ");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = ScStr.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                con.Open();
                res = comm.ExecuteNonQuery();
            }
            return res;
        }


    }
}
