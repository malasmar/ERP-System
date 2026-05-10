using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Sales.SyncPush
{
    public class PaymentMethods
    {
        public Guid? Key { get; set; }
        public string Display { get; set; }
        public int AccountKind { get; set; }
        public Guid? Account { get; set; }
        public int Order { get; set; }
        public List<PaymentMethods> GetList(string DB, Guid? Key)
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
                    item.Display = Convert.ToString(reader["pm_Display"]);
                    item.AccountKind = Convert.ToInt32(reader["pm_AccountKind"]);
                    item.Account = iCore.IsDbNullRtNull(reader["pm_Account"]);
                    item.Order = Convert.ToInt32(reader["pm_Order"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
