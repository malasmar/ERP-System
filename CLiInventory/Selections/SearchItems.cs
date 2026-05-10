using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiInventory.Selections
{
    public class SearchItems
    {
        public Guid? Key { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public Guid? vatKey { get; set; }
        public decimal Total { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }
        public string Display { get; set; }
        public List<SearchItems> GetList(string DB,string xLan, string Key)
        {
            List<SearchItems> items = new List<SearchItems>();
            string selQuery = "select top 100 percent * from dbo.fninvSelection_SearchItems(@Key) order by [Code] ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.NVarChar, 200).Value = Key ?? "";
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SearchItems item = new SearchItems();
                    item.Key = iCore.IsDbNullRtNull(reader["Key"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Cost = Convert.ToDecimal(reader["Cost"]);
                    item.Price = Convert.ToDecimal(reader["Price"]);
                    item.vatKey = iCore.IsDbNullRtNull(reader["vatKey"]);
                    item.Total = Convert.ToDecimal(reader["Total"]);
                    item.Barcode = Convert.ToString(reader["Barcode"]);
                    item.Unit = Convert.ToString(reader["Unit"]);
                    switch (xLan)
                    {
                        case "en":
                            item.Display = (item.Name2 == "" ? item.Name1 : item.Name2) + " (" + item.Code + ")";
                          
                            break;
                        case "ar":
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                        
                            break;
                        default:
                            item.Display = (item.Name1 == "" ? item.Name2 : item.Name1) + " (" + item.Code + ")";
                         
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
