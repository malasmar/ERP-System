using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiInventory.Audit.Cost
{
    public class ItemsCost
    {
        public Guid? Key { get; set; }
        public Guid? Category { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal StandardCost { get; set; }
        public decimal Cost { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal Price { get; set; }
        public Guid? vatKey { get; set; }
        public decimal Total { get; set; }
        public string Unit { get; set; }
        public int Transactions { get; set; }

        public List<ItemsCost> GetList(string DB)
        {
            List<ItemsCost> items = new List<ItemsCost>();
            string selQuery = "select top 100 percent * from dbo.fnInvAudit_ItemsCost() order by [Category],[code]";
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
                    ItemsCost item = new ItemsCost();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Category = iCore.IsDbNullRtNull(reader["Category"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.StandardCost = Convert.ToDecimal(reader["StandardCost"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    item.UpdateDate = iCore.IsDbNullRtNullDate(reader["UpdateDate"]);
                    item.Price = Convert.ToDecimal(reader["Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.Transactions = Convert.ToInt32(reader["Transactions"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
