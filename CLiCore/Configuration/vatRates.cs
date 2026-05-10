using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Configuration
{
    public class vatRates
    {
        public Guid? Key { get; set; }
        public int Order { get; set; }
        public decimal Rate { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Guid? Debit { get; set; }
        public Guid? Credit { get; set; }
        public Boolean Disable { get; set; }


        public vatRates GetItem(string DB, Guid? Key)
        {
            vatRates item = new vatRates();
            item.Order = MaxOrder(DB);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from com_vatRates where [vat_Key]=@Key ";
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

                    item.Key = iCore.IsDbNullRtNull(reader["vat_Key"]);
                    item.Order = (int)reader["vat_Order"];
                    item.Rate = (decimal)reader["vat_Rate"];
                    item.Name1 = (string)reader["vat_Name1"];
                    item.Name2 = (string)reader["vat_Name2"];
                    item.Debit = iCore.IsDbNullRtNull(reader["vat_Debit"]);
                    item.Credit = iCore.IsDbNullRtNull(reader["vat_Credit"]);
                    item.Disable = (Boolean)reader["vat_Disable"];
                }
                reader.Close();
            }
            return item;
        }
        public List<vatRates> GetList(string DB)
        {
            List<vatRates> SHLL = new List<vatRates>();
            string selQuery = "select top 100 percent * from com_vatRates order by [vat_Order]";
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
                    vatRates item = new vatRates();
                    item.Key = iCore.IsDbNullRtNull(reader["vat_Key"]);
                    item.Order = (int)reader["vat_Order"];
                    item.Rate = (decimal)reader["vat_Rate"];
                    item.Name1 = (string)reader["vat_Name1"];
                    item.Name2 = (string)reader["vat_Name2"];
                    item.Debit = iCore.IsDbNullRtNull(reader["vat_Debit"]);
                    item.Credit = iCore.IsDbNullRtNull(reader["vat_Credit"]);
                    item.Disable = (Boolean)reader["vat_Disable"];
                    SHLL.Add(item);
                }
                reader.Close();
            }
            return SHLL;
        }
        public static void Insert(string DB, vatRates SHLL)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("INSERT INTO com_vatRates");
                ScStr.Append("([vat_Order]");
                ScStr.Append(",[vat_Rate]");
                ScStr.Append(",[vat_Name1]");
                ScStr.Append(",[vat_Name2]");
                ScStr.Append(",[vat_Debit]");
                ScStr.Append(",[vat_Credit]");
                ScStr.Append(",[vat_Disable])");
                ScStr.Append(" VALUES ");
                ScStr.Append("(@vat_Order");
                ScStr.Append(",@vat_Rate");
                ScStr.Append(",@vat_Name1");
                ScStr.Append(",@vat_Name2");
                ScStr.Append(",@vat_Debit");
                ScStr.Append(",@vat_Credit");
                ScStr.Append(",@vat_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = ScStr.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@vat_Order", SqlDbType.Int).Value = SHLL.Order;
                comm.Parameters.Add("@vat_Rate", SqlDbType.Decimal).Value = SHLL.Rate;
                comm.Parameters.Add("@vat_Name1", SqlDbType.NVarChar, 50).Value = SHLL.Name1 ?? "";
                comm.Parameters.Add("@vat_Name2", SqlDbType.NVarChar, 50).Value = SHLL.Name2 ?? "";
                comm.Parameters.Add("@vat_Debit", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(SHLL.Debit);
                comm.Parameters.Add("@vat_Credit", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(SHLL.Credit);
                comm.Parameters.Add("@vat_Disable", SqlDbType.Bit).Value = SHLL.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, vatRates SHLL)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("UPDATE [com_vatRates] SET ");
                ScStr.Append("[vat_Order] =@vat_Order");
                ScStr.Append(",[vat_Rate] =@vat_Rate");
                ScStr.Append(",[vat_Name1] =@vat_Name1");
                ScStr.Append(",[vat_Name2] =@vat_Name2");
                ScStr.Append(",[vat_Debit] =@vat_Debit");
                ScStr.Append(",[vat_Credit] =@vat_Credit");
                ScStr.Append(",[vat_Disable] =@vat_Disable");
                ScStr.Append(" WHERE vat_Key=@vat_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = ScStr.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@vat_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(SHLL.Key);
                comm.Parameters.Add("@vat_Order", SqlDbType.Int).Value = SHLL.Order;
                comm.Parameters.Add("@vat_Rate", SqlDbType.Decimal).Value = SHLL.Rate;
                comm.Parameters.Add("@vat_Name1", SqlDbType.NVarChar, 50).Value = SHLL.Name1 ?? "";
                comm.Parameters.Add("@vat_Name2", SqlDbType.NVarChar, 50).Value = SHLL.Name2 ?? "";
                comm.Parameters.Add("@vat_Debit", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(SHLL.Debit);
                comm.Parameters.Add("@vat_Credit", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(SHLL.Credit);
                comm.Parameters.Add("@vat_Disable", SqlDbType.Bit).Value = SHLL.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([vat_Order])+1,1) from [com_vatRates]";
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
                ScStr.Append("delete from [com_vatRates] where [vat_Key]=@Key ");
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
