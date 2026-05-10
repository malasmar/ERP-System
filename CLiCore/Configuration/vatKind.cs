using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Configuration
{
    public class vatKind
    {
        public Guid? Key { get; set; }
        public int Order { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public Boolean Calculate { get; set; }
        public Boolean Disable { get; set; }


        public vatKind GetItem(string DB, Guid? Key)
        {
            vatKind item = new vatKind();
            item.Order = MaxOrder(DB);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from com_vatKind where [vat_Key]=@Key ";
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
                    item.Order = Convert.ToInt32(reader["vat_Order"]);
                    item.Name1 = Convert.ToString(reader["vat_Name1"]);
                    item.Name2 = Convert.ToString(reader["vat_Name2"]);
                    item.Calculate = Convert.ToBoolean(reader["vat_Calculate"]);
                    item.Disable = Convert.ToBoolean(reader["vat_Disable"]);
                }
                reader.Close();
            }
            return item;
        }
        public List<vatKind> GetList(string DB)
        {
            List<vatKind> SHLL = new List<vatKind>();
            string selQuery = "select top 100 percent * from com_vatKind order by [vat_Order]";
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
                    vatKind item = new vatKind();
                    item.Key = iCore.IsDbNullRtNull(reader["vat_Key"]);
                    item.Order = Convert.ToInt32(reader["vat_Order"]);
                    item.Name1 = Convert.ToString(reader["vat_Name1"]);
                    item.Name2 = Convert.ToString(reader["vat_Name2"]);
                    item.Calculate = Convert.ToBoolean(reader["vat_Calculate"]);
                    item.Disable = Convert.ToBoolean(reader["vat_Disable"]);
                    SHLL.Add(item);
                }
                reader.Close();
            }
            return SHLL;
        }
        public static void Insert(string DB, vatKind item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO com_vatKind");
                str.Append("([vat_Order]");
                str.Append(",[vat_Name1]");
                str.Append(",[vat_Name2]");
                str.Append(",[vat_Calculate]");
                str.Append(",[vat_Disable])");
                str.Append(" VALUES ");
                str.Append("(@vat_Order");
                str.Append(",@vat_Name1");
                str.Append(",@vat_Name2");
                str.Append(",@vat_Calculate");
                str.Append(",@vat_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@vat_Order", SqlDbType.Int).Value = item.Order;
                comm.Parameters.Add("@vat_Name1", SqlDbType.NVarChar, 50).Value = item.Name1 ?? "";
                comm.Parameters.Add("@vat_Name2", SqlDbType.NVarChar, 50).Value = item.Name2 ?? "";
                comm.Parameters.Add("@vat_Calculate", SqlDbType.Bit).Value = item.Calculate;
                comm.Parameters.Add("@vat_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, vatKind item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update com_vatKind SET ");
                str.Append("[vat_Order]=@vat_Order");
                str.Append(",[vat_Name1]=@vat_Name1");
                str.Append(",[vat_Name2]=@vat_Name2");
                str.Append(",[vat_Calculate]=@vat_Calculate");
                str.Append(",[vat_Disable]=@vat_Disable");
                str.Append(" WHERE vat_Key=@vat_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@vat_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@vat_Order", SqlDbType.Int).Value = item.Order;
                comm.Parameters.Add("@vat_Name1", SqlDbType.NVarChar, 50).Value = item.Name1 ?? "";
                comm.Parameters.Add("@vat_Name2", SqlDbType.NVarChar, 50).Value = item.Name2 ?? "";
                comm.Parameters.Add("@vat_Calculate", SqlDbType.Bit).Value = item.Calculate;
                comm.Parameters.Add("@vat_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }
        public int MaxOrder(string DB)
        {
            int res;
            res = 0;
            string selQuery = "select top 100 percent isnull(max([vat_Order])+1,1) from [com_vatKind]";
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
                ScStr.Append("delete from [com_vatKind] where [vat_Key]=@Key ");
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
