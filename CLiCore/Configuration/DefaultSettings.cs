using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Configuration
{
    public class DefaultSettings
    {
        public int RecNo { get; set; }
        public string Currency { get; set; }
        public string SubCurrency { get; set; }
        public string CurrencyName1 { get; set; }
        public string CurrencyName2 { get; set; }
        public string CurrencySubName1 { get; set; }
        public string CurrencySubName2 { get; set; }
        public int UTCOffset { get; set; }
        public DefaultSettings GetItem(string DB)
        {
            DefaultSettings item = new DefaultSettings();
            string selQuery = "select top(1) * from com_DefaultSettings";
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
                    
                    item.RecNo = (int)reader["com_RecNo"];
                    item.Currency = (string)reader["com_Currency"];
                    item.SubCurrency = (string)reader["com_SubCurrency"];
                    item.CurrencyName1 = (string)reader["com_CurrencyName1"];
                    item.CurrencyName2 = (string)reader["com_CurrencyName2"];
                    item.CurrencySubName1 = (string)reader["com_CurrencySubName1"];
                    item.CurrencySubName2 = (string)reader["com_CurrencySubName2"];
                    item.UTCOffset = (int)reader["com_UTCOffset"];
                  
                }
                reader.Close();
            }
            return item;
        }
        public static void Insert(string DB, DefaultSettings item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("delete from com_DefaultSettings ");
                ScStr.Append("INSERT INTO com_DefaultSettings ");
                ScStr.Append("([com_Currency]");
                ScStr.Append(",[com_SubCurrency]");
                ScStr.Append(",[com_CurrencyName1]");
                ScStr.Append(",[com_CurrencyName2]");
                ScStr.Append(",[com_CurrencySubName1]");
                ScStr.Append(",[com_CurrencySubName2]");
                ScStr.Append(",[com_UTCOffset])");
                ScStr.Append(" VALUES ");
                ScStr.Append("(@com_Currency");
                ScStr.Append(",@com_SubCurrency");
                ScStr.Append(",@com_CurrencyName1");
                ScStr.Append(",@com_CurrencyName2");
                ScStr.Append(",@com_CurrencySubName1");
                ScStr.Append(",@com_CurrencySubName2");
                ScStr.Append(",@com_UTCOffset)");
                SqlCommand ScCom = new SqlCommand();
                ScCom = new SqlCommand();
                ScCom.Connection = con;
                ScCom.CommandType = CommandType.Text;
                ScCom.CommandText = ScStr.ToString();
                ScCom.Parameters.Clear();
                ScCom.Parameters.Add("@com_Currency", SqlDbType.NVarChar, 25).Value = item.Currency ?? "";
                ScCom.Parameters.Add("@com_SubCurrency", SqlDbType.NVarChar, 25).Value = item.SubCurrency ?? "";
                ScCom.Parameters.Add("@com_CurrencyName1", SqlDbType.NVarChar, 100).Value = item.CurrencyName1 ?? "";
                ScCom.Parameters.Add("@com_CurrencyName2", SqlDbType.NVarChar, 100).Value = item.CurrencyName2 ?? "";
                ScCom.Parameters.Add("@com_CurrencySubName1", SqlDbType.NVarChar, 100).Value = item.CurrencySubName1 ?? "";
                ScCom.Parameters.Add("@com_CurrencySubName2", SqlDbType.NVarChar, 100).Value = item.CurrencySubName2 ?? "";
                ScCom.Parameters.Add("@com_UTCOffset", SqlDbType.Int).Value = item.UTCOffset;
                con.Open();
                ScCom.ExecuteNonQuery();
            }
        }

    }
}
