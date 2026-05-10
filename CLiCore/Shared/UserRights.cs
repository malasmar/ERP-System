using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiCore.Shared
{
    public class UserRights
    {
        public Guid? Key { get; set; }
        public Boolean CanDelete { get; set; }
        public Boolean CanPost { get; set; }
        public Boolean ConfirmFinance { get; set; }
        public Boolean ConfirmInventory { get; set; }
        public Boolean ConfirmPurchase { get; set; }
        public Boolean ConfirmSales { get; set; }
        public Boolean CloseSalesPrice { get; set; }
        public Boolean CloseSalesDiscount { get; set; }
        public UserRights GetItem(Guid? Key)
        {
            UserRights item = new UserRights();
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from dbo.fnWeb_UserRights(@Key)";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
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
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.CanDelete = Convert.ToBoolean(reader["CanDelete"]);
                    item.CanPost = Convert.ToBoolean(reader["CanPost"]);
                    item.ConfirmFinance = Convert.ToBoolean(reader["ConfirmFinance"]);
                    item.ConfirmInventory = Convert.ToBoolean(reader["ConfirmInventory"]);
                    item.ConfirmPurchase = Convert.ToBoolean(reader["ConfirmPurchase"]);
                    item.ConfirmSales = Convert.ToBoolean(reader["ConfirmSales"]);
                    item.CloseSalesPrice = Convert.ToBoolean(reader["CloseSalesPrice"]);
                    item.CloseSalesDiscount = Convert.ToBoolean(reader["CloseSalesDiscount"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
