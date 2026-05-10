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
    public class ItemsCostPrices
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Unit { get; set; }
        public decimal UnitCost { get; set; }
        public decimal P1 { get; set; }
        public decimal P2 { get; set; }
        public decimal P3 { get; set; }
        public decimal P4 { get; set; }
        public decimal P5 { get; set; }
        public decimal P6 { get; set; }
        public decimal P7 { get; set; }
        public decimal P8 { get; set; }
        public decimal P9 { get; set; }
        public decimal P10 { get; set; }
        public List<ItemsCostPrices> GetList(string DB,int Year)
        {
            DateTime First = new DateTime(Year, 1, 1);
            DateTime Last = new DateTime(Year, 12, 31);
            List<ItemsCostPrices> items = new List<ItemsCostPrices>();
            string selQuery = "select top 100 percent * from dbo.fnInvAudit_ItemsCostPrices(@First,@Last) order by [Code]";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ItemsCostPrices item = new ItemsCostPrices();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    item.UnitCost = Convert.ToDecimal(reader["UnitCost"]);
                    item.P1 = Convert.ToDecimal(reader["P1"]);
                    item.P2 = Convert.ToDecimal(reader["P2"]);
                    item.P3 = Convert.ToDecimal(reader["P3"]);
                    item.P4 = Convert.ToDecimal(reader["P4"]);
                    item.P5 = Convert.ToDecimal(reader["P5"]);
                    item.P6 = Convert.ToDecimal(reader["P6"]);
                    item.P7 = Convert.ToDecimal(reader["P7"]);
                    item.P8 = Convert.ToDecimal(reader["P8"]);
                    item.P9 = Convert.ToDecimal(reader["P9"]);
                    item.P10 = Convert.ToDecimal(reader["P10"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
