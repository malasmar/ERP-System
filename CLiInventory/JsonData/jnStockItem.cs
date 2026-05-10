using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.JsonData
{
    public class jnStockItem
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
      

        public jnStockItem GetItem(string DB,Guid? Key)
        {
            jnStockItem item = new jnStockItem();
            if (Key == null)
                return item;
            string selQuery = "select top 100 percent [item_Key],[item_Code],[item_Name1],[item_Name2] from invCard_StockItem where [item_Key]=@Key";
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
               
                    item.Key = iCore.IsDbNullRtNull(reader["item_Key"]);
                    item.Code = Convert.ToString(reader["item_Code"]);
                    item.Name1 = Convert.ToString(reader["item_Name1"]);
                    item.Name2 = Convert.ToString(reader["item_Name2"]);
             
                
                }
                reader.Close();
            }
            return item;
        }

    }
}
