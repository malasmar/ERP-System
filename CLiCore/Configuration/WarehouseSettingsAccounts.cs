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
    public class WarehouseSettingsAccounts
    {
     
        public Guid? wPurchase { get; set; }
        public Guid? wSales { get; set; }
        public Guid? wCost { get; set; }
        public Guid? wOnRoad { get; set; }
        public Guid? wProduction { get; set; }
        public Guid? wFinishGood { get; set; }
        public Boolean wProductionDetailed { get; set; }
        public Boolean wFinishGoodDetailed { get; set; }
 
        public WarehouseSettingsAccounts GetItem(string DB)
        {
            WarehouseSettingsAccounts item = new WarehouseSettingsAccounts();
            string selQuery = "select top(1) * from com_Settings";
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
                   
                    item.wPurchase = iCore.IsDbNullRtNull(reader["com_wPurchase"]);
                    item.wSales = iCore.IsDbNullRtNull(reader["com_wSales"]);
                    item.wCost = iCore.IsDbNullRtNull(reader["com_wCost"]);
                    item.wOnRoad = iCore.IsDbNullRtNull(reader["com_wOnRoad"]);
                    item.wProduction = iCore.IsDbNullRtNull(reader["com_wProduction"]);
                    item.wFinishGood = iCore.IsDbNullRtNull(reader["com_wFinishGood"]);
                    item.wProductionDetailed = Convert.ToBoolean(reader["com_wProductionDetailed"]);
                    item.wFinishGoodDetailed = Convert.ToBoolean(reader["com_wFinishGoodDetailed"]);
               
                }
                reader.Close();
            }
            return item;
        }
    }
}
