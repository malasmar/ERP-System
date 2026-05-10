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
    public class Currency
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string SubCode { get; set; }
        public string SubName1 { get; set; }
        public string SubName2 { get; set; }
        public decimal Rate { get; set; }
        public int RateKind { get; set; }
        public Boolean Disable { get; set; }
        public List<Currency> GetList(string DB)
        {
            List<Currency> items = new List<Currency>();
            string selQuery = "select top 100 percent * from com_Currency";
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
                    Currency item = new Currency();
                    item.Key = iCore.IsDbNullRtNull(reader["cry_Key"]);
                    item.Code = Convert.ToString(reader["cry_Code"]);
                    item.Name1 = Convert.ToString(reader["cry_Name1"]);
                    item.Name2 = Convert.ToString(reader["cry_Name2"]);
                    item.SubCode = Convert.ToString(reader["cry_SubCode"]);
                    item.SubName1 = Convert.ToString(reader["cry_SubName1"]);
                    item.SubName2 = Convert.ToString(reader["cry_SubName2"]);
                    item.Rate = Convert.ToDecimal(reader["cry_Rate"]);
                    item.RateKind = Convert.ToInt32(reader["cry_RateKind"]);
                    item.Disable = Convert.ToBoolean(reader["cry_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public Currency GetItem(string DB, Guid? Key)
        {
            Currency item = new Currency();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from com_Currency where [cry_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["cry_Key"]);
                    item.Code = Convert.ToString(reader["cry_Code"]);
                    item.Name1 = Convert.ToString(reader["cry_Name1"]);
                    item.Name2 = Convert.ToString(reader["cry_Name2"]);
                    item.SubCode = Convert.ToString(reader["cry_SubCode"]);
                    item.SubName1 = Convert.ToString(reader["cry_SubName1"]);
                    item.SubName2 = Convert.ToString(reader["cry_SubName2"]);
                    item.Rate = Convert.ToDecimal(reader["cry_Rate"]);
                    item.RateKind = Convert.ToInt32(reader["cry_RateKind"]);
                    item.Disable = Convert.ToBoolean(reader["cry_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, Currency item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO com_Currency");
                str.Append("([cry_Code]");
                str.Append(",[cry_Name1]");
                str.Append(",[cry_Name2]");
                str.Append(",[cry_SubCode]");
                str.Append(",[cry_SubName1]");
                str.Append(",[cry_SubName2]");
                str.Append(",[cry_Rate]");
                str.Append(",[cry_RateKind]");
                str.Append(",[cry_Disable])");
                str.Append(" VALUES ");
                str.Append("(@cry_Code");
                str.Append(",@cry_Name1");
                str.Append(",@cry_Name2");
                str.Append(",@cry_SubCode");
                str.Append(",@cry_SubName1");
                str.Append(",@cry_SubName2");
                str.Append(",@cry_Rate");
                str.Append(",@cry_RateKind");
                str.Append(",@cry_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cry_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@cry_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cry_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cry_SubCode", SqlDbType.NVarChar, 25).Value = item.SubCode ?? "";
                comm.Parameters.Add("@cry_SubName1", SqlDbType.NVarChar, 100).Value = item.SubName1 ?? "";
                comm.Parameters.Add("@cry_SubName2", SqlDbType.NVarChar, 100).Value = item.SubName2 ?? "";
                comm.Parameters.Add("@cry_Rate", SqlDbType.Decimal).Value = item.Rate;
                comm.Parameters.Add("@cry_RateKind", SqlDbType.Int).Value = item.RateKind;
                comm.Parameters.Add("@cry_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, Currency item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update com_Currency SET ");
                str.Append("[cry_Code]=@cry_Code");
                str.Append(",[cry_Name1]=@cry_Name1");
                str.Append(",[cry_Name2]=@cry_Name2");
                str.Append(",[cry_SubCode]=@cry_SubCode");
                str.Append(",[cry_SubName1]=@cry_SubName1");
                str.Append(",[cry_SubName2]=@cry_SubName2");
                str.Append(",[cry_Rate]=@cry_Rate");
                str.Append(",[cry_RateKind]=@cry_RateKind");
                str.Append(",[cry_Disable]=@cry_Disable");
                str.Append(" WHERE cry_Key=@cry_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@cry_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@cry_Code", SqlDbType.NVarChar, 25).Value = item.Code ?? "";
                comm.Parameters.Add("@cry_Name1", SqlDbType.NVarChar, 100).Value = item.Name1 ?? "";
                comm.Parameters.Add("@cry_Name2", SqlDbType.NVarChar, 100).Value = item.Name2 ?? "";
                comm.Parameters.Add("@cry_SubCode", SqlDbType.NVarChar, 25).Value = item.SubCode ?? "";
                comm.Parameters.Add("@cry_SubName1", SqlDbType.NVarChar, 100).Value = item.SubName1 ?? "";
                comm.Parameters.Add("@cry_SubName2", SqlDbType.NVarChar, 100).Value = item.SubName2 ?? "";
                comm.Parameters.Add("@cry_Rate", SqlDbType.Decimal).Value = item.Rate;
                comm.Parameters.Add("@cry_RateKind", SqlDbType.Int).Value = item.RateKind;
                comm.Parameters.Add("@cry_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [com_Currency] where [cry_Key]=@Key";
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
