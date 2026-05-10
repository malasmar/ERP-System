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
    public class ClientCategories
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public Guid? Parent { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public List<ClientCategories> GetList(string DB)
        {
            List<ClientCategories> items = new List<ClientCategories>();
            string selQuery = "select top 100 percent * from AppSales_ClientCategories";
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
                    ClientCategories item = new ClientCategories();
                    item.Key = iCore.IsDbNullRtNull(reader["cat_Key"]);
                    item.No = Convert.ToInt32(reader["cat_No"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["cat_Parent"]);
                    item.Name1 = Convert.ToString(reader["cat_Name1"]);
                    item.Name2 = Convert.ToString(reader["cat_Name2"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public ClientCategories GetItem(string DB,Guid? Key)
        {
            ClientCategories item = new ClientCategories();
            item.No = MaxOrder(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from AppSales_ClientCategories where [cat_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["cat_Key"]);
                    item.No = Convert.ToInt32(reader["cat_No"]);
                    item.Parent = iCore.IsDbNullRtNull(reader["cat_Parent"]);
                    item.Name1 = Convert.ToString(reader["cat_Name1"]);
                    item.Name2 = Convert.ToString(reader["cat_Name2"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, ClientCategories item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO AppSales_ClientCategories");
                str.Append("([cat_No]");
                str.Append(",[cat_Parent]");
                str.Append(",[cat_Name1]");
                str.Append(",[cat_Name2])");
                str.Append(" VALUES ");
                str.Append("(@cat_No");
                str.Append(",@cat_Parent");
                str.Append(",@cat_Name1");
                str.Append(",@cat_Name2)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cat_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@cat_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@cat_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cat_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, ClientCategories item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update AppSales_ClientCategories SET ");
                str.Append("[cat_No]=@cat_No");
                str.Append(",[cat_Parent]=@cat_Parent");
                str.Append(",[cat_Name1]=@cat_Name1");
                str.Append(",[cat_Name2]=@cat_Name2");
                str.Append(" WHERE cat_Key=@cat_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cat_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@cat_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@cat_Parent", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Parent);
                comm.Parameters.Add("@cat_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cat_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([cat_No])+1,1) from [AppSales_ClientCategories]";
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
                ScStr.Append("delete from [AppSales_ClientCategories] where [cat_Key]=@Key ");
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
