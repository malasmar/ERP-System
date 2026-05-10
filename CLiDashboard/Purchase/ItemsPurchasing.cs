using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiDashboard.Purchase
{
    public class ItemsPurchasing
    {
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Purchase { get; set; }
        public decimal Return { get; set; }
        public decimal PurchaseQty { get; set; }
        public decimal ReturnQty { get; set; }
        public string Display { get; set; }
        public List<ItemsPurchasing> GetList(string DB, string xLan, int Year)
        {
            DateTime First = new DateTime(Year, 1, 1);
            DateTime Last = new DateTime(Year, 12, 31);

            List<ItemsPurchasing> items = new List<ItemsPurchasing>();
            string selQuery = "select top(10) * from dbo.fnDashPurchase_Items(@FirstDate,@LastDate) order by [Purchase] desc ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@FirstDate", SqlDbType.Date).Value = First;
                com.Parameters.Add("@LastDate", SqlDbType.Date).Value = Last;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ItemsPurchasing item = new ItemsPurchasing();
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Purchase = Convert.ToDecimal(reader["Purchase"]);
                    item.Return = Convert.ToDecimal(reader["Return"]);
                    item.PurchaseQty = Convert.ToDecimal(reader["PurchaseQty"]);
                    item.ReturnQty = Convert.ToDecimal(reader["ReturnQty"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = item.Name2 == "" ? item.Name1 : item.Name2;
                            break;
                        case "ar":
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                        default:
                            item.Display = item.Name1 == "" ? item.Name2 : item.Name1;
                            break;
                    }
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
