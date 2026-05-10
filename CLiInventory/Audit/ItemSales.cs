using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Audit
{
    public class ItemSales
    {
        public decimal Quantity { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public List<ItemSales> GetList(string DB,Guid? Key,DateTime? Date,DateTime? StartDate)
        {
            List<ItemSales> items = new List<ItemSales>();
            string selQuery = "select top 100 percent * from dbo.fninvAuditCost_ItemSalesHistory(@Key,@Date,@StartDate) order by [InvoiceDate] desc";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key",SqlDbType.UniqueIdentifier).Value= Key;
                com.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
                com.Parameters.Add("@StartDate", SqlDbType.Date).Value = StartDate;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ItemSales item = new ItemSales();
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.InvoiceDate = iCore.IsDbNullRtNullDate(reader["InvoiceDate"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
