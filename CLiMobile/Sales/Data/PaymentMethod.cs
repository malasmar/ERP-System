using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Sales.Data
{
    public class PaymentMethod
    {
        public Guid? Key { get; set; }
        public Guid? User { get; set; }
        public string Display { get; set; }
        public int AccountKind { get; set; }
        public Guid? Account { get; set; }
        public int Order { get; set; }
        public bool Disable { get; set; }
        public PaymentMethod GetItem(string DB, Guid? Key)
        {
            PaymentMethod item = new PaymentMethod();
          
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from AppUser_PaymentMethods where [pm_Key]=@Key ";
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
    }
}
