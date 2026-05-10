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
    public class Color
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<Color> GetList(string DB)
        {
            List<Color> items = new List<Color>();
            string selQuery = "select top 100 percent * from invStockTag_Color order by [color_No] ";
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
                    Color item = new Color();
                    item.Key = iCore.IsDbNullRtNull(reader["color_Key"]);
                    item.No = Convert.ToInt32(reader["color_No"]);
                    item.Name1 = Convert.ToString(reader["color_Name1"]);
                    item.Name2 = Convert.ToString(reader["color_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["color_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Color GetItem(string DB,Guid? Key)
        {
            Color item = new Color();
            item.No = MaxOrder(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from invStockTag_Color where [color_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["color_Key"]);
                    item.No = Convert.ToInt32(reader["color_No"]);
                    item.Name1 = Convert.ToString(reader["color_Name1"]);
                    item.Name2 = Convert.ToString(reader["color_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["color_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Color item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO invStockTag_Color");
                str.Append("([color_No]");
                str.Append(",[color_Name1]");
                str.Append(",[color_Name2]");
                str.Append(",[color_Disable])");
                str.Append(" VALUES ");
                str.Append("(@color_No");
                str.Append(",@color_Name1");
                str.Append(",@color_Name2");
                str.Append(",@color_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@color_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@color_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@color_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@color_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Color item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update invStockTag_Color SET ");
                str.Append("[color_No]=@color_No");
                str.Append(",[color_Name1]=@color_Name1");
                str.Append(",[color_Name2]=@color_Name2");
                str.Append(",[color_Disable]=@color_Disable");
                str.Append(" WHERE color_Key=@color_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@color_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@color_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@color_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@color_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@color_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([color_No])+1,1) from [invStockTag_Color]";
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
                string delQuery = " Delete from [invStockTag_Color] where [color_Key]=@Key";
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
