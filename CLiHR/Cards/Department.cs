using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiHR.Cards
{
    public class Department
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<Department> GetList(string DB)
        {
            List<Department> items = new List<Department>();
            string selQuery = "select top 100 percent * from hrCard_Department order by [dep_No] ";
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
                    Department item = new Department();
                    item.Key = iCore.IsDbNullRtNull(reader["dep_Key"]);
                    item.No = Convert.ToInt32(reader["dep_No"]);
                    item.Name1 = Convert.ToString(reader["dep_Name1"]);
                    item.Name2 = Convert.ToString(reader["dep_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["dep_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
        public Department GetItem(string DB,Guid? Key)
        {
            Department item = new Department();
            item.No = MaxNo(DB);
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from hrCard_Department where [dep_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["dep_Key"]);
                    item.No = Convert.ToInt32(reader["dep_No"]);
                    item.Name1 = Convert.ToString(reader["dep_Name1"]);
                    item.Name2 = Convert.ToString(reader["dep_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["dep_Disable"]);
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB, Department item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO hrCard_Department");
                str.Append("([dep_No]");
                str.Append(",[dep_Name1]");
                str.Append(",[dep_Name2]");
                str.Append(",[dep_Disable])");
                str.Append(" VALUES ");
                str.Append("(@dep_No");
                str.Append(",@dep_Name1");
                str.Append(",@dep_Name2");
                str.Append(",@dep_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@dep_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@dep_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@dep_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@dep_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public static void Update(string DB, Department item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update hrCard_Department SET ");
                str.Append("[dep_No]=@dep_No");
                str.Append(",[dep_Name1]=@dep_Name1");
                str.Append(",[dep_Name2]=@dep_Name2");
                str.Append(",[dep_Disable]=@dep_Disable");
                str.Append(" WHERE dep_Key=@dep_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@dep_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@dep_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@dep_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@dep_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@dep_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public int MaxNo(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([dep_No])+1,1) from [hrCard_Department]";
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
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [hrCard_Department] where [dep_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
              res=  comm.ExecuteNonQuery();
            }
            return res;
        }
    }
}
