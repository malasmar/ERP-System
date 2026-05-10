using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;
using CLiInventory.Cards.Stock;

namespace CLiInventory.Data
{
    public class WarehouseAccounts
    {
        public Guid? SalesAccount { get; set; }
        public Guid? CostAccount { get; set; }
        public Guid? PurchaseAccount { get; set; }
 

        public WarehouseAccounts GetItem(string DB, int Key)
        {
            WarehouseAccounts item = new WarehouseAccounts();
          
            string selQuery = "select top 100 percent [whs_accPurchase],[whs_accSales],[whs_accCost] from [InvCard_Warehouse] where [whs_No]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.Int).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.SalesAccount = iCore.IsDbNullRtNull(reader["whs_accSales"]);
                    item.CostAccount = iCore.IsDbNullRtNull(reader["whs_accCost"]);
                    item.PurchaseAccount = iCore.IsDbNullRtNull(reader["whs_accPurchase"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
