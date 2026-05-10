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
    public class Products
    {
        
        public Guid? Key { get; set; }
        public int Kind { get; set; }
        public string Display { get; set; }
        public string Comment { get; set; }
        public decimal Price { get; set; }
        public Guid? vatKey { get; set; }
        public decimal vatRate { get; set; }
        public decimal Total { get; set; }
        public decimal Qty { get; set; }

        public List<Products> GetList(string DB)
        {
            List<Products> items = new List<Products>();
            string selQuery = "select top 100 percent * from dbo.fnAppSync_PushItems() order by [Kind] ";
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
                    Products item = new Products();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Kind = Convert.ToInt32(reader["Kind"]);
                    item.Display = Convert.ToString(reader["Display"]);
                    item.Comment = Convert.ToString(reader["Comment"]);
                    item.Price = Convert.ToDecimal(reader["Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.vatRate = Convert.ToDecimal(reader["vatRate"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
