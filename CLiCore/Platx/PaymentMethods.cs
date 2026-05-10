using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CLiCore.Platx
{
    public class PaymentMethods
    {
        public Guid? Key { get; set; }
        public Guid? User { get; set; }
        public string Display { get; set; }
        public int AccountKind { get; set; }
        public Guid? Account { get; set; }
        public int Order { get; set; }
        public bool Disable { get; set; }
        public List<PaymentMethods> GetList(string DB,Guid? Key)
        {
            List<PaymentMethods> items = new List<PaymentMethods>();
            if (Key == null)
                return items;
            string selQuery = "select top 100 percent * from AppSales_PaymentMethods where [pm_User]=@Key ";
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
                    PaymentMethods item = new PaymentMethods();
                    item.Key = iCore.IsDbNullRtNull(reader["pm_Key"]);
                    item.User = iCore.IsDbNullRtNull(reader["pm_User"]);
                    item.Display = Convert.ToString(reader["pm_Display"]);
                    item.AccountKind = Convert.ToInt32(reader["pm_AccountKind"]);
                    item.Account = iCore.IsDbNullRtNull(reader["pm_Account"]);
                    item.Order = Convert.ToInt32(reader["pm_Order"]);
                    item.Disable = Convert.ToBoolean(reader["pm_Disable"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public PaymentMethods GetItem(string DB,Guid? Key,Guid? User)
        {
            PaymentMethods item = new PaymentMethods();
            item.User = User;
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from AppSales_PaymentMethods where [pm_Key]=@Key ";
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
                    item.Key = iCore.IsDbNullRtNull(reader["pm_Key"]);
                    item.User = iCore.IsDbNullRtNull(reader["pm_User"]);
                    item.Display = Convert.ToString(reader["pm_Display"]);
                    item.AccountKind = Convert.ToInt32(reader["pm_AccountKind"]);
                    item.Account = iCore.IsDbNullRtNull(reader["pm_Account"]);
                    item.Order = Convert.ToInt32(reader["pm_Order"]);
                    item.Disable = Convert.ToBoolean(reader["pm_Disable"]);
                }
                reader.Close();
            }
            return item;
        }

        public static void Insert(string DB, PaymentMethods item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO AppSales_PaymentMethods ");
                str.Append("([pm_User]");
                str.Append(",[pm_Display]");
                str.Append(",[pm_AccountKind]");
                str.Append(",[pm_Account]");
                str.Append(",[pm_Order]");
                str.Append(",[pm_Disable])");
                str.Append(" VALUES ");
                str.Append("(@pm_User");
                str.Append(",@pm_Display");
                str.Append(",@pm_AccountKind");
                str.Append(",@pm_Account");
                str.Append(",@pm_Order");
                str.Append(",@pm_Disable)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@pm_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.User);
                comm.Parameters.Add("@pm_Display", SqlDbType.NVarChar, 200).Value = item.Display ?? "";
                comm.Parameters.Add("@pm_AccountKind", SqlDbType.Int).Value = item.AccountKind;
                comm.Parameters.Add("@pm_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                comm.Parameters.Add("@pm_Order", SqlDbType.Int).Value = item.Order;
                comm.Parameters.Add("@pm_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }

        public static void Update(string DB, PaymentMethods item)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update AppSales_PaymentMethods SET ");
                str.Append("[pm_User]=@pm_User");
                str.Append(",[pm_Display]=@pm_Display");
                str.Append(",[pm_AccountKind]=@pm_AccountKind");
                str.Append(",[pm_Account]=@pm_Account");
                str.Append(",[pm_Order]=@pm_Order");
                str.Append(",[pm_Disable]=@pm_Disable");
                str.Append(" WHERE pm_Key=@pm_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@pm_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                comm.Parameters.Add("@pm_User", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.User);
                comm.Parameters.Add("@pm_Display", SqlDbType.NVarChar, 200).Value = item.Display ?? "";
                comm.Parameters.Add("@pm_AccountKind", SqlDbType.Int).Value = item.AccountKind;
                comm.Parameters.Add("@pm_Account", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Account);
                comm.Parameters.Add("@pm_Order", SqlDbType.Int).Value = item.Order;
                comm.Parameters.Add("@pm_Disable", SqlDbType.Bit).Value = item.Disable;
                con.Open();
                comm.ExecuteNonQuery();
            }
        }


        public static int Delete(string DB, Guid? Key)
        {
            int res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = " Delete from [AppSales_PaymentMethods] where [pm_Key]=@Key";
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = delQuery;
                comm.Parameters.Clear();
                comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
               res= comm.ExecuteNonQuery();
            }
            return res;
        }

    }
}
