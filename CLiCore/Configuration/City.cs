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
    public class City
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<City> GetList(string DB)
        {
            List<City> items = new List<City>();
            string selQuery = "select top 100 percent * from com_City";
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
                    City item = new City();
                    item.Key = iCore.IsDbNullRtNull(reader["cit_Key"]);
                    item.No = Convert.ToInt32(reader["cit_No"]);
                    item.Name1 = Convert.ToString(reader["cit_Name1"]);
                    item.Name2 = Convert.ToString(reader["cit_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["cit_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public City GetItem(string DB,Guid? Key)
        {
            City item = new City();
            item.No = MaxOrder(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from com_City";
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
                    item.Key = iCore.IsDbNullRtNull(reader["cit_Key"]);
                    item.No = Convert.ToInt32(reader["cit_No"]);
                    item.Name1 = Convert.ToString(reader["cit_Name1"]);
                    item.Name2 = Convert.ToString(reader["cit_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["cit_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, City item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO com_City");
                str.Append("([cit_No]");
                str.Append(",[cit_Name1]");
                str.Append(",[cit_Disable])");
                str.Append(" VALUES ");
                str.Append("(@cit_No");
                str.Append(",@cit_Name1");
                str.Append(",@cit_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cit_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@cit_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cit_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cit_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, City item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update com_City SET ");
                str.Append("[cit_No]=@cit_No");
                str.Append(",[cit_Name1]=@cit_Name1");
                str.Append(",[cit_Name2]=@cit_Name2");
                str.Append(",[cit_Disable]=@cit_Disable");
                str.Append(" WHERE cit_Key=@cit_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cit_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@cit_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@cit_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cit_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cit_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([cit_No])+1,1) from [com_City]";
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
                ScStr.Append("delete from [com_City] where [cit_Key]=@Key ");
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
