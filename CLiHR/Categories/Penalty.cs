using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiHR.Categories
{
    public class Penalty
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<Penalty> GetList(string DB)
        {
            List<Penalty> items = new List<Penalty>();
            string selQuery = "select top 100 percent * from hrCard_Categories_Penalty";
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
                    Penalty item = new Penalty();
                    item.Key = iCore.IsDbNullRtNull(reader["cpen_Key"]);
                    item.No = Convert.ToInt32(reader["cpen_No"]);
                    item.Name1 = Convert.ToString(reader["cpen_Name1"]);
                    item.Name2 = Convert.ToString(reader["cpen_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["cpen_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Penalty GetItem(string DB, Guid? Key)
        {
            Penalty item = new Penalty();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from hrCard_Categories_Penalty where [cpen_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["cpen_Key"]);
                    item.No = Convert.ToInt32(reader["cpen_No"]);
                    item.Name1 = Convert.ToString(reader["cpen_Name1"]);
                    item.Name2 = Convert.ToString(reader["cpen_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["cpen_Disable"]);
                }
                reader.Close();
            }
            return item;
        }
        public int MaxNo(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([cpen_No])+1,1) from [hrCard_Categories_Penalty]";
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
        public static void Insert(string DB, Penalty item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO hrCard_Categories_Penalty");
                str.Append("([cpen_No]");
                str.Append(",[cpen_Name1]");
                str.Append(",[cpen_Name2]");
                str.Append(",[cpen_Disable])");
                str.Append(" VALUES ");
                str.Append("(@cpen_No");
                str.Append(",@cpen_Name1");
                str.Append(",@cpen_Name2");
                str.Append(",@cpen_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cpen_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@cpen_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cpen_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cpen_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, Penalty item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update hrCard_Categories_Penalty SET ");
                str.Append("[cpen_No]=@cpen_No");
                str.Append(",[cpen_Name1]=@cpen_Name1");
                str.Append(",[cpen_Name2]=@cpen_Name2");
                str.Append(",[cpen_Disable]=@cpen_Disable");
                str.Append(" WHERE cpen_Key=@cpen_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cpen_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@cpen_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@cpen_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cpen_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cpen_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [hrCard_Categories_Penalty] where [cpen_Key]=@Key";
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
