using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Selections.Stock
{
    public class ItemUnits
    {
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Rate { get; set; }
        public int index { get; set; }
        public decimal Price { get; set; }
        public  List<ItemUnits> GetList(string DB,Guid? Key)
        {
            List<ItemUnits> items = new List<ItemUnits>();
            if (Key == null)
                return items;

            string selQuery = "select top 100 percent * from dbo.fninvSelection_StockUnits(@Key) order by [index] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value=Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ItemUnits item = new ItemUnits();
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Rate = Convert.ToDecimal(reader["Rate"]);
                    item.index = Convert.ToInt32(reader["index"]);
                    item.Price = Convert.ToDecimal(reader["Price"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
