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
    public class BankNames 
    {
   
        public Guid? Key { get; set; }
        public string ID { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Disable { get; set; }
        public List<BankNames> GetList(string DB)
        {
            List<BankNames> items = new List<BankNames>();
            string selQuery = "select top 100 percent * from HRCard_BankNames order by [Bank_ID] ";
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
                    BankNames item = new BankNames();
                    item.Key = iCore.IsDbNullRtNull(reader["Bank_Key"]);
                    item.ID = Convert.ToString(reader["Bank_ID"]);
                    item.Name1 = Convert.ToString(reader["Bank_Name1"]);
                    item.Name2 = Convert.ToString(reader["Bank_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["Bank_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public BankNames GetItem(string DB,Guid? Key)
        {
            BankNames item = new BankNames();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent * from HRCard_BankNames where [Bank_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["Bank_Key"]);
                    item.ID = Convert.ToString(reader["Bank_ID"]);
                    item.Name1 = Convert.ToString(reader["Bank_Name1"]);
                    item.Name2 = Convert.ToString(reader["Bank_Name2"]);
                    item.Disable = Convert.ToBoolean(reader["Bank_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, BankNames item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO HRCard_BankNames");
                str.Append("([Bank_ID]");
                str.Append(",[Bank_Name1]");
                str.Append(",[Bank_Name2]");
                str.Append(",[Bank_Disable])");
                str.Append(" VALUES ");
                str.Append("(@Bank_ID");
                str.Append(",@Bank_Name1");
                str.Append(",@Bank_Name2");
                str.Append(",@Bank_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Bank_ID", SqlDbType.NVarChar, 10).Value = item.ID ?? "";
                comm.Parameters.Add("@Bank_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Bank_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Bank_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, BankNames item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update HRCard_BankNames SET ");
                str.Append("[Bank_ID]=@Bank_ID");
                str.Append(",[Bank_Name1]=@Bank_Name1");
                str.Append(",[Bank_Name2]=@Bank_Name2");
                str.Append(",[Bank_Disable]=@Bank_Disable");
                str.Append(" WHERE Bank_Key=@Bank_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@Bank_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@Bank_ID", SqlDbType.NVarChar, 10).Value = item.ID ?? "";
                comm.Parameters.Add("@Bank_Name1", SqlDbType.NVarChar, 150).Value = item.Name1 ?? "";
                comm.Parameters.Add("@Bank_Name2", SqlDbType.NVarChar, 150).Value = item.Name2 ?? "";
                comm.Parameters.Add("@Bank_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [HRCard_BankNames] where [Bank_Key]=@Key";
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
