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
    public class Reward
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<Reward> GetList(string DB)
        {
            List<Reward> items = new List<Reward>();
            string selQuery = "select top 100 percent * from hrCard_Categories_Reward";
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
                    Reward item = new Reward();
                    item.Key = iCore.IsDbNullRtNull(reader["crew_Key"]);
                    item.No = Convert.ToInt32(reader["crew_No"]);
                    item.Name1 = Convert.ToString(reader["crew_Name1"]);
                    item.Name2 = Convert.ToString(reader["crew_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["crew_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Reward GetItem(string DB, Guid? Key)
        {
            Reward item = new Reward();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from hrCard_Categories_Reward where [crew_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["crew_Key"]);
                    item.No = Convert.ToInt32(reader["crew_No"]);
                    item.Name1 = Convert.ToString(reader["crew_Name1"]);
                    item.Name2 = Convert.ToString(reader["crew_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["crew_Disable"]);
                }
                reader.Close();
            }
            return item;
        }
        public int MaxNo(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([crew_No])+1,1) from [hrCard_Categories_Reward]";
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
        public static void Insert(string DB, Reward item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO hrCard_Categories_Reward");
                str.Append("([crew_No]");
                str.Append(",[crew_Name1]");
                str.Append(",[crew_Name2]");
                str.Append(",[crew_Disable])");
                str.Append(" VALUES ");
                str.Append("(@crew_No");
                str.Append(",@crew_Name1");
                str.Append(",@crew_Name2");
                str.Append(",@crew_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@crew_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@crew_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@crew_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@crew_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, Reward item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update hrCard_Categories_Reward SET ");
                str.Append("[crew_No]=@crew_No");
                str.Append(",[crew_Name1]=@crew_Name1");
                str.Append(",[crew_Name2]=@crew_Name2");
                str.Append(",[crew_Disable]=@crew_Disable");
                str.Append(" WHERE crew_Key=@crew_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@crew_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@crew_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@crew_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@crew_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@crew_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [hrCard_Categories_Reward] where [crew_Key]=@Key";
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
