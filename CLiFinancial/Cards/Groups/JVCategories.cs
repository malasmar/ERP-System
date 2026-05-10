using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
namespace CLiFinancial.Cards.Groups
{
    public class JVCategories
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }

        public List<JVCategories> GetList(string DB)
        {
            List<JVCategories> items = new List<JVCategories>();
            string selQuery = "select top 100 percent * from finCard_CategoriesJournalVoucher order by [cat_No] ";
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
                    JVCategories item = new JVCategories();
                    item.Key = iCore.IsDbNullRtNull(reader["cat_Key"]);
                    item.No = Convert.ToInt32(reader["cat_No"]);
                    item.Name1 = Convert.ToString(reader["cat_Name1"]);
                    item.Name2 = Convert.ToString(reader["cat_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["cat_Disable"]);

                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public JVCategories GetItem(string DB, Guid? Key)
        {
            JVCategories item = new JVCategories();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from finCard_CategoriesJournalVoucher where [cat_Key]=@Key ";
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
                    item.Name1 = Convert.ToString(reader["cat_Name1"]);
                    item.Name2 = Convert.ToString(reader["cat_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["cat_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, JVCategories item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO finCard_CategoriesJournalVoucher");
                str.Append("([cat_No]");
                str.Append(",[cat_Name1]");
                str.Append(",[cat_Name2]");
                str.Append(",[cat_Disable])");
                str.Append(" VALUES ");
                str.Append("(@cat_No");
                str.Append(",@cat_Name1");
                str.Append(",@cat_Name2");
                str.Append(",@cat_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cat_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@cat_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cat_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cat_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, JVCategories item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update finCard_CategoriesJournalVoucher SET ");
                str.Append("[cat_No]=@cat_No");
                str.Append(",[cat_Name1]=@cat_Name1");
                str.Append(",[cat_Name2]=@cat_Name2");
                str.Append(",[cat_Disable]=@cat_Disable");
                str.Append(" WHERE cat_Key=@cat_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cat_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@cat_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@cat_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cat_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cat_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [finCard_CategoriesJournalVoucher] where [cat_Key]=@Key";
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
        public int MaxNo(string DB)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "select top 100 percent isnull(max([cat_No])+1,1) from [finCard_CategoriesJournalVoucher]";
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    res = Convert.ToInt32(reader[0]);
                }
                reader.Close();
            }
            return res;
        }
    }
}
