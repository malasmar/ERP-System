using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiSales.Reports
{
    public class SalesAnalysis
    {
        public string Kind { get; set; }
        public string WarehouseName1 { get; set; }
        public string WarehouseName2 { get; set; }
        public string Code { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public string PersonCode { get; set; }
        public string PersonName1 { get; set; }
        public string PersonName2 { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string Warehouse { get; set; }
        public string Item { get; set; }
        public string Person { get; set; }

        public List<SalesAnalysis> GetList(string DB,string xLan)
        {
            List<SalesAnalysis> items = new List<SalesAnalysis>();
            string selQuery = "select top 100 percent * from dbo.ReportAnalysis_SalesFull() order by [Year],[Month]";
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
                    SalesAnalysis item = new SalesAnalysis();
                    item.Kind = Convert.ToString(reader["Kind"]);
                    item.WarehouseName1 = Convert.ToString(reader["WarehouseName1"]);
                    item.WarehouseName2 = Convert.ToString(reader["WarehouseName2"]);
                    item.Code = Convert.ToString(reader["Code"]);
                    item.Name1 = Convert.ToString(reader["Name1"]);
                    item.Name2 = Convert.ToString(reader["Name2"]);
                    item.Amount = Convert.ToDecimal(reader["Amount"]);
                    item.Quantity = Convert.ToDecimal(reader["Quantity"]);
                    item.PersonCode = Convert.ToString(reader["PersonCode"]);
                    item.PersonName1 = Convert.ToString(reader["PersonName1"]);
                    item.PersonName2 = Convert.ToString(reader["PersonName2"]);
                    item.Year = Convert.ToInt32(reader["Year"]);
                    item.Month = Convert.ToInt32(reader["Month"]);
                    item.Warehouse = xLan == "en" ? item.WarehouseName2 : item.WarehouseName1;
                    item.Item = xLan == "en" ? item.Name2 + " (" + item.Code + ")" : item.Name1 + " (" + item.Code + ")";
                    item.Person = xLan == "en" ? item.PersonName2 + " (" + item.PersonCode + ")" : item.PersonName1 + " (" + item.PersonCode + ")";
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }
    }
}
