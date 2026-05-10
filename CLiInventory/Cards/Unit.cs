using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Cards
{
    public class Unit
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<Unit> GetList(string DB)
        {
            List<Unit> items = new List<Unit>();
            string selQuery = "select top 100 percent * from invCard_Unit";
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
                    Unit item = new Unit();
                    item.Key = iCore.IsDbNullRtNull(reader["unit_Key"]);
                    item.Code = Convert.ToString(reader["unit_Code"]);
                    item.Name1 = Convert.ToString(reader["unit_Name1"]);
                    item.Name2 = Convert.ToString(reader["unit_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["unit_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Unit GetItem(string DB,Guid? Key)
        {
            Unit item = new Unit();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from invCard_Unit where [unit_Key]=@Key";
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
                    item.Key = iCore.IsDbNullRtNull(reader["unit_Key"]);
                    item.Code = Convert.ToString(reader["unit_Code"]);
                    item.Name1 = Convert.ToString(reader["unit_Name1"]);
                    item.Name2 = Convert.ToString(reader["unit_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["unit_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Unit item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO invCard_Unit");
                str.Append("([unit_Code]");
                str.Append(",[unit_Name1]");
                str.Append(",[unit_Name2]");
                str.Append(",[unit_Disable])");
                str.Append(" VALUES ");
                str.Append("(@unit_Code");
                str.Append(",@unit_Name1");
                str.Append(",@unit_Name2");
                str.Append(",@unit_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@unit_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@unit_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@unit_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@unit_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Unit item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update invCard_Unit SET ");
                str.Append("[unit_Code]=@unit_Code");
                str.Append(",[unit_Name1]=@unit_Name1");
                str.Append(",[unit_Name2]=@unit_Name2");
                str.Append(",[unit_Disable]=@unit_Disable");
                str.Append(" WHERE unit_Key=@unit_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@unit_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@unit_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@unit_Name1", SqlDbType.NVarChar, 200).Value = item.Name1 ?? "";
                comm.Parameters.Add("@unit_Name2", SqlDbType.NVarChar, 200).Value = item.Name2 ?? "";
                comm.Parameters.Add("@unit_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [invCard_Unit] where [unit_Key]=@Key";
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
