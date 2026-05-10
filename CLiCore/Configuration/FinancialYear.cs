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
    public class FinancialYear
    {
        public Guid? Key { get; set; }
        public int No { get; set; }
        public DateTime? From { get; set; }
        public DateTime? Last { get; set; }
        public Boolean Closed { get; set; }
        public Boolean Disable { get; set; }

        public List<FinancialYear> GetList(string DB)
        {
            List<FinancialYear> items = new List<FinancialYear>();
            string selQuery = "select top 100 percent * from com_FinancialYear";
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
                    FinancialYear item = new FinancialYear();
                    item.Key = iCore.IsDbNullRtNull(reader["fyr_Key"]);
                    item.No = Convert.ToInt32(reader["fyr_No"]);
                    item.From = iCore.IsDbNullRtNullDate(reader["fyr_From"]);
                    item.Last = iCore.IsDbNullRtNullDate(reader["fyr_Last"]);
                    item.Closed = Convert.ToBoolean(reader["fyr_Closed"]);
                    item.Disable = Convert.ToBoolean(reader["fyr_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public FinancialYear GetItem(string DB,Guid? Key)
        {
            FinancialYear item = new FinancialYear();
            item.No = MaxOrder(DB);
            item.From=new DateTime(item.No,1,1);
            item.Last = new DateTime(item.No, 12, 31);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from com_FinancialYear where [fyr_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["fyr_Key"]);
                    item.No = Convert.ToInt32(reader["fyr_No"]);
                    item.From = iCore.IsDbNullRtNullDate(reader["fyr_From"]);
                    item.Last = iCore.IsDbNullRtNullDate(reader["fyr_Last"]);
                    item.Closed = Convert.ToBoolean(reader["fyr_Closed"]);
                    item.Disable = Convert.ToBoolean(reader["fyr_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, FinancialYear item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO com_FinancialYear");
                str.Append("([fyr_No]");
                str.Append(",[fyr_From]");
                str.Append(",[fyr_Last]");
                str.Append(",[fyr_Closed]");
                str.Append(",[fyr_Disable])");
                str.Append(" VALUES ");
                str.Append("(@fyr_No");
                str.Append(",@fyr_From");
                str.Append(",@fyr_Last");
                str.Append(",@fyr_Closed");
                str.Append(",@fyr_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@fyr_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@fyr_From", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.From);
                comm.Parameters.Add("@fyr_Last", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.Last);
                comm.Parameters.Add("@fyr_Closed", SqlDbType.Bit).Value = item.Closed;
                comm.Parameters.Add("@fyr_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, FinancialYear item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update com_FinancialYear SET ");
                str.Append("[fyr_No]=@fyr_No");
                str.Append(",[fyr_From]=@fyr_From");
                str.Append(",[fyr_Last]=@fyr_Last");
                str.Append(",[fyr_Closed]=@fyr_Closed");
                str.Append(",[fyr_Disable]=@fyr_Disable");
                str.Append(" WHERE fyr_Key=@fyr_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@fyr_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@fyr_No", SqlDbType.Int).Value = item.No;
                comm.Parameters.Add("@fyr_From", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.From);
                comm.Parameters.Add("@fyr_Last", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(item.Last);
                comm.Parameters.Add("@fyr_Closed", SqlDbType.Bit).Value = item.Closed;
                comm.Parameters.Add("@fyr_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([fyr_No])+1,datepart(year,getdate())) from [com_FinancialYear]";
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
                ScStr.Append("delete from [com_FinancialYear] where [fyr_Key]=@Key ");
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
